using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using QTP.Plugin;
using QTP.Infra;
using QTP.DBAccess;
using GMSDK;

namespace QTP.Domain
{
    public class MyStrategy : Strategy
    {
        #region static members
        /// <summary>
        /// Static 
        /// </summary>
        public static Dictionary<string, Instrument> DictInstruments;

        #endregion

        #region Members

        private TLogin gmLogin;
        private TStrategy strategyT;

        private Dictionary<string, Monitor> monitors;
        private RiskM riskM;
        private MDMode mdmode = MDMode.MD_MODE_NULL;

        // WebTrade and HeartPusle
        private WebTrade webTD;
        private System.Timers.Timer heartTimer;         // 1s 

        // Strategy's connect status;
        private int countMDConnect;
        private int countTDConnect;
        private TInstrument focusInstrument;

        // StopWatch
        private Stopwatch watch = new Stopwatch();
        #endregion

        #region Properties

        // strategyT internal fields
        public string Name
        {
            get { return strategyT.Name; }
        }

        public string GMID
        {
            get { return strategyT.GMID; }
        }

        public string RunType
        {
            get { return strategyT.RunType; }
        }

        public string PoolName
        {
            get { return strategyT.Pool.Name; }
        }

        // used for classes in strategy
        public TInstrument FocusInstrument
        {
            get { return focusInstrument; }
            set { focusInstrument = value; }
        }

        public MDMode MDMode 
        {
            get { return mdmode; }
        }

        public System.Timers.Timer HeartTimer
        {
            get { return heartTimer; }
        }
        #endregion

        #region delegates and events

        // exception when prepare
        public delegate void InitializeExceptionCallback(string message);
        public event InitializeExceptionCallback InitializeExceptionOccur; 


        // service connection
        public delegate void ConnectStatusChangedCallback(bool md, string status);
        public event ConnectStatusChangedCallback ConnectStatusChanged;

        public delegate void LogCallback(string msg);
        public event LogCallback MDLog;
        public event LogCallback TDLog;

        // FocusInstrument's realtime data
        public delegate void FocusTickArrivedCallback(TickTA tickTA);
        public event FocusTickArrivedCallback FocusTickArrived;

        public delegate void FocusBarArrivedCallback(Bar bar, Bar tickBar1M);
        public event FocusBarArrivedCallback FocusBarArrived;


        public delegate void KBTradeEventHandler(string sec_id, double price, double volume);



        public event KBTradeEventHandler OnKBOpenLong;
        public event KBTradeEventHandler OnKBCloseLong;

        #endregion

        #region Public methods

        // Create
        public MyStrategy(TStrategy s, TLogin login)
        {
            strategyT = s;
            this.gmLogin = login;

            // monitors
            monitors = new Dictionary<string, Monitor>();
            foreach (TInstrument ins in strategyT.Instruments)
            {
                Monitor monitor = (Monitor)Activator.CreateInstance(strategyT.MonitorType);

                // get Instrumnt
                monitor.SetTInstrument(this, ins);
                monitors.Add(ins.Symbol, monitor);
            }

            // RiskM
            riskM = (RiskM)Activator.CreateInstance(s.RiskMType);
            riskM.SetStrategy(this);

            // heartTimer
            heartTimer = new System.Timers.Timer();
            heartTimer.Interval = 1000;     // 1s
        }

        // initialize
        public void Initialize()
        {
            // InitWebTrade
            if (strategyT.TradeChannelName != "掘金" && RunType != "实盘")
                InitWebTrade();

            // start HeartTimer, becuse WebTrade will hook it.
            heartTimer.Start();

            // InitGM and prepare preStart data
            InitGM();          
            Prepare();
        }

        // Start strategy
        public void Start()
        {
            // async run strategy
            Task.Run<int>(new Func<int>(base.Run));
        }

        public void MyStrategyStop()
        {
            heartTimer.Stop();
            base.Stop();
        }

        /// <summary>
        /// Enumerable of Monitor
        /// </summary>
        public IEnumerable<Monitor> GetMonitorEnumerator()
        {
            foreach (KeyValuePair<string, Monitor> pair in monitors)
            {
                yield return pair.Value;
            }
        }

        public Monitor GetMonitor(string symbol)
        {
            if (monitors.ContainsKey(symbol))
                return monitors[symbol];
            return null;
        }
        private Monitor GetMonitor(string exchange, string sec_id)
        {
            string symbol = string.Format("{0}.{1}", exchange, sec_id);
            return GetMonitor(symbol);
        }

        public double GetVolumn(string exchange, string sec_id)
        {
            return riskM.GetVolume(exchange, sec_id);
        }
        #endregion


        #region MD events and Error override

        public override void OnTick(Tick tick)
        {
            string symbol = string.Format("{0}.{1}", tick.exchange, tick.sec_id);
            Monitor monitor = GetMonitor(symbol);
            if (monitor != null)
            {
                monitor.OnTick(tick);
            }

            // Process focusInstument
            if (focusInstrument != null && symbol == focusInstrument.Symbol && FocusTickArrived != null)
            {
                FocusTickArrived(monitor.TickTA);
            }
        }

        public override void OnBar(Bar bar)
        {
            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            Monitor monitor = GetMonitor(symbol);
            if (monitor != null)
            {
                monitor.OnBar(bar);
            }

            // Process focusInstument
            if (focusInstrument != null && symbol == focusInstrument.Symbol && FocusBarArrived != null)
            {
                FocusBarArrived(bar, monitor.TickTA.Bar1M);
            }

        }


        public override void OnMDLogin()
        {
            // used for connectstatus changed
            countMDConnect++;
            if (ConnectStatusChanged != null) ConnectStatusChanged(true, string.Format("数据已连接({0})", countMDConnect));

            // async run: 当天数据的重建
            Task.Run(new Action(PrepareMDLogin));
        }

        public override void OnTDLogin()
        {
            // used for connectstatus changed
            countTDConnect++;
            if (ConnectStatusChanged != null) ConnectStatusChanged(false, string.Format("交易已连接({0})", countTDConnect));

            // Trader 资管 
            System.Threading.Thread.Sleep(100);
            watch.Reset();
            watch.Start();
            riskM.Initialize();
            watch.Stop();
            WriteTDLog(string.Format("资管初始化完成, 用时({0})", watch.ElapsedMilliseconds));
        }

        public override void OnMdEvent(MDEvent md_event)
        {
            WriteMDLog(string.Format("重要行情事件({0})", md_event.event_type == 1 ? "开市":"收市"));
        }

        public override void OnError(int error_code, string error_msg)
        {
            if (error_code == 2000)     // TD
            {
                if (ConnectStatusChanged != null) ConnectStatusChanged(false, string.Format("交易已断({0})", countTDConnect));

                WriteMDLog(error_msg);
            }
            // 数据服务器断了标志
            if (error_code == 3000)
            {
                if (ConnectStatusChanged != null) ConnectStatusChanged(true, string.Format("数据已断({0})", countMDConnect));
                WriteMDLog(error_msg);
            }                  
        }

        #endregion

        #region Trader events overide

        public void MyOpenLongSync(string exchange, string sec_id, double price, double volume)
        {
            if (strategyT.TradeChannelName == "掘金" || mdmode != MDMode.MD_MODE_LIVE)
            {
                // trade
                OpenLongSync(exchange, sec_id, price, volume);
            }
            else
            {
                // KBTrade
                OnKBOpenLong(sec_id, price, volume);

            }
        }

        public void MyCloseLongSync(string exchange, string sec_id, double price, double volume)
        {
            if (strategyT.TradeChannelName == "掘金" || mdmode != MDMode.MD_MODE_LIVE)
            {
                // trade
                CloseLongSync(exchange, sec_id, price, volume);
            }
            else
            {
                // KBTrade
                OnKBCloseLong(sec_id, price, volume);

            }

        }

        #endregion

        #region public and private utils

        public void WriteTDLog(string msg)
        {
            DateTime dt = DateTime.Now;
            if (TDLog != null)
                TDLog(string.Format("{0} {1}", dt.ToLongTimeString(), msg));
        }

        public void WriteMDLog(string msg)
        {
            DateTime dt = DateTime.Now;
            if (MDLog != null)
                MDLog(string.Format("{0} {1}", dt.ToLongTimeString(), msg));
        }

        private void InitWebTrade()
        {
            webTD = WebTrade.CreatWebTD(strategyT.TradeChannelName);
            if (webTD != null)
            {
                watch.Reset();
                watch.Start();

                webTD.Init(strategyT.TradeChannelParameters);
                HeartTimer.Elapsed += new System.Timers.ElapsedEventHandler(webTD.HeartTimer_Elapsed);
                webTD.ConnectStatusChanged += new WebTrade.ConnectStatusChangedCallback(WebTrade_ConnectStatusChanged);

                watch.Stop();
                WriteTDLog(string.Format("外挂交易服务初始化用时({0}),结果({1})", watch.ElapsedMilliseconds, webTD.IsLoginOK));
            }
        }

        private void WebTrade_ConnectStatusChanged(bool status)
        {
            WriteTDLog(string.Format("外挂交易服务连接状态改变为{0},", status));
        }
        private void PrepareMDLogin()
        {
            try
            {
                foreach (Monitor m in GetMonitorEnumerator())
                {
                    watch.Reset();
                    watch.Start();
                    m.PrepareMDLogin();
                    watch.Stop();
                    WriteMDLog(string.Format("{0}处理({1}Bs和{2}Ts,用时({3})", m.Target.InstrumentId, m.GetCounBarsPrepared(), m.TickTA.Count, watch.ElapsedMilliseconds));
                }
            }
            catch
            {
                WriteMDLog("PrepareMDLogin出错!");
            }
        }

        private void InitGM()
        {
            // MDMode
            if (RunType == "实盘")
            {
                mdmode = MDMode.MD_MODE_LIVE;
            }
            else if (RunType == "虚拟")
            {
                if (Utils.IsInStockMarkerOpenPeriod(DateTime.Now)) mdmode = MDMode.MD_MODE_LIVE;
                else mdmode = MDMode.MD_MODE_LIVE;  //MDMode.MD_MODE_SIMULATED;
            }

            // Subscrible Instruments
            StringBuilder sb = new StringBuilder();
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (mdmode == MDMode.MD_MODE_LIVE || mdmode == MDMode.MD_MODE_SIMULATED)
                    sb.Append(m.Target.Symbol + ".tick,");
                //base.Subscribe(m.Target.Symbol + ".tick");

                sb.Append(m.Target.Symbol + ".bar.60," + m.Target.Symbol + ".bar.900,");
                //base.Subscribe(m.Target.Symbol + ".bar.60");            // 1M
                //base.Subscribe(m.Target.Symbol + ".bar.900");           // 15M
            }

            int ret = base.Init(gmLogin.UserName, gmLogin.Password, strategyT.GMID, sb.ToString(), mdmode, "localhost:8001");
            if (ret != 0)
            {
                InitializeExceptionOccur(string.Format("初始化掘金错误{0}", ret));
            }
        }

        private void Prepare()
        {
            // Prepare Get DictInstruments
            if (DictInstruments == null)
            {
                DictInstruments = new Dictionary<string, Instrument>();

                List<Instrument> lst = new List<Instrument>();
                lst = base.GetInstruments("SZSE", 1, 0);
                lst.AddRange(base.GetInstruments("SHSE", 1, 0));

                foreach (Instrument ins in lst)
                {
                    DictInstruments.Add(ins.symbol, ins);
                }
            }

            // prepare monitors'data 
            watch.Reset();
            watch.Start();
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (DictInstruments.ContainsKey(m.Target.Symbol))
                    m.GMInstrument = DictInstruments[m.Target.Symbol];
                m.Prepare();
            }
            watch.Stop();
            WriteMDLog(string.Format("启动前准备完成, 用时({0})", watch.ElapsedMilliseconds));
        }
        #endregion
    }
}

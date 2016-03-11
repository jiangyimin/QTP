using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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


        // Strategy's connect status;
        private int countConnect;
        private bool connectSucceed;

        private bool flagMDOnline;

        private TInstrument focusInstrument;

        // StopWatch
        private Stopwatch watch = new Stopwatch();
        #endregion

        #region Properties

        // Name: strategyT internal field 
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

        public TInstrument FocusInstrument
        {
            get { return focusInstrument; }
            set { focusInstrument = value; }
        }

        public MDMode MDMode 
        {
            get { return mdmode; }
        }

        #endregion

        #region delegates and events

        public delegate void ConnectStatusChangedCallback(bool connectSucceed, int num);
        public event ConnectStatusChangedCallback ConnectStatusChanged;

        public delegate void LogCallback(string msg);
        public event LogCallback MDLog;
        public event LogCallback TDLog;

        public delegate void KBTradeEventHandler(string sec_id, double price, double volume);



        public event KBTradeEventHandler OnKBOpenLong;
        public event KBTradeEventHandler OnKBCloseLong;

        // FocusInstrument's realtime data
        public delegate void FocusTickArrivedCallback(TickTA tickTA);
        public FocusTickArrivedCallback FocusTickArrived;

        public delegate void FocusBarArrivedCallback(Bar bar, Bar tickBar1M);
        public FocusBarArrivedCallback FocusBarArrived;

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

            riskM = (RiskM)Activator.CreateInstance(s.RiskMType);
            riskM.SetStrategy(this);

        }

        // init GM and prepare data
        public void Prepare()
        {
            // Init GM
            // set mdmode
            if (RunType == "实盘") 
            {
                mdmode = MDMode.MD_MODE_LIVE;
            }
            else if (RunType == "模拟")
            {
                if (Utils.IsInStockMarkerOpenPeriod(DateTime.Now)) mdmode = MDMode.MD_MODE_LIVE;
                else mdmode = MDMode.MD_MODE_SIMULATED;
            }

            int ret = base.Init(gmLogin.UserName, gmLogin.Password, strategyT.GMID, "", mdmode, "localhost:8001");
            if (ret != 0)
            {
                throw new Exception(string.Format("初始化掘金错误{0}", ret));
            }

            // Once Get DictInstruments
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
            watch.Start();
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (DictInstruments.ContainsKey(m.Target.Symbol))
                    m.GMInstrument = DictInstruments[m.Target.Symbol];
                m.Prepare();
            }
            watch.Stop();
            WriteMDLog(string.Format("监控器数据准备完成，用时({0})", watch.ElapsedMilliseconds));

            // Trader 资管 
            //riskM.Initialize();
        }

        // Start
        public void Start()
        {
            // Subscrible Instruments
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (mdmode == MDMode.MD_MODE_LIVE || mdmode == MDMode.MD_MODE_SIMULATED)
                    base.Subscribe(m.Target.Symbol + ".tick");

                base.Subscribe(m.Target.Symbol + ".bar.60");            // 1M
                base.Subscribe(m.Target.Symbol + ".bar.900");           // 15M
            }

            // async run strategy
            Task.Run<int>(new Func<int>(base.Run));
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

        #region utils

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


        public override void OnLogin()
        {
            // used for connectstatus changed
            connectSucceed = true;
            countConnect++;
            if (ConnectStatusChanged != null)
                ConnectStatusChanged(connectSucceed, countConnect);

            // 数据服务连上（包括重连后）的处理（当天数据的重建等）

            if (flagMDOnline == false)
            {
                // PrepareBarsToday
                watch.Start();
                foreach (Monitor m in GetMonitorEnumerator())
                    m.PrepareBarsToday();
                watch.Stop();
                WriteMDLog(string.Format("准备当天Bars完成，用时({0})", watch.ElapsedMilliseconds));

                // Prepare Ticks
                watch.Start();
                foreach (Monitor m in GetMonitorEnumerator())
                    m.PrepareTicksToday();
                watch.Stop();
                WriteMDLog(string.Format("准备当天Ticks完成，用时({0})", watch.ElapsedMilliseconds));

                flagMDOnline = true;
            }


        }

        public override void OnMdEvent(MDEvent md_event)
        {
            WriteMDLog(string.Format("重要行情事件({0})", md_event.event_type == 1 ? "开市":"收市"));
        }

        public override void OnError(int error_code, string error_msg)
        {            
            if (error_code == 2000 || error_code == 3000)       // connect fail
            {
                connectSucceed = false;
                if (ConnectStatusChanged != null) ConnectStatusChanged(connectSucceed, countConnect);

                // 数据服务器断了标志
                if (error_code == 3000)
                    flagMDOnline = false;
                    
            }

            WriteMDLog(error_msg);
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

    }
}

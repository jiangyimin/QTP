using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using QTP.Plugin;
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
        private MDMode mdmode = MDMode.MD_MODE_NULL;            // 不订阅

        // monitors
        private Dictionary<string, Monitor> monitors;
        private string symbols;

        // PoolManager and RiskM
        private RiskM riskM;
        private PoolManager pm;

        // WebTrade and HeartPusle
        private WebTrade webTD;
        private System.Timers.Timer heartTimer;         // 1s 
        private const int IntervalTDCashHeart = 15;     // 15s 
        private int countTDCashHeartPusle = IntervalTDCashHeart;

        // Strategy's status;
        private bool isRuning;
        private int countMDConnect;
        private int countTDConnect;
        private TInstrument focusInstrument;

        // StopWatch
        private Stopwatch watch = new Stopwatch();

        // BackTest
        private MyBackTest backTest;

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
        public string Symbols
        {
            get { return symbols; }
        }

        public MDMode MDMode 
        {
            get { return mdmode; }
        }

        public WebTrade WebTD
        {
            get { return webTD; }
        }

        public PoolManager PM
        {
            get { return pm; }
        }

        public MyBackTest BackTest
        {
            get { return backTest; }
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
        public void FireFocusTickArrived(TickTA tickTA)
        {
            if (FocusTickArrived != null) FocusTickArrived(tickTA);
        }

        public delegate void FocusBarArrivedCallback(Bar bar);
        public event FocusBarArrivedCallback FocusBarArrived;
        public void FireFocusBarArrived(Bar bar)
        {
            if (FocusBarArrived != null) FocusBarArrived(bar);
        }

        // webTD heart
        public delegate void TradeCashHeartPusleCallback(Cash cash, long elapsed, bool isWeb=true);
        public event TradeCashHeartPusleCallback TradeCashHeartPusle;

        #endregion

        #region Public methods

        // Create
        public MyStrategy(TStrategy s, TLogin login)
        {
            strategyT = s;
            this.gmLogin = login;

            // monitors
            TAInfo.CreateStaticMembers(strategyT.TAInfoParameters);
            
            monitors = new Dictionary<string, Monitor>();
            foreach (TInstrument ins in strategyT.Instruments)
            {
                Monitor monitor = new Monitor(this, ins, new TAInfo(strategyT.DLLName, strategyT.TAInfoParameters));

                symbols += ins.Symbol + ",";
                monitors.Add(ins.Symbol, monitor);
            }

            // PoolManager and RiskM

            pm = (PoolManager)Activator.CreateInstance(strategyT.Pool.ManagerType, this, strategyT.Pool.ManagerParameters);
            riskM = new RiskM(this, strategyT.RiskMInfoParameters);

            // heartTimer
            heartTimer = new System.Timers.Timer();
            heartTimer.Elapsed += new System.Timers.ElapsedEventHandler(heartTimer_Elapsed);
            heartTimer.Interval = 1000;     // 1s
        }

        // initialize
        public void Initialize()
        {
            // InitGM 
            InitGM();

            // GetInsrtruments and set to monitor
            GetAllInstruments();
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (DictInstruments.ContainsKey(m.Target.Symbol))
                    m.GMInstrument = DictInstruments[m.Target.Symbol];
                else
                    WriteMDLog(string.Format("监控品种{0}的代码不在清单中!", m.Target.Symbol));
            }

            // prepare monitors's need data before Run
            if (RunType == "回测")
            {
                DateTime fromTime = Convert.ToDateTime(strategyT.BackTestInfoParameters["Start"]);
                PrepareDaily(fromTime.Subtract(new TimeSpan(1, 0, 0)));      // 不取当天的日线 (GM用开始时点标注) 
                PrepareMDLogin(fromTime); // 同步调用 when backtest

                backTest = new MyBackTest(this, strategyT.BackTestInfoParameters, TAInfo.Periods);
            }
            else
            {
                PrepareDaily(DateTime.Now);     // 真实状态是还没有当天的日线生成
            }

            // InitWebTrade
            if (strategyT.TradeChannelName != "掘金" && RunType == "实盘")
                InitWebTrade();
        }

        // Start strategy
        public void Start()
        {
            // async run strategy
            Task.Run<int>(new Func<int>(base.Run));
            isRuning = true;

            // heartTimer
            heartTimer.Start();
        }

        public void MyStrategyStop()
        {
            if (webTD != null) webTD.LogoutWeb();

            heartTimer.Stop();

            if (isRuning)
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

        public void WriteTDLog(string msg)
        {
            DateTime dt = DateTime.Now;
            if (TDLog != null)
                TDLog(string.Format("{0} {1}", dt.ToLongTimeString(), msg));
        }

        public void WriteMDLog(string msg, bool pause=false)
        {
            if (MDLog != null)
            {
                if (pause)
                {
                    MDLog(msg);
                    MDLog("按空格继续...");
                }
                else
                    MDLog(string.Format("{0} {1}", DateTime.Now.ToLongTimeString(), msg));
            }
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
        }

        public override void OnBar(Bar bar)
        {
            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            Monitor monitor = GetMonitor(symbol);
            if (monitor != null)
            {
                monitor.OnBar(bar);
            }
        }

        public override void OnMDLogin()
        {
            // used for connectstatus changed
            countMDConnect++;
            if (ConnectStatusChanged != null) ConnectStatusChanged(true, string.Format("数据已连接({0})", countMDConnect));

            // 实盘LIVE时需要先清理并不检测信号  
            foreach (Monitor m in GetMonitorEnumerator())
            {
                m.TA.Clear();
                m.TA.Enabled = false;
            }

            // async run: 当天数据的重建
            Task task = new Task(PrepareMDLogin, DateTime.Now);
            task.Start();
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

        #region TD Methods
        public double GetVolumn(string exchange, string sec_id)
        {
            return riskM.GetVolume(exchange, sec_id);
        }

        public override void OnTDLogin()
        {
            // used for connectstatus changed
            countTDConnect++;
            if (ConnectStatusChanged != null) ConnectStatusChanged(false, string.Format("交易已连接({0})", countTDConnect));

            // Trader 资管 
            System.Threading.Thread.Sleep(50);      // need wait after login?
            watch.Reset();
            watch.Start();
            riskM.Initialize();
            watch.Stop();
            WriteTDLog(string.Format("资管初始化完成【仓位数({0}), 用时({1})】", riskM.CountRiskPositions, watch.ElapsedMilliseconds));
        }

        public RiskOrder MyOpenLong(string exchange, string sec_id, double price, double volume)
        {
            Order ord = OpenLong(exchange, sec_id, price, volume);

            RiskOrder order = new RiskOrder(ord);

            if (webTD != null && webTD.IsLoginOK)
            {
                webTD.BuyWeb(exchange, sec_id, price, volume);
            }

            return order;
        }

        public void MyCloseLong(string exchange, string sec_id, double price, double volume)
        {
            Order order = CloseLong(exchange, sec_id, price, volume);

            if (webTD != null && webTD.IsLoginOK)
            {
                webTD.SellWeb(exchange, sec_id, price, volume);
            }

        }

        public override void OnOrderNew(Order order)
        {
            WriteTDLog(string.Format("OrderNew ({0}, {1})", order.cl_ord_id, order.ord_rej_reason));
        }

        public override void OnOrderRejected(Order order)
        {
            WriteTDLog(string.Format("OrderRejected ({0}, {1})", order.cl_ord_id, order.ord_rej_reason_detail));
        }
        #endregion

        #region data utils

        private void PrepareDaily(DateTime from)
        {
            watch.Reset(); watch.Start();

            int count = 0;
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                m.PrepareDaily(from);               // monitor 自己读DailyBar
                count += m.TA.GetCountKLine(0);     // 日K线
            }

            watch.Stop();
            WriteMDLog(string.Format("预读并压入({0})个日线数据.总用时({1})", count, watch.ElapsedMilliseconds));
        }


        private void PrepareMDLogin(object from)
        {
            DateTime fromTime = (DateTime)from;

            // Get bars of each period
            watch.Reset(); watch.Start();

            foreach (int ktype in TAInfo.Periods)
                PushBarsToMonitor(ktype, fromTime); 

            watch.Stop();
            WriteMDLog(string.Format("预读各周期Bar数据完成.用时({0})", watch.ElapsedMilliseconds));

            // process buffered and enable
            watch.Reset(); watch.Start();
            int count = 0;
            int countPush = 0;
            foreach (Monitor m in GetMonitorEnumerator())
            {
                count += m.PrepareTick(fromTime);           // monitor 自己读Tick
                countPush += m.TA.GetCountTick();
                m.StartTA(fromTime);
            }
            watch.Stop();
            WriteMDLog(string.Format("预处理完成! 共{0}/{1}Ts。用时({2})", count, countPush, watch.ElapsedMilliseconds));
        }

        private void PushBarsToMonitor(int ktype, DateTime fromTime)
        {
            string start = Utils.GetStartTimeString(ktype, TAInfo.PreNBars, fromTime);
            var bars = GetBars(Symbols, 60 * ktype, start, Utils.DTLongString(fromTime));

            int count = 0;
            foreach (Bar bar in bars)
            {
                string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
                Monitor m = GetMonitor(symbol);
                m.PushBar(bar, 0F);      // adj_factor为0F时，采用预取的日线对应的adj_factor.

                count += m.TA.GetCountKLine(ktype);
            }

            WriteMDLog(string.Format("压入预读的 {0}M 分时K线({1})个", ktype, count));
        }

        private void InitGM()
        {
            // MDMode
            if (RunType == "实盘" || RunType == "虚拟")
            {
                mdmode = MDMode.MD_MODE_LIVE;  //MDMode.MD_MODE_SIMULATED;
            }

            // Subscrible Instruments
            StringBuilder sb = new StringBuilder();
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (mdmode == MDMode.MD_MODE_LIVE || mdmode == MDMode.MD_MODE_SIMULATED)
                    sb.Append(m.Target.Symbol + ".tick,");

                foreach (int period in TAInfo.Periods)
                    sb.Append(string.Format("{0}.bar.{1},", m.Target.Symbol, period * 60));         // 按s
            }

            int ret = base.Init(gmLogin.UserName, gmLogin.Password, strategyT.GMID, sb.ToString(), mdmode, "localhost:8001");
            if (ret != 0)
            {
                InitializeExceptionOccur(string.Format("初始化掘金错误{0}", ret));
            }
        }

        private void GetAllInstruments()
        {
            watch.Reset();
            watch.Start();
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
            watch.Stop();
            WriteMDLog(string.Format("得到市场所有品种。用时({0})", watch.ElapsedMilliseconds));
        }

        #endregion

        #region other private

        private void heartTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // TDCashHeart
            countTDCashHeartPusle++;
            if (countTDCashHeartPusle >= IntervalTDCashHeart)
            {
                // for GM Trade
                watch.Reset();
                watch.Start();
                Cash cash = this.GetCash();
                riskM.SetLastCash(cash);
                watch.Stop();

                if (cash != null && TradeCashHeartPusle != null)
                {
                    TradeCashHeartPusle(cash, watch.ElapsedMilliseconds, false);
                }

                // for Web Trade, let web alive.
                if (webTD != null)
                {
                    watch.Reset();
                    watch.Start();
                    cash = webTD.GetCashWeb();
                    riskM.SetLastCash(cash);        // webTD is prior for GMTD'cash.
                    watch.Stop();

                    if (cash != null && TradeCashHeartPusle != null)
                        TradeCashHeartPusle(cash, watch.ElapsedMilliseconds, true);

                    // relogin async
                    if (cash == null) Task.Run(new Action(webTD.LoginWeb));
                }
                
                // reset count
                countTDCashHeartPusle = 0;
            }

        }

        private void InitWebTrade()
        {
            webTD = WebTrade.CreatWebTD(strategyT.TradeChannelName, strategyT.TradeChannelParameters);
            if (webTD != null)
            {
                watch.Reset();
                watch.Start();

                webTD.OnException += new WebTrade.ExceptionCallback(WebTrade_OnException);
                webTD.ConnectStatusChanged += new WebTrade.ConnectStatusChangedCallback(WebTrade_ConnectStatusChanged);
                webTD.LoginWeb();

                watch.Stop();
                WriteTDLog(string.Format("外挂交易服务初始化用时({0})", watch.ElapsedMilliseconds));
            }
        }

        private void WebTrade_OnException(string message)
        {
            WriteTDLog(message);
        }
        private void WebTrade_ConnectStatusChanged(bool status)
        {
            WriteTDLog(string.Format("外挂交易服务连接状态改变为{0},", status));
        }
        #endregion

        #region Process Monitor's public Parameters

        // static quota names
        private static Type quotaType;
        private Dictionary<string, List<string>> quotaNames;

        // static for quotas
        public Type QuotaType
        {
            get { return quotaType; }
        }

        public Dictionary<string, List<string>> QuotaNames
        {
            get { return quotaNames; }
        }

        #endregion


    }
}

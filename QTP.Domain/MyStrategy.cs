using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
//using System.Data.SqlClient;

using QTP.Infra;
using QTP.DBAccess;
using GMSDK;
using System.Threading;

namespace QTP.Domain
{
    public class MyStrategy : Strategy
    {
        #region Members

        /// <summary>
        /// Static 
        /// </summary>
        public static Dictionary<string, Instrument> DictInstruments;

        private TLogin gmLogin;
        private TStrategy strategyT;

        private Dictionary<string, Monitor> monitors;
        private RiskM riskM;
        private MDMode mdmode = MDMode.MD_MODE_SIMULATED;


        // Strategy's connect status;
        private int countConnected;
        private bool connectSucceed;

        private TInstrument focusInstrument;

        // pusle
        private System.Timers.Timer timer;
        private int countPusle;
        private int messageInterval = 30;           // 30s

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



        #endregion

        #region delegates and events

        public delegate void ConnectStatusChangedCallback(bool connectSucceed, int num);
        public event ConnectStatusChangedCallback ConnectStatusChanged;

        public delegate void MessageHintCallback(string msg);
        public delegate void KBTradeEventHandler(string sec_id, double price, double volume);


        public event MessageHintCallback MessageHint;

        public event KBTradeEventHandler OnKBOpenLong;
        public event KBTradeEventHandler OnKBCloseLong;

        // FocusInstrument
        public delegate void FocusTickHandlerDelegate(Tick tick);
        public FocusTickHandlerDelegate FocusTickHandler;

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

            timer = new System.Timers.Timer(1000);          // set 1s pusle timer 
            timer.Elapsed += PusleTimerHandler;
        }

        // init GM and prepare data
        public void Prepare()
        {
            // Init GM
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
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                if (DictInstruments.ContainsKey(m.Target.Symbol))
                    m.GMInstrument = DictInstruments[m.Target.Symbol];
                m.Prepare();
            }

            // Trader 资管 
            //riskM.Initialize();
        }

        // Start
        public void Start()
        {
            // Subscrible Instruments
            foreach (Monitor m in this.GetMonitorEnumerator())
            {
                base.Subscribe(m.Target.Symbol + ".tick");
                base.Subscribe(m.Target.Symbol + ".bar.60");
            }

            // pusle timer of strategy
            timer.Start();

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

        public void WriteDebug(string msg)
        {
            //log.WriteDebug(msg);
        }
        public void WriteError(string msg)
        {
            //Console.WriteLine("[{0}.{1}] {2}", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond, msg);
            //log.WriteError(msg);
        }

        public void WriteInfo(string msg)
        {
            if (MessageHint != null)
                MessageHint(msg);
            //log.WriteInfo(msg);
        }


        private void PusleTimerHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            // message Pusle
            countPusle++;
            if (countPusle % messageInterval == 0)
            {
                string msg = null;
                foreach (KeyValuePair<string, Monitor> pair in monitors)
                {
                    Monitor m = pair.Value;
                    msg += m.PulseHintMessage();
                }

                WriteInfo(msg);
            }

            // process OnPusle per monitor
            foreach (KeyValuePair<string, Monitor> pair in monitors)
            {
                Monitor m = pair.Value;
                m.OnPulse();
            }
        }

        #endregion

        #region MD events and Error override

        public override void OnTick(Tick tick)
        {
            string symbol = string.Format("{0}.{1}", tick.exchange, tick.sec_id);
            //if (symbol == focusInstrument.Symbol)
            //{
            //    FocusTickHandler(tick);
            //}

            Monitor monitor = GetMonitor(symbol);
            if (monitor != null)
            {
                monitor.OnTick(tick);
            }
        }

        public override void OnLogin()
        {
            // used for connectstatus changed
            connectSucceed = true;
            countConnected++;
            if (ConnectStatusChanged != null)
                ConnectStatusChanged(true, countConnected);
        }

        public override void OnBar(Bar bar)
        {
            Monitor monitor = GetMonitor(string.Format("{0}.{1}", bar.exchange, bar.sec_id));
            if (monitor != null)
            {
                monitor.OnBar(bar);
            }
        }
        public override void OnMdEvent(MDEvent md_event)
        {
            WriteInfo(string.Format("重要行情事件({0})", md_event.event_type == 1 ? "开市":"收市"));
        }

        public override void OnError(int error_code, string error_msg)
        {            
            if (error_code == 2000 || error_code == 3000)       // connect fail
            {
                connectSucceed = false;
                if (ConnectStatusChanged != null) ConnectStatusChanged(false, countConnected);
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

        /// <summary>
        /// 委托执行回报，订单的任何执行回报都会触发本事件，通过rpt可访问回报信息。
        /// </summary>
        /// <param name="rpt"></param>
        public override void OnExecRpt(ExecRpt rpt)
        {
            //Console.WriteLine(
            //    "rpt: cl_ord_id={0} price={1} amount={2} exec_type={3}",
            //    rpt.cl_ord_id,
            //    rpt.price,
            //    rpt.amount,
            //    rpt.exec_type);
        }

        /// <summary>
        /// 订单被拒绝时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderRejected(Order order)
        {
            WriteError(string.Format("{0} {1} 订单被拒绝[原因:{2}]", order.exchange, order.sec_id, order.ord_rej_reason));
        }

        /// <summary>
        /// 当订单已被交易所接受时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderNew(Order order)
        {
            WriteDebug(string.Format("{0}.{1} 新订单({2}]", order.exchange, order.sec_id, order.cl_ord_id));
        }

        /// <summary>
        /// 订单全部成交时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderFilled(Order order)
        {
            WriteInfo(string.Format("{0}.{1} 订单完成", order.exchange, order.sec_id));

            Monitor monitor = GetMonitor(order.exchange, order.sec_id);
            if (monitor != null)
                monitor.OnOrderFilled();
        }

        /// <summary>
        /// 订单部分成交时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderPartiallyFilled(Order order)
        {
            WriteInfo(string.Format("{0}.{1} 订单部分完成", order.exchange, order.sec_id));
        }

        /// <summary>
        /// 订单被停止执行时，触发本事件, 比如限价单到收市仍未成交，作订单过期处理。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderStopExecuted(Order order)
        {
            Console.WriteLine("order stop executed: {0}", order.cl_ord_id);
        }

        /// <summary>
        /// 撤单成功时，触发本事件。order参数包含最新的order状态。
        /// </summary>
        /// <param name="order"></param>
        public override void OnOrderCancelled(Order order)
        {
            Console.WriteLine("order cancelled: {0}", order.cl_ord_id);
        }

        /// <summary>
        /// 撤单请求被拒绝时，触发本事件
        /// </summary>
        /// <param name="rpt"></param>
        public override void OnOrderCancelRejected(ExecRpt rpt)
        {
            Console.WriteLine("order cancel failed: {0}", rpt.cl_ord_id);
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.DBAccess;
using QTP.TAlib;
using GMSDK;

namespace QTP.Domain
{
    public class Monitor
    {
        #region Properties
        /// <summary>
        /// GM's Instrument and TInstrument(Target)
        /// </summary>
        public Instrument GMInstrument { get; set; }

        public TInstrument Target
        {
            get { return target; }
        }
        public string TargetTitle
        {
            get {
                if (GMInstrument == null)
                    return "非法代码";
                if (GMInstrument.is_active == 1)
                    return GMInstrument.sec_name;
                else
                    return GMInstrument.sec_name + "(停牌)";
            }
        }

        // Is Focus, output Bar Arrived
        public bool Focus { get; set; }

        public bool IsBench { get; set; }
        /// <summary>
        /// 分类：0：观察期  1：候选期   2：持仓期
        /// </summary>
        public int Category
        {
            get
            {
                if (riskPosition != null)
                    return 2;
                else
                    return 0;                
            }
        }

        public TA TA
        {
            get { return ta; }
        }

        #endregion

        #region protected members

        // import
        protected MyStrategy strategy;          // parent
        protected TInstrument target;
        protected TA ta;


        // MDs: Buffered
        protected List<Bar> barBuffer;
        protected List<Tick> tickBuffer;


        // TDs : Position and Order
        protected RiskPosition riskPosition;
        protected double stopLossPrice;

        protected Order orderLast;

        #endregion

        #region private member

        // base factor and Dictionary for PrepareMDLogin Bars ( Method PushBar 's factor = 0)
        private float baseFactor;
        private Dictionary<string, float> dictFactorPrepare;

        #endregion

        #region Public Methods

        public Monitor(MyStrategy strategy, TInstrument target, TAInfo taInfo, string dllName)
        {
            this.strategy = strategy;
            this.target = target;

            this.ta = new TA(this, taInfo, dllName);

            this.barBuffer = new List<Bar>();
            this.tickBuffer = new List<Tick>();
        }

        public void SetFocus()
        {
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {
                m.Focus = false;
            }
            this.Focus = true;
        }

        // Prepare DailyBar for Normalize price
        public void PrepareDaily(DateTime from)
        {
            // Get LastNDailyBars, [0] is latest!
            var bars = strategy.GetLastNDailyBars(Target.Symbol, TAInfo.PreNBars, Utils.DTString(from));
            if (bars.Count == 0) throw new Exception("没有用来Normalize的日线数据!");
            // set factors
            baseFactor = bars[0].adj_factor;
            dictFactorPrepare = new Dictionary<string, float>();

            for (int i = bars.Count - 1;  i >=0; i--)
            {
                DailyBar bar = bars[i];

                // Set dictFactor
                string key = Utils.DTString(Utils.UtcToDateTime(bar.utc_time));
                if (!IsBench)
                    dictFactorPrepare[key] = bar.adj_factor;

                // Push KLine
                PushDailyBar(bar);
            }

        }

        public int PrepareTick(DateTime from)
        {

            // tickTA
            List<Tick> ticks = strategy.GetLastNTicks(Target.Symbol, TAInfo.PreNTicks, Utils.DTLongString(from));
            for (int i = ticks.Count - 1; i >= 0; i--)
            {
                Tick tick = ticks[i];
                // if tick is not Today, ignore
                if (Utils.IsToday(tick.utc_time))
                    ta.PushTick(tick);
            }

            return ta.TickTA.Count;
        }

        public void PushBar(Bar bar, float factor)
        {
            if (!IsBench)
                Adj_factor(bar, factor);

            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            KLine k = new KLine(symbol, bar.utc_time, bar.open, bar.close, bar.high, bar.low, bar.volume);
            ta.PushKLine(bar.bar_type / 60, k);

            // Process focusInstument
            if (Focus) strategy.FireFocusBarArrived(bar);
        }

        public KLine GetKLine(DailyBar bar)
        {
            if (!IsBench)
                Adj_factor(bar);

            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            return new KLine(symbol, bar.utc_time, bar.open, bar.close, bar.high, bar.low, bar.volume);
        }

        public KLine GetKLine(Bar bar, float factor)
        {
            if (!IsBench)
                Adj_factor(bar, factor);

            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            return new KLine(symbol, bar.utc_time, bar.open, bar.close, bar.high, bar.low, bar.volume);
        }

        public void PushKLine(int ktype, KLine k)
        {
            ta.PushKLine(ktype, k);
        }

        public void PushDailyBar(DailyBar bar)
        {
            if (!IsBench)
                Adj_factor(bar);

            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            ta.PushKLine(0, new KLine(symbol, bar.utc_time, bar.open, bar.close, bar.high, bar.low, bar.volume));
        }

        public void StartTA()
        {
             lock (this)
             {
                 // push buffered bars
                 foreach (Bar bar in barBuffer)
                     PushBar(bar, baseFactor);
                 barBuffer.Clear();

                 // push buffeded ticks
                 foreach (Tick tick in tickBuffer)
                     ta.PushTick(tick);
                 tickBuffer.Clear();

                 ta.Enabled = true;
             }

        }

        #endregion

        #region some private utils

        private void Adj_factor(DailyBar bar)
        {
            bar.open = bar.open * bar.adj_factor / baseFactor;
            bar.close = bar.close * bar.adj_factor / baseFactor;
            bar.high = bar.high * bar.adj_factor / baseFactor;
            bar.low = bar.low * bar.adj_factor / baseFactor;
        }

        private void Adj_factor(Bar bar, float factor)
        {
            if (factor != 0F)
            {
                bar.open = bar.open * factor / baseFactor;
                bar.close = bar.close * factor / baseFactor;
                bar.high = bar.high * factor / baseFactor;
                bar.low = bar.low * factor / baseFactor;
            }
            else    // use dictFactorPrepare
            {
                string key = Utils.DTString(Utils.UtcToDateTime(bar.utc_time));
                if (dictFactorPrepare.ContainsKey(key))
                    Adj_factor(bar, dictFactorPrepare[key]);
            }
        }

        #endregion

        #region MD abstract or virtual

        // On Events
        public void OnTick(Tick tick)
        {
            lock (this)
            {
                if (ta.Enabled)
                {
                    ta.PushTick(tick);
                    if (Focus) strategy.FireFocusTickArrived(ta.TickTA);
                }
                else
                {
                    tickBuffer.Add(tick);
                }
            }
        }

        public void OnBar(Bar bar)
        {
            lock (this)
            {
                if (ta.Enabled)
                    PushBar(bar, baseFactor);
                else
                    barBuffer.Add(bar);
            }
        }

        #endregion

        #region TD methods


        public RiskOrder IssueOpenLongSignal(string fname, int ktype, KLine thisK)
        {
            double stoplessRisk = ta.GetATR(60);

            strategy.WriteTDLog(string.Format("{0}({1} {2})", thisK.Symbol, Utils.DTLongString(Utils.UtcToDateTime(thisK.UTC)), thisK.CLOSE));
            return null;
        }
        //public abstract RiskOrder IssueCloseLongSignal();

        public virtual void OnOrderFilled()
        {
            orderLast = null;
        }


        public virtual void SetPosition(RiskPosition pos)
        {
            riskPosition = pos;
        }

        public virtual void SetStopLossPrice()
        {
            //if (riskPosition.side == 1)
            //    stopLossPrice = riskPosition.price * 0.95;
            //else
            //    stopLossPrice = riskPosition.price * 1.05;
        }

        #endregion
    }
}

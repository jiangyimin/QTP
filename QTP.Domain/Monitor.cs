using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.DBAccess;
using GMSDK;

namespace QTP.Domain
{
    public abstract class Monitor
    {

        #region public properties

        /// <summary>
        /// TInstrument(Target) and GM's Instrument
        /// </summary>
        public Instrument GMInstrument { get; set; }
        public TInstrument Target { get; set; }

        /// <summary>
        /// 分类：0：观察期  1：候选期   3：持仓期
        /// </summary>
        public int Category { get; set; }

        public TickTA TickTA { get; set; }

        #endregion

        #region protected members

        // static quota and scalar Names
        protected static List<string> quotaNames;
        protected static Dictionary<string, List<string>> scalarNames;

        // strategy
        protected MyStrategy strategy;

        #endregion

        #region Public Methods

        // Static Quota Names
        public static List<string> GetQuotaNames()
        {
            return quotaNames;
        }

        public static List<string> GetScalarNames(string name)
        {
            return scalarNames[name];
        }


        public void SetTInstrument(MyStrategy strategy, TInstrument target)
        {
            this.strategy = strategy;
            this.Target = target;

            this.TickTA = new TickTA();
        }

        public void SetFocus()
        {
            this.strategy.FocusInstrument = Target;
        }

        #endregion
        
        #region abstract or virtual 

        // Data Prepare
        public abstract void Prepare();
        public abstract void PrepareBarsToday();
        public abstract void PrepareTicksToday();

        public abstract double GetQuotaScalarValue(string name);

        // On Events
        public virtual void OnTick(Tick tick)
        {
            if (strategy.MDMode == MDMode.MD_MODE_LIVE)
                TickTA.Push(tick, true);
            else
                TickTA.Push(tick, false);

        }
        public abstract void OnBar(Bar bar);

        #endregion

        #region Monitor Position and Order

        // Position and Order
        protected Order orderLast;
        protected Position posTrace;
        protected double stopLossPrice;


        public virtual void OnOrderFilled()
        {
            orderLast = null;
        }

        public abstract void OnPosition(Position pos);

        public virtual void SetStopLossPrice()
        {
            if (posTrace.side == 1)
                stopLossPrice = posTrace.price * 0.95;
            else
                stopLossPrice = posTrace.price * 1.05;
        }

        #endregion
    }
}

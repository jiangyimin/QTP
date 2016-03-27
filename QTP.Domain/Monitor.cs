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
    public abstract class Monitor
    {

        #region public properties

        // static for quotas
        public static Type QuotaType
        {
            get { return quotaType; }
        }

        public static Dictionary<string, List<string>> QuotaNames
        {
            get { return quotaNames; }
        }

        /// <summary>
        /// GM's Instrument and TInstrument(Target)
        /// </summary>
        public Instrument GMInstrument { get; set; }
        public TInstrument Target { get; set; }

        /// <summary>
        /// 分类：0：观察期  1：候选期   3：持仓期
        /// </summary>
        public int Category { get; set; }

        // tick process class
        public TickTA TickTA { get; set; }


        #endregion

        #region protected members

        // static quota names
        protected static Type quotaType;
        protected static Dictionary<string, List<string>> quotaNames;


        // strategy
        protected MyStrategy strategy;

        // Buffered
        protected bool needBuffer;
        protected List<Bar> barBuffer;
        protected List<Tick> tickBuffer;

        #endregion

        #region Public Methods

        public void SetTInstrument(MyStrategy strategy, TInstrument target)
        {
            this.strategy = strategy;
            this.Target = target;
            this.TickTA = new TickTA();

            this.needBuffer = true;
            this.barBuffer = new List<Bar>();
            this.tickBuffer = new List<Tick>();
        }

        public void SetFocus()
        {
            this.strategy.FocusInstrument = Target;
        }

        #endregion
        
        #region abstract or virtual 

        // Data Prepare
        public abstract void Prepare();
        public abstract void PrepareMDLogin();
        public abstract int GetCounBarsPrepared();

        // Quotas
        public abstract object GetLatestQuota(int ktype);
        public abstract RList<object> GetQuotas(int ktype);

        public abstract RList<KLine> GetKLines(int ktype);

        // On Events
        public virtual void OnTick(Tick tick)
        {
            if (needBuffer)
            {
                tickBuffer.Add(tick);
                return;
            }

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

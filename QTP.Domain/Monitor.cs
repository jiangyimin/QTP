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


        public Tick LatestTick { get; set; }

        public int NumTicks { get { return ticksBuffer.Count; } }
        public int Num1mBars { get { return barsBuffer.Count; } }

        #endregion

        #region protected members

        protected MyStrategy strategy;

        protected List<Bar> barsBuffer;
        protected List<Tick> ticksBuffer;

        #endregion

        #region Methods
        public void SetTInstrument(MyStrategy strategy, TInstrument target)
        {
            this.strategy = strategy;
            this.Target = target;
            //this.instrument = StrategyQTP.DictInstruments[target.Symbol];

            barsBuffer = new List<Bar>();
            ticksBuffer = new List<Tick>();
        }

        public void SetFocus()
        {
            this.strategy.FocusInstrument = Target;
        }

        #endregion
        
        #region Event Process
        public abstract string PulseHintMessage();
        public abstract void OnPulse();

        public abstract void Prepare();
        public abstract void InitializeBufferData();
        public virtual void OnTick(Tick tick)
        {
            LatestTick = tick;
            ticksBuffer.Add(tick);
        }
        public virtual void OnBar(Bar bar)
        {
            barsBuffer.Add(bar);
        }

        public virtual void Close() { }

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

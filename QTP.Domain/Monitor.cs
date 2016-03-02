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
        #region MD
        protected Instrument instrument { get; set; }
        protected TInstrument target { get; set; }
        protected MyStrategy strategy;

        protected bool prepared = false;
        protected List<Bar> barsBuffer;
        protected List<Tick> ticksBuffer;

        public void SetTInstrument(MyStrategy strategy, TInstrument target)
        {
            this.strategy = strategy;
            this.target = target;
            //this.instrument = StrategyQTP.DictInstruments[target.Symbol];

            barsBuffer = new List<Bar>();
            ticksBuffer = new List<Tick>();
        }


        public abstract string PulseHintMessage();
        public abstract void OnPulse();

        public abstract void Prepare();
        public abstract void InitializeBufferData();
        public abstract void OnTick(Tick tick);
        public abstract void OnBar(Bar bar);

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

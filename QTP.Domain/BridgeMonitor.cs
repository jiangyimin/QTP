using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.Infra;
using QTP.DBAccess;
using QTP.TAlib;
using GMSDK;

namespace QTP.Domain
{
    public class BridgeMonitor : Monitor
    {
        #region members

        /// <summary>
        /// Parameters
        /// </summary>
        private int NDailyBars = 200;
        private int NBars = 500;
        private int NTicks = 100;
        
        // Quota Name
        private class Quota
        {
            public double EMA20 { get; set; }
            public double EMA5 { get; set; }
        }

        // TAs
        protected MBarBridgeTA barsTA;      // for bars
        protected DailyBridgeTA dailyTA;    // for Daily

        
        private RList<Tick> xsTick;
        private RList<KLineBar> xs;
        private RList<Quota> ys;

        private double maxprice;

        #endregion

        #region Prepare Data Methods

        public BridgeMonitor()
        {
            // process static, only once
            if (quotaNames == null)
            {
                quotaNames = new List<string>();
                quotaNames.Add("均线");

                // 均线
                List<string> maScalars = new List<string>();
                maScalars.Add("EMA5"); maScalars.Add("EMA20");

                // dict
                scalarNames = new Dictionary<string, List<string>>();
                scalarNames.Add("均线", maScalars);
            }
        }
        public override void Prepare()
        {
            // Get Upperlevel Data
            List<DailyBar> barsDaily = strategy.GetLastNDailyBars(Target.Symbol, NDailyBars);
            dailyTA = new DailyBridgeTA();

            for (int i = barsDaily.Count - 1; i >= 0; i--)
            {
                DailyBar bar = barsDaily[i];
                if (bar.flag == 1)
                {
                    KLineDaily kx = new KLineDaily(bar);
                    dailyTA.Push(kx);
                }
                else
                {

                }
            }

            xsTick = new RList<Tick>();
            xs = new RList<KLineBar>();
            ys = new RList<Quota>();


        }

        public override void PrepareBarsToday()
        {
            lock (this)
            {
                List<Bar> bars = strategy.GetLastNBars(Target.Symbol, 60, NBars);

            }
        }

        public override void PrepareTicksToday()
        {
            lock (this)
            {
                TickTA.Clear();
                // 获取前NTicks的Ticks
                List<Tick> lst = strategy.GetLastNTicks(Target.Symbol, NTicks);
                for (int i = lst.Count - 1; i >= 0; i--)
                {
                    // if tick is not Today, ignore
                    if (Utils.IsToday(lst[i].utc_time))
                        TickTA.Push(lst[i], false);
                }
            }
        }

        #endregion

        #region RealTime Analyst

        private void Push(Tick tick)
        {
            xsTick.Add(tick);

            // set maxprice
            if (tick.last_price > maxprice) maxprice = tick.last_price;

            // set stopLosssPrice;
            if (posTrace != null && maxprice > posTrace.price * 1.05)
            {
                stopLossPrice = posTrace.price * 0.95;
            }
        }
        
        private void Push(Bar bar)
        {
            xs.Add(new KLineBar(bar));
            ys.Add(new Quota());

            // caculate quotas
            Formula.MA<KLineBar, Quota>(xs, "CLOSE", 5, ys, "EMA5");
            Formula.MA<KLineBar, Quota>(xs, "CLOSE", 20, ys, "EMA20");
        }

        private bool StopLossTrigger(int side)
        {
            // caculate average price of last 5 ticks
            double sum = 0.0;
            for (int i = 0; i < 5; i++)
                sum += xsTick[i].last_price;
            double a = sum / 5;

            if (side == 1 && a <= stopLossPrice)            // Long side
                return true;
            if (side == 2 && a >= stopLossPrice)            // short side
                return true;

            return false;
        }


        private bool OpenTrigger(int side)
        {
            if (side == 1)
                return CrossUp();

            if (side == 2)
                return CrossDown();

            return false;
        }

        private bool CrossUp()
        {
            //Quota q0 = ys[0];
            //Quota q1 = ys[1];

            //if (q1.EMA5 < q1.EMA20 && q0.EMA5 > q0.EMA20)
            //    return true;

            return false;
        }

        private bool CrossDown()
        {
            //Quota q0 = ys[0];
            //Quota q1 = ys[1];

            //if (q1.EMA5 > q1.EMA20 && q0.EMA5 < q0.EMA20)
            //    return true;

            return false;
        }


        #endregion

        #region override methods


        public override double GetQuotaScalarValue(string name)
        {
            Random r = new Random();
           
            return r.NextDouble();
        }

        // On events
        public override void OnPosition(Position pos)
        {
            posTrace = pos;
            SetStopLossPrice();
        }

        public override void OnTick(Tick tick)
        {
            // first call base
            base.OnTick(tick);

                //// 触发平单
                //if (posTrace != null)
                //{
                //    if (posTrace.side == 1 && StopLossTrigger(posTrace.side))     // Long
                //    {
                //        strategy.WriteTDLog(Target.Symbol + " 触发平多信号");
                //        strategy.MyCloseLongSync(Target.Exchange, Target.Symbol, xsTick[0].bid_p1, posTrace.volume);
                //    }

                //}

        }

        public override void OnBar(Bar bar)
        {
            Push(bar);


            // strategy.MyOpenLongSync(Target.Exchange, Target.Symbol, xsTick[0].ask_p1 * 0.92, 100);
 
            if (OpenTrigger(1))     // Long
            {
                // skip trigger
                if (posTrace != null || orderLast != null)
                {
                    strategy.WriteTDLog(Target.Symbol + " 触发开多信号但有仓位或有订单");
                    return;
                }

                double vol = strategy.GetVolumn(Target.Exchange, Target.Symbol);

                if (vol == 0.0)
                {
                    strategy.WriteTDLog(Target.Symbol + " 触发开多信号但未通过风控");
                }
                else
                {
                    //strategy.MyOpenLongSync(target.Exchange, target.InstrumentId, xsTick[0].ask_p1, vol);
                    strategy.WriteTDLog(string.Format("{0} 触发开多信号（订单{1})", Target.Symbol, 1));
                }
            }

        }
        #endregion

    }
}

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
        private int NBars = 800;
        private int NTicks = 100;

        // TAs
        private BridgeTA bridgeTA;      // for Daily

        public BridgeMonitor()
        {
            bridgeTA = new BridgeTA();

            // Init Monitor's Quota Names
            if (quotaType == null)
            {
                quotaType = BridgeTA.QuotaType;

                quotaNames = new Dictionary<string, List<string>>() 
                {
                    {"均线", new List<string>() { "EMA5", "EMA20" }},
                    {"真实波幅", new List<string>() { "MTR", "ATR" }}
                };
            }
        }
       
        #endregion


        #region override methods

        // Prepare Data
        public override void Prepare()
        {
            // DailyBars
            List<DailyBar> barsDaily = strategy.GetLastNDailyBars(Target.Symbol, NDailyBars);

            bridgeTA.Push(barsDaily);
        }

        public override void PrepareMDLogin()
        {
            lock (this)
            {
                // bridgeTA
                bridgeTA.ClearBars();       // clear minute bars first

                bridgeTA.Push(strategy.GetLastNBars(Target.Symbol, 60, NBars));
                bridgeTA.Push(strategy.GetLastNBars(Target.Symbol, 900, NBars));

                // push buffered bars
                foreach (Bar bar in barBuffer)
                {
                    bridgeTA.Push(bar);
                }
                barBuffer.Clear();

                // tickTA
                TickTA.Clear();     // clear first
                List<Tick> ticks = strategy.GetLastNTicks(Target.Symbol, NTicks);
                for (int i = ticks.Count - 1; i >= 0; i--)
                {
                    // if tick is not Today, ignore
                    if (Utils.IsToday(ticks[i].utc_time))
                        TickTA.Push(ticks[i]);
                }

                // push buffeded ticks
                foreach (Tick tick in tickBuffer)
                {
                    TickTA.Push(tick);
                }

                tickBuffer.Clear();

                needBuffer = false;
            }
        }

        public override int GetCounBarsPrepared()
        {
            return bridgeTA.CountBars;
        }

        // Quotas
        public override object GetLatestQuota(int ktype)
        {
            return bridgeTA.GetLatestQuota(ktype);
        }

        public override RList<object> GetQuotas(int ktype)
        {
            return bridgeTA.GetQuotas(ktype);
        }

        // KLines
        public override RList<KLine> GetKLines(int ktype)
        {
            return bridgeTA.GetKLines(ktype);
        }

        #endregion

        #region RealTime Analyst


        private bool StopLossTrigger(int side)
        {
            //// caculate average price of last 5 ticks
            //double sum = 0.0;
            //for (int i = 0; i < 5; i++)
            //    sum += xsTick[i].last_price;
            //double a = sum / 5;

            //if (side == 1 && a <= stopLossPrice)            // Long side
            //    return true;
            //if (side == 2 && a >= stopLossPrice)            // short side
            //    return true;

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

        #region override On Methods


        // On events
        public override void OnPosition(Position pos)
        {
            posTrace = pos;
            SetStopLossPrice();
        }

        public override void OnTick(Tick tick)
        {
            lock (this)
            {
                // first call base
                base.OnTick(tick);

            }
        }

        public override void OnBar(Bar bar)
        {
            if (needBuffer)
            {
                barBuffer.Add(bar);
                return;
            }

            bridgeTA.Push(bar);

            // strategy.MyOpenLongSync(Target.Exchange, Target.Symbol, xsTick[0].ask_p1 * 0.92, 100);
 
            //if (OpenTrigger(1))     // Long
            //{
            //    // skip trigger
            //    if (posTrace != null || orderLast != null)
            //    {
            //        strategy.WriteTDLog(Target.Symbol + " 触发开多信号但有仓位或有订单");
            //        return;
            //    }

            //    double vol = strategy.GetVolumn(Target.Exchange, Target.Symbol);

            //    if (vol == 0.0)
            //    {
            //        strategy.WriteTDLog(Target.Symbol + " 触发开多信号但未通过风控");
            //    }
            //    else
            //    {
            //        //strategy.MyOpenLongSync(target.Exchange, target.InstrumentId, xsTick[0].ask_p1, vol);
            //        strategy.WriteTDLog(string.Format("{0} 触发开多信号（订单{1})", Target.Symbol, 1));
            //    }
            //}

        }
        #endregion

    }
}

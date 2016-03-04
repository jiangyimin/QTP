﻿using System;
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
        #region parameters
        private int NDailyBars = 200;
        private int NBars = 200;
        private int NTicks = 500;
        #endregion

        // basic TA 
        private DailyTA baseTA;

        // cancel order interval
        private int orderLasting;
        private int cancelInterval = 15;

        #region RealTime Analyst
        private class Quota
        {
            public double EMA20 { get; set; }
            public double EMA5 { get; set; }
        }

        private RList<Tick> xsTick;
        private RList<KLineBar> xs;
        private RList<Quota> ys;

        private double maxprice;

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
            Quota q0 = ys[0];
            Quota q1 = ys[1];

            if (q1.EMA5 < q1.EMA20 && q0.EMA5 > q0.EMA20)
                return true;

            return false;
        }

        private bool CrossDown()
        {
            Quota q0 = ys[0];
            Quota q1 = ys[1];

            if (q1.EMA5 > q1.EMA20 && q0.EMA5 < q0.EMA20)
                return true;

            return false;
        }


        #endregion

        #region Init
        public override void Prepare()
        {
            // Get Upperlevel Data
            List<DailyBar> barsDaily =  strategy.GetLastNDailyBars(Target.Symbol, NDailyBars);
            baseTA = new DailyTA();

            string str = null;
            for (int i = barsDaily.Count - 1; i >= 0; i--)
            {
                DailyBar bar = barsDaily[i];
                str += Utils.StampToDateTimeString(bar.utc_time) + ",";
                if (bar.flag == 1)
                {
                    KLineDaily kx = new KLineDaily(bar);
                    baseTA.Push(kx);
                }
            }

            xsTick = new RList<Tick>();
            xs = new RList<KLineBar>();
            ys = new RList<Quota>();

            List<Bar> bars = strategy.GetLastNBars(Target.Symbol, 60, NBars);
            for (int i = bars.Count - 1; i >= 0; i--)
            {
                Push(bars[i]);
            }

            List<Tick> ticks = strategy.GetLastNTicks(Target.Symbol, NTicks);
            for (int i = ticks.Count - 1; i >= 0; i--)
            {
                Push(ticks[i]);
            }

            // 监控器完成数据准备
            // strategy.WriteInfo(string.Format("{0}监控器完成数据准备", Target.Symbol));
        }

        public override void InitializeBufferData()
        {
            for (int i = 0; i < barsBuffer.Count; i++)
            {
                Push(barsBuffer[i]);
            }

            for (int i = ticksBuffer.Count - 1; i >= 0; i--)
            {
                Push(ticksBuffer[i]);
            }

        }

        #endregion

        #region override
        public override string PulseHintMessage()
        {
            return string.Format("[{0} T:{1},B:{2}]", Target.Symbol, xsTick.Count - NTicks, xs.Count - NBars);
        }
        public override void OnPulse()
        {
            if (orderLast != null)
            {
                orderLasting++;
                if (orderLasting >= cancelInterval)
                {
                    strategy.CancelOrder(orderLast.cl_ord_id);
                    orderLast = null;
                }
            }
        }

        public override void OnPosition(Position pos)
        {
            posTrace = pos;
            SetStopLossPrice();
        }

        public override void OnTick(Tick tick)
        {
            // first call base
            base.OnTick(tick);

            Push(tick);

            // 触发平单
            if (posTrace != null)
            {
                if (posTrace.side == 1 && StopLossTrigger(posTrace.side))     // Long
                {
                    strategy.WriteInfo(Target.Symbol + " 触发平多信号");
                    strategy.MyCloseLongSync(Target.Exchange, Target.Symbol, xsTick[0].bid_p1, posTrace.volume);
                    orderLasting = 0;
                }

            }
        }

        public override void OnBar(Bar bar)
        {
            // must call base
            base.OnBar(bar);

            Push(bar);


            // strategy.MyOpenLongSync(Target.Exchange, Target.Symbol, xsTick[0].ask_p1 * 0.92, 100);
 
            if (OpenTrigger(1))     // Long
            {
                // skip trigger
                if (posTrace != null || orderLast != null)
                {
                    strategy.WriteInfo(Target.Symbol + " 触发开多信号但有仓位或有订单");
                    return;
                }

                double vol = strategy.GetVolumn(Target.Exchange, Target.Symbol);

                if (vol == 0.0)
                {
                    strategy.WriteInfo(Target.Symbol + " 触发开多信号但未通过风控");
                }
                else
                {
                    //strategy.MyOpenLongSync(target.Exchange, target.InstrumentId, xsTick[0].ask_p1, vol);
                    strategy.WriteInfo(string.Format("{0} 触发开多信号（订单{1})", Target.Symbol, 1));
                    orderLasting = 0;
                }
            }

        }
        #endregion

    }
}

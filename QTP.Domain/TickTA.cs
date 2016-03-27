using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMSDK;
using QTP.TAlib;

namespace QTP.Domain
{
    public class TickQuota
    {
        public Tick Tick { get; set; }

        public float Delay { get; set; }

        public float Interval { get; set; }
        public float Range { get; set; }

        public float AverageDelay { get; set; }
        public float AverageRange { get; set; }

    }
    public class TickTA
    {
        private RList<TickQuota> lstTickQuota = new RList<TickQuota>();
        private Stopwatch watch = new Stopwatch();

        // used for generater tickBar1M
        private Bar bar1M, semiBar;
        private bool isFirstTick;
        private int lastMinute;

        #region Properties

        public TickQuota LastestTickQuota 
        {
            get 
            {
                if (Count > 0) return lstTickQuota[0];
                return null;
            }
        }

        public TickQuota PrevTickQuota
        {
            get
            {
                if (Count > 1) return lstTickQuota[1];
                return null;
            }
        }

        public int Count 
        {
            get { return lstTickQuota.Count; }
        }

        public long TickProcessElapsed
        {
            get { return watch.ElapsedMilliseconds; }
        }

        public Bar Bar1M
        {
            get { return bar1M; }
        }
        #endregion

        #region Public methods

        public void Push(Tick tick, bool isLive = false)
        {
            watch.Reset();
            watch.Start();

            TickQuota tq = new TickQuota();
            tq.Tick = tick;

            if (isLive)      // 需要计算延时和 TickBar1M
            {
                CalculateBar(tick);
                tq.Delay = CalculateDelay(tick.utc_time);
            }

            if (Count > 0)
            {
                tq.Interval = (float)(tick.utc_time - lstTickQuota[0].Tick.utc_time);
                tq.Range = Math.Abs(tick.last_price - lstTickQuota[0].Tick.last_price);
            }

            // add to list
            lstTickQuota.Add(tq);

            watch.Stop();
        }

        public void Clear()
        {
            lstTickQuota.Clear();
        }

        #endregion

        #region private methods

        private void CalculateBar(Tick tick)
        {
            if (semiBar == null)
            {
                semiBar = new Bar();
                bar1M = new Bar();
                isFirstTick = true;
                lastMinute = Utils.UtcToDateTime(tick.utc_time).Minute;
            }

            // judge minute changed
            DateTime dt = Utils.UtcToDateTime(tick.utc_time);
            if (dt.Minute > lastMinute)
            {
                isFirstTick = true;
                lastMinute = dt.Minute;

                // copy semiBar to Bar
                bar1M.high = semiBar.high;
                bar1M.low = semiBar.low;
                bar1M.open = semiBar.open;
                bar1M.close = semiBar.close;
                bar1M.volume = semiBar.volume;
                bar1M.strtime = string.Format("{0:00}:{1:00}", DateTime.Now.Minute, DateTime.Now.Second);

                // clear semibar
                semiBar.high = semiBar.low = semiBar.open = semiBar.close = 0;
                semiBar.volume = 0.0;
            }

            // open
            if (isFirstTick)
            {
                semiBar.high = semiBar.low = semiBar.open = semiBar.close = tick.last_price;
            }
                
            // high
            if (semiBar.high < tick.last_price) semiBar.high = tick.last_price;
            // low
            if (semiBar.low > tick.last_price) semiBar.low = tick.last_price;
            // close
            semiBar.close = tick.last_price;
            // volumn
            semiBar.volume += tick.last_volume;

            isFirstTick = false;

        }
        private float CalculateDelay(double utc_time)
        {
            DateTime dt = Utils.UtcToDateTime(utc_time);

            if (DateTime.Now.CompareTo(dt) < 0 )
                return 0;

            TimeSpan ts = DateTime.Now.Subtract(dt); 

            float ret = ts.Seconds + (float)ts.Milliseconds / 1000;
            return ret;
        }

        #endregion
    }
}

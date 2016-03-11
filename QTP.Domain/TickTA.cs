using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMSDK;
using QTP.TAlib;
using QTP.Infra;

namespace QTP.Domain
{
    public class TickQuota
    {
        public Tick Tick { get; set; }

        public float Delay { get; set; }

        public float Range { get; set; }

        public float AverageDelay { get; set; }
        public float AverageRange { get; set; }

    }
    public class TickTA
    {
        private RList<TickQuota> lstTickQuota = new RList<TickQuota>();
        private Stopwatch watch = new Stopwatch();

        private Bar bar;

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
            get { return bar; }
        }
        #endregion

        #region Public methods

        public void Push(Tick tick, bool isLive)
        {
            watch.Start();

            TickQuota tq = new TickQuota();
            tq.Tick = tick;

            if (isLive)      // 需要计算延时
            {
                tq.Delay = CalculateDelay(tick.utc_time);
            }

            if (Count > 0)
            {
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

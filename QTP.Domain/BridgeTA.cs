using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using GMSDK;
using QTP.TAlib;
using QTP.Infra;

namespace QTP.Domain
{
    public class BridgeTA
    {
        #region internal Quata class
        private class Quota
        {
            public double EMA5 { get; set; }
            public double EMA20 { get; set; }

            public double MTR { get; set; }
            public double ATR { get; set; }

        }

        #endregion

        #region memebers

        // DailyBar
        private Dictionary<string, float> dictFactor = new Dictionary<string,float>();
        private float baseFactor;

        private RList<KLine> xsDaily = new RList<KLine>();
        private RList<Quota> ysDaily = new RList<Quota>();

        // 1M and 15M bars
        private RList<KLine> xs1M = new RList<KLine>();
        private RList<KLine> xs15M = new RList<KLine>();

        private RList<Quota> ys1M = new RList<Quota>();
        private RList<Quota> ys15M = new RList<Quota>();
        
        // Property
        public int CountBars
        {
            get { return xs1M.Count + xs1M.Count; }
        }
        #endregion

        #region Push and TA

        // For Daily
        public void Push(List<DailyBar> bars, int baseIndex = 0)
        {
            // set base adj_factor
            baseFactor = bars[baseIndex].adj_factor;

            for (int i = bars.Count - 1; i >= 0; i--)
            {
                DailyBar bar = bars[i];
                Push(bar);

                // Set dictFactor
                string key = Utils.UtcToDateTime(bar.utc_time).ToString("yyyyMMdd");
                if (!dictFactor.ContainsKey(key))
                    dictFactor[key] = bar.adj_factor;
            }

        }

        public void Push(DailyBar b)
        {
            Adj_factor(b);

            xsDaily.Add(new KLine(b.utc_time, b.open, b.close, b.high, b.low, b.volume));
            ysDaily.Add(new Quota());

            Formula.ATR<KLine, Quota>(xsDaily, 14, ysDaily);
        }

        // Minute Bars

        public void ClearBars()
        {
            xs1M.Clear();
            xs15M.Clear();
            ys1M.Clear();
            ys15M.Clear();
        }

        public void Push(List<Bar> bars)
        {
            for (int i = bars.Count - 1; i >= 0; i--)
            {
                Push(bars[i]);
            }
        }


        public void Push(Bar b)
        {
            Adj_factor(b);
            if (b.bar_type == 60)
            {
                xs1M.Add(new KLine(b.utc_time, b.open, b.close, b.high, b.low, b.volume));
                ys1M.Add(new Quota());

                // caculate quotas
                Formula.EMA<KLine, Quota>(xs1M, "CLOSE", 5, ys1M, "EMA5");
                Formula.EMA<KLine, Quota>(xs1M, "CLOSE", 20, ys1M, "EMA20");

            }
            else if (b.bar_type == 900)
            {
                xs15M.Add(new KLine(b.utc_time, b.open, b.close, b.high, b.low, b.volume));
                ys15M.Add(new Quota());

                // caculate quotas
                Formula.EMA<KLine, Quota>(xs15M, "CLOSE", 5, ys15M, "EMA5");
                Formula.EMA<KLine, Quota>(xs15M, "CLOSE", 20, ys15M, "EMA20");
            }
        }
        
        // private utils
        private void Adj_factor(DailyBar bar)
        {
            if (bar.adj_factor == baseFactor) return;

            bar.open = bar.open * bar.adj_factor / baseFactor;
            bar.close = bar.close * bar.adj_factor / baseFactor;
            bar.high = bar.high * bar.adj_factor / baseFactor;
            bar.low = bar.low * bar.adj_factor / baseFactor;
        }

        private void Adj_factor(Bar bar)
        {
            if (bar.adj_factor == baseFactor) return;

            string key = Utils.UtcToDateTime(bar.utc_time).ToString("yyyyMMdd");
            if (dictFactor.ContainsKey(key))
            {
                float factor = dictFactor[key];
                bar.open = bar.open * factor / baseFactor;
                bar.close = bar.close * factor / baseFactor;
                bar.high = bar.high * factor / baseFactor;
                bar.low = bar.low * factor / baseFactor;
            }
        }
        #endregion

        #region Get Quota and KLine

        // Quotas
        public static Type QuotaType
        {
            get { return typeof(Quota); }
        }

        public object GetLatestQuota(int ktype)
        {
            switch (ktype)
            {
                case 0:
                case 240:
                    return ysDaily[0];
                case 1:
                    return ys1M[0];
                case 15:
                    return ys15M[0];
                default:
                    return null;
            }
        }

        public RList<object> GetQuotas(int ktype)
        {
            switch (ktype)
            {
                case 0:
                case 240:
                    return ysDaily.GetObjectList();
                case 1:
                    return ys1M.GetObjectList();
                case 15:
                    return ys15M.GetObjectList();
                default:
                    return null;
            }
        }

        public RList<KLine> GetKLines(int ktype)
        {
            switch (ktype)
            {
                case 0:
                case 240:
                    return xsDaily;
                case 1:
                    return xs1M;
                case 15:
                    return xs15M;
                default:
                    return null;
            }
        }

        #endregion
    }
}

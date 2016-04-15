using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace QTP.TAlib
{
    public class Formula
    {
        public static void MA(RList<double> s, int n, RList<double> t)
        {
            int length = s.Count;
            if (length < n)
                return;

            if (length == n)        // 设定初始值
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    sum += s[i];
                }
                t[0] = sum / n;
            }
            else
            {
                t[0] = t[1] + (s[0] - s[n]) / n;
            }
        }

        public static void MA(RList<KLine> s, string sname, int n, RList<double> t)
        {
            int length = s.Count;
            if (length < n)
                return;

            // get property by name
            PropertyInfo propS = typeof(KLine).GetProperty(sname);

            if (length == n)        // 设定初始值
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    sum += Convert.ToDouble(propS.GetValue(s[i]));
                }
                t[0] = sum / n;
            }
            else
            {
                double s0 = Convert.ToDouble(propS.GetValue(s[0]));
                double sn = Convert.ToDouble(propS.GetValue(s[n]));
                t[0] = t[1] + (s0 - sn) / n;
            }
        }

        public static void EMA(RList<double> s, int n, RList<double> t)
        {
            int length = s.Count;
            if (length < n) return;

            if (length == n)        // 设定初始值
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    sum += s[i];
                }
                t[0] = sum / n;
            }
            else
            {
                t[0] = (2 * s[0] + t[1] * (n - 1)) / (n + 1);
            }

        }

        public static void EMA(RList<KLine> s, string sname, int n, RList<double> t)
        {
            int length = s.Count;
            if (length < n) return;

            // get property by name
            PropertyInfo propS = typeof(KLine).GetProperty(sname);

            if (length == n)        // 设定初始值
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    sum += Convert.ToDouble(propS.GetValue(s[i]));
                }
                t[0] = sum / n;
            }
            else
            {
                double s0 = Convert.ToDouble(propS.GetValue(s[0]));
                double t1 = t[1];
                t[0] = (2 * s0 + t1 * (n - 1)) / (n + 1);
            }
        }

        public static void ATR(RList<KLine> s, int n, RList<double> t1, RList<double> t2)
        {
            int length = s.Count;
            if (length <= 1)
                return;

            PropertyInfo propHIGH = typeof(KLine).GetProperty("HIGH");
            PropertyInfo propLOW = typeof(KLine).GetProperty("LOW");
            PropertyInfo propCLOSE = typeof(KLine).GetProperty("CLOSE");

            // Caculate MTR

            double high = Convert.ToDouble(propHIGH.GetValue(s[0]));
            double low = Convert.ToDouble(propLOW.GetValue(s[0]));

            double mtr = Math.Max(high - low, Math.Abs(Convert.ToDouble(propCLOSE.GetValue(s[1])) - high));
            mtr = Math.Max(mtr, Math.Abs(Convert.ToDouble(propCLOSE.GetValue(s[1])) - low));

            t1[0] = mtr;

            // Caculate ATR
            MA(t1, n, t2);
        }

        public static void SKDJ(RList<KLine> s, int n, int m, RList<double> rLLV, RList<double> rHHV, RList<double> rRat, RList<double> rRsv, RList<double> rK, RList<double> rD)
        {
            int length = s.Count;
            if (length < n)
                return;

            PropertyInfo propHIGH = typeof(KLine).GetProperty("HIGH");
            PropertyInfo propLOW = typeof(KLine).GetProperty("LOW");
            PropertyInfo propCLOSE = typeof(KLine).GetProperty("CLOSE");

            rLLV[0] = LLV(s, propLOW, n);
            rHHV[0] = HHV(s, propHIGH, n);

            double close = Convert.ToDouble(propCLOSE.GetValue(s[0]));
            if (rHHV[0] == rLLV[0])
                rRat[0] = 100;
            else
                rRat[0] = (close - rLLV[0]) / (rHHV[0] - rLLV[0]) * 100;
            EMA(rRat, m, rRsv);
            EMA(rRsv, m, rK);
            MA(rK, m, rD);
        }

        public static void TDM(RList<KLine> s, RList<double> rU, RList<double> rD)
        {
            int length = s.Count;
            if (length < 6)
                return;

            PropertyInfo prop = typeof(KLine).GetProperty("CLOSE");

            double curr = Convert.ToDouble(prop.GetValue(s[0]));
            double prev4 = Convert.ToDouble(prop.GetValue(s[4]));

            if (rU[1] == 0.0)
            {                       // find td struct
                double prev1 = Convert.ToDouble(prop.GetValue(s[1]));
                double prev5 = Convert.ToDouble(prop.GetValue(s[5]));

                if (curr > prev4 && prev1 < prev5) 
                    rU[0] = 1;
            }
            else
            {
                if (curr > prev4) 
                    rU[0] = rU[1] + 1;
            }

            if (rD[1] == 0.0)
            {                       // find td struct
                double prev1 = Convert.ToDouble(prop.GetValue(s[1]));
                double prev5 = Convert.ToDouble(prop.GetValue(s[5]));

                if (curr < prev4 && prev1 > prev5) 
                    rD[0] = 1;
            }
            else
            {
                if (curr < prev4) 
                    rD[0] = rD[1] + 1;
            }

        }


        #region private utils

        private static double LLV(RList<KLine>s, PropertyInfo prop, int n)
        {
            double min = Convert.ToDouble(prop.GetValue(s[0]));

            for (int i = 1; i < n; i++)
            {
                double d = Convert.ToDouble(prop.GetValue(s[i]));
                if (min > d) min = d;
            }

            return min;
        }
        private static double HHV(RList<KLine>s, PropertyInfo prop, int n)
        {
            double max = Convert.ToDouble(prop.GetValue(s[0]));

            for (int i = 1; i < n; i++)
            {
                double d = Convert.ToDouble(prop.GetValue(s[i]));
                if (max < d) max = d;
            }

            return max;
        }

        #endregion
    }

}

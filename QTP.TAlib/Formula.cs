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

        public static void EMA<S, T>(RList<S> s, string sname, int n, RList<T> t, string tname)
        {
            EMA<S, T>(s, 0, sname, n, t, tname);
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

        public static void EMA<S, T>(RList<S> s, int start, string sname, int n, RList<T> t, string tname)
        {
            int length = s.Count;
            if (length < n + start)
                return;

            // get property by name
            PropertyInfo propS = typeof(S).GetProperty(sname);
            
            PropertyInfo propT = typeof(T).GetProperty(tname);

            if (length == n + start)        // 设定初始值
            {
                double sum = 0.0;
                for (int i = start; i < n + start; i++)
                {
                    sum+= Convert.ToDouble(propS.GetValue(s[i]));
                }
                propT.SetValue(t[0], sum / n);
            }
            else
            {
                double s0 = Convert.ToDouble(propS.GetValue(s[0]));
                double t1 = Convert.ToDouble(propT.GetValue(t[1]));
                propT.SetValue(t[0], (2*s0 + t1*(n-1))/(n+1));
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
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace QTP.Domain
{
    public class Utils
    {
        public static string DTString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string DTLongString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }


        public static bool IsToday(double utc)
        {
            DateTime dt = UtcToDateTime(utc);
            return dt.Year == DateTime.Now.Year && dt.Month == DateTime.Now.Month && dt.Day == DateTime.Now.Day;
        }
        public static DateTime UtcToDateTime(double utc)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dt = dateTimeStart.AddSeconds(utc);
            return dt;
        }

        public static string GetStartTimeString(int bar_type, int num, DateTime from)
        {
            DateTime start = new DateTime(from.Year, from.Month, from.Day, 15, 0, 0);

            // 日线
            if (bar_type == 0)
            {
                start = from.Subtract(new TimeSpan(num, 0, 0, 0, 0));
                return start.ToString("yyyy-MM-dd");
            }
            else
            {
                int numMinutes = bar_type * num;
                int days = numMinutes / 240;
                start = from.Subtract(new TimeSpan(days, 0, 0, 0, 0));

                return GetStartTimeString(numMinutes % 240, start);
            }
        }

        private static string GetStartTimeString(int minutes, DateTime from)
        {
            DateTime openM = new DateTime(from.Year, from.Month, from.Day, 9, 30, 0);
            DateTime openP = new DateTime(from.Year, from.Month, from.Day, 13, 0, 0);

            int lastingMinutes = GetLastingMinutes(from);
            if (lastingMinutes > minutes)
            {
                if (lastingMinutes - minutes >= 120)
                    return DTLongString(openP.Add(new TimeSpan(0, lastingMinutes - minutes - 120, 0)));
                else
                    return DTLongString(openM.Add(new TimeSpan(0, lastingMinutes - minutes, 0)));
            }
            else
            {
                openM = openM.Subtract(new TimeSpan(1, 0, 0, 0));
                openP = openP.Subtract(new TimeSpan(1, 0, 0, 0));
                if (240 - minutes + lastingMinutes >= 120)
                    return DTLongString(openP.Add(new TimeSpan(0, 120 - minutes + lastingMinutes, 0)));
                else
                    return DTLongString(openM.Add(new TimeSpan(0, 240 - minutes + lastingMinutes, 0)));
            }
        }

        private static int GetLastingMinutes(DateTime dt)
        {
            DateTime openM = new DateTime(dt.Year, dt.Month, dt.Day, 9, 30, 0);
            DateTime endM = new DateTime(dt.Year, dt.Month, dt.Day, 11, 30, 0);
            DateTime openP = new DateTime(dt.Year, dt.Month, dt.Day, 13, 0, 0);
            DateTime endP = new DateTime(dt.Year, dt.Month, dt.Day, 15, 0, 0);

            if (dt < openM)
                return 0;
            else if (dt >= openM && dt <= endM)
                return dt.Subtract(openM).Minutes;
            else if (dt > endM && dt < openP)
                return 120;
            else if (dt >= openP && dt <= endP)
                return dt.Subtract(openP).Minutes + 120;
            else
                return 240;
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME 
        { 
            public short year; 
            public short month; 
            public short dayOfWeek; 
            public short day; 
            public short hour; 
            public short minute; 
            public short second; 
            public short milliseconds;
        }
        /// <summary>  /// 设置系统时间 /// </summary> ///
        /// <param name="dt">需要设置的时间</param>  
        /// <returns>返回系统时间设置状态，true为成功，false为失败</returns>  ///
        public static bool SetDate(DateTime dt) 
        {   
            SYSTEMTIME st;   
            st.year = (short)dt.Year;   
            st.month = (short)dt.Month;   
            st.dayOfWeek = (short)dt.DayOfWeek;   
            st.day = (short)dt.Day;   
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;
            st.milliseconds = (short)dt.Millisecond;
            bool rt = SetLocalTime(ref st);
            return rt;
        }
    }
}

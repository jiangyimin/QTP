using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace QTP.Infra
{
    public class Utils
    {
        private static DateTime StockMarketBeginTime = new DateTime(2000, 1, 1, 9, 10, 0);    // 上午9点
        private static DateTime StockMarketEndTime = new DateTime(2000, 1, 1, 15, 10, 0);     // 下午3点

        public static bool IsInStockMarkerOpenPeriod(DateTime dtNow)
        {
            if (dtNow.DayOfWeek == DayOfWeek.Saturday || dtNow.DayOfWeek == DayOfWeek.Sunday)
                return false;
                
            DateTime dt = new DateTime(2000, 1, 1, dtNow.Hour, dtNow.Minute, 0);
            return  dt.CompareTo(StockMarketBeginTime) > 0 && dt.CompareTo(StockMarketEndTime) < 0;
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

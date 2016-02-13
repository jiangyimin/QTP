using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using QTP.Domain;
using QTP.DBAccess;

namespace QTP.Main
{
    public class Global
    {
        #region global settings

        // can close
        public static bool CanClose = false;

        // Tlogin
        public static TLogin Login;

        // EventServer
//        public EventServer ES;

        
        // Stocks
        // public SecList SecNames; 
        // http://cloud.myquant.cn:7000/v1/stkbase?fields=sec_name
        // http://cloud.myquant.cn:7000/v1/contracts?fields=sec_name

        // Running Strategies
        public static Dictionary<int, StrategyQTP> RunningStrategies = new Dictionary<int, StrategyQTP>();



        #endregion

        #region tils
        public static void Load()
        {
            // Load SecNames
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://cloud.myquant.cn:7000/v1/stkbase?fields=sec_name");
            request.Timeout = 5000; 
            request.Method = "GET"; 

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
//                SecNames = JSON.parse<SecList>(sr.ReadToEnd());
                sr.Close();
            }
            catch
            {
                Console.WriteLine("GetResponse wrong!");
            }

        }
        #endregion
    }

}

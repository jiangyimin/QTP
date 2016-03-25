using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using System.Configuration;

using QTP.DBAccess;

namespace QTP.Main
{
    public class Global
    {
        #region global settings

        // Tlogin
        public static TLogin Login;
        public static string ExePath, ExeName;

        public static List<TStrategy> RealStrategies = new List<TStrategy>();
        public static List<TStrategy> SimuStrategies = new List<TStrategy>();

        #endregion

        #region tils
        public static void Load()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            NameValueCollection nv = (NameValueCollection)ConfigurationManager.GetSection("GMStrategyRun");
            Login.UserName = nv["UserName"];
            Login.Password = nv["Password"];
            ExePath = nv["ExePath"];
            ExeName = nv["ExeName"];


            // Load TStatrategies
            List<TStrategy> lst = CRUD.GetTStrategies();
            foreach (TStrategy t in lst)
            {
                // parse monitorName, riskMName, tradeChannel
                t.Parse();

                // get subTables.
                t.Pool = CRUD.GetTStrategyPool(t.PoolId);
                t.Instruments = CRUD.GetTStrategyInstruments(t.PoolId);

                // Add to list.
                if (t.RunType == "实盘") RealStrategies.Add(t);
                if (t.RunType == "虚拟") SimuStrategies.Add(t);
            }
        }

        #endregion
    }

}

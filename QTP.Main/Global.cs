using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;

using QTP.Domain;
using QTP.DBAccess;

namespace QTP.Main
{
    public class Global
    {
        #region global settings

        // Tlogin
        public static TLogin Login;

        public static List<StrategyQTP> RealStrategies = new List<StrategyQTP>();
        public static List<StrategyQTP> SimuStrategies = new List<StrategyQTP>();

        // EventServer
//        public EventServer ES;

        
        // Stocks
        // public SecList SecNames; 
        

        #endregion

        #region tils
        public static void Load()
        {
            // Load TStatrategies
            List<TStrategy> lst = CRUD.GetTStrategies();
            foreach (TStrategy t in lst)
            {
                // get subTables.
                t.Instruments = CRUD.GetTStrategyInstruments(t.Id);

                // Get Type of Monitor and RiskM
                Assembly assembly = Assembly.LoadFrom(t.DLLName+".DLL");
                string name = System.Text.RegularExpressions.Regex.Match(t.MonitorClass, @"[^(]+").Value;
                Type monitorType = assembly.GetType(string.Format("{0}.{1}", t.DLLName, name));

                name = System.Text.RegularExpressions.Regex.Match(t.RiskMClass, @"[^(]+").Value;
                Type riskType = assembly.GetType(string.Format("{0}.{1}", t.DLLName, name));

                // new StrategyQTP and Add to list.
                StrategyQTP qtp = new StrategyQTP(t, monitorType, riskType, Global.Login);
                if (t.RunType == "实盘") RealStrategies.Add(qtp);
                if (t.RunType == "模拟") SimuStrategies.Add(qtp);
            }

        }
        #endregion
    }

}

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

        public static List<MyStrategy> RealStrategies = new List<MyStrategy>();
        public static List<MyStrategy> SimuStrategies = new List<MyStrategy>();

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
                // parse monitorName, riskMName, tradeChannel
                t.Parse();

                // get subTables.
                t.Instruments = CRUD.GetTStrategyInstruments(t.Id);

                // new StrategyQTP and Add to list.
                MyStrategy qtp = new MyStrategy(t, Global.Login);
                if (t.RunType == "实盘") RealStrategies.Add(qtp);
                if (t.RunType == "模拟") SimuStrategies.Add(qtp);
            }

        }
        #endregion
    }

}

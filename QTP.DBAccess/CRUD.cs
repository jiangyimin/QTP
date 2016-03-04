using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

using QTP.Infra;

namespace QTP.DBAccess
{
    public class CRUD
    {
        public static string ConnectionString;
        public static string Provider;

        public static TLogin GetTLogin()
        {
            return MySqlHelper.ExecuteObject<TLogin>(ConnectionString, "SELECT * FROM Login");
        }

        public static List<TStrategy> GetTStrategies()
        {
            return MySqlHelper.ExecuteObjects<TStrategy>(ConnectionString, "SELECT * FROM Strategy");
        }

        public static List<TInstrument> GetTStrategyInstruments(int id)
        {
            return MySqlHelper.ExecuteObjects<TInstrument>(ConnectionString, string.Format("SELECT * FROM Instrument WHERE StrategyId={0}", id));
        }

        public static TStrategy GetTStrategy(string id)
        {
            TStrategy t = new TStrategy();

            t = MySqlHelper.ExecuteObject<TStrategy>(ConnectionString, string.Format("SELECT * FROM Strategy WHERE Id={0}", id));
            t.Parse();          // parse fields
            t.Instruments = GetTStrategyInstruments(int.Parse(id));
            //t.Positions = MySqlHelper.ExecuteObjects<TPosition>(ConnectionString, string.Format("SELECT * FROM Position WHERE StrategyId={0}", id)); 

            return t;
        }

    }
}

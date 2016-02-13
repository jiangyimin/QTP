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

        public static TStrategy GetTStrategy(string id, NLog log)
        {
            TStrategy t = new TStrategy();

            try
            {
                t = MySqlHelper.ExecuteObject<TStrategy>(ConnectionString, string.Format("SELECT * FROM Strategy WHERE Id={0}", id));

                t.Login = MySqlHelper.ExecuteObject<TLogin>(ConnectionString, "SELECT * FROM Login", null);
                t.Instruments = MySqlHelper.ExecuteObjects<TInstrument>(ConnectionString, string.Format("SELECT * FROM Instrument WHERE StrategyId={0}", id));
                t.Positions = MySqlHelper.ExecuteObjects<TPosition>(ConnectionString, string.Format("SELECT * FROM Position WHERE StrategyId={0}", id)); 
            }
            catch 
            {
                log.WriteError("Read TStrategies");
            }

            return t;
        }

    }
}

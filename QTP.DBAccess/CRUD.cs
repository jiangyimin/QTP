using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using QTP.Infra;

namespace QTP.DBAccess
{
    public class CRUD
    {
        public static string ConnectionString;

        public static TStrategy GetTStrategy(string id, NLog log)
        {
            TStrategy t = new TStrategy();

            try
            {
                t = SqlHelper.ExecuteObject<TStrategy>(ConnectionString, string.Format("SELECT * FROM Strategy WHERE Id={0}", id), null);

                t.Login = SqlHelper.ExecuteObject<TLogin>(ConnectionString, "SELECT * FROM Login", null);
                t.Instruments = SqlHelper.ExecuteObjects<TInstrument>(ConnectionString, string.Format("SELECT * FROM PoolInstrument WHERE PoolId={0}", t.PoolId), null);
                t.Positions = SqlHelper.ExecuteObjects<TPosition>(ConnectionString, string.Format("SELECT * FROM Position WHERE StrategyId={0}", id), null); 
            }
            catch 
            {
                log.WriteError("Read TStrategies");
            }

            return t;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace QTP.DBAccess
{
    public class TStrategy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RunType { get; set; }
        public string TAInfo { get; set; }
        public string RiskMInfo { get; set; }
        public string GMID { get; set; }
        public string TradeChannel { get; set; }
        public string DLLName { get; set; }
        public int PoolId { get; set; }
        public string BackTestInfo { get; set; }

        public TPool Pool { get; set; }
        public List<TInstrument> Instruments { get; set; }
        public List<TPosition> Positions { get; set; }

        #region parsed properties
        public Dictionary<string, string> TAInfoParameters { get; set; }
        public Dictionary<string, string> RiskMInfoParameters { get; set; }
        public string TradeChannelName { get; set; }
        public Dictionary<string, string> TradeChannelParameters { get; set; }
        public Dictionary<string, string> BackTestInfoParameters { get; set; }

        #endregion
        public void Parse()
        {
            // parse TInfo
            Assembly assembly = Assembly.LoadFrom(DLLName + ".DLL");
            Pool.ManagerType = assembly.GetType(string.Format("{0}.{1}", DLLName, Pool.ManagerName));
            Pool.ManagerParameters = GetDictByString(Pool.ManagerInfo);

            // TA Monitor and RiskM
            TAInfoParameters = GetDictByString(TAInfo);
            RiskMInfoParameters = GetDictByString(RiskMInfo);

            // Tradge Channel
            string[] ns = TradeChannel.Split('(', ')');
            TradeChannelName = ns[0];
            TradeChannelParameters = GetDictByString(ns[1]);

            BackTestInfoParameters = GetDictByString(BackTestInfo);
        }

        private Dictionary<string, string> GetDictByString(string parsStr)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] flds = parsStr.Split(';');

            foreach (string fld in flds)
            {
                string[] pair = fld.Split('=');
                if (pair.Length < 2)
                    break;
                dict[pair[0].Trim()] = pair[1].Trim();
            }

            return dict;
        }
    }
}

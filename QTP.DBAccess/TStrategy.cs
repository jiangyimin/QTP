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

        public string MonitorClass { get; set; }
        public string RiskMClass { get; set; }
        public string GMID { get; set; }
        public string TradeChannel { get; set; }
        public string DLLName { get; set; }

        public List<TInstrument> Instruments { get; set; }
        public List<TPosition> Positions { get; set; }

        #region parsed properties
        public Type MonitorType { get; set; }
        public string MonitorParemeters { get; set; }

        public Type RiskMType { get; set; }
        public string RiskMParemeters { get; set; }
        public string TradeChannelName { get; set; }
        public string TradeChannelParameters { get; set; }

        #endregion
        public void Parse()
        {
            // Get Type of Monitor and RiskM
            Assembly assembly = Assembly.LoadFrom(DLLName + ".DLL");
            string[] ns = MonitorClass.Split('(', ')');
            MonitorType = assembly.GetType(string.Format("{0}.{1}", DLLName, ns[0]));
            MonitorParemeters = ns[1];

            ns = RiskMClass.Split('(', ')');
            RiskMType = assembly.GetType(string.Format("{0}.{1}", DLLName, ns[0]));
            RiskMParemeters = ns[1];


            ns = TradeChannel.Split('(', ')');
            TradeChannelName = TradeChannel.Split('(')[0];
            TradeChannelParameters = ns[1];
        }

    }
}

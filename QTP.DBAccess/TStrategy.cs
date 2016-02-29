using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

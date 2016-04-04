using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.DBAccess
{
    public class TInstrument
    {
        public string InstrumentId { get; set; }
        public string Exchange { get; set; }

        public int Volume { get; set; }
        public float StopLossPrice { get; set; }
        public string MonitorClass { get; set; }
        public string Symbol
        {
            get { return string.Format("{0}.{1}", Exchange, InstrumentId); }
        }
    }
}

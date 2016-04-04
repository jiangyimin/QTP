using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.TAlib
{
    [Serializable]
    public class KLine
    {
        public KLine (string symbol, string time, double utc, float o, float c, float h, float l, double v)
        {
            Symbol = symbol;
            StrTime = time;
            UTC = utc;
            OPEN = o;
            CLOSE = c;
            HIGH = h;
            LOW = l;
            VOLUMN = v;
        }

        public string Symbol { get; set; }
        public string StrTime { get; set; }
        public double UTC { get; set; }
        public float OPEN { get; set; }
        public float CLOSE { get; set; }
        public float HIGH { get; set; }
        public float LOW { get; set; }
        public double VOLUMN { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.TAlib
{
    public class KLine
    {
        public KLine (double utc, float o, float c, float h, float l, double v)
        {
            UTC = utc;
            OPEN = o;
            CLOSE = c;
            HIGH = h;
            LOW = l;
            VOLUMN = v;
        }

        public double UTC { get; set; }
        public float OPEN { get; set; }
        public float CLOSE { get; set; }
        public float HIGH { get; set; }
        public float LOW { get; set; }
        public double VOLUMN { get; set; }
    }
}

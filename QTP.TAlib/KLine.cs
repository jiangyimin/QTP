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
        public KLine (string symbol, double utc, float o, float c, float h, float l, double v)
        {
            Symbol = symbol;
            UTC = utc;
            OPEN = o;
            CLOSE = c;
            HIGH = h;
            LOW = l;
            VOLUME = v;
        }

        public KLine Copy()
        {
            return new KLine(this.Symbol, this.UTC, this.OPEN, this.CLOSE, this.HIGH, this.LOW, this.VOLUME);
        }
        public string Symbol { get; set; }
        public double UTC { get; set; }
        public float OPEN { get; set; }
        public float CLOSE { get; set; }
        public float HIGH { get; set; }
        public float LOW { get; set; }
        public double VOLUME { get; set; }
    }
}

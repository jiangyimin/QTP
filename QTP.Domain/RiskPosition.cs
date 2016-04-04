using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.Domain
{
    public class RiskPosition
    {
        public RiskPosition(double volume, double cost_price)
        {
            Volume = volume;
            CostPrice = cost_price;
        }
        
        public double Volume { get; set; }

        public double CostPrice { get; set; }

        public int VolumeAux { get; set; }
        public float StopLossPrice { get; set; }
    }
}

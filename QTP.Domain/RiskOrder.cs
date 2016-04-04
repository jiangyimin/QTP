using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMSDK;
namespace QTP.Domain
{
    public class RiskOrder
    {
        public RiskOrder(Order gmOrder)
        {
            GMOrder = gmOrder;
        }

        public Order GMOrder { get; set; }

        // Web Order (entrust)
    

    }
}

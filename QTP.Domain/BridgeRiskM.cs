using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.DBAccess;
using GMSDK;

namespace QTP.Domain
{
    public class BridgeRiskM : RiskM
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override double GetVolume(string exchange, string sec_id)
        {
            return 1000;
        }
    }
}

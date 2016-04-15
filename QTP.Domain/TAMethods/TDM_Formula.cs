using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;

namespace QTP.Domain
{
    public class TDM_Formula : TAFormula
    {
        private RList<double> rU = new RList<double>();
        private RList<double> rD = new RList<double>();

        public TDM_Formula()
        {
            name = "九转";
            scalarNames = new List<string>() { "U", "D" };

        }

        #region implement ITAData
        public override void Push(RList<KLine> x)
        {
            rU.Add(0.0);
            rD.Add(0.0);
            Formula.TDM(x, rU, rD);
        }

        public override List<double> GetLatestScalarValues()
        {
            List<double> ret = new List<double>();
            if (rU.Count > 0)
            {
                ret.Add(rU[0]); ret.Add(rD[0]);
            }
            return ret;
        }

        public override List<RList<double>> GetScalarValues()
        {
            return new List<RList<double>>(){ rU, rD };
        }

        public override void Clear()
        {
            rU.Clear();
            rD.Clear();
        }
        #endregion

        #region implement ITATrade
        public override bool Filter()
        {
            if (rD.Count > 1)
                return rD[0] >= 7;

            return false;
        }

        #endregion
    }
}

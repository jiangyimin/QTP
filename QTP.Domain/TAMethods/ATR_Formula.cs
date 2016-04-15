using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;
namespace QTP.Domain
{
    public class ATR_Formula : TAFormula
    {
        private int n = 14;

        private RList<double> yMTR = new RList<double>();
        private RList<double> yATR = new RList<double>();

        public ATR_Formula(params string[] pars)
        {
            name = "真实波幅";
            scalarNames = new List<string>() { "MTR", "ATR" };

            if (pars != null && pars.Length == 1)
            this.n = Convert.ToInt32(pars[0]);
        }

        public override void Push(RList<KLine> x)
        {
            yMTR.Add(0.0);
            yATR.Add(0.0);
            Formula.ATR(x, n, yMTR, yATR);

        }
        public override List<double> GetLatestScalarValues()
        {
            List<double> ret = new List<double>();
            if (yMTR.Count > 0)
            {
                ret.Add(yMTR[0]); ret.Add(yATR[0]);
            }
            return ret;
        }
        public override List<RList<double>> GetScalarValues()
        {
            return new List<RList<double>>() { yMTR, yATR };
        }

        public override void Clear()
        {
        }


        #region implement ITATrade 

        public override double GetStopLessRisk()
        {
            return 2 * yATR[0];
        }
        #endregion
    }
}

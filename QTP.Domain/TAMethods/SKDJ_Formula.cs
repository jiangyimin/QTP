using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;

namespace QTP.Domain
{
    public class SKDJ_Formula : TAFormula
    {
        private int cN = 9;
        private int cM = 3;

        private RList<double> rLLV = new RList<double>();
        private RList<double> rHHV = new RList<double>();
        private RList<double> rRat = new RList<double>();
        private RList<double> rRsv = new RList<double>();
        private RList<double> rK = new RList<double>();
        private RList<double> rD = new RList<double>();

        public SKDJ_Formula(params string[] pars)
        {
            name = "慢速KDJ";
            scalarNames = new List<string>() { "K", "D" };

            if (pars != null && pars.Length == 2)
            {
                this.cN = Convert.ToInt32(pars[0]);
                this.cM = Convert.ToInt32(pars[1]);
            }
        }

        public override void Push(RList<KLine> x)
        {
            rLLV.Add(0.0);
            rHHV.Add(0.0);
            rRat.Add(0.0);
            rRsv.Add(0.0); 
            rK.Add(0.0);
            rD.Add(0.0);
            Formula.SKDJ(x, cN, cM, rLLV, rHHV, rRat, rRsv, rK, rD);
        }

        public override List<double> GetLatestScalarValues()
        {
            List<double> ret = new List<double>();
            if (rK.Count > 0)
            {
                ret.Add(rK[0]); ret.Add(rD[0]);
            }
            return ret;
        }

        public override List<RList<double>> GetScalarValues()
        {
            return new List<RList<double>>(){ rK, rD };
        }

        public override void Clear()
        {
            rLLV.Clear();
            rHHV.Clear();
            rRat.Clear();
            rRsv.Clear(); 
            rK.Clear();
            rD.Clear();
        }

        #region ITATrade

        public override bool IsBuyPoint()
        {
            return (rD[0] <= 20 && rK[1] < rD[1] && rK[0] > rD[0]);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;

namespace QTP.Domain
{
    public class EMAT_Formula : TAFormula
    {
        private int n1 = 5;
        private int n2 = 10;
        private int n3 = 21;

        private RList<double> y1 = new RList<double>();
        private RList<double> y2 = new RList<double>();
        private RList<double> y3 = new RList<double>();

        public EMAT_Formula(params string[] pars)
        {
            name = "三均线";
            scalarNames = new List<string>() { "短期", "中期", "长期" };

            if (pars != null && pars.Length == 2)
            {
                this.n1 = Convert.ToInt32(pars[0]);
                this.n2 = Convert.ToInt32(pars[1]);
                this.n3 = Convert.ToInt32(pars[2]);
            }
        }


        #region implement ITAData interface

        public override void Push(RList<KLine> x)
        {
            y1.Add(0.0); 
            Formula.EMA(x, "CLOSE", n1, y1);
            y2.Add(0.0);
            Formula.EMA(x, "CLOSE", n2, y2);
            y3.Add(0.0);
            Formula.EMA(x, "CLOSE", n3, y3);
        }

        public override List<double> GetLatestScalarValues()
        {
            List<double> ret = new List<double>();
            if (y1.Count > 0)
            {
                ret.Add(y1[0]); ret.Add(y2[0]); ret.Add(y3[0]);
            }
            return ret;
        }

        public override List<RList<double>> GetScalarValues()
        {
            return new List<RList<double>>(){ y1, y2, y3 };
        }

        public override void Clear()
        {
            y1.Clear();
            y2.Clear();
            y3.Clear();
        }
        #endregion

        #region implement ITATrade interface

        public override bool Filter()
        {
            if (y3.Count > 2)
                return (y3[0] >= y3[1] && y3[1] >= y3[2]);
            else
                return false;
        }

        #endregion

    }
}

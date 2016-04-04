using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;
namespace QTP.Domain
{
    public class EMA2_Formula : TAFormula
    {
        private int n1 = 5;
        private int n2 = 20;

        private RList<double> y1 = new RList<double>();
        private RList<double> y2 = new RList<double>();

        #region implement interface

        #endregion

        public EMA2_Formula(params string[] pars)
        {
            name = "双均线";
            scalarNames = new List<string>() { "短期线", "长期线" };

            if (pars != null && pars.Length == 2)
            {
                this.n1 = Convert.ToInt32(pars[0]);
                this.n2 = Convert.ToInt32(pars[1]);
            }
        }


        public override void Push(RList<KLine> x)
        {
            y1.Add(0.0); 
            Formula.EMA(x, "CLOSE", n1, y1);
            y2.Add(0.0);
            Formula.EMA(x, "CLOSE", n2, y2);
        }

        public override List<double> GetLatestScalarValues()
        {
            List<double> ret = new List<double>();
            if (y1.Count > 0)
            {
                ret.Add(y1[0]); ret.Add(y2[0]);
            }
            return ret;
        }

        public override List<RList<double>> GetScalarValues()
        {
            return new List<RList<double>>(){ y1, y2 };
        }

        public override void Clear()
        {
            y1.Clear();
            y2.Clear();
        }
    }
}

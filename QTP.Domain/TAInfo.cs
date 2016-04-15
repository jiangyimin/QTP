using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace QTP.Domain
{
    public class FormulaInfo
    {
        public int KType { get; set; }

        public string Name { get; set; }

        public string[] Parameters { get; set; }

        public TAFormula Formula { get; set; }

        public FormulaInfo(int ktype, string name, string[] pars)
        {
            KType = ktype;
            Name = name;
            Parameters = pars;
        }

        public FormulaInfo Copy()
        {
            FormulaInfo fi = new FormulaInfo(this.KType, this.Name, this.Parameters);
            return fi;
        }
    }

    public class BuyPointInfo
    {
        public int Index { get; set; }

        public List<int> Filters { get; set; }

        public BuyPointInfo(int index, List<int> filters)
        {
            Index = index;
            Filters = filters;
        }

    }

    public class TAInfo
    {
        #region static
        public static int PreNTicks = 100;
        public static int PreNBars = 100;

        public static List<int> Periods;

        public static void CreateStaticMembers(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("PreNBars")) PreNBars = Convert.ToInt32(parameters["PreNBars"]);
            if (parameters.ContainsKey("PreNTicks")) PreNTicks = Convert.ToInt32(parameters["PreNTicks"]);

            Periods = new List<int>();
            if (parameters.ContainsKey("Periods"))
            {
                string[] ps = parameters["Periods"].Split(',');
                foreach (string p in ps)
                {
                    if (p == "0") continue;     // 为分时周期
                    Periods.Add(Convert.ToInt32(p));
                }
            }
        }

        #endregion


        public List<FormulaInfo> FList { get; set; }
        public List<BuyPointInfo> BuyPoints { get; set; }

        public TAInfo(Dictionary<string, string> parameters)
        {
            CreateStaticMembers(parameters);
            // Parse parmeters
            Parse(parameters);
        }

        private void Parse(Dictionary<string, string> pars)
        {
            // Get Flist
            int ktype = 0;
            string fname = null;
            string[] fpars = null;

            FList = new List<FormulaInfo>();
            if (!pars.ContainsKey("FList")) return;
            foreach (string str in pars["FList"].Split(','))
            {
                ktype = ParseFormula(str.Trim(), ref fname, ref fpars);
                FList.Add(new FormulaInfo(ktype, fname, fpars));
            }

            BuyPoints = new List<BuyPointInfo>();
            if (!pars.ContainsKey("BPs")) return;
            foreach (string bpStr in pars["BPs"].Split(','))
            {
                string[] ss = bpStr.Split('|');

                List<int> filters = new List<int>();
                if (ss.Length > 1)
                {
                    string[] afs = ss[1].Split(' ');
                    foreach (string af in afs)
                    {
                        filters.Add(Convert.ToInt32(af) - 1);
                    }
                }
                BuyPoints.Add(new BuyPointInfo(Convert.ToInt32(ss[0]) - 1, filters));
            }
        }

        private int ParseFormula(string str, ref string name, ref string[] pars)
        {
            string[] ss = str.Split('|');

            string[] fs = ss[1].Split('(', ')');

            name = fs[0];

            if (fs.Length > 1)
                pars = fs[1].Split(' ');
            else
                pars = null;

            return Convert.ToInt32(ss[0]);
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class BuyPointInfo
    {
        public FormulaInfo BuyFormula { get; set; }

        public List<FormulaInfo> Filters { get; set; }

        public BuyPointInfo(FormulaInfo buyF, List<FormulaInfo> filters)
        {
            BuyFormula = buyF;
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


        private const int maxBPNum = 5;

        public string DLLName { get; set; }


        public List<FormulaInfo> AList { get; set; }
        public List<FormulaInfo> BList { get; set; }
        public List<BuyPointInfo> BuyPoints { get; set; }

        public TAInfo(string dllName, Dictionary<string, string> parameters)
        {
            this.DLLName = dllName;
            // Parse parmeters
            Parse(parameters);
        }

        private void Parse(Dictionary<string, string> pars)
        {

            // Get AList and BList
            int ktype = 0;
            string fname = null;
            string[] fpars = null;

            AList = new List<FormulaInfo>();
            if (!pars.ContainsKey("AList")) return;
            foreach (string str in pars["AList"].Split(','))
            {
                ktype = ParseFormula(str.Trim(), ref fname, ref fpars);
                AList.Add(new FormulaInfo(ktype, fname, fpars));
            }

            BList = new List<FormulaInfo>();
            if (!pars.ContainsKey("BList")) return;
            foreach (string str in pars["BList"].Split(','))
            {
                ktype = ParseFormula(str.Trim(), ref fname, ref fpars);
                BList.Add(new FormulaInfo(ktype, fname, fpars));
            }

            ParseBuyPoints(pars);
        }

        private void ParseBuyPoints(Dictionary<string, string> pars)
        {
            // Get BuyPoints
            BuyPoints = new List<BuyPointInfo>();
            for (int i = 1; i <= maxBPNum; i++)
            {
                string key = string.Format("B{0}", i);
                if (!pars.ContainsKey(key)) break;

                string[] ss = pars[key].Split('|');
                FormulaInfo bf = BList[Convert.ToInt32(ss[0]) - 1];

                List<FormulaInfo> filters = new List<FormulaInfo>();
                if (ss.Length > 1)
                {
                    string[] afs = ss[1].Split(' ');
                    foreach (string af in afs)
                    {
                        filters.Add(AList[Convert.ToInt32(af) - 1]);
                    }
                }


                BuyPoints.Add(new BuyPointInfo(bf, filters));
            }

        }
        private int ParseFormula(string str, ref string name, ref string[] pars)
        {
            string[] ss = str.Split('|');

            string[] fs = ss[1].Split('<', '>');

            name = fs[0];

            if (fs.Length > 1)
                pars = fs[1].Split(' ');
            else
                pars = null;

            return Convert.ToInt32(ss[0]);
        }

        
    }
}

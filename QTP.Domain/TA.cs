using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using GMSDK;
using QTP.TAlib;

namespace QTP.Domain
{
    public class TA
    {
        #region Members and Properties

        // construct
        private Monitor monitor;
        private TAInfo taInfo;
        private bool enabled;

        // store of KLines of each ktype
        private Dictionary<int, RList<KLine>> dictKS;

        #endregion

        #region Properties
        public bool Enabled 
        {
            get { return enabled; }
            set { enabled = value; }
        }

        // TA for ticks of this monitor
        public TickTA TickTA { get; set; }

        #endregion

        #region Public Methods

        public TA(Monitor monitor, TAInfo taInfo)
        {
            this.monitor = monitor;
            this.taInfo = taInfo;

            this.TickTA = new TickTA(monitor);

            // Create KLines storage
            dictKS = new Dictionary<int, RList<KLine>>() 
            { 
                { 0, new RList<KLine>() }       // 缺省有日线
            };                              
            foreach (int p in TAInfo.Periods)
            {
                dictKS[p] = new RList<KLine>();
            }

            // create Formula in taInfo's List
            Assembly assembly = Assembly.LoadFrom(taInfo.DLLName + ".DLL");
            CreateFormula(assembly, taInfo.AList);
            CreateFormula(assembly, taInfo.BList);          
        }


        // Counts
        public int GetCountKLine(int ktype)
        {
            if (dictKS.ContainsKey(ktype))
            {
                return dictKS[ktype].Count;
            }
            return 0;
        }

        public int GetCountTick()
        {
            return TickTA.Count;
        }

        public void PushKLine(int ktype, KLine k)
        {
            if (!dictKS.ContainsKey(ktype))
                return;

            // Add to rlist of this ktype
            dictKS[ktype].Add(k);

            // Push To AList and BList
            foreach (FormulaInfo fi in taInfo.AList)
                if (fi.KType == ktype) fi.Formula.Push(dictKS[ktype]);

            foreach (FormulaInfo fi in taInfo.BList)
                if (fi.KType == ktype) fi.Formula.Push(dictKS[ktype]);

            if (enabled)
            {
                 CheckBuyPoints(taInfo.BuyPoints);
            }
        }

        public void PushTick(Tick tick)
        {
            TickTA.Push(tick);
        }

        public void Clear()
        {
            // Clear ticks.
            TickTA.Clear();

            // Clear 分时K线。保留日线(KType=0)
            foreach (int ktype in TAInfo.Periods)
            {
                dictKS[ktype].Clear();
            }

            // Clear AList and BList of TAFormula
            foreach (FormulaInfo fi in taInfo.AList)
            {
                fi.Formula.Clear();
            }

            foreach (FormulaInfo fi in taInfo.BList)
            {
                fi.Formula.Clear();
            }
        }

        #endregion

        #region public quotas
        // Quotas
        public Dictionary<string, List<string>> GetQuotaNames(int ktype)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string,List<string>>();
            foreach (FormulaInfo fi in taInfo.AList)
                if (ktype == fi.KType)
                    dict.Add(fi.Formula.Name, fi.Formula.ScalarNames);
            
            foreach (FormulaInfo fi in taInfo.BList)
                if (ktype == fi.KType)
                    dict.Add(fi.Formula.Name, fi.Formula.ScalarNames);

            return dict;
        }

        public List<double> GetLatestScalarValues(int ktype)
        {
            List<double> ret = new List<double>();
            foreach (FormulaInfo fi in taInfo.AList)
                if (ktype == fi.KType)
                    ret.AddRange(fi.Formula.GetLatestScalarValues());

            foreach (FormulaInfo fi in taInfo.BList)
                if (ktype == fi.KType)
                    ret.AddRange(fi.Formula.GetLatestScalarValues());

            return ret;
        }

        public RList<KLine> GetKLines(int ktype)
        {
            if (dictKS.ContainsKey(ktype))
                return dictKS[ktype];
            else
                return new RList<KLine>();
        }

        public List<RList<double>> GetScalarValues(string name, int ktype)
        {
            List<RList<double>> ret = new List<RList<double>>();

            foreach (FormulaInfo fi in taInfo.AList)
                if (ktype == fi.KType)
                {
                    foreach (RList<double> rl in fi.Formula.GetScalarValues())
                        ret.Add(rl);
                }

            foreach (FormulaInfo fi in taInfo.BList)
                if (ktype == fi.KType)
                    ret.AddRange(fi.Formula.GetScalarValues());

            return ret;
        }

        #endregion

        #region private

        private void CreateFormula(Assembly assembly, List<FormulaInfo> lst)
        {
            foreach (FormulaInfo fi in lst)
            {
                Type type = assembly.GetType(string.Format("{0}.{1}_Formula", taInfo.DLLName, fi.Name));

                fi.Formula = (TAFormula)Activator.CreateInstance(type, fi.Parameters);
            }

        }

        private void CheckBuyPoints(List<BuyPointInfo> buyPoints)
        {
            foreach (BuyPointInfo bpi in buyPoints)
            {

            }
        }
        #endregion
    }

}

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
        private bool enabled;

        // store of KLines of each ktype
        private Dictionary<int, RList<KLine>> dictKS;

        private List<FormulaInfo> FList;

        private List<BuyPointInfo> BPList;

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

        public TA(Monitor monitor, TAInfo taInfo, string dllName)
        {
            this.monitor = monitor;

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
            Assembly assembly = Assembly.LoadFrom(dllName + ".DLL");
            FList = new List<FormulaInfo>();
            foreach (FormulaInfo fi in taInfo.FList)
            {
                FormulaInfo fiClone = fi.Copy();

                Type type = assembly.GetType(string.Format("{0}.{1}_Formula", dllName, fi.Name));
                fiClone.Formula = (TAFormula)Activator.CreateInstance(type, fi.Parameters);

                FList.Add(fiClone);
            }

            BPList = taInfo.BuyPoints;
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
            foreach (FormulaInfo fi in FList)
                if (fi.KType == ktype) 
                    fi.Formula.Push(dictKS[ktype]);

            if (enabled)
            {
                 CheckBuyPoints(BPList, ktype, k);
            }
        }

        public void PushTick(Tick tick)
        {
            TickTA.Push(tick, enabled);
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

            // Clear FList of TAFormula
            foreach (FormulaInfo fi in FList)
            {
                // 保留日线(KType=0)
                if (fi.KType != 0) fi.Formula.Clear();
            }
        }

        public double GetATR(int ktype)
        {
            foreach (FormulaInfo fi in FList)
            {
                if (fi.Formula.Name == "真实波幅" && fi.KType == ktype)
                {
                    return fi.Formula.GetStopLessRisk();
                }
            }

            return 0.0;
        }

        #endregion

        #region public quotas
        // Quotas
        public Dictionary<string, List<string>> GetQuotaNames(int ktype)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string,List<string>>();
            foreach (FormulaInfo fi in FList)
                if (ktype == fi.KType)
                    dict.Add(fi.Formula.Name, fi.Formula.ScalarNames);
            
            return dict;
        }

        public List<double> GetLatestScalarValues(int ktype)
        {
            List<double> ret = new List<double>();
            foreach (FormulaInfo fi in FList)
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

            foreach (FormulaInfo fi in FList)
                if (name == fi.Formula.Name && ktype == fi.KType)
                     ret.AddRange(fi.Formula.GetScalarValues());

            return ret;
        }

        #endregion

        #region private

        private void CheckBuyPoints(List<BuyPointInfo> buyPoints, int ktype, KLine k)
        {
            foreach (BuyPointInfo bpi in buyPoints)
            {
                FormulaInfo fi = FList[bpi.Index];
                if (fi.KType != ktype) continue;

                if (CheckFilters(bpi.Filters))
                {
                    if (fi.Formula.IsBuyPoint())
                    {
                        monitor.IssueOpenLongSignal(fi.Name, fi.KType, k);
                    }
                }
            }
        }

        private bool CheckFilters(List<int> filters)
        {
            foreach (int i in filters)
            {
                if (FList[i].Formula.Filter() == false)
                    return false;
            }
            return true;
        }
        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.DBAccess;
namespace QTP.Domain
{
    public class PoolManager
    {
        #region protected members

        protected MyStrategy strategy;
        protected string name;

        protected Monitor monitor;
        private string symbolBench;

        #endregion

        public string Name
        {
            get { return name; }
        }

        public string SymbolBench
        {
            get { return symbolBench; }
        }

        public Monitor Monitor
        {
            get { return monitor; }
            set { monitor = value; }
        }

        public PoolManager(MyStrategy strategy, TPool pool)
        {
            this.strategy = strategy;
            this.name = pool.Name;

            symbolBench = pool.ManagerParameters["Bench"];
        }

    }
}

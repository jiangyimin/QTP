using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.TAlib;
namespace QTP.Domain
{

    public abstract class TAFormula : ITAData, ITATrade
    {
        protected string name;
        protected List<string> scalarNames;

        #region implement ITAData
        public string Name { get { return name; } }

        public List<string> ScalarNames { get { return scalarNames; } }

        public abstract List<double> GetLatestScalarValues();

        public abstract List<RList<double>> GetScalarValues();


        public abstract void Push(RList<KLine> rl);

        public abstract void Clear();

        #endregion


        #region implement ITATrade

        public virtual bool Filter()
        {
            return false;
        }

        public virtual bool IsBuyPoint()
        {
            return false;
        }

        public virtual double GetStopLessRisk()
        {
            return 0.0;
        }

        #endregion
    }

    public interface ITAData
    {
        string Name { get; }

        List<string> ScalarNames { get; }

        List<double> GetLatestScalarValues();
        List<RList<double>> GetScalarValues();
        
        void Push(RList<KLine> rl);

        void Clear();
    }

    public interface ITATrade
    {
        bool Filter();

        bool IsBuyPoint();

        double GetStopLessRisk();
    }
}

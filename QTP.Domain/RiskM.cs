using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMSDK;
namespace QTP.Domain
{
    public class RiskM
    {
        #region Members

        // 对应的策略
        protected MyStrategy strategy;

        // Important parameter
        private const double maxRisk = 2.0;
        private const double minRisk = 1.0;
        private const int maxCSize = 100;

        protected double currentRisk;
        protected int currentCSize; 
        protected Cash lastCash;
        protected DateTime lastCashArriveTime;

        // risk Positions
        protected List<RiskPosition> riskPositions = new List<RiskPosition>();
        // risk orders of today
        protected List<RiskOrder> rejectOrders = new List<RiskOrder>();
        protected List<RiskOrder> acceptOrders = new List<RiskOrder>();

        #endregion

        #region Property

        public int CountRiskPositions
        {
            get { return riskPositions.Count; }
        }
        public RiskM(MyStrategy s, Dictionary<string, string> pars)
        {
            strategy = s;

            currentCSize = Convert.ToInt32(pars["CSize"]);
            currentRisk = Convert.ToDouble(pars["Risk"]);
        }

        public void SetLastCash(Cash cash)
        {
            lastCash = cash;
            lastCashArriveTime = DateTime.Now;
        }

        #endregion

        #region virtual or abstract methods

        public virtual void Initialize()
        {
            // Get Positions 
            List<Position> positions = null;
            if (strategy.WebTD != null)
            {
                positions = strategy.WebTD.GetPositionsWeb();
            }
            else
                positions = strategy.GetPositions();

            if (positions == null || positions.Count == 0)
            {
                strategy.WriteTDLog("RiskM 没有持仓，请确认!");
                return;
            }

            // process positions
            int i = 1;
            foreach (Position pos in positions)
            {
                Monitor monitor = strategy.GetMonitor(string.Format("{0}.{1}", pos.exchange, pos.sec_id));
                if (monitor != null)
                {
                    RiskPosition riskPos = new RiskPosition(pos.available, pos.vwap);
                    riskPos.VolumeAux = monitor.Target.Volume;
                    riskPos.StopLossPrice = monitor.Target.StopLossPrice;
                    riskPositions.Add(riskPos);

                    monitor.SetPosition(riskPos);
                    strategy.WriteTDLog(string.Format("({0}) {1}的可用持仓为({2})", i++, monitor.TargetTitle, riskPos.Volume));
                }
                else
                {
                    strategy.WriteTDLog(string.Format("账户{0}的持仓没有设置对应的监控器!", monitor.TargetTitle));
                }
            }
        }


        public virtual double GetVolume(string exchange, string sec_id)
        {
            return 1000.0;
        }

        #endregion
    }
}

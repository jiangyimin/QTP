﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.Infra;
using GMSDK;
namespace QTP.Domain
{
    public abstract class RiskM
    {
        #region Members

        protected MyStrategy strategy;

        protected Cash cash;
        protected List<Position> positions;

        #endregion

        public void SetStrategy(MyStrategy s)
        {
            strategy = s;
        }

        public virtual void Initialize()
        {
            // get cash and positions
            cash = strategy.GetCash();

            if (cash == null)
            {
                strategy.WriteTDLog("不能打开账户");
                return;
            }

            //strategy.WriteInfo(string.Format("账户总资产({0:N2}), 可用资金({1:N2})", cash.nav, cash.available));

            string info = "持仓:";
            positions = strategy.GetPositions();
            foreach (Position pos in positions)
            {               
                info += string.Format("[{0}:{1}({2})]",pos.sec_id, pos.volume, pos.side == 1 ? "多" : "空");

                Monitor monitor = strategy.GetMonitor(string.Format("{0}.{1}", pos.exchange, pos.sec_id));
                if (monitor != null)
                {
                    monitor.OnPosition(pos);
                }
            }
        }


        public abstract double GetVolume(string exchange, string sec_id);

    }
}

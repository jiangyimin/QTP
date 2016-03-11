using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.Domain;

namespace QTP.Console
{
    public partial class RiskTradeUC : UserControl, IStrategyUC
    {
        private MyStrategy strategy;

        public RiskTradeUC()
        {
            InitializeComponent();
        }

        #region IStrategyUI
        public MyStrategy Subject
        {
            set 
            {
                strategy = value;

                strategy.MDLog += MDLogHandler;
                strategy.TDLog += TDLogHandler;
            }
        }

        public void ShowData()
        {

        }

        public void TimerRefresh()
        { }

        #endregion

        #region event handers
        public void MDLogHandler(string msg)
        {
            if (boxMDLog.InvokeRequired == false)
            {
                boxMDLog.Items.Add(msg);
                boxMDLog.SetSelected(boxMDLog.Items.Count - 1, true);

            }
            else
            {
                MyStrategy.LogCallback handle = new MyStrategy.LogCallback(MDLogHandler);
                boxMDLog.BeginInvoke(handle, msg);
            }
        }
        public void TDLogHandler(string msg)
        {
            if (boxTDLog.InvokeRequired == false)
            {
                boxTDLog.Items.Add(msg);
                boxTDLog.SetSelected(boxTDLog.Items.Count - 1, true);

            }
            else
            {
                MyStrategy.LogCallback handle = new MyStrategy.LogCallback(TDLogHandler);
                boxTDLog.BeginInvoke(handle, msg);
            }
        }

        #endregion
    }
}

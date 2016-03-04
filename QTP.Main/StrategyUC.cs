using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.DBAccess;

namespace QTP.Main
{
    public partial class StrategyUC : UserControl
    {
        private Control parent;         // parent Container
        private TStrategy strategy;

        private ExeRunUC exeUC;


        public StrategyUC(TStrategy strategy, Control parent)
        {
            InitializeComponent();

            this.parent = parent;
            this.strategy = strategy;

            // Display information
            lblName.Text = strategy.Name;

            // groupMonitor
            lblClassName1.Text = string.Format("监控: {0}", strategy.MonitorClassName);
            lblParameter1.Text = string.Format("参数: {0}", strategy.MonitorParemeters);
            lblNum.Text = string.Format("监控数量:{0}", strategy.Instruments.Count);

            // groupRiskM
            lblClassName2.Text = string.Format("资管: {0}", strategy.RiskMClassName);
            lblParameter2.Text = string.Format("参数: {0}", strategy.MonitorParemeters);
            lblTradeChannel.Text = string.Format("交易通道:{0}", strategy.TradeChannelName);
        }

        public void Close()
        {
            if (exeUC != null)
            {
                exeUC.Close();
                parent.Controls.Remove(exeUC);
                exeUC = null;
            }
        }

        private void Open()
        {
            exeUC = new ExeRunUC();
            exeUC.Dock = DockStyle.Fill;

            string args = string.Format("{0} {1} {2}", strategy.Id, Global.Login.UserName, Global.Login.Password);
            exeUC.Open(Global.ExePath, Global.ExeName, args);

            parent.Controls.Add(exeUC);
            exeUC.BringToFront();
        }

        #region Actions

        private void btnDetail_Click(object sender, EventArgs e)
        {
            exeUC.BringToFront();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;

            try
            {
                if (btnOpen.Text == "打开")
                {
                    Open();

                    // if success, then enable detail.
                    panel1.BackColor = Color.Orchid;
                    btnOpen.Text = "关闭";
                    btnDetail.Enabled = true;
                }
                else  // close this strategy
                {
                    Close();
                    // then disable detail
                    panel1.BackColor = SystemColors.GradientActiveCaption;
                    btnOpen.Text = "打开";
                    btnDetail.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            btnOpen.Enabled = true;
        }


        #endregion

    }
}

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

namespace QTP.Main
{
    public partial class StrategyUC : UserControl
    {
        private StrategyQTP qtp;
        private StrategyQTP.BringRunUCDelegate bringRunUC;

        public StrategyUC()
        {
            InitializeComponent();
        }

        public void SetStrategy(StrategyQTP qtp, StrategyQTP.BringRunUCDelegate bringRunUC)
        {
            this.qtp = qtp;
            this.bringRunUC = bringRunUC;

            // panel Title
            lblName.Text = qtp.StrategyT.Name;

            // panel Status
            UpdateStatus();

            // panel trade
            lblMonitors.Text = string.Format("品种数:{0}", qtp.StrategyT.Instruments.Count);
            lblTradeChannel.Text = string.Format("通道:{0}", qtp.TradeChannel);

        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAction.Text == "打开")
                    qtp.Open();
                else if (btnAction.Text == "启动")
                    qtp.Start();
                else if (btnAction.Text == "停止")
                    qtp.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            UpdateStatus();

        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            if (qtp.Status == "未打开")
            {
                MessageBox.Show("请先打开策略!");
                return;
            }

            // this is a delegate, call mainForm's CreateOrBringRunUCToFront.
            bringRunUC(qtp);
        }

        private void StrategyUC_DoubleClick(object sender, EventArgs e)
        {
            btnDetail_Click(this, null);
        }

        private void UpdateStatus()
        {
            lblStatus.Text = qtp.Status;

            if (qtp.Status == "未打开")
            {
                btnAction.Text = "打开";
                btnDetail.Enabled = false;
            }
            else if (qtp.Status == "已打开")
            {
                btnAction.Text = "启动";
                btnDetail.Enabled = true;
            }
            else if (qtp.Status == "运行中")
            {
                btnAction.Text = "停止";
                btnDetail.Enabled = true;
            }
        }

    }
}

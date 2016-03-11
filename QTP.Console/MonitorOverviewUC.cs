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
using QTP.Domain;

namespace QTP.Console
{
    public partial class MonitorOverviewUC : UserControl, IStrategyUC
    {
        /// <summary>
        ///  three FlowLayoutPanel
        /// </summary>
        private FlowLayoutPanel[] monitorNavs = new FlowLayoutPanel[3];
        private FlowLayoutPanel panelFocus;


        private MyStrategy strategy;

        public MonitorOverviewUC()
        {
            InitializeComponent();

            for (int i = 0; i < 3; i++)
            {
                FlowLayoutPanel panel = new FlowLayoutPanel();
                
                panel.Dock = DockStyle.Fill;
                this.panelClient.Controls.Add(panel);
                monitorNavs[i] = panel;
            }
        }

        #region IStrategyUC
        public MyStrategy Subject
        {
            set 
            {
                strategy = value;

                // now only process Normal
                foreach (Monitor monitor in strategy.GetMonitorEnumerator())
                {
                    MonitorUC uc = new MonitorUC(monitor);
                    monitorNavs[monitor.Category].Controls.Add(uc);
                    
                }
            }
        }

        public void ShowData()
        {
            // default is Observe Nav
            btnObserve_Click(this, null);
        }

        public void TimerRefresh()
        {
            foreach (Control uc in panelFocus.Controls)
            {
                if (uc is MonitorUC)
                {
                    ((MonitorUC)uc).Display();
                }
            }
        }

        #endregion

        #region events handlers


        private void btnObserve_Click(object sender, EventArgs e)
        {
            panelFocus = monitorNavs[0];

            lblTitle.Text = "观察期监控器";
            lblRightTitle.Text = string.Format("数量：{0}", panelFocus.Controls.Count);

            panelFocus.BringToFront();
        }

        private void btnCandidate_Click(object sender, EventArgs e)
        {
            panelFocus = monitorNavs[1];

            lblTitle.Text = "候选期监控器";
            lblRightTitle.Text = string.Format("数量：{0}", panelFocus.Controls.Count);

            panelFocus.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelFocus = monitorNavs[2];

            lblTitle.Text = "持仓期监控器";
            lblRightTitle.Text = string.Format("数量：{0}", panelFocus.Controls.Count);

            panelFocus.BringToFront();
        }

        #endregion
    }
}

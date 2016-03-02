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
        private Control parent;         // parent Container

        private MyStrategy subject;


        public StrategyUC(Control parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        public MyStrategy Subject
        {
            get { return subject; }
            set 
            { 
                subject = value; 
                subject.MessageHint += MessageHintHandler;
                Subject.StatusChanged += status_Changed;
            }
        }

        private void StrategyUC_Load(object sender, EventArgs e)
        {
            // panel Title
            lblName.Text = subject.StrategyT.Name;

            // panel trade
            lblMonitors.Text = string.Format("监控数量:{0}", subject.StrategyT.Instruments.Count);
            lblTradeChannel.Text = string.Format("交易通道:{0}", subject.StrategyT.TradeChannelName);

        }

        #region Actions

        private void btnDetail_Click(object sender, EventArgs e)
        {
            //if (runUC == null)
            //{
            //    runUC = new StrategyRunUC();                
            //    runUC.SetStrategy(subject);
            //    runUC.Dock = DockStyle.Fill;
            //    parent.Controls.Add(runUC);
            //}

            //runUC.BringToFront();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            if (btnConnect.Text == "连接")
            {
                try
                {
                    subject.Connect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                subject.Stop();
            }
            btnConnect.Enabled = true;
        }

        private void MessageHintHandler(string msg)
        {
            if (this.listException.InvokeRequired == false)
            {
                this.listException.Items.Add(msg);
                listException.SetSelected(listException.Items.Count - 1, true);     // set last line to visible
            }
            else
            {
                MyStrategy.MessageHintCallback handler = new MyStrategy.MessageHintCallback(MessageHintHandler);
                this.listException.BeginInvoke(handler, msg);
            }
        }

        private void status_Changed(bool running)
        {
            if (this.InvokeRequired == false)
            {
                if (running)
                {
                    btnDetail.Enabled = true;
                    btnConnect.Text = "断开";
                }
                else
                {
                    btnDetail.Enabled = false;
                    btnConnect.Text = "连接";
                }
            }
            else
            {
                MyStrategy.StrategyStatusChangedCallback handler = new MyStrategy.StrategyStatusChangedCallback(status_Changed);
                this.BeginInvoke(handler, running);
            }
        }
        #endregion

    }
}

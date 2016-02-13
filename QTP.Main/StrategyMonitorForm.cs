using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.Domain;

namespace QTP.Main
{
    public partial class StrategyMonitorForm : Form
    {
        private StrategyQTP qtp;
        private int strategyId;
        public StrategyMonitorForm()
        {
            InitializeComponent();
        }

        public void SetStrategy(StrategyQTP s, int id)
        {
            qtp = s;
            strategyId = id;
            s.OnMessage += OnMessage;
            Task t = new Task(s.Start);
            t.Start();
        }

        private void OnMessage(string msg)
        {
            if (this.listBox1.InvokeRequired == false)
            {
                listBox1.Items.Add(msg);
            }
            else
            {
                DispMSGDelegate DMSGD = new DispMSGDelegate(OnMessage);
                this.listBox1.Invoke(DMSGD, msg);
            }
        }

        private delegate void DispMSGDelegate(string msg);

        private void StrategyMonitorForm_Load(object sender, EventArgs e)
        {

        }

        private void StrategyMonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            qtp.Stop();
            Global.RunningStrategies.Remove(strategyId);
        }
    }
  }

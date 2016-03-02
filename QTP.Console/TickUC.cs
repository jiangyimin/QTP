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
using GMSDK;

namespace QTP.Console
{
    public partial class TickUC : UserControl
    {
        private MyStrategy strategy;
        private string symbol;

        public TickUC()
        {
            InitializeComponent();
        }

        public void ShowTick(Tick tick)
        {
            if (this.InvokeRequired == false)
            {
                lblAsk1.Text = string.Format("卖一\t    {0:0.00}\t    {1}", tick.ask_p1, tick.ask_v1);
            }
            else
            {
                MyStrategy.FocusTickHandlerDelegate handler = new MyStrategy.FocusTickHandlerDelegate(this.ShowTick);
                this.BeginInvoke(handler, tick);
            }
        }
    }
}

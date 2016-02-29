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
    public partial class TicksControl : UserControl
    {
        private StrategyQTP strategy;
        private string symbol;

        public TicksControl()
        {
            InitializeComponent();
        }

        public void ShowTick(Tick tick)
        {
            lblAsk1.Text = string.Format("卖一\t{0:0.00}\t{1}", tick.ask_p1, tick.ask_v1);
        }
    }
}

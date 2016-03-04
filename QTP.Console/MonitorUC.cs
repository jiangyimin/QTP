using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMSDK;
using QTP.Domain;

namespace QTP.Console
{
    public partial class MonitorUC : UserControl
    {
        private Monitor mon;
        public MonitorUC(Monitor mon)
        {
            InitializeComponent();

            this.mon = mon;
            lblTarge.Text = mon.Target.InstrumentId + mon.GMInstrument.sec_name;
        }

        public void Display()
        {
            Tick tick = mon.LatestTick;

            if (tick != null) lblPrice.Text = string.Format("{0:.00}", tick.last_price);

            lblTick.Text = String.Format("T: {0}", mon.NumTicks);
            lblBar1m.Text = String.Format("1B: {0}", mon.Num1mBars);

        }
    }
}

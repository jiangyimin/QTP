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
        }

        public void Display()
        {
            if (mon.GMInstrument != null)
                lblTarge.Text = mon.Target.InstrumentId + mon.GMInstrument.sec_name;
            else
                lblTarge.Text = mon.Target.InstrumentId;

            TickTA tickTA = mon.TickTA;
            if (tickTA.LastestTickQuota != null)
                lblPrice.Text = string.Format("{0:.00}", tickTA.LastestTickQuota.Tick.last_price);

            lblTick.Text = String.Format("T: {0}", tickTA.Count);
            // lblBar1m.Text = String.Format("B1M: {0}", mon.Bar1MsCount);

        }
    }
}

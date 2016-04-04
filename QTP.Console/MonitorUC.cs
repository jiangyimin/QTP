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
            lblTarge.Text = mon.TargetTitle;

            TickTA tickTA = mon.TA.TickTA;
            if (tickTA.LastestTickQuota != null)
                lblPrice.Text = string.Format("{0:.00}", tickTA.LastestTickQuota.Tick.last_price);

            lblTick.Text = String.Format("T: {0}", tickTA.Count);

        }
    }
}

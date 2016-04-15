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
    public partial class BarUC : UserControl
    {
        public BarUC()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            boxBars.Items.Clear();
            boxOrders.Items.Clear();
        }

        public void OnBarArrived(Bar bar)
        {
            if (boxBars.InvokeRequired == false)
            {
                if (boxBars.Items.Count > 320) 
                    boxBars.Items.Clear();

                string msg = string.Format("{0} {1}M", Utils.DTLongString(Utils.UtcToDateTime(bar.utc_time)), bar.bar_type / 60);
                boxBars.Items.Add(msg);
                boxBars.SetSelected(boxBars.Items.Count - 1, true);

            }
            else
            {
                MyStrategy.FocusBarArrivedCallback handle = new MyStrategy.FocusBarArrivedCallback(OnBarArrived);
                boxBars.BeginInvoke(handle, bar);
            }
        }
    }
}

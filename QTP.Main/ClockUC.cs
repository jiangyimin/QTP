using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTP.Main
{
    public partial class ClockUC : UserControl
    {
        public ClockUC()
        {
            InitializeComponent();

            timerClock.Tick += timerClock_Tick;
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;

            lblDate.Text = string.Format("{0}月{1}日", dt.Month, dt.Day);

            lblMH.Text = string.Format("{0}:{1}", dt.Hour, dt.Minute);

            lblSecond.Text = string.Format("{0}秒", dt.Second);
        }

        public void Start()
        {
            timerClock.Start();
        }

    }
}

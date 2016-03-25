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
using QTP.Infra;

namespace QTP.Console
{
    public partial class BarUC : UserControl
    {
        public BarUC()
        {
            InitializeComponent();
        }

        public void OnBarArrived(Bar bar, Bar tickBar1M)
        {
            if (this.InvokeRequired == false)
            {
                // Show DateTime
                DateTime dt = DateTime.Now;
                lblArrivedTime.Text = string.Format("到达：{0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);
                dt = Utils.UtcToDateTime(bar.utc_time);
                lblBarTime.Text = string.Format("Bar时间 {0:00}:{1:00}:{2:00}", dt.Hour, dt.Minute, dt.Second);

                // Bar info
                groupBox1.Text = string.Format("Bar({0}M）", bar.bar_type / 60);
                lblHigh.Text = string.Format("高:{0:0.00}", bar.high);
                lblLow.Text = string.Format("低:{0:0.00}", bar.high);
                lblOpen.Text = string.Format("开:{0:0.00}", bar.open);
                lblClose.Text = string.Format("收:{0:0.00}", bar.close);
                lblVolumn.Text = string.Format("量:{0:0}", bar.volume / 100);
                lblFactor.Text = string.Format("因子:{0:0.00}", bar.adj_factor);

                // TickBar info
                if (tickBar1M == null) return;
                lblHighT.Text = string.Format("高:{0:0.00}", tickBar1M.high);
                lblLowT.Text = string.Format("低:{0:0.00}", tickBar1M.high);
                lblOpenT.Text = string.Format("开:{0:0.00}", tickBar1M.open);
                lblCloseT.Text = string.Format("收:{0:0.00}", tickBar1M.close);
                lblVolumnT.Text = string.Format("量:{0:0}", tickBar1M.volume / 100);
                lblStrTime.Text = string.Format("时间:{0:0.00}", tickBar1M.strtime);

            }
            else
            {
                MyStrategy.FocusBarArrivedCallback handler = new MyStrategy.FocusBarArrivedCallback(this.OnBarArrived);
                this.BeginInvoke(handler, bar, tickBar1M);
            }
        }
    }
}

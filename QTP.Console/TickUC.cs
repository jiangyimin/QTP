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
        //private MyStrategy strategy;
        //private string symbol;

        public TickUC()
        {
            InitializeComponent();
        }

        public void OnTickArrived(TickTA tickTA)
        {
            if (this.InvokeRequired == false)
            {
                // Tick Time
                DateTime dt = DateTime.Now;

                TickQuota lastestTQ = tickTA.LastestTickQuota;
                if (lastestTQ == null) return;
                Tick tick = lastestTQ.Tick;
                dt = Utils.UtcToDateTime(tick.utc_time);
                lblTickTime.Text = string.Format("{0:00}:{1:00}:{2:00}(时延={3:0.000} 波幅={4:0.00})", dt.Hour, dt.Minute, dt.Second, lastestTQ.Delay, lastestTQ.Range);

                TickQuota prevTQ = tickTA.PrevTickQuota;
                if (prevTQ != null)
                {
                    dt = Utils.UtcToDateTime(prevTQ.Tick.utc_time);
                    lblPrevTickTime.Text = string.Format("{0:00}:{1:00}:{2:00}(处理={3}ms 间隔={4})", dt.Hour, dt.Minute, dt.Second, tickTA.TickProcessElapsed, lastestTQ.Interval);
                }

                // ask5-ask1
                lblAsk5Price.ForeColor = GetColor(tick.ask_p5, tick.pre_close); lblAsk5Price.Text = tick.ask_p5.ToString(".00");
                lblAsk4Price.ForeColor = GetColor(tick.ask_p4, tick.pre_close); lblAsk4Price.Text = tick.ask_p4.ToString(".00");
                lblAsk3Price.ForeColor = GetColor(tick.ask_p3, tick.pre_close); lblAsk3Price.Text = tick.ask_p3.ToString(".00");
                lblAsk2Price.ForeColor = GetColor(tick.ask_p2, tick.pre_close); lblAsk2Price.Text = tick.ask_p2.ToString(".00");
                lblAsk1Price.ForeColor = GetColor(tick.ask_p1, tick.pre_close); lblAsk1Price.Text = tick.ask_p1.ToString(".00");

                lblAsk5Volumn.Text = (tick.ask_v5 / 100).ToString();
                lblAsk4Volumn.Text = (tick.ask_v4 / 100).ToString();
                lblAsk3Volumn.Text = (tick.ask_v3 / 100).ToString();
                lblAsk2Volumn.Text = (tick.ask_v2 / 100).ToString();
                lblAsk1Volumn.Text = (tick.ask_v1 / 100).ToString();

                // bid1-bid5
                lblBid1Price.ForeColor = GetColor(tick.bid_p1, tick.pre_close); lblBid1Price.Text = tick.bid_p1.ToString(".00");
                lblBid2Price.ForeColor = GetColor(tick.bid_p2, tick.pre_close); lblBid5Price.Text = tick.bid_p2.ToString(".00");
                lblBid3Price.ForeColor = GetColor(tick.bid_p3, tick.pre_close); lblBid4Price.Text = tick.bid_p3.ToString(".00");
                lblBid4Price.ForeColor = GetColor(tick.bid_p4, tick.pre_close); lblBid3Price.Text = tick.bid_p4.ToString(".00");
                lblBid5Price.ForeColor = GetColor(tick.bid_p5, tick.pre_close); lblBid2Price.Text = tick.bid_p5.ToString(".00");

                lblBid5Volumn.Text = (tick.bid_v5 / 100).ToString();
                lblBid4Volumn.Text = (tick.bid_v4 / 100).ToString();
                lblBid3Volumn.Text = (tick.bid_v3 / 100).ToString();
                lblBid2Volumn.Text = (tick.bid_v2 / 100).ToString();
                lblBid1Volumn.Text = (tick.bid_v1 / 100).ToString();

                // other
                lblLastPrice.ForeColor = GetColor(tick.last_price, tick.pre_close); lblLastPrice.Text = tick.last_price.ToString(".00");
                lblOpenPrice.ForeColor = GetColor(tick.open, tick.pre_close); lblOpenPrice.Text = tick.open.ToString(".00");
                lblHighPrice.ForeColor = GetColor(tick.high, tick.pre_close); lblHighPrice.Text = tick.high.ToString(".00");
                lblLowPrice.ForeColor = GetColor(tick.low, tick.pre_close); lblLowPrice.Text = tick.low.ToString(".00");

                lblCumVolumn.Text = (tick.cum_volume / 100).ToString();
                lblCumAmount.Text = string.Format("{0:0}", tick.cum_amount / 10000);
                lblLastVolumn.Text = (tick.last_volume / 100).ToString();
                lblLastAmount.Text = string.Format("{0:0.0}", tick.last_amount / 10000);

                // Other
            }
            else
            {
                MyStrategy.FocusTickArrivedCallback handler = new MyStrategy.FocusTickArrivedCallback(this.OnTickArrived);
                this.BeginInvoke(handler, tickTA);
            }

        }

        private Color GetColor(float price, float pre_close)
        {
            if (price > pre_close)
                return Color.Red;
            else if (price == pre_close)
                return Color.White;
            else
                return Color.ForestGreen;

        }

    }
}

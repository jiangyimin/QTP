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
using QTP.Plugin;

using GMSDK;

namespace QTP.Console
{
    public partial class AccountUC : UserControl
    {
        private MyStrategy strategy;

        public AccountUC()
        {
            InitializeComponent();
        }

        public void ShowData(MyStrategy strategy)
        {
            this.strategy = strategy;

            strategy.TradeCashHeartPusle += new MyStrategy.TradeCashHeartPusleCallback(TradeCash_HeartPusle);

            if (strategy.WebTD == null)
            {
                splitContainer1.Panel2Collapsed = true; ;
                splitContainer2.Panel2Collapsed = true; ;
            }

            ShowGMAccount();
        }


        public void ShowGMAccount()
        {
            Cash cash = strategy.GetCash();
            if (cash == null) return;

            // show GM Cash by heartpulse
            TradeCash_HeartPusle(cash, 0, false);

            // Then GMPositions
            ShowGMPositions();
        }

        public void ShowWebAccount()
        {
            Cash cash = strategy.WebTD.GetCash();
            if (cash == null) return;

            // show GM Cash by heartpulse
            TradeCash_HeartPusle(cash, 0, true);

            // Then GMPositions
            ShowWebPositions();
        }

        private void TradeCash_HeartPusle(Cash cash, long elapsed, bool isWeb)
        {
            if (this.splitContainer1.Panel1.InvokeRequired == false)
            {
                DateTime dt = DateTime.Now;
                if (isWeb)
                {
                    grpWEB.Text = string.Format("Web 账户（{0}, 用时{1})", dt.ToString("hh:mm:ss"), elapsed);
                    ShowWebCashInfo(cash);
                }
                else
                {
                    grpGM.Text = string.Format("GM 账户（{0}, 用时{1})", dt.ToString("hh:mm:ss"), elapsed);
                    ShowGMCashInfo(cash);
                }

            }
            else
            {
                MyStrategy.TradeCashHeartPusleCallback handler = new MyStrategy.TradeCashHeartPusleCallback(this.TradeCash_HeartPusle);
                this.BeginInvoke(handler, cash, elapsed, isWeb);
            }
        }



        #region private utils

        private void ShowWebCashInfo(Cash cash)
        {
            txtNavW.Text = cash.nav.ToString("N");
            txtAvailableW.Text = cash.available.ToString("N");
            txtFrozenW.Text = cash.frozen.ToString("N");
            txtOrderFrozenW.Text = cash.order_frozen.ToString("N");
        }

        private void ShowGMCashInfo(Cash cash)
        {
            txtNav.Text = cash.nav.ToString("N");
            txtAvailable.Text = cash.available.ToString("N");
            txtFrozen.Text = cash.frozen.ToString("N");
            txtOrderFrozen.Text = cash.order_frozen.ToString("N");

            txtPnl.Text = cash.pnl.ToString("N");
            txtFpnl.Text = cash.fpnl.ToString("N");
        }

        private void ShowGMPositions()
        {
            List<Position> positions = strategy.GetPositions();

            // clear first
            dgvPosition.Rows.Clear();

            int i = 1;
            foreach (Position pos in positions)
            {
                string symbol = string.Format("{0}.{1}", pos.exchange, pos.sec_id);
                string sec_name = MyStrategy.DictInstruments.ContainsKey(symbol) ? MyStrategy.DictInstruments[symbol].sec_name : null;
                string side = pos.side == (int)OrderSide.Bid ? "买/多" : "卖/空";
                string vol = string.Format("{0}/{1}", pos.volume, pos.volume_today);

                dgvPosition.Rows.Add(i++, pos.exchange, pos.sec_id, sec_name, vol,
                    pos.last_price.ToString("N"), pos.cost.ToString("N"), pos.fpnl.ToString("N"));
            }
        }

        private void ShowWebPositions()
        {
            List<Position> positions = strategy.WebTD.GetPositions();
            // clear first
            dgvPositionW.Rows.Clear();

            int i = 1;
            foreach (Position pos in positions)
            {
                string symbol = string.Format("{0}.{1}", pos.exchange, pos.sec_id);
                string sec_name = MyStrategy.DictInstruments.ContainsKey(symbol) ? MyStrategy.DictInstruments[symbol].sec_name : null;

                dgvPositionW.Rows.Add(i++, pos.exchange, pos.sec_id, sec_name, pos.volume,
                    pos.last_price.ToString("N"), pos.cost.ToString("N"), pos.fpnl.ToString("N"));
            }
        }

        #endregion

    }
}

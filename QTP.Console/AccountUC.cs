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
    public partial class AccountUC : UserControl
    {
        private MyStrategy strategy;
        public AccountUC()
        {
            InitializeComponent();
        }

        public void GMAccount(MyStrategy strategy)
        {
            this.strategy = strategy;
            btnGM_Click(this, null);
        }

        private void btnGM_Click(object sender, EventArgs e)
        {
            Cash cash = strategy.GetCash();
            if (cash == null)
            {
                return;
            }

            ShowCashInfo(cash);

            List<Position> positions = strategy.GetPositions();

            ShowPositions(positions);

        }

        #region private utils

        private void ShowCashInfo(Cash cash)
        {
            txtNav.Text = cash.nav.ToString("N");
            txtAvailable.Text = cash.available.ToString("N");
            txtFrozen.Text = cash.frozen.ToString("N");
            txtOrderFrozen.Text = cash.order_frozen.ToString("N");

            txtPnl.Text = cash.pnl.ToString("N");
            txtFpnl.Text = cash.fpnl.ToString("N");
            txtProfitRatio.Text = string.Format("{0:0.0000}%", cash.profit_ratio * 100);
        }

        private void ShowPositions(List<Position> positions)
        {
            // clear first
            dgvPosition.Rows.Clear();

            int i = 1;
            foreach (Position pos in positions)
            {
                string symbol = string.Format("{0}.{1}", pos.exchange, pos.sec_id);
                string sec_name = MyStrategy.DictInstruments.ContainsKey(symbol) ? MyStrategy.DictInstruments[symbol].sec_name : null;
                string side = pos.side == (int)OrderSide.Bid ? "买/多" : "卖/空"; 
                string vol = string.Format("{0}/{1}", pos.volume, pos.volume_today);

                dgvPosition.Rows.Add(i++, pos.exchange, pos.sec_id, sec_name, side, vol, 
                    pos.price.ToString("N"), pos.cost.ToString("N"), pos.fpnl.ToString("N"));
            }
        }

        #endregion
    }
}

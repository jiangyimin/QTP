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

namespace QTP.Console
{
    public partial class TradeUC : UserControl
    {
        private MyStrategy strategy;

        public TradeUC()
        {
            InitializeComponent();
        }

        public void ShowData(MyStrategy strategy)
        {
            this.strategy = strategy;
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            
            strategy.MyOpenLong("SZSE", txtInstrument.Text, Convert.ToDouble(txtPrice.Text), Convert.ToDouble(txtVolumn.Text));
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            strategy.MyCloseLong("SZSE", txtInstrument.Text, Convert.ToDouble(txtPrice.Text), Convert.ToDouble(txtVolumn.Text));

        }
    }
}

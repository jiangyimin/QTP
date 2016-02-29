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

namespace QTP.Main
{
    public partial class StrategyNavUC : UserControl
    {
        public StrategyNavUC()
        {
            InitializeComponent();
        }

        public string Title
        {
            set 
            {
                lblTitle.Text = value;
                lblRightTitle.Text = value;
            }
        }

        public void SetStrategies(List<StrategyQTP> strategies, StrategyQTP.BringRunUCDelegate bringRunUC)
        {
            foreach (StrategyQTP s in strategies)
            {
                StrategyUC uc = new StrategyUC();
                uc.SetStrategy(s, bringRunUC);
                panelNav.Controls.Add(uc);
            }
        }

    }
}

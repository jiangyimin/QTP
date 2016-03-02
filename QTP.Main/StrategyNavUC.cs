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
        private Control parent;
        public StrategyNavUC(Control parent)
        {
            InitializeComponent();

            this.parent = parent;
        }

        public string Title
        {
            set 
            {
                lblTitle.Text = value;
                lblRightTitle.Text = value;
            }
        }

        public void SetStrategies(List<MyStrategy> strategies, MyStrategy.BringRunUCDelegate bringRunUC)
        {
            foreach (MyStrategy ms in strategies)
            {
                StrategyUC uc = new StrategyUC(parent);
                uc.Subject = ms;
                panelNav.Controls.Add(uc);
            }
        }

    }
}

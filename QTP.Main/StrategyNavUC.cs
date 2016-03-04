using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.DBAccess;

namespace QTP.Main
{
    public partial class StrategyNavUC : UserControl
    {
        public StrategyNavUC(List<TStrategy> strategies, Control parent)
        {
            InitializeComponent();

            foreach (TStrategy s in strategies)
            {
                StrategyUC uc = new StrategyUC(s, parent);
                panelNav.Controls.Add(uc);
            }

        }

        public string Title
        {
            set 
            {
                lblTitle.Text = value;
                lblRightTitle.Text = value;
            }
        }

        public void Close()
        {
            foreach (Control c in panelNav.Controls)
            {
                if (c is StrategyUC)
                    ((StrategyUC)c).Close();
            }
        }
    }
}

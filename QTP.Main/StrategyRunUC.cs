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
    public partial class StrategyRunUC : UserControl
    {
        public StrategyRunUC()
        {
            InitializeComponent();
        }

        public void SetStrategy(StrategyQTP qtp)
        {
            // PanelTitle
            lblTitle.Text = string.Format("{0}({1})", qtp.StrategyT.Name, qtp.Status);
            lblRightTitle.Text = string.Format("ID:{0} {1}策略", qtp.StrategyT.GMID, qtp.StrategyT.RunType);
        }
    }
}

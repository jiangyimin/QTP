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
using QTP.DBAccess;

namespace QTP.Console
{
    public partial class MonitorDataUC : UserControl
    {
        private MyStrategy strategy;
        public MonitorDataUC()
        {
            InitializeComponent();
        }

        public MyStrategy Subject
        {
            set
            {
                strategy = value;
                strategy.FocusTickHandler = tickUC.ShowTick;


                // dgvPool
                int i = 1;
                foreach (Monitor m in strategy.GetMonitorEnumerator())
                {
                    string sec_name = null; 
                    if (m.GMInstrument != null) sec_name = m.GMInstrument.sec_name;

                    int index = dgvPool.Rows.Add(i++, m.Target.Exchange, m.Target.InstrumentId, sec_name);

                    dgvPool.Rows[index].Tag = m;
                }

            // ChartUC and TickUC is auto called dgvPool_RowEnter.

            }
        }

        public void Infomation()
        {
            DataGridView uc = dgvPool;
            MessageBox.Show(string.Format("{0} {1}, {2} {3}, {4} {5}", uc.Width, uc.Height, uc.Location.X, uc.Location.Y,
                uc.Enabled, uc.Visible));

        }

        private void dgvPool_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPool.Rows.Count == 0) return;

            // display ChartUC and TickUC of this instrument.
            if (dgvPool.Rows[e.RowIndex].Tag != null)
            {
                Monitor m = (Monitor)dgvPool.Rows[e.RowIndex].Tag;

                m.SetFocus();
            }
        }
    }
}

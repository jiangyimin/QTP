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
    public partial class MonitorDataUC : UserControl, IStrategyUC
    {
        private MyStrategy strategy;
        public MonitorDataUC()
        {
            InitializeComponent();
        }

        #region IStrategyUC
        public MyStrategy Subject
        {
            set
            {
                strategy = value;

                strategy.FocusTickArrived = tickUC.OnTickArrived;
                strategy.FocusBarArrived = tickUC.OnBarArrived;

                // dgvPool's columns
                foreach (string name in Monitor.GetQuotaNames())
                    foreach (string scalar in Monitor.GetScalarNames(name))
                        dgvPool.Columns.Add(scalar, string.Format("{0}({1})", name, scalar));
            }

        }

        public void ShowData()
        {
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

        public void TimerRefresh()
        {
            int rowIndex = 0;
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {
                DataGridViewRow row = dgvPool.Rows[rowIndex++];

                int columnIndex = 4;
                foreach (string name in Monitor.GetQuotaNames())
                    foreach (string scalar in Monitor.GetScalarNames(name))
                        row.Cells[columnIndex++].Value = m.GetQuotaScalarValue(scalar);
            }

        }
        #endregion

        #region event handlers
        private void dgvPool_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPool.Rows.Count == 0) return;

            // display ChartUC and TickUC of this instrument.
            if (dgvPool.Rows[e.RowIndex].Tag != null)
            {
                Monitor m = (Monitor)dgvPool.Rows[e.RowIndex].Tag;
                m.SetFocus();

                // display tick at once
                strategy.FocusTickArrived(m.TickTA);                
            }
        }

        #endregion
    }
}

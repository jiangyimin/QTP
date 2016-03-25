using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using QTP.Domain;
using QTP.DBAccess;
using QTP.TAlib;

namespace QTP.Console
{
    public partial class MonitorDataUC : UserControl, IStrategyUC
    {
        #region member
        private const int QuataStartColumnIndex = 4;

        // strategy
        private bool dataShowed;
        private MyStrategy strategy;
        private Monitor currentMonitor;

        // Quotas
        private Dictionary<string, List<string>> quotaNames;
        private Dictionary<string, PropertyInfo> scalarProps = new Dictionary<string,PropertyInfo>();
        private string currentQuota;
        private int currentKType;  

        public MonitorDataUC()
        {
            InitializeComponent();
        }

        #endregion

        #region IStrategyUC
        public MyStrategy Subject
        {
            set
            {
                strategy = value;
            }

        }

        public void ShowData()
        {
            if (dataShowed) return;
            dataShowed = true;

            // hooked now
            strategy.FocusTickArrived += new MyStrategy.FocusTickArrivedCallback(tickUC.OnTickArrived);
            strategy.FocusBarArrived += new MyStrategy.FocusBarArrivedCallback(barUC.OnBarArrived);

            // Set Quata Columns
            Type type = Monitor.QuotaType;
            quotaNames = Monitor.QuotaNames;
            int i = 0;
            foreach (string name in quotaNames.Keys)
            {
                foreach (string scalar in quotaNames[name])
                {
                    // column
                    i = dgvPool.Columns.Add(scalar, string.Format("{0}({1})", name, scalar));
                    dgvPool.Columns[i].Tag = name;

                    // property
                    scalarProps.Add(scalar, type.GetProperty(scalar));
                }
            }

            chartUC.ScalarProps = scalarProps;

            // Fill dgvPool' Data (仅一次)
            i = 1;
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {                
                string sec_name = null;
                if (m.GMInstrument != null) sec_name = m.GMInstrument.sec_name;

                int index = dgvPool.Rows.Add(i++, m.Target.Exchange, m.Target.InstrumentId, sec_name);

                dgvPool.Rows[index].Tag = m;
            }

            // Dispaly ChartUC of current monitor.
            dgvPool_RowEnter(null, new DataGridViewCellEventArgs(0, 0));

        }

        /// <summary>
        /// Show Latest Quata in dgv
        /// </summary>
        public void TimerRefresh()
        {
            int rowIndex = 0;
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {
                DataGridViewRow row = dgvPool.Rows[rowIndex++];
                int columnIndex = QuataStartColumnIndex;

                // get quota
                object quota = m.GetLatestQuota(currentKType);
                if (quota == null) return;

                // show 
                foreach (string name in quotaNames.Keys)
                    foreach (string scalar in quotaNames[name])
                        row.Cells[columnIndex++].Value = scalarProps[scalar].GetValue(quota);
            }
        }

        #endregion

        #region event handlers
        private void dgvPool_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPool.Rows.Count == 0) return;

            // display ChartUC this instrument.
            if (dgvPool.Rows[e.RowIndex].Tag != null)
            { 
                Monitor m = (Monitor)dgvPool.Rows[e.RowIndex].Tag;
                if (currentMonitor != m)
                {
                    currentMonitor = m;
                    currentMonitor.SetFocus();
                    DrawChartButtonClick();

                    // display tick at once
                    tickUC.OnTickArrived(currentMonitor.TickTA);
                }
            }
        }

        private void dgvPool_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn col = dgvPool.Columns[e.ColumnIndex];
            if (col != null && col.Tag != null)
            {
                currentQuota = col.Tag as string;
            }

            DrawChartButtonClick();
        }

        private void DrawChartButtonClick()
        {
            // show klines and Quotas
            switch (currentKType)
            {
                case 1:
                    btnM1_Click(this, null);
                    break;
                case 15:
                    btnM15_Click(this, null);
                    break;
                default:
                    btnDay_Click(this, null);
                    break;
            }
        }

        private void btnDay_Click(object sender, EventArgs e)
        {
            currentKType = 0;
            if (currentMonitor != null)
                DrawChart();
        }


        private void btnM15_Click(object sender, EventArgs e)
        {
            currentKType = 15;
            if (currentMonitor != null)
                DrawChart();
        }

        private void btnM1_Click(object sender, EventArgs e)
        {
            currentKType = 1;
            if (currentMonitor != null)
                DrawChart();
        }

        private void DrawChart()
        {
            RList<KLine> ks = currentMonitor.GetKLines(currentKType);

            RList<object> qs = null;
            List<string> names = null;
            if (currentQuota != null)
            {
                names = quotaNames[currentQuota];
                qs = currentMonitor.GetQuotas(currentKType);
            }

            chartUC.DrawChart(ks, currentKType, qs, names);
        }

        #endregion
    }
}

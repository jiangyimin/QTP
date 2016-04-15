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
    public partial class MonitorDataUC : UserControl
    {
        #region member
        private const int QuataStartColumnIndex = 5;

        // strategy
        private bool dataShowed;
        private MyStrategy strategy;

        private Monitor currentMonitor;
        private int currentKType = 0;           // 日线做为缺省类型  


        public MonitorDataUC()
        {
            InitializeComponent();
        }

        #endregion

        public MyStrategy Subject
        {
            set
            {
                strategy = value;
            }
        }

        public void ShowData()
        {
            if (dataShowed) return;  // 仅一次
            dataShowed = true;

            // hooked now
            strategy.FocusTickArrived += new MyStrategy.FocusTickArrivedCallback(tickUC.OnTickArrived);
            strategy.FocusBarArrived += new MyStrategy.FocusBarArrivedCallback(barUC.OnBarArrived);

            FillDataGridView(currentKType);     // 缺省是日线
            dgvPool_CellEnter(this, new DataGridViewCellEventArgs(0, 0));
        }


        #region event handlers

        private void dgvPool_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPool.Rows[e.RowIndex].Tag != null)
            { 
                Monitor m = (Monitor)dgvPool.Rows[e.RowIndex].Tag;
                if (currentMonitor != m)
                {
                    currentMonitor = m;
                    currentMonitor.SetFocus();

                    DrawChart(currentMonitor, currentKType);

                    // tickUC and BarUC 
                    tickUC.OnTickArrived(currentMonitor.TA.TickTA);
                    barUC.Clear();
                }
            }
        }

        private void btnDay_Click(object sender, EventArgs e)
        {
            currentKType = 0;
            FillDataGridView(currentKType);
        }

        private void btnM60_Click(object sender, EventArgs e)
        {
            currentKType = 60;
            FillDataGridView(currentKType);
        }

        private void btnM30_Click(object sender, EventArgs e)
        {
            currentKType = 30;
            FillDataGridView(currentKType);
        }


        private void btnM15_Click(object sender, EventArgs e)
        {
            currentKType = 15;
            FillDataGridView(currentKType);
        }
        private void btnM5_Click(object sender, EventArgs e)
        {
            currentKType = 5;
            FillDataGridView(currentKType);
        }

        private void btnM1_Click(object sender, EventArgs e)
        {
            currentKType = 1;
            FillDataGridView(currentKType);
        }

        #endregion

        #region private methods

        private void FillDataGridView(int ktype)
        {
            // First Clear dgv
            dgvPool.Rows.Clear();
            dgvPool.Columns.Clear();

            // create dgvPool' columns
            dgvPool.Columns.Add("Column1", "序号");
            dgvPool.Columns.Add("Column2", "代码");
            dgvPool.Columns.Add("Column3", "名称");
            dgvPool.Columns.Add("Column4", "可卖仓位");
            dgvPool.Columns.Add("Column5", "止损价格");

            int i = 1;      // 序号
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {
                int index = dgvPool.Rows.Add(i++, m.Target.Symbol, m.TargetTitle, m.Target.Volume, m.Target.StopLossPrice);
                dgvPool.Rows[index].Tag = m;
            }

            if (dgvPool.Rows.Count > 0)
            {
                Monitor m = currentMonitor == null ? (Monitor)dgvPool.Rows[0].Tag : currentMonitor;
                // Get QuotaNames by currenKType
                Dictionary<string, List<string>> quotaNames = m.TA.GetQuotaNames(ktype);
                FillQuotaToDataGridView(quotaNames, ktype);
            }
        }

        private void FillQuotaToDataGridView(Dictionary<string, List<string>> quotaNames, int ktype)
        {
            // columns
            int columnIndex = 0;
            foreach (string name in quotaNames.Keys)
            {
                foreach (string scalar in quotaNames[name])
                {
                    // column
                    columnIndex = dgvPool.Columns.Add(scalar, string.Format("{0}({1})", name, scalar));
                    dgvPool.Columns[columnIndex].Tag = new KeyValuePair<string, List<string>>(name, quotaNames[name]);
                }
            }

            // fill Data
            int rowIndex = 0;
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {

                DataGridViewRow row = dgvPool.Rows[rowIndex++];

                // Get QuotaScalarValues
                List<double> v = m.TA.GetLatestScalarValues(currentKType);
                if (v.Count == 0) continue;

                int count = 0; 
                foreach (string name in quotaNames.Keys)
                {
                    foreach (string scalar in quotaNames[name])
                    {
                        if (count <= v.Count - 1)
                        {
                            row.Cells[QuataStartColumnIndex + count].Value = v[count];
                            count++;
                        }
                    }
                }
            }
        }

        private void DrawChart(Monitor m, int ktype)
        {
            object tag = dgvPool.Columns[dgvPool.CurrentCell.ColumnIndex].Tag;
            if (tag == null)
            {
                 chartUC.DrawChart(m.TA.GetKLines(ktype), ktype, new List<RList<double>>(), null);
            }
            else
            {
                KeyValuePair<string, List<string>> pair = (KeyValuePair<string, List<string>>)tag;
                chartUC.DrawChart(m.TA.GetKLines(ktype), ktype, m.TA.GetScalarValues(pair.Key, ktype), pair.Value);
            }
        }

        #endregion


    }
}

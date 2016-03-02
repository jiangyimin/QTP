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
    public partial class MonitorUC : UserControl
    {
        private MyStrategy qtp;
        public MonitorUC()
        {
            InitializeComponent();
        }

        public void SetStrategy(MyStrategy qtp)
        {
            // hook with Strategy
            this.qtp = qtp;
            qtp.FocusTickHandler = tickUC.ShowTick;


            // dgvPool
            int i = 1;
            foreach (TInstrument ins in qtp.StrategyT.Instruments)
            {
                int index = dgvPool.Rows.Add(i++, ins.Exchange, ins.InstrumentId, MyStrategy.DictInstruments[ins.Symbol].sec_name);
                dgvPool.Rows[index].Tag = ins;
            }

            // ChartUC and TickUC is auto called dgvPool_RowEnter.
        }

        private void SetPagePool()
        {
        }

        private void dgvPool_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPool.Rows.Count == 0) return;

            // display ChartUC and TickUC of this instrument.
            TInstrument ins = (TInstrument)dgvPool.Rows[e.RowIndex].Tag;

            qtp.FocusInstrument = ins;
        }
    }
}

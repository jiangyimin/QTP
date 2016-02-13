using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

using QTP.Domain;
using QTP.DBAccess;
using QTP.Infra;

namespace QTP.Main
{
    public partial class StrategyForm : Form
    {
        private List<TStrategy> lst;
        private TStrategy current;

        public StrategyForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void StrategyForm_Load(object sender, EventArgs e)
        {
            lst = CRUD.GetTStrategies();

            foreach (TStrategy t in lst)
            {
                t.Instruments = CRUD.GetTStrategyInstruments(t.Id);

                ListViewItem item = lvStrategy.Items.Add(t.Id.ToString());
                item.SubItems.Add(t.Name);
                item.SubItems.Add(t.Status);
                item.SubItems.Add(t.MonitorClass);
                item.SubItems.Add(t.RiskMClass);
                item.SubItems.Add(t.TradeChannel);
            }
        }


        private void lvStrategy_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvStrategy.SelectedIndices.Count == 0) return;

            current = lst[lvStrategy.SelectedIndices[0]];

            // Show SubTables
            int i = 1;
            lvInstrument.Items.Clear();
            foreach (TInstrument instrument in current.Instruments)
            {
                ListViewItem item = lvInstrument.Items.Add(i++.ToString());
                item.SubItems.Add(instrument.InstrumentId);
                item.SubItems.Add(instrument.Exchange);
            }
        }

        private void btnVRun_Click(object sender, EventArgs e)
        {
            RunCurrentStrategy(true);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunCurrentStrategy(false);
        }

        private void RunCurrentStrategy(bool vt)
        {
            // check
            if (current == null) return;

            if (current.Status != "生产")
            {
                MessageBox.Show("此策略实例为非生产态");
                return;
            }

            if (Global.RunningStrategies.ContainsKey(current.Id))
            {
                MessageBox.Show("此策略实例已在运行中");
                return;
            }

            try
            {
                // Get class type
                Assembly assembly = Assembly.LoadFrom(@"QTP.Domain.dll");
                string name = System.Text.RegularExpressions.Regex.Match(current.MonitorClass, @"[^(]+").Value;
                Type monitorType = assembly.GetType(string.Format("QTP.Domain.{0}", name));

                name = System.Text.RegularExpressions.Regex.Match(current.RiskMClass, @"[^(]+").Value;
                Type riskType = assembly.GetType(string.Format("QTP.Domain.{0}", name));

                current.Login = Global.Login;
                StrategyQTP s = new StrategyQTP(current, monitorType, riskType, vt);

                // new Monitor form
                StrategyMonitorForm frm = new StrategyMonitorForm();
                frm.MdiParent = this.MdiParent;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
                frm.SetStrategy(s, current.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void StrategyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Global.CanClose)
                e.Cancel = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.Domain;
using System.Diagnostics;
//using QTP.MDWrapper;

namespace QTP.Main
{
    public partial class MainForm : Form
    {
        #region members
        private StrategyNavUC realStrategiesNavUC;
        private StrategyNavUC simuStrategiesNavUC;

        #endregion


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();

            if (form.ShowDialog() == DialogResult.Cancel)
            {
                this.Close();
                return;
            }

            try
            {
                Global.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                return;
            }

            // default action.
            btnReal_Click(this, null);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //关闭所有运行策略ToolStripMenuItem_Click(this, null);
            //strategyForm.Stop();
            //this.Close();

            Process.GetCurrentProcess().Kill();  //

        }

        #region menus

        // 展现实盘策略
        private void btnReal_Click(object sender, EventArgs e)
        {
            // create first
            if (realStrategiesNavUC == null)
            {
                realStrategiesNavUC = new StrategyNavUC(this.splitMain.Panel2);
                realStrategiesNavUC.Dock = DockStyle.Fill;

                realStrategiesNavUC.Title = "实盘交易";
                realStrategiesNavUC.SetStrategies(Global.RealStrategies, null);

                splitMain.Panel2.Controls.Add(realStrategiesNavUC);
            }

            // 置顶
            realStrategiesNavUC.BringToFront();
        }

        // 展现模拟策略
        private void btnSimulate_Click(object sender, EventArgs e)
        {
            // create first
            if (simuStrategiesNavUC == null)
            {
                simuStrategiesNavUC = new StrategyNavUC(this.splitMain.Panel2);
                simuStrategiesNavUC.Dock = DockStyle.Fill;

                simuStrategiesNavUC.Title = "模拟交易";
                simuStrategiesNavUC.SetStrategies(Global.SimuStrategies, null);

                splitMain.Panel2.Controls.Add(simuStrategiesNavUC);
            }

            // 置顶
            simuStrategiesNavUC.BringToFront();
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using QTP.DBAccess;
using QTP.Domain;

namespace QTP.Console
{
    public partial class MainForm : Form
    {
        private string[] args;
        private MyStrategy strategy;


        // three UC
        private MonitorOverviewUC monitorOverviewUC;
        private MonitorDataUC monitorDataUC;
        private RiskTradeUC riskTradeUC;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string[] args)
        {
            InitializeComponent();

            this.args = args;

            monitorOverviewUC = new MonitorOverviewUC();
            monitorOverviewUC.Dock = DockStyle.Fill;
            monitorDataUC = new MonitorDataUC();
            monitorDataUC.Dock = DockStyle.Fill;
            riskTradeUC = new RiskTradeUC();
            riskTradeUC.Dock = DockStyle.Fill; 

            panelClient.Controls.Add(monitorOverviewUC);
            panelClient.Controls.Add(monitorDataUC);
            panelClient.Controls.Add(riskTradeUC);

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // alert args length
            if (args.Length < 3) return;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            CRUD.ConnectionString =
                config.ConnectionStrings.ConnectionStrings["QTP_DB"].ConnectionString.ToString();
            
            try
            {
                // MyStrategy
                TStrategy strategyT = CRUD.GetTStrategy(args[0]);
                TLogin gmLogin = new TLogin(args[1], args[2]);
                strategy = new MyStrategy(strategyT, gmLogin);

                // InitGUI and start it
                strategy.Prepare();
                InitStrategyGUI();
                strategy.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // default UI.
            btnOverView_Click(this, null);
        }

        private void InitStrategyGUI()
        {
            // set title
            lblTitle.Text = strategy.Name;
            lblRightTitle.Text = string.Format("{0} {1}", strategy.GMID, strategy.RunType);

            // lblConnecStatus hook
            strategy.ConnectStatusChanged += strategy_ConnectStatusChanged;

            monitorOverviewUC.Subject = strategy;   // pageMonitorOverView
            monitorDataUC.Subject = strategy;       // pageMonitorData
            riskTradeUC.Subject = strategy;         // pageRiskTrade
        }

        void strategy_ConnectStatusChanged(bool connectSucceed, int num)
        {
            if (lblConnectStatus.InvokeRequired == false)
            {
                if (connectSucceed)
                    lblConnectStatus.Text = string.Format("已连接({0})", num);
                else
                    lblConnectStatus.Text = string.Format("已断开({0})", num);
            }
            else
            {
                MyStrategy.ConnectStatusChangedCallback cb = new MyStrategy.ConnectStatusChangedCallback(strategy_ConnectStatusChanged);
                lblConnectStatus.BeginInvoke(cb, connectSucceed, num);
            }
        }

        private void btnOverView_Click(object sender, EventArgs e)
        {
            monitorOverviewUC.BringToFront();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            monitorDataUC.BringToFront();

        }

        private void btnRiskTrade_Click(object sender, EventArgs e)
        {
            riskTradeUC.BringToFront();
        }

    }
}

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
        #region member
        private string[] args;
        private MyStrategy strategy;


        // three UC
        private MonitorOverviewUC monitorOverviewUC;
        private MonitorDataUC monitorDataUC;
        private RiskTradeUC riskTradeUC;

        // Timer
        private Timer timerRefresh;
        private IStrategyUC refreshUC;

        #endregion
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string[] args)
        {
            InitializeComponent();

            this.args = args;

            riskTradeUC = new RiskTradeUC();
            riskTradeUC.Dock = DockStyle.Fill;
            monitorOverviewUC = new MonitorOverviewUC();
            monitorOverviewUC.Dock = DockStyle.Fill;
            monitorDataUC = new MonitorDataUC();
            monitorDataUC.Dock = DockStyle.Fill;

            panelClient.Controls.Add(riskTradeUC);
            panelClient.Controls.Add(monitorOverviewUC);
            panelClient.Controls.Add(monitorDataUC);

            // timer
            timerRefresh = new Timer();
            timerRefresh.Interval = (int)nucInterval.Value * 1000;
            timerRefresh.Tick += timerRefresh_Tick;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (strategy != null)
                strategy.MyStrategyStop();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // alert args length
            if (args.Length < 1) return;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            CRUD.ConnectionString =
                config.ConnectionStrings.ConnectionStrings["QTP_DB"].ConnectionString.ToString();

            PopupForm popup = new PopupForm();

            // async create and initialize strategy 
            Task task = new Task(Initialize, popup);
            task.Start();
            // and using popupWindow to wait for finished 
            
            if (popup.ShowDialog() != DialogResult.OK)
            {
                this.Close();
                return;
            }

            // start strategy
            strategy.Start();

            // set title
            lblTitle.Text = string.Format("{0} ({1})", strategy.Name, strategy.PoolName);
            lblRightTitle.Text = string.Format("{0} {1}", strategy.GMID, strategy.RunType);

            btnRiskTrade_Click(this, null);
            timerRefresh.Start();
        }

        private void Initialize(object pop)
        {
            try
            {
                // DataBase Operation
                TStrategy strategyT = CRUD.GetTStrategy(args[0]);

                // TLogin
                TLogin gmLogin = null;
                if (args.Length == 3)
                    gmLogin = new TLogin(args[1], args[2]);
                else
                    gmLogin = CRUD.GetTLogin();

                strategy = new MyStrategy(strategyT, gmLogin);
            }
            catch (Exception ex)
            {
                ProcessInitializeExecption(ex.Message);
            }

            // new MyStrategy and Prepare preStart Data.
            strategy.InitializeExceptionOccur += new MyStrategy.InitializeExceptionCallback(ProcessInitializeExecption);

            HookStrategyToGUI();
            strategy.Initialize();

            ((PopupForm)pop).TaskFinished();
        }

        // Handler for exception when Initializing
        private void ProcessInitializeExecption(string message)
        {
            if (this.InvokeRequired == false)
            {
                MessageBox.Show(message);
                this.Close();
            }
            else
            {
                MyStrategy.InitializeExceptionCallback cb = new MyStrategy.InitializeExceptionCallback(ProcessInitializeExecption);
                lblTDStatus.BeginInvoke(cb, message);
            }
        }

        private void HookStrategyToGUI()
        {
            // lblConnecStatus hook
            strategy.ConnectStatusChanged += strategy_ConnectStatusChanged;

            riskTradeUC.Subject = strategy;         // pageRiskTrade
            monitorOverviewUC.Subject = strategy;   // pageMonitorOverView
            monitorDataUC.Subject = strategy;       // pageMonitorData
        }

        void strategy_ConnectStatusChanged(bool md, string status)
        {
            if (lblTDStatus.InvokeRequired == false)
            {
                if (md)
                    lblMDStatus.Text = status;
                else
                    lblTDStatus.Text = status;
            }
            else
            {
                MyStrategy.ConnectStatusChangedCallback cb = new MyStrategy.ConnectStatusChangedCallback(strategy_ConnectStatusChanged);
                lblTDStatus.BeginInvoke(cb, md, status);
            }
        }

        private void btnOverView_Click(object sender, EventArgs e)
        {
            monitorOverviewUC.BringToFront();
            refreshUC = monitorOverviewUC;
            refreshUC.ShowData();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            monitorDataUC.BringToFront();
            refreshUC = monitorDataUC;
            refreshUC.ShowData();
        }

        private void btnRiskTrade_Click(object sender, EventArgs e)
        {
            riskTradeUC.BringToFront();
            refreshUC = riskTradeUC;
            refreshUC.ShowData();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            refreshUC.TimerRefresh();
        }

        private void nucInterval_ValueChanged(object sender, EventArgs e)
        {
            timerRefresh.Interval = (int)nucInterval.Value * 1000;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using QTP.Infra;
using QTP.Domain;

namespace QTP.Main
{
    public partial class StrategyMonitorForm : Form
    {
        private StrategyQTP qtp;
        private int strategyId;

        private IntPtr tradeWin; 
        public StrategyMonitorForm()
        {
            InitializeComponent();
        }

        public void SetStrategy(StrategyQTP s, int id)
        {
            qtp = s;
            strategyId = id;
            s.OnMessage += OnMessage;
            Task t = new Task(s.Start);
            t.Start();
        }

        private void OnMessage(string msg)
        {
            if (this.listBox1.InvokeRequired == false)
            {
                listBox1.Items.Add(msg);
            }
            else
            {
                DispMSGDelegate DMSGD = new DispMSGDelegate(OnMessage);
                this.listBox1.BeginInvoke(DMSGD, msg);
            }
        }

        private delegate void DispMSGDelegate(string msg);

        private void StrategyMonitorForm_Load(object sender, EventArgs e)
        {

        }

        private void StrategyMonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            qtp.Stop();
            Global.RunningStrategies.Remove(strategyId);
        }

        private void btnStartTrade_Click(object sender, EventArgs e)
        {
            string[] splits = qtp.StrategyT.TradeChannel.Split(new char[] { '(', ',', ')' });
            string exePathName = splits[1];
            string exeName = splits[2];

            // find windows of this exeName
            Process[] processes = Process.GetProcessesByName(exeName);

            // Start the process 
            Process process;
            if (processes.Length == 0)
                process = System.Diagnostics.Process.Start(exePathName);
            else
                process = processes[0];

            // Wait for process to be created and enter idle condition 
            tradeWin = process.MainWindowHandle;

            WinAPI.SetParent(process.MainWindowHandle, this.splitContainer1.Panel2.Handle);
            // Move the window to overlay it on this window
            WinAPI.MoveWindow(tradeWin, 0, 0, this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height, true);
            WinAPI.SetForegroundWindow(tradeWin);
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            WinAPI.SetForegroundWindow(tradeWin);
            if (tradeWin != IntPtr.Zero)
            {
                WinAPI.MoveWindow(tradeWin, 0, 0, this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WinAPI.SetForegroundWindow(tradeWin);
            SendKeys.Send("600100");
            SendKeys.Send("{TAB}");
            SendKeys.Send("11.11");
            SendKeys.Send("{Tab}");
            SendKeys.Send("100");
            SendKeys.SendWait("{Enter}");
            SendKeys.SendWait("^y");

        }
    }
  }

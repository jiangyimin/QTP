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
        private bool canClose = false;

        private MyStrategy qtp;

        private IntPtr tradeWin; 
        public StrategyMonitorForm()
        {
            InitializeComponent();
        }

        public void StartStrategy(MyStrategy s)
        {
            qtp = s;
            qtp.MessageHint += OnMessage;
            qtp.OnKBOpenLong += KBBuy;
            qtp.OnKBCloseLong += KBSell;
            Task t = new Task(s.Connect);
            t.Start();
        }

        public void StopStrategy()
        {
            canClose = true;
            qtp.Stop();
            this.Close();
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
            {
                process = System.Diagnostics.Process.Start(exePathName);
                System.Threading.Thread.Sleep(10000);
            }
            else
                process = processes[0];

            // Wait for process to be created and enter idle condition 
            tradeWin = process.MainWindowHandle;

            WinAPI.SetParent(tradeWin, this.splitContainer1.Panel2.Handle);

            Int32 wndStyle = WinAPI.GetWindowLong(tradeWin, WinAPI.GWL_STYLE);
            wndStyle &= ~WinAPI.WS_BORDER;
            wndStyle &= ~WinAPI.WS_THICKFRAME;
            WinAPI.SetWindowLong(tradeWin, WinAPI.GWL_STYLE, wndStyle);

            WinAPI.SetWindowPos(tradeWin, IntPtr.Zero, 0, 0, 0, 0, WinAPI.SWP_NOMOVE | WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER | WinAPI.SWP_FRAMECHANGED);
            
            // 在Resize事件中更新目标应用程序的窗体尺寸.
            splitContainer1_Panel2_Resize(this, null); 
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            WinAPI.SetWindowPos(tradeWin, IntPtr.Zero, 0, 0, this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height, WinAPI.SWP_NOZORDER);
        }


        private delegate void KBTradeDelegate(string sec_id, double price, double volume);

        private void KBBuy(string sec_id, double price, double volume)
        {
            if (this.InvokeRequired == false)
            {
                WinAPI.SetWindowPos(tradeWin, WinAPI.HWND_TOPMOST, 0, 0, this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height, WinAPI.SWP_NOZORDER);
                //WinAPI.SetForegroundWindow(tradeWin);
                SendKeys.SendWait("{F1}");
                SendKeys.Send(sec_id);
                SendKeys.Send("{TAB}");
                SendKeys.Send(string.Format("{0:0.00}", price));
                SendKeys.Send("{Tab}");
                SendKeys.Send(string.Format("{0}", volume));
                SendKeys.SendWait("{Enter}");
//                SendKeys.SendWait("^y");
            }
            else
            {
                KBTradeDelegate KBBuyD = new KBTradeDelegate(KBBuy);
                this.Invoke(KBBuyD, sec_id, price, volume);

            }
        }
        private void KBSell(string sec_id, double price, double volume)
        {
            if (this.InvokeRequired == false)
            {
                WinAPI.SetWindowPos(tradeWin, WinAPI.HWND_TOPMOST, 0, 0, this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height, WinAPI.SWP_NOZORDER);
                //WinAPI.SetForegroundWindow(tradeWin);
                SendKeys.SendWait("{F2}");
                SendKeys.Send(sec_id);
                SendKeys.Send("{TAB}");
                SendKeys.Send(string.Format("{0}", price));
                SendKeys.Send("{Tab}");
                SendKeys.Send(string.Format("{0}", volume));
                SendKeys.SendWait("{Enter}");
                SendKeys.SendWait("^y");
            }
            else
            {
                KBTradeDelegate KBSellD = new KBTradeDelegate(KBSell);
                this.Invoke(KBSellD, sec_id, price, volume);

            }
        }

        private void StrategyMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.canClose)
                e.Cancel = true;
        }
    }
  }

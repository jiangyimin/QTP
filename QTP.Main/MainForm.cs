using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.DBAccess;

namespace QTP.Main
{
    public partial class MainForm : Form
    {
        #region members
        private StrategyNavUC realStrategiesNavUC;
        private StrategyNavUC simuStrategiesNavUC;
        private StrategyNavUC btStrategiesNavUC;

        // show second timer
        private Timer secondTimer;
        #endregion


        public MainForm()
        {
            InitializeComponent();

            secondTimer = new Timer();
            secondTimer.Interval = 1000;
            secondTimer.Tick += secondTimer_Tick;
        }

        void secondTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblSecond.Text = string.Format("{0:00}:{1:00}", dt.Minute, dt.Second);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();

            if (form.ShowDialog() == DialogResult.Cancel)
            {
                this.Close();
                return;
            }

            // timer
            secondTimer.Start();

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
            //关闭所有运行策略
            if (realStrategiesNavUC != null)
                realStrategiesNavUC.Close();
            if (simuStrategiesNavUC != null)
                simuStrategiesNavUC.Close();
            if (btStrategiesNavUC != null)
                btStrategiesNavUC.Close();

            System.Diagnostics.Process.GetCurrentProcess().Kill(); 
        }

        #region menus

        // 展现实盘策略
        private void btnReal_Click(object sender, EventArgs e)
        {
            ShowStrategyNavUC(ref realStrategiesNavUC, Global.RealStrategies, "实盘交易");
        }

        // 展现模拟策略
        private void btnSimulate_Click(object sender, EventArgs e)
        {
            ShowStrategyNavUC(ref simuStrategiesNavUC, Global.SimuStrategies, "虚拟交易");
        }

        private void btnBackTest_Click(object sender, EventArgs e)
        {
            ShowStrategyNavUC(ref btStrategiesNavUC, Global.BTStrategies, "策略回测");
        }

        private void ShowStrategyNavUC(ref StrategyNavUC uc, List<TStrategy> strategies, string title)
        {
            if (uc == null)
            {
                uc = new StrategyNavUC(strategies, this.splitMain.Panel2);
                uc.Dock = DockStyle.Fill;
                uc.Title = title;

                splitMain.Panel2.Controls.Add(uc);
            }

            // 置顶
            uc.BringToFront();
        }

        private void btnSetTime_Click(object sender, EventArgs e)
        {
            DateTime dt = GetBeijingTime();

            //QTP.Infra.Utils.SetDate(dt);
        }

        private DateTime GetBeijingTime()  
        {  
            DateTime dt;  
            WebRequest wrt = null;  
            WebResponse wrp = null;
            StreamReader sr = null;
            try  
            {  
                wrt = WebRequest.Create("http://www.time.ac.cn/stime.asp");
                wrt.Credentials = CredentialCache.DefaultCredentials;
                wrp = wrt.GetResponse();
                sr = new StreamReader(wrp.GetResponseStream(),Encoding.UTF8);
                string html = sr.ReadToEnd();
                int yearIndex = html.IndexOf("<year>") + 6;
                int monthIndex = html.IndexOf("<month>") + 7;
                int dayIndex = html.IndexOf("<day>") + 5;
                int hourIndex = html.IndexOf("<hour>") + 6;
                int miniteIndex = html.IndexOf("<minite>") + 8;
                int secondIndex = html.IndexOf("<second>") + 8;
                string year = html.Substring(yearIndex, html.IndexOf("</year>") - yearIndex);
                string month = html.Substring(monthIndex, html.IndexOf("</month>") - monthIndex);  
                string day = html.Substring(dayIndex, html.IndexOf("</day>") - dayIndex);  
                string hour = html.Substring(hourIndex, html.IndexOf("</hour>") - hourIndex);  
                string minite = html.Substring(miniteIndex, html.IndexOf("</minite>") - miniteIndex);  
                string second = html.Substring(secondIndex, html.IndexOf("</second>") - secondIndex);  
                dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minite + ":" + second);
            }  
            catch (WebException)  
            {  
                MessageBox.Show("同步出错");
                dt = DateTime.Now;
            }  
            catch (Exception)  
            {  
                MessageBox.Show("同步出错");
                dt = DateTime.Now;
            }  
            finally  
            { 
                if (sr != null) sr.Close();
                if (wrp != null) wrp.Close();  
                if (wrt != null) wrt.Abort();  
            }

            return dt;

        }

        #endregion


    }
}

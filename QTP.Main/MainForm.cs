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
        private StrategyForm strategyForm;

        // Running Strategies
        private static Dictionary<int, StrategyQTP> runningStrategies = new Dictionary<int, StrategyQTP>();

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

            策略ToolStripMenuItem_Click(this, null);
        }

        #region menus
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            关闭所有运行策略ToolStripMenuItem_Click(this, null);
            strategyForm.Stop();
            this.Close();
        }


        private void 行情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe");//设置运行的命令行文件问ping.exe文件，这个文件系统会自己找到

            //如果是其它exe文件，则有可能需要指定详细路径，如运行winRar.exe

            //start.Arguments = "www.126.com";    //设置命令参数

            start.CreateNoWindow = false;    //不显示dos命令行窗口

            //start.RedirectStandardOutput = true;//

            //start.RedirectStandardInput = true;//

            start.UseShellExecute = true;//是否指定操作系统外壳进程启动程序

            Process p= Process.Start(start);


            p.WaitForExit();//等待程序执行完退出进程

            p.Close();//关闭进程

        }

        private void 策略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ShowChildrenForm("策略管理"))
            {
                strategyForm = new StrategyForm(this);
                strategyForm.MdiParent = this;
                strategyForm.WindowState = FormWindowState.Maximized;
                strategyForm.Show();
            }
        }
        #endregion

        #region util
        // 防止打开多个窗体
        private bool ShowChildrenForm(string text)
        {
            int i;
            //依次检测当前窗体的子窗体
            for (i = 0; i < this.MdiChildren.Length; i++)
            {
                //判断当前子窗体的Text属性值是否与传入的字符串值相同
                if (this.MdiChildren[i].Text == text)
                {
                    //如果值相同则表示此子窗体为想要调用的子窗体，激活此子窗体并返回true值
                    this.MdiChildren[i].Activate();
                    return true;
                }
            }
            //如果没有相同的值则表示要调用的子窗体还没有被打开，返回false值
            return false;
        }

        public bool ExistRunningStrategy(int id)
        {
            return runningStrategies.ContainsKey(id);
        }

        public void NewStrategyMonitorForm(StrategyQTP qtp)
        {
            // new form
            StrategyMonitorForm frm = new StrategyMonitorForm();
            frm.Text = string.Format("{0} {1}", qtp.StrategyT.Id, qtp.StrategyT.Name);
            frm.MdiParent = this;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();

            // insert menu
            ToolStripMenuItem item = new ToolStripMenuItem(frm.Text, null, new System.EventHandler(StrategyMenuItem_Click));
            窗口ToolStripMenuItem.DropDownItems.Add(item);

            // Start Run Strategy
            frm.StartStrategy(qtp);
            runningStrategies.Add(qtp.StrategyT.Id, qtp);
        }

        public void CloseStrategyMonitorForm(int id)
        {
            StrategyQTP qtp = runningStrategies[id];

            // close form  
            string formText = string.Format("{0} {1}", id, qtp.StrategyT.Name);
            int i;
            //依次检测当前窗体的子窗体
            for (i = 0; i < this.MdiChildren.Length; i++)
            {
                //判断当前子窗体的Text属性值是否与传入的字符串值相同
                if (this.MdiChildren[i].Text == formText)
                {
                    StrategyMonitorForm frm = (StrategyMonitorForm)this.MdiChildren[i];
                    frm.StopStrategy();
                    break;
                }
            }

            // remove menu item
            i = 0;
            foreach (ToolStripItem item in 窗口ToolStripMenuItem.DropDownItems)
            {
                if (item.Text == formText) break;
                i++;
            }

            窗口ToolStripMenuItem.DropDownItems.RemoveAt(i);
            runningStrategies.Remove(id);
        }

        private void StrategyMenuItem_Click(object sender, EventArgs e)
        {
            // Set activeted
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            ShowChildrenForm(item.Text);
        }

        #endregion

        private void 关闭所有运行策略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<int> IDs = new List<int>();
            foreach (KeyValuePair<int, StrategyQTP> pair in runningStrategies)
            {
                IDs.Add(pair.Key);
            }

            foreach (int id in IDs)
                CloseStrategyMonitorForm(id);

        }

    }
}

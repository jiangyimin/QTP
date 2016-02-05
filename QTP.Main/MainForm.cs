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
            }

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        #region menus
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

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
            if (!ShowChildrenForm("StrategyForm"))
            {
                Form frm = new StrategyForm();
                frm.MdiParent = this;
                frm.Show();
            }
        }

        //防止打开多个窗体
        private bool ShowChildrenForm(string name)
        {
            int i;
            //依次检测当前窗体的子窗体
            for (i = 0; i < this.MdiChildren.Length; i++)
            {
                //判断当前子窗体的Text属性值是否与传入的字符串值相同
                if (this.MdiChildren[i].Name == name)
                {
                    //如果值相同则表示此子窗体为想要调用的子窗体，激活此子窗体并返回true值
                    this.MdiChildren[i].Activate();
                    return true;
                }
            }
            //如果没有相同的值则表示要调用的子窗体还没有被打开，返回false值
            return false;
        }
    }
}

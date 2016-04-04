using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTP.Console
{
    public partial class PopupForm : Form
    {
        private int count;
        public PopupForm()
        {
            InitializeComponent();
        }

        private delegate void TaskFinishedCallback(string errorMessage);

        public void TaskFinished(string errorMessage)
        {
            if (this.InvokeRequired == false)
            {
                if (errorMessage == null)
                {
                    timer.Stop();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.Text = errorMessage;
                    System.Threading.Thread.Sleep(3000);
                    this.DialogResult = DialogResult.Abort;
                }
            }
            else
            {
                TaskFinishedCallback handler = new TaskFinishedCallback(this.TaskFinished);
                this.BeginInvoke(handler, errorMessage);
            }
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            count += 1;
            label1.Text = string.Format("数据装载中...({0})", count);
        }
    }
}

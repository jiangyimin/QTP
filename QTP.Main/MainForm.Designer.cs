namespace QTP.Main
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.lblSecond = new System.Windows.Forms.Label();
            this.btnSimulate = new System.Windows.Forms.Button();
            this.btnReal = new System.Windows.Forms.Button();
            this.btnSetTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.splitMain.Panel1.Controls.Add(this.btnSetTime);
            this.splitMain.Panel1.Controls.Add(this.lblSecond);
            this.splitMain.Panel1.Controls.Add(this.btnSimulate);
            this.splitMain.Panel1.Controls.Add(this.btnReal);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitMain.Size = new System.Drawing.Size(933, 334);
            this.splitMain.SplitterDistance = 65;
            this.splitMain.TabIndex = 2;
            // 
            // lblSecond
            // 
            this.lblSecond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSecond.AutoSize = true;
            this.lblSecond.ForeColor = System.Drawing.SystemColors.Window;
            this.lblSecond.Location = new System.Drawing.Point(13, 304);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(35, 12);
            this.lblSecond.TabIndex = 0;
            this.lblSecond.Text = "09:01";
            // 
            // btnSimulate
            // 
            this.btnSimulate.Location = new System.Drawing.Point(9, 94);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(45, 39);
            this.btnSimulate.TabIndex = 1;
            this.btnSimulate.Text = "模拟";
            this.btnSimulate.UseVisualStyleBackColor = true;
            this.btnSimulate.Click += new System.EventHandler(this.btnSimulate_Click);
            // 
            // btnReal
            // 
            this.btnReal.Location = new System.Drawing.Point(9, 39);
            this.btnReal.Name = "btnReal";
            this.btnReal.Size = new System.Drawing.Size(45, 40);
            this.btnReal.TabIndex = 0;
            this.btnReal.Text = "实盘";
            this.btnReal.UseVisualStyleBackColor = true;
            this.btnReal.Click += new System.EventHandler(this.btnReal_Click);
            // 
            // btnSetTime
            // 
            this.btnSetTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetTime.Location = new System.Drawing.Point(7, 268);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(51, 23);
            this.btnSetTime.TabIndex = 0;
            this.btnSetTime.Text = "同步";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.Click += new System.EventHandler(this.btnSetTime_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 334);
            this.Controls.Add(this.splitMain);
            this.Name = "MainForm";
            this.Text = "QTP-量化交易平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.Button btnReal;
        private System.Windows.Forms.Label lblSecond;
        private System.Windows.Forms.Button btnSetTime;
    }
}


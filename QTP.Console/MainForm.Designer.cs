namespace QTP.Console
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.btnRiskTrade = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.btnOverView = new System.Windows.Forms.Button();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelClient = new System.Windows.Forms.Panel();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.SystemColors.Control;
            this.panelTitle.Controls.Add(this.btnRiskTrade);
            this.panelTitle.Controls.Add(this.btnData);
            this.panelTitle.Controls.Add(this.btnOverView);
            this.panelTitle.Controls.Add(this.lblConnectStatus);
            this.panelTitle.Controls.Add(this.lblRightTitle);
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(923, 36);
            this.panelTitle.TabIndex = 1;
            // 
            // btnRiskTrade
            // 
            this.btnRiskTrade.Location = new System.Drawing.Point(530, 0);
            this.btnRiskTrade.Name = "btnRiskTrade";
            this.btnRiskTrade.Size = new System.Drawing.Size(72, 36);
            this.btnRiskTrade.TabIndex = 6;
            this.btnRiskTrade.Text = "资金交易";
            this.btnRiskTrade.UseVisualStyleBackColor = true;
            this.btnRiskTrade.Click += new System.EventHandler(this.btnRiskTrade_Click);
            // 
            // btnData
            // 
            this.btnData.Location = new System.Drawing.Point(442, 0);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(72, 36);
            this.btnData.TabIndex = 5;
            this.btnData.Text = "指标数据";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnOverView
            // 
            this.btnOverView.Location = new System.Drawing.Point(349, 0);
            this.btnOverView.Name = "btnOverView";
            this.btnOverView.Size = new System.Drawing.Size(75, 36);
            this.btnOverView.TabIndex = 4;
            this.btnOverView.Text = "监控概览";
            this.btnOverView.UseVisualStyleBackColor = true;
            this.btnOverView.Click += new System.EventHandler(this.btnOverView_Click);
            // 
            // lblConnectStatus
            // 
            this.lblConnectStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblConnectStatus.Location = new System.Drawing.Point(827, 16);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new System.Drawing.Size(89, 13);
            this.lblConnectStatus.TabIndex = 3;
            this.lblConnectStatus.Text = "连接标志";
            this.lblConnectStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightTitle.BackColor = System.Drawing.SystemColors.Control;
            this.lblRightTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRightTitle.Location = new System.Drawing.Point(651, 16);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(179, 13);
            this.lblRightTitle.TabIndex = 2;
            this.lblRightTitle.Text = "右标题";
            this.lblRightTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.SystemColors.Control;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.Location = new System.Drawing.Point(6, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(49, 19);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "标题";
            // 
            // panelClient
            // 
            this.panelClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClient.Location = new System.Drawing.Point(0, 36);
            this.panelClient.Name = "panelClient";
            this.panelClient.Size = new System.Drawing.Size(923, 398);
            this.panelClient.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 434);
            this.Controls.Add(this.panelClient);
            this.Controls.Add(this.panelTitle);
            this.Name = "MainForm";
            this.Text = "策略运行窗口";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblRightTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblConnectStatus;
        private System.Windows.Forms.Button btnRiskTrade;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Button btnOverView;
        private System.Windows.Forms.Panel panelClient;
    }
}


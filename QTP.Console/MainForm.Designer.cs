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
            this.panlTitle = new System.Windows.Forms.Panel();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabRun = new System.Windows.Forms.TabControl();
            this.pageMonitor = new System.Windows.Forms.TabPage();
            this.pageRiskM = new System.Windows.Forms.TabPage();
            this.monitorUC = new QTP.Console.MonitorUC();
            this.panlTitle.SuspendLayout();
            this.tabRun.SuspendLayout();
            this.pageMonitor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panlTitle
            // 
            this.panlTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panlTitle.Controls.Add(this.lblRightTitle);
            this.panlTitle.Controls.Add(this.lblTitle);
            this.panlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panlTitle.Location = new System.Drawing.Point(0, 0);
            this.panlTitle.Name = "panlTitle";
            this.panlTitle.Size = new System.Drawing.Size(778, 36);
            this.panlTitle.TabIndex = 1;
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRightTitle.Location = new System.Drawing.Point(594, 14);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(179, 13);
            this.lblRightTitle.TabIndex = 2;
            this.lblRightTitle.Text = "右标题";
            this.lblRightTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTitle.Location = new System.Drawing.Point(6, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(49, 19);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "标题";
            // 
            // tabRun
            // 
            this.tabRun.Controls.Add(this.pageMonitor);
            this.tabRun.Controls.Add(this.pageRiskM);
            this.tabRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRun.Location = new System.Drawing.Point(0, 36);
            this.tabRun.Name = "tabRun";
            this.tabRun.SelectedIndex = 0;
            this.tabRun.Size = new System.Drawing.Size(778, 398);
            this.tabRun.TabIndex = 3;
            // 
            // pageMonitor
            // 
            this.pageMonitor.Controls.Add(this.monitorUC);
            this.pageMonitor.Location = new System.Drawing.Point(4, 22);
            this.pageMonitor.Name = "pageMonitor";
            this.pageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.pageMonitor.Size = new System.Drawing.Size(770, 372);
            this.pageMonitor.TabIndex = 0;
            this.pageMonitor.Text = "监控品种";
            this.pageMonitor.UseVisualStyleBackColor = true;
            // 
            // pageRiskM
            // 
            this.pageRiskM.Location = new System.Drawing.Point(4, 22);
            this.pageRiskM.Name = "pageRiskM";
            this.pageRiskM.Padding = new System.Windows.Forms.Padding(3);
            this.pageRiskM.Size = new System.Drawing.Size(599, 230);
            this.pageRiskM.TabIndex = 1;
            this.pageRiskM.Text = "资金交易";
            this.pageRiskM.UseVisualStyleBackColor = true;
            // 
            // monitorUC
            // 
            this.monitorUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorUC.Location = new System.Drawing.Point(3, 3);
            this.monitorUC.Name = "monitorUC";
            this.monitorUC.Size = new System.Drawing.Size(764, 366);
            this.monitorUC.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 434);
            this.Controls.Add(this.tabRun);
            this.Controls.Add(this.panlTitle);
            this.Name = "MainForm";
            this.Text = "策略运行窗口";
            this.panlTitle.ResumeLayout(false);
            this.panlTitle.PerformLayout();
            this.tabRun.ResumeLayout(false);
            this.pageMonitor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panlTitle;
        private System.Windows.Forms.Label lblRightTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabRun;
        private System.Windows.Forms.TabPage pageMonitor;
        private MonitorUC monitorUC;
        private System.Windows.Forms.TabPage pageRiskM;
    }
}


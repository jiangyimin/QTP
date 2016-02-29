namespace QTP.Main
{
    partial class StrategyRunUC
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panlTitle = new System.Windows.Forms.Panel();
            this.tabRun = new System.Windows.Forms.TabControl();
            this.pageMonitor = new System.Windows.Forms.TabPage();
            this.pageRiskM = new System.Windows.Forms.TabPage();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.panlTitle.SuspendLayout();
            this.tabRun.SuspendLayout();
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
            this.panlTitle.Size = new System.Drawing.Size(718, 36);
            this.panlTitle.TabIndex = 0;
            // 
            // tabRun
            // 
            this.tabRun.Controls.Add(this.pageMonitor);
            this.tabRun.Controls.Add(this.pageRiskM);
            this.tabRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRun.Location = new System.Drawing.Point(0, 36);
            this.tabRun.Name = "tabRun";
            this.tabRun.SelectedIndex = 0;
            this.tabRun.Size = new System.Drawing.Size(718, 408);
            this.tabRun.TabIndex = 2;
            // 
            // pageMonitor
            // 
            this.pageMonitor.Location = new System.Drawing.Point(4, 22);
            this.pageMonitor.Name = "pageMonitor";
            this.pageMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.pageMonitor.Size = new System.Drawing.Size(710, 382);
            this.pageMonitor.TabIndex = 0;
            this.pageMonitor.Text = "监控品种";
            this.pageMonitor.UseVisualStyleBackColor = true;
            // 
            // pageRiskM
            // 
            this.pageRiskM.Location = new System.Drawing.Point(4, 22);
            this.pageRiskM.Name = "pageRiskM";
            this.pageRiskM.Padding = new System.Windows.Forms.Padding(3);
            this.pageRiskM.Size = new System.Drawing.Size(710, 382);
            this.pageRiskM.TabIndex = 1;
            this.pageRiskM.Text = "资金交易";
            this.pageRiskM.UseVisualStyleBackColor = true;
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
            // lblRightTitle
            // 
            this.lblRightTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRightTitle.Location = new System.Drawing.Point(534, 14);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(179, 13);
            this.lblRightTitle.TabIndex = 2;
            this.lblRightTitle.Text = "右标题";
            this.lblRightTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StrategyRunUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabRun);
            this.Controls.Add(this.panlTitle);
            this.Name = "StrategyRunUC";
            this.Size = new System.Drawing.Size(718, 444);
            this.panlTitle.ResumeLayout(false);
            this.panlTitle.PerformLayout();
            this.tabRun.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panlTitle;
        private System.Windows.Forms.TabControl tabRun;
        private System.Windows.Forms.TabPage pageMonitor;
        private System.Windows.Forms.TabPage pageRiskM;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRightTitle;

    }
}

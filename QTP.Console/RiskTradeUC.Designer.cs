namespace QTP.Console
{
    partial class RiskTradeUC
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitLog = new System.Windows.Forms.SplitContainer();
            this.boxMDLog = new System.Windows.Forms.ListBox();
            this.boxTDLog = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLog)).BeginInit();
            this.splitLog.Panel1.SuspendLayout();
            this.splitLog.Panel2.SuspendLayout();
            this.splitLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitLog);
            this.splitContainer1.Size = new System.Drawing.Size(659, 361);
            this.splitContainer1.SplitterDistance = 385;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitLog
            // 
            this.splitLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLog.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitLog.Location = new System.Drawing.Point(0, 0);
            this.splitLog.Name = "splitLog";
            this.splitLog.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitLog.Panel1
            // 
            this.splitLog.Panel1.Controls.Add(this.boxMDLog);
            // 
            // splitLog.Panel2
            // 
            this.splitLog.Panel2.Controls.Add(this.boxTDLog);
            this.splitLog.Size = new System.Drawing.Size(270, 361);
            this.splitLog.SplitterDistance = 271;
            this.splitLog.TabIndex = 0;
            // 
            // boxMDLog
            // 
            this.boxMDLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxMDLog.FormattingEnabled = true;
            this.boxMDLog.ItemHeight = 12;
            this.boxMDLog.Location = new System.Drawing.Point(0, 0);
            this.boxMDLog.Name = "boxMDLog";
            this.boxMDLog.Size = new System.Drawing.Size(270, 271);
            this.boxMDLog.TabIndex = 0;
            // 
            // boxTDLog
            // 
            this.boxTDLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxTDLog.FormattingEnabled = true;
            this.boxTDLog.ItemHeight = 12;
            this.boxTDLog.Location = new System.Drawing.Point(0, 0);
            this.boxTDLog.Name = "boxTDLog";
            this.boxTDLog.Size = new System.Drawing.Size(270, 86);
            this.boxTDLog.TabIndex = 1;
            // 
            // RiskTradeUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RiskTradeUC";
            this.Size = new System.Drawing.Size(659, 361);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitLog.Panel1.ResumeLayout(false);
            this.splitLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLog)).EndInit();
            this.splitLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitLog;
        private System.Windows.Forms.ListBox boxMDLog;
        private System.Windows.Forms.ListBox boxTDLog;

    }
}

namespace QTP.Console
{
    partial class MonitorUC
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblTarge = new System.Windows.Forms.Label();
            this.panelMD = new System.Windows.Forms.Panel();
            this.lblUnProcessTime = new System.Windows.Forms.Label();
            this.lblBar = new System.Windows.Forms.Label();
            this.lblTick = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.panelMD.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelTitle.Controls.Add(this.lblPrice);
            this.panelTitle.Controls.Add(this.lblTarge);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(280, 35);
            this.panelTitle.TabIndex = 1;
            // 
            // lblPrice
            // 
            this.lblPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrice.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrice.Location = new System.Drawing.Point(179, 10);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(95, 16);
            this.lblPrice.TabIndex = 1;
            this.lblPrice.Text = "最新价格";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTarge
            // 
            this.lblTarge.AutoSize = true;
            this.lblTarge.Location = new System.Drawing.Point(12, 10);
            this.lblTarge.Name = "lblTarge";
            this.lblTarge.Size = new System.Drawing.Size(53, 12);
            this.lblTarge.TabIndex = 0;
            this.lblTarge.Text = "监控目标";
            // 
            // panelMD
            // 
            this.panelMD.BackColor = System.Drawing.SystemColors.Info;
            this.panelMD.Controls.Add(this.lblUnProcessTime);
            this.panelMD.Controls.Add(this.lblBar);
            this.panelMD.Controls.Add(this.lblTick);
            this.panelMD.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMD.Location = new System.Drawing.Point(0, 141);
            this.panelMD.Name = "panelMD";
            this.panelMD.Size = new System.Drawing.Size(280, 41);
            this.panelMD.TabIndex = 2;
            // 
            // lblUnProcessTime
            // 
            this.lblUnProcessTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnProcessTime.Location = new System.Drawing.Point(216, 15);
            this.lblUnProcessTime.Name = "lblUnProcessTime";
            this.lblUnProcessTime.Size = new System.Drawing.Size(58, 12);
            this.lblUnProcessTime.TabIndex = 2;
            this.lblUnProcessTime.Text = "0秒";
            this.lblUnProcessTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUnProcessTime.Visible = false;
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point(71, 15);
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size(23, 12);
            this.lblBar.TabIndex = 1;
            this.lblBar.Text = "B:0";
            this.lblBar.Visible = false;
            // 
            // lblTick
            // 
            this.lblTick.AutoSize = true;
            this.lblTick.Location = new System.Drawing.Point(12, 15);
            this.lblTick.Name = "lblTick";
            this.lblTick.Size = new System.Drawing.Size(23, 12);
            this.lblTick.TabIndex = 0;
            this.lblTick.Text = "T:0";
            // 
            // MonitorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.panelMD);
            this.Controls.Add(this.panelTitle);
            this.Name = "MonitorUC";
            this.Size = new System.Drawing.Size(280, 182);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelMD.ResumeLayout(false);
            this.panelMD.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTarge;
        private System.Windows.Forms.Panel panelMD;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblUnProcessTime;
        private System.Windows.Forms.Label lblBar;
        private System.Windows.Forms.Label lblTick;
    }
}

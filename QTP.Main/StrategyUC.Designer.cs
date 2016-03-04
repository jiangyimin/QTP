namespace QTP.Main
{
    partial class StrategyUC
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
            this.btnDetail = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblTradeChannel = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupMonitor = new System.Windows.Forms.GroupBox();
            this.lblParameter1 = new System.Windows.Forms.Label();
            this.lblClassName1 = new System.Windows.Forms.Label();
            this.groupRiskM = new System.Windows.Forms.GroupBox();
            this.lblParameter2 = new System.Windows.Forms.Label();
            this.lblClassName2 = new System.Windows.Forms.Label();
            this.panelTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupMonitor.SuspendLayout();
            this.groupRiskM.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.SystemColors.Highlight;
            this.panelTitle.Controls.Add(this.btnDetail);
            this.panelTitle.Controls.Add(this.lblName);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(339, 35);
            this.panelTitle.TabIndex = 0;
            // 
            // btnDetail
            // 
            this.btnDetail.Enabled = false;
            this.btnDetail.Location = new System.Drawing.Point(254, 0);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(85, 35);
            this.btnDetail.TabIndex = 1;
            this.btnDetail.Text = "运行详情";
            this.btnDetail.UseVisualStyleBackColor = true;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 10);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(29, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "名称";
            // 
            // lblTradeChannel
            // 
            this.lblTradeChannel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTradeChannel.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTradeChannel.Location = new System.Drawing.Point(181, 26);
            this.lblTradeChannel.Name = "lblTradeChannel";
            this.lblTradeChannel.Size = new System.Drawing.Size(149, 12);
            this.lblTradeChannel.TabIndex = 1;
            this.lblTradeChannel.Text = "交易通道";
            this.lblTradeChannel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNum
            // 
            this.lblNum.Location = new System.Drawing.Point(249, 26);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(79, 12);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "监控数量";
            this.lblNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Location = new System.Drawing.Point(0, 201);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 55);
            this.panel1.TabIndex = 4;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(96, 11);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(130, 35);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "打开";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // groupMonitor
            // 
            this.groupMonitor.Controls.Add(this.lblParameter1);
            this.groupMonitor.Controls.Add(this.lblClassName1);
            this.groupMonitor.Controls.Add(this.lblNum);
            this.groupMonitor.Location = new System.Drawing.Point(0, 37);
            this.groupMonitor.Name = "groupMonitor";
            this.groupMonitor.Size = new System.Drawing.Size(339, 78);
            this.groupMonitor.TabIndex = 5;
            this.groupMonitor.TabStop = false;
            // 
            // lblParameter1
            // 
            this.lblParameter1.AutoSize = true;
            this.lblParameter1.Location = new System.Drawing.Point(24, 49);
            this.lblParameter1.Name = "lblParameter1";
            this.lblParameter1.Size = new System.Drawing.Size(29, 12);
            this.lblParameter1.TabIndex = 2;
            this.lblParameter1.Text = "参数";
            // 
            // lblClassName1
            // 
            this.lblClassName1.AutoSize = true;
            this.lblClassName1.Location = new System.Drawing.Point(24, 26);
            this.lblClassName1.Name = "lblClassName1";
            this.lblClassName1.Size = new System.Drawing.Size(29, 12);
            this.lblClassName1.TabIndex = 1;
            this.lblClassName1.Text = "类名";
            // 
            // groupRiskM
            // 
            this.groupRiskM.Controls.Add(this.lblTradeChannel);
            this.groupRiskM.Controls.Add(this.lblParameter2);
            this.groupRiskM.Controls.Add(this.lblClassName2);
            this.groupRiskM.Location = new System.Drawing.Point(0, 117);
            this.groupRiskM.Name = "groupRiskM";
            this.groupRiskM.Size = new System.Drawing.Size(339, 78);
            this.groupRiskM.TabIndex = 6;
            this.groupRiskM.TabStop = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.AutoSize = true;
            this.lblParameter2.Location = new System.Drawing.Point(24, 49);
            this.lblParameter2.Name = "lblParameter2";
            this.lblParameter2.Size = new System.Drawing.Size(29, 12);
            this.lblParameter2.TabIndex = 2;
            this.lblParameter2.Text = "参数";
            // 
            // lblClassName2
            // 
            this.lblClassName2.AutoSize = true;
            this.lblClassName2.Location = new System.Drawing.Point(24, 26);
            this.lblClassName2.Name = "lblClassName2";
            this.lblClassName2.Size = new System.Drawing.Size(29, 12);
            this.lblClassName2.TabIndex = 1;
            this.lblClassName2.Text = "类名";
            // 
            // StrategyUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.groupRiskM);
            this.Controls.Add(this.groupMonitor);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTitle);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "StrategyUC";
            this.Size = new System.Drawing.Size(339, 256);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupMonitor.ResumeLayout(false);
            this.groupMonitor.PerformLayout();
            this.groupRiskM.ResumeLayout(false);
            this.groupRiskM.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTradeChannel;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.GroupBox groupMonitor;
        private System.Windows.Forms.Label lblClassName1;
        private System.Windows.Forms.Label lblParameter1;
        private System.Windows.Forms.GroupBox groupRiskM;
        private System.Windows.Forms.Label lblParameter2;
        private System.Windows.Forms.Label lblClassName2;
    }
}

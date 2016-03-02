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
            this.panelTrade = new System.Windows.Forms.Panel();
            this.lblTradeChannel = new System.Windows.Forms.Label();
            this.lblMonitors = new System.Windows.Forms.Label();
            this.listException = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.panelTitle.SuspendLayout();
            this.panelTrade.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.btnDetail.Location = new System.Drawing.Point(283, 3);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(53, 29);
            this.btnDetail.TabIndex = 1;
            this.btnDetail.Text = "详情";
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
            // panelTrade
            // 
            this.panelTrade.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelTrade.Controls.Add(this.lblTradeChannel);
            this.panelTrade.Controls.Add(this.lblMonitors);
            this.panelTrade.Location = new System.Drawing.Point(198, 201);
            this.panelTrade.Name = "panelTrade";
            this.panelTrade.Size = new System.Drawing.Size(141, 55);
            this.panelTrade.TabIndex = 2;
            // 
            // lblTradeChannel
            // 
            this.lblTradeChannel.AutoSize = true;
            this.lblTradeChannel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTradeChannel.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTradeChannel.Location = new System.Drawing.Point(5, 32);
            this.lblTradeChannel.Name = "lblTradeChannel";
            this.lblTradeChannel.Size = new System.Drawing.Size(53, 12);
            this.lblTradeChannel.TabIndex = 1;
            this.lblTradeChannel.Text = "交易通道";
            // 
            // lblMonitors
            // 
            this.lblMonitors.AutoSize = true;
            this.lblMonitors.Location = new System.Drawing.Point(5, 9);
            this.lblMonitors.Name = "lblMonitors";
            this.lblMonitors.Size = new System.Drawing.Size(53, 12);
            this.lblMonitors.TabIndex = 0;
            this.lblMonitors.Text = "监控数量";
            // 
            // listException
            // 
            this.listException.BackColor = System.Drawing.SystemColors.Window;
            this.listException.FormattingEnabled = true;
            this.listException.ItemHeight = 12;
            this.listException.Location = new System.Drawing.Point(0, 37);
            this.listException.Name = "listException";
            this.listException.Size = new System.Drawing.Size(339, 160);
            this.listException.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Location = new System.Drawing.Point(0, 201);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 55);
            this.panel1.TabIndex = 4;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(64, 9);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(69, 35);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // StrategyUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listException);
            this.Controls.Add(this.panelTrade);
            this.Controls.Add(this.panelTitle);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "StrategyUC";
            this.Size = new System.Drawing.Size(339, 256);
            this.Load += new System.EventHandler(this.StrategyUC_Load);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelTrade.ResumeLayout(false);
            this.panelTrade.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Panel panelTrade;
        private System.Windows.Forms.Label lblTradeChannel;
        private System.Windows.Forms.Label lblMonitors;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.ListBox listException;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnConnect;
    }
}

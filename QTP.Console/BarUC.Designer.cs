namespace QTP.Console
{
    partial class BarUC
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.boxBars = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.boxOrders = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFactor = new System.Windows.Forms.Label();
            this.lblVolumn = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblOpen = new System.Windows.Forms.Label();
            this.lblLow = new System.Windows.Forms.Label();
            this.lblHigh = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.boxBars);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 72);
            this.panel1.TabIndex = 0;
            // 
            // boxBars
            // 
            this.boxBars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxBars.FormattingEnabled = true;
            this.boxBars.ItemHeight = 12;
            this.boxBars.Location = new System.Drawing.Point(0, 0);
            this.boxBars.Name = "boxBars";
            this.boxBars.Size = new System.Drawing.Size(199, 72);
            this.boxBars.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.boxOrders);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(199, 180);
            this.panel2.TabIndex = 2;
            // 
            // boxOrders
            // 
            this.boxOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxOrders.FormattingEnabled = true;
            this.boxOrders.ItemHeight = 12;
            this.boxOrders.Location = new System.Drawing.Point(0, 83);
            this.boxOrders.Name = "boxOrders";
            this.boxOrders.Size = new System.Drawing.Size(199, 97);
            this.boxOrders.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblFactor);
            this.groupBox1.Controls.Add(this.lblVolumn);
            this.groupBox1.Controls.Add(this.lblClose);
            this.groupBox1.Controls.Add(this.lblOpen);
            this.groupBox1.Controls.Add(this.lblLow);
            this.groupBox1.Controls.Add(this.lblHigh);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 83);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "交易记录";
            // 
            // lblFactor
            // 
            this.lblFactor.AutoSize = true;
            this.lblFactor.Location = new System.Drawing.Point(97, 62);
            this.lblFactor.Name = "lblFactor";
            this.lblFactor.Size = new System.Drawing.Size(29, 12);
            this.lblFactor.TabIndex = 5;
            this.lblFactor.Text = "因子";
            // 
            // lblVolumn
            // 
            this.lblVolumn.AutoSize = true;
            this.lblVolumn.Location = new System.Drawing.Point(15, 62);
            this.lblVolumn.Name = "lblVolumn";
            this.lblVolumn.Size = new System.Drawing.Size(17, 12);
            this.lblVolumn.TabIndex = 4;
            this.lblVolumn.Text = "量";
            // 
            // lblClose
            // 
            this.lblClose.AutoSize = true;
            this.lblClose.Location = new System.Drawing.Point(97, 39);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(17, 12);
            this.lblClose.TabIndex = 3;
            this.lblClose.Text = "收";
            // 
            // lblOpen
            // 
            this.lblOpen.AutoSize = true;
            this.lblOpen.Location = new System.Drawing.Point(15, 39);
            this.lblOpen.Name = "lblOpen";
            this.lblOpen.Size = new System.Drawing.Size(17, 12);
            this.lblOpen.TabIndex = 2;
            this.lblOpen.Text = "开";
            // 
            // lblLow
            // 
            this.lblLow.AutoSize = true;
            this.lblLow.Location = new System.Drawing.Point(97, 17);
            this.lblLow.Name = "lblLow";
            this.lblLow.Size = new System.Drawing.Size(17, 12);
            this.lblLow.TabIndex = 1;
            this.lblLow.Text = "低";
            // 
            // lblHigh
            // 
            this.lblHigh.AutoSize = true;
            this.lblHigh.Location = new System.Drawing.Point(15, 17);
            this.lblHigh.Name = "lblHigh";
            this.lblHigh.Size = new System.Drawing.Size(17, 12);
            this.lblHigh.TabIndex = 0;
            this.lblHigh.Text = "高";
            // 
            // BarUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "BarUC";
            this.Size = new System.Drawing.Size(199, 252);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblHigh;
        private System.Windows.Forms.Label lblFactor;
        private System.Windows.Forms.Label lblVolumn;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblOpen;
        private System.Windows.Forms.Label lblLow;
        private System.Windows.Forms.ListBox boxBars;
        private System.Windows.Forms.ListBox boxOrders;
    }
}

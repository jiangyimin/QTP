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
            this.lblBarTime = new System.Windows.Forms.Label();
            this.lblArrivedTime = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStrTime = new System.Windows.Forms.Label();
            this.lblVolumnT = new System.Windows.Forms.Label();
            this.lblCloseT = new System.Windows.Forms.Label();
            this.lblOpenT = new System.Windows.Forms.Label();
            this.lblLowT = new System.Windows.Forms.Label();
            this.lblHighT = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFactor = new System.Windows.Forms.Label();
            this.lblVolumn = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblOpen = new System.Windows.Forms.Label();
            this.lblLow = new System.Windows.Forms.Label();
            this.lblHigh = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblBarTime);
            this.panel1.Controls.Add(this.lblArrivedTime);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 48);
            this.panel1.TabIndex = 0;
            // 
            // lblBarTime
            // 
            this.lblBarTime.AutoSize = true;
            this.lblBarTime.Location = new System.Drawing.Point(15, 31);
            this.lblBarTime.Name = "lblBarTime";
            this.lblBarTime.Size = new System.Drawing.Size(47, 12);
            this.lblBarTime.TabIndex = 1;
            this.lblBarTime.Text = "Bar时间";
            // 
            // lblArrivedTime
            // 
            this.lblArrivedTime.AutoSize = true;
            this.lblArrivedTime.Location = new System.Drawing.Point(15, 9);
            this.lblArrivedTime.Name = "lblArrivedTime";
            this.lblArrivedTime.Size = new System.Drawing.Size(29, 12);
            this.lblArrivedTime.TabIndex = 0;
            this.lblArrivedTime.Text = "到达";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(199, 227);
            this.panel2.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStrTime);
            this.groupBox2.Controls.Add(this.lblVolumnT);
            this.groupBox2.Controls.Add(this.lblCloseT);
            this.groupBox2.Controls.Add(this.lblOpenT);
            this.groupBox2.Controls.Add(this.lblLowT);
            this.groupBox2.Controls.Add(this.lblHighT);
            this.groupBox2.Location = new System.Drawing.Point(0, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 85);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "TickBar(1M)";
            // 
            // lblStrTime
            // 
            this.lblStrTime.AutoSize = true;
            this.lblStrTime.Location = new System.Drawing.Point(97, 62);
            this.lblStrTime.Name = "lblStrTime";
            this.lblStrTime.Size = new System.Drawing.Size(29, 12);
            this.lblStrTime.TabIndex = 5;
            this.lblStrTime.Text = "时间";
            // 
            // lblVolumnT
            // 
            this.lblVolumnT.AutoSize = true;
            this.lblVolumnT.Location = new System.Drawing.Point(15, 62);
            this.lblVolumnT.Name = "lblVolumnT";
            this.lblVolumnT.Size = new System.Drawing.Size(17, 12);
            this.lblVolumnT.TabIndex = 4;
            this.lblVolumnT.Text = "量";
            // 
            // lblCloseT
            // 
            this.lblCloseT.AutoSize = true;
            this.lblCloseT.Location = new System.Drawing.Point(97, 39);
            this.lblCloseT.Name = "lblCloseT";
            this.lblCloseT.Size = new System.Drawing.Size(17, 12);
            this.lblCloseT.TabIndex = 3;
            this.lblCloseT.Text = "收";
            // 
            // lblOpenT
            // 
            this.lblOpenT.AutoSize = true;
            this.lblOpenT.Location = new System.Drawing.Point(15, 39);
            this.lblOpenT.Name = "lblOpenT";
            this.lblOpenT.Size = new System.Drawing.Size(17, 12);
            this.lblOpenT.TabIndex = 2;
            this.lblOpenT.Text = "开";
            // 
            // lblLowT
            // 
            this.lblLowT.AutoSize = true;
            this.lblLowT.Location = new System.Drawing.Point(97, 17);
            this.lblLowT.Name = "lblLowT";
            this.lblLowT.Size = new System.Drawing.Size(17, 12);
            this.lblLowT.TabIndex = 1;
            this.lblLowT.Text = "低";
            // 
            // lblHighT
            // 
            this.lblHighT.AutoSize = true;
            this.lblHighT.Location = new System.Drawing.Point(15, 17);
            this.lblHighT.Name = "lblHighT";
            this.lblHighT.Size = new System.Drawing.Size(17, 12);
            this.lblHighT.TabIndex = 0;
            this.lblHighT.Text = "高";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblFactor);
            this.groupBox1.Controls.Add(this.lblVolumn);
            this.groupBox1.Controls.Add(this.lblClose);
            this.groupBox1.Controls.Add(this.lblOpen);
            this.groupBox1.Controls.Add(this.lblLow);
            this.groupBox1.Controls.Add(this.lblHigh);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 85);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bar(1M)";
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
            this.Size = new System.Drawing.Size(199, 275);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblHigh;
        private System.Windows.Forms.Label lblBarTime;
        private System.Windows.Forms.Label lblArrivedTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblStrTime;
        private System.Windows.Forms.Label lblVolumnT;
        private System.Windows.Forms.Label lblCloseT;
        private System.Windows.Forms.Label lblOpenT;
        private System.Windows.Forms.Label lblLowT;
        private System.Windows.Forms.Label lblHighT;
        private System.Windows.Forms.Label lblFactor;
        private System.Windows.Forms.Label lblVolumn;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblOpen;
        private System.Windows.Forms.Label lblLow;
    }
}

namespace QTP.Console
{
    partial class MonitorDataUC
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
            this.dgvPool = new System.Windows.Forms.DataGridView();
            this.panelTool = new System.Windows.Forms.Panel();
            this.btnM5 = new System.Windows.Forms.Button();
            this.btnM60 = new System.Windows.Forms.Button();
            this.btnM30 = new System.Windows.Forms.Button();
            this.btnM1 = new System.Windows.Forms.Button();
            this.btnM15 = new System.Windows.Forms.Button();
            this.btnDay = new System.Windows.Forms.Button();
            this.chartUC = new QTP.Console.ChartUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.barUC = new QTP.Console.BarUC();
            this.tickUC = new QTP.Console.TickUC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPool)).BeginInit();
            this.panelTool.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvPool);
            this.splitContainer1.Panel1.Controls.Add(this.panelTool);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chartUC);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(758, 621);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvPool
            // 
            this.dgvPool.AllowUserToAddRows = false;
            this.dgvPool.AllowUserToDeleteRows = false;
            this.dgvPool.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPool.Location = new System.Drawing.Point(48, 0);
            this.dgvPool.MultiSelect = false;
            this.dgvPool.Name = "dgvPool";
            this.dgvPool.ReadOnly = true;
            this.dgvPool.RowTemplate.Height = 23;
            this.dgvPool.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPool.Size = new System.Drawing.Size(710, 145);
            this.dgvPool.TabIndex = 3;
            this.dgvPool.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPool_CellEnter);
            // 
            // panelTool
            // 
            this.panelTool.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelTool.Controls.Add(this.btnM5);
            this.panelTool.Controls.Add(this.btnM60);
            this.panelTool.Controls.Add(this.btnM30);
            this.panelTool.Controls.Add(this.btnM1);
            this.panelTool.Controls.Add(this.btnM15);
            this.panelTool.Controls.Add(this.btnDay);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTool.Location = new System.Drawing.Point(0, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(48, 145);
            this.panelTool.TabIndex = 2;
            // 
            // btnM5
            // 
            this.btnM5.Location = new System.Drawing.Point(6, 98);
            this.btnM5.Name = "btnM5";
            this.btnM5.Size = new System.Drawing.Size(36, 22);
            this.btnM5.TabIndex = 10;
            this.btnM5.Text = "5M";
            this.btnM5.UseVisualStyleBackColor = true;
            this.btnM5.Click += new System.EventHandler(this.btnM5_Click);
            // 
            // btnM60
            // 
            this.btnM60.Location = new System.Drawing.Point(6, 29);
            this.btnM60.Name = "btnM60";
            this.btnM60.Size = new System.Drawing.Size(36, 23);
            this.btnM60.TabIndex = 9;
            this.btnM60.Text = "60M";
            this.btnM60.UseVisualStyleBackColor = true;
            this.btnM60.Click += new System.EventHandler(this.btnM60_Click);
            // 
            // btnM30
            // 
            this.btnM30.Location = new System.Drawing.Point(6, 52);
            this.btnM30.Name = "btnM30";
            this.btnM30.Size = new System.Drawing.Size(36, 22);
            this.btnM30.TabIndex = 8;
            this.btnM30.Text = "30M";
            this.btnM30.UseVisualStyleBackColor = true;
            this.btnM30.Click += new System.EventHandler(this.btnM30_Click);
            // 
            // btnM1
            // 
            this.btnM1.Location = new System.Drawing.Point(6, 121);
            this.btnM1.Name = "btnM1";
            this.btnM1.Size = new System.Drawing.Size(36, 22);
            this.btnM1.TabIndex = 6;
            this.btnM1.Text = "1M";
            this.btnM1.UseVisualStyleBackColor = true;
            this.btnM1.Click += new System.EventHandler(this.btnM1_Click);
            // 
            // btnM15
            // 
            this.btnM15.Location = new System.Drawing.Point(6, 74);
            this.btnM15.Name = "btnM15";
            this.btnM15.Size = new System.Drawing.Size(36, 23);
            this.btnM15.TabIndex = 4;
            this.btnM15.Text = "15M";
            this.btnM15.UseVisualStyleBackColor = true;
            this.btnM15.Click += new System.EventHandler(this.btnM15_Click);
            // 
            // btnDay
            // 
            this.btnDay.Location = new System.Drawing.Point(6, 5);
            this.btnDay.Name = "btnDay";
            this.btnDay.Size = new System.Drawing.Size(36, 23);
            this.btnDay.TabIndex = 0;
            this.btnDay.Text = "日";
            this.btnDay.UseVisualStyleBackColor = true;
            this.btnDay.Click += new System.EventHandler(this.btnDay_Click);
            // 
            // chartUC
            // 
            this.chartUC.BackColor = System.Drawing.Color.Black;
            this.chartUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartUC.Location = new System.Drawing.Point(0, 0);
            this.chartUC.Name = "chartUC";
            this.chartUC.Size = new System.Drawing.Size(558, 472);
            this.chartUC.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.barUC);
            this.panel1.Controls.Add(this.tickUC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(558, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 472);
            this.panel1.TabIndex = 2;
            // 
            // barUC
            // 
            this.barUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.barUC.Location = new System.Drawing.Point(0, 397);
            this.barUC.Name = "barUC";
            this.barUC.Size = new System.Drawing.Size(200, 75);
            this.barUC.TabIndex = 1;
            // 
            // tickUC
            // 
            this.tickUC.Dock = System.Windows.Forms.DockStyle.Top;
            this.tickUC.Location = new System.Drawing.Point(0, 0);
            this.tickUC.Name = "tickUC";
            this.tickUC.Size = new System.Drawing.Size(200, 397);
            this.tickUC.TabIndex = 0;
            // 
            // MonitorDataUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MonitorDataUC";
            this.Size = new System.Drawing.Size(758, 621);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPool)).EndInit();
            this.panelTool.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ChartUC chartUC;
        private System.Windows.Forms.Panel panel1;
        private BarUC barUC;
        private TickUC tickUC;
        private System.Windows.Forms.DataGridView dgvPool;
        private System.Windows.Forms.Panel panelTool;
        private System.Windows.Forms.Button btnM5;
        private System.Windows.Forms.Button btnM60;
        private System.Windows.Forms.Button btnM30;
        private System.Windows.Forms.Button btnM1;
        private System.Windows.Forms.Button btnM15;
        private System.Windows.Forms.Button btnDay;
    }
}

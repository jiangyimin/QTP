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
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartUC = new QTP.Console.ChartUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.barUC = new QTP.Console.BarUC();
            this.tickUC = new QTP.Console.TickUC();
            this.panelTool = new System.Windows.Forms.Panel();
            this.btnM1 = new System.Windows.Forms.Button();
            this.btnM15 = new System.Windows.Forms.Button();
            this.btnDay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPool)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelTool.SuspendLayout();
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chartUC);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.panelTool);
            this.splitContainer1.Size = new System.Drawing.Size(758, 621);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvPool
            // 
            this.dgvPool.AllowUserToAddRows = false;
            this.dgvPool.AllowUserToDeleteRows = false;
            this.dgvPool.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPool.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.dgvPool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPool.Location = new System.Drawing.Point(0, 0);
            this.dgvPool.MultiSelect = false;
            this.dgvPool.Name = "dgvPool";
            this.dgvPool.ReadOnly = true;
            this.dgvPool.RowTemplate.Height = 23;
            this.dgvPool.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPool.Size = new System.Drawing.Size(758, 145);
            this.dgvPool.TabIndex = 1;
            this.dgvPool.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPool_ColumnHeaderMouseDoubleClick);
            this.dgvPool.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPool_RowEnter);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "交易所";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "代码";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "名称";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // chartUC
            // 
            this.chartUC.BackColor = System.Drawing.Color.Black;
            this.chartUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartUC.Location = new System.Drawing.Point(48, 0);
            this.chartUC.Name = "chartUC";
            this.chartUC.ScalarProps = null;
            this.chartUC.Size = new System.Drawing.Size(510, 472);
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
            // panelTool
            // 
            this.panelTool.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelTool.Controls.Add(this.btnM1);
            this.panelTool.Controls.Add(this.btnM15);
            this.panelTool.Controls.Add(this.btnDay);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTool.Location = new System.Drawing.Point(0, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(48, 472);
            this.panelTool.TabIndex = 1;
            // 
            // btnM1
            // 
            this.btnM1.Location = new System.Drawing.Point(6, 99);
            this.btnM1.Name = "btnM1";
            this.btnM1.Size = new System.Drawing.Size(36, 36);
            this.btnM1.TabIndex = 6;
            this.btnM1.Text = "1M";
            this.btnM1.UseVisualStyleBackColor = true;
            this.btnM1.Click += new System.EventHandler(this.btnM1_Click);
            // 
            // btnM15
            // 
            this.btnM15.Location = new System.Drawing.Point(6, 52);
            this.btnM15.Name = "btnM15";
            this.btnM15.Size = new System.Drawing.Size(36, 36);
            this.btnM15.TabIndex = 4;
            this.btnM15.Text = "15M";
            this.btnM15.UseVisualStyleBackColor = true;
            this.btnM15.Click += new System.EventHandler(this.btnM15_Click);
            // 
            // btnDay
            // 
            this.btnDay.Location = new System.Drawing.Point(6, 6);
            this.btnDay.Name = "btnDay";
            this.btnDay.Size = new System.Drawing.Size(36, 36);
            this.btnDay.TabIndex = 0;
            this.btnDay.Text = "日";
            this.btnDay.UseVisualStyleBackColor = true;
            this.btnDay.Click += new System.EventHandler(this.btnDay_Click);
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
            this.panel1.ResumeLayout(false);
            this.panelTool.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvPool;
        private System.Windows.Forms.Panel panelTool;
        private ChartUC chartUC;
        private System.Windows.Forms.Panel panel1;
        private BarUC barUC;
        private TickUC tickUC;
        private System.Windows.Forms.Button btnM1;
        private System.Windows.Forms.Button btnM15;
        private System.Windows.Forms.Button btnDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}

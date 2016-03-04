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
            this.panelTool = new System.Windows.Forms.Panel();
            this.tickUC = new QTP.Console.TickUC();
            this.chartUC = new QTP.Console.ChartUC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPool)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.tickUC);
            this.splitContainer1.Panel2.Controls.Add(this.panelTool);
            this.splitContainer1.Size = new System.Drawing.Size(758, 384);
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
            this.dgvPool.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPool_RowEnter);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 40;
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
            // panelTool
            // 
            this.panelTool.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTool.Location = new System.Drawing.Point(0, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(48, 235);
            this.panelTool.TabIndex = 1;
            // 
            // tickUC
            // 
            this.tickUC.Dock = System.Windows.Forms.DockStyle.Right;
            this.tickUC.Location = new System.Drawing.Point(548, 0);
            this.tickUC.Name = "tickUC";
            this.tickUC.Size = new System.Drawing.Size(210, 235);
            this.tickUC.TabIndex = 2;
            // 
            // chartUC
            // 
            this.chartUC.BackColor = System.Drawing.Color.Black;
            this.chartUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartUC.Location = new System.Drawing.Point(48, 0);
            this.chartUC.Name = "chartUC";
            this.chartUC.Size = new System.Drawing.Size(500, 235);
            this.chartUC.TabIndex = 3;
            // 
            // MonitorDataUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MonitorDataUC";
            this.Size = new System.Drawing.Size(758, 384);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPool)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvPool;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Panel panelTool;
        private TickUC tickUC;
        private ChartUC chartUC;
    }
}

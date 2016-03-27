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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tradeUC = new QTP.Console.TradeUC();
            this.accountUC = new QTP.Console.AccountUC();
            this.splitLog = new System.Windows.Forms.SplitContainer();
            this.boxMDLog = new System.Windows.Forms.ListBox();
            this.boxTDLog = new System.Windows.Forms.ListBox();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清理TDLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.掘金持仓ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.web持仓ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLog)).BeginInit();
            this.splitLog.Panel1.SuspendLayout();
            this.splitLog.Panel2.SuspendLayout();
            this.splitLog.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tradeUC);
            this.splitContainer1.Panel1.Controls.Add(this.accountUC);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitLog);
            this.splitContainer1.Size = new System.Drawing.Size(1146, 589);
            this.splitContainer1.SplitterDistance = 823;
            this.splitContainer1.TabIndex = 0;
            // 
            // tradeUC
            // 
            this.tradeUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeUC.Location = new System.Drawing.Point(0, 362);
            this.tradeUC.Name = "tradeUC";
            this.tradeUC.Size = new System.Drawing.Size(823, 227);
            this.tradeUC.TabIndex = 1;
            // 
            // accountUC
            // 
            this.accountUC.Dock = System.Windows.Forms.DockStyle.Top;
            this.accountUC.Location = new System.Drawing.Point(0, 0);
            this.accountUC.Name = "accountUC";
            this.accountUC.Size = new System.Drawing.Size(823, 362);
            this.accountUC.TabIndex = 0;
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
            this.splitLog.Size = new System.Drawing.Size(319, 589);
            this.splitLog.SplitterDistance = 330;
            this.splitLog.TabIndex = 0;
            // 
            // boxMDLog
            // 
            this.boxMDLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxMDLog.FormattingEnabled = true;
            this.boxMDLog.ItemHeight = 12;
            this.boxMDLog.Location = new System.Drawing.Point(0, 0);
            this.boxMDLog.Name = "boxMDLog";
            this.boxMDLog.Size = new System.Drawing.Size(319, 330);
            this.boxMDLog.TabIndex = 0;
            // 
            // boxTDLog
            // 
            this.boxTDLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxTDLog.FormattingEnabled = true;
            this.boxTDLog.ItemHeight = 12;
            this.boxTDLog.Location = new System.Drawing.Point(0, 0);
            this.boxTDLog.Name = "boxTDLog";
            this.boxTDLog.Size = new System.Drawing.Size(319, 255);
            this.boxTDLog.TabIndex = 1;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清理ToolStripMenuItem,
            this.清理TDLogToolStripMenuItem,
            this.toolStripMenuItem1,
            this.掘金持仓ToolStripMenuItem,
            this.web持仓ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(153, 126);
            // 
            // 清理ToolStripMenuItem
            // 
            this.清理ToolStripMenuItem.Name = "清理ToolStripMenuItem";
            this.清理ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清理ToolStripMenuItem.Text = "清理MDLog";
            this.清理ToolStripMenuItem.Click += new System.EventHandler(this.清理MDLogToolStripMenuItem_Click);
            // 
            // 清理TDLogToolStripMenuItem
            // 
            this.清理TDLogToolStripMenuItem.Name = "清理TDLogToolStripMenuItem";
            this.清理TDLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.清理TDLogToolStripMenuItem.Text = "清理TDLog";
            this.清理TDLogToolStripMenuItem.Click += new System.EventHandler(this.清理TDLogToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // 掘金持仓ToolStripMenuItem
            // 
            this.掘金持仓ToolStripMenuItem.Name = "掘金持仓ToolStripMenuItem";
            this.掘金持仓ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.掘金持仓ToolStripMenuItem.Text = "掘金持仓";
            this.掘金持仓ToolStripMenuItem.Click += new System.EventHandler(this.掘金持仓ToolStripMenuItem_Click);
            // 
            // web持仓ToolStripMenuItem
            // 
            this.web持仓ToolStripMenuItem.Name = "web持仓ToolStripMenuItem";
            this.web持仓ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.web持仓ToolStripMenuItem.Text = "Web持仓";
            this.web持仓ToolStripMenuItem.Click += new System.EventHandler(this.web持仓ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // RiskTradeUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RiskTradeUC";
            this.Size = new System.Drawing.Size(1146, 589);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitLog.Panel1.ResumeLayout(false);
            this.splitLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLog)).EndInit();
            this.splitLog.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitLog;
        private System.Windows.Forms.ListBox boxMDLog;
        private System.Windows.Forms.ListBox boxTDLog;
        private AccountUC accountUC;
        private TradeUC tradeUC;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem 清理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清理TDLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 掘金持仓ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem web持仓ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;

    }
}

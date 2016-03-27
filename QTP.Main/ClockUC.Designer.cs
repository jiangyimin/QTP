namespace QTP.Main
{
    partial class ClockUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.lblDate = new System.Windows.Forms.Label();
            this.lblMH = new System.Windows.Forms.Label();
            this.lblSecond = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.Window;
            this.lblDate.Location = new System.Drawing.Point(11, 18);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(35, 14);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "日期";
            // 
            // lblMH
            // 
            this.lblMH.AutoSize = true;
            this.lblMH.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMH.ForeColor = System.Drawing.SystemColors.Window;
            this.lblMH.Location = new System.Drawing.Point(12, 55);
            this.lblMH.Name = "lblMH";
            this.lblMH.Size = new System.Drawing.Size(35, 14);
            this.lblMH.TabIndex = 1;
            this.lblMH.Text = "时分";
            // 
            // lblSecond
            // 
            this.lblSecond.AutoSize = true;
            this.lblSecond.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSecond.ForeColor = System.Drawing.SystemColors.Window;
            this.lblSecond.Location = new System.Drawing.Point(10, 89);
            this.lblSecond.Name = "lblSecond";
            this.lblSecond.Size = new System.Drawing.Size(21, 14);
            this.lblSecond.TabIndex = 2;
            this.lblSecond.Text = "秒";
            // 
            // ClockUCcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.lblSecond);
            this.Controls.Add(this.lblMH);
            this.Controls.Add(this.lblDate);
            this.Name = "ClockUCcs";
            this.Size = new System.Drawing.Size(61, 129);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblMH;
        private System.Windows.Forms.Label lblSecond;
    }
}

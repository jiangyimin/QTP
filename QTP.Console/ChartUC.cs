using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTP.Console
{
    public partial class ChartUC : UserControl
    {
        public ChartUC()
        {
            InitializeComponent();

            // Set double buffer
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics s = e.Graphics;
            s.Clear(Color.Aqua);
            //DrawCaption(e.Graphics);
            base.OnPaint(e);
        }

    }
}

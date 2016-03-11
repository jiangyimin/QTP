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
        #region member
        private Point mouseLocation;
        private Pen dashRedPen;


        #endregion
        public ChartUC()
        {
            InitializeComponent();

            // Set double buffer
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            dashRedPen = new Pen(Color.Red);
            dashRedPen.DashStyle = DashStyle.Dot;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics, e.ClipRectangle);
            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            mouseLocation = e.Location;
            Invalidate();
            
            base.OnMouseMove(e);
        }

        #region Draw
        private void Draw(Graphics g, Rectangle rect)
        {
            // frame
            DrawFrame(g, rect);
            
            if (mouseLocation != null)
            {
                g.DrawLine(Pens.White, new Point(0, mouseLocation.Y), new Point(rect.Right, mouseLocation.Y));     // h
                g.DrawLine(Pens.White, new Point(mouseLocation.X, 0), new Point(mouseLocation.X, rect.Bottom));     // v

            }
            // g.DrawString("K线图", new Font())
        }


        private void DrawFrame(Graphics g, Rectangle rect)
        {
            Rectangle r = new Rectangle(0, 0, rect.Width - 1, rect.Height - 1);
            g.DrawRectangle(Pens.Red, r);
            g.DrawLine(dashRedPen, new Point(0, 50), new Point(rect.Right, 50));
            g.DrawLine(dashRedPen, new Point(0, 100), new Point(rect.Right, 100));
            g.DrawLine(dashRedPen, new Point(0, 150), new Point(rect.Right, 150));
        }
    
        #endregion
    }
}

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
using System.Reflection;

using QTP.Domain;
using QTP.TAlib;

namespace QTP.Console
{
    public partial class ChartUC : UserControl
    {
        #region Consts
        // Xs
        private const int KWidth = 7;
        private const int KMid = 3;
        private const int KGap = 3;
        private const int XPrefixBlank = 40;
        private const int XSuffixBlank = 6;
        private const int DTWidth = 130;

        private const int YCoordWidth = 50;

        // Ys
        private const int LineHeight = 16;
        private const int YPrefixBlank = 20;
        private const int YSuffixBlank = 40;

        // Page
        private const int NumPerPage = 64;

        #endregion

        #region member
        
        // klines and quotas
        private RList<KLine> klines;
        private int ktype;
        private RList<object> quotas;
        private List<string> names;
        public Dictionary<string, PropertyInfo> ScalarProps { get; set; }

        // pages
        private int page;
        private int maxPage;

        private int maxKCount;
        private int yKChart;

        // kline range
        private double minLog;
        private double factorKLine;

        // quota range
        private double minQuota;
        private double factorQuota;

        // mouse
        private bool inCrossCursor;
        private Point mouseLocation;

        // Draw object
        private Pen dashRedPen;
        private Pen[] pens = new Pen[] { Pens.White, Pens.Yellow, Pens.Pink };

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

        public void DrawChart(RList<KLine> klines, int ktype, RList<object> quotas, List<string> names)
        {
            this.klines = klines;
            this.ktype = ktype;
            this.quotas = quotas;
            this.names = names;

            if (klines == null) return;
            
            page = 0;
            maxPage = klines.Count / NumPerPage;
            if (maxPage % NumPerPage != 0) maxPage += 1;

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics, e.ClipRectangle);
            //base.OnPaint(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                inCrossCursor = true;

            if (e.KeyCode == Keys.PageUp)
            {
                if (page < maxPage - 1) page += 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.PageDown)
            {
                if (page > 0) page -= 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.Escape)
                inCrossCursor = false;

            base.OnKeyDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (inCrossCursor)
            {
                mouseLocation = e.Location;
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        #region private draw methods
        private void Draw(Graphics g, Rectangle rect)
        {
            // Draw frame and Coordinate
            DrawFrame(g, rect);

            // klines and quotas
            if (klines != null && klines.Count > 0) DrawKlines(g, rect);
            if (names != null && quotas != null && quotas.Count > 0) DrawQuotas(g, rect);
          
            // Draw mouse
            if (inCrossCursor && mouseLocation != null)
            {
                g.DrawLine(Pens.White, 0, mouseLocation.Y, rect.Right, mouseLocation.Y);     // h
                g.DrawLine(Pens.White, mouseLocation.X, 0, mouseLocation.X, rect.Bottom);     // v

                if (klines == null || klines.Count == 0) return;
                // index of 
                if (mouseLocation.X > XSuffixBlank && mouseLocation.X < rect.Right - YCoordWidth - XPrefixBlank)
                {
                    int i = (rect.Right - YCoordWidth - XPrefixBlank - mouseLocation.X) / (KWidth + KGap);
                    if (i >= klines.Count) return;
                    KLine k = klines[i];

                    // draw datetime
                    g.DrawString(Utils.UtcToDateTime(k.UTC).ToString("yyyy/MM/dd hh:mm"), SystemFonts.MenuFont, Brushes.White, (rect.Width - YCoordWidth)/2, yKChart);               

                    // show kline text
                    string s = string.Format("O:{0:0.00}  C:{1:0.00}  H:{2:0.00}  L:{3:0.00}  V:{4:0}", k.OPEN, k.CLOSE, k.HIGH, k.LOW, k.VOLUMN/100);
                    g.DrawString(s, SystemFonts.MenuFont, Brushes.White, 4, 0);               

                    if (names == null && quotas == null) return;
                    // show quota text
                    object q = quotas[i];
                    s = null;
                    foreach (string scalar in names)
                    {
                        s += string.Format("{0}: {1:G5}  ", scalar, ScalarProps[scalar].GetValue(q));
                    }
                    g.DrawString(s, SystemFonts.MenuFont, Brushes.White, 4, yKChart + LineHeight);               
                }
            }
        }

        private void DrawFrame(Graphics g, Rectangle rect)
        {
            // Calcualte Import variable
            maxKCount = (rect.Width - YCoordWidth - XPrefixBlank - XSuffixBlank) / (KWidth + KGap);
            yKChart = rect.Height * 2 / 3;

            Rectangle r = new Rectangle(0, 0, rect.Width - 1, rect.Height - 1);
            g.DrawRectangle(Pens.Red, r);

            // Draw Coordinate
            g.DrawLine(Pens.Red, new Point(0, yKChart), new Point(rect.Right, yKChart));
            g.DrawLine(Pens.Red, new Point(0, yKChart + LineHeight), new Point(rect.Right, yKChart + LineHeight));
            g.DrawLine(Pens.Red, new Point(rect.Width - YCoordWidth, 0), new Point(rect.Width - YCoordWidth, rect.Height));

            g.DrawString(ktype.ToString(), SystemFonts.MenuFont, Brushes.White, new PointF(rect.Width - YCoordWidth + 4, yKChart));               
        }
    
        private void DrawKlines(Graphics g, Rectangle rect)
        {
            // get period of prices
            int start = page * NumPerPage;
            int len = maxKCount > klines.Count - start ? klines.Count - start : maxKCount;
            SetKLineRange(start, len);

            // 5 dashlines
            for (int i = 0; i < 5; i++ )
            {
                int h = (yKChart - YPrefixBlank - YSuffixBlank + 2) / 4;
                int y = yKChart - YPrefixBlank - i * h;
                g.DrawLine(dashRedPen, 0, y, rect.Width - YCoordWidth, y);

                // get price and draw
                double price = Math.Pow(10, (double)i * h / factorKLine + minLog);
                g.DrawString(price.ToString("0.00"), SystemFonts.MenuFont, Brushes.Red, new PointF(rect.Width - YCoordWidth + 4, y - LineHeight / 2));               
            }

            // for KLines
            for (int i = start; i < start + len; i++)
            {
                KLine k = klines[i];
                int x0 = rect.Width - YCoordWidth - XPrefixBlank - (i - start) * (KWidth + KGap) - KWidth;
                int xMid = x0 + KMid;        // X of up and down line
                int y0 = 0, y1 = 0;
                if (k.OPEN > k.CLOSE)
                {
                    y0 = GetKY(k.OPEN); y1 = GetKY(k.CLOSE);
                    if ((y1 - y0) > 0)
                        g.FillRectangle(Brushes.SkyBlue, x0, y0, KWidth, y1 - y0);
                    else
                        g.DrawLine(Pens.SkyBlue, x0, y0, x0 + KWidth, y0);

                    g.DrawLine(Pens.SkyBlue, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.SkyBlue, xMid, y1, xMid, GetKY(k.LOW));      // down line;
                }
                else if (k.OPEN == k.CLOSE)
                {
                    y0 = GetKY(k.OPEN);
                    g.DrawLine(Pens.White, x0, y0, x0 + KWidth, y0);

                    g.DrawLine(Pens.White, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.White, xMid, y0, xMid, GetKY(k.LOW));      // down line;
                } 
                else
                {
                    y0 = GetKY(k.CLOSE); y1 = GetKY(k.OPEN);
                    if ((y1 - y0) > 0)
                        g.DrawRectangle(Pens.Red, x0, y0, KWidth, y1 - y0);
                    else
                        g.DrawLine(Pens.Red, x0, y0, x0 + KWidth, y0);

                    g.DrawLine(Pens.Red, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.Red, xMid, y1, xMid, GetKY(k.LOW));      // down line;
                }
            }

            // draw datetime in X 
            DateTime dt1 = Utils.UtcToDateTime(klines[start + len - 1].UTC);
            DateTime dt2 = Utils.UtcToDateTime(klines[start].UTC);

            g.DrawString(dt1.ToString("yyyy/MM/dd hh:mm"), SystemFonts.MenuFont, Brushes.Red, new PointF(XSuffixBlank, yKChart));
            g.DrawString(dt2.ToString("yyyy/MM/dd hh:mm"), SystemFonts.MenuFont, Brushes.Red, new PointF(rect.Width - YCoordWidth - DTWidth, yKChart));
        }

        private void DrawQuotas(Graphics g, Rectangle rect)
        {
            // get period of prices
            int start = page * NumPerPage;
            int len = maxKCount > klines.Count - start ? klines.Count - start : maxKCount;

            SetQuotaRange(start, len);

            int num = 0;
            foreach (string name in names)
            {
                // Get pen according num
                Pen pen = pens[num % 3];

                double quota = Convert.ToDouble(ScalarProps[name].GetValue(quotas[start]));

                int x0 = rect.Width - YCoordWidth - XPrefixBlank - KMid;
                int y0 = rect.Height - GetQY(quota);
                for (int i = start + 1; i < start + len; i++)
                {
                    quota = Convert.ToDouble(ScalarProps[name].GetValue(quotas[i]));
                    int x1 = rect.Width - YCoordWidth - XPrefixBlank - (i - start) * (KWidth + KGap) - KMid;
                    int y1 = rect.Bottom - GetQY(quota);

                    g.DrawLine(pen, x0, y0, x1, y1);

                    x0 = x1; y0 = y1;
                }

                num++;
            }
        }

        #endregion

        #region private utils

        private void SetKLineRange(int start, int len)
        {
            float max = klines[start].HIGH;
            float min = klines[start].LOW;
            for (int i = start; i < start + len; i++)
            {
                KLine k = klines[i];
                if (max < k.HIGH) max = k.HIGH;
                if (min > k.LOW) min = k.LOW;
            }

            minLog = Math.Log10(min);
            factorKLine = (yKChart - YPrefixBlank - YSuffixBlank + 2) / (Math.Log10(max) - minLog);
        }
        private void SetQuotaRange(int start, int len)
        {
            double max = 0;
            double min = 0;
            bool first = true;
            foreach (string name in names)
            {
                double quota = Convert.ToDouble(ScalarProps[name].GetValue(quotas[start]));

                if (first)
                {
                    max = min = quota;
                    first = false;
                }

                for (int i = start; i < start + len; i++)
                {
                    quota = Convert.ToDouble(ScalarProps[name].GetValue(quotas[i]));
                    if (max < quota) max = quota;
                    if (min > quota) min = quota;
                }
            }

            minQuota = min;
            if (max > min)
            {
                factorQuota = (yKChart / 2 - 2 * LineHeight - 6) / (max - min);
            }
        }

        private int GetKY(float price)
        {
            int y = (int)((Math.Log10(price) - minLog) * factorKLine);
            return (yKChart - YPrefixBlank - y);
        }

        private int GetQY(double q)
        {
            int y = (int)((q - minQuota) * factorQuota);
            return y;
        }
        #endregion
    }
}

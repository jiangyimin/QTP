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
        internal struct KSize
        {
            public KSize(int w, int m, int g)
            {
                width = w;
                mid = m;
                gap = g;
            }

            public int width;
            public int mid;
            public int gap;
        }

        private KSize[] KS = new KSize[]
        { 
            new KSize(6, 3, 4),
            new KSize(4, 2, 3),
            new KSize(2, 1, 2),
            new KSize(1, 1, 1),
            new KSize(1, 1, 0)
        };

        // Xs
        private const int KWidth = 7;
        private const int KMid = 3;
        private const int KGap = 3;
        private const int RightBlank = 20;
        private const int LeftBlank = 4;
        private const int DTWidth = 110;

        private const int PriceWidth = 50;

        // Ys
        private const int LineHeight = 16;
        private const int TopBlank = 20;
        private const int BottomBlank = 40;

        private const int QMargin = 6;

        #endregion

        #region member
        
        // all input: klines and quotas
        private RList<KLine> klines;
        private int ktype;
        private List<RList<double>> quotas;
        private List<string> names;

        // pages and current
        private int ksIndex;
        private int currentPage;        //[0, maxPage]
        private int maxPage;
        private int rightKIndex;        // 右边K线下表
        private int leftKIndex;         // 左边K线下表

        // caculated from ksize
        private int countKPage;
        private int ySplit;

        // kline range
        private double minLog;
        private double ratioKLine;

        // quota range
        private double minQuota;
        private double ratioQuota;

        // mouse
        private bool useCrossCursor;
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

            // Create ZoomInfo

        }

        public void DrawChart(RList<KLine> klines, int ktype, List<RList<double>> quotas, List<string> names)
        {
            // 保存要画的4个数据
            this.klines = klines;
            this.ktype = ktype;
            this.quotas = quotas;
            this.names = names;

            currentPage = 0;
            ksIndex = 0;       // 结构下标

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e.Graphics, e.ClipRectangle);
            //base.OnPaint(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (ksIndex > 0) ksIndex -= 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.Down)
            {
                if (ksIndex < KS.Length - 1) ksIndex += 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                useCrossCursor = true;
            }

            if (e.KeyCode == Keys.PageUp)
            {
                if (currentPage < maxPage - 1) currentPage += 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.PageDown)
            {
                if (currentPage > 0) currentPage -= 1;
                this.Invalidate();
            }

            if (e.KeyCode == Keys.Escape)
            {
                useCrossCursor = false;
                this.Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (useCrossCursor)
            {
                mouseLocation = e.Location;
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        #region private draw methods
        private void Draw(Graphics g, Rectangle rect)
        {
            if (klines == null || klines.Count == 0) return;
            // Caculate 
            CaculateParameters(rect);

            // Draw frame and Coordinate
            DrawFrame(g, rect);

            // klines and quotas
            DrawKlines(g, rect);
            if (quotas != null && quotas.Count > 0 && quotas[0].Count > 0)
                DrawQuotas(g, rect);
          
            // Draw mouse
            DrawMouse(g, rect);
        }

        private void DrawMouse(Graphics g, Rectangle rect)
        {
            if (useCrossCursor && mouseLocation != null)
            {
                g.DrawLine(Pens.White, 0, mouseLocation.Y, rect.Right, mouseLocation.Y);     // h
                g.DrawLine(Pens.White, mouseLocation.X, 0, mouseLocation.X, rect.Bottom);     // v

                // caculate currentKIndex                
                int kindex = 0;
                if (mouseLocation.X < GetX(rect, leftKIndex) - KS[ksIndex].gap)
                    kindex = leftKIndex;
                else if (mouseLocation.X > rect.Right - PriceWidth - RightBlank)
                    kindex = rightKIndex;
                else
                {
                    kindex = rightKIndex + (rect.Right - PriceWidth - RightBlank - mouseLocation.X) / (KS[ksIndex].width + KS[ksIndex].gap);
                    if (kindex > leftKIndex) kindex = leftKIndex;
                }

                KLine k = klines[kindex];

                // draw datetime
                if (rect.Right - mouseLocation.X < DTWidth)     // 左边
                    g.DrawString(Utils.UtcToDateTime(k.UTC).ToString("yyyy/MM/dd HH:mm"), SystemFonts.MenuFont, Brushes.White, mouseLocation.X - DTWidth - 1, ySplit);
                else                                            // 右边
                    g.DrawString(Utils.UtcToDateTime(k.UTC).ToString("yyyy/MM/dd HH:mm"), SystemFonts.MenuFont, Brushes.White, mouseLocation.X, ySplit);

                // show kline detail text
                string s = string.Format("[{0}] 开:{1:0.00} 收:{2:0.00} 高:{3:0.00} 低:{4:0.00} 量:{5:0}", k.Symbol, k.OPEN, k.CLOSE, k.HIGH, k.LOW, k.VOLUME/100);
                g.DrawString(s, SystemFonts.MenuFont, Brushes.White, LeftBlank, 0);

                // show quota detail text
                if (quotas == null || quotas.Count == 0) return;
                s = null;
                for (int i = 0; i < names.Count; i++)
                    s += string.Format("{0}: {1:G5}  ", names[i], quotas[i][kindex]);
                g.DrawString(s, SystemFonts.MenuFont, Brushes.White, LeftBlank, ySplit + LineHeight);               
            }
        }

        private void CaculateParameters(Rectangle rect)
        {
            // 首先计算每页K线数
            countKPage = (rect.Width - PriceWidth - RightBlank - LeftBlank) / (KS[ksIndex].width + KS[ksIndex].gap);

            // maxPage
            maxPage = klines.Count / countKPage;
            if (maxPage % countKPage > 0) maxPage += 1;

            // 计算currentPage and rightKIndex and leftKindex
            if (currentPage > maxPage) currentPage = maxPage;
            rightKIndex = currentPage * countKPage;

            int len = countKPage > klines.Count - rightKIndex ? klines.Count - rightKIndex : countKPage;
            leftKIndex = rightKIndex + len - 1;

            // 上下分割位置
            ySplit = rect.Height * 2 / 3;
        }

        private void DrawFrame(Graphics g, Rectangle rect)
        {
            // frame
            Rectangle r = new Rectangle(0, 0, rect.Width - 1, rect.Height - 1);
            g.DrawRectangle(Pens.Red, r);

            // Draw Coordinate
            g.DrawLine(Pens.Red, 0, ySplit, rect.Right, ySplit);
            g.DrawLine(Pens.Red, 0, ySplit + LineHeight, rect.Right, ySplit + LineHeight);
            g.DrawLine(Pens.Red, rect.Width - PriceWidth, 0, rect.Width - PriceWidth, rect.Height);

            // 标识
            g.DrawString(string.Format("{0}分钟", ktype), SystemFonts.MenuFont, Brushes.White, rect.Width - PriceWidth + 4, ySplit);               
        }
    
        private void DrawKlines(Graphics g, Rectangle rect)
        {
            // get period of prices
            CaculateKLineYRange();

            // 5 dashlines            
            for (int i = 0; i < 5; i++ )
            {
                int height = (ySplit - TopBlank - BottomBlank + 1) / 4;
                int y = TopBlank + i * height;
                g.DrawLine(dashRedPen, 0, y, rect.Width - PriceWidth, y);

                // get price and draw
                double price = Math.Pow(10, (double)(4-i) * height / ratioKLine + minLog);
                g.DrawString(price.ToString("0.000"), SystemFonts.MenuFont, Brushes.Red, rect.Width - PriceWidth + 4, y - LineHeight / 2);               
            }

            // for KLines
            for (int i = rightKIndex; i <= leftKIndex; i++)
            {
                KLine k = klines[i];
                int x0 = GetX(rect, i);
                int xMid = x0 + KS[ksIndex].mid;        // X of up and down line
                int y0 = 0, y1 = 0;
                if (k.OPEN > k.CLOSE)
                {
                    y0 = GetKY(k.OPEN); y1 = GetKY(k.CLOSE);
                    if ((y1 - y0) > 0)
                        g.FillRectangle(Brushes.SkyBlue, x0, y0, KS[ksIndex].width + 1, y1 - y0 + 1);       // fill need +1
                    else
                        g.DrawLine(Pens.SkyBlue, x0, y0, x0 + KS[ksIndex].width, y0);

                    g.DrawLine(Pens.SkyBlue, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.SkyBlue, xMid, y1, xMid, GetKY(k.LOW));      // down line;
                }
                else if (k.OPEN == k.CLOSE)
                {
                    y0 = GetKY(k.OPEN);
                    g.DrawLine(Pens.White, x0, y0, x0 + KS[ksIndex].width, y0);

                    g.DrawLine(Pens.White, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.White, xMid, y0, xMid, GetKY(k.LOW));      // down line;
                } 
                else
                {
                    y0 = GetKY(k.CLOSE); y1 = GetKY(k.OPEN);
                    if ((y1 - y0) > 0)
                        g.DrawRectangle(Pens.Red, x0, y0, KS[ksIndex].width, y1 - y0);
                    else
                        g.DrawLine(Pens.Red, x0, y0, x0 + KS[ksIndex].width, y0);

                    g.DrawLine(Pens.Red, xMid, GetKY(k.HIGH), xMid, y0);     // up line
                    g.DrawLine(Pens.Red, xMid, y1, xMid, GetKY(k.LOW));      // down line;
                }
            }

            // draw datetime in X 
            DateTime dt1 = Utils.UtcToDateTime(klines[leftKIndex].UTC);
            DateTime dt2 = Utils.UtcToDateTime(klines[rightKIndex].UTC);

            if (GetX(rect, leftKIndex) > DTWidth + LeftBlank)
                g.DrawString(dt1.ToString("yyyy/MM/dd HH:mm"), SystemFonts.MenuFont, Brushes.Red, new PointF(GetX(rect, leftKIndex) - DTWidth, ySplit));
            else
                g.DrawString(dt1.ToString("yyyy/MM/dd HH:mm"), SystemFonts.MenuFont, Brushes.Red, new PointF(LeftBlank, ySplit));

            g.DrawString(dt2.ToString("yyyy/MM/dd HH:mm"), SystemFonts.MenuFont, Brushes.Red, new PointF(rect.Right - PriceWidth - DTWidth, ySplit));
        }

        private void DrawQuotas(Graphics g, Rectangle rect)
        {
            SetQuotaYRange();

            // 3 dashlines            
            for (int i = 0; i < 3; i++)
            {
                int height = (ySplit / 2 - 2 * LineHeight - 2 * QMargin + 1) / 2;
                int y = rect.Bottom - QMargin - i * height;
                g.DrawLine(dashRedPen, 0, y, rect.Width - PriceWidth, y);

                if (ratioQuota > 0)
                {
                    // get quota value and draw
                    double quota = (double)i * height / ratioQuota + minQuota;
                    g.DrawString(quota.ToString("0.000"), SystemFonts.MenuFont, Brushes.Red, rect.Width - PriceWidth + 4, y - LineHeight / 2);
                }
            }

            int num = 0;
            for (int index = 0; index < names.Count; index++)
            {
                // Get pen according num
                Pen pen = pens[num % 3];

                double quota = quotas[index][rightKIndex];

                int x0 = GetX(rect, rightKIndex) + KS[ksIndex].mid;
                int y0 = rect.Bottom - QMargin - GetQY(quota);
                for (int i = rightKIndex + 1; i <= leftKIndex; i++)
                {
                    quota = quotas[index][i];
                    int x1 = GetX(rect, i) + KS[ksIndex].mid;
                    int y1 = rect.Bottom - QMargin - GetQY(quota);

                    g.DrawLine(pen, x0, y0, x1, y1);

                    x0 = x1; y0 = y1;
                }

                num++;
            }
        }

        #endregion

        #region private utils

        private void CaculateKLineYRange()
        {
            float max = klines[rightKIndex].HIGH;
            float min = klines[rightKIndex].LOW;
            for (int i = rightKIndex; i <= leftKIndex; i++)
            {
                KLine k = klines[i];
                if (max < k.HIGH) max = k.HIGH;
                if (min > k.LOW) min = k.LOW;
            }

            minLog = Math.Log10(min);
            ratioKLine = (ySplit - TopBlank - BottomBlank + 1) / (Math.Log10(max) - minLog);
        }
        private void SetQuotaYRange()
        {
            double max = 0.0;
            double min1 = 0.0;
            double min2 = 0.0;

            max = min1 = min2 = quotas[0][rightKIndex];
            for (int index = 0; index < names.Count; index++ )
            {                
                for (int i = rightKIndex; i < leftKIndex; i++)
                {
                    double quota = quotas[index][i];
                    if (max < quota) max = quota;
                    if (quota > 0 && min1 > quota) min1 = quota;
                    if (min2 > quota) min2 = quota;
                }
            }

            //if (min1 > 0 && min2 == 0.0)
            //    minQuota = min1;
            //else
                minQuota = min2;

            if (max > minQuota)
            {
                ratioQuota = ( ySplit / 2 - 2 * LineHeight - 2 * QMargin) / (max - minQuota);
            }
        }

        private int GetX(Rectangle rect, int kindex)
        {
            return rect.Right - PriceWidth - RightBlank - (kindex - rightKIndex) * (KS[ksIndex].width + KS[ksIndex].gap) - KS[ksIndex].width;
        }
        private int GetKY(float price)
        {
            int y = (int)((Math.Log10(price) - minLog) * ratioKLine);

            int heigh = ySplit - TopBlank - BottomBlank + 1;
            return (TopBlank + heigh - y);
        }

        private int GetQY(double q)
        {
            int y = (int)((q - minQuota) * ratioQuota);
            return y;
        }
        #endregion
    }
}

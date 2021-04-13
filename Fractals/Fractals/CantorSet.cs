using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{
    /// <summary>
    /// Draws Cantor's set.
    /// </summary>
    class CantorSet : Fractal
    {
        private int dist;

        /// <summary>
        /// Method that draws fractals.
        /// </summary>
        /// <param name="pen"></param>
        public override void DrawFractal(Pen pen)
        {
            this.pen = pen;
            PictureBox pictureBox = sender as PictureBox;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            int xSize = (int)(pictureBox.Size.Width * 0.6f);
            int xStart = (int)(pictureBox.Size.Width * 0.1f);
            int xEnd = xSize + (int)(pictureBox.Size.Width * 0.1f);
            int ySize = pictureBox.Size.Height;
            Point p1 = new Point(xStart, 20);
            Point p2 = new Point(xEnd, 20);
            Generate(p1, p2, 0);
        }

        /// <summary>
        /// Setter for depth.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns>If it was set</returns>
        public override bool SetDepth(int depth)
        {
            if (depth >= 0 || depth < 10)
            {
                this.depth = depth;
                return true;
            }
            return false;
        }

        public void SetDist(int dist)
        {
            this.dist = dist;
        }


        /// <summary>
        /// Generates fractal and draws it rectursively.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="curDepth"></param>
        public void Generate(Point p1, Point p2, int curDepth)
        {
            DrawLine(p1, p2);
            if (curDepth < depth)
            {
                curDepth++;
                int dXD3 = (p2.X - p1.X) / 3;
                Generate(new Point(p1.X, p1.Y + dist), new Point(p1.X + dXD3, p2.Y + dist), curDepth);
                Generate(new Point(p1.X + 2 * dXD3,p1.Y + dist), new Point(p2.X, p2.Y +dist), curDepth);
            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public CantorSet(int depth, object sender, PaintEventArgs e, Pen pen) : base(depth, sender, e, pen)
        {
        }
    }
}

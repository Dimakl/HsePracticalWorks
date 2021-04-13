using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{

    /// <summary>
    /// Draws Kochs curve, depth is limited by 6
    /// </summary>
    class KochCurve : Fractal
    {
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
            Point p1 = new Point(xStart, ySize / 2);
            Point p2 = new Point(xEnd, ySize / 2);
            Generate(p1, p2, 0);
        }

        /// <summary>
        /// Setter for depth.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns>If it was set</returns>
        public override bool SetDepth(int depth)
        {
            if (depth >= 0 || depth < 7)
            {
                this.depth = depth;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generates fractal and draws it rectursively.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="curDepth"></param>
        public void Generate(Point f, Point s, int curDepth)
        {
            if (curDepth == depth)
                DrawLine(f, s);
            if (curDepth <= depth)
            {
                double a = Math.Sqrt(Math.Pow(f.X - s.X, 2) + Math.Pow(f.Y - s.Y, 2));
                a /= 3;
                double h = Math.Sqrt(Math.Pow(a, 2) - Math.Pow((a / 2), 2) / 4);
                Point m = new Point((f.X + s.X) / 2, (f.Y + s.Y) / 2);
                Point p1 = new Point((2 * f.X + s.X) / 3, (2 * f.Y + s.Y) / 3);
                Point p2 = new Point((2 * s.X + f.X) / 3, (2 * s.Y + f.Y) / 3);
                Point p3 = new Point(
                        (int)(m.X - (h * (-p2.Y + m.Y)) / (a / 2)),
                        (int)(m.Y - (h * (p2.X - m.X)) / (a / 2))
                    );
                curDepth++;
                Generate(f, p1, curDepth);
                Generate(p1, p3, curDepth);
                Generate(p3, p2, curDepth);
                Generate(p2, s, curDepth);
            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public KochCurve(int depth, object sender, PaintEventArgs e, Pen pen) : base(depth, sender, e, pen)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{
    /// <summary>
    /// Draws triangle of Serpinsky.
    /// </summary>
    class TriangleSerpinsky : Fractal
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
            Point[] p = GenerateTriangle(pictureBox);
            Point p1 = p[0], p2 = p[1], p3 = p[2];
            Generate(p1, p2, p3, 0);
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

        /// <summary>
        /// Generates first triangle.
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <returns></returns>
        private Point[] GenerateTriangle(PictureBox pictureBox)
        {
            int xSize = (int)(pictureBox.Size.Width * 0.6f);
            int xStart = (int)(pictureBox.Size.Width * 0.1f);
            int xEnd = xSize + (int)(pictureBox.Size.Width * 0.1f);
            int ySize = pictureBox.Size.Height;
            Point f = new Point(xStart, (int)(ySize / 1.5));
            Point s = new Point(xEnd, (int)(ySize / 1.5));
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
            return new Point[] { p1, p2, p3 };
        }

        /// <summary>
        /// Generates fractal and draws it rectursively.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="curDepth"></param>
        public void Generate(Point p1, Point p2, Point p3, int curDepth)
        {
            DrawTriangle(p1, p2, p3);
            if (curDepth <= depth)
            {
                Point withP3 = GetCenterOfPoints(p1, p2), withP2 = GetCenterOfPoints(p1, p3), withP1 = GetCenterOfPoints(p2, p3);
                curDepth++;
                Generate(withP1, p2, withP3, curDepth);
                Generate(withP1, withP2, p3, curDepth);
                Generate(p1, withP2, withP3, curDepth);
            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public TriangleSerpinsky(int depth, object sender, PaintEventArgs e, Pen pen) : base(depth, sender, e, pen)
        {
        }
    }
}

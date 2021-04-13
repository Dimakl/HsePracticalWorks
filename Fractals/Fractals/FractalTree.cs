using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{
    /// <summary>
    /// Draws fractal tree.
    /// </summary>
    class FractalTree : Fractal
    {

        /// <summary>
        /// Setter and getter for LengthCoef
        /// </summary>
        public double LengthCoef { get; set; } = 1.5;

        /// <summary>
        /// Setter and getter for LeftAngle
        /// </summary>
        public int LeftAngle { get; set; } = 45;

        /// <summary>
        /// Setter and getter for RightAngle
        /// </summary>
        public int RightAngle { get; set; } = 45;

        /// <summary>
        /// Method that draws fractals.
        /// </summary>
        /// <param name="pen"></param>
        public override void DrawFractal(Pen pen)
        {
            this.pen = pen;
            PictureBox pictureBox = sender as PictureBox;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Point p1 = new Point(pictureBox.Width / 2, pictureBox.Height);
            Point p2 = new Point(pictureBox.Width / 2, pictureBox.Height - 100);
            Generate(p1, p2, 100, 0, 45, 45);
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
        /// Generates fractal and draws it rectursively.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="curDepth"></param>
        public void Generate(Point p1, Point p2, int curLength, int curDepth, int lA, int rA)
        {
            DrawLine(p1, p2);
            if (curDepth < depth)
            {
                curDepth++;
                double cLength = (int)(VectorLength(SubstrPoints(p2, p1)) * LengthCoef);
                Generate(p2, new Point(p2.X - (int)(curLength * Math.Sin(lA * Math.PI / 180)), p2.Y - (int)(curLength * Math.Cos(lA * Math.PI / 180))), (int)cLength, curDepth, (lA + LeftAngle), (rA - LeftAngle));
                Generate(p2, new Point(p2.X + (int)(curLength * Math.Sin(rA * Math.PI / 180)), p2.Y - (int)(curLength * Math.Cos(rA * Math.PI / 180))), (int)cLength, curDepth, (lA - RightAngle), (rA + RightAngle));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public FractalTree(int depth, object sender, PaintEventArgs e, Pen pen) : base(depth, sender, e, pen)
        {
        }
    }
}

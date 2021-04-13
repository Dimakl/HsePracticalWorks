using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{
    /// <summary>
    /// Abstract class for fractals.
    /// </summary>
    abstract class Fractal
    {
        protected int depth = 11;
        protected readonly object sender;
        protected readonly PaintEventArgs e;
        protected Pen pen;

        /// <summary>
        /// Method that draws fractals.
        /// </summary>
        /// <param name="pen"></param>
        public abstract void DrawFractal(Pen pen);


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public Fractal(int depth, object sender, PaintEventArgs e, Pen pen)
        {
            this.depth = depth;
            this.sender = sender;
            this.e = e;
            this.pen = pen;
        }


        /// <summary>
        /// Setter for depth.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns>If it was set</returns>
        public virtual bool SetDepth(int depth)
        {
            if (depth >= 0)
            {
                this.depth = depth;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sumarises points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected Point SumPoints(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Multiplies points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        protected Point MultPoint(Point a, double d) => new Point((int)(a.X * d), (int)(a.Y * d));

        /// <summary>
        /// Substracts points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected Point SubstrPoints(Point a, Point b) => SumPoints(a, MultPoint(b, -1));

        /// <summary>
        /// Divides point by num.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        protected Point DividePoint(Point a, double d) => MultPoint(a, 1 / d);

        /// <summary>
        /// Length of vector from O to v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected int VectorLength(Point v) => (int)Math.Sqrt(VectorLengthPow2(v));

        /// <summary>
        /// Length of vector from O to v pow 2.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected int VectorLengthPow2(Point v) => (v.X * v.X) + (v.Y * v.Y);

        /// <summary>
        /// Draws line between 2 points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        protected void DrawLine(Point a, Point b) => e.Graphics.DrawLine(pen, a, b);

        /// <summary>
        /// Draws triangle betweeen 3 points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        protected void DrawTriangle(Point a, Point b, Point c) => e.Graphics.DrawPolygon(pen, new Point[] { a, b, c });


        /// <summary>
        /// Finds center of points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected Point GetCenterOfPoints(Point a, Point b) => new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
    }
}

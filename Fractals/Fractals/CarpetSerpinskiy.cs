using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fractals
{
    /// <summary>
    /// Draws carpet of Serpinsky.
    /// </summary>
    class CarpetSerpinskiy : Fractal
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
            Rectangle rect = new Rectangle(10, 10, Math.Min(pictureBox.Width - 30, pictureBox.Height - 30), Math.Min(pictureBox.Width - 30, pictureBox.Height - 30));
            Generate(rect, 0);
        }

        /// <summary>
        /// Setter for depth.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns>If it was set</returns>
        public override bool SetDepth(int depth)
        {
            if (depth >= 0 || depth < 6)
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
        public void Generate(Rectangle r, int curDepth)
        {
            if (curDepth == depth)
                e.Graphics.FillRectangle(pen.Brush, r);
            if (curDepth <= depth)
            {
                float w = r.Width / 3f, h = r.Width / 3f;
                var x1 = r.Left;
                var x2 = x1 + w;
                var x3 = x1 + 2f * w;
                var y1 = r.Top;
                var y2 = y1 + h;
                var y3 = y1 + 2f * h;
                curDepth++;
                Generate(new Rectangle((int)x1, (int)y1, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x2, (int)y1, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x3, (int)y1, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x1, (int)y2, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x3, (int)y2, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x1, (int)y3, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x2, (int)y3, (int)w, (int)h), curDepth);
                Generate(new Rectangle((int)x3, (int)y3, (int)w, (int)h), curDepth);
            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="pen"></param>
        public CarpetSerpinskiy(int depth, object sender, PaintEventArgs e, Pen pen) : base(depth, sender, e, pen)
        {
        }
    }
}

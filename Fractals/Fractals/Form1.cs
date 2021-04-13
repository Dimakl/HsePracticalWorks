using System;
using System.Drawing;
using System.Windows.Forms;

namespace Fractals
{
    public partial class Form1 : Form
    {

        private int depth = 1;
        private KochCurve koch;
        private TriangleSerpinsky triangle;
        private CarpetSerpinskiy carpet;
        private int pickedFractal = 0;
        private CantorSet cantor;
        FractalTree tree;
        private int cantorDist = 40;
        private int leftAngle = 45, rightAngle = 45;
        private double coef = 0.5;
        private readonly Pen p = new Pen(Color.Black, 1);
        private string message = "Значение глубины рекурсии фрактала должно быть не меньше 0 и быть ";
        private MessageBoxButtons buttons = MessageBoxButtons.OK;

        /// <summary>
        /// Initialization of form.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.MinimumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            pictureBox.Size = new Size(Size.Width, Size.Height);
            pictureBox.Invalidate();
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// On drawButton click actions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawButton_Click(object sender, EventArgs e)
        {
            if (!checkTextInp())
                return;
            int newDepth = int.Parse(textBox1.Text);
            if (!CheckDepth(newDepth))
                return;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    pickedFractal = 0;
                    break;
                case 1:
                    pickedFractal = 1;
                    break;
                case 2:
                    if (CheckTextInp3() && CheckTextInp4() && CheckTextInp5())
                    {
                        coef = double.Parse(textBox3.Text);
                        leftAngle = int.Parse(textBox4.Text);
                        rightAngle = int.Parse(textBox5.Text);
                    }
                    else
                        return;
                    pickedFractal = 2;
                    break;
                case 3:
                    pickedFractal = 3;
                    break;
                case 4:
                    if (!CheckTextInp2("Расстояние между отрезками должно быть челым числом от 20 до 100", textBox2))
                        return;
                    cantorDist = int.Parse(textBox2.Text);
                    pickedFractal = 4;
                    break;

            }
            depth = newDepth;
            pictureBox.Invalidate();
        }

        /// <summary>
        /// Checks if set depth is correct.
        /// </summary>
        /// <param name="newDepth"></param>
        /// <returns></returns>
        private bool CheckDepth(int newDepth)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    if (newDepth > 6)
                        return MessageBoxShowError(message + "меньшим чем 7", "Ошибка во вводе", buttons);
                    break;
                case 1:
                    if (newDepth > 9)
                        return MessageBoxShowError(message + "меньшим чем 10", "Ошибка во вводе", buttons);
                    break;
                case 2:
                    if (newDepth > 15)
                        return MessageBoxShowError(message + "меньшим чем 16", "Ошибка во вводе", buttons);
                    break;
                case 3:
                    if (newDepth > 5)
                        return MessageBoxShowError(message + "меньшим чем 6", "Ошибка во вводе", buttons);
                    break;
                case 4:
                    if (newDepth > 9)
                        return MessageBoxShowError(message + "меньшим чем 10", "Ошибка во вводе", buttons);

                    break;
            }
            return true;
        }

        /// <summary>
        /// Additional method for throwing Message error.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        private bool MessageBoxShowError(string message, string caption, MessageBoxButtons buttons)
        {
            MessageBox.Show(message, "Ошибка во вводе", buttons);
            return false;
        }

        /// <summary>
        /// Checks textInp for validity/
        /// </summary>
        /// <returns>If text field value is valid</returns>
        private bool checkTextInp()
        {
            string inp = textBox1.Text;
            string message = "Значение глубины рекурсии фрактала должно быть не меньше 0 и быть числом";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (!int.TryParse(inp, out int a) || a < 0)
            {
                MessageBox.Show(message, "Ошибка во вводе", buttons);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks textInp2 for validity/
        /// </summary>
        /// <returns>If text field value is valid</returns>
        private bool CheckTextInp2(string text, TextBox textBox)
        {
            string inp = textBox.Text;
            string message = text;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (!int.TryParse(inp, out int a) || a < 20 || a > 100)
            {
                MessageBox.Show(message, "Ошибка во вводе", buttons);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks textInp3 for validity/
        /// </summary>
        /// <returns>If text field value is valid</returns>
        private bool CheckTextInp3()
        {
            string inp = textBox3.Text;
            string message = "Значение коэффициентов длин должно быть числом от 0,2 до 2";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (!double.TryParse(inp, out double a) || a < 0.2 || a > 2)
            {
                MessageBox.Show(message, "Ошибка во вводе", buttons);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks textInp4 for validity/
        /// </summary>
        /// <returns>If text field value is valid</returns>
        private bool CheckTextInp4()
        {
            string inp = textBox4.Text;
            string message = "Значение левого угла должно быть целым числом от 1 до 89";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (!int.TryParse(inp, out int a) || a < 1 || a > 89)
            {
                MessageBox.Show(message, "Ошибка во вводе", buttons);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks textInp5 for validity/
        /// </summary>
        /// <returns>If text field value is valid</returns>
        private bool CheckTextInp5()
        {
            string inp = textBox5.Text;
            string message = "Значение правого угла должно быть целым числом от 1 до 89";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            if (!int.TryParse(inp, out int a) || a < 1 || a > 89)
            {
                MessageBox.Show(message, "Ошибка во вводе", buttons);
                return false;
            }
            return true;
        }

        /// <summary>
        /// When pictureBox redraws.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            switch (pickedFractal)
            {
                case 0:
                    koch = new KochCurve(depth, sender, e, p);
                    koch.DrawFractal(p);
                    break;
                case 1:
                    triangle = new TriangleSerpinsky(depth, sender, e, p);
                    triangle.DrawFractal(p);
                    break;
                case 2:
                    tree = new FractalTree(depth, sender, e, p);
                    tree.LengthCoef = coef;
                    tree.LeftAngle = leftAngle;
                    tree.RightAngle = rightAngle;
                    tree.DrawFractal(p);
                    break;
                case 3:
                    carpet = new CarpetSerpinskiy(depth, sender, e, p);
                    carpet.DrawFractal(p);
                    break;
                case 4:
                    cantor = new CantorSet(depth, sender, e, p);
                    cantor.SetDist(cantorDist);
                    cantor.DrawFractal(p);
                    break;
            }
        }

        /// <summary>
        /// Accidentely clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Actions on form size changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox.Size = new Size(Size.Width, Size.Height);
            pictureBox.Invalidate();
        }

        /// <summary>
        /// Change in comboBox choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    label1.Text = "Выбор глубины рекурсии фрактала:\n " + "от 0 и до 6 включительно";
                    break;
                case 1:
                    label1.Text = "Выбор глубины рекурсии фрактала:\n " + "от 0 и до 9 включительно";
                    break;
                case 2:
                    label1.Text = "Выбор глубины рекурсии фрактала:\n " + "от 0 и до 15 включительно";
                    break;
                case 3:
                    label1.Text = "Выбор глубины рекурсии фрактала:\n " + "от 0 и до 5 включительно";
                    break;
                case 4:
                    label1.Text = "Выбор глубины рекурсии фрактала:\n " + "от 0 и до 9 включительно";
                    break;
            }
            SetCantorVisible();
            SetTreeVisible();
        }


        /// <summary>
        /// Sets cantor set fields visible.
        /// </summary>
        private void SetCantorVisible()
        {
            if (comboBox1.SelectedIndex == 4)
            {
                label2.Visible = true;
                textBox2.Visible = true;
            }
            else
            {
                label2.Visible = false;
                textBox2.Visible = false;
            }
        }

        /// <summary>
        /// Sets tree fields visible.
        /// </summary>
        private void SetTreeVisible()
        {
            if (comboBox1.SelectedIndex == 2)
            {
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
            }
        }

        /// <summary>
        /// Saving fractal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
            pictureBox.DrawToBitmap(bmp, pictureBox.ClientRectangle);
            bmp.Save("fractal");
        }
    }
}

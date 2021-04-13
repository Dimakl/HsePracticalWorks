using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Notepad_
{
    public partial class OptionsForm : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public OptionsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Пустой метод.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionsForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Сохранение настроек.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            StaticNotepadData.autosave = (int)numericUpDown1.Value;
            Close();
        }
    }
}

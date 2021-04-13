using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Notepad_
{
    public partial class ParentForm : Form
    {
        /// <summary>
        /// Конструктор родительской формы.
        /// </summary>
        public ParentForm()
        {
            InitializeComponent();
            if (StaticNotepadData.tabData.Length == 0)
                StaticNotepadData.tabData = new TabData[][] { new TabData[] { } };
            for (int i = 0; i <StaticNotepadData.tabData.Length; i++)
                Application.Run(new Form1(i, this));
        }

        /// <summary>
        /// Создание дочерней формы.
        /// </summary>
        public void CreateForm()
        {
            List<TabData[]> td = new List<TabData[]>(StaticNotepadData.tabData);
            td.Add(new TabData[] { new TabData("New file") });
            StaticNotepadData.tabData = td.ToArray();
            Form1 form = new Form1(StaticNotepadData.tabData.Length - 1, this);
            form.Hide();
            form.Show();
        }
    }
}

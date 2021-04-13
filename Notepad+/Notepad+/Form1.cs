using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad_
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parent"></param>
        public Form1(int index, ParentForm parent)
        {
            this.index = index;
            this.parent = parent;
            InitializeComponent();
            SetupTabs();
            SetUpToolStrip();
            SetUpToolStripEdit();
            SetUpToolFile();
        }

        /// <summary>
        /// Первоначальная настройка вкладок.
        /// </summary>
        private void SetupTabs()
        {
            if (StaticNotepadData.tabData[index] == null || StaticNotepadData.tabData[index].Length == 0)
                StaticNotepadData.tabData[index] = new TabData[] { new TabData("New File") };
            foreach (var tabData in StaticNotepadData.tabData[index])
                AddTab(tabData);
            openedTextBox = (RichTextBox)tabControl1.TabPages[0].Controls[0];
        }

        RichTextBox openedTextBox = null;
        FontDialog mainFontDialog = new FontDialog();
        private int index;
        private readonly ParentForm parent;
        /// <summary>
        /// Добавление вкладки.
        /// </summary>
        /// <param name="tabData"></param>
        private void AddTab(TabData tabData)
        {
            TabPage page = new TabPage(tabData.Name);
            tabControl1.TabPages.Add(page);
            RichTextBox rtb = new RichTextBox();
            rtb.BackColor = StaticNotepadData.GetRTBBackground();
            rtb.ForeColor = StaticNotepadData.GetRTBTextColor();
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add(new ToolStripMenuItem("Вырезать", null, CutClick));
            cms.Items.Add(new ToolStripMenuItem("Копировать", null, CopyClick));
            cms.Items.Add(new ToolStripMenuItem("Вставить", null, PasteClick));
            cms.Items.Add(new ToolStripMenuItem("Задать формат", null, SelectFormatClick));
            cms.Items.Add(new ToolStripMenuItem("Выбрать все", null, (s, e) => openedTextBox.SelectAll()));
            cms.Opened += (sender, e) => openedTextBox = (RichTextBox)cms.SourceControl;
            rtb.Dock = DockStyle.Fill;
            rtb.Multiline = true;
            page.Controls.Add(rtb);
            rtb.ContextMenuStrip = cms;
        }
        /// <summary>
        /// Настройка верхнего меню.
        /// </summary>
        private void SetUpToolStrip()
        {
            toolStripMenuItem4.Click += (s, e) =>
            {
                StaticNotepadData.currentTheme = "light";
                openedTextBox.BackColor = StaticNotepadData.GetRTBBackground();
                openedTextBox.ForeColor = StaticNotepadData.GetRTBTextColor();
            };
            toolStripMenuItem5.Click += (s, e) =>
            {
                StaticNotepadData.currentTheme = "dark";
                openedTextBox.BackColor = StaticNotepadData.GetRTBBackground();
                openedTextBox.ForeColor = StaticNotepadData.GetRTBTextColor();
            };
            toolStripMenuItem6.Click += (s, e) =>
            {
                OptionsForm optionsForm = new OptionsForm();
                optionsForm.Hide();
                optionsForm.Show();
            };
            toolStripDropDownButton3.Click += (s, e) =>
            {
                mainFontDialog.ShowDialog();
                openedTextBox.Font = mainFontDialog.Font;
            };
        }
        /// <summary>
        /// Настройка Правки в верхнем меню.
        /// </summary>
        private void SetUpToolStripEdit()
        {
            toolStripMenuItem8.Click += (s, e) =>
            {
                openedTextBox.SelectAll();
            };
            toolStripMenuItem9.Click += (s, e) =>
            {
                CutClick(s, e);
            };
            toolStripMenuItem10.Click += (s, e) =>
            {
                CopyClick(s, e);
            };
            toolStripMenuItem11.Click += (s, e) =>
            {
                PasteClick(s, e);
            };
        }
        /// <summary>
        /// Настройка Файлы в верхнем меню.
        /// </summary>
        private void SetUpToolFile()
        {
            toolStripMenuItem1.Click += (s, e) =>
            {
                List<TabData> td = new List<TabData>(StaticNotepadData.tabData[index]);
                td.Add(new TabData("New file"));
                StaticNotepadData.tabData[index] = td.ToArray();
                AddTab(StaticNotepadData.tabData[index][^1]);
            };
            toolStripMenuItem2.Click += (s, e) =>
            {
                StaticNotepadData.RemoveTab(index, tabControl1.SelectedIndex);
                tabControl1.TabPages.Clear();
                SetupTabs();
            };
            toolStripMenuItem3.Click += (s, e) => parent.CreateForm();
            toolStripMenuItem7.Click += (s, e) => Close();
            toolStripMenuItem12.Click += (s, e) => Application.Exit();
            toolStripMenuItem13.Click += (s, e) =>
            {
                string name = OpenFile();
                if (name != null)
                {
                    StaticNotepadData.tabData[index][tabControl1.SelectedIndex].FilePath = name;
                    openedTextBox.Text = File.ReadAllText(name);
                }
            };
            toolStripMenuItem14.Click += (s, e) => SaveFile();
        }
        /// <summary>
        /// Открытие файла.
        /// </summary>
        /// <returns></returns>
        private string OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.txt|*.rtf";
            string name = null;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                name = ofd.FileName;
            }
            return name;
        }
        /// <summary>
        /// Сохранение файла.
        /// </summary>
        private void SaveFile()
        {
            if (StaticNotepadData.tabData[index][tabControl1.SelectedIndex].FilePath == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = "rtf";
                sfd.Filter = "*.txt|*.rtf";
                if (sfd.ShowDialog() == DialogResult.OK)
                    openedTextBox.SaveFile(sfd.FileName);

            }
            else
                openedTextBox.SaveFile(StaticNotepadData.tabData[index][tabControl1.SelectedIndex].FilePath);
        }
        /// <summary>
        /// Изменение настроек шрифтов и т.д.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFormatClick(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.ShowDialog();
            openedTextBox.SelectionFont = fd.Font;
        }
        /// <summary>
        /// Вырезать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CutClick(object sender, EventArgs e)
        {
            if (openedTextBox.SelectedText.Length > 0)
                openedTextBox.Cut();
        }
        /// <summary>
        /// Копировать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CopyClick(object sender, EventArgs e)
        {
            if (openedTextBox.SelectedText.Length > 0)
                openedTextBox.Copy();
        }
        /// <summary>
        /// Вставить.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PasteClick(object sender, EventArgs e)
        {
            DataFormats.Format textFormat = DataFormats.GetFormat(DataFormats.Text);
            if (openedTextBox.CanPaste(textFormat))
                openedTextBox.Paste();
        }
        /// <summary>
        /// Пустой метод.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        /// <summary>
        /// Пустой метод.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        /// <summary>
        /// Пустой метод.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStrip1_ItemClicked_2(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        /// <summary>
        /// Измение выбора вкладки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == null)
                return;
            openedTextBox = (RichTextBox)tabControl1.SelectedTab.Controls[0];
            openedTextBox.BackColor = StaticNotepadData.GetRTBBackground();
            openedTextBox.ForeColor = StaticNotepadData.GetRTBTextColor();
            if (mainFontDialog != null)
                openedTextBox.Font = mainFontDialog.Font;
        }
        /// <summary>
        /// Пустой метод.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// События при нажатии клавиш в форме.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N && e.Control)
                parent.CreateForm();
            if (e.KeyCode == Keys.T && e.Control)
            {
                List<TabData> td = new List<TabData>(StaticNotepadData.tabData[index]);
                td.Add(new TabData("New file"));
                StaticNotepadData.tabData[index] = td.ToArray();
                AddTab(StaticNotepadData.tabData[index][^1]);
            }
            if (e.KeyCode == Keys.Q && e.Control)
                Application.Exit();
            if (e.KeyCode == Keys.S && e.Control)
            {
                if (StaticNotepadData.tabData[index][tabControl1.SelectedIndex].FilePath == null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.DefaultExt = "rtf";
                    sfd.Filter = "*.txt|*.rtf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                        openedTextBox.SaveFile(sfd.FileName);

                }
                else
                    openedTextBox.SaveFile(StaticNotepadData.tabData[index][tabControl1.SelectedIndex].FilePath);
            }
        }
    }
}

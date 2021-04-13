using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TableAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".csv",
                Filter = "Table files (*.csv)|*.csv"
            };
            Nullable<bool> result = dlg.ShowDialog();
            string filename;
            if (result.HasValue && result.Value)
            {
                filename = dlg.FileName;
                //TextBox1.Text = filename;
                //ParseCsvIntoDT(filename);
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Name");
                dt.Columns.Add("Marks");
                DataRow _ravi = dt.NewRow();
                _ravi["Name"] = "ravi";
                _ravi["Marks"] = "500";
                dt.Rows.Add(_ravi);
                TableGrid.DataContext = dt.DefaultView;
                var a = dt.Columns.Count;
            }
        }

        private void ParseCsvIntoDT(string filename)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string[] lines = System.IO.File.ReadAllLines(filename);
            if (lines.Length > 0)
            {
                string firstLine = lines[0];
                string[] labels = firstLine.Split(',');
                /*foreach (string header in labels)
                {
                    var s = header.Substring(1, header.Length - 2);
                    dt.Columns.Add("123");
                }*/
                dt.Columns.Add(new System.Data.DataColumn("123"));
                var a = dt.Columns.Count;
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(',');
                    System.Data.DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in labels)
                    {
                        var s = "123";
                        dr[s] = '[' + data[columnIndex] + ']';
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                TableGrid.DataContext = dt.DefaultView;
            }
        }
    }
}

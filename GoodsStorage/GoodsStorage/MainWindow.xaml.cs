using System;
using System.Collections.Generic;
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

namespace GoodsStorage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Section root = new Section("Root");
            Section first = new Section("First");
            Section second = new Section("Second"); 
            Section third = new Section("Third");
            root.Items.Add(first);
            root.Items.Add(second);
            first.Items.Add(third);

            treeMenu.Items.Add(root);
        }

        private void treeMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Section item = (Section) treeMenu.SelectedItem;
            sectionNameTextBlock.Text = item.Title;
        }
    }
}

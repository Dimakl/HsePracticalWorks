using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            
            Section root = Section.ROOT;
            Section first = new Section("First");
            Section second = new Section("Second"); 
            Section third = new Section("Third");
            root.Items.Add(first);
            root.Items.Add(second);
            first.Items.Add(third);
            root.Products.Add(new Product() { Name = "Test", Code = "12312",LeftInStorage = 14, Price = 324 });
            root.Products.Add(new Product() { Name = "Test", Code = "12312", LeftInStorage = 123, Price = 324 });
            root.Products.Add(new Product() { Name = "Test", Code = "12312", LeftInStorage = 1, Price = 324 });
            third.Products.Add(new Product() { Name = "Test", Code = "12312", LeftInStorage = 6, Price = 324 });
            first.Products.Add(new Product() { Name = "Test", Code = "12312", LeftInStorage = 3, Price = 324 });
            
            treeMenu.Items.Add(Section.ROOT);
        }

        private void treeMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Section item = (Section) treeMenu.SelectedItem;
            if (item == null)
                return;
            sectionNameTextBlock.Text = item.Title;
            productsGrid.ItemsSource = item.Products;
        }

        private void UploadJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Stream stream = File.Open("storage.bin", FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, Section.ROOT);
                    stream.Close();
                }
            }
            catch (Exception)
            { }
        }

        private void LoadJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Stream stream = File.Open("storage.bin", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    Section.ROOT = (Section)binaryFormatter.Deserialize(stream);
                    treeMenu.Items.Clear();
                    treeMenu.Items.Add(Section.ROOT);
                }

            }
            catch (Exception)
            { }
        }

        private void CreateCsv_Click(object sender, RoutedEventArgs e)
        {
            string input = csvInput.Text;
            if (!int.TryParse(input, out int n) || n < 0)
            {
                csvInput.Text = "Неверный ввод!";
                return;
            }
            List<CsvProduct> products = new List<CsvProduct>();
            GetAllCsvProducts("", Section.ROOT, products, n);
            try
            {
                using (StreamWriter streamReader = new StreamWriter("data.csv"))
                {
                    using (CsvWriter csvReader = new CsvWriter(streamReader, CultureInfo.CurrentCulture))
                    {
                        csvReader.WriteRecords(products);
                    }
                }
                csvInput.Text = "Данные успешно записаны в data.csv";
            }
            catch (Exception)
            {
                csvInput.Text = "Не удалось записать данные в data.csv";
            }
        }

        private void GetAllCsvProducts(string fullPath, Section section, List<CsvProduct> products, int n)
        {
            if (fullPath.Length == 0)
                fullPath = section.Title;
            else
                fullPath = fullPath + "/" + section.Title;
            foreach (var product in section.Products)
                if (product.LeftInStorage < n)
                    products.Add(new CsvProduct(fullPath, product));
            foreach (var sec in section.Items)
                GetAllCsvProducts(fullPath, sec, products, n);
        }
    }
}

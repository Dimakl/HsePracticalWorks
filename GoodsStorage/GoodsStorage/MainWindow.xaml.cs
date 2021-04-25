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
            MessageBox.Show("ПКМ по нужному разделу, чтобы открыть контекстное меню. Root нельзя удалить или переименовать - это корневой раздел.");
            treeMenu.Items.Add(Section.ROOT);

        }

        /// <summary>
        /// Меняем выбранный элемент при клике на элемент treeMenu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Section item = (Section)treeMenu.SelectedItem;
            if (item == null)
                return;
            productsGrid.ItemsSource = item.Products;
            tableHelpText.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Выгружаем текущий Section.ROOT в бинарный файл. (UploadJson - просто имя кнопки).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Загружаем текущий Section.ROOT из бинарного файла. (LoadJson - просто имя кнопки).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Создаем csv файл с выбранными пользователем настройками при клике.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Получаем полный список товаров для упаковки в csv.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="section"></param>
        /// <param name="products"></param>
        /// <param name="n"></param>
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


        /// <summary>
        /// Вызываем контекстное меню при ПКМ на treeMenu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeMenu_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (treeViewItem == null)
                return;
            treeViewItem.Focus();
            e.Handled = true;
            MenuItem menuCreate = new MenuItem();
            menuCreate.Header = "Создать подраздел";
            SetupMenuCreate(menuCreate);

            MenuItem menuDelete = new MenuItem();
            menuDelete.Header = "Удалить раздел";
            SetupMenuDelete(menuDelete);

            MenuItem menuChange = new MenuItem();
            menuChange.Header = "Изменить имя раздела";
            SetupMenuChange(menuChange);
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add(menuCreate);
            if (((Section)treeMenu.SelectedItem) != Section.ROOT)
            {
                contextMenu.Items.Add(menuDelete);
                contextMenu.Items.Add(menuChange);
            }
            contextMenu.IsOpen = true;
        }


        /// <summary>
        /// Добавляем Click к menuChange кнопке контекстного меню.
        /// </summary>
        /// <param name="menuChange"></param>
        private void SetupMenuChange(MenuItem menuChange)
        {
            menuChange.Click += new RoutedEventHandler(
                            (s, args) =>
                            {

                                string name = (string)PromptDialog.Dialog.Prompt("Введите новое имя раздела (оно должно быть уникальным в рамках раздела-предка)", "Изменение имени раздела");
                                if (name == null)
                                {
                                    // Нажата кнопка cancel. 
                                }
                                else if (name == "")
                                    MessageBox.Show("Неверное имя раздела!");
                                else if (!((Section)treeMenu.SelectedItem).IsProductPresent(name))
                                    MessageBox.Show($"Подраздел с именем {name} уже существует в разделе {((Section)treeMenu.SelectedItem).Parent.Title}!");
                                else
                                {
                                    ((Section)treeMenu.SelectedItem).Title = name;
                                    treeMenu.Items.Clear();
                                    treeMenu.Items.Add(Section.ROOT);
                                    MessageBox.Show($"Имя раздела было изменено на {name}!");
                                }
                            });
        }


        /// <summary>
        /// Добавляем Click к menuDelete кнопке контекстного меню.
        /// </summary>
        /// <param name="menuDelete"></param>
        private void SetupMenuDelete(MenuItem menuDelete)
        {
            menuDelete.Click += new RoutedEventHandler(
                           (s, args) =>
                           {
                               ((Section)treeMenu.SelectedItem).Delete();
                               MessageBox.Show($" Раздел {((Section)treeMenu.SelectedItem).Title} был удален!");

                           });
        }


        /// <summary>
        /// Добавляем Click к menuCreate кнопке контекстного меню.
        /// </summary>
        /// <param name="menuCreate"></param>
        private void SetupMenuCreate(MenuItem menuCreate)
        {
            menuCreate.Click += new RoutedEventHandler(
                            (s, args) =>
                            {

                                string name = (string)PromptDialog.Dialog.Prompt("Введите имя подраздела (оно должно быть уникальным в рамках раздела-предка)", "Новый раздел");
                                if (name == null)
                                {
                                    // Нажата кнопка cancel. 
                                }
                                else if (name == "")
                                    MessageBox.Show("Неверное имя нового раздела!");
                                else if (!((Section)treeMenu.SelectedItem).IsProductPresent(name))
                                    MessageBox.Show($"Подраздел с именем {name} уже существует в разделе {((Section)treeMenu.SelectedItem).Title}!");
                                else
                                {
                                    ((Section)treeMenu.SelectedItem).Items.Add(new Section(name, (Section)treeMenu.SelectedItem));
                                    MessageBox.Show($"Подраздел с именем {name} успешно добавлен в раздел {((Section)treeMenu.SelectedItem).Title}!");
                                }
                            });
        }

        /// <summary>
        /// Ищем ту ноду, на которую пользователь нажал при помощи ПКМ.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
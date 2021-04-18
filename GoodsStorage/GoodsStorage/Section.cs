using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace GoodsStorage
{
    [Serializable]
    class Section
    {
        public static Section ROOT = new Section("Root");
        public string Title { get; set; }

        public ObservableCollection<Section> Items { get; set; }
        public ObservableCollection<Product> Products { get; private set; }

        public Section(string title)
        {
            this.Title = title;
            this.Items = new ObservableCollection<Section>();
            this.Products = new ObservableCollection<Product>();
        }

        internal bool IsProductPresent(string name)
        {
            foreach (var product in Products)
                if (product.Name == name)
                    return false;
            return true;
        }
    }
}

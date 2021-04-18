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
        public static Section ROOT = new Section("Root", null);
        public string Title { get; set; }

        public ObservableCollection<Section> Items { get; set; }
        public ObservableCollection<Product> Products { get; private set; }

        public Section Parent { get;}

        public Section(string title, Section parent)
        {
            Title = title;
            Items = new ObservableCollection<Section>();
            Products = new ObservableCollection<Product>();
            Parent = parent;
        }

        public bool IsProductPresent(string name)
        {
            foreach (var product in Products)
                if (product.Name == name)
                    return false;
            return true;
        }

        public void Delete()
        {
            Parent.Items.Remove(this);
        }
    }
}

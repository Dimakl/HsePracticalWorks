using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace GoodsStorage
{
    class Section
    {

        public string Title { get; set; }

        public ObservableCollection<Section> Items { get; set; }

        public Section(string title)
        {
            this.Title = title;
            this.Items = new ObservableCollection<Section>();
        }
    }
}

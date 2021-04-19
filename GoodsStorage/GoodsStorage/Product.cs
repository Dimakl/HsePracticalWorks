using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsStorage
{

    /// <summary>
    /// Класс товара.
    /// </summary>
    [Serializable]
    class Product
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }
        public int LeftInStorage { get; set; }
        public Product()
        {
        }
    }
}

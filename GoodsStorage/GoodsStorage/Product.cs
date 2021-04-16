using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsStorage
{
    [Serializable]
    class Product
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }
        public int LeftInStorage { get; set; }
        public string Description { get; set; }
        public string PathToPhoto { get; set; }

        public Product()
        {
        }

        public override string ToString()
        {
            return $"Название:{Name},Код:{Code}," +
                $"Цена:{Price},Осталось:{LeftInStorage}," +
                $"Описание:{Description},Файл фотографии:{PathToPhoto}";
        }
    }
}

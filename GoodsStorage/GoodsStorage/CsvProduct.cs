using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodsStorage
{
    /// <summary>
    /// Класс товара для выгрузки в CSV.
    /// </summary>
    class CsvProduct
    {
        [Name("Full path to product")]
        public string FullPath { get; set; }
        [Name("Code")]
        public string Code { get; set; }
        [Name("Product name")]
        public string Name { get; set; }
        [Name("Quantity in stock")]
        public int LeftInStorage { get; set; }

        public CsvProduct(string fullPath, Product product)
        {
            FullPath = fullPath;
            Code = product.Code;
            Name = product.Name;
            LeftInStorage = product.LeftInStorage;
        }
    }
}

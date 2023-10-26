﻿using IT_Arg_API.Models.Helpers;
using System.Data;

namespace IT_Arg_API.Models
{
    public class Product
    {
        private int? _id;
        private string _name;
        private string _description;
        private int _stock;
        private string _photo;
        private string _code;
        private float _price;
        public Product()
        {

        }

        public List<ReceiptLine> GetListProductsPricesById(List<ReceiptLine> receiptLineList)
        {            
            DataTable productIdsTable = new DataTable();
            productIdsTable.Columns.Add("IdProduct", typeof(int));

            foreach (var line in receiptLineList)
            {
                DataRow row = productIdsTable.NewRow();
                row["IdProduct"] = Convert.ToInt32(line.IdProduct);
                productIdsTable.Rows.Add(row);
            }

            //all of this is broken
            //List<Dictionary<object, object>> productPrices = DBHelper.callProcedureDataTableReader("spProductsGetPricesById", "TypeNameChiaraXd", productIdsTable);

            //foreach (var line in receiptLineList)
            //{
            //    if (productPrices.TryGetValue(Convert.ToInt32(line.IdProduct), out double price))
            //    {
            //        line.Price = price;
            //    }
            //    else
            //    {

            //    }
            //}

            return receiptLineList;
        }
        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public string Photo { get => _photo; set => _photo = value; }
        public string Code { get => _code; set => _code = value; }
        public float Price { get => _price; set => _price = value; }
    }
}

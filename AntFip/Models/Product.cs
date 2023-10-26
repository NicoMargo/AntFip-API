using IT_Arg_API.Models.Helpers;
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
            using DataTable productIdsTable = new DataTable();
            productIdsTable.Columns.Add("IdProduct", typeof(int));

            HashSet<int> uniqueProductIds = new HashSet<int>(receiptLineList.Select(r => r.IdProduct));

            foreach (var id in uniqueProductIds)
            {
                productIdsTable.Rows.Add(id);
            }

            List<Dictionary<string, object>> productPricesList = DBHelper.callProcedureDataTableReader("spProductsGetPricesById", "ProductsIdsTableType", productIdsTable);

            var priceLookup = productPricesList.ToDictionary(
                item => (int)item["id"],
                item => (double)item["price"]);

            foreach (var line in receiptLineList)
            {
                if (priceLookup.TryGetValue(line.IdProduct, out double price))
                {
                    line.Price = price;
                }
            }

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

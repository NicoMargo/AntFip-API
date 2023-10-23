namespace IT_Arg_API.Models
{
    public class ReceiptLine
    {
        private int _id;
        private string _productCode;
        private double _price;
        private int _quantity;

        public ReceiptLine()
        {

        }

        public int Id { get => _id; set => _id = value; }
        public string ProductCode { get => _productCode; set => _productCode = value; }
        public double Price { get => _price; set => _price = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
    }
}

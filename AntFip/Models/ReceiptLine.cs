namespace IT_Arg_API.Models
{
    public class ReceiptLine
    {
        private int _idProduct;
        private decimal _price;
        private int _quantity;

        public ReceiptLine()
        {

        }

        public int IdProduct { get => _idProduct; set => _idProduct = value; }
        public decimal Price { get => _price; set => _price = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }

        public decimal GetSubtotal()
        {
            return _quantity * _price;
        }
    }
}

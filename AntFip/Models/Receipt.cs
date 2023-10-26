using System.Data;

namespace IT_Arg_API.Models
{
    public class Receipt
    {
        private DateTime _date;
        private float _total;
        private string _CuitUser;
        private int _idClient;
        private List<ReceiptLine> _receiptLineList;


        public Receipt()
        {

        }

        public DateTime Date { get => _date; set => _date = value; }
        public float Total { get => _total; set => _total = value; }
        public string CuitUser { get => _CuitUser; set => _CuitUser = value; }
        public int IdClient { get => _idClient; set => _idClient = value; }
        public List<ReceiptLine> ReceiptLineList { get => _receiptLineList; set => _receiptLineList = value; }

        public double GetTotal()
        {
            Product product = new Product();
            _receiptLineList = product.GetListProductsPricesById(_receiptLineList);

            //Sum of all the subtotal of each receiptLine with 21% iva
            return _receiptLineList.Sum(line => line.GetSubtotal()) * 1.21;
        }
    }
   
}

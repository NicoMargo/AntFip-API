namespace IT_Arg_API.Models
{
    public class Receipt
    {
        private DateTime _date;
        private float _total;
        private string _CuitUser;
        private int _idClient;

        public Receipt()
        {

        }

        public DateTime Date { get => _date; set => _date = value; }
        public float Total { get => _total; set => _total = value; }
        public string CuitUser { get => _CuitUser; set => _CuitUser = value; }
        public int IdClient { get => _idClient; set => _idClient = value; }
    }
   
}

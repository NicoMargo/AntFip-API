namespace IT_Arg_API.Models
{
    public class User
    {
        private string _businessName;
        private string _password;
        private string _cuit;

        public User(string businessName, string password, string cuit)
        {
            _businessName = businessName;
            _password = password;
            _cuit = cuit;
        }

        public string BusinessName { get => _businessName; set => _businessName = value; }
        public string Password { get => _password; set => _password = value; }
        public string Cuit { get => _cuit; set => _cuit = value; }
    }
}

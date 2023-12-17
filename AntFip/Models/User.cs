using IT_Arg_API.Models.Helpers;
using System.Xml.Linq;

namespace IT_Arg_API.Models
{
    public class User
    {
        private int _id;
        private string _name;
        private string _password;
        private string _cuit;
        

        public User(string name, string password, string cuit)
        {
            _name = name;
            _password = password;
            _cuit = cuit;
        }

        public User()
        {
        }

        public string Name { get => _name; set => _name = value; }
        public string Password { get => _password; set => _password = value; }
        public string Cuit { get => _cuit; set => _cuit = value; }
        public int Id { get => _id; set => _id = value; }
    }
}

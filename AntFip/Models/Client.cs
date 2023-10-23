namespace IT_Arg_API.Models
{
    public class Client
    {
        private int? _id;
        private string _name;
        private string _surname;
        private int? _dni;
        private string _address;
        private string _phone;
        private string _email;
        public Client()
        {

        }
        public Client(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }

        //Client Create Constructor
        public Client(string name, string surname, int dni, string address, string phone, string email)
        {
            Name = name;
            Surname = surname;
            Dni = dni;
            Address = address;
            Phone = phone;
            Email = email;
        }

        public int? Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Surname { get => _surname; set => _surname = value; }
        public int? Dni { get => _dni; set => _dni = value; }
        public string Address { get => _address; set => _address = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Email { get => _email; set => _email = value; }
    }
}

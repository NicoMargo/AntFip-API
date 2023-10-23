namespace IT_Arg_API.Models.Authorization
{
    public partial class HistoryRefreshToken
    {
        private int? _idUser;

        private string _token;

        private string _refreshToken;

        private DateTime? _creationDate;

        private DateTime? _expireDate;

        public HistoryRefreshToken(int? idUser, string refreshToken, DateTime? creationDate, DateTime? expireDate)
        {
            _idUser = idUser;
            _refreshToken = refreshToken;
            _creationDate = creationDate;
            _expireDate = expireDate;
        }
        public int? IdUser { get => _idUser; set => _idUser = value; }
        public string Token { get => _token; set => _token = value; }
        public string RefreshToken { get => _refreshToken; set => _refreshToken = value; }
        public DateTime? CreationDate { get => _creationDate; set => _creationDate = value; }
        public DateTime? ExpireDate { get => _expireDate; set => _expireDate = value; }
    }
}

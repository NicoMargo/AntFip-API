namespace IT_Arg_API.Models.Authorization
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int StatusCode { get; set; }
        public string RefreshToken { get; set; }

    }
}

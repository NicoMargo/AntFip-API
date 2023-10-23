using System.ComponentModel.DataAnnotations;

namespace IT_Arg_API.Models.Authorization
{
    public class AuthorizationRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

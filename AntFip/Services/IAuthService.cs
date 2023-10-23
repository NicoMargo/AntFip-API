using IT_Arg_API.Models.Authorization;

namespace IT_Arg_API.Services
{
    public interface IAuthService
    {

        Task<Token> ReturnToken(AuthorizationRequest authorizationRequest);
        Task<Token> ReturnNewToken(string refreshToken);
        Task<int> Logout(string refreshToken);

    }
}

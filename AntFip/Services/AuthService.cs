using IT_Arg_API.Models;
using IT_Arg_API.Models.Authorization;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IT_Arg_API.Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Token> ReturnToken(AuthorizationRequest authorizationRequest)
        {
            Dictionary<string, object> args = new Dictionary<string, object> {
                     {"pName",authorizationRequest.Username},
                     {"pPassword",authorizationRequest.Password},
            };

            int idUser = Convert.ToInt32(DBHelper.callProcedureReader("spUserLogin", args));

            Token Token = new Token();

            if (idUser > 0)
            {
                Token.AccessToken = GenerateToken(idUser);
                Token.RefreshToken = GenerateRefreshToken();
                int success = SaveHistoryToken(idUser, Token.RefreshToken);
                if (success == 1)
                {
                    Token.StatusCode = 200;
                    return Token;
                }
                else
                {
                    Token.StatusCode = 500;
                    Token.AccessToken = "";
                }
            }
            return Token;

        }

        public async Task<Token> ReturnNewToken(string refreshToken)
        {

            Token tokenObj = new Token();

            if (refreshToken != null)
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pRefreshToken",refreshToken}
                };


                int idUser = Convert.ToInt32(DBHelper.callProcedureReader("spRefreshTokenCheck", args));
                tokenObj.AccessToken = GenerateToken(idUser);
                tokenObj.RefreshToken = GenerateRefreshToken();
                int success = SaveHistoryToken(idUser, tokenObj.RefreshToken);
                if (success == 1)
                {
                    tokenObj.StatusCode = 200;
                }
                else
                {
                    tokenObj.StatusCode = 500;
                }

            }
            return tokenObj;
        }

        public async Task<int> Logout(string refreshToken)
        {

            Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pRefreshToken",refreshToken}
            };

            return Convert.ToInt32(DBHelper.CallNonQuery("spRefreshTokenDelete", args));

        }
        public string GenerateToken(int idUser)
        {
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                SecurityAlgorithms.HmacSha256);

            //Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(idUser))
            };

            //Create token
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public int SaveHistoryToken(int idUser, string refreshToken)
        {
            try
            {
                HistoryRefreshToken historyRefreshToken = new HistoryRefreshToken(idUser, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

                if (idUser != null && refreshToken != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pIdUser",idUser},
                         {"pRefreshToken",refreshToken},
                         {"pCreationDate",historyRefreshToken.CreationDate},
                         {"pExpireDate",historyRefreshToken.ExpireDate}

                    };

                    return Convert.ToInt32(DBHelper.CallNonQuery("spHistoryTokenCreate", args));

                }

            }
            catch
            {
            }
            return 0;
        }

    }
}

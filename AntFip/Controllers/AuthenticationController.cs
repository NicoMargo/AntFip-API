using IT_Arg_API.Models;
using IT_Arg_API.Models.Authorization;
using IT_Arg_API.Models.Helpers;
using IT_Arg_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {

        private readonly IAuthService _authService;

        public Authentication(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthorizationRequest authorizationRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Token token = await _authService.ReturnToken(authorizationRequest);
                    if (token.StatusCode == 200)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.UtcNow.AddDays(7)
                        };

                        Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);
                        return Ok(token);
                    }
                    else
                    {
                        return NotFound("Usuario no encontrado");
                    }

                }
                return NotFound("Campos de usuario incompletos");
            }
            catch
            {
                return NotFound("Usuario no encontrado");
            }
        }


        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> NewToken(string refreshToken2)
        {
            try
            {
                Request.Cookies.TryGetValue("refreshToken", out string refreshToken);

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest("Refresh token is missing");
                }


                Token token = await _authService.ReturnNewToken(refreshToken);
                if (token.StatusCode == 200)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };
                    HttpContext.Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);

                    return Ok(token);
                }

                return NotFound("Usuario no encontrado");
            }
            catch
            {
                return NotFound("Tokens no validos");
            }
        }


        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Request.Cookies.TryGetValue("refreshToken", out string refreshToken);

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest("Refresh token is missing");
                }

                HttpContext.Response.Cookies.Delete("refreshToken");


                int respond =  await _authService.Logout(refreshToken);
                if (respond == 1)
                {
                    return Ok();
                }
            }
            catch
            {
            }
            return NotFound("Falla al hacer logout");

        }

    }
}

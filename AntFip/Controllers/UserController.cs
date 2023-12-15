using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("userLogin")]
        public async Task<IActionResult> Login(User user)
        {
            string token;
            try
            {
                if (user.BusinessName != null && user.Password != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                     {"pUsername",user.BusinessName},
                     {"pPassword",user.Password},
                    };

                    string success = DBHelper.callProcedureReader("spUserLogin", args);
                    if (success == "true")
                    {
                        token = JWT.GenerateToken(user);

                        RefreshToken refreshToken = JWT.GenerateRefreshToken();

                        return Ok(token);
                    }
                }
                return NotFound("Usuario no encontrado");
            }
            catch
            {
                return NotFound("Usuario no encontrado");
            }
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            string success = "";
            try
            {
                if (user.Password != null && user.BusinessName != null && user.Cuit != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",user.BusinessName},
                         {"pPassword",user.Password},
                         {"pCuit",user.Cuit}
                    };

                    success = Convert.ToString(DBHelper.CallNonQuery("spUserCreate", args));

                    if (success == "1")
                    {
                        return Ok(success);
                    }
                    else
                    {
                        return StatusCode(500, "Error al crear el usuario");
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error al crear el usuario" + e.Message);

            }
            return StatusCode(500, "Error al crear el usuario");

        }

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string success = "Error al eliminar el usuario.";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };

                success = DBHelper.CallNonQuery("spUserDelete", args);

                if (success == "1")
                {
                    return Ok();
                }

                return StatusCode(500, success);
            }
            catch
            {
                return StatusCode(500, success);
            }
        }


    }
}

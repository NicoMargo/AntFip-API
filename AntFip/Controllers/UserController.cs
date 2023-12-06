using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

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


    }
}

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

        // CREATE
        [HttpPost]
        public IActionResult Create([FromBody]User user)
        {
            string success = "";
            try
            {
                if (user.Password != "" && user.Name != "" && user.Cuit != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",user.Name},
                         {"pPassword",user.Password},
                         {"pCuit",user.Cuit}
                    };

                    success = Convert.ToString(DBHelper.CallNonQuery("spUserCreate", args));

                    if (success == "1")
                    {
                        success = "Usuario creado con exito";
                        return Ok(success);
                    }
                    else if(success == "-1")
                    {
                        success = "Error al crear el usuario, el cuit o el nombre de la empresa proporcionada ya existe";
                        return StatusCode(400, success);
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


        // DELETE
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
                    success = "Usuario eliminado con exito";
                    return Ok(success);
                }
                else if (success == "-1")
                {
                    success = "Error al eliminar. Usuario no encontrado";
                    return StatusCode(400, success);
                }
                
                success = "Error al eliminar. Usuario no encontrado";
                return StatusCode(500, success);
            }
            catch
            {
                return StatusCode(500, success);
            }
        }


    }
}

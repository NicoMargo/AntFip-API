using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {

        // GET BY ID USER
        [HttpGet]
        public IActionResult ClientGetByIdUser()
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pPage",0},
                    {"pIdUser", idUser}
                };
                return Ok(DBHelper.callProcedureReader("spClientGetAllByIdUser", args));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los clientes." + e.Message);
            }
        }


        // GET BY Dni
        [HttpGet("{dni}")]
        public IActionResult ClientGetByDni(int dni)
        { 
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pDni",dni},
                    {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                };
                return Ok(DBHelper.callProcedureReader("spClientGetByDni", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los clientes.");
            }
        }


        // DELETE
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string success ="Error al eliminar al cliente.";
            try
            {               
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                 
                success = DBHelper.CallNonQuery("spClientDelete", args);

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


        // CREATE
        [HttpPost]
        public IActionResult Create(Client client = null)
        {
            string success = "Error al crear al cliente." ;
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            try
            {
                if (client.Surname != "" && client.Name != "" && idUser != null && client.Dni != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",client.Name},
                         {"pSurname",client.Surname},
                         {"pDni",client.Dni},
                         {"pAddress",client.Address},
                         {"pPhone",client.Phone},
                         {"pEmail",client.Email},
                         {"pIdUser", idUser},
                    };

                    success = DBHelper.CallNonQuery("spClientCreate", args);  

                    if (success == "1")
                    {
                        return Ok();
                    }
                    else if (success == "-1")
                    {
                        success = "Error al crear el cliente, el dni proporcionado ya existe";
                        return StatusCode(400, success);
                    }
                }               
                else
                {
                    return StatusCode(500, success);
                }
                
            }                
            catch
            {
                success = "Hay campos invalidos, por favor vuelva a intentarlo.";
            }
            return StatusCode(500, success);
        }


        // UPDATE
        [HttpPost("Update")]
        public IActionResult Update(Client client)
        {
            string success = "Error al modificar al cliente.";
            try
            {
                if (client.Surname != "" && client.Name != "" && client.Id != null && client.Dni != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pId", client.Id},
                         {"pName",client.Name},
                         {"pSurname",client.Surname},
                         {"pDni",client.Dni},
                         {"pAddress",client.Address},
                         {"pPhone",client.Phone},
                         {"pEmail",client.Email},
                         {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                    };
                    success = DBHelper.CallNonQuery("spClientUpdate", args);
                    if (success == "1")
                    {
                        return Ok();
                    }
                    else if (success == "-1")
                    {
                        success = "Error al crear el cliente, el dni proporcionado ya existe";
                        return StatusCode(400, success);
                    }
                    else
                    {
                        return StatusCode(500, success);
                    }
                }
                else
                {
                    success = "Hay campos invalidos, por favor vuelva a intentarlo.";
                }
            }
            catch
            {
            }
            return StatusCode(500, success);
        }
         
    }
}

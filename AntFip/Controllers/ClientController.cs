using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IActionResult ClientGetByIdUser()
        {
            int id = 2;
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pPage",0},
                    {"pIdUser", id}
                };
                return Ok(DBHelper.callProcedureReader("spClientGetAllByIdUser", args));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los clientes." + e.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult ClientGetById(int id)
        { 
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                return Ok(DBHelper.callProcedureReader("spClientGetById", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los clientes.");
            }
        }

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

        [HttpPost]
        public IActionResult Create(Client client = null)
        {
            string success = "Error al crear al cliente." ;
            try
            {
                if (client.Surname != null && client.Name != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",client.Name},
                         {"pSurname",client.Surname},
                         {"pDni",client.Dni},
                         {"pAddress",client.Address},
                         {"pPhone",client.Phone},
                         {"pEmail",client.Email},
                    };
                    success = DBHelper.CallNonQuery("spClientCreate", args);  
                    if (success == "1")
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500, success);
                    }
                }
            }                
            catch
            {
            }
            return StatusCode(500, success);
        }

        [HttpPost("Update")]
        public IActionResult Update(Client client)
        {
            string success = "Error al modificar al cliente.";
            try
            {
                if (client.Surname != null && client.Name != null && client.Id != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pId", client.Id},
                         {"pName",client.Name},
                         {"pSurname",client.Surname},
                         {"pDni",client.Dni},
                         {"pAddress",client.Address},
                         {"pPhone",client.Phone},
                         {"pEmail",client.Email},
                    };
                    success = DBHelper.CallNonQuery("spClientUpdate", args);
                    if (success == "1")
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500, success);
                    }
                }
                else
                {
                    success = "Hay campos que no pueden estar vacios, vuelva a intentarlo.";
                }
            }
            catch
            {
            }
            return StatusCode(500, success);
        }
         
    }
}

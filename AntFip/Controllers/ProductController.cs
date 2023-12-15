using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult ProductGet()
        {
            try
            {
                return Ok(DBHelper.callProcedureReader("spProductGetAll"));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los productos." + e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ProductGetById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                return Ok(DBHelper.callProcedureReader("spProductGetById", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los productos.");
            }
        }

        [HttpGet("IdUser")]
        public IActionResult ProductGetByIdUser()
        {
            int? idUser = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pIdUser", idUser}
                };
                return Ok(DBHelper.callProcedureReader("spProductGetByIdUser", args));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los productos." + e.Message);
            }
        }

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string success = "Error al eliminar el producto.";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };

                success = DBHelper.CallNonQuery("spProductDelete", args);

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
        public IActionResult Create(Product product = null)
        {
            string success = "Error al crear el producto.";
            try
            {
                if (product.Name != null && product.Description != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",product.Name},
                         {"pDescription",product.Description},
                         {"pStock",product.Stock},
                         {"pPhoto",product.Photo},
                         {"pCode",product.Code},
                         {"pPrice",product.Price},
                    };

                    success = DBHelper.CallNonQuery("spProductCreate", args);

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
        public IActionResult Update(Product product)
        {
            string success = "Error al modificar el producto.";
            try
            {
                if (product.Name != null && product.Description != null && product.Id != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pId", product.Id},
                         {"pName",product.Name},
                         {"pDescription",product.Description},
                         {"pStock",product.Stock},
                         {"pPhoto",product.Photo},
                         {"pCode",product.Code},
                         {"pPrice",product.Price},
                    };

                    success = DBHelper.CallNonQuery("spProductUpdate", args);

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
                    success = "Hay campos invalidos, por favor vuelva a intentar";
                }
            }
            catch
            {
            }
            return StatusCode(500, success);
        }

    }
}

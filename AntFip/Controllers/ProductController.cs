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
    public class ProductController : ControllerBase
    {

        // GET ALL
        [HttpGet]
        public IActionResult ProductGet()
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                };

                return Ok(DBHelper.callProcedureReader("spProductGetByIdUser", args));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los productos." + e.Message);
            }
        }


        // GET BY ID
        [HttpGet("{id}")]
        public IActionResult ProductGetById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id},
                    {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                };
                return Ok(DBHelper.callProcedureReader("spProductGetById", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los productos.");
            }
        }

        // GET BY CODE
        [HttpGet("code/{idCode}")]
        public IActionResult ProductGetByCode(int idCode)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pCode",idCode},
                    {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                };
                return Ok(DBHelper.callProcedureReader("spProductGetByCode", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los productos.");
            }
        }


        // DELETE
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


        // CREATE
        [HttpPost]
        public IActionResult Create([FromBody]Product product)
        {
            string success = "Error al crear el producto.";
            try
            {
                if (product.Name != "" && product.Description != null && product.Stock != null && product.Code != null && product.Price != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",product.Name},
                         {"pDescription",product.Description},
                         {"pStock",product.Stock},
                         {"pPhoto",product.Photo},
                         {"pCode",product.Code},
                         {"pPrice",product.Price},
                         {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}
                    };

                    success = DBHelper.CallNonQuery("spProductCreate", args);

                    if (success == "1")
                    {
                        return Ok();
                    }
                    else if (success == "-1")
                    {
                        success = "Error al crear el producto, el codigo proporcionado ya existe";
                        return StatusCode(400, success);
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


        // UPDATE
        [HttpPost("Update/{id}")]
        public IActionResult Update(Product product, int id)
        {
            string success = "Error al modificar el producto.";
            try
            {
                if (product.Name != "" && product.Description != null && id != null && product.Stock != null && product.Code != null && product.Price != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pId", id},
                         {"pName",product.Name},
                         {"pDescription",product.Description},
                         {"pStock",product.Stock},
                         {"pPhoto",product.Photo},
                         {"pCode",product.Code},
                         {"pPrice",product.Price},
                         {"pIdUser", Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)}

                    };

                    success = DBHelper.CallNonQuery("spProductUpdate", args);

                    if (success == "1")
                    {
                        return Ok();
                    }
                    else if (success == "-1")
                    {
                        success = "Error al crear el producto, el codigo proporcionado ya existe";
                        return StatusCode(400, success);
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

using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        [HttpGet]        
        public IActionResult CourseGet()
        {
            try
            {
                return Ok(DBHelper.callProcedureReader("spCourseGetAll"));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los cursos");
            }
        }

        [HttpGet("{id}")]
        public IActionResult CourseGetWithModulesByIdGroup(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                return Ok(DBHelper.callProcedureReader("spCourseGetByIdGroup", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los cursos");
            }
        }



    }
}

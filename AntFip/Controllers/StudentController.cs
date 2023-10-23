using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult StudentGet()
        {
            try
            {
                return Ok(DBHelper.callProcedureReader("spStudentGetAll"));
            }
            catch (Exception e)
            {

                return StatusCode(500, "Error al obtener la informacion de los alumnos " + e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("StudentGetShort")]
        public IActionResult StudentGetShort()
        {
            try
            {
                return Ok(DBHelper.callProcedureReader("spStudentGetAllShort"));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los alumnos");
            }
        }

        [HttpGet("StudentGetShort/{idGroup}")]
        public IActionResult StudentGetShortByGroup(int idGroup)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pIdGroup",idGroup}
                };
                return Ok(DBHelper.callProcedureReader("spStudentGetAllShortByGroup", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los alumnos");
            }
        }

        [HttpGet("{id}")]
        public IActionResult StudentGetById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                return Ok(DBHelper.callProcedureReader("spStudentGetById", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los alumnos");
            }
        }

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string success ="Error al eliminar al estudiante";
            try
            {               
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };

                success = DBHelper.CallNonQuery("spStudentDelete", args);

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
        public IActionResult Create(Client? student = null)
        {
            string success = "Error al crear al alumno" ;
            try
            {
                if (student.Surname != null && student.Name != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pName",student.Name},
                         {"pSurname",student.Surname},
                         {"pDni",student.Dni},
                         {"pAddress",student.Address},
                         {"pPhone",student.Phone},
                         {"pEmail",student.Email},
                         {"pBirthday",student.Birthdate}
                    };
                    success = DBHelper.CallNonQuery("spStudentCreate", args);  
                    if (success == "2")
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
        public IActionResult Update(Client student = null)
        {
            string success = "Error al modificar el alumno";
            try
            {
                if (student.Surname != null && student.Name != null && student.Id != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                        {"pId", student.Id},
                         {"pName",student.Name},
                         {"pSurname",student.Surname},
                         {"pDni",student.Dni},
                         {"pAddress",student.Address},
                         {"pPhone",student.Phone},
                         {"pEmail",student.Email},
                         {"pBirthday",student.Birthdate}
                    };
                    success = DBHelper.CallNonQuery("spStudentUpdate", args);
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
                    success = "El nombre y apellido no pueden estar vacios";
                }
            }
            catch
            {
            }
            return StatusCode(500, success);
        }

    }
}

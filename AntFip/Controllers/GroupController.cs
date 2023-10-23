using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace IT_Arg_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        //private readonly ILogger _logger;

        //public GroupController(ILogger<GroupController> logger)
        //{
        //    _logger = logger;
        //}


        [HttpGet]
        public IActionResult GroupsGet(ILogger<GroupController> logger)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object>
                {
                };
                return Ok(DBHelper.callProcedureReader("spCourseGroupGetAll", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion de los grupos");
            }
        }

        //Returns all the modules with all the students, certificates, module finished and mentions
        [HttpGet("AllDataGroup/{id}")]
        public IActionResult GroupModulesCertificatesGetById(int id)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };
                return Ok(DBHelper.callProcedureReader("spCertificatesGetByGroup", args));
            }
            catch
            {
                return StatusCode(500, "Error al obtener la informacion del curso");
            }
        }

        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string success = "Error al eliminar el grupo";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pId",id}
                };

                success = DBHelper.CallNonQuery("spCourseGroupDelete", args);

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

        [HttpPost("DeleteStudentFromGroup/{idStudent}/{idGroup}")]
        public IActionResult DeleteStudentFromGroup(int idStudent, int idGroup)
        {
            string success = "Error al eliminar el alumno del grupo";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pIdStudent",idStudent},
                    {"pIdGroup",idGroup}
                };

                success = DBHelper.CallNonQuery("spCourseGroupStudentDelete", args);

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

        [HttpPost("AddStudentFromGroup/{idStudent}/{idGroup}")]
        public IActionResult AddStudentFromGroup(int idStudent, int idGroup)
        {
            string success = "Error al agregar el alumno al grupo";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pIdStudent",idStudent},
                    {"pIdGroup",idGroup}
                };

                success = DBHelper.CallNonQuery("spCourseGroupStudentAdd", args);

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
        public IActionResult Create(Models.Group group)
        {
            string success = "Error al crear al grupo";
            try
            {
                if (group.IdCourse != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pIdCourse",group.IdCourse},
                    };

                    DataTable studentIdTable = new DataTable();
                    studentIdTable.Columns.Add("id", typeof(int));

                    foreach (int studentId in group.IdStudents)
                    {
                        studentIdTable.Rows.Add(studentId);
                    }

                    success = DBHelper.CallNonQueryTable("spCourseGroupCreate", args, studentIdTable, "StudentsGroupType");

                    if (Convert.ToInt32(success) > 0)
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

        [HttpPost("UpdateGroupCertificates")]
        public IActionResult UpdateGroupCertificates(List<List<Dictionary<string, JsonElement>>> dataGroup)
        {
            string success = "Error al modificar al grupo";
            try
            {
                if (dataGroup.Count > 0)
                {

                    Dictionary<string, object> args = new Dictionary<string, object> {
                        {"pIdModule",null},
                        {"pIdStudent",null},
                        {"pFinished",null},
                        {"pHasCertificate",null},
                        {"pHasMention",null},
                    }
;

                    List<Certificate> certificatesToCreate = new List<Certificate>();
                    Certificate certficateClass = new Certificate();

                    //The data group is an array of all the modules an each module has all the students to be modified
                    foreach (List<Dictionary<string, JsonElement>> dataList in dataGroup)
                    {
                        foreach (Dictionary<string, JsonElement> oneRow in dataList)
                        {
                            args["pIdModule"] = oneRow["idModule"].GetInt32();
                            args["pIdStudent"] = oneRow["idStudent"].GetInt32();
                            args["pFinished"] = oneRow["finished"].GetBoolean();
                            args["pHasCertificate"] = oneRow["hasCertificate"].GetBoolean();
                            args["pHasMention"] = oneRow["hasMention"].GetBoolean();

                            success = DBHelper.callProcedureReader("spCourseGroupUpdate", args);


                            if (oneRow["hasCertificate"].GetBoolean() && success == "1")
                            {
                                certificatesToCreate.Add(new Certificate() { StudentId = oneRow["idStudent"].GetInt32(), ModuleId = oneRow["idModule"].GetInt32(), IsMention = oneRow["hasMention"].GetBoolean() });
                            }
                            else if (success != "1")
                            {
                                if (!deleteImgCertifcate(success, oneRow["hasCertificate"].GetBoolean(), oneRow["hasMention"].GetBoolean()))
                                {
                                    return StatusCode(500, "Error al eliminar las fotos de los certificados");
                                }
                            }

                        }
                        if (certificatesToCreate.Count > 0)
                        {
                            try
                            {
                                certficateClass.certificateCreate(certificatesToCreate);
                                certificatesToCreate.Clear();
                            }
                            catch
                            {
                                return StatusCode(500, "Error al crear el certificado");
                            }
                        }
                    }

                    return Ok();
                }
                else
                {
                    return StatusCode(500, success);
                }

            }
            catch
            {
                return StatusCode(500, success);
            }

            bool deleteImgCertifcate(string code, bool hasCertificate, bool hasMention)
            {
                string directoryPath = Path.Combine("wwwroot", "imgCertificates", code + ".jpg");

                try
                {
                    if(!hasCertificate)
                    {
                        System.IO.File.Delete(directoryPath);
                    }
                    if(!hasMention)
                    {
                        string newFileName = Path.GetFileNameWithoutExtension(code) + "m.jpg";
                        directoryPath = Path.Combine("wwwroot", "imgCertificates", newFileName);
                        System.IO.File.Delete(directoryPath);
                    }
                  
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

    }

}

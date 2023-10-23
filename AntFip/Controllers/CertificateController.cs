using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Reflection;
using System.Text.Json;


namespace IT_Arg_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CertificateController : ControllerBase
    {
        /*private readonly ILogger<CertificateController> _logger;

        public CertificateController(ILogger<CertificateController> logger)
        {
            _logger = logger;
        }*/

        [HttpGet]
        [Route("{code}")]
        public IActionResult certificateGet(string code)
        {
            try
            {
                if (code.EndsWith('m'))
                {
                    code = code.Remove(code.Length - 1);
                }
                Dictionary<string, object> args = new Dictionary<string, object> {
                 {"pCode",code},
                };
                return Ok(DBHelper.callProcedureReader("spCertificateGetSmallByCode", args));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }


        //Returns all the certificates code from one student using student code
        [HttpGet]
        [Route("certificatesGetCode/{studentCode}")]
        public IActionResult certificatesGetCode(string studentCode)
        {
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                 {"pStudentCode",studentCode},
                };
                return Ok(DBHelper.callProcedureReader("spCertificatesGetByCodeStudent", args));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("downloadCertificate/{code}")]
        public IActionResult downloadCertificate(string code)
        {
            try
            {
                if (code != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","imgCertificates", code + ".jpg");

                    // check if file exists
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound();
                    }

                    // Get the MIME of the file
                    var mimeType = GetMimeType(filePath);

                    return PhysicalFile(filePath, mimeType, code + ".jpg");
                }
            }
            catch
            {
            }
            return StatusCode(500);
        }

        [HttpGet]
        [Route("certificateShare/{code}")]
        public ContentResult certificateShare(string code)
        {
            return base.Content("<!doctype html> <html> <head> <meta charset='utf-8'> <title>IT-Arg</title> <meta property='og:url' content='https://api.it-arg.com/imgCertificates/" + code + ".jpg' /> <meta property='og:type' content='website' /> <meta property='og:title' content='Certificado' /> <meta property='og:description' content='Certificado de IT-Arg' /> <meta property='og:image' https://api.it-arg.com/imgCertificates/" + code + ".jpg' /> <meta property='og:image:type' content='image/png' /> </head> <body><img width='75%' src='https://api.it-arg.com/imgCertificates/" + code + ".jpg' alt='certificado'/> </body> </html>", "text/html");
        }

        [HttpPost("certificateRegenerate/{idModule}")]
        public string certificateRegenerate(int idModule)
        {
            string err = "";
            try
            {
                Dictionary<string, object> args = new Dictionary<string, object> {
                    {"pModuleId",idModule}
                };

                JsonDocument docModule = JsonDocument.Parse(DBHelper.callProcedureReader("spModuleGetById", args));
                JsonElement root = docModule.RootElement;

                var module = root[0];

                List<string> courseItems = new List<string>();
                foreach (JsonElement description in root[1].EnumerateArray())
                    courseItems.Add(Convert.ToString(description.GetProperty("description")));


                JsonDocument docStudents = JsonDocument.Parse(DBHelper.callProcedureReader("spCertificatesGetByModuleId", args));

                foreach (var item in docStudents.RootElement.EnumerateArray())
                {

                   CertificateDraw.drawCertificate(
                   item.GetProperty("studentName").GetString(),
                   item.GetProperty("studentSurname").GetString(),
                   module.GetProperty("courseName").GetString(),
                   module.GetProperty("moduleName").GetString(),
                   item.GetProperty("certificateDate").GetString(),
                   item.GetProperty("code").GetString(),
                   courseItems);

                }
                courseItems.Clear();
                docModule.Dispose();
                docStudents.Dispose();
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return err;
        }

        [HttpPost]
        [Route("certificateTemplate")]
        [Authorize]
        public bool certificateTemplate()
        {
            try
            {
                CertificateDraw.drawCertificateTemplate();
                return true;
            }
            catch
            {
                return false;
            }

        }

        [HttpPost]
        [Route("mentionTemplate")]
        [Authorize]
        public IActionResult MentionTemplate()
        {
            try
            {
                CertificateDraw.drawMentionTemplate();
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPost]
        public IActionResult certificateCreate(List <Certificate> listCertificates)
        {
            string err = "";
            try
            {
                Certificate certficateClass = new Certificate();
                if (certficateClass.certificateCreate(listCertificates))
                {
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            catch (Exception ex)
            {
                err = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

        }


        private string GetMimeType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var mimeType))
            {
                mimeType = "application/octet-stream"; 
            }
            return mimeType;
        }

    }
}
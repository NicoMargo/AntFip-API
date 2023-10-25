using IT_Arg_API.Models;
using IT_Arg_API.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT_Arg_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        // GET api/<User>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }        
       

        [HttpPost]
        [Route("userCreate")]
        [Authorize]       
        public string Create(User user)
        {
            string success = "" ;
            try
            {
                if (user.Password != null && user.BusinessName != null)
                {
                    Dictionary<string, object> args = new Dictionary<string, object> {
                         {"pBusinessName",user.BusinessName},
                         {"pPassword",user.Password},
                    };
                    success = Convert.ToString(DBHelper.callProcedureReader("spUserCreate", args));                    
                }
            }                
            catch
            {                
            }
            return success;
        }

        
    }
}

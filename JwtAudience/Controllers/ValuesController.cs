using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAudience.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [BearerAuthorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
    }
}

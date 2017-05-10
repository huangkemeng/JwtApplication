using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAudience.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly AudienceDbContext _dbContext;

        public TokenController(AudienceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get() => Json(_dbContext.BlackRecords);

        // DELETE api/values/5
        [BearerAuthorize]
        [HttpDelete]
        public IActionResult Delete()
        {
            var jti = User.FindFirst("jti")?.Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (jti == null)
            {
                HttpContext.Response.StatusCode = 400;
                return Json(new { Result = false });
            }
            _dbContext.BlackRecords.Add(new BlackRecord { Jti = jti, UserId = userId });
            _dbContext.SaveChanges();
            return Json(new {Result = true});
        }
    }
}

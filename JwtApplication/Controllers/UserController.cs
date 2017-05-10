using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtIssuer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AuthDbContext _dbContext;

        public UserController(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // POST api/user
        [HttpPost]
        public void Post([FromBody]User newUser)
        {
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using JwtUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtIssuer.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly JWTTokenOptions _tokenOptions;
        private readonly AuthDbContext _dbContext;

        public TokenController(JWTTokenOptions tokenOptions, AuthDbContext dbContext)
        {
            _tokenOptions = tokenOptions;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 生成一个新的 Token
        /// </summary>
        /// <param name="user">用户信息实体</param>
        /// <param name="expire">token 过期时间</param>
        /// <param name="audience">Token 接收者</param>
        /// <returns></returns>
        private string CreateToken(User user, DateTime expire, string audience)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer32)
            };
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(user.Username, "TokenAuth"), claims);
            var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor()
            {
                Issuer = "TestIssuer",
                Audience = audience,
                SigningCredentials = _tokenOptions.Credentials,
                Subject = identity,
                Expires = expire
            });
            return token;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/token/audience
        [HttpPost("{audience}")]
        public IActionResult Post([FromBody]User user, [FromQuery] string audience)
        {
            DateTime expire = DateTime.Now.AddDays(7);

            var result = _dbContext.Users.First(u => u.Username == user.Username && u.Password == user.Password);
            if (result == null)
            {
                return Json(new { Error = "用户名或密码错误" });
            }
            return Json(new {Token = CreateToken(result, expire, "TestAudience")});
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

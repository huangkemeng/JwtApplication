using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace JwtAudience
{
    public class ValidJtiRequirement : IAuthorizationRequirement
    {
    }

    public class ValidJtiHandler : AuthorizationHandler<ValidJtiRequirement>
    {
        private readonly AudienceDbContext _dbContext;

        public ValidJtiHandler(AudienceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJtiRequirement requirement)
        {
            Console.WriteLine("validate jti");
            // 检查 Jti 是否存在
            var jti = context.User.FindFirst("jti")?.Value;
            if (jti == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // 检查 jti 是否在黑名单
            var tokenExists = _dbContext.BlackRecords.Any(r => r.Jti == jti);
            if (tokenExists)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;

        }
    }
}

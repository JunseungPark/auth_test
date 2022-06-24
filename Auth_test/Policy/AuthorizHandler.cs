using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Auth_test.Policy
{
    public class AuthorizHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private HttpContext _cont;
        public AuthorizHandler(IHttpContextAccessor accessor) {
            _cont = accessor.HttpContext;
        }
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               AuthorizationRequirement requirement)
        {

            if (_cont.Request.Cookies != null) {
                var value = _cont.Request.Cookies;
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

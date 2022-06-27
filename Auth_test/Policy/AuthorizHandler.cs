using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Auth_test.Policy
{
    public class AuthorizHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               AuthorizationRequirement requirement)
        {
            HttpContextAccessor IHttpContextAccessor = new HttpContextAccessor();
            string Cookies = IHttpContextAccessor.HttpContext.Request.Cookies["Set-Cookies"];
            if (Cookies != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

    }
}

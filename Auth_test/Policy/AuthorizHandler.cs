using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            else {
                context.Fail();
            } 
            return Task.CompletedTask;
        }

    }
}

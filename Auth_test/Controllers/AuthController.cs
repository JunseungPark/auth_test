using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Security.Claims;

namespace Auth_test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private HttpContext _context;

        public AuthController(IHttpContextAccessor accessor)
        {
            _context = accessor.HttpContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public void Authorization()
        {
            Console.WriteLine("성공");
        }

        [HttpGet]
        public RedirectResult Callback()
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            //Create the identity for the user
            identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "dpfwl8745"),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            isAuthenticated = true;

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);

                var login = _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectPermanent("/api/Auth/Authorization");
            }

            return RedirectPermanent("/");
        }

        [HttpGet]
        public void Forbidden()
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);

            paramReturnUrl = exists ? _context.Request.RouteValues + paramReturnUrl[0] : string.Empty;
            Console.WriteLine($"{paramReturnUrl}에 대한 권한 없음");

            RedirectPermanent("/api/Auth/Login");
        }
    }
}

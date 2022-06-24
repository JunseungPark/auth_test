using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth_test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private HttpContext _context;
        private IConfiguration _configuration;

        public AuthController(IHttpContextAccessor accessor, IConfiguration config)
        {
            _context = accessor.HttpContext;
            _configuration = config;
        }

        [HttpGet]
        [Authorize(Policy = "Over21")]
        public void Authorization()
        {
            Console.WriteLine("권한있음");
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

                //_context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _context.Response.Headers.Add("Set-Cookie", GenerateJwtToken("dpfwl8745"));

                return RedirectPermanent("/api/Auth/Authorization");
            }

            return RedirectPermanent("/");
        }

        private string GenerateJwtToken(string username)
        {
            //ClaimsIdentity identity = null;
            //identity = new ClaimsIdentity(new[] {
            //    new Claim(JwtRegisteredClaimNames.Sub, username),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(ClaimTypes.NameIdentifier, username),
            //    new Claim(ClaimTypes.Role, "User") }
            //);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpGet]
        public RedirectResult Forbidden()
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);

            paramReturnUrl = exists ? _context.Request.RouteValues + paramReturnUrl[0] : string.Empty;
            Console.WriteLine($"{paramReturnUrl}에 대한 권한 없음");

            return RedirectPermanent("/api/Home/Index");
        }
        
        public RedirectResult googlelogin()
        {
            return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar.readonly&access_type=offline&include_granted_scopes=true&response_type=code&state=state_parameter_passthrough_value&redirect_uri=https%3A%2F%2Flocalhost%3A44364%2Fapi%2FAuth%2FCallback&client_id=148638893775-f1bbqle3vgkmjvkhiadki3c79snbkpcp.apps.googleusercontent.com");
        }
    }
}

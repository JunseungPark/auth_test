using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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
        [Authorize(Policy = "Check")]
        public IActionResult Authorization()
        {
            //로그인이 되면 여기서 code 확인가능
           return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar.readonly%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar&access_type=offline&include_granted_scopes=true&response_type=code&state=state_parameter_passthrough_value&redirect_uri=https%3A%2F%2Flocalhost%3A44364%2Fapi%2FHome%2Freceivepage&client_id=148638893775-f1bbqle3vgkmjvkhiadki3c79snbkpcp.apps.googleusercontent.com");
        }

        [HttpGet]
        [AllowAnonymous]
        public RedirectResult Callback(string code)
        {

            string _code = code;
            bool isAuthenticated = false;

            if (_code != null) isAuthenticated = true;

            if (isAuthenticated)
            {
                _context.Response.Cookies.Append("Set-Cookies", GenerateJwtToken("dpfwl8745"));
                return Redirect("/api/Home/CheckAuth");
            }
            return Redirect("/");
        }

        private string GenerateJwtToken(string username)
        {

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
        [AllowAnonymous]
        public RedirectResult Forbidden()
        {
            StringValues paramReturnUrl;
            bool exists = _context.Request.Query.TryGetValue("returnUrl", out paramReturnUrl);

            paramReturnUrl = exists ? _context.Request.RouteValues + paramReturnUrl[0] : string.Empty;

            return Redirect("/api/Home/Index");
        }

        public RedirectResult googlelogin()
        {

            return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar.readonly&access_type=offline&include_granted_scopes=true&response_type=code&state=state_parameter_passthrough_value&redirect_uri=https%3A%2F%2Flocalhost%3A44364%2Fapi%2FAuth%2FCallback&client_id=148638893775-f1bbqle3vgkmjvkhiadki3c79snbkpcp.apps.googleusercontent.com");

            //return Redirect("https://accounts.google.com/o/oauth2/v2/auth?scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar.readonly%20https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fcalendar&access_type=offline&include_granted_scopes=true&response_type=code&state=state_parameter_passthrough_value&redirect_uri=https%3A%2F%2Flocalhost%3A44364%2Fapi%2FHome%2Freceivepage&client_id=148638893775-f1bbqle3vgkmjvkhiadki3c79snbkpcp.apps.googleusercontent.com");
        }
    }
}

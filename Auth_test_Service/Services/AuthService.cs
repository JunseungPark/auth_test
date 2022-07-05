using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Auth_test_Service.Interfaces;

namespace Auth_test_Service.Services
{
    public class AuthService : IAuthService
    {
        private HttpContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(IHttpContextAccessor accessor, IConfiguration config)
        {
            _context = accessor.HttpContext;
            _configuration = config;
        }


        private string isCallBack(string code) {
            string _code = code;
            bool isAuthenticated = false;

            if (_code != null) isAuthenticated = true;

            if (isAuthenticated)
            {
                _context.Response.Cookies.Append("Set-Cookies", GenerateJwtToken("dpfwl8745"));
                return "/api/Home/CheckAuth";
            }
            return "/";
        }
        private string GenerateJwtToken(string username)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Issuer"],
                _configuration["Audience"],
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        string IAuthService.isCallBack(string code)
        {
            return isCallBack(code);
        }
    }
}

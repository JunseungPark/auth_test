using Auth_test_Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Auth_test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private HttpContext _context;
        public IAuthService _authService;

        public AuthController(IHttpContextAccessor accessor, IAuthService auth)
        {
            _context = accessor.HttpContext;
            _authService = auth;
        }

        [HttpGet]
        [Authorize(Policy = "Check")]
        public IActionResult Authorization()
        {
            return Redirect("/api/Home/privacy");
        }

        [HttpGet]
        [AllowAnonymous]
        public RedirectResult Callback(string code)
        {

            string url = _authService.isCallBack(code);
            return Redirect(url);
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
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth_test.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            HttpContextAccessor IHttpContextAccessor = new HttpContextAccessor();
            string Cookies = IHttpContextAccessor.HttpContext.Request.Cookies["Set-Cookies"];
            if (Cookies != null)
            {
                TempData["isCookies"] = false;
            }
            else {
                TempData["isCookies"] = true;
            }
            return View();           
        }
        public RedirectResult CheckAuth()
        {   
            return Redirect("/api/Auth/Authorization");
        }
        public IActionResult Login()
        {
            return RedirectPermanent("/api/auth/googlelogin");
        }

        [Authorize(Policy = "Check")]
        public IActionResult privacy()
        {
            return View();
        }
    }
}

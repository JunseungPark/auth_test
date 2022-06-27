using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_test.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
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

        public IActionResult privacy()
        {
            return View();
        }
    }
}

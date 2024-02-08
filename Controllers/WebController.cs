using Microsoft.AspNetCore.Mvc;

namespace ScopeIndia.Controllers
{
    public class WebController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult StudentDashboard()
        {
            return View();
        }
    }
}




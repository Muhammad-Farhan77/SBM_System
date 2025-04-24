using Microsoft.AspNetCore.Mvc;

namespace Self.Controllers.Controllers

//namespace YourProjectNamespace.Controllers
{
    public class SurpriseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gift()
        {
            return View();
        }
    }
}


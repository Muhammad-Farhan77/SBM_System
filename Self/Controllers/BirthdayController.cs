using Microsoft.AspNetCore.Mvc;

namespace Self.Controllers
{
    public class BirthdayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBM_System_Frontend.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}
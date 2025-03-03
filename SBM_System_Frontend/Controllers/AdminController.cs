using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBM_System_Frontend.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult CreateService()
        {
            ViewBag.Message = "Your contact new service page.";

            return View();
        }

        public ActionResult UpdateService()
        {
            ViewBag.Message = "Edit your service here.";
            return View();
        }
    }
}
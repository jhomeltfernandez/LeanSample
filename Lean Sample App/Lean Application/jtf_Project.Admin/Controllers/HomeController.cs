using jtf_Project.Admin.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Truck Monitoring")]
    public class HomeController : Controller
    {
       [Authorize(Roles = ConstantRoles.AdminRoles)]
       [DisplayName("Sample Application")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using jtf_Project.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jtf_Project.Admin.Controllers
{
    [DisplayName("Report")]
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        [DisplayName("Specific Date")]
        public ActionResult SpecificDate()
        {
            ViewBag.Trucks = new SelectList(Context.TruckRepo.Get(s => s.Deleted == false).ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult SpecificDate(DateTime date, int truckid = 0)
        {
            if (date == null || truckid == 0)
            {
                ModelState.AddModelError("", "Please enter a Value for on all fields");

                ViewBag.Trucks = new SelectList(Context.TruckRepo.Get(s => s.Deleted == false).ToList(), "Id", "Name", truckid);

                return View();
            }

            return Redirect("~/Reports/TruckSaleReport.aspx?truckid=" + truckid + "&startDate=" + date);
        }


        [DisplayName("Date Range")]
        public ActionResult DateRange()
        {
            ViewBag.Trucks = new SelectList(Context.TruckRepo.Get(s => s.Deleted == false).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DateRange(DateTime start, DateTime end, int truckid = 0)
        {
            if (start == null || end == null || truckid == 0)
            {
                ModelState.AddModelError("", "Please enter a Value for on all fields");

                ViewBag.Trucks = new SelectList(Context.TruckRepo.Get(s=>s.Deleted == false).ToList(), "Id", "Name", truckid);

                return View();
            }

            return Redirect("~/Reports/TruckSaleReport.aspx?truckid=" + truckid + "&startDate=" + start + "&endDate=" + end);
        }


        [DisplayName("Annual")]
        public ActionResult Annual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TimeRange(DateTime date, int truckid)
        {
            return View();
        }
    }
}
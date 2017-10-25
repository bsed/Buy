using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
namespace Buy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Comm.IsMobileDrive)
            {
                return RedirectToAction("Index", "Coupon");
            }
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

        [Authorize]
        public ActionResult Backstage()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(s => s.UserName == User.Identity.Name);
                if (user.UserType != Enums.UserType.System)
                {
                    return RedirectToAction("Login", "Account", new { ReturnUrl = Url.Action() });
                }
            }

            return View();
        }
    }
}
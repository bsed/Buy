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

            if (Comm.IsWeChat)
                ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Download()
        {
            //string sda = "~/Download/malieme.apk";
            if (Comm.IsMobileDrive)
            {
                string agent = System.Web.HttpContext.Current.Request.UserAgent.ToLower();
                var type = agent.Contains("iphone") ? Enums.UpdateLogType.IOS : Enums.UpdateLogType.Android;
                if (type == Enums.UpdateLogType.Android)
                {
                    string filePath = Server.MapPath("~/Download/malieme.apk");//路径
                    return File(filePath, "text/plain", "malieme.apk"); //arrow-right.png是客户端保存的名字
                }
                else
                {
                    return Redirect("https://itunes.apple.com/cn/app/id1294184032?mt=8");
                }
            }
            return RedirectToAction("Index", "Home");
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
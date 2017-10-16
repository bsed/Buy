using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;

namespace Buy.Controllers
{
    public class ProxyController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetChild(string userID, int? page = 1)
        {
            var query = db.Users
                .Where(s => s.ParentUserID == userID)
                .OrderByDescending(s => s.RegisterDateTime);
            var paged = query.ToPagedList(page.Value);
            var model = paged
                .Select(s => new UserViewModel(s))
                .ToList();
            return Json(Comm.ToJsonResultForPagedList(paged, model)); ;
        }

        

        [HttpPost]
        public ActionResult Apply(string userID, string proxyID)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == userID);
            if (user == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "用户不存在"));
            }

            var proxy = db.Users.FirstOrDefault(s => s.Id == proxyID);
            if (proxy == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "用户不存在"));
            }

            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
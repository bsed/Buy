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
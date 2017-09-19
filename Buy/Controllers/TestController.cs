using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Buy.Controllers
{
    public class TestController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index(string phone)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //var c = db.CouponTypes
                //   .Where(s => s.Platform == Enums.CouponPlatform.MoGuJie && s.Keyword != null)
                //   .ToList();
                //foreach (var item in c)
                //{
                //    item.Keyword = item.Keyword.Replace(";", ",");
                //}
                //db.SaveChanges();
                var cid = db.CouponTypes
                    .Where(s => s.Platform == Enums.CouponPlatform.MoGuJie && s.Keyword != null)
                    .ToList()
                    .Select(s => s.Keyword.SplitToArray<string>(','))
                    .SelectMany(s => s)
                    .ToList();
                return Json(Comm.ToJsonResult("Success", "", cid), JsonRequestBehavior.AllowGet);
            }

        }
    }

}
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
        public ActionResult Index()
        {
            List<Models.Coupon> models = new List<Coupon>();
            for (int i = 0; i < 10; i++)
            {
                models.Add(new Coupon
                {
                    Commission = 1,
                    CommissionRate = 0.1m,
                    CreateDateTime = DateTime.Now,
                    EndDateTime = DateTime.Now.AddDays(3),
                    Link = "http://www.baidu.com/",
                    Name = "测试",
                    Left = 100,
                    Platform = Enums.CouponPlatform.MoGuJie
                });
            }
            var dLinks = models.GroupBy(s => s.Link).Select(s => new
            {
                Link = s.Key,
                Count = s.Count()
            }).Where(s => s.Count > 1)
                .Select(s => s.Link)
                .ToList();
            foreach (var link in dLinks)
            {
                var dModels = models.Where(s => s.Link == link)
                     .Skip(1).ToList();
                models.RemoveAll(s => dModels.Contains(s));
            }

            return Json(Comm.ToJsonResult("Success", ""), JsonRequestBehavior.AllowGet);
        }
    }

}
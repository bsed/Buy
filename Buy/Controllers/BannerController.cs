using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
namespace Buy.Controllers
{
    public class BannerController : Controller
    {
        // GET: Banner
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetBanner(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            var model = Bll.SystemSettings.BannerSetting.Where(s => s.Platform == platform)
                .Select(s =>
                {
                    var item = new Models.ActionCell.BaseCell()
                    {
                        ID = s.ID.ToString(),
                        Image = Comm.ResizeImage(s.Image),
                        Title = s.Title,
                    };
                    item.UrlToAction(s.Link);
                    return item;
                });

            return Json(Comm.ToJsonResult("Success", "成功", new { Data = model }),JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetClassify(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            var model = Bll.SystemSettings.ClassifySetting.Where(s => s.Platform == platform)
                .Select(s =>
                {
                    var item = new Models.ActionCell.BaseCell()
                    {
                        ID = s.ID.ToString(),
                        Image = Comm.ResizeImage(s.Image),
                        Title = s.Title,
                    };
                    item.UrlToAction(s.Link);
                    return item;
                });
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = model }), JsonRequestBehavior.AllowGet);
        }
    }
}
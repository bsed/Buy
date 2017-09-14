using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class LocalCouponController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IQueryable<LocalCoupon> QueryShops(List<int> typeIds = null)
        {
            var query = db.LocalCoupons.AsQueryable();
            if (typeIds != null && typeIds.Count > 0)
            {
                query = query.Where(s => typeIds.Contains(s.ShopID));
            }
            return query;
        }

        // GET: LocalCoupon
        public ActionResult Index(string typeId = null, int page = 1)
        {
            var paged = QueryShops(typeId?.SplitToIntArray())
                .OrderByDescending(s => s.CreateDateTime)
                .ToPagedList(page);
            return View(paged);
        }

        // GET: LocalCoupon/Details/5
        public ActionResult Details(int id)
        {
            var ticket = db.LocalCoupons.FirstOrDefault(s => s.ID == id);
            return View(ticket);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetShop()
        {
            var shops = db.Shops.OrderBy(s => s.Sort).ToList();
            return Json(Comm.ToJsonResult("Success", "成功", new
            {
                Data = shops.Select(s => new
                {
                    Image = Url.ResizeImage(s.Logo),
                    s.Code,
                    s.Name,
                    s.ID
                })
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetAll(string typeId = null, int page = 1)
        {
            var paged = QueryShops(typeId?.SplitToIntArray())
                .OrderByDescending(s => s.CreateDateTime)
                .ToPagedList(page);
            var model = paged.Select(s => new Models.ActionCell.LocalCouponCell(s)).ToList();
            return Json(Comm.ToJsonResultForPagedList(paged, model)
                , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Get(int id)
        {
            var lc = db.LocalCoupons.FirstOrDefault(s => s.ID == id);
            if (lc == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "本地券不存在"), JsonRequestBehavior.AllowGet);
            }
            var data = new
            {
                lc.ID,
                lc.Name,
                lc.Price,
                lc.Remark,
                Image = Url.ContentFull(lc.Image),
                ShopName = lc.Shop.Name,
                CreateDateTime = lc.CreateDateTime.ToString("yyyy-MM-dd"),
                EndDateTime = lc.EndDateTime.ToString("yyyy-MM-dd"),
                lc.ShopID,
                ShopLogo = Url.ContentFull(lc.Shop.Logo),
            };
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = data }), JsonRequestBehavior.AllowGet);
        }
    }
}
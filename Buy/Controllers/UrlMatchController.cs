using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.Data.Entity;

namespace Buy.Controllers
{
    public class UrlMatchController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();

        // GET: UrlMatch
        [HttpGet]
        public ActionResult Index(string keyWord, Enums.UrlMatchType type, int page = 1, int pageSize = 15)
        {
            List<UrlMatch> model = new List<UrlMatch>();
            switch (type)
            {
                case Enums.UrlMatchType.Taobao:
                    {
                        var couponTypes = db.CouponTypes.Where(s => s.Platform == Enums.CouponPlatform.TaoBao)
                            .AsQueryable();
                        if (!string.IsNullOrWhiteSpace(keyWord))
                        {
                            couponTypes = couponTypes.Where(s => s.Name.Contains(keyWord));
                        }
                        model = couponTypes.OrderBy(s => s.ID).ToPagedList(page, pageSize)
                             .Select(s => new UrlMatch()
                             {
                                 Title = s.Name,
                                 URL = $"~/Coupon/Second?typeID={s.ID}&platform=Taobao",
                             }).ToList();
                    }
                    break;
                case Enums.UrlMatchType.MoGuJie:
                    {
                        var couponTypes = db.CouponTypes.Where(s => s.Platform == Enums.CouponPlatform.MoGuJie)
                            .AsQueryable();
                        if (!string.IsNullOrWhiteSpace(keyWord))
                        {
                            couponTypes = couponTypes.Where(s => s.Name.Contains(keyWord));
                        }
                        model = couponTypes.OrderBy(s => s.ID).ToPagedList(page, pageSize)
                             .Select(s => new UrlMatch()
                             {
                                 Title = s.Name,
                                 URL = $"~/Coupon/Second?typeID={s.ID}&platform=MoGuJie",
                             }).ToList();
                    }
                    break;
                case Enums.UrlMatchType.LocationCoupon:
                    {
                        var shops = db.Shops.AsQueryable();
                        if (!string.IsNullOrWhiteSpace(keyWord))
                        {
                            shops = shops.Where(s => s.Name.Contains(keyWord));
                        }
                        model = shops.OrderBy(s => s.ID).ToPagedList(page, pageSize)
                             .Select(s => new UrlMatch()
                             {
                                 Title = s.Name,
                                 URL = $"~/LocalCoupon/Index?shopId={s.ID}",
                             }).ToList();
                    }
                    break;
                default:
                    break;
            }
            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using Microsoft.AspNet.Identity;

namespace Buy.Controllers
{
    public class FavoriteController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {

            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: Favorite
        [Authorize]
        public ActionResult Coupon(string platforms, int page = 1)
        {
            var list = GetCoupon(UserID, platforms);
            var paged = list.OrderByDescending(s => s.CreateDateTime).ToPagedList(page);
            return View(paged);
        }

        [Authorize]
        public ActionResult LocalCoupon(int page = 1)
        {
            var list = GetLocalCoupon(UserID);
            var paged = list.OrderByDescending(s => s.Favorite.CreateDateTime).ToPagedList(page);
            return View(paged);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        //收藏
        public ActionResult GetCouponFavorite(string userId, string platforms, int page = 1)
        {
            var list = GetCoupon(userId, platforms);
            var paged = list.OrderByDescending(s => s.CreateDateTime).ToPagedList(page);
            var model = paged.Select(s => new Models.ActionCell.CouponCell(s));
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        //卡包
        public ActionResult GetLocalCouponFavorite(string userId, int page = 1)
        {
            var list = GetLocalCoupon(userId);
            var paged = list.OrderByDescending(s => s.Favorite.CreateDateTime).ToPagedList(page);
            var model = paged.Select(s => new Models.ActionCell.LocalCouponCell(s.LocalCoupon));
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet);
        }

        public IQueryable<CouponUserViewModel> GetCoupon(string userId, string platforms)
        {
            var ps = platforms.SplitToArray<Enums.CouponPlatform>();
            if (ps == null || ps.Count <= 0)
            {
                ps = new List<Enums.CouponPlatform>();
                ps.Add(Enums.CouponPlatform.TaoBao);
                ps.Add(Enums.CouponPlatform.TMall);
            }
            else
            {
                if (ps.Any(s => s == Enums.CouponPlatform.TaoBao || s == Enums.CouponPlatform.TMall))
                {
                    ps = new List<Enums.CouponPlatform>();
                    ps.Add(Enums.CouponPlatform.TaoBao);
                    ps.Add(Enums.CouponPlatform.TMall);
                }
            }

            string couponUserID = Bll.Accounts.GetCouponUserID(userId);
            IQueryable<CouponUserViewModel> list;

            list = from f in db.Favorites
                   from c in db.Coupons
                   from cu in db.CouponUsers
                   where f.Type == Enums.FavoriteType.Coupon
                    && f.CouponID == c.ID
                    && f.UserID == userId
                    && cu.CouponID == c.ID
                    && cu.UserID == couponUserID
                    && ps.Contains(c.Platform)
                   select new CouponUserViewModel()
                   {
                       Commission = c.Commission,
                       CommissionRate = c.CommissionRate,
                       CreateDateTime = c.CreateDateTime,
                       DataJson = c.DataJson,
                       EndDateTime = c.EndDateTime,
                       Image = c.Image,
                       Left = c.Left,
                       ID = c.ID,
                       Name = c.Name,
                       OriginalPrice = c.OriginalPrice,
                       PCouponID = c.PCouponID,
                       Platform = c.Platform,
                       PLink = c.PLink,
                       Price = c.Price,
                       ProductID = c.ProductID,
                       ProductType = c.ProductType,
                       Sales = c.Sales,
                       ShopName = c.ShopName,
                       StartDateTime = c.StartDateTime,
                       Subtitle = c.Subtitle,
                       Total = c.Total,
                       TypeID = c.TypeID,
                       UrlLisr = c.UrlLisr,
                       Value = c.Value,
                       FavoriteID = f.ID,
                       IsFavorite = true,
                   };

            return list;
        }


        public IQueryable<FavoriteLocalCouponList> GetLocalCoupon(string userId)
        {
            var localCoupons = from f in db.Favorites
                               from localCoupon in db.LocalCoupons
                               where f.Type == Enums.FavoriteType.LocalCoupon && f.UserID == userId && f.CouponID == localCoupon.ID
                               select new FavoriteLocalCouponList
                               {
                                   Favorite = f,
                                   LocalCoupon = new LocalCouponList()
                                   {
                                       ID = localCoupon.ID,
                                       ShopID = localCoupon.ShopID,
                                       Remark = localCoupon.Remark,
                                       Commission = localCoupon.Commission,
                                       CreateDateTime = localCoupon.CreateDateTime,
                                       EndDateTime = localCoupon.EndDateTime,
                                       Image = localCoupon.Image,
                                       IsFavorite = true,
                                       Name = localCoupon.Name,
                                       Price = localCoupon.Price,
                                       Shop = localCoupon.Shop,
                                       FavoriteID = f.ID
                                   }
                               };
            return localCoupons;
        }

        // POST: Favorite/Create
        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Create(Favorite model)
        {
            model.UserID = UserID == null ? model.UserID : UserID;
            if (model.UserID == null)
            {
                return Json(Comm.ToJsonResult("Error", "没有登录"));
            }
            var favorites = db.Favorites.Where(s => s.UserID == model.UserID &&
                s.CouponID == model.CouponID && s.Type == model.Type);
            if (favorites.Count() > 0)
            {
                return Json(Comm.ToJsonResult("Error", "已经收藏了"));
            }
            model.CreateDateTime = DateTime.Now;
            db.Favorites.Add(model);
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功", model.ID));
        }

        // POST: Favorite/Delete/5
        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Delete(int id)
        {
            var favorite = db.Favorites.FirstOrDefault(s => s.ID == id);
            if (favorite == null)
            {
                return Json(Comm.ToJsonResult("Error", "没有收藏券"));
            }
            db.Favorites.Remove(favorite);
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功"));
        }
    }
}

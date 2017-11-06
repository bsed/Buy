using Buy.Models;
using Microsoft.AspNet.Identity;
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

        private string UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        private IQueryable<LocalCouponList> QueryShops(string userId, List<int> typeIds = null)
        {
            var query = (from l in db.LocalCoupons
                         join f in db.Favorites
                             .Where(s => s.Type == Enums.FavoriteType.LocalCoupon
                                 && s.UserID == userId)
                          on l.ID equals f.CouponID
                          into lf
                         from s in db.Shops
                         where l.CreateDateTime < DateTime.Now && l.EndDateTime > DateTime.Now && l.ShopID == s.ID
                         select new { LocalCoupon = l, Favorite = lf, Shop = s });
            if (typeIds != null && typeIds.Count > 0)
            {
                query = query.Where(s => typeIds.Contains(s.LocalCoupon.ShopID));
            }
            var model = query.Select(s => new LocalCouponList
            {
                ID = s.LocalCoupon.ID,
                ShopID = s.LocalCoupon.ShopID,
                Remark = s.LocalCoupon.Remark,
                Commission = s.LocalCoupon.Commission,
                CreateDateTime = s.LocalCoupon.CreateDateTime,
                EndDateTime = s.LocalCoupon.EndDateTime,
                Image = s.LocalCoupon.Image,
                IsFavorite = s.Favorite.Any(),
                FavoriteID = s.Favorite.Any() ? s.Favorite.FirstOrDefault().ID : 0,
                Name = s.LocalCoupon.Name,
                Shop = s.Shop,
                Price = s.LocalCoupon.Price,
                Link = s.LocalCoupon.Link,
                Type = s.LocalCoupon.Type,
            });
            return model;
        }

        // GET: LocalCoupon
        public ActionResult Index(int shopId)
        {
            var shops = db.Shops.FirstOrDefault(s => s.ID == shopId);
            return View(shops);
        }

        public ActionResult GetList(string shopId = null, int page = 1)
        {
            var paged = QueryShops(UserID, shopId?.SplitToIntArray())
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
        public ActionResult GetShop(string userId)
        {
            var shops = db.Shops.AsQueryable();
            userId = UserID == null ? userId : UserID;
            var users = db.Users.Where(s => s.UserName == "15999737564" || s.Id == userId);
            var testuser = users.FirstOrDefault(s => s.UserName == "15999737564").Id;
            var user = users.FirstOrDefault(s => s.Id == userId);
            if (user != null && user.UserType == Enums.UserType.Normal && user.ParentUserID != testuser)
            {
                user = db.Users.FirstOrDefault(s => s.Id == user.ParentUserID);
            }
            if (user != null && (user.Id == testuser || user.ParentUserID == testuser))
            { }
            else
            {
                shops = shops.Where(s => s.Code != "xianggu");
            }
            return Json(Comm.ToJsonResult("Success", "成功", new
            {
                Data = shops.OrderBy(s => s.Sort).ToList().Select(s => new
                {
                    Logo = Url.ResizeImage(s.Logo),
                    s.Code,
                    s.Name,
                    s.ID,
                })
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetAll(string userId, string shopId = null, int page = 1)
        {
            var paged = QueryShops(userId, shopId?.SplitToIntArray())
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
                lc.Type,
                lc.Link
            };
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = data }), JsonRequestBehavior.AllowGet);
        }
    }
}
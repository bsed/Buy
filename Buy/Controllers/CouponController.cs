using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OpenQA.Selenium.PhantomJS;
using CsQuery;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
namespace Buy.Controllers
{
    public class CouponController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {

            get
            {
                return User.Identity.GetUserId();
            }
        }

        private IQueryable<CouponUserViewModel> QueryCoupon(CouponSearchModel model)
        {
            string couponUserID = Bll.Accounts.GetCouponUserID(model.UserId);
            IQueryable<CouponQuery> query;
            if (!string.IsNullOrWhiteSpace(couponUserID))
            {
                query = from u in db.CouponUsers
                        from s in db.Coupons
                        where u.CouponID == s.ID && u.UserID == couponUserID
                        select new CouponQuery
                        {
                            CreateDateTime = s.CreateDateTime,
                            DataJson = s.DataJson,
                            Discount = s.OriginalPrice - s.Price,
                            DiscountRate = (s.OriginalPrice - s.Price) / s.OriginalPrice,
                            EndDateTime = s.EndDateTime,
                            Commission = s.Commission,
                            CommissionRate = s.Commission,
                            ID = s.ID,
                            Image = s.Image,
                            Link = u.Link,
                            Name = s.Name,
                            OriginalPrice = s.OriginalPrice,
                            Platform = s.Platform,
                            Price = s.Price,
                            ProductID = s.ProductID,
                            ProductType = s.ProductType,
                            Sales = s.Sales,
                            ShopName = s.ShopName,
                            StartDateTime = s.StartDateTime,
                            Subtitle = s.Subtitle,
                            Type = s.Type,
                            TypeID = s.TypeID,
                            Value = s.Value,
                            UserID = u.UserID,
                        };
            }
            else
            {
                query = db.Coupons.Select(s =>
                         new CouponQuery
                         {
                             CreateDateTime = s.CreateDateTime,
                             DataJson = s.DataJson,
                             Discount = s.OriginalPrice - s.Price,
                             DiscountRate = (s.OriginalPrice - s.Price) / s.OriginalPrice,
                             EndDateTime = s.EndDateTime,
                             Commission = s.Commission,
                             CommissionRate = s.Commission,
                             ID = s.ID,
                             Image = s.Image,
                             Link = null,
                             Name = s.Name,
                             OriginalPrice = s.OriginalPrice,
                             Platform = s.Platform,
                             Price = s.Price,
                             ProductID = s.ProductID,
                             ProductType = s.ProductType,
                             Sales = s.Sales,
                             ShopName = s.ShopName,
                             StartDateTime = s.StartDateTime,
                             Subtitle = s.Subtitle,
                             Type = s.Type,
                             TypeID = s.TypeID,
                             Value = s.Value,
                             UserID = null,
                         });
            }

            //不显示创建时间是未来的和过期的
            if (model.OrderByTime)
            {
                if (!model.IsUpdate)
                {
                    query = query.Where(s => s.CreateDateTime < model.LoadTime && s.EndDateTime > model.LoadTime);
                }
                else
                {
                    query = query.Where(s => s.CreateDateTime < model.UpdateTime && s.CreateDateTime >= model.LoadTime && s.EndDateTime > model.LoadTime);
                }
            }
            if (model.Type != null && model.Type.Count > 0 && model.Type.Contains(0))
            {
                model.Type.Remove(0);
            }
            if (model.Type != null && model.Type.Count > 0)
            {
                query = query.Where(s => s.TypeID.HasValue && (model.Type.Contains(s.Type.ID)
                || model.Type.Contains(s.Type.ParentID)));
            }
            if (model.Platform != null && model.Platform.Count > 0)
            {
                if (model.Platform.Contains(Enums.CouponPlatform.TaoBao)
                    || model.Platform.Contains(Enums.CouponPlatform.TMall))
                {
                    model.Platform.Add(Enums.CouponPlatform.TaoBao);
                    model.Platform.Add(Enums.CouponPlatform.TMall);
                }
                query = query.Where(s => model.Platform.Contains(s.Platform));
            }
            if (!string.IsNullOrWhiteSpace(model.Filter))
            {
                var filterList = model.Filter.SplitToArray<string>(' ');
                foreach (var item in filterList)
                {
                    query = query.Where(s => s.Name.Contains(item) || s.ProductType.Contains(item) || s.ShopName.Contains(item));
                }
            }
            if (model.MinPrice > 0)
            {
                query = query.Where(s => s.Price >= model.MinPrice);
            }
            if (model.MaxPrice > 0)
            {
                query = query.Where(s => s.Price <= model.MaxPrice);
            }
            switch (model.Sort)
            {
                case Enums.CouponSort.Sales:
                    {
                        query = query.OrderByDescending(s => s.Sales);
                    }
                    break;
                case Enums.CouponSort.CreateTime:
                    {
                        query = query.OrderByDescending(s => s.CreateDateTime);
                    }
                    break;
                case Enums.CouponSort.CouponValue:
                    {
                        query = query.OrderByDescending(s => s.Discount);
                    }
                    break;
                case Enums.CouponSort.CouponPrice:
                    {
                        query = query.OrderBy(s => s.Price);
                    }
                    break;
                case Enums.CouponSort.Default:
                default:
                    {
                        query = query.OrderByDescending(s => s.Commission);
                    }
                    break;
            }
            return query;
        }

        // GET: Coupon
        [AllowCrossSiteJson]
        public ActionResult GetAll(string filter,
            DateTime? loadTime, DateTime? updateTime, bool isUpdate = false,
            int page = 1, string types = null,
            string platforms = null,
            bool orderByTime = false, Enums.CouponSort sort = Enums.CouponSort.Default,
            decimal minPrice = 0, decimal maxPrice = 0, string userId = null)
        {
            loadTime = loadTime.HasValue ? loadTime : DateTime.Now;
            var model = new CouponSearchModel()
            {
                Platform = platforms.SplitToArray<Enums.CouponPlatform>(),
                UserId = userId,
                Type = types.SplitToArray<int>(),
                Filter = filter,
                LoadTime = loadTime.Value,
                UpdateTime = updateTime,
                OrderByTime = orderByTime,
                IsUpdate = isUpdate,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Sort = sort,
            };
            if (isUpdate)
            {
                var models = QueryCoupon(model).Select(s => new Models.ActionCell.CouponCell(s)).ToList();
                return Json(Comm.ToJsonResult("Success", "成功", models), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var paged = QueryCoupon(model).ToPagedList(page, 20);
                var models = paged.Distinct(new CouponUserViewModelComparer())
                    .Select(s => new Models.ActionCell.CouponCell(s)).ToList();
                return Json(Comm.ToJsonResultForPagedList(paged, models), JsonRequestBehavior.AllowGet);
            }
        }

        [AllowCrossSiteJson]
        public ActionResult Get(int id, string userID)
        {
            string couponUserID = Bll.Accounts.GetCouponUserID(userID);
            CouponQuery tpt;
            if (couponUserID != null)
            {
                tpt = (from s in db.Coupons
                       from u in db.CouponUsers
                       where s.ID == id && s.ID == u.CouponID && u.UserID == couponUserID
                       select new CouponQuery
                       {
                           CreateDateTime = s.CreateDateTime,
                           DataJson = s.DataJson,
                           Discount = s.OriginalPrice - s.Price,
                           DiscountRate = (s.OriginalPrice - s.Price) / s.OriginalPrice,
                           EndDateTime = s.EndDateTime,
                           Commission = s.Commission,
                           CommissionRate = s.Commission,
                           ID = s.ID,
                           Image = s.Image,
                           Link = u.Link,
                           Name = s.Name,
                           OriginalPrice = s.OriginalPrice,
                           Platform = s.Platform,
                           Price = s.Price,
                           ProductID = s.ProductID,
                           ProductType = s.ProductType,
                           Sales = s.Sales,
                           ShopName = s.ShopName,
                           StartDateTime = s.StartDateTime,
                           Subtitle = s.Subtitle,
                           Type = s.Type,
                           TypeID = s.TypeID,
                           Value = s.Value,
                       }).FirstOrDefault();
            }
            else
            {
                tpt = (from s in db.Coupons
                       select new CouponQuery
                       {
                           CreateDateTime = s.CreateDateTime,
                           DataJson = s.DataJson,
                           Discount = s.OriginalPrice - s.Price,
                           DiscountRate = (s.OriginalPrice - s.Price) / s.OriginalPrice,
                           EndDateTime = s.EndDateTime,
                           Commission = s.Commission,
                           CommissionRate = s.Commission,
                           ID = s.ID,
                           Image = s.Image,
                           Link = null,
                           Name = s.Name,
                           OriginalPrice = s.OriginalPrice,
                           Platform = s.Platform,
                           Price = s.Price,
                           ProductID = s.ProductID,
                           ProductType = s.ProductType,
                           Sales = s.Sales,
                           ShopName = s.ShopName,
                           StartDateTime = s.StartDateTime,
                           Subtitle = s.Subtitle,
                           Type = s.Type,
                           TypeID = s.TypeID,
                           Value = s.Value,
                       }).FirstOrDefault();
            }

            if (tpt == null)
            {
                return Json(Comm.ToJsonResult("Error", "优惠券不存在"), JsonRequestBehavior.AllowGet);
            }
            string productUrl = null;
            switch (tpt.Platform)
            {
                case Enums.CouponPlatform.TaoBao:
                    productUrl = $"http://h5.m.taobao.com/awp/core/detail.htm?id={tpt.ProductID}";
                    break;
                case Enums.CouponPlatform.TMall:
                    productUrl = $"https://detail.m.tmall.com/item.htm?id={tpt.ProductID}";
                    break;
                case Enums.CouponPlatform.Jd:
                    productUrl = $"https://item.m.jd.com/product/{tpt.ProductID}.html";
                    break;
                //case Enums.CouponPlatform.Vip:
                //    break;
                case Enums.CouponPlatform.MoGuJie:
                    productUrl = $"https://detail.m.tmall.com/item.htm?id={tpt.ProductID}";
                    break;
                default:
                    break;
            }
            var data = new
            {
                tpt.ID,
                StartDateTime = tpt.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                CreateDateTime = tpt.CreateDateTime.ToString("yyyy-MM-dd HH:mm"),
                EndDateTime = tpt.EndDateTime.ToString("yyyy-MM-dd HH:mm"),
                tpt.Image,
                tpt.Link,
                tpt.Name,
                tpt.OriginalPrice,
                tpt.Platform,
                tpt.Price,
                tpt.ProductID,
                tpt.ProductType,
                tpt.ShopName,
                tpt.Subtitle,
                Values = Bll.Coupons.GetValues(tpt),
                tpt.Sales,
                ShareUrl = Url.ContentFull($"~/Coupon/Details?id={tpt.ID}"),
                ProductUrl = productUrl,
            };
            return Json(Comm.ToJsonResult("Success", "成功", new
            {
                Data = data
            }), JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public ActionResult GetDetailImgs(int id)
        {
            List<string> img = new List<string>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var tt = db.Coupons.FirstOrDefault(s => s.ID == id);
                if (tt == null)
                {
                    return Json(Comm.ToJsonResult("Error", "优惠券不存在"), JsonRequestBehavior.AllowGet);
                }
                if (tt.UrlLisr == null)
                {
                    switch (tt.Platform)
                    {
                        case Enums.CouponPlatform.TaoBao:
                            {
                                using (var driver = new PhantomJSDriver())
                                {
                                    driver.Url = $"http://h5.m.taobao.com/awp/core/detail.htm?id={tt.ProductID}";
                                    driver.Navigate();
                                    var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                    try
                                    {
                                        wait.Until(s => s.FindElement(OpenQA.Selenium.By.ClassName("detail-desc")));
                                    }
                                    catch (Exception)
                                    {
                                        driver.Quit();
                                        return Json(Comm.ToJsonResult("Error", "超时"), JsonRequestBehavior.AllowGet);
                                    }
                                    var source = driver.PageSource;
                                    var dom = CQ.CreateDocument(source);
                                    img = dom.Select(".detail-desc img").Select(s => s.Attributes["data-src"]).ToList();
                                    driver.Quit();
                                }
                            }
                            break;
                        case Enums.CouponPlatform.TMall:
                            {
                                var dom = CQ.CreateFromUrl($"https://detail.m.tmall.com/item.htm?id={tt.ProductID}");
                                img = dom.Select(".module-content img").Select(s => s.Attributes["data-ks-lazyload"]).ToList();
                                if (img.Count == 0)
                                {
                                    img = dom.Select(".itemPhotoDetail #s-desc").Select(s => s.Attributes["data-ks-lazyload"]).ToList();
                                }
                            }
                            break;
                        case Enums.CouponPlatform.Jd:
                            {
                                using (var driver = new PhantomJSDriver())
                                {
                                    driver.Url = $"https://item.m.jd.com/product/{tt.ProductID}.html";
                                    driver.Navigate();
                                    driver.FindElement(OpenQA.Selenium.By.Id("detailInfo")).Click();
                                    var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                    try
                                    {
                                        wait.Until(s => s.FindElement(OpenQA.Selenium.By.ClassName("scale-box")));
                                    }
                                    catch (Exception)
                                    {
                                        driver.Quit();
                                        return Json(Comm.ToJsonResult("Error", "超时"), JsonRequestBehavior.AllowGet);
                                    }
                                    var source = driver.PageSource;
                                    var dom = CQ.CreateDocument(source);
                                    img = dom.Select(".scale-box img").Select(s => s.Attributes["src"]).ToList();
                                    driver.Quit();
                                }
                            }
                            break;
                        //case Enums.CouponPlatform.Vip:
                        //    break;
                        case Enums.CouponPlatform.MoGuJie:
                            {
                                var options = new PhantomJSOptions();
                                options.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
                                using (var driver = new PhantomJSDriver(options))
                                {
                                    driver.Url = $"http://h5.mogujie.com/detail-normal/index.html?itemId={tt.ProductID}";
                                    driver.Navigate();
                                    var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                    try
                                    {
                                        wait.Until(s =>
                                            s.FindElement(OpenQA.Selenium.By.ClassName("tabs-content")));
                                    }
                                    catch (Exception)
                                    {
                                        driver.Quit();
                                        return Json(Comm.ToJsonResult("Error", "超时"), JsonRequestBehavior.AllowGet);
                                    }
                                    var source = driver.PageSource;
                                    var dom = CQ.CreateDocument(source);
                                    img = dom.Select(".tabs-content [data-key=0] img").Select(s => s.Attributes["origin-src"]).ToList();
                                    driver.Quit();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (img.Count > 0)
                    {
                        tt.UrlLisr = img.Count > 0 ? string.Join(",", img) : "";
                        db.SaveChanges();
                    }
                }
                else
                {
                    img = string.IsNullOrWhiteSpace(tt.UrlLisr) ? img : tt.UrlLisr.SplitToArray<string>();
                }
            }
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = img }), JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public ActionResult GetPwd(int id, string userID)
        {
            userID = UserID == null ? userID : UserID;
            var couponUserID = Bll.Accounts.GetCouponUserID(userID);
            var tpt = db.CouponUsers.Include(s => s.Coupon).FirstOrDefault(s => s.CouponID == id && s.UserID == couponUserID);
            if (tpt == null)
            {
                return Json(Comm.ToJsonResult("Error", "优惠券不存在"), JsonRequestBehavior.AllowGet);
            }
            string pwd = "";
            if (tpt.Platform == Enums.CouponPlatform.TaoBao || tpt.Platform == Enums.CouponPlatform.TMall)
            {
                pwd = new Taobao().GetWirelessShareTpwd(tpt.Coupon.Image, tpt.Link, tpt.Coupon.Name, 0);
            }
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = pwd }), JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public ActionResult GetCouponTypes(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            var data = CouponTypes(platform);
            data.ForEach(s =>
            {
                s.Childs.Select(x => x.Image = Comm.ResizeImage(x.Image, image: null));
            });
            return Json(Comm.ToJsonResult("Success", "成功", new { Data = data }), JsonRequestBehavior.AllowGet);
        }

        public List<CouponTypeTreeNode> CouponTypes(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao, bool resizeImage = true)
        {
            var data = new List<CouponTypeTreeNode>();
            Action<List<CouponTypeTreeNode>, int> setTree = null;
            setTree = (childs, pid) =>
            {
                childs.AddRange(Bll.SystemSettings.CouponType.Where(s => s.ParentID == pid && s.Platform == platform)
                     .OrderBy(s => s.Sort)
                     .Select(s => new CouponTypeTreeNode
                     {
                         Childs = new List<CouponTypeTreeNode>(),
                         Name = s.Name,
                         ID = s.ID,
                         Image = resizeImage ? Comm.ResizeImage(s.Image, image: null) : s.Image,
                         ParentID = s.ParentID,
                     })
                     .ToList());
                foreach (var item in childs)
                {
                    setTree(item.Childs, item.ID);
                }
            };
            setTree(data, 0);
            return data;
        }

        public ActionResult Index(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao,
            Enums.CouponSort sort = Enums.CouponSort.Default, int? typeID = null)
        {
            if (!Comm.IsMobileDrive)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Banner = Bll.SystemSettings.BannerSetting.Where(s => s.Platform == platform).OrderBy(s => s.Sort).ToList();
            ViewBag.Classify = Bll.SystemSettings.ClassifySetting.Where(s => s.Platform == platform).OrderBy(s => s.Sort).ToList();
            return View();
        }

        public ActionResult Second(string name, string filter, string types = null, decimal maxPrice = 0,
            decimal minPrice = 0, Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao,
            Enums.CouponSort sort = Enums.CouponSort.Default)
        {
            var model = new CouponSearchViewModel()
            {
                Filter = filter,
                Platform = platform,
                Sort = sort,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
            };
            if (!string.IsNullOrWhiteSpace(types))
            {
                var typeID = types.SplitToArray<int>().FirstOrDefault();
                ViewBag.TypeName = Bll.SystemSettings.CouponType.First(s => s.ID == typeID);
            }

            return View(model);
        }

        public ActionResult GetList(string filter, DateTime loadTime, DateTime? updateTime, int page = 1,
            string types = null, string platforms = null, bool isUpdate = false,
            bool orderByTime = false, Enums.CouponSort sort = Enums.CouponSort.Default,
            decimal minPrice = 0, decimal maxPrice = 0)
        {
            var model = new CouponSearchModel()
            {
                Platform = platforms.SplitToArray<Enums.CouponPlatform>(),
                UserId = User.Identity.GetUserId(),
                Type = types.SplitToArray<int>(),
                Filter = filter,
                LoadTime = loadTime,
                UpdateTime = updateTime,
                OrderByTime = orderByTime,
                IsUpdate = isUpdate,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                Sort = sort
            };
            ViewBag.IsUpdate = isUpdate;
            if (isUpdate)
            {
                var list = QueryCoupon(model);
                return View(list);
            }
            else
            {
                var paged = QueryCoupon(model).ToPagedList(page, 20);
                var models = paged.Distinct(new CouponUserViewModelComparer());
                ViewBag.Paged = paged;
                return View(models);
            }
        }

        public ActionResult Details(int? id)
        {
            var coupons = db.CouponUsers.Include(s => s.Coupon)
                .Where(s => s.CouponID == id.Value);
            var coupon = coupons.FirstOrDefault();
            string codeMessage = null, link = null;
            if (string.IsNullOrWhiteSpace(UserID))
            {
                codeMessage = "NotLogin";
            }
            else
            {
                var user = db.Users.FirstOrDefault(s => s.Id == UserID);
                if (user.UserType == Enums.UserType.Normal)
                {
                    var code = db.RegistrationCodes.FirstOrDefault(s => s.UseUser == UserID);
                    if (code == null)
                    {
                        codeMessage = "NotActivation";
                    }
                    else
                    {
                        user.Id = user.ParentUserID;
                        var pUser = db.Users.FirstOrDefault(s => s.Id == user.ParentUserID);
                        //如果父级是一个普通用户就拿父级的上一级
                        if (pUser.UserType == Enums.UserType.Normal)
                        {
                            if (pUser.ParentUserID != coupon.UserID)
                            {
                                codeMessage = "NotOwnUser";
                            }
                            else
                            {
                                user.Id = pUser.ParentUserID;
                            }
                        }
                    }
                }
                link = !string.IsNullOrWhiteSpace(codeMessage) ? null : coupons.FirstOrDefault(s => s.UserID == user.Id).Link;
            }
            ViewBag.codeMessage = codeMessage;
            ViewBag.Link = link;
            return View(coupon.Coupon);
        }

        public ActionResult Search()
        {
            var keywords = db.Keywords.OrderByDescending(s => s.SearchCount).Take(4).ToList();
            return View(keywords);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult AutoComplate(string keyword)
        {
            var titles = db.Keywords.Where(s => s.Word.Contains(keyword))
                .OrderByDescending(s => s.CouponNameCount).Take(10).Select(s => s.Word).ToList();
            return Json(Comm.ToJsonResult("Success", "成功", titles), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchConfirm(string filter, int page = 1, Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao,
            Enums.CouponSort sort = Enums.CouponSort.Default)
        {
            var model = new CouponSearchViewModel()
            {
                Filter = filter,
                Platform = platform,
                Sort = sort,
            };
            return View(model);
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
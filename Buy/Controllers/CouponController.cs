using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OpenQA.Selenium.PhantomJS;
using CsQuery;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace Buy.Controllers
{
    public class CouponController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IQueryable<Coupon> QueryCoupon(string filter = null, List<int> type = null
            , List<Enums.CouponPlatform> platform = null, bool orderByTime = false
            , Enums.CouponSort sort = Enums.CouponSort.Default
            , decimal minPrice = 0, decimal maxPrice = 0, string userId = null)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var code = db.RegistrationCodes.FirstOrDefault(s => s.UseUser == userId);
                userId = code == null ? null : code.OwnUser;
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = db.Coupons.GroupBy(s => s.UserID)
                    .Select(s => new { UserID = s.Key, count = s.Count() })
                    .OrderByDescending(s => s.count).FirstOrDefault().UserID;
            }
            var query = db.Coupons.Where(s => s.UserID == userId).Select(s => new CouponQuery
            {
                CreateDateTime = s.CreateDateTime,
                DataJson = s.DataJson,
                Discount = s.OriginalPrice - s.Price,
                DiscountRate = (s.OriginalPrice - s.Price) / s.OriginalPrice,
                EndDateTime = s.EndDateTime,
                ID = s.ID,
                Image = s.Image,
                Link = s.Link,
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
                UserID = s.UserID
            });
            //不显示创建时间是未来的和过期的
            if (orderByTime)
            {
                query = query.Where(s => s.CreateDateTime < DateTime.Now && s.EndDateTime > DateTime.Now);
            }
            if (type != null && type.Count > 0 && type.Contains(0))
            {
                type.Remove(0);
            }
            var coiu = query.Count();
            if (type != null && type.Count > 0)
            {
                query = query.Where(s => s.TypeID.HasValue && (type.Contains(s.Type.ID) || type.Contains(s.Type.ParentID)));
            }
            var dfd = query.Count();
            if (platform != null && platform.Count > 0)
            {
                if (platform.Contains(Enums.CouponPlatform.TaoBao) || platform.Contains(Enums.CouponPlatform.TMall))
                {
                    platform.Add(Enums.CouponPlatform.TaoBao);
                    platform.Add(Enums.CouponPlatform.TMall);
                }
                query = query.Where(s => platform.Contains(s.Platform));
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filterList = filter.SplitToArray<string>(' ');
                foreach (var item in filterList)
                {
                    query = query.Where(s => s.Name.Contains(item) || s.ProductType.Contains(item) || s.ShopName.Contains(item));
                }
            }
            if (minPrice > 0)
            {
                query = query.Where(s => s.Price >= minPrice);
            }
            if (maxPrice > 0)
            {
                query = query.Where(s => s.Price <= maxPrice);
            }
            switch (sort)
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
                case Enums.CouponSort.Default:
                default:
                    {
                        query = query.OrderByDescending(s => s.ID);
                    }
                    break;
            }
            return query;
        }

        // GET: Coupon
        [AllowCrossSiteJson]
        public ActionResult GetAll(string filter, int page = 1, string types = null, string platforms = null
          , bool orderByTime = false, Enums.CouponSort sort = Enums.CouponSort.Default,
            decimal minPrice = 0, decimal maxPrice = 0, string userId = null)
        {
            var paged = QueryCoupon(filter, types.SplitToArray<int>()
                , platforms.SplitToArray<Enums.CouponPlatform>(), orderByTime, sort, minPrice, maxPrice, userId)
                .ToPagedList(page, 20);
            var models = paged.Select(s => new Models.ActionCell.CouponCell(s)).ToList();
            return Json(Comm.ToJsonResultForPagedList(paged, models), JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public ActionResult Get(int id)
        {
            var tpt = db.Coupons.FirstOrDefault(s => s.ID == id);
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
        public ActionResult GetPwd(int id)
        {
            var tpt = db.Coupons.Find(id);
            if (tpt == null)
            {
                return Json(Comm.ToJsonResult("Error", "优惠券不存在"), JsonRequestBehavior.AllowGet);
            }
            var pwd = new Taobao().GetWirelessShareTpwd(tpt.Image, tpt.Link, tpt.Name, 0);
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

        public List<CouponTypeTreeNode> CouponTypes(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
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
                         Image = Comm.ResizeImage(s.Image,image:null),
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
            var couponTypes = new List<CouponTypeTreeNode>();
            couponTypes.Add(new CouponTypeTreeNode()
            {
                ID = 0,
                Name = "首页",
                ParentID = -1,
                Childs = new List<CouponTypeTreeNode>()
            });
            couponTypes.AddRange(CouponTypes(platform));
            ViewBag.CouponTypes = couponTypes;
            return View();
        }

        public ActionResult Second(int typeID, Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            ViewBag.TypeName = Bll.SystemSettings.CouponType.First(s => s.ID == typeID);
            return View();
        }

        public ActionResult GetList(string filter, int page = 1, string types = null, string platforms = null
          , bool orderByTime = false, Enums.CouponSort sort = Enums.CouponSort.Default,
            decimal minPrice = 0, decimal maxPrice = 0)
        {
            var paged = QueryCoupon(filter, types.SplitToArray<int>()
               , platforms.SplitToArray<Enums.CouponPlatform>(), orderByTime, sort, minPrice, maxPrice, User.Identity.GetUserId())
               .ToPagedList(page, 20);
            return View(paged);
        }

        public ActionResult Details(int? id)
        {
            var coupon = db.Coupons.FirstOrDefault(s => s.ID == id.Value);
            string codeMessage = null;
            var userid = User.Identity.GetUserId();
            if (string.IsNullOrWhiteSpace(userid))
            {
                codeMessage = "NotLogin";
            }
            else
            {
                var code = db.RegistrationCodes.FirstOrDefault(s => s.UseUser == userid);
                if (code == null)
                {
                    codeMessage = "NotActivation";
                }
                else
                {
                    if (code.OwnUser != coupon.UserID)
                    {
                        codeMessage = "NotOwnUser";
                    }
                }
            }
            ViewBag.codeMessage = codeMessage;
            return View(coupon);
        }


        public ActionResult Search()
        {
            return View();
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
    }
}
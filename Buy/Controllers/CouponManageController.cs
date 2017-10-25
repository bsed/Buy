using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OpenQA.Selenium.PhantomJS;
using CsQuery;
using Newtonsoft.Json;

namespace Buy.Controllers
{
    [Authorize]
    public class CouponManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "优惠券管理";
        }

        // GET: CouponManage
        [Authorize(Roles = SysRole.CouponManageRead)]
        public ActionResult Index(string filter, Enums.CouponPlatform? platform, int? typeid, DateTime? createTime, int page = 1)
        {
            Sidebar();
            var query = db.Coupons.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var filterList = filter.SplitToArray<string>(' ');
                foreach (var item in filterList)
                {
                    query = query.Where(s => s.Name.Contains(item) || s.ProductType.Contains(item) || s.ShopName.Contains(item));
                }
            }
            if (platform.HasValue)
            {
                query = query.Where(s => s.Platform == platform);
            }
            if (typeid.HasValue)
            {
                query = query.Where(s => s.Type.ParentID == typeid.Value || s.TypeID == typeid.Value);
            }
            if (createTime.HasValue)
            {
                query = query.Where(s => s.CreateDateTime.Day == createTime.Value.Day && s.CreateDateTime.Month == createTime.Value.Month);
            }
            var paged = query.OrderByDescending(s => s.ID).ToPagedList(page);
            //未分类数
            ViewBag.NoTypeCount = db.Coupons.Count(s => !s.TypeID.HasValue);
            //分类
            ViewBag.Type = Bll.SystemSettings.CouponType.Where(s => s.ParentID == 0).ToList();
            return View(paged);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.CouponManageDelete)]
        public ActionResult Delete(DateTime date, List<Enums.CouponPlatform> types)
        {
            if (!User.IsInRole(SysRole.CouponManageDelete))
            {
                return Json(Comm.ToJsonResult("NoRole", "没有权限"));
            }
            var delOld = db.Coupons
                .Where(s => types.Contains(s.Platform)
                            && s.CreateDateTime < date)
                .ToList();
            db.Coupons.RemoveRange(delOld);
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        [HttpPost]
        [Authorize(Roles = SysRole.CouponManageDelete)]
        public ActionResult DeleteByID(string ids)
        {
            if (!User.IsInRole(SysRole.CouponManageDelete))
            {
                return Json(Comm.ToJsonResult("NoRole", "没有权限"));
            }
            var idList = ids.SplitToIntArray();
            if (ids.Count() <= 0)
            {
                return Json(Comm.ToJsonResult("Error", "没有勾选商品"));
            }
            var tickets = db.Coupons.Where(s => idList.Contains(s.ID)).ToList();
            db.Coupons.RemoveRange(tickets);
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "删除成功"));
        }

        [Authorize(Roles = SysRole.CouponManageRead)]
        public ActionResult NoProductType(int page = 1)
        {
            Sidebar();
            var tptlist = db.Coupons
                .Where(s => !s.TypeID.HasValue)
                .GroupBy(s => new { s.ProductType, s.Platform })
                .Select(s => new CouponNotType
                {
                    Type = s.Key.ProductType,
                    Platform = s.Key.Platform,
                    Count = s.Count(),
                })
                .OrderByDescending(s => s.Count)
                .ToPagedList(page);
            return View(tptlist);
        }

        [HttpPost]
        public ActionResult CheckTypes()
        {
            if (!User.IsInRole(SysRole.CouponManageEdit))
            {
                return Json(Comm.ToJsonResult("NoRole", "没有权限"));
            }
            var groupProductType = db.Coupons
                .Where(s => !s.TypeID.HasValue && s.ProductType != null)
                .GroupBy(s => s.ProductType)
                .Select(s => s.Key)
                .ToList();
            int changeCount = 0;
            foreach (var productType in groupProductType)
            {
                var typeID = Bll.Coupons.CheckType(productType, Enums.CouponPlatform.TaoBao);
                if (typeID == null)
                {
                    continue;
                }
                var coupons = db.Coupons.Where(s => !s.TypeID.HasValue
                    && s.ProductType == productType
                    && (s.Platform == Enums.CouponPlatform.TaoBao
                        || s.Platform == Enums.CouponPlatform.TMall)).ToList();
                foreach (var item in coupons)
                {
                    item.TypeID = typeID;
                }
                changeCount += db.SaveChanges();
            }
            return Json(Comm.ToJsonResult("Success", "成功", new { Count = changeCount }));
        }


        //方法
        [AllowCrossSiteJson]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CleanType()
        {
            int count = db.Coupons.Where(s => (s.Platform == Enums.CouponPlatform.TaoBao ||
            s.Platform == Enums.CouponPlatform.TMall) &&
            s.TypeID.HasValue && s.ProductType != null).Count();
            int totalPage = count / 50 + (count % 50 > 0 ? 1 : 0);
            int changeCount = 0;
            for (int i = 1; i <= totalPage; i++)
            {
                var data = db.Coupons.Where(s => (s.Platform == Enums.CouponPlatform.TaoBao ||
                s.Platform == Enums.CouponPlatform.TMall) &&
                s.TypeID.HasValue && s.ProductType != null)
                    .OrderBy(s => s.ID).ToPagedList(i, 50);
                foreach (var item in data)
                {
                    item.TypeID = null;
                }
                changeCount += db.SaveChanges();
            }
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        [AllowCrossSiteJson]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateTimeAddDay()
        {
            int count = db.Coupons.Where(s => s.CreateDateTime.Day == DateTime.Now.Day).Count();
            int totalPage = count / 50 + (count % 50 > 0 ? 1 : 0);
            int changeCount = 0;
            for (int i = 1; i <= totalPage; i++)
            {
                var data = db.Coupons.Where(s => s.CreateDateTime.Day == DateTime.Now.Day).OrderBy(s => s.ID).ToPagedList(i, 50);
                foreach (var item in data)
                {
                    item.CreateDateTime = item.CreateDateTime.AddDays(-1);
                }
                changeCount += db.SaveChanges();
            }
            return Json(Comm.ToJsonResult("Success", "cg", changeCount));
        }

        [HttpGet]
        [AllowAnonymous]
        [AllowCrossSiteJson]
        public ActionResult DelInvalidCoupon()
        {
            System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch();
            oTime.Start();

            var datetime = DateTime.Now.Date.AddDays(-1);
            //var query = from c in db.Coupons
            //            join uc in db.CouponUsers on c.ID equals uc.CouponID into ucc
            //            from ucg in ucc.DefaultIfEmpty()
            //            where c.CreateDateTime > datetime && c.EndDateTime >= DateTime.Now &&
            //                (c.Platform == Enums.CouponPlatform.TaoBao || c.Platform == Enums.CouponPlatform.TMall)
            //            select new { c.ID, Link = ucg == null ? null : ucg.Link };
            var query = from c in db.Coupons
                        join uc in db.CouponUsers on c.ID equals uc.CouponID into ucc
                        from ucg in ucc.DefaultIfEmpty()
                        where (c.Platform == Enums.CouponPlatform.TaoBao || c.Platform == Enums.CouponPlatform.TMall)
                        select new { c.ID, Link = ucg.Link };
            var temp = query.GroupBy(s => s.ID).Select(s => new { s.Key, Count = s.Count() }).Where(s => s.Count > 1).ToList();
            var count = query.Count();
            int pageSize = 50;
            int totalPage = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            int hasCounpon = 0, noCounpon = 0, changeCount = 0; ;

            throw new Exception();
            //var msg = new List<SmsResult>();
            var driver = new PhantomJSDriver();
            List<int> invalidCouponIds = new List<int>();
            try
            {
                Action<int> addInvalid = id =>
                {
                    noCounpon++;
                    invalidCouponIds.Add(id);
                };
                for (int i = 1; i <= 1; i++)
                {
                    var data = query.OrderBy(s => s.ID).ToPagedList(i, pageSize);
                    foreach (var item in data)
                    {
                        if (string.IsNullOrWhiteSpace(item.Link))
                        {
                            addInvalid(item.ID);
                            continue;
                        }
                        driver.Url = $"{item.Link}";
                        driver.Navigate();
                        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(1));
                        try
                        {

                            wait.Until(s => s.FindElement(OpenQA.Selenium.By.CssSelector($".atom-dialog,.coupons-wrap,.coupons-container-no")));
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        var source = driver.PageSource;
                        var dom = CQ.CreateDocument(source);
                        if (dom.Select(".coupons-price").Select(s => s.ClassName).ToList().Count > 0)
                        {
                            hasCounpon++;
                        }
                        else
                        {
                            noCounpon++;
                            invalidCouponIds.Add(item.ID);
                        }
                        //msg.Add(new SmsResult()
                        //{
                        //    IsSuccess = sdsd.Count > 0 ? true : false,
                        //    Message = item.ID.ToString(),
                        //});
                    }
                }
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
            changeCount = invalidCouponIds.Count();
            oTime.Stop();

            return Json(Comm.ToJsonResult("Success", "成功",
                new
                {
                    //msg,
                    HasCounpon = hasCounpon,
                    NoCounpon = noCounpon,
                    ChangeCount = changeCount,
                    Time = oTime.Elapsed.TotalSeconds,
                }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        //统计券是否有券
        public ActionResult HasCoupon(DateTime? date)
        {
            System.Diagnostics.Stopwatch oTime = new System.Diagnostics.Stopwatch();
            oTime.Start();
            date = date.HasValue ? date : DateTime.Now;
            var query = from c in db.Coupons
                        join uc in db.CouponUsers on c.ID equals uc.CouponID into ucc
                        from ucg in ucc.DefaultIfEmpty()
                        where (c.Platform == Enums.CouponPlatform.TaoBao || c.Platform == Enums.CouponPlatform.TMall)
                            && c.CreateDateTime <= date.Value && c.EndDateTime <= date.Value
                        select new { c.ID, Link = ucg.Link, Days = DbFunctions.DiffDays(c.CreateDateTime, date.Value) };
            var count = query.Count();
            int pageSize = 50;
            int totalPage = count / pageSize + (count % pageSize > 0 ? 1 : 0);
            var driver = new PhantomJSDriver();
            List<VerCode> invalidCouponIds = new List<VerCode>();
            try
            {
                Action<int> addInvalid = id =>
                {
                    invalidCouponIds.Add(new VerCode() { IsSuccess = false, Message = id.ToString() });
                };
                for (int i = 1; i <= totalPage; i++)
                {
                    var data = query.OrderByDescending(s => s.Days).ThenBy(s => s.ID).ToPagedList(i, pageSize);
                    foreach (var item in data)
                    {
                        if (string.IsNullOrWhiteSpace(item.Link))
                        {
                            addInvalid(item.Days.Value);
                            continue;
                        }
                        driver.Url = $"{item.Link}";
                        driver.Navigate();
                        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(1));
                        try
                        {
                            wait.Until(s => s.FindElement(OpenQA.Selenium.By.CssSelector($".atom-dialog,.coupons-wrap,.coupons-container-no")));
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        var source = driver.PageSource;
                        var dom = CQ.CreateDocument(source);
                        if (dom.Select(".coupons-price").Select(s => s.ClassName).ToList().Count > 0)
                        {
                            invalidCouponIds.Add(new VerCode() { IsSuccess = true, Message = item.Days.ToString() });
                        }
                        else
                        {
                            addInvalid(item.Days.Value);
                        }
                    }
                }
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
            var model = invalidCouponIds.GroupBy(s => s.Message).Select(s => new
            {
                Days = s.Key,
                HasCoupon = s.Count(x => x.IsSuccess),
                NoCoupon = s.Count(x => !x.IsSuccess)
            }).OrderBy(s => s.Days);
            oTime.Stop();
            Comm.WriteLog("CouponStatistics", new
            {
                data = JsonConvert.SerializeObject(model),
                Time = oTime.Elapsed.TotalSeconds
            }.ToString(), Enums.DebugLogLevel.Normal);
            return Json(Comm.ToJsonResult("Success", "成功"), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        //券有效期统计
        public ActionResult GetTime(int count = 1000)
        {
            var list = db.Coupons.Take(count).Select(s => new
            {
                days = DbFunctions.DiffDays(s.StartDateTime, s.EndDateTime),
                Platform = s.Platform
            }).GroupBy(s => new { s.Platform, s.days })
            .Select(s => new
            {
                s.Key.Platform,
                s.Key.days,
                Count = s.Count()
            }).OrderBy(s => s.Count);
            return Json(Comm.ToJsonResult("Success", "成功", list), JsonRequestBehavior.AllowGet);
        }

    }
}
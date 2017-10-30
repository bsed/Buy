using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;

namespace Buy.Controllers
{
    public class ProxyController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Search(string filter, string userID)
        {
            var query = QueryUser(userID);
            query = query.Where(s => (s.UserType == Enums.UserType.Proxy
                 || s.UserType == Enums.UserType.ProxySec)
                 && (s.NickName.Contains(filter)
                 || s.PhoneNumber.Contains(filter)));
            var model = query.ToList().Select(s => new
            {
                s.UserName,
                s.PhoneNumber,
                Avatar = Url.ResizeImage(s.Avatar, null),
                s.Id,
                s.NickName,
                s.CanAddChild,
                RegisterDateTime = s.RegisterDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                s.WeChatCode,
                s.ThisMonthCount,
                s.LastMonthCount,
                s.TotalCount,
                s.Remark,
            }).ToList();
            return Json(Comm.ToJsonResult("Success", "成功", model), JsonRequestBehavior.AllowGet);
        }


        public IQueryable<UserQueryModelForProxy> QueryUser(string userID)
        {
            var date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var date2 = date1.AddDays(-1);
            var role = db.Roles.FirstOrDefault(s => s.Name == SysRole.UserTakeChildProxy);
            date2 = new DateTime(date2.Year, date2.Month, 1);
            var query = from u in db.Users
                        join r in db.UserRemarks.Where(s => s.UserID == userID)
                            on u.Id equals r.RemarkUser into ur
                        from urr in ur.DefaultIfEmpty()
                        join m1 in db.RegistrationCodes.Where(s => s.UseTime >= date1)
                            on u.Id equals m1.OwnUser into m1g
                        join m2 in db.RegistrationCodes.Where(s => s.UseTime >= date2 && s.UseTime < date1)
                            on u.Id equals m2.OwnUser into m2g
                        join t in db.RegistrationCodes.Where(s => s.UseTime.HasValue)
                            on u.Id equals t.OwnUser into tg
                        select new UserQueryModelForProxy
                        {
                            UserName = u.UserName,
                            PhoneNumber = u.PhoneNumber,
                            Avatar = u.Avatar,
                            Id = u.Id,
                            NickName = u.NickName,
                            CanAddChild = u.Roles.Any(s => s.RoleId == role.Id),
                            Remark = urr == null ? null : urr.Remark,
                            ThisMonthCount = m1g.Count(),
                            LastMonthCount = m2g.Count(),
                            TotalCount = tg.Count(),
                            UserType = u.UserType,
                            RegisterDateTime = u.RegisterDateTime,
                            WeChatCode = u.WeChatCode,
                            ParentID = u.ParentUserID
                        };

            return query;
        }


        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetChild(string userID, int page = 1, int pageSize = 20, Enums.UserType? type = null)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                return Json(Comm.ToJsonResult("Error", "UserID不能为空"));
            }
            var query = QueryUser(userID).Where(s => s.ParentID == userID);

            if (type.HasValue)
            {
                query = query.Where(s => s.UserType == type);
            }
            var paged = query
                .OrderByDescending(s => s.RegisterDateTime)
                .ToPagedList(page, pageSize);

            var model = paged
                .Select(s => new
                {
                    s.UserName,
                    s.PhoneNumber,
                    Avatar = Url.ResizeImage(s.Avatar, null),
                    s.Id,
                    s.NickName,
                    s.CanAddChild,
                    RegisterDateTime = s.RegisterDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    s.WeChatCode,
                    s.ThisMonthCount,
                    s.LastMonthCount,
                    s.TotalCount,
                    s.Remark,
                })
                .ToList();
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet); ;
        }

        //private void GetProxyUserDetail(List<UserViewModelForProxy> users)
        //{
        //    var date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    var date2 = date1.AddDays(-1);
        //    date2 = new DateTime(date2.Year, date2.Month, 1);
        //    var userIds = users.Select(s => s.Id).ToList();
        //    var codes = db.RegistrationCodes
        //            .Where(s => s.UseTime > date2 && userIds.Contains(s.OwnUser))
        //            .GroupBy(s => new
        //            {
        //                s.OwnUser,
        //                UseTime = s.UseTime.Value.Month
        //            })
        //            .Select(s => new
        //            {
        //                s.Key.OwnUser,
        //                s.Key.UseTime,
        //                Count = s.Count()
        //            }).ToList();

        //    var codesTotal = db.RegistrationCodes
        //        .Where(s => s.UseTime.HasValue
        //            && userIds.Contains(s.OwnUser))
        //        .GroupBy(s => s.OwnUser)
        //        .Select(s => new
        //        {
        //            OwnUser = s.Key,
        //            Count = s.Count()
        //        })
        //        .ToList();
        //    foreach (var item in users)
        //    {
        //        var userCodes = codes.Where(s => s.OwnUser == item.Id).ToList();
        //        item.ThisMonthCount = userCodes.FirstOrDefault(s => s.UseTime == date1.Month)?.Count ?? 0;
        //        item.LastMonthCount = userCodes.FirstOrDefault(s => s.UseTime == date2.Month)?.Count ?? 0;
        //        item.TotalCount = codesTotal.FirstOrDefault(s => s.OwnUser == item.Id)?.Count ?? 0;
        //    }
        //    return;
        //}

        /// <summary>
        /// 查询用户和子代理某采集数据输
        /// </summary>
        /// <param name="userID">一级代理</param>
        /// <param name="platform">平台，多个平台之间用,分隔</param>
        /// <param name="date">日期，默认今天</param>
        /// <returns></returns>
        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetCouponCount(string userID, string platform, DateTime? date = null)
        {
            if (userID == null)
            {
                return Json(Comm.ToJsonResult("Error", "UserID不能为空"));
            }
            var platforms = platform.SplitToArray<Enums.CouponPlatform>();
            DateTime dt = date == null ? DateTime.Now.Date : date.Value.Date;
            var userCoupons = from u in db.Users
                              where (u.Id == userID || u.ParentUserID == userID)
                                    && (u.UserType == Enums.UserType.Proxy || u.UserType == Enums.UserType.ProxySec)
                              join ccu in
                                (from c in db.Coupons
                                 from cu in db.CouponUsers
                                 where c.ID == cu.CouponID
                                      && System.Data.Entity.DbFunctions.TruncateTime(c.CreateDateTime) == dt
                                      && platforms.Contains(c.Platform)
                                 group cu by cu.UserID into ccug
                                 select new { UserID = ccug.Key, Count = ccug.Count() })
                              on u.Id equals ccu.UserID into ucc
                              from lucc in ucc.DefaultIfEmpty()
                              select new { u.Id, u.PhoneNumber, Count = lucc == null ? 0 : lucc.Count };
            return Json(Comm.ToJsonResult("Success", "成功", userCoupons), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult SetRemark(string userID, string remarkUserID, string remark)
        {
            var model = db.UserRemarks.FirstOrDefault(s => s.UserID == userID && s.RemarkUser == remarkUserID);
            if (model == null)
            {
                db.UserRemarks.Add(new UserRemark
                {
                    Remark = remark,
                    RemarkUser = remarkUserID,
                    UserID = userID
                });

            }
            else
            {
                model.Remark = remark;
            }
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功"));
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
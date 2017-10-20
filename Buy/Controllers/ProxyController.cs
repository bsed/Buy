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
        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Search(string filter)
        {
            var query = db.Users
                 .Where(s => (s.UserType == Enums.UserType.Proxy
                     || s.UserType == Enums.UserType.ProxySec)
                     && (s.NickName.Contains(filter)
                     || s.PhoneNumber.Contains(filter)))
                 .OrderBy(s => s.NickName)
                 .Select(s => new UserViewModelForProxy
                 {
                     UserName = s.UserName,
                     PhoneNumber = s.PhoneNumber,
                     Avatar = s.Avatar,
                     Id = s.Id,
                     NickName = s.NickName,
                     CanAddChild = true
                 })
                 .ToList();
            foreach (var item in query)
            {
                item.Avatar = Comm.ResizeImage(item.Avatar, image: null);
            }
            CountRegCode(query);
            return Json(Comm.ToJsonResult("Success", "成功", query), JsonRequestBehavior.AllowGet);
        }

        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetChild(string userID, int page = 1, int pageSize = 20, Enums.UserType? type = null)
        {
            var query = db.Users
                .Where(s => s.ParentUserID == userID);
            if (type.HasValue)
            {
                query = query.Where(s => s.UserType == type);
            }
            var paged = query
                .OrderByDescending(s => s.RegisterDateTime)
                .ToPagedList(page, pageSize);
            var model = paged
                .Select(s => new UserViewModelForProxy
                {
                    UserName = s.UserName,
                    PhoneNumber = s.PhoneNumber,
                    Avatar = s.Avatar,
                    Id = s.Id,
                    NickName = s.NickName,
                    CanAddChild = true
                })
                .ToList();
            CountRegCode(model);
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet); ;
        }

        private void CountRegCode(List<UserViewModelForProxy> users)
        {
            var date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var date2 = date1.AddDays(-1);
            date2 = new DateTime(date2.Year, date2.Month, 1);
            var userIds = users.Select(s => s.Id).ToList();
            var codes = db.RegistrationCodes
                    .Where(s => s.UseTime > date2 && userIds.Contains(s.OwnUser))
                    .GroupBy(s => new
                    {
                        s.OwnUser,
                        UseTime = s.UseTime.Value.Month
                    })
                    .Select(s => new
                    {
                        s.Key.OwnUser,
                        s.Key.UseTime,
                        Count = s.Count()
                    }).ToList();

            var codesTotal = db.RegistrationCodes
                .Where(s => s.UseTime.HasValue && userIds.Contains(s.OwnUser))
                .GroupBy(s => s.OwnUser)
                .Select(s => new { OwnUser = s.Key, Count = s.Count() })
                .ToList();
            foreach (var item in users)
            {
                var userCodes = codes.Where(s => s.OwnUser == item.Id).ToList();
                item.ThisMonthCount = userCodes.FirstOrDefault(s => s.UseTime == date1.Month)?.Count ?? 0;
                item.LastMonthCount = userCodes.FirstOrDefault(s => s.UseTime == date2.Month)?.Count ?? 0;
                item.TotalCount = codesTotal.FirstOrDefault(s => s.OwnUser == item.Id)?.Count ?? 0;
            }
            return;
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
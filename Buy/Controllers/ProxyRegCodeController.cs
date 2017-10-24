using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Buy.Controllers
{
    public class ProxyRegCodeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetLog(string userID, string from, int page = 1, int pageSize = 1)
        {
            var query = (from l in db.RegistrationCodeLogs
                         join u in db.Users.Select(s => new
                         {
                             s.Id,
                             s.Avatar,
                             s.PhoneNumber,
                             s.NickName,
                             s.UserName,
                         }) on l.From equals u.Id into ug
                         from ugg in ug.DefaultIfEmpty()
                         where l.UserID == userID
                         select new
                         {
                             User = ugg,
                             Log = l
                         });
            if (!string.IsNullOrWhiteSpace(from))
            {
                query = query.Where(s => s.Log.From == from);
            }
            var paged = query.OrderByDescending(s => s.Log.CreateDateTime).ToPagedList(page, pageSize);
            var model = paged.Select(s => new RegistrationCodeLogViewModel
            {
                Avatar = Comm.ResizeImage(s.User?.Avatar, image: null),
                Count = s.Log.Count,
                CreateDateTime = s.Log.CreateDateTime,
                From = s.Log.From,
                ID = s.Log.ID,
                NickName = s.User?.NickName ?? "系统",
                PhoneNumber = s.User?.PhoneNumber,
                Remark = string.IsNullOrWhiteSpace(s.Log.Remark) ? (s.Log.Count > 0 ? "给你转码" : "收到你的转码") : s.Log.Remark,
                UserID = s.Log.UserID,
                UserName = s.User?.UserName ?? "系统"
            }).Select(s => new
            {
                s.Avatar,
                s.Count,
                CreateDateTime = s.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                s.From,
                s.ID,
                s.NickName,
                s.UserID,
                s.Remark
            });
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetFirst(string userID)
        {
            var code = db.RegistrationCodes
                .FirstOrDefault(s => s.OwnUser == userID
                    && s.UseEndDateTime == null);

            if (code == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "没有可用的激活码", new { Code = code?.Code, Lave = 0 }), JsonRequestBehavior.AllowGet);
            }
            var count = db.RegistrationCodes
                .Count(s => s.OwnUser == userID
                    && s.UseEndDateTime == null);
            return Json(Comm.ToJsonResult("Success", "成功", new { Code = code.Code, Lave = count }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Give(string userID, string phoneNumber, int count)
        {
            var fUser = db.Users.FirstOrDefault(s => s.Id == userID);
            if (fUser == null)
            {
                return Json(Comm.ToJsonResult("NoFound", $"转出人不存在"));
            }
            var tUser = db.Users.FirstOrDefault(s => s.PhoneNumber == phoneNumber);
            var enableGive = new Enums.UserType[] {
                Enums.UserType.Proxy,
                Enums.UserType.ProxySec
            };
            if (tUser == null)
            {
                return Json(Comm.ToJsonResult("NoFound", $"手机号不存在"));
            }
            else if (!enableGive.Contains(tUser.UserType))
            {
                return Json(Comm.ToJsonResult("Error", $"转送对象没有权限接收"));
            }
            if (tUser.Id == userID)
            {
                return Json(Comm.ToJsonResult("Error", $"不能转给自己"));
            }
            var codes = db.RegistrationCodes
                .Where(s => s.OwnUser == userID
                    && s.UseEndDateTime == null).Take(count)
                    .ToList();
            if (codes.Count < count)
            {
                return Json(Comm.ToJsonResult("NoEnough", $"剩余数量不足以完成该提交", new { Lave = codes.Count }));
            }
            foreach (var item in codes)
            {
                item.OwnUser = phoneNumber;
            }
            db.SaveChanges();
            var tLog = new RegistrationCodeLog { Count = count, CreateDateTime = DateTime.Now, From = userID, UserID = tUser.Id };
            var fLog = new RegistrationCodeLog { Count = -count, CreateDateTime = DateTime.Now, From = tUser.Id, UserID = userID };
            db.RegistrationCodeLogs.Add(tLog);
            db.RegistrationCodeLogs.Add(fLog);
            db.SaveChanges();
            var lave = db.RegistrationCodes.Where(s => s.OwnUser == userID && s.UseTime == null).Count();
            return Json(Comm.ToJsonResult("Success", $"转码成功", new { Lave = lave }));
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
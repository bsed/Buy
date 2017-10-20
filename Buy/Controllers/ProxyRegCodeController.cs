using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
namespace Buy.Controllers
{
    public class ProxyRegCodeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetLog(string userID, int page = 1, int pageSize = 1)
        {
            var paged =
                (from l in db.RegistrationCodeLogs
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
                 orderby l.CreateDateTime descending
                 select new
                 {
                     User = ugg,
                     Log = l
                 })
                .ToPagedList(page, pageSize);
            var model = paged.Select(s => new RegistrationCodeLogViewModel
            {
                Avatar = Comm.ResizeImage(s.User?.Avatar, image: null),
                Count = s.Log.Count,
                CreateDateTime = s.Log.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                From = s.Log.From,
                ID = s.Log.ID,
                NickName = s.User?.NickName ?? "系统",
                PhoneNumber = s.User?.PhoneNumber,
                Remark = s.Log.Remark,
                UserID = s.Log.UserID,
                UserName = s.User?.UserName ?? "系统"
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
            var count = db.RegistrationCodes
                .Count(s => s.OwnUser == userID
                    && s.UseEndDateTime == null);
            if (code == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "没有可用的激活码", new { Code = code.Code, Lave = count }), JsonRequestBehavior.AllowGet);
            }
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
            if (tUser == null)
            {
                return Json(Comm.ToJsonResult("NoFound", $"手机号不存在"));
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
            return Json(Comm.ToJsonResult("Success", $"转码成功", new { Lave = codes.Count }));
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
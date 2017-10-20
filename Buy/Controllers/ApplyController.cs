using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
namespace Buy.Controllers
{
    public class ApplyController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetAll(string userID, int page = 1, int pageSize = 20)
        {
            var query = (from u in db.Users.Select(s => new { s.Id, s.Avatar, s.NickName, s.PhoneNumber, s.UserName })
                         from a in db.ChildProxyApplys
                         where u.Id == a.UserID && a.ProxyID == userID
                         select new { User = u, Apply = a }
                         );
            var paged = query.OrderByDescending(s => s.Apply.CreateDateTime).ToPagedList(page, pageSize);
            var models = paged.Select(s => new ChildProxyApplyViewModel
            {
                Avatar = Comm.ResizeImage(s.User.Avatar, image: null),
                CheckDateTime = s.Apply.CheckDateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                NickName = s.User.NickName,
                PhoneNumber = s.User.PhoneNumber,
                ProxyID = s.Apply.ProxyID,
                ID = s.Apply.ID,
                UserID = s.User.Id,
                UserName = s.User.UserName,
                State = s.Apply.State,
                CreateDateTime = s.Apply.CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Remark = s.Apply.Remark
            });
            return Json(Comm.ToJsonResultForPagedList(paged, models), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Create(string userID, string proxyID, string remark)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == userID);
            if (user == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "用户不存在"));
            }

            var proxy = db.Users.FirstOrDefault(s => s.Id == proxyID);
            if (proxy == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "用户不存在"));
            }
            if (!string.IsNullOrWhiteSpace(user.ParentUserID))
            {
                return Json(Comm.ToJsonResult("Error", "一个用户只能拥有一个上级代理"));
            }
            if (!db.ChildProxyApplys.Any(s => s.ProxyID == proxyID && s.UserID == userID))
            {
                var apply = new ChildProxyApply
                {
                    CreateDateTime = DateTime.Now,
                    ProxyID = proxyID,
                    State = Enums.ChildProxyApplyState.NoCheck,
                    UserID = userID,
                    Remark = remark,
                };
                db.ChildProxyApplys.Add(apply);
                db.SaveChanges();
            }
            return Json(Comm.ToJsonResult("Success", "成功"));
        }



        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Edit(int id, bool check, string userID)
        {
            var apply = db.ChildProxyApplys
                .FirstOrDefault(s => s.ID == id
                    && s.UserID == userID);

            if (apply == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "申请不存在"));
            }
            if (apply.State != Enums.ChildProxyApplyState.NoCheck)
            {
                return Json(Comm.ToJsonResult("HadCheck", $"申请已处理"));
            }
            apply.State = check ? Enums.ChildProxyApplyState.Pass : Enums.ChildProxyApplyState.NoPass;
            apply.CheckDateTime = DateTime.Now;
            if (check)
            {
                var user = db.Users.FirstOrDefault(s => s.Id == apply.UserID);
                user.ParentUserID = apply.ProxyID;

            }
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", $"处理成功"));
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
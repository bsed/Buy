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
        public ActionResult GetChild(string userID, int? page = 1)
        {
            var query = db.Users
                .Where(s => s.ParentUserID == userID)
                .OrderByDescending(s => s.RegisterDateTime);
            var paged = query.ToPagedList(page.Value);
            var model = paged
                .Select(s => new UserViewModel(s))
                .ToList();
            return Json(Comm.ToJsonResultForPagedList(paged, model)); ;
        }



        [HttpPost]
        public ActionResult Apply(string userID, string proxyID)
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
                    UserID = userID
                };
                db.ChildProxyApplys.Add(apply);
                db.SaveChanges();
            }
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        [HttpPost]
        public ActionResult Check(int id, bool check, string userID)
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
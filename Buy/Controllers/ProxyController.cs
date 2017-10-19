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
            return Json(Comm.ToJsonResultForPagedList(paged, model), JsonRequestBehavior.AllowGet); ;
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
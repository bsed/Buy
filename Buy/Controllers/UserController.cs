using Buy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: User
        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            return View(user);
        }

        public ActionResult Edit()
        {
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            return View(new UserSetting(user));
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Edit(UserSetting model)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == model.ID);
            user.NickName = model.NickName;
            user.Avatar = model.Avatar.ImageUrl;
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        public ActionResult CustomerService()
        {
            var model = new SystemSetting()
            {
                Value = GetService(),
            };
            return View(model);
        }

        public string GetService()
        {
            return Bll.SystemSettings.CustomerService;
        }

        [AllowCrossSiteJson]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetCustomerService()
        {
            return Json(Comm.ToJsonResult("Success", "成功", GetService()), JsonRequestBehavior.AllowGet);
        }

    }
}
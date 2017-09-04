using Buy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class UserManageController : Controller
    {
        private string UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "用户管理";
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: UserManage
        public ActionResult Index(int page = 1)
        {
            Sidebar();
            var userlist = (from u in db.Users
                            where u.UserType == Enums.UserType.Proxy
                            join r in db.RegistrationCodes
                            on u.Id equals r.OwnUser
                            into c
                            select new UserManage()
                            {
                                Count = c.Count(),
                                Id = u.Id,
                                RegisterDateTime = u.RegisterDateTime,
                                UnUseCount = c.Count(s => !s.UseTime.HasValue),
                                UseCount = c.Count(s => s.UseTime.HasValue),
                                UserName = u.UserName
                            })
                 .OrderBy(s => s.RegisterDateTime)
                .ToPagedList(page);
            return View(userlist);
        }

        // GET: UserManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserManage/Create
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel model)
        {
            var user = db.Users.FirstOrDefault(s => s.UserName == model.PhoneNumber);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "用户名已存在");
            }
            if (ModelState.IsValid)
            {
                user = new ApplicationUser
                {
                    UserName = model.PhoneNumber,
                    UserType = Enums.UserType.Proxy,
                    RegisterDateTime = DateTime.Now,
                    LastLoginDateTime = DateTime.Now
                };
                var result = UserManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);

        }

        // GET: UserManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserManage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}

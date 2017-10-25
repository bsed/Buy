using Buy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Data.Entity;

namespace Buy.Controllers
{
    public class AdminManageController : Controller
    {
        private Bll.Roles _roles = new Bll.Roles();

        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "管理员管理";
        }

        private string UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
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

        [Authorize(Roles = SysRole.AdminManageRead)]
        public ActionResult Index(int page = 1, string filter = null)
        {
            Sidebar();
            var query = from u in db.Users
                        from gr in db.RoleGroups
                        where u.UserType == Enums.UserType.System
                            && u.RoleGroupID == gr.ID
                        select new AdminManageIndexViewModel()
                        {
                            Id = u.Id,
                            RegisterDateTime = u.RegisterDateTime,
                            UserName = u.UserName,
                            NickName = u.NickName,
                            RoleGroupID = gr.ID,
                            RoleGroupName = gr.Name
                        };
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(s =>
                    s.NickName.Contains(filter)
                    || s.UserName.Contains(filter));
            }
            var paged = query
                 .OrderBy(s => s.RegisterDateTime)
                .ToPagedList(page);
            return View(paged);
        }



        // GET: UserManage/Create
        [Authorize(Roles = SysRole.AdminManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            var roles = db.RoleGroups.ToList();
            var model = new AdminManageCreateViewModel();
            model.SelRole = new SelectList(roles, "ID", "Name");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.AdminManageCreate)]
        public ActionResult Create(AdminManageCreateViewModel model)
        {
            var user = db.Users.FirstOrDefault(s => s.UserName == model.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "用户名已被使用");
            }
            if (ModelState.IsValid)
            {
                user = new ApplicationUser
                {
                    UserName = model.UserName,
                    UserType = Enums.UserType.System,
                    RegisterDateTime = DateTime.Now,
                    LastLoginDateTime = DateTime.Now,
                    NickName = model.NickName,
                    RoleGroupID = model.RoleGroupID
                };
                var result = UserManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                {
                    user = db.Users.FirstOrDefault(s => s.UserName == model.UserName);
                    _roles.EditUserRoleByGroupID(user.Id, user.RoleGroupID.Value);
                    var returnUrl = this.GetReturnUrl();
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        returnUrl = Url.Action("index", new { UserType = Enums.UserType.System });
                    }
                    return Redirect(returnUrl);
                }
            }
            var roles = db.RoleGroups.ToList();
            model.SelRole = new SelectList(roles, "ID", "Name");
            return View(model);
        }

        // GET: UserManage/Edit/5
        [Authorize(Roles = SysRole.AdminManageEdit)]
        public ActionResult Edit(string id)
        {
            Sidebar();
            var user = db.Users.FirstOrDefault(s => s.Id == id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var roles = db.RoleGroups.ToList();
            var model = new AdminManageCreateViewModel
            {
                Id = user.Id,
                NickName = user.NickName,
                RoleGroupID = user.RoleGroupID.Value,
                SelRole = new SelectList(roles, "ID", "Name"),
                UserName = user.UserName
            };
            return View(model);
        }

        // POST: UserManage/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.AdminManageEdit)]
        public ActionResult Edit(AdminManageCreateViewModel model)
        {
            var users = db.Users.Where(s => s.Id == model.Id || s.UserName == model.UserName);
            var returnUrl = this.GetReturnUrl();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index");
            }
            if (users.Any(s => s.UserName == model.UserName && s.Id != model.Id))
            {
                ModelState.AddModelError("UserName", "用户名有重复的");

            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                UserManager.RemovePassword(model.Id);
                var result = UserManager.AddPassword(model.Id, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Password", item);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var user = users.FirstOrDefault(s => s.Id == model.Id);
                if (user == null)
                {
                    return this.ToError("错误", "用户不存在", returnUrl);
                }
                user.UserName = model.UserName;
                user.NickName = model.NickName;
                if (user.RoleGroupID != model.RoleGroupID)
                {
                    user.RoleGroupID = model.RoleGroupID;
                    _roles.EditUserRoleByGroupID(user.Id, model.RoleGroupID);
                }
                db.SaveChanges();
                return Redirect(returnUrl);
            }
            var roles = db.RoleGroups.ToList();
            model.SelRole = new SelectList(roles, "ID", "Name");
            return View(model);
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
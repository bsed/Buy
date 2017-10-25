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
    [Authorize]
    public class UserManageController : Controller
    {
        private Bll.Roles _roles = new Bll.Roles();

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

        [Authorize(Roles = SysRole.UserManageRead)]
        public ActionResult Index(int page = 1, Enums.UserType type = Enums.UserType.Proxy, string filter = null)
        {
            Sidebar();
            var query = from u in db.Users
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
                            UserName = u.UserName,
                            NickName = u.NickName,
                            UserType = u.UserType,
                            PhoneNumber = u.PhoneNumber,
                            WeChatCode = u.WeChatCode
                        };
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(s => s.PhoneNumber == filter
                    || s.NickName.Contains(filter)
                    || s.UserName.Contains(filter)
                    || s.WeChatCode == filter);
            }
            else
            {
                query = query.Where(s => s.UserType == type);
            }
            var userlist = query
                 .OrderBy(s => s.RegisterDateTime)
                .ToPagedList(page);
            return View(userlist);
        }


        [Authorize(Roles = SysRole.UserManageCreate)]
        public ActionResult Create(Enums.UserType userType = Enums.UserType.Proxy, string pid = null)
        {
            Sidebar();
            Models.ApplicationUser user = null;
            if (!string.IsNullOrWhiteSpace(pid))
            {
                user = db.Users.FirstOrDefault(s => s.Id == pid);
            }
            var model = new UserManageCreateUserViewModel()
            {
                UserType = Enums.UserType.Proxy,
                ParentUserID = user?.Id,
                ParentUserNickName = user?.NickName
            };
            return View(model);
        }


        [HttpPost]
        public ActionResult Create(UserManageCreateUserViewModel model, string returnUrl)
        {
            if (!User.IsInRole(SysRole.UserManageCreate))
            {
                return Json(Comm.ToJsonResult("NoRole", "没有权限"));
            }
            var user = db.Users.FirstOrDefault(s => s.UserName == model.PhoneNumber);

            if (user != null)
            {
                return Json(Comm.ToJsonResult("Error", "手机号已被使用"));
                //ModelState.AddModelError("PhoneNumber", "手机号已被使用");
            }
            if (!string.IsNullOrWhiteSpace(model.ParentUserID))
            {
                var pUser = db.Users.FirstOrDefault(s => s.Id == model.ParentUserID);
                if (pUser == null)
                {
                    return Json(Comm.ToJsonResult("Error", "不存在父用户"));
                    //ModelState.AddModelError("ParentUserID", "不存在父用户");
                }
                if (pUser.UserType != Enums.UserType.Proxy)
                {
                    return Json(Comm.ToJsonResult("Error", $"用户“{pUser.NickName}”不是代理"));
                    //ModelState.AddModelError("ParentUserID", $"用户“{pUser.NickName}”不是代理");
                }
            }

            user = new ApplicationUser
            {
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                UserType = model.UserType,
                RegisterDateTime = DateTime.Now,
                LastLoginDateTime = DateTime.Now,
                NickName = model.NickName
            };
            if (!string.IsNullOrWhiteSpace(model.ParentUserID))
            {
                user.ParentUserID = model.ParentUserID;
            }
            var result = UserManager.CreateAsync(user, model.Password);
            if (result.Result.Succeeded)
            {
                user = db.Users.FirstOrDefault(s => s.UserName == model.PhoneNumber);
                //var returnUrl = Request["ReturnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = Url.Action("index");
                }
                //return Redirect(returnUrl);
                return Json(Comm.ToJsonResult("Success", "添加成功", new
                {
                    ReturnUrl = returnUrl,
                    Id = user.Id
                }));
            }
            return Json(Comm.ToJsonResult("Error", result.Result.Errors.FirstOrDefault()));
        }

       

        [HttpGet]
        [Authorize(Roles = SysRole.UserManageEdit)]
        public ActionResult Edit(string id)
        {
            Sidebar();
            var user = db.Users.Include(s => s.Roles).FirstOrDefault(s => s.Id == id);
            var model = new UserManageEditProxyViewModel(user);
            var roles = db.Roles.FirstOrDefault(s => s.Name == SysRole.UserTakeChildProxy);
            model.TakeChildProxy = user.Roles.Any(s => s.RoleId == roles.Id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.UserManageEdit)]
        public ActionResult Edit(UserManageEditProxyViewModel model)
        {
            Sidebar();
            var user = db.Users
                .Include(s => s.Roles)
                .FirstOrDefault(s => s.Id == model.Id);
            var roles = db.Roles.FirstOrDefault(s => s.Name == SysRole.UserTakeChildProxy);
            var takeChildProxy = user.Roles.Any(s => s.RoleId == roles.Id);
            if (user == null || user.UserType != Enums.UserType.Proxy)
            {
                return this.ToError("错误", "不存在该代理", Url.Action("Index"));
            }
            if (db.Users.Any(s => s.UserName == model.UserName && s.Id != model.Id))
            {
                ModelState.AddModelError("UserName", "用户名有重复的");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.NickName = model.NickName;
                if (User.IsInRole(SysRole.UserManageEnableTakeChildProxy)
                    && model.TakeChildProxy != takeChildProxy)
                {
                    if (model.TakeChildProxy)
                    {
                        user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole
                        {
                            UserId = user.Id,
                            RoleId = roles.Id,
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        var delRole = user.Roles.FirstOrDefault(s => s.RoleId == roles.Id);
                        user.Roles.Remove(delRole);
                        db.SaveChanges();
                    }
                }
                var returnUrl = Url.Action("Index");
                if (this.GetReturnUrl() != null)
                {
                    returnUrl = this.GetReturnUrl();
                }
                return this.Redirect(returnUrl);
            }
            else
            {

            }


            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = SysRole.UserManageRead)]
        public ActionResult Child(string id, int page = 1)
        {
            Sidebar();
            ViewBag.User = db.Users.FirstOrDefault(s => s.Id == id);
            var userlist = (from u in db.Users
                            where u.ParentUserID == id
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
                                UserName = u.UserName,
                                NickName = u.NickName,
                                UserType = u.UserType
                            })
                .OrderByDescending(s => s.RegisterDateTime)
                .ToPagedList(page);
            return View(userlist);
        }

        /// <summary>
        /// 把用户升级到一级代理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(string id)
        {
            if (!User.IsInRole(SysRole.UserManageUpdate))
            {
                return Json(Comm.ToJsonResult("NoRole", "没有权限"));
            }
            var user = db.Users.FirstOrDefault(s => s.Id == id);
            if (user == null)
            {
                return Json(Comm.ToJsonResult("NoFound", "用户不存在"));
            }
            else if (user.UserType == Enums.UserType.Proxy)
            {
                return Json(Comm.ToJsonResult("Error", "已经是代理"));
            }
            else
            {
                user.ParentUserID = null;
                user.UserType = Enums.UserType.Proxy;
                var codes = db.RegistrationCodes
                    .Where(s => s.OwnUser == user.Id)
                    .Select(s => s.UseTime.HasValue)
                    .GroupBy(s => s)
                    .Select(s => new
                    {
                        CodeType = s.Key ? "已使用" : "未使用",
                        Count = s.Count()
                    })
                    .ToList();

                db.SaveChanges();
                var code1 = codes.FirstOrDefault(s => s.CodeType == "未使用")?.Count ?? 0;
                var code2 = codes.FirstOrDefault(s => s.CodeType == "已使用")?.Count ?? 0;
                return Json(Comm.ToJsonResult("Success", $"升级成功,{code1}个激活码和{code2}个用户", new
                {
                    CodeCount = code1,
                    UserCount = code2
                }));
            }

        }

        [HttpPost]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult AddUsers()
        {
            var users = new List<ApplicationUser>();
            FileInfo fileInfo = new FileInfo("C:/Users/Administrator/Desktop/daochu.xml");
            if (fileInfo.Exists)
            {
                XDocument doc = XDocument.Load("C:/Users/Administrator/Desktop/daochu.xml");
                if (doc != null)
                {
                    IEnumerable<XElement> elementlist = doc.Root.Elements("user");
                    foreach (var item in elementlist)
                    {
                        var time = DateTime.Now;
                        DateTime.TryParse(item.Attribute("RegisterDateTime").Value, out time);
                        ApplicationUser user = new ApplicationUser()
                        {
                            UserName = item.Attribute("UserName").Value,
                            PhoneNumber = item.Attribute("UserName").Value,
                            NickName = item.Attribute("NickName").Value,
                            UserType = Enums.UserType.Proxy,
                            LastLoginDateTime = time,
                            RegisterDateTime = time,
                        };
                        users.Add(user);
                        var result = UserManager.CreateAsync(user, "123456");
                        if (!result.Result.Succeeded)
                        {
                            return Json(Comm.ToJsonResult("Error", "失败"));
                        }
                    }
                    return Json(Comm.ToJsonResult("Success", "成功", new { data = users }));
                }
            }
            return Json(Comm.ToJsonResult("Error", "文件不存在"));
        }

        [HttpPost]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult AddCode(string id, int count)
        {
            var codes = new List<RegistrationCode>();
            FileInfo fileInfo = new FileInfo("C:/Users/Administrator/Desktop/daochuCode.xml");
            if (fileInfo.Exists)
            {
                XDocument doc = XDocument.Load("C:/Users/Administrator/Desktop/daochuCode.xml");
                if (doc != null)
                {
                    var admin = db.Users.FirstOrDefault(s => s.UserType == Enums.UserType.System);
                    IEnumerable<XElement> elementlist = doc.Root.Elements("code");
                    foreach (var item in elementlist)
                    {
                        var time = DateTime.Now;
                        //if (!string.IsNullOrWhiteSpace(item.Attribute("CreateTime").Value) || item.Attribute("CreateTime") != null)
                        //{
                        //    DateTime.TryParse(item.Attribute("CreateTime").Value, out time);
                        //}
                        RegistrationCode code = new RegistrationCode()
                        {
                            Code = item.Attribute("Code").Value,
                            CreateTime = time,
                            CreateUser = admin.Id,
                            OwnUser = id,
                        };
                        codes.Add(code);
                    }
                    if (codes.Count == count)
                    {
                        db.RegistrationCodes.AddRange(codes);
                        db.SaveChanges();
                        return Json(Comm.ToJsonResult("Success", "成功"));
                    }
                    else
                    {
                        return Json(Comm.ToJsonResult("Error", "数量不对"));
                    }
                }
            }
            return Json(Comm.ToJsonResult("Error", "文件不存在"));
        }

        [HttpPost]
        [AllowCrossSiteJson]
        [AllowAnonymous]
        public ActionResult DelCode(string id)
        {
            var codes = db.RegistrationCodes.Where(s => s.OwnUser == id);
            db.RegistrationCodes.RemoveRange(codes);
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

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;

namespace Buy.Controllers
{
    [Authorize]
    public class RegistrationCodeController : Controller
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
            ViewBag.Sidebar = "注册码管理";
        }

        // GET: RegistrationCode
        [Authorize(Roles = SysRole.RegistrationCodeManageRead)]
        public ActionResult Index(string userId, int page = 1)
        {
            Sidebar();
            var userlist = db.Users.Where(s => s.UserType == Enums.UserType.Proxy).ToList();
            ViewBag.UserList = userlist;
            var registrationCodes = db.RegistrationCodes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                registrationCodes = registrationCodes.Where(s => s.OwnUser == userId);
            }
            var list = registrationCodes
                .OrderByDescending(s => s.ID)
                .ToPagedList(page);

            var userids = list.Select(s => s.OwnUser).ToList();
            userids.AddRange(list.Select(s => s.CreateUser).ToList());
            userids.AddRange(list.Select(s => s.UseUser).ToList());
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();

            var model = list.Select(s =>
            {
                var create = users.FirstOrDefault(u => u.Id == s.CreateUser);
                var own = users.FirstOrDefault(u => u.Id == s.OwnUser);
                var use = users.FirstOrDefault(u => u.Id == s.UseUser);
                var item = new RegistrationCodeViewModel()
                {
                    Code = s.Code,
                    Create = create,
                    CreateTime = s.CreateTime,
                    CreateUser = s.CreateUser,
                    ID = s.ID,
                    Own = own,
                    OwnUser = s.OwnUser,
                    Use = use,
                    UseTime = s.UseTime,
                    UseUser = s.UseUser,
                    ActiveEndDateTime = s.ActiveEndDateTime,
                    UseEndDateTime = s.UseEndDateTime
                };
                return item;
            });
            ViewBag.Paged = list;
            return View(model);
        }

        [Authorize(Roles = SysRole.RegistrationCodeManageCreate)]
        public ActionResult Create(string userId)
        {
            Sidebar();
            var model = new RegistrationCodeCreate()
            {
                OwnUser = userId,
            };
            if (string.IsNullOrWhiteSpace(userId))
            {
                var userlist = db.Users.Where(s => s.UserType == Enums.UserType.Proxy)
                    .Select(s => new SelectListItem()
                    {
                        Text = s.UserName,
                        Value = s.Id
                    }).ToList();
                ViewBag.UserList = userlist;
            }
            else
            {
                var user = db.Users.FirstOrDefault(s => s.Id == userId);
                model.Own = user;
                if (!string.IsNullOrWhiteSpace(user.ParentUserID))
                {
                    var gCode = db.RegistrationCodes
                          .Where(s => s.OwnUser == user.ParentUserID
                              && !s.UseTime.HasValue
                              && ((!s.ActiveEndDateTime.HasValue && !s.UseEndDateTime.HasValue)
                              || (s.ActiveEndDateTime > DateTime.Now
                              && s.UseEndDateTime > DateTime.Now)))
                          .GroupBy(s => new { s.ActiveEndDateTime, s.UseEndDateTime })
                          .Select(s => new RegistrationCodeCountViewModel
                          {
                              ActiveEndDateTime = s.Key.ActiveEndDateTime,
                              Max = s.Count(),
                              UseEndDateTime = s.Key.UseEndDateTime
                          }).ToList();
                    model.CodeCount = new List<RegistrationCodeCountViewModel>();
                    model.CodeCount.AddRange(gCode);

                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.RegistrationCodeManageCreate)]
        public ActionResult Create(RegistrationCodeCreate model, int length = 10)
        {

            Func<IQueryable<RegistrationCode>> queryCode = () =>
            {
                var checkCode = model.CodeCount.FirstOrDefault(s => s.Checked);
                return db.RegistrationCodes
                  .Where(s => s.ActiveEndDateTime == checkCode.ActiveEndDateTime
                      && s.UseEndDateTime == checkCode.UseEndDateTime);
            };


            if (model.Count < 1)
            {
                ModelState.AddModelError("Count", "数量不可小于1");
            }
            if (string.IsNullOrWhiteSpace(model.OwnUser))
            {
                ModelState.AddModelError("OwnUser", "请选择拥有用户");
            }
            if (!model.CodeCount?.Any(s => s.Checked) ?? false)
            {
                ModelState.AddModelError("CodeCount", "请选择批次");
            }
            if (model.CodeCount != null)
            {
                var max = queryCode().Count();
                if (model.Count > max)
                {
                    ModelState.AddModelError("Count", $"数量已超过批次的最大值{max}");
                }
            }
            if (ModelState.IsValid)
            {
                if (model.CodeCount == null)//普通的添加激活
                {
                    var codelist = db.RegistrationCodes.Select(s => s.Code).ToList();
                    var list = new List<RegistrationCode>();
                    var creteDateTime = DateTime.Now;
                    while (list.Count < model.Count)
                    {
                        var code = CreateCode(length);
                        if (list.Where(s => s.Code == code).Count() <= 0)
                        {
                            if (!codelist.Contains(code))
                            {
                                list.Add(new RegistrationCode()
                                {
                                    Code = code,
                                    CreateTime = creteDateTime,
                                    CreateUser = UserID,
                                    OwnUser = model.OwnUser,
                                    ActiveEndDateTime = model.ActiveDateTime?.AddHours(24).AddSeconds(-1),
                                    UseEndDateTime = model.UseEndDateTime?.AddHours(24).AddSeconds(-1),
                                });
                            }
                        }
                    }
                    db.RegistrationCodes.AddRange(list);
                    db.SaveChanges();
                }
                else//从父级买激活码
                {
                    var list = queryCode().Take(model.Count).ToList();
                    foreach (var item in list)
                    {
                        item.OwnUser = model.OwnUser;
                    }

                    db.SaveChanges();
                }
                if (this.GetReturnUrl() != null)
                {
                    return Redirect(this.GetReturnUrl());
                }
                return RedirectToAction("Index", "UserManage", null);
            }

            if (string.IsNullOrWhiteSpace(model.OwnUser))
            {
                var userlist = db.Users.Where(s => s.UserType == Enums.UserType.Proxy)
                    .Select(s => new SelectListItem()
                    {
                        Text = s.UserName,
                        Value = s.Id
                    }).ToList();
                ViewBag.UserList = userlist;
            }
            else
            {
                var user = db.Users.FirstOrDefault(s => s.Id == model.OwnUser);
                model.Own = user;
            }
            return View(model);
        }

        public string CreateCode(int length)
        {
            string str = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int len = str.Length;
            Random r = new Random();
            string result = null;
            for (int i = 0; i < length; i++)
            {
                int m = r.Next(0, len);
                string s = str.Substring(m, 1);
                result += s;
            }
            return result;
        }

        public ActionResult ChangeOwn(int id, string userId)
        {
            var code = db.RegistrationCodes.FirstOrDefault(s => s.ID == id);
            if (code == null)
            {
                return Json(Comm.ToJsonResult("Error", "没有这个注册码"));
            }
            if (code.UseTime.HasValue)
            {
                return Json(Comm.ToJsonResult("Success", "注册码已使用"));
            }
            code.OwnUser = userId;
            db.SaveChanges();
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        public ActionResult Export(string userId)
        {
            var query = from code in db.RegistrationCodes
                        from owner in db.Users
                        join uu in db.Users on code.UseUser equals uu.Id into u1
                        from use in u1.DefaultIfEmpty()
                        where code.OwnUser == owner.Id && (userId == null || code.OwnUser == userId)
                        orderby code.ID
                        select new RegistrationCodeViewModel()
                        {
                            Code = code.Code,
                            CreateTime = code.CreateTime,
                            ID = code.ID,
                            OwnUser = owner.PhoneNumber,
                            UseTime = code.UseTime,
                            UseUser = use.PhoneNumber,
                            ActiveEndDateTime = code.ActiveEndDateTime,
                            UseEndDateTime = code.UseEndDateTime
                        };
            this.AddExcelExportHead($"注册码");
            return View(query.ToList());
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
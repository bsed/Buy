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
            ViewBag.Sidebar = "用户管理";
        }

        // GET: RegistrationCode
        public ActionResult Index(string userId, int page = 1)
        {
            var list = db.RegistrationCodes.Where(s => s.OwnUser == userId).OrderBy(s => s.CreateTime).ToPagedList(page);
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
                    UseUser = s.UseUser
                };
                return item;
            });
            ViewBag.Paged = list;
            return View(model);
        }

        public ActionResult Create()
        {
            Sidebar();
            var userlist = db.Users.Where(s => s.UserType == Enums.UserType.Proxy)
                .Select(s => new SelectListItem()
                {
                    Text = s.UserName,
                    Value = s.Id
                }).ToList();
            ViewBag.UserList = userlist;
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegistrationCodeCreate model, int length = 10)
        {
            if (model.Count < 1)
            {
                ModelState.AddModelError("Count", "数量不可小于1");
            }
            if (string.IsNullOrWhiteSpace(model.OwnUser))
            {
                ModelState.AddModelError("OwnUser", "请选择拥有用户");
            }
            if (ModelState.IsValid)
            {
                var codelist = db.RegistrationCodes.Select(s => s.Code).ToList();
                var list = new List<RegistrationCode>();
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
                                CreateTime = DateTime.Now,
                                CreateUser = UserID,
                                OwnUser = model.OwnUser,
                            });
                        }
                    }
                }
                db.RegistrationCodes.AddRange(list);
                db.SaveChanges();
                return RedirectToAction("Index", "UserManage", null);
            }
            return View(model);
        }

        public string CreateCode(int length)
        {
            string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
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
    }
}
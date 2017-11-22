using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class UpdateLogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "版本管理";
        }

        // GET: UpdateLog
        [Authorize(Roles =SysRole.UpdateLogManageRead)]
        public ActionResult Index(int page = 1)
        {
            var logs = db.UpdateLogs.OrderByDescending(s => s.CreateDateTime).ToPagedList(page);
            return View(logs);
        }

        // GET: UpdateLog/Create
        [Authorize(Roles = SysRole.UpdateLogManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            var model = new UpdateLogViewModel()
            {
                FileUpload = new FileUpload()
                {
                    FilePath = "~/download/hisver",
                    IsResetName = true,
                    Name = "AndroidUrl",
                    Type = FileType.File,
                },
                CreateDateTime = DateTime.Now,
                Type = Enums.UpdateLogType.Android,
            };
            return View(model);
        }

        // POST: UpdateLog/Create
        [HttpPost]
        [Authorize(Roles = SysRole.UpdateLogManageCreate)]
        public ActionResult Create(UpdateLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(model.Content))
                {
                    ModelState.AddModelError("Content", "填写更新内容");
                }
                if (string.IsNullOrWhiteSpace(model.Ver))
                {
                    ModelState.AddModelError("Ver", "填写版本号");
                }
                if (model.Type == Enums.UpdateLogType.Android)
                {
                    if (model.FileUpload.Images.Count() <= 0)
                    {
                        ModelState.AddModelError("Url", "上传文件");
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.Url))
                    {
                        ModelState.AddModelError("Url", "下载链接");
                    }
                }
                if (ModelState.IsValid)
                {
                    var updateLog = new UpdateLog()
                    {
                        Content = model.Content,
                        CreateDateTime = model.CreateDateTime,
                        Type = model.Type,
                        Ver = model.Ver,
                        Url = model.Url,
                    };
                    if (model.Type == Enums.UpdateLogType.Android)
                    {
                        updateLog.Url = model.FileUpload.Images.First();
                    }
                    db.UpdateLogs.Add(updateLog);
                    db.SaveChanges();
                    string sourceFile = Server.MapPath($"{updateLog.Url}");
                    string destFile = Server.MapPath($"~/download/malieme.apk");
                    System.IO.File.Copy(sourceFile, destFile, true);
                    return RedirectToAction("Index");
                }
            }
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

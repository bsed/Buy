using Buy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class ClassManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ObservableCollection<BannerSetting> classifySetting = Bll.SystemSettings.ClassifySetting;

        private void Sidebar()
        {
            ViewBag.Sidebar = "分类管理";
        }

        // GET: ClassManage
        [Authorize(Roles = SysRole.ClassifyManageRead)]
        public ActionResult Index(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao, int page = 1)
        {
            Sidebar();
            var model = classifySetting.Where(s => s.Platform == platform)
                .AsQueryable().OrderBy(s => s.Code).ThenBy(s => s.Sort).ToPagedList(page);
            return View(model);
        }

        // GET: ClassManage/Create
        [HttpGet]
        [Authorize(Roles = SysRole.ClassifyManageCreate)]
        public ActionResult Create(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            Sidebar();
            var model = new BannerSettingCreateEditViewModel();
            model.Platform = platform;
            return View(model);
        }

        // POST: ClassManage/Create
        [HttpPost]
        [Authorize(Roles = SysRole.ClassifyManageCreate)]
        public ActionResult Create(BannerSettingCreateEditViewModel model)
        {
            Sidebar();
            if (model.Image.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (string.IsNullOrWhiteSpace(model.Code))
            {
                ModelState.AddModelError("Code", "编号 字段是必需的。");
            }
            if (ModelState.IsValid)
            {
                var id = classifySetting.Count == 0 ? 1 : (classifySetting.Max(s => s.ID) + 1);
                classifySetting.Add(new BannerSetting
                {
                    Image = model.Image.Images.FirstOrDefault(),
                    Link = model.Link,
                    Sort = model.Sort,
                    ID = id,
                    Code = model.Code,
                    Title = model.Title,
                    Platform = model.Platform,
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ClassManage/Edit/5
        [HttpGet]
        [Authorize(Roles = SysRole.ClassifyManageEdit)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var model = new BannerSettingCreateEditViewModel(classifySetting.FirstOrDefault(s => s.ID == id));
            return View(model);
        }

        // POST: ClassManage/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.ClassifyManageEdit)]
        public ActionResult Edit(BannerSettingCreateEditViewModel model)
        {
            Sidebar();
            if (model.Image.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (string.IsNullOrWhiteSpace(model.Code))
            {
                ModelState.AddModelError("Code", "填写编号");
            }
            if (ModelState.IsValid)
            {
                var setting = classifySetting.FirstOrDefault(s => s.ID == model.ID);
                var index = classifySetting.IndexOf(setting);

                setting.Image = model.Image.Images.FirstOrDefault();
                setting.Link = model.Link;
                setting.Sort = model.Sort;
                setting.Title = model.Title;
                setting.Code = model.Code;
                setting.Platform = model.Platform;

                classifySetting[index] = setting;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: ClassManage/Delete/5
        [HttpPost]
        [Authorize(Roles = SysRole.ClassifyManageDelete)]
        public ActionResult Delete(int id)
        {
            var t = classifySetting.FirstOrDefault(s => s.ID == id);
            if (t == null)
            {
                return Json(Comm.ToJsonResult("Error", "数据不存在"));
            }
            Bll.SystemSettings.BannerSetting.Remove(t);
            return Json(Comm.ToJsonResult("Success", "成功"));
        }
    }
}

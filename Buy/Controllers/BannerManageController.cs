using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.Collections.ObjectModel;

namespace Buy.Controllers
{
    public class BannerManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ObservableCollection<BannerSetting> bannerSetting = Bll.SystemSettings.BannerSetting;

        private void Sidebar()
        {
            ViewBag.Sidebar = "Banner管理";
        }

        [Authorize(Roles = SysRole.BannerManageRead)]
        public ActionResult Index(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            Sidebar();
            var model = bannerSetting.Where(s => s.Platform == platform);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = SysRole.BannerManageCreate)]
        public ActionResult Create(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            Sidebar();
            var model = new BannerSettingCreateEditViewModel();
            model.Platform = platform;
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = SysRole.BannerManageCreate)]
        public ActionResult Create(BannerSettingCreateEditViewModel model)
        {
            Sidebar();
            if (model.Image.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (ModelState.IsValid)
            {
                var id = bannerSetting.Count == 0 ? 1 : (bannerSetting.Max(s => s.ID) + 1);
                bannerSetting.Add(new BannerSetting
                {
                    Image = model.Image.Images.FirstOrDefault(),
                    Link = model.Link,
                    Sort = model.Sort,
                    ID = id,
                    Title = model.Title,
                    Platform = model.Platform,
                });
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = SysRole.BannerManageEdit)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var model = new BannerSettingCreateEditViewModel(bannerSetting.FirstOrDefault(s => s.ID == id));
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.BannerManageEdit)]
        public ActionResult Edit(BannerSettingCreateEditViewModel model)
        {
            Sidebar();
            if (model.Image.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (ModelState.IsValid)
            {
                var setting = bannerSetting.FirstOrDefault(s => s.ID == model.ID);
                var index = bannerSetting.IndexOf(setting);

                setting.Image = model.Image.Images.FirstOrDefault();
                setting.Link = model.Link;
                setting.Sort = model.Sort;
                setting.Title = model.Title;
                setting.Platform = model.Platform;

                bannerSetting[index] = setting;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.BannerManageDelete)]
        public ActionResult Delete(int id)
        {
            var t = bannerSetting.FirstOrDefault(s => s.ID == id);
            if (t == null)
            {
                return Json(Comm.ToJsonResult("Error", "数据不存在"));
            }
            Bll.SystemSettings.BannerSetting.Remove(t);
            return Json(Comm.ToJsonResult("Success", "成功"));
        }
    }
}
using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class ShopManageController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "商家管理";
        }

        // GET: ShopManage
        [Authorize(Roles = SysRole.ShopManageRead)]
        public ActionResult Index(int page = 1)
        {
            Sidebar();
            var shop = db.Shops.OrderBy(s=>s.Sort).ToPagedList(page);
            return View(shop);
        }

        // GET: ShopManage/Create
        [Authorize(Roles = SysRole.ShopManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            var shop = new ShopManageViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Name = "LogoFileUpload",
                    Type = FileType.Image
                }
            };
            return View(shop);
        }

        // POST: ShopManage/Create
        [HttpPost]
        [Authorize(Roles = SysRole.ShopManageCreate)]
        public ActionResult Create(ShopManageViewModel model)
        {
            if (model.FileUpload.Images.Length != 1)
            {
                ModelState.AddModelError("Logo", "上传logo");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (ModelState.IsValid)
            {
                var shop = new Shop()
                {
                    Code = model.Code,
                    Sort = model.Sort,
                    Logo = model.FileUpload.Images.FirstOrDefault(),
                    Name = model.Name,
                };
                db.Shops.Add(shop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ShopManage/Edit/5
        [Authorize(Roles = SysRole.ShopManageEdit)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var shop = db.Shops.FirstOrDefault(s => s.ID == id);
            var model = new ShopManageViewModel()
            {
                Code = shop.Code,
                Sort = shop.Sort,
                Logo = shop.Logo,
                Name = shop.Name,
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Images = new string[] { shop.Logo },
                    Type = FileType.Image,
                    Name = "LogoFileUpload",
                },
            };
            return View(model);
        }

        // POST: ShopManage/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.ShopManageEdit)]
        public ActionResult Edit(ShopManageViewModel model)
        {
            if (model.FileUpload.Images.Length != 1)
            {
                ModelState.AddModelError("Logo", "上传logo");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (ModelState.IsValid)
            {
                var shop = db.Shops.FirstOrDefault(s => s.ID == model.ID);
                shop.Name = model.Name;
                shop.Sort = model.Sort;
                shop.Logo = model.FileUpload.Images.FirstOrDefault();
                shop.Code = model.Code;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ShopManage/Delete/5
        [Authorize(Roles = SysRole.ShopManageDelete)]
        public ActionResult Delete(int id)
        {
            Sidebar();
            var shop = db.Shops.FirstOrDefault(s => s.ID == id);
            var model = new ShopManageViewModel()
            {
                Code = shop.Code,
                Sort = shop.Sort,
                Logo = shop.Logo,
                Name = shop.Name,
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Images = new string[] { shop.Logo },
                    Type = FileType.Image,
                    Name = "LogoFileUpload",
                },
            };
            return View(model);
        }

        // POST: ShopManage/Delete/5
        [Authorize(Roles = SysRole.ShopManageDelete)]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var shop = db.Shops.FirstOrDefault(s => s.ID == id);
            if (shop.Coupons.Count > 0)
            {
                ModelState.AddModelError("", "先删除本地券");
                return View(shop);
            }
            db.Shops.Remove(shop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

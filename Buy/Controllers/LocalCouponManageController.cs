using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class LocalCouponManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "本地券管理";
        }

        // GET: LocalCouponManage
        public ActionResult Index(int? shopId, int page = 1)
        {
            Sidebar();
            GetShop();
            var lc = db.LocalCoupons.AsQueryable();
            if (shopId.HasValue)
            {
                lc = lc.Where(s => s.ShopID == shopId.Value);
            }
            var model = lc.OrderBy(s => s.CreateDateTime).ToPagedList(page);
            return View(model);
        }

        // GET: LocalCouponManage/Create
        public ActionResult Create()
        {
            Sidebar();
            GetShop();
            var model = new LocalCouponViewModel()
            {
                CreateDateTime = DateTime.Now,
                EndDateTime = DateTime.Now,
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "ImageFileUpload",
                },
            };
            return View(model);
        }

        // POST: LocalCouponManage/Create
        [HttpPost]
        public ActionResult Create(LocalCouponViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (model.FileUpload.Images.Length <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (model.ShopID <= 0)
            {
                ModelState.AddModelError("ShopID", "选择商家");
            }
            if (ModelState.IsValid)
            {
                var lc = new LocalCoupon()
                {
                    CreateDateTime = model.CreateDateTime,
                    ShopID = model.ShopID,
                    Remark = model.Remark,
                    EndDateTime = model.EndDateTime,
                    Image = model.FileUpload.Images.FirstOrDefault(),
                    Name = model.Name,
                    Price = model.Price,
                    Commission=model.Commission
                };
                db.LocalCoupons.Add(lc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: LocalCouponManage/Edit/5
        public ActionResult Edit(int id)
        {
            Sidebar();
            GetShop();
            var lc = db.LocalCoupons.FirstOrDefault(s => s.ID == id);
            var model = new LocalCouponViewModel()
            {
                CreateDateTime = lc.CreateDateTime,
                EndDateTime = lc.EndDateTime,
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "ImageFileUpload",
                    Images = new string[] { lc.Image },
                },
                ShopID = lc.ShopID,
                Shop = lc.Shop,
                Image = lc.Image,
                Name = lc.Name,
                ID = lc.ID,
                Price = lc.Price,
                Remark = lc.Remark,
                Commission = lc.Commission
            };
            return View(model);
        }

        // POST: LocalCouponManage/Edit/5
        [HttpPost]
        public ActionResult Edit(LocalCouponViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (model.FileUpload.Images.Length <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (model.ShopID <= 0)
            {
                ModelState.AddModelError("ShopID", "选择商家");
            }
            if (ModelState.IsValid)
            {
                var lc = db.LocalCoupons.FirstOrDefault(s => s.ID == model.ID);
                lc.Image = model.FileUpload.Images.FirstOrDefault();
                lc.Name = model.Name;
                lc.Price = model.Price;
                lc.Remark = model.Remark;
                lc.ShopID = model.ShopID;
                lc.CreateDateTime = model.CreateDateTime;
                lc.EndDateTime = model.EndDateTime;
                lc.Commission = model.Commission;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            GetShop();
            Sidebar();
            return View(model);
        }

        // GET: LocalCouponManage/Delete/5
        public ActionResult Delete(int id)
        {
            Sidebar();
            var lc = db.LocalCoupons.FirstOrDefault(s => s.ID == id);
            return View(lc);
        }

        // POST: LocalCouponManage/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var lc = db.LocalCoupons.FirstOrDefault(s => s.ID == id);
            db.LocalCoupons.Remove(lc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void GetShop()
        {
            var shop = db.Shops.Select(s => new SelectListItem()
            {
                Value = s.ID.ToString(),
                Text = s.Name,
            }).ToList();
            ViewBag.Shop = shop;
        }

    }
}

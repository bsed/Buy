using Buy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class CouponTypeManageController : Controller
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
            ViewBag.Sidebar = "优惠券类型管理";
        }

        private ObservableCollection<CouponType> couponType = Bll.SystemSettings.ThirdPartyTicketType;

        // GET: CouponTypeManage
        public ActionResult Index(int page = 1)
        {
            var type = couponType.Where(s => s.ParentID == 0)
                .AsQueryable()
                .OrderBy(s => s.Sort)
                .ToPagedList(page);
            return View(type);
        }

        // GET: CouponTypeManage/Details/5
        public ActionResult Details(int parentID, int page = 1)
        {
            var type = couponType.Where(s => s.ParentID == parentID).AsQueryable()
                .OrderBy(s => s.Sort)
                .ToPagedList(page);
            return View(type);
        }

        // GET: CouponTypeManage/Create
        public ActionResult Create(int parentID=0)
        {
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage"
                },
                ParentID = parentID,
            };
            if (parentID!=0)
            {
                ViewBag.ParentCouponType = db.CouponTypes.FirstOrDefault(s => s.ID == parentID);
            }
            return View(model);
        }

        // POST: CouponTypeManage/Create
        [HttpPost]
        public ActionResult Create(CouponTypeViewModel model)
        {
            if (model.ParentID != 0)
            {
                if (model.FileUpload.Images.Count() <= 0)
                {
                    ModelState.AddModelError("Image", "二级分类要上传图片");
                }
            }
            if (ModelState.IsValid)
            {
                var type = new CouponType()
                {
                    Image = model.FileUpload.Images.FirstOrDefault(),
                    Keyword = model.Keyword,
                    Name = model.Name,
                    Sort = model.Sort,
                    ParentID = model.ParentID,
                };
                couponType.Add(type);
                if (model.ParentID == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { parentID = model.ParentID });
                }
            }
            return View(model);
        }

        // GET: CouponTypeManage/Edit/5
        public ActionResult Edit(int id)
        {
            var type = couponType.FirstOrDefault(s => s.ID == id);
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage",
                    Images = new string[] { type.Image }
                },
                ParentID = type.ParentID,
                ID = type.ID,
                Image = type.Image,
                Keyword = type.Keyword,
                Name = type.Name,
                Sort = type.Sort,
            };
            if (model.ParentID != 0)
            {
                ViewBag.ParentCouponType = db.CouponTypes.FirstOrDefault(s => s.ID == model.ParentID);
            }
            return View(model);
        }

        // POST: CouponTypeManage/Edit/5
        [HttpPost]
        public ActionResult Edit(CouponTypeViewModel model)
        {
            if (model.ParentID != 0)
            {
                if (model.FileUpload.Images.Count() <= 0)
                {
                    ModelState.AddModelError("Image", "二级分类要上传图片");
                }
            }
            if (ModelState.IsValid)
            {
                var type = couponType.FirstOrDefault(s => s.ID == model.ID);
                var index = couponType.IndexOf(type);

                type.Image = model.FileUpload.Images.FirstOrDefault();
                type.Keyword = model.Keyword;
                type.Name = model.Name;
                type.Sort = model.Sort;

                couponType[index] = type;
                if (model.ParentID == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { parentID = model.ParentID });
                }
            }
            return View(model);
        }

        // POST: CouponTypeManage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (couponType.Any(s => s.ParentID == id))
            {
                return Json(Comm.ToMobileResult("Error", "先删除他的二级分类"));
            }
            var type = couponType.FirstOrDefault(s => s.ID == id);
            if (type == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有这个类型"));
            }
            couponType.Remove(type);
            return Json(Comm.ToMobileResult("Success", "成功"));
        }
    }
}

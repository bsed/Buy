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
        private ApplicationDbContext db = new ApplicationDbContext();

        private ObservableCollection<CouponType> couponType = Bll.SystemSettings.CouponType;

        private void Sidebar()
        {
            ViewBag.Sidebar = "优惠券类型管理";
        }

        // GET: CouponTypeManage
        public ActionResult Index(int pid = 0)
        {
            Sidebar();
            var tree = new CouponTypeTreeNode
            {
                Childs = new List<CouponTypeTreeNode>(),
                ID = 0,
                Name = "全部",
                ParentID = -1
            };
            Action<CouponTypeTreeNode> setTree = null;
            setTree = p =>
            {
                var childs = couponType.Where(s => s.ParentID == p.ID)
                    .OrderBy(s => s.Sort)
                    .ThenBy(s => s.ID)
                    .Select(s => new CouponTypeTreeNode
                    {
                        ID = s.ID,
                        Childs = new List<CouponTypeTreeNode>(),
                        Name = s.Name,
                        ParentID = s.ParentID
                    })
                    .ToList();
                if (childs.Count > 0)
                {
                    p.Childs.AddRange(childs);
                    foreach (var item in childs)
                    {
                        setTree(item);
                    };
                }
            };
            setTree(tree);
            ViewBag.Tree = tree;
            Sidebar();
            return View(couponType.ToList().Where(s => s.ParentID == pid).OrderBy(s => s.Sort).ThenBy(s => s.ID));
        }

        // GET: CouponTypeManage/Create
        public ActionResult Create(int pid = 0)
        {
            Sidebar();
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage"
                },
                ParentID = pid,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Create
        [HttpPost]
        public ActionResult Create(CouponTypeViewModel model)
        {
            if (model.FileUpload.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                ModelState.AddModelError("Keyword", "填写关键字");
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
                db.SaveChanges();
                return RedirectToAction("Index", new { pid = model.ParentID });
            }
            Sidebar();
            return View(model);
        }

        // GET: CouponTypeManage/Edit/5
        public ActionResult Edit(int id)
        {
            Sidebar();
            var type = couponType.FirstOrDefault(s => s.ID == id);
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage",
                    Images = string.IsNullOrWhiteSpace(type.Image) ? new string[] { } : new string[] { type.Image }
                },
                ParentID = type.ParentID,
                ID = type.ID,
                Image = type.Image,
                Keyword = type.Keyword,
                Name = type.Name,
                Sort = type.Sort,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Edit/5
        [HttpPost]
        public ActionResult Edit(CouponTypeViewModel model)
        {
            if (model.FileUpload.Images.Count() <= 0)
            {
                ModelState.AddModelError("Image", "上传图片");
            }
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                ModelState.AddModelError("Keyword", "填写关键字");
            }
            if (ModelState.IsValid)
            {
                var type = couponType.FirstOrDefault(s => s.ID == model.ID);
                type.Image = model.FileUpload.Images.FirstOrDefault();
                type.Keyword = model.Keyword;
                type.Name = model.Name;
                type.Sort = model.Sort;
                db.SaveChanges();
                return RedirectToAction("Index", new { pid = model.ParentID });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Move(int pid, List<int> ids)
        {
            var items = couponType.Where(s => ids.Contains(s.ID)).ToList();
            foreach (var id in ids)
            {
                var item = couponType.FirstOrDefault(s => s.ID == id);
                var index = couponType.IndexOf(item);
                item.ParentID = pid;
                item.Sort = ids.IndexOf(id) * 10;
                couponType[index] = item;
            }
            return Json(Comm.ToJsonResult("Success", "成功"));
        }

        // GET: CouponTypeManage/Edit/5
        public ActionResult Delete(int id)
        {
            Sidebar();
            var type = couponType.FirstOrDefault(s => s.ID == id);
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage",
                    Images = string.IsNullOrWhiteSpace(type.Image) ? new string[] { } : new string[] { type.Image }
                },
                ParentID = type.ParentID,
                ID = type.ID,
                Image = type.Image,
                Keyword = type.Keyword,
                Name = type.Name,
                Sort = type.Sort,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var type = couponType.FirstOrDefault(s => s.ID == id);
            if (couponType.Any(s => s.ParentID == id))
            {
                ModelState.AddModelError("", "先删除他的二级分类");
            }
            if (type == null)
            {
                ModelState.AddModelError("", "没有这个类型");
            }
            if (ModelState.IsValid)
            {
                couponType.Remove(type);
                return RedirectToAction("Index", new { pid = type.ParentID });
            }
            var model = new CouponTypeViewModel()
            {
                FileUpload = new FileUpload()
                {
                    Max = 1,
                    Type = FileType.Image,
                    Name = "CouponTypeViewModelImage",
                    Images = string.IsNullOrWhiteSpace(type.Image) ? new string[] { } : new string[] { type.Image }
                },
                ParentID = type.ParentID,
                ID = type.ID,
                Image = type.Image,
                Keyword = type.Keyword,
                Name = type.Name,
                Sort = type.Sort,
            };
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

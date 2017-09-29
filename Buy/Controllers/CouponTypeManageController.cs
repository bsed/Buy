using Buy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        [Authorize(Roles = SysRole.CouponTypeManageRead)]
        public ActionResult Index(Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao, int pid = 0)
        {
            Sidebar();
            var tree = new CouponTypeTreeNode
            {
                Childs = new List<CouponTypeTreeNode>(),
                ID = 0,
                Name = "全部",
                ParentID = -1
            };
            if (platform == Enums.CouponPlatform.TMall)
            {
                platform = Enums.CouponPlatform.TaoBao;
            }
            Action<CouponTypeTreeNode> setTree = null;
            setTree = p =>
            {
                var childs = couponType.Where(s => s.ParentID == p.ID && s.Platform == platform)
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
            return View(couponType.ToList().Where(s => s.ParentID == pid && s.Platform == platform).OrderBy(s => s.Sort).ThenBy(s => s.ID));
        }

        // GET: CouponTypeManage/Create
        [Authorize(Roles = SysRole.CouponTypeManageCreate)]
        public ActionResult Create(int pid = 0, Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
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
                Platform = platform,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Create
        [HttpPost]
        [Authorize(Roles = SysRole.CouponTypeManageCreate)]
        public ActionResult Create(CouponTypeViewModel model)
        {
            //if (model.FileUpload.Images.Count() <= 0)
            //{
            //    ModelState.AddModelError("Image", "上传图片");
            //}
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            //if (string.IsNullOrWhiteSpace(model.Keyword))
            //{
            //    ModelState.AddModelError("Keyword", "填写关键字");
            //}
            if (ModelState.IsValid)
            {
                var type = new CouponType()
                {
                    Image = model.FileUpload.Images.FirstOrDefault(),
                    Keyword = model.Keyword.Replace(" ", ""),
                    Name = model.Name,
                    Sort = model.Sort,
                    ParentID = model.ParentID,
                    Platform = model.Platform,
                };
                couponType.Add(type);
                return RedirectToAction("Index", new { pid = model.ParentID, platform = model.Platform });
            }
            Sidebar();
            return View(model);
        }

        // GET: CouponTypeManage/Edit/5
        [Authorize(Roles = SysRole.CouponTypeManageEdit)]
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
                Platform = type.Platform,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.CouponTypeManageEdit)]
        public ActionResult Edit(CouponTypeViewModel model)
        {
            //if (model.FileUpload.Images.Count() <= 0)
            //{
            //    ModelState.AddModelError("Image", "上传图片");
            //}
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "填写名称");
            }
            //if (string.IsNullOrWhiteSpace(model.Keyword))
            //{
            //    ModelState.AddModelError("Keyword", "填写关键字");
            //}
            if (ModelState.IsValid)
            {
                var type = couponType.FirstOrDefault(s => s.ID == model.ID);
                var index = couponType.IndexOf(type);

                type.Image = model.FileUpload.Images.FirstOrDefault();
                type.Keyword = model.Keyword.Replace(" ", "");
                type.Name = model.Name;
                type.Sort = model.Sort;
                type.Platform = model.Platform;

                couponType[index] = type;
                return RedirectToAction("Index", new { pid = model.ParentID, platform = model.Platform });
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.CouponTypeManageEdit)]
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

        // GET: CouponTypeManage/Delete/5
        [Authorize(Roles = SysRole.CouponTypeManageDelete)]
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
                Platform = type.Platform,
            };
            return View(model);
        }

        // POST: CouponTypeManage/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = SysRole.CouponTypeManageDelete)]
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
                return RedirectToAction("Index", new { pid = type.ParentID, platform = type.Platform });
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


        [HttpPost]
        [Authorize(Roles = SysRole.CouponTypeManageDelete)]
        public ActionResult DeleteAll(string ids)
        {
            var idList = ids.SplitToIntArray();
            if (idList != null && idList.Count > 0)
            {
                var type = couponType.Where(s => idList.Contains(s.ID));
                if (couponType.Any(s => idList.Contains(s.ParentID)))
                {
                    return Json(Comm.ToJsonResult("Error", "删除二级分类"));
                }
                foreach (var item in type.ToList())
                {
                    couponType.Remove(item);
                }
                return Json(Comm.ToJsonResult("Success", "成功"));
            }
            return Json(Comm.ToJsonResult("Error", "没有选择类型"));
        }
        
        public ActionResult MGJType()
        {
            Sidebar();
            string json = GetFileJson(Server.MapPath("~/App_Data/temp.json"));
            JArray json1 = (JArray)JsonConvert.DeserializeObject(json);
            List<CouponTypeTreeNode> aa = new List<CouponTypeTreeNode>();
            foreach (var item in json1)
            {
                int? typeid = null;
                if (!string.IsNullOrWhiteSpace(item["TypeID"].ToString()))
                {
                    typeid = int.Parse(item["TypeID"].ToString());
                }
                //赋值属性
                if (int.Parse(item["Count"].ToString()) > 0)
                {
                    aa.Add(new CouponTypeTreeNode()
                    {
                        TestID = item["ID"].ToString(),
                        Count = int.Parse(item["Count"].ToString()),
                        IsLeaf = bool.Parse(item["IsLeaf"].ToString()),
                        Name = item["Name"].ToString(),
                        TypeID = typeid,
                        ParentId = item["ParentId"].ToString(),
                        Childs = new List<CouponTypeTreeNode>()
                    });
                }
            }
            var tree = new CouponTypeTreeNode
            {
                Childs = new List<CouponTypeTreeNode>(),
                TestID = "",
                Name = "全部",
                ParentId = "",
                Count = 0,
                IsLeaf = true,
                TypeID = null
            };
            Action<CouponTypeTreeNode> setTree = null;
            setTree = p =>
            {
                var childs = aa.Where(s => s.ParentId == p.TestID)
                    .Select(s => new CouponTypeTreeNode
                    {
                        ID = s.ID,
                        Childs = new List<CouponTypeTreeNode>(),
                        Name = s.Name,
                        ParentId = s.ParentId,
                        Count = s.Count,
                        IsLeaf = s.IsLeaf,
                        TypeID = s.TypeID,
                        TestID = s.TestID
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
            return View(tree);
        }
        public string GetFileJson(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    byte[] byArray = new byte[fs.Length];
                    fs.Read(byArray, 0, (int)fs.Length);
                    json = Encoding.UTF8.GetString(byArray).Replace("\n", "").Replace("\t", "").Replace("\r", "");
                }
            }
            return json;
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

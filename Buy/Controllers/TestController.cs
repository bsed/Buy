using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JiebaNet.Segmenter;
using JiebaNet.Analyser;
using JiebaNet.Segmenter.PosSeg;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace Buy.Controllers
{
    public class TestController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index(string text)
        {
            return Json(Comm.ToJsonResult("Success", ""), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReAddCodeLog()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var codes = db.RegistrationCodes.GroupBy(s => new
                {
                    s.OwnUser,
                    Date = DbFunctions.TruncateTime(s.CreateTime)
                })
                    .Select(s => new
                    {
                        s.Key.OwnUser,
                        s.Key.Date,
                        Count = s.Count()
                    }).ToList()
                    .Select(s => new RegistrationCodeLog
                    {
                        Count = s.Count,
                        CreateDateTime = s.Date.Value,
                        UserID = s.OwnUser,

                    });
                //db.RegistrationCodeLogs.AddRange(codes);
                //db.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);
            };

        }

        public ActionResult DeletTempFile()
        {
            var path = Request.MapPath("~/Upload/");
            var keys = new string[] { ".json", ".xls" };
            var dir = new DirectoryInfo(path);
            var date = DateTime.Now.Date;
            var files = dir.GetFiles().Where(s => keys.Contains(s.Extension.ToLower()) && s.CreationTime < date).ToList();
            foreach (var item in files)
            {
                item.Delete();
            }
            return Json(Comm.ToJsonResult("Success", $"{files.Count}个缓存文件已删除"), JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteRepeatCoupon()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userCoupons = db.CouponUsers.GroupBy(s => new
                {
                    s.CouponID,
                    s.UserID
                }).Select(s => new
                {
                    s.Key.CouponID,
                    s.Key.UserID,
                    Count = s.Count()
                }).Where(s => s.Count > 1).ToList();
                int count = 0;
                foreach (var item in userCoupons)
                {
                    var coupons = db.CouponUsers
                        .Where(s => s.CouponID == item.CouponID && s.UserID == item.UserID)
                        .OrderBy(s => s.ID).Skip(1).ToList();
                    db.CouponUsers.RemoveRange(coupons);
                    count += db.SaveChanges();
                }
                return Json(Comm.ToJsonResult("State", "", count), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult FixCountType()
        {
            MoGuJie.Method.ReSetCidFile();
            return null;
        }
    }

}
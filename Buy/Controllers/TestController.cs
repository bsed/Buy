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

namespace Buy.Controllers
{
    public class TestController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index(string text)
        {


            return Json(Comm.ToJsonResult("Success", ""), JsonRequestBehavior.AllowGet);
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

        public ActionResult FixCountType()
        {
            MoGuJie.Method.ReSetCidFile();
            return null;
        }
    }

}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace Buy.Controllers
{
    public class TaobaoController : Controller
    {
        // GET: Taobao
        [HttpPost]
        public ActionResult Import(string userID, string url)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                return Json(Comm.ToJsonResult("Error", "失败"));
            }
            string path = Request.MapPath(url);
            Taobao.Import(userID, path);
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            return Json(Comm.ToJsonResult("Success", "成功"));
        }
    }
}
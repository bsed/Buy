using System;
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
        [AllowCrossSiteJson]
        public ActionResult Import(string userID, string url)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                return Json(Comm.ToJsonResult("Error", "失败"));
            }
            string path = Request.MapPath(url);
            Taobao.Import(userID, path);
            try
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
            catch (Exception ex)
            {
                Comm.WriteLog("TaoBaoImort", $"删除缓存失败：{ex.Message}", Enums.DebugLogLevel.Error, Url.Action(), ex);
            }

            return Json(Comm.ToJsonResult("Success", "成功"));
        }
    }
}
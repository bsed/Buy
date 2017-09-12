using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class SecurityController : Controller
    {
        string key = "YWSecurity";

        [HttpPost]
        public ActionResult Encrypt(string content)
        {
           
            var x = Buy.Security.Encrypt(content, key);
            return Json(Comm.ToJsonResult("Success", "成功", x));
        }

        [HttpPost]
        public ActionResult Decrypt(string content)
        {
            var y = Buy.Security.Decrypt(content, key);
            return Json(Comm.ToJsonResult("Success", "成功", y));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            string key = "yw123456";
            var x = Buy.Security.Encrypt("12312", key);
            var y = Buy.Security.Decrypt(x, key);
            return Json("1", JsonRequestBehavior.AllowGet);

        }



    }

}
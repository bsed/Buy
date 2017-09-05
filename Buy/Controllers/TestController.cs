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
            var api = new Api.BaseApi("http://localhost:62209/test/test", "POST", new { UserName = "admin", Password = "123456" });
            var result = api.CreateRequestReturnJson();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test(string userName, string password)
        {
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }
    }
}
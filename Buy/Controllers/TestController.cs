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
        public ActionResult Index(string phone)
        {
            var url = "https://taoquan.taobao.com/coupon/unify_apply.htm?sellerId=2081659912&activityId=0afbb33985824d91bafbf40f7916e12b";
            Uri myUri = new Uri(url);
            string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("activityId");
            return Json("1");
        }



    }

}
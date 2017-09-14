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
            ISms sms = new YunPianSms();
            sms.Send(phone, Comm.Random.Next(1000, 9999).ToString());
            return Json("1");
        }



    }

}
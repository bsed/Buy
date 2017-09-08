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
            using (var db = new Models.ApplicationDbContext())
            {
                var list = new List<Models.Coupon>();
                list.Add(new Models.Coupon { ID = 1, Name = "1" });
                list.Add(new Models.Coupon { ID = 2, Name = "2" });
                var data = new TestModel()
                {
                    Coupons = list,
                    IDS = new string[] { "1", "2" },
                    Time = DateTime.Now,
                    UserName = "admin"
                };
                var api = new Api.BaseApi("http://localhost:62209/test/test", "POST", data);
                //api.Files.Add("file", @"E:\project\Buy\Buy\123123.xls");
                var result = api.CreateRequestReturnJson();
                return Json(Comm.ToMobileResult("Success", "OK", result), JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult Test(TestModel model)
        {
            this.UploadFile();
            return Json(Comm.ToMobileResult("Success", "成功"), JsonRequestBehavior.AllowGet);
        }


        public class TestModel
        {
            public string UserName { get; set; }

            public string[] IDS { get; set; }

            public DateTime Time { get; set; }

            public List<Models.Coupon> Coupons { get; set; }
        }
    }

}
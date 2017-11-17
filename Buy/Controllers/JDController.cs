using Buy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
namespace Buy.Controllers
{
    public class JDController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: JD
        public ActionResult Login(string code, string state)
        {
            var redirect_uri = Url.ContentFull(Url.Action("Login"));
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (string.IsNullOrWhiteSpace(state))
                {

                }

                var jd = new Buy.Jd.Method();
                jd.GetAccessToken(code, redirect_uri, state);
                return Redirect(state);
            }

            string urlAuthorize = $"https://oauth.jd.com/oauth/authorize?response_type=code&client_id={Buy.Jd.Config.AppKey}&redirect_uri={redirect_uri}&state={state}";
            return Redirect(urlAuthorize);
        }


        public ActionResult LoginTemp()
        {
            return Redirect("http://passport.jd.com/common/loginPage?from=media&ReturnUrl=https://media.jd.com/gotoadv/theme?type=0&pageSize=50");
        }

        public ActionResult GetToken()
        {
            return Json(new Jd.Method().Token, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Test(string ids)
        {
            var jd = new Jd.Method();
            jd.Token = new Jd.Token
            {
                AccessToken = "ac982a0d-3bf8-448d-83a4-7e47d41bed3a",
                RefreshToken = "efad5f14-5283-48c9-8839-c50155b635cd",
                UserID = "8316176536"
            };
            var result = jd.QueryCouponGoods();
            return Json(result, JsonRequestBehavior.AllowGet); ;
        }


        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Import(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Json(Comm.ToJsonResult("Error", "Url为空"));
            }
            string path = Server.MapPath(url);
            if (!System.IO.File.Exists(path))
            {
                return Json(Comm.ToJsonResult("FileNoFound", "导入数据不存在"));
            }
            try
            {
                var result = System.IO.File.ReadAllText(path);
                List<CouponUserViewModel> models = JsonConvert.DeserializeObject<List<CouponUserViewModel>>(result);
                Bll.Coupons.DbAdd(models);
                System.IO.File.Delete(path);
                return Json(Comm.ToJsonResult("Success", "成功"));
            }
            catch (Exception ex)
            {
                return Json(Comm.ToJsonResult("Error", ex.Message));
            }


        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetCid()
        {
            var model = db.CouponTypes
                  .Where(s => s.Platform == Enums.CouponPlatform.Jd)
                  .Select(s => new { s.ID, s.Keyword, s.Name })
                  .ToList()
                  .Where(s=>!string.IsNullOrWhiteSpace(s.Keyword))
                  .ToList();
            return Json(Comm.ToJsonResult("Success", "成功", model), JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
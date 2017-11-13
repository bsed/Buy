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
        public ActionResult Import()
        {
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                return Json(Comm.ToJsonResult("FileNoFound", "导入数据不存在"));
            }
            try
            {
                var file = Request.Files[0];
                System.IO.BinaryReader b = new System.IO.BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(Convert.ToInt32(file.InputStream.Length));

                string result = System.Text.Encoding.UTF8.GetString(binData);
                List<CouponUserViewModel> models = JsonConvert.DeserializeObject<List<CouponUserViewModel>>(result);
                Bll.Coupons.DbAdd(models);
                return Json(Comm.ToJsonResult("Success", "成功"));
            }
            catch (Exception ex)
            {
                return Json(Comm.ToJsonResult("Error", ex.Message));
            }


        }
        
    }
}
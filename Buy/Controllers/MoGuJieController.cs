using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Buy.Models;

namespace Buy.Controllers
{
    public class MoGuJieController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [AllowCrossSiteJson]
        public ActionResult Login(string code, string state)
        {
            var redirect_uri = Url.ContentFull(Url.Action("Login"));
            if (!string.IsNullOrWhiteSpace(code))
            {
                var mgj = new Buy.MoGuJie.Method();
                mgj.GetAccessToken(code, redirect_uri, state);
                if (state.ToLower() == "token")
                {
                    return Json(Comm.ToJsonResult("Success", "成功", mgj.Token), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Redirect(state);
                }

            }

            string urlAuthorize = $"https://oauth.mogujie.com/authorize?response_type=code&app_key={MoGuJie.Config.AppKey}&redirect_uri={redirect_uri}&state={state}";
            return Redirect(urlAuthorize);
        }

        public ActionResult LoginSuccess()
        {
            var mgj = new MoGuJie.Method();
            var token = mgj.Token;
            return View(token);
        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult GetCategory()
        {
            var mgj = new MoGuJie.Method();
            var cids = MoGuJie.Method.AllCategory;
            foreach (var cid in cids)
            {
                cid.TypeID = Bll.Coupons.CheckType(cid.Name, Enums.CouponPlatform.MoGuJie);
            }
            return Json(Comm.ToJsonResult("Success", "成功", cids), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult ImportItems(string url)
        {
            var path = Request.MapPath(url);
            string text = System.IO.File.ReadAllText(path);
            List<Models.Coupon> models = JsonConvert.DeserializeObject<List<Models.Coupon>>(text);
            Bll.Coupons.DbAdd(models);

            return Json(Comm.ToJsonResult("Success", "成功"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RefreshTaken()
        {
            var mgj = new Buy.MoGuJie.Method();
            try
            {
                if (mgj.Token == null)
                {
                    return Json(Comm.ToJsonResult("Error", "授权过期"));
                }
                mgj.RefeashToken();
                return Json(Comm.ToJsonResult("Success", "成功"));
            }
            catch (Exception ex)
            {
                return Json(Comm.ToJsonResult("Error", ex.Message));
            }

        }

        [HttpGet]
        [AllowCrossSiteJson]
        public ActionResult Count(string userID)
        {
            var count = db.Coupons.Count(s => s.Platform == Enums.CouponPlatform.MoGuJie);

            return Json(Comm.ToJsonResult("Success", "成功", new { Count = count }), JsonRequestBehavior.AllowGet);
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
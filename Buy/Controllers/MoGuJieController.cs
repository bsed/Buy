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
            return View();
        }


        public ActionResult GetCategory()
        {
            var mgj = new MoGuJie.Method();
            var cids = MoGuJie.Method.AllCategory;
            foreach (var cid in cids)
            {
                cid.TypeID = Bll.Coupons.CheckType(cid.Name);
            }
            return Json(Comm.ToJsonResult("Success", "成功", cids), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImportItems(List<Models.Coupon> models)
        {
            Bll.Coupons.DbAdd(models);

            return Json(Comm.ToJsonResult("Success", "成功"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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

    }
}
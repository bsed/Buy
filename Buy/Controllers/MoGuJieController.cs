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


        // GET: MoGuJie
        public ActionResult Login(string code, string state)
        {
            var redirect_uri = Url.ContentFull(Url.Action("Login"));
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (state.ToLower() == "token")
                {
                    var mgj = new Buy.MoGuJie.Method();
                    mgj.GetAccessToken(code, redirect_uri, state);
                    return Json(Comm.ToJsonResult("Success", "成功", mgj.Token));
                }
                else
                {
                    var mgj = new Buy.MoGuJie.Method();
                    mgj.GetAccessToken(code, redirect_uri, state);
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


        public ActionResult ImportItems()
        {
            var mgj = new MoGuJie.Method();


            var cids = MoGuJie.Method.AllCategory;
            //判断CID属于哪个TypeID
            cids.ForEach(s =>
            {
                s.TypeID = Bll.ThirdPartyTickets.CheckType(s.Name);
            });

            DateTime dtCreate = DateTime.Now.Date;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {


                //var pageSize = Bll.SystemSettings.ThirPartyTicketSetting.PageSize;
                var pageSize = 100;
                var models = new List<Coupon>();
                foreach (var cid in cids)
                {
                    //获取第一页
                    var result = mgj.GetItemList(pageNo: 1, pageSize: pageSize, cid: cid.ID);
                    Action<IEnumerable<Coupon>> set = items =>
                    {
                        try
                        {
                            //设置导入没有的字段
                            foreach (var item in items)
                            {
                                item.ProductType = cid.Name;
                                item.TypeID = cid.TypeID;
                            }
                        }
                        catch (Exception ex)
                        {
                            Comm.WriteLog("mgj", $"{ex.Message}", Enums.DebugLogLevel.Error);
                        }

                    };
                    if (result.Total > 0)
                    {
                        set(result.Items);
                        models.AddRange(result.Items);
                        //获取
                        for (int i = 2; i <= result.TotalPage; i++)
                        {
                            var result2 = mgj.GetItemList(pageNo: i, pageSize: pageSize, cid: cid.ID);
                            set(result2.Items);
                            models.AddRange(result2.Items);
                        }

                    }

                }
                Bll.ThirdPartyTickets.DbAdd(models);
            }


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

        public ActionResult TestImportItems()
        {
            var mgj = new MoGuJie.Method();
            var cids = MoGuJie.Method.AllCategory;
            //判断CID属于哪个TypeID
            cids.ForEach(s =>
            {
                s.TypeID = Bll.ThirdPartyTickets.CheckType(s.Name);
            });

            DateTime dtCreate = DateTime.Now;
            mgj.Token = new MoGuJie.Token
            {
                AccessToken = "53DD26057B44982C303C3FE642F034F2",
                RefreshToken = "E8920E3E16587AA8DBE010EA551831F6",
                UserID = "155ya1o"
            };

            //获取第一页
            var result = mgj.GetItemList(pageNo: 1, cid: cids[0].ID);



            return Json(Comm.ToJsonResult("Success", "成功"), JsonRequestBehavior.AllowGet);
        }
    }
}
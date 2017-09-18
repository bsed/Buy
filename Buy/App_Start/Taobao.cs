using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Top.Api.Domain;
using static Top.Api.Response.TbkDgItemCouponGetResponse;

namespace Buy
{
    public class Taobao
    {
        private string _server = "http://gw.api.taobao.com/router/rest";

        public Taobao()
        {
            var key = "24469004";
            var secret = "480fd9930a4563e4979341cbc8566224";
            _client = new DefaultTopClient(_server, key, secret);
        }

        public Taobao(string key, string secret)
        {
            _client = new DefaultTopClient(_server, key, secret);
        }

        private ITopClient _client;

        public ITopClient TopClient
        {
            get { return _client; }
        }

        public string GetWirelessShareTpwd(string logo, string url, string title, long userID, object ext = null)
        {
            WirelessShareTpwdCreateRequest req = new WirelessShareTpwdCreateRequest();
            WirelessShareTpwdCreateRequest.GenPwdIsvParamDtoDomain obj1 = new WirelessShareTpwdCreateRequest.GenPwdIsvParamDtoDomain();

            if (ext != null)
            {
                obj1.Ext = JsonConvert.SerializeObject(ext);
            }
            obj1.Logo = logo;
            obj1.Url = url;
            obj1.Text = title;
            obj1.UserId = userID;
            req.TpwdParam_ = obj1;
            WirelessShareTpwdCreateResponse rsp = _client.Execute(req);
            return rsp.Model;
        }

        public TbkDgItemCouponGetResponse GetCoupon(long userID, IEnumerable<string> cats, string filter, int pageSize = 50, int page = 1)
        {

            TbkDgItemCouponGetRequest req = new TbkDgItemCouponGetRequest();

            req.AdzoneId = userID;
            req.Platform = 1L;
            //req.Cat = string.Join(",", cats);
            req.PageSize = pageSize;
            //req.Q = filter;
            req.PageNo = page;
            return _client.Execute(req);
        }




        public TbkUatmFavoritesGetResponse GetFavorites(int pageSize = 50, int page = 1, int type = -1)
        {
            TbkUatmFavoritesGetRequest req = new TbkUatmFavoritesGetRequest();
            req.PageNo = page;
            req.PageSize = pageSize;
            req.Fields = "favorites_title,favorites_id,type";
            req.Type = type;
            return _client.Execute(req);


        }

        public TbkUatmFavoritesItemGetResponse GetFavoriteItems(long adzoneId, long favoritesId, int pageSize = 50, int page = 1)
        {
            string[] fields = new string[] {
                "num_iid",
                "title",
                "pict_url",
                "small_images",
                "zk_final_price",
                "user_type",
                "nick",
                "volume",
                "tk_rate",
                "shop_title",
                "status",
                "category",
                "coupon_click_url",
                "coupon_end_time",
                "coupon_info",
                "coupon_start_time",
                "coupon_total_count",
                "coupon_remain_count"
            };
            TbkUatmFavoritesItemGetRequest req = new TbkUatmFavoritesItemGetRequest();
            req.Platform = 1L;
            req.PageSize = pageSize;
            req.AdzoneId = adzoneId;
            req.FavoritesId = favoritesId;
            req.PageNo = page;
            req.Fields = string.Join(",", fields);
            return _client.Execute(req);


        }

        public TbkUatmEventGetResponse GetEvent(int pageSize = 50, int page = 1)
        {
            TbkUatmEventGetRequest req = new TbkUatmEventGetRequest();
            req.PageNo = page;
            req.PageSize = pageSize;
            req.Fields = "event_id,event_title,start_time,end_time";
            return _client.Execute(req);
        }

        public TbkUatmEventItemGetResponse GetEventItems(long adzoneId, long eventID, int pageSize = 50, int page = 1)
        {
            TbkUatmEventItemGetRequest req = new TbkUatmEventItemGetRequest();
            req.Platform = 1L;
            req.PageSize = pageSize;
            req.AdzoneId = adzoneId;
            req.EventId = eventID;
            req.PageNo = page;
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick,shop_title,zk_final_price_wap,event_start_time,event_end_time,tk_rate,type,status,category";
            return _client.Execute(req);
        }

        /// <summary>
        /// 计算券后价
        /// </summary>
        /// <param name="oprice">原价</param>
        /// <param name="val">面额</param>
        /// <returns></returns>
        public static decimal GetAfterCouponPrice(decimal oprice, string val)
        {
            if (val.Contains("满"))
            {
                decimal pp = Convert.ToDecimal(val.Remove(0, 1).Remove(val.IndexOf("元") - 1));
                if (oprice < pp)
                {
                    throw new Exception("未满足价格条件");
                }
                string strSale = val.Remove(0, val.IndexOf("减") + 1).Replace("元", "");
                return oprice - Convert.ToDecimal(strSale);
            }
            else if (val.Contains("无条件券"))
            {
                string strSale = val.Replace("元无条件券", "");
                return oprice - Convert.ToDecimal(strSale);
            }
            else
            {
                throw new Exception("无法解析该优惠券");
            }
        }


        public static void Import(string userID, string path)
        {
            List<Models.Coupon> models = new List<Models.Coupon>();
            try
            {
                var dtable = new ExcelHelper(path).ExcelToDataTable(null, true);

                foreach (System.Data.DataRow item in dtable.Rows)
                {
                    var index = dtable.Rows.IndexOf(item);
                    try
                    {
                        Uri myUri = new Uri(item["优惠券链接"].ToString());
                        string activityId = HttpUtility.ParseQueryString(myUri.Query).Get("activityId");
                        var model = new Models.Coupon
                        {
                            EndDateTime = Convert.ToDateTime(item["优惠券结束时间"]).AddDays(1).AddSeconds(-1),
                            ProductID = item["商品id"].ToString(),
                            TypeID = Bll.Coupons.CheckType(item["商品一级类目"].ToString()),
                            Image = item["商品主图"].ToString(),
                            Link = item["商品优惠券推广链接"].ToString(),
                            Name = item["商品名称"].ToString(),
                            OriginalPrice = Convert.ToDecimal(item["商品价格(单位：元)"]),
                            ProductType = item["商品一级类目"].ToString(),
                            ShopName = item["店铺名称"].ToString(),
                            StartDateTime = Convert.ToDateTime(item["优惠券开始时间"].ToString()),
                            Subtitle = null,
                            Value = item["优惠券面额"].ToString(),
                            Platform = item["平台类型"].ToString() == "淘宝" ? Enums.CouponPlatform.TaoBao : Enums.CouponPlatform.TMall,
                            Sales = Convert.ToInt32(item["商品月销量"]),
                            Commission = Convert.ToDecimal(item["佣金"]),
                            CommissionRate = Convert.ToDecimal(item["收入比率(%)"]),
                            Left = Convert.ToInt32(item["优惠券剩余量"]),
                            Total = Convert.ToInt32(item["优惠券总量"]),
                            UserID = userID,
                            PCouponID = activityId

                        };
                        try
                        {
                            model.Price = Taobao.GetAfterCouponPrice(model.OriginalPrice, model.Value);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        if (model.EndDateTime < DateTime.Now || model.OriginalPrice <= model.Price)
                        {
                            continue;
                        }
                        if (model.Price < 0)
                        {
                            continue;
                        }
                        models.Add(model);

                    }
                    catch (Exception ex)//有异常的数据忽略
                    {

                    }
                }
                //分段添加到数据库
                Bll.Coupons.DbAdd(models);

            }
            catch (Exception ex)
            {
                //Comm.WriteLog("excel", $"{ex.Message}", Enums.DebugLogLevel.Error);
            }
        }
    }
}
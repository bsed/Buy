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
    }
}
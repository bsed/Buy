using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace Buy.MoGuJie
{
    public static class Config
    {
        public const string AppKey = "100561";

        public const string AppSecret = "A64A5E45A245B4366F0DC56E2CD53916";

    }

    public class Method
    {

        private Token _token;

        public Method()
        {


        }

        public void GetAccessToken(string code, string url, string state)
        {
            var p = new Dictionary<string, string>();
            p.Add("app_key", Config.AppKey);
            p.Add("app_secret", Config.AppSecret);
            p.Add("redirect_uri", url);
            p.Add("grant_type", "authorization_code");
            p.Add("code", code);
            p.Add("state", state);
            var tokenUrl = "https://oauth.mogujie.com/token";
            var api = new Api.BaseApi(tokenUrl + p.ToParam("?"), "POST");
            var j = api.CreateRequestReturnJson();

            Token = new Token(j);
        }

        public Token Token
        {
            get
            {
                if (_token == null)
                {
                    string val = HttpContext.Current.Request.Cookies["MGJToken"]?.Value;
                    if (val != null)
                    {
                        _token = JsonConvert.DeserializeObject<Token>(val);
                    }
                }

                return _token;
            }
            set
            {
                _token = value;
                HttpContext.Current.Response.Cookies["MGJToken"].Value = JsonConvert.SerializeObject(_token);
                HttpContext.Current.Response.Cookies["MGJToken"].Expires = DateTime.Now.AddDays(1);
            }
        }

        public void RefeashToken()
        {

            var p = new Dictionary<string, string>();
            p.Add("app_key", Config.AppKey);
            p.Add("app_secret", Config.AppSecret);
            p.Add("grant_type", "refresh_token");
            p.Add("refresh_token", Token.RefreshToken);
            var tokenUrl = "https://oauth.mogujie.com/token";
            var api = new Api.BaseApi(tokenUrl + p.ToParam("?"), "POST");
            var j = api.CreateRequestReturnJson();
            if (string.IsNullOrWhiteSpace(j["errorMsg"]?.Value<string>()))
            {
                throw new Exception("蘑菇街授权过期");
            }
            Token = new Token(j);
        }

        //public GetItemListResult GetItemList(string keyword = null, int pageNo = 1, int pageSize = 50, SortType sortType = SortType.Default, string cid = null)
        //{
        //    //{ "keyword":    字符串 否    搜索词（商品名称关键词或商品描述关键词） 
        //    //"pageNo":    整型 否    页码 
        //    //"pageSize": 整型 否    每页数据个数 
        //    //"sortType":    整型 是    0:默认排序，11:佣金升序，12:佣金降序，21:价格升序，22：价格降序；销量升序，32:销量降序，41:优惠券升序，42:优惠券降序 
        //    //"tag":    字符串 否    商品标签，待官方确认要素值 
        //    //"cid":    整型 否    类目id }

        //    var p = new Dictionary<string, string>();
        //    var q = new
        //    {
        //        keyword = keyword,
        //        pageNo = pageNo,
        //        pageSize = pageSize,
        //        //sortType = (int)sortType,
        //        cid = cid,
        //        hasCoupon = true
        //    };

        //    p.Add("promInfoQuery", JsonConvert.SerializeObject(q));
        //    p.Add("userId", Token.UserID);
        //    var url = GetUrl("xiaodian.cpsdata.promitem.get", p);
        //    var result = new Api.BaseApi(url, "POST", p).CreateRequestReturnJson();
        //    var model = new GetItemListResult();
           
        //    model.Total = result["result"]["data"]["total"].Value<int>();
        //    model.TotalPage = (model.Total / 50) + (model.Total % 50 > 0 ? 1 : 0);
        //    model.Page = pageNo;
        //    foreach (JObject item in result["result"]["data"]["items"])
        //    {
        //        try
        //        {
        //            var obj = new
        //            {
        //                ItemID = item["itemId"].Value<string>(),
        //                DayLeft = item["dayLeft"].Value<int>(),
        //                PictUrl = item["pictUrl"].Value<string>(),
        //                Title = item["title"].Value<string>(),
        //                ShopTitle = item["shopTitle"].Value<string>(),
        //                ExtendDesc = item["extendDesc"].Value<string>(),
        //                AfterCouponPrice = item["afterCouponPrice"].Value<decimal>(),
        //                CouponStartFee = item["couponStartFee"].Value<decimal>(),
        //                CouponInfo = item["couponInfo"].Value<string>(),
        //                Promid = item["promid"].Value<string>(),
        //                BIZ30day = item["biz30day"].Value<int>(),
        //                CommissionRate = Convert.ToDecimal(item["commissionRate"].Value<string>().Replace("%", "")),
        //                CouponLeftCount = item["couponLeftCount"].Value<int>(),
        //                CouponTotalCount = item["couponTotalCount"].Value<int>()
        //            };
        //            if (!string.IsNullOrWhiteSpace(obj.CouponInfo))
        //            {
        //                var tm = new Models.Coupon
        //                {
        //                    CreateDateTime = DateTime.Now,
        //                    EndDateTime = DateTime.Now.Date.AddDays(obj.DayLeft + 1).AddSeconds(-1),
        //                    StartDateTime = DateTime.Now.Date,
        //                    ProductID = obj.ItemID,
        //                    Image = obj.PictUrl,
        //                    Link = $"http://union.mogujie.com/jump?userid={Token.UserID}&itemid={obj.ItemID}&promid={obj.Promid}",
        //                    Name = obj.Title,
        //                    OriginalPrice = obj.AfterCouponPrice + obj.CouponStartFee,
        //                    Platform = Enums.CouponPlatform.MoGuJie,
        //                    Price = obj.AfterCouponPrice,
        //                    ShopName = obj.ShopTitle,
        //                    Subtitle = obj.ExtendDesc,
        //                    Value = obj.CouponInfo,
        //                    Sales = obj.BIZ30day,
        //                    CommissionRate = obj.CommissionRate,
        //                    Commission = Math.Round(obj.AfterCouponPrice * obj.CommissionRate / 100, 2),
        //                    Total = obj.CouponTotalCount,
        //                    Left = obj.CouponLeftCount
        //                };
        //                model.Items.Add(tm);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            continue;
        //        }

        //    }
        //    return model;

        //}

        public string GetUrl(string method, Dictionary<string, string> p)
        {
            string url = "https://openapi.mogujie.com/invoke";
            p.Add("app_key", Config.AppKey);
            p.Add("method", method);
            p.Add("access_token", Token.AccessToken);
            p.Add("format", "json");
            p.Add("timestamp", ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString());

            System.Text.StringBuilder strP = new System.Text.StringBuilder(Config.AppSecret);
            foreach (var item in p.OrderBy(s => s.Key))
            {
                strP.Append($"{item.Key}{item.Value}");
            }
            strP.Append(Config.AppSecret);
            p.Add("sign", Comm.GetMd5Hash(strP.ToString()).ToUpper());
            return url + p.ToParam("?");
        }

        public List<Cid> GetAllCategory(string pid = null)
        {
            var cids = new List<Cid>();
            var p = new Dictionary<string, string>();
            p.Add("parentID", pid);
            p.Add("userId", Token.UserID);
            var url = GetUrl("xiaodian.item.getAllCategory", p);
            var result = new Api.BaseApi(url, "POST", p).CreateRequestReturnJson();
            var model = result["result"]["data"].Values<JObject>()
                .Select(s => new Cid(s))
                .ToList();
            return model;
        }

        private static List<Cid> _allCategory;

        /// <summary>
        /// 解密后的分类
        /// </summary>
        public static List<Cid> AllCategory
        {
            get
            {
                if (Method._allCategory == null)
                {
                    string path = HttpContext.Current.Request.MapPath("~/App_Data/mgjCids.json");
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                    {
                        string json = sr.ReadToEnd();
                        _allCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cid>>(json);
                    }
                }

                return _allCategory;
            }
        }
        public string DecryptID(string id)
        {
            var p = new Dictionary<string, string>();
            p.Add("id", id);
            var url = GetUrl("xiaodian.common.decryptID", p);
            var result = new Api.BaseApi(url, "POST", p).CreateRequestReturnJson();
            return result["result"]["data"].Value<string>();
        }
    }



    public class Token
    {
        public Token() { }

        public Token(JObject o)
        {
            AccessToken = o["access_token"].Value<string>();
            RefreshToken = o["refresh_token"].Value<string>();
            UserID = o["userId"].Value<string>();

        }

        public string UserID { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

    }

    public enum SortType
    {
        [Display(Name = "默认")]
        Default = 0,
        [Display(Name = "佣金升序")]
        CommissionAsc = 11,
        [Display(Name = "佣金降序")]
        CommissionDesc = 12,
        [Display(Name = "价格升序")]
        PriceAsc = 21,
        [Display(Name = "价格降序")]
        PriceDesc = 22,
        [Display(Name = "销量升序")]
        SalesAsc = 31,
        [Display(Name = "销量降序")]
        SalesDesc = 32,
        [Display(Name = "优惠券升序")]
        CouponPriceAsc = 41,
        [Display(Name = "优惠券降序序")]
        CouponPriceDesc = 42,
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetItemListResult
    {
        public int Page { get; set; }

        public int Total { get; set; }

        public int TotalPage { get; set; }

        public List<Models.Coupon> Items = new List<Models.Coupon>();
    }

    /// <summary>
    /// 类别
    /// </summary>
    public class Cid
    {
        public Cid() { }

        public Cid(JObject o)
        {
            ID = o["categoryId"].Value<string>();
            IsLeaf = o["isLeaf"].Value<bool>();
            Name = o["name"].Value<string>();
            ParentId = o["parentId"].Value<string>();
        }


        public string ID { get; set; }

        public bool IsLeaf { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public int? TypeID { get; set; }
    }
}
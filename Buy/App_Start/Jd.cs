using Jd.Api;
using Jd.Api.Request;
using Jd.Api.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Jd
{
    public static class Config
    {
        public const string AppKey = "01033F980DC500993B993CFBB8179B33";

        public const string AppSecret = "3044d944140c404b93c55362ec87b873";

        public const long UnionID = 1000282367;

        public const string SiteID = "874160964";

    }

    public class Method
    {
        private IJdClient _client;

        private Token _token;

        public Token Token
        {
            get
            {
                if (_token == null)
                {
                    string val = HttpContext.Current.Request.Cookies["JDToken"]?.Value;
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
                HttpContext.Current.Response.Cookies["JDToken"].Value = JsonConvert.SerializeObject(_token);
                HttpContext.Current.Response.Cookies["JDToken"].Expires = DateTime.Now.AddDays(1);
            }
        }

        public Method()
        {
            _client = new DefaultJdClient("https://api.jd.com/routerjson", Config.AppKey, Config.AppSecret);
        }

        public void GetAccessToken(string code, string url, string state)
        {
            var p = new Dictionary<string, string>();
            p.Add("grant_type", "authorization_code");
            p.Add("client_id", Config.AppKey);
            p.Add("client_secret", Config.AppSecret);
            p.Add("scope", "read");
            p.Add("redirect_uri", url);
            p.Add("code", code);
            p.Add("state", state);
            var tokenUrl = "https://oauth.jd.com/oauth/token";
            var api = new Api.BaseApi(tokenUrl + p.ToParam("?"), "POST");
            var j = api.CreateRequestReturnJson();
            Token = new Token(j);
        }

        public void RefeashToken()
        {
            var p = new Dictionary<string, string>();
            p.Add("client_id", Config.AppKey);
            p.Add("client_secret", Config.AppSecret);
            p.Add("grant_type", "refresh_token");
            p.Add("refresh_token", Token.RefreshToken);
            var tokenUrl = "https://oauth.jd.com/oauth/token";
            var api = new Api.BaseApi(tokenUrl + p.ToParam("?"), "POST");
            var j = api.CreateRequestReturnJson();
            if (string.IsNullOrWhiteSpace(j["code"]?.Value<string>()))
            {
                throw new Exception("京东授权过期");
            }
            Token = new Token(j);
        }


        public void GetGoodsInfo(long[] ids)
        {

            ServicePromotionGoodsInfoRequest req = new ServicePromotionGoodsInfoRequest();

            req.skuIds = string.Join(",", ids);

            ServicePromotionGoodsInfoResponse response = _client.Execute(req, Token.AccessToken, DateTime.Now.ToLocalTime());
            var json = JsonConvert.DeserializeObject<JObject>(response.Body);

            var strData = json["jingdong_service_promotion_goodsInfo_responce"]["getpromotioninfo_result"].Value<string>();
            json = JsonConvert.DeserializeObject<JObject>(strData);
            var result = json["result"].Values<JObject>().ToList();

        }

        public string QueryCouponGoods()
        {

            UnionThemeGoodsServiceQueryCouponGoodsRequest req = new UnionThemeGoodsServiceQueryCouponGoodsRequest();

            req.from = 0; req.pageSize = 100;

            UnionThemeGoodsServiceQueryCouponGoodsResponse response = _client.Execute(req, Token.AccessToken, DateTime.Now.ToLocalTime());
            return response.Body;
        }

        public void GetTest()
        {


            ServicePromotionBatchGetcodeRequest req = new ServicePromotionBatchGetcodeRequest();

            req.id = "1031530549";
            req.url = "http://item.jd.com/1031530549.html";
            req.unionId = Config.UnionID;
            req.channel = "PC";
            req.webId = Config.SiteID;

            ServicePromotionBatchGetcodeResponse response = _client.Execute(req, Token.AccessToken, DateTime.Now.ToLocalTime());
        }


    }

    public class Token
    {
        public Token() { }

        public Token(JObject o)
        {
            AccessToken = o["access_token"].Value<string>();
            RefreshToken = o["refresh_token"].Value<string>();
            UserID = o["uid"].Value<string>();
        }

        public string UserID { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

    }

}
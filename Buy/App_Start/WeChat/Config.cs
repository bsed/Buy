using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Buy.WeChat
{
    public class Config
    {
        /// <summary>
        /// 网页
        /// </summary>
        public const string AppID = "wx2dc3b9081bffcb72";

        /// <summary>
        /// 网页
        /// </summary>
        public const string AppSecret = "2abd36c74172e007066f969897a0aab7";

        /// <summary>
        /// APP
        /// </summary>
        public const string AppID2 = "wxfad3a6c671290f70";

        /// <summary>
        /// APP
        /// </summary>
        public const string AppSecret2 = "37cf7b2810468af175f2997e0b46fb83";

        /// <summary>
        /// 小程序
        /// </summary>
        public const string AppID3 = "wx403e6919cb2ca7eb";

        /// <summary>
        /// 小程序
        /// </summary>
        public const string AppSecret3 = "2c712ad0236b39ba88c1ff3393ec65aa";

        public const string JsapiTimeStamp = "1414587457";

        /// <summary>
        /// AppID对应的公共AccessToken
        /// </summary>
        public static string AccessToken = null;

        /// <summary>
        /// AppID对应的公共RefreshToken
        /// </summary>
        public static string RefreshToken = null;

        public static String JsSign(string url)
        {

            var api1 = new Api.BaseApi($"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={AppID}&secret={AppSecret}", "GET");
            var json = api1.CreateRequestReturnJson();
            var access_token = json["access_token"].Value<string>();
            var api2 = new Api.BaseApi($"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={access_token}&type=jsapi", "GET");
            var josn2 = api2.CreateRequestReturnJson();
            var str = $"jsapi_ticket={josn2["ticket"].Value<string>()}&noncestr=Octopus&timestamp={JsapiTimeStamp}&url={url}";
            byte[] StrRes = Encoding.Default.GetBytes(str);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();

        }
    }
}
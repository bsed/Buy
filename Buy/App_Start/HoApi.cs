using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy;
namespace HoApi
{
    public class CheckResult
    {
        /// <summary>
        /// 没有敏感词
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 敏感词
        /// </summary>
        public string[] Word { get; set; }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public string NewStr { get; set; }

        /// <summary>
        /// 输入的内容
        /// </summary>
        public string InputStr { get; set; }
    }


    public class Apis
    {
        /// <summary>
        /// 敏感词检查
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CheckResult Check(string text)
        {
            var url = $"http://www.hoapi.com/index.php/Home/Api/check";
            var parameter = new Dictionary<string, string>();
            parameter.Add("str", text);
            parameter.Add("token", "74b5fc5039d35d0dac45cff0bf100ab9");
            var api = new Buy.Api.BaseApi(url, "POST", parameter).CreateRequestReturnJson();
            var model = new CheckResult
            {
                Msg = api["msg"].ToString(),
                Status = bool.Parse(api["status"].ToString()),
            };
            if (!bool.Parse(api["status"].ToString()))
            {
                var data = (JObject)JsonConvert.DeserializeObject(api["data"].ToString());
                var error = (JArray)JsonConvert.DeserializeObject(data["error"].ToString());
                var list = new List<string>();
                foreach (var item in error)
                {
                    list.Add(item["word"].ToString());
                }
                model.InputStr = data["input_str"].ToString();
                model.NewStr = data["new_str"].ToString();
                model.Word = list.ToArray();
            }
            return model;
        }
        
    }

    
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Buy.Api
{
    public class BaseApi
    {
        public BaseApi() { }

        public BaseApi(string url, string type)
        {
            Url = url;
            Type = type.ToUpper();
        }


        public BaseApi(string url, string type, Object data)
        {
            Url = url;
            Type = type.ToUpper();
            Data = data;
        }



        public string Url { get; set; }

        public string Type { get; set; }

        public object Data { get; set; }

        public Dictionary<string, System.IO.Stream> Files { get; set; } = new Dictionary<string, Stream>();

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">类别</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public virtual Stream CreateRequest()
        {

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(JsonConvert.SerializeObject(Data)));
                foreach (var item in Files)
                {
                    HttpContent content = new StreamContent(item.Value);
                    formData.Add(content, item.Key, item.Key);
                }

                var response = client.PostAsync(Url, formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content.ReadAsStreamAsync().Result;
            }
        }

        /// <summary>
        /// 创建请求返回Jobject
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">类别</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public virtual JObject CreateRequestReturnJson()
        {
            var steam = CreateRequest();
            string txtData = "";
            if (steam == null)
            {
                return null;

            }
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<JObject>(txtData);
        }


        public virtual string CreateRequestReturnString()
        {
            var steam = CreateRequest();
            string txtData = "";
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return txtData;
        }
    }
}
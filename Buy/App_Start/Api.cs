using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();

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
                StringContent temp = null;
                if (Data != null)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(Data), System.Text.Encoding.UTF8, "application/json");
                    temp = content;
                    formData.Add(content);

                }

                foreach (var item in Files)
                {
                    var stream = File.OpenRead(item.Value);
                    HttpContent content = new StreamContent(stream);

                    content.Headers.Add("Content-Type", "application/octet-stream");
                    content.Headers.Add("Content-Disposition", $"form-data; name=\"{item.Key}\"; filename=\"{new FileInfo(item.Value).Name}\"");
                    formData.Add(content, item.Key);
                }
                //var response = client.PostAsync(Url, formData).Result;
                var response = client.PostAsync(Url, temp).Result;
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
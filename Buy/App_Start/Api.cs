using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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


        public BaseApi(string url, string type, Dictionary<string, string> parameter)
        {
            Url = url;
            Type = type.ToUpper();
            Parameter = parameter;
        }

        public BaseApi(string url, string type, Object json)
        {
            Url = url;
            Type = type.ToUpper();
            Json = json;
        }

        public string Url { get; set; }

        public string Type { get; set; }

        public Dictionary<string, string> Parameter { get; set; } = new Dictionary<string, string>();

        public object Json { get; set; }

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">类别</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public virtual HttpWebResponse CreateRequest()
        {

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Url);
            request.Method = Type;
            request.ContentType = "application/x-www-form-urlencoded";
            System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
            int i = 0;

            if (Parameter != null && Parameter.Keys.Count > 0)
            {
                foreach (var key in Parameter.Keys)
                {
                    if (i == 0)
                    {
                        sbPostData.AppendFormat("{0}={1}", key, Parameter[key]);
                    }
                    else
                    {
                        sbPostData.AppendFormat("&{0}={1}", key, Parameter[key]);
                    }
                    i++;
                }
                var data = System.Text.Encoding.UTF8.GetBytes(sbPostData.ToString());
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            else if (Json != null)
            {
                string temp;
                if (Json.GetType() == typeof(string))
                {
                    temp = (string)Json;
                }
                else
                {
                    temp = JsonConvert.SerializeObject(Json);
                }
                byte[] data = System.Text.Encoding.UTF8.GetBytes(temp);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return (System.Net.HttpWebResponse)request.GetResponse();
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
            var response = CreateRequest();
            var steam = response.GetResponseStream();
            string txtData = "";
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<JObject>(txtData);
        }


        public virtual string CreateRequestReturnString()
        {
            var response = CreateRequest();
            var steam = response.GetResponseStream();
            string txtData = "";
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return txtData;
        }
    }
}
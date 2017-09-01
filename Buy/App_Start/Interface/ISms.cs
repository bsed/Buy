using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Buy
{
    public interface ISms
    {
        SmsResult Send(string phone, string code);
    }
    public class RLSms : ISms
    {
        public SmsResult Send(string phone, string code)
        {
            SmsResult result = new SmsResult() { IsSuccess = false, Message = "" };

            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            //ip格式如下，不带https://
            bool isInit = api.init("app.cloopen.com", "8883");  //服务器地址,服务器端口
            api.setAccount("8a48b5514c2fd22f014c459195700d9f", "86fa4f3d18e44600bbbd88707f3c02c2");   //主帐号,主帐号令牌
            api.setAppId("8aaf07085aabcbbd015ab6581f35048a");  //应用ID
            try
            {
                if (isInit)
                {
                    String[] count = { code, "10" };
                    Dictionary<string, object> retData = api.SendTemplateSMS(phone, "159230", count);
                    result = getDictionaryData(retData, result);
                }
                else
                {
                    result.Message = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                result.Message = exc.Message;
            }
            return result;
        }

        private SmsResult getDictionaryData(Dictionary<string, object> data, SmsResult result)
        {
            string ret = string.Empty;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Key.ToString() == "statusCode")
                {
                    if (item.Value != null && item.Value.ToString() == "000000")
                    {
                        result.IsSuccess = true;
                    }
                }
                if (item.Key.ToString() == "statusMsg")
                {
                    result.Message = (item.Value == null ? "null" : item.Value.ToString());
                }

                //if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                //{
                //    ret += item.Key.ToString() + "={";
                //    ret += getDictionaryData((Dictionary<string, object>)item.Value,result);
                //    ret += "};";
                //}
                //else
                //{
                //    if (item.Value != null && item.Value.ToString() == "000000" )
                //    {
                //        result.IsSuccess = true;
                //    }
                //    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                //}
            }
            //result.Message = ret;
            return result;
        }

    }

    public class SmsResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
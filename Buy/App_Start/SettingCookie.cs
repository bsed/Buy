using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Buy
{
    public class SettingCookie
    {
        public SettingCookie()
        {
            var setting = HttpContext.Current.Request.Cookies["SettingCookie"]?.Value;
            if (setting != null)
            {
                try
                {
                    setting = HttpContext.Current.Server.UrlDecode(setting);
                    var item = JsonConvert.DeserializeObject<JObject>(setting);
                    TicketIndexSort = item["TicketIndexSort"].Value<string>();
                    TicketIndexState = item["TicketIndexState"].Value<string>();
                }
                catch (Exception)
                {

                }

            }
        }

        public string TicketIndexSort { get; set; } = "0";

        public string TicketIndexState { get; set; } = "0,1";

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
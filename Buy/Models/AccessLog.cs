using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class AccessLog
    {
        public int ID { get; set; }

        public string IP { get; set; }

        public Enums.AccessLogType Type { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string Data { get; set; }

        public string UserID { get; set; }

        public string Source { get; set; }
    }
}
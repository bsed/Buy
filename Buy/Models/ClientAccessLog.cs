using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class ClientAccessLog
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public string Code { get; set; }

        public DateTime LoginDateTime { get; set; }

        public string IP { get; set; }
    }
}
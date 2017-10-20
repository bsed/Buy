using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class RegistrationCodeLog
    {
        public int ID { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string From { get; set; }

        public int Count { get; set; }

        public string UserID { get; set; }

        public string Remark { get; set; }
    }
}
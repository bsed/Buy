using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class UserRemark
    {
        public int ID { get; set; }

        public string UserID { get; set; }


        public string RemarkUser { get; set; }


        public string Remark { get; set; }
    }
}
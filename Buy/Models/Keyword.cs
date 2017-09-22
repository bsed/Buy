using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class Keyword
    {
        public int ID { get; set; }

        public string Word { get; set; }

        public int CouponNameCount { get; set; }

        public int SearchCount { get; set; }
    }
}
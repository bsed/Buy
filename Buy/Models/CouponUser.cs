using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class CouponUser
    {
        public int ID { get; set; }

        public int CouponID { get; set; }

        public Coupon Coupon { get; set; }

        public string UserID { get; set; }

        public string Link { get; set; }

        public Enums.CouponPlatform Platform { get; set; }

        public string PCouponID { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class Favorite
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int CouponID { get; set; }

        public Enums.FavoriteType Type { get; set; }
    }

    public class FavoriteLocalCouponList
    {
        public Favorite Favorite { get; set; }

        public LocalCouponList LocalCoupon { get; set; }
    }
}
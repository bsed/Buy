using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class LocalCoupon
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int ShopID { get; set; }

        public DateTime CreateDateTime { get; set; }

        public decimal Price { get; set; }

        public string Remark { get; set; }

        public string Image { get; set; }

        public DateTime EndDateTime { get; set; }

        public virtual Shop Shop { get; set; }

    }
}
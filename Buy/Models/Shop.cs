using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class Shop
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Logo { get; set; }

        public virtual int Sort { get; set; }

        public virtual string Code { get; set; }


        public virtual List<LocalCoupon> Coupons { get; set; }

        public virtual List<ShopMember> Members { get; set; }
    }
}
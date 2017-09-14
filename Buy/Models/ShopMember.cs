using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class ShopMember
    {
        public virtual int ID { get; set; }

        public virtual string UserID { get; set; }

        public virtual int ShopID { get; set; }

        public virtual Shop Shop { get; set; }
    }
}
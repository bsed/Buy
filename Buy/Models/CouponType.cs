using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class CouponType
    {
        public int ID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "关键字")]
        public string Keyword { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "父类别")]
        public int ParentID { get; set; }

        public virtual List<Coupon> Tickets { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class Shop
    {
        public virtual int ID { get; set; }

        [Display(Name = "名称")]
        public virtual string Name { get; set; }

        [Display(Name = "Logo")]
        public virtual string Logo { get; set; }

        [Display(Name = "排序")]
        public virtual int Sort { get; set; }

        [Display(Name = "编号")]
        public virtual string Code { get; set; }


        public virtual List<LocalCoupon> Coupons { get; set; }

        public virtual List<ShopMember> Members { get; set; }
    }

    [NotMapped]
    public class ShopManageViewModel : Shop
    {
        public FileUpload FileUpload { get; set; }
    }
}
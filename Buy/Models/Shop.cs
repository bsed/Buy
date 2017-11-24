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

        [Display(Name = "简介")]
        public virtual string Remark { get; set; }

        [Display(Name = "图片介绍")]
        public virtual string Images { get; set; }

        [Display(Name = "联系电话")]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = "纬度")]
        public double? Lat { get; set; }

        [Display(Name = "经度")]
        public double? Lng { get; set; }

        [Display(Name = "详细地址")]
        public string Address { get; set; }

        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "地区")]
        public string District { get; set; }

        [Display(Name = "商圈")]
        public string TradingArea { get; set; }
    }

    [NotMapped]
    public class ShopManageViewModel : Shop
    {
        public FileUpload FileUpload { get; set; }
    }
}
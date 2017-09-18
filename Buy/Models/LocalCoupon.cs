using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Models
{
    public class LocalCoupon
    {
        public int ID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "商家")]
        public int ShopID { get; set; }

        [Display(Name = "开始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "价格")]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "图片")]
        public string Image { get; set; }

        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        public virtual Shop Shop { get; set; }

        [Display(Name = "佣金")]
        public virtual decimal Commission { get; set; }

    }

    [NotMapped]
    public class LocalCouponViewModel : LocalCoupon
    {
        public FileUpload FileUpload { get; set; }
    }

}
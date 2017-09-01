using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    public class FoodCoupon
    {
        public int ID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "类别")]
        public virtual FoodCouponType Type { get; set; }

        [Display(Name = "类别")]
        public int TypeID { get; set; }

        [Display(Name = "列表图")]
        public string Image { get; set; }

        [Display(Name = "明细图")]
        public string DetailImage { get; set; }

        [Display(Name = "券后价")]
        public decimal Price { get; set; }

        [Display(Name = "开始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "备注")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        [NotMapped]
        [Display(Name = "列表图")]
        public FileUpload FileUploadImage { get; set; }

        [NotMapped]
        [Display(Name = "明细图")]
        public FileUpload FileUploadDetailImage { get; set; }
    }
}
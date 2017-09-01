using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    public class FoodCouponType
    {
        public int ID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "Logo")]
        public string Image { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "启动")]
        public bool Enable { get; set; }

        [NotMapped]
        [Display(Name = "Logo")]
        public FileUpload FileUpload { get; set; }

        public virtual List<FoodCoupon> Tickets { get; set; }
    }
}
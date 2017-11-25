using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Buy.Models
{
    public class LocalCouponKind
    {
        public int ID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "图片")]
        public string Image { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }
        
    }
}
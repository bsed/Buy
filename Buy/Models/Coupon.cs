using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    public class Coupon
    {
        public int ID { get; set; }

        [Display(Name = "分类")]
        public virtual int? TypeID { get; set; }
        [Display(Name = "分类")]
        public virtual CouponType Type { get; set; }

        [Display(Name = "类目")]
        public virtual string ProductType { get; set; }

        [Display(Name = "商品ID")]
        public virtual string ProductID { get; set; }

        [Display(Name = "平台")]
        public Enums.CouponPlatform Platform { get; set; }

        [Display(Name = "店铺名称")]
        public virtual string ShopName { get; set; }

        [Display(Name = "名称")]
        public virtual string Name { get; set; }

        [Display(Name = "图片")]
        public virtual string Image { get; set; }



        [Display(Name = "副标题")]
        public virtual string Subtitle { get; set; }


        [Display(Name = "价格")]
        [DataType(DataType.Currency)]
        public virtual decimal Price { get; set; }

        [Display(Name = "原价")]
        [DataType(DataType.Currency)]
        public virtual decimal OriginalPrice { get; set; }


        [Display(Name = "面值")]
        public virtual string Value { get; set; }


        [Display(Name = "开始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime StartDateTime { get; set; }

        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime EndDateTime { get; set; }

        [Display(Name = "特殊参数")]
        public virtual string DataJson { get; set; }

        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public virtual DateTime CreateDateTime { get; set; }

        [Display(Name = "销量")]
        public virtual int Sales { get; set; }

        [Display(Name = "地址列表")]
        public string UrlLisr { get; set; }

        [Display(Name = "佣金")]
        [DataType(DataType.Currency)]
        public decimal Commission { get; set; }

        [Display(Name = "佣金比例")]
        [DisplayFormat(DataFormatString = "{0}%")]
        public decimal CommissionRate { get; set; }

        [Display(Name = "剩余")]
        public int Left { get; set; }

        [Display(Name = "总数")]
        public int Total { get; set; }

        [Display(Name = "用户ID")]
        public string UserID { get; set; }

        [Display(Name = "连接")]
        public string Link { get; set; }

        [Display(Name = "优惠券ID")]
        public string PCouponID { get; set; }

        [Display(Name = "优惠券链接不带佣金")]
        public string PLink { get; set; }

        public List<CouponUser> Users { get; set; }
    }


    public class CouponNotType
    {
        public CouponNotType() { }

        [Display(Name = "类型")]
        public string Type { get; set; }

        [Display(Name = "数量")]
        public int Count { get; set; }
    }
}
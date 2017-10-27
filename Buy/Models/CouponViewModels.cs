using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    [NotMapped]
    public class CouponQuery : CouponUserViewModel
    {


        public decimal Discount { get; set; }

        public decimal DiscountRate { get; set; }

        public bool IsFavorite { get; set; }
    }

    public class CouponSearchViewModel
    {
        public string Filter { get; set; }


        public Enums.CouponSort Sort { get; set; }

        public Enums.CouponPlatform? Platform { get; set; }

        public int? TypeID { get; set; }

        public List<CouponType> Types { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public List<string> SearchTop { get; set; }

        public int? FootTypeID { get; set; }

    }

    public class CouponValue
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class CouponDetailViewModel
    {
        public Coupon Coupon { get; set; }

        public List<Coupon> CouponList { get; set; }
    }


    public class CouponTypeTreeNode
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int ParentID { get; set; }

        public string Image { get; set; }

        public List<CouponTypeTreeNode> Childs { get; set; }

        //以下添加测试的的
        public string TestID { get; set; }

        public bool IsLeaf { get; set; }

        public string ParentId { get; set; }

        public int? TypeID { get; set; }

        public int Count { get; set; }
        //以上添加测试的的
    }

    [NotMapped]
    public class CouponUserViewModel : Coupon
    {
        public string UserID { get; set; }

        public string Link { get; set; }

        public Coupon ToCoupon()
        {
            return new Coupon
            {
                Commission = Commission,
                CommissionRate = CommissionRate,
                CreateDateTime = CreateDateTime,
                DataJson = DataJson,
                EndDateTime = EndDateTime,
                Image = Image,
                Left = Left,
                ID = ID,
                Name = Name,
                OriginalPrice = OriginalPrice,
                PCouponID = PCouponID,
                Platform = Platform,
                PLink = PLink,
                Price = Price,
                ProductID = ProductID,
                ProductType = ProductType,
                Sales = Sales,
                ShopName = ShopName,
                StartDateTime = StartDateTime,
                Subtitle = Subtitle,
                Total = Total,
                TypeID = TypeID,
                UrlLisr = UrlLisr,
                Value = Value

            };
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    [NotMapped]
    public class CouponQuery : Coupon
    {
        public decimal Discount { get; set; }

        public decimal DiscountRate { get; set; }
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

    public class ThirdPartyTicketValue
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class CouponDetailViewModel
    {
        public Coupon ThirdPartyTicket { get; set; }

        public List<Coupon> ThirdPartyTicketList { get; set; }
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
}
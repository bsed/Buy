using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;
using System.ComponentModel.DataAnnotations;
namespace Buy.Models
{
    public class ChildProxyApply
    {
        public int ID { get; set; }

        [Display(Name = "代理")]
        public string ProxyID { get; set; }

        [Display(Name = "申请人")]
        public string UserID { get; set; }

        [Display(Name = "状态")]
        public Enums.ChildProxyApplyState State { get; set; }

        [Display(Name = "申请时间")]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "审核时间")]
        public DateTime? CheckDateTime { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
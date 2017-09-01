using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class RegistrationCode
    {
        public int ID { get; set; }

        [Display(Name ="注册码")]
        public string Code { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get;set; }

        [Display(Name = "创建用户")]
        public string CreateUser { get; set; }

        [Display(Name = "拥有用户")]
        public string OwnUser { get; set; }

        [Display(Name = "使用时间")]
        public DateTime? UseTime { get; set; }

        [Display(Name = "使用用户")]
        public string UseUser { get; set; }

    }
}
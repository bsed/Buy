using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class UserManage
    {
        [Display(Name ="ID")]
        public string Id { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "注册码数")]
        public int Count { get; set; }

        [Display(Name = "注册码未使用数")]
        public int UnUseCount { get; set; }

        [Display(Name = "注册码已使用数")]
        public int UseCount { get; set; }

        [Display(Name = "注册时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDateTime { get; set; }
    }
}
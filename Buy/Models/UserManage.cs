using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class UserManage
    {
        [Display(Name = "ID")]
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDateTime { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Display(Name = "类别")]
        public Enums.UserType UserType { get; set; }

        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }


        [Display(Name = "微信号")]
        public string WeChatCode { get; set; }
    }
}
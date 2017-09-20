﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get;set; }

        [Display(Name = "创建用户")]
        public string CreateUser { get; set; }

        [Display(Name = "拥有用户")]
        public string OwnUser { get; set; }

        [Display(Name = "使用时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UseTime { get; set; }

        [Display(Name = "使用用户")]
        public string UseUser { get; set; }
    }

    public class RegistrationCodeCreate
    {
        [Display(Name = "拥有用户")]
        [Required(ErrorMessage = "请选择{0}")]
        public string OwnUser { get; set; }

        public ApplicationUser Own { get; set; }

        [Display(Name = "数量")]
        [Range(1,int.MaxValue,ErrorMessage ="{0}不能小于{1}")]
        public int Count { get; set; }
    }

    [NotMapped]
    public class RegistrationCodeViewModel: RegistrationCode
    {
        public ApplicationUser Create { get; set; }

        public ApplicationUser Own { get; set; }

        public ApplicationUser Use { get; set; }
    }
}
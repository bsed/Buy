using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    /// <summary>
    /// 验证码表
    /// </summary>
    public class VerificationCode
    {
        public int ID { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string IP { get; set; }
    }

    public class VerCode
    {
        [Display(Name ="验证码是否正确")]
        public bool IsSuccess { get; set; }
        [Display(Name = "备注")]
        public string  Message { get; set; }
    }
}
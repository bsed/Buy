using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Buy.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        [Display(Name = "手机号")]
        [Required]
        [RegularExpression(Reg.MOBILE, ErrorMessage = "{0} 格式有误")]
        public string Phone { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(Reg.MOBILE, ErrorMessage = "{0} 格式有误")]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string Code { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

    }


    public class UserMangeCreateUserViewModel
    {
        [Required]
        [RegularExpression(Reg.MOBILE, ErrorMessage = "{0} 格式有误")]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [Display(Name = "验证码")]
        public string Code { get; set; }

        [Display(Name = "用户类型")]
        public Enums.UserType UserType { get; set; }

        [Display(Name = "权限分组")]
        public int? RoleGroupID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "上级昵称")]
        public string ParentUserNickName { get; set; }

        [Display(Name = "上级ID")]
        public string ParentUserID { get; set; }

        [Required]
        [Display(Name = "昵称")]

        public string NickName { get; set; }
    }


    public class RegisterChildViewModel
    {
        [Required]
        [RegularExpression(Reg.MOBILE)]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [Display(Name = "验证码")]
        public string Code { get; set; }

        [Display(Name = "用户类型")]
        public Enums.UserType UserType { get; set; }

        [Display(Name = "权限分组")]
        public string Par { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [RegularExpression(Reg.MOBILE)]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "验证码")]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }


    public class UserViewModelForProxy
    {
        public string Id { get; set; }

        public string Avatar { get; set; }

        public string PhoneNumber { get; set; }

        public bool CanAddChild { get; set; }

        public string WeChatCode { get; set; }


        public string UserName { get; set; }

        public string NickName { get; set; }


        public int ThisMonthCount { get; set; }

        public int LastMonthCount { get; set; }

        public int TotalCount { get; set; }
    }
}

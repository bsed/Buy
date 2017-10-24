using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Buy.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(ApplicationUser user)
        {
            SetBaseInfo(user);

        }


        private void SetBaseInfo(ApplicationUser user)
        {
            Id = user.Id;
            AvatarRel = user.Avatar;
            PhoneNumber = user.PhoneNumber;
            NickName = user.NickName;
            UserName = user.UserName;
            IsFrozen = user.LockoutEndDateUtc.HasValue ? (user.LockoutEndDateUtc.Value - DateTime.Now).TotalDays > 365 : false;
            IsActivation = user.IsActive;
            UserType = user.UserType;
            ParentUserID = user.ParentUserID;
            WeChatCode = user.WeChatCode;
        }


        public string Id { get; set; }

        public string AvatarRel { get; set; }

        public string Avatar
        {
            get
            {
                string url = string.IsNullOrWhiteSpace(AvatarRel) ? "~/Content/Images/DefaultAvatar.png" : AvatarRel;
                return Comm.ResizeImage(url, image: null);
            }
        }

        public string PhoneNumber { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public bool IsFrozen { get; set; }


        public bool IsActivation { get; set; }

        public Enums.UserType UserType { get; set; }


        public string ParentUserID { get; set; }

        public string WeChatCode { get; set; }


    }

    public class ChildCreateViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string OwnerID { get; set; }

        [Required]
        public string NickName { get; set; }

        public string Avatar { get; set; }

        public bool CanSend { get; set; }

        public bool CanUse { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string PhoneNumber { get; set; }
    }

    public class ChildEditViewModel
    {
        [Required]
        public string OwnerID { get; set; }

        [Required]
        public string ID { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        public string NickName { get; set; }

        public string Avatar { get; set; }

        public bool CanSend { get; set; }

        public bool CanUse { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string PhoneNumber { get; set; }
    }

    public class UserSetting
    {
        public UserSetting(ApplicationUser u)
        {
            Avatar = new ImageResizer("Avatar", 150, 150, u.Avatar, 150, 150);
            NickName = u.NickName;
            Avatar.AutoInit = false;
            ID = u.Id;
        }

        public string ID { get; set; }

        [Display(Name = "头像")]
        public ImageResizer Avatar { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }
    }


    public class UserManageEditProxyViewModel
    {
        public UserManageEditProxyViewModel()
        {

        }

        public UserManageEditProxyViewModel(ApplicationUser user)
        {

            Id = user.Id;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            NickName = user.NickName;
        }



        public string Id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Display(Name = "收二级代理")]
        public bool TakeChildProxy { get; set; }
        
    }

}
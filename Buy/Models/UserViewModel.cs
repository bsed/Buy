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
            ID = user.Id;
            Avatar = user.Avatar;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            NickName = user.NickName;
            UserName = user.UserName;
            IsFrozen = user.LockoutEndDateUtc.HasValue ? (user.LockoutEndDateUtc.Value - DateTime.Now).TotalDays > 365 : false;
          
        }


        public string ID { get; set; }

        public string Avatar { get; set; }

        public string AvatarFull
        {
            get
            {
                string url = string.IsNullOrWhiteSpace(Avatar) ? "~/Content/Images/DefaultAvatar.png" : Avatar;
                return Comm.ResizeImage(url, image: null);
            }
        }

        public string PhoneNumber { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public string QRCode { get; set; }

        public string Email { get; set; }

      

        public List<string> Roles { get; set; }

        public bool IsFrozen { get; set; }

        public bool IsFollow { get; set; }

        public int Fan { get; set; }

        public int Follow { get; set; }

        public string ShareUrl { get; set; }
        
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
}
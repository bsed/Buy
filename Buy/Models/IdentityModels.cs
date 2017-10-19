using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Buy.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        public string Avatar { get; set; }

        [Display(Name = "昵称")]
        public string NickName { get; set; }

        [Display(Name = "微信号")]
        public string WeChatCode { get; set; }

        public string WeChatID { get; set; }

        public string QRCode { get; set; }

        [Display(Name = "注册时间")]
        public DateTime RegisterDateTime { get; set; }

        public DateTime LastLoginDateTime { get; set; }


        public int? RoleGroupID { get; set; }

        public Enums.UserType UserType { get; set; }

        [Display(Name = "上级")]
        public string ParentUserID { get; set; }

        [Display(Name = "激活状态")]
        public bool IsActive { get; set; }

        [Display(Name = "有效日期")]
        public DateTime? EndDateTime { get; set; }

        public bool IsLocked()
        {
            return LockoutEndDateUtc.HasValue && (LockoutEndDateUtc.Value - DateTime.Now).TotalDays > 365;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public Enums.RoleType Type { get; set; }

        public string Group { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<RoleGroup> RoleGroups { get; set; }

        public virtual DbSet<VerificationCode> VerificationCodes { get; set; }

        public virtual DbSet<SystemSetting> SystemSettings { get; set; }

        public virtual DbSet<UpdateLog> UpdateLogs { get; set; }

        public virtual DbSet<Help> Helps { get; set; }

        public virtual DbSet<Coupon> Coupons { get; set; }

        public virtual DbSet<CouponType> CouponTypes { get; set; }

        public virtual DbSet<FoodCoupon> FoodCoupons { get; set; }

        public virtual DbSet<FoodCouponType> FoodCouponTypes { get; set; }

        public virtual DbSet<RegistrationCode> RegistrationCodes { get; set; }

        public virtual DbSet<ClientAccessLog> ClientAccessLogs { get; set; }

        public virtual DbSet<Shop> Shops { get; set; }


        public virtual DbSet<LocalCoupon> LocalCoupons { get; set; }

        public virtual DbSet<ShopMember> ShopMembers { get; set; }

        public virtual DbSet<AccessLog> AccessLogs { get; set; }


        public virtual DbSet<Keyword> Keywords { get; set; }

        public virtual DbSet<CouponUser> CouponUsers { get; set; }

        public virtual DbSet<ChildProxyApply> ChildProxyApplys { get; set; }

        public virtual DbSet<RegistrationCodeLog> RegistrationCodeLogs { get; set; }


        public virtual DbSet<UserRemark> UserRemarks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Buy.Bll
{
    public class Roles : IDisposable
    {
        public Roles()
        {
            _appRoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
            _appUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        private RoleManager<ApplicationRole> _appRoleManager;

        private UserManager<ApplicationUser> _appUserManager;

        public void EditUserRole(string userID, IEnumerable<string> roles)
        {
            var old = _appUserManager.GetRoles(userID).ToArray();
            _appUserManager.RemoveFromRoles(userID, old);
            foreach (var item in roles)
            {
                _appUserManager.AddToRole(userID, item);
            }
        }

        public void EditUserRoleByGroupID(string userID, int groupID)
        {
            var roles = db.RoleGroups.FirstOrDefault(s => s.ID == groupID).Roles.SplitToArray<string>();
            var old = _appUserManager.GetRoles(userID).ToArray();
            _appUserManager.RemoveFromRoles(userID, old);
            foreach (var item in roles)
            {
                _appUserManager.AddToRole(userID, item);
            }
        }

        public void InitNormalUserRole(string userID)
        {
            EditUserRole(userID, new string[] { });
        }

        public bool IsInRole(string userID, string role)
        {
            return _appUserManager.IsInRole(userID, role);
        }

        public IEnumerable<string> GetRoles(string userID)
        {
            return _appUserManager.GetRoles(userID);
        }

        public void Init()
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();
            Action<string, string, string> addUserRole = (name, gourp, desc) =>
             {
                 roles.Add(new ApplicationRole
                 {
                     Description = desc,
                     Group = gourp,
                     Name = name,
                     Type = Enums.RoleType.User
                 });
             };
            Action<string, string, string> addSystemRole = (name, gourp, desc) =>
            {
                roles.Add(new ApplicationRole
                {
                    Description = desc,
                    Group = gourp,
                    Name = name,
                    Type = Enums.RoleType.System
                });
            };
            #region 用户权限
            //addUserRole(SysRole.TicketCreate, "用户权限", "卡券创建");
            //addUserRole(SysRole.TicketSend, "用户权限", "卡券发放");
            //addUserRole(SysRole.TicketUse, "用户权限", "卡券使用");
            #endregion
            #region 后台权限
            //addSystemRole(SysRole.SystemUserManageRead, "系统用户", "系统用户查看");
            //addSystemRole(SysRole.SystemUserManageCreate, "系统用户", "系统用户创建");
            //addSystemRole(SysRole.SystemUserManageEdit, "系统用户", "系统用户编辑");
            //addSystemRole(SysRole.SystemUserManageDelete, "系统用户", "系统用户删除");

            //addSystemRole(SysRole.NormalUserManageRead, "用户管理", "普通用户查看");
            //addSystemRole(SysRole.NormalUserManageVidCreate, "用户管理", "普通用户验证");
            //addSystemRole(SysRole.NormalUserManageVidEdit, "用户管理", "普通用户验证编辑");

            //addSystemRole(SysRole.RoleManageRead, "权限管理", "权限管理查看");
            //addSystemRole(SysRole.RoleManageCreate, "权限管理", "权限管理创建");
            //addSystemRole(SysRole.RoleManageEdit, "权限管理", "权限管理编辑");
            //addSystemRole(SysRole.RoleManageDelete, "权限管理", "权限管理删除");

            //addSystemRole(SysRole.TicketManageRead, "卡券管理", "卡券管理查看");
            //addSystemRole(SysRole.TicketManageEdit, "卡券管理", "卡券管理编辑");
            //addSystemRole(SysRole.TicketManageDelete, "卡券管理", "卡券管理删除");

            //addSystemRole(SysRole.CompanyManageRead, "商家管理", "查看商家查看");
            //addSystemRole(SysRole.CompanyManageEdit, "商家管理", "查看商家编辑");

            //addSystemRole(SysRole.AccessLogRead, "点击统计", "点击统计查看");

            //addSystemRole(SysRole.CompanyRankManageRead, "商家榜管理", "商家榜查看");
            //addSystemRole(SysRole.CompanyRankManageEdit, "商家榜管理", "商家榜编辑");

            //addSystemRole(SysRole.UpdateLogManageRead, "更新日志", "更新日志查看");
            //addSystemRole(SysRole.UpdateLogManageCreate, "更新日志", "更新日志创建");
            //addSystemRole(SysRole.UpdateLogManageEdit, "更新日志", "更新日志编辑");
            //addSystemRole(SysRole.UpdateLogManageDelete, "更新日志", "更新日志删除");

            //addSystemRole(SysRole.PostManageRead, "帖子管理", "帖子查看");
            //addSystemRole(SysRole.PostManageDelete, "帖子管理", "帖子编辑");

            //addSystemRole(SysRole.SystemMessageRead, "消息提醒", "消息查看");
            //addSystemRole(SysRole.SystemMessageEdit, "消息提醒", "消息编辑");

            //addSystemRole(SysRole.PostThemeManageRead, "主题贴管理", "主题贴查看");
            //addSystemRole(SysRole.PostThemeManageCreate, "主题贴管理", "主题贴创建");
            //addSystemRole(SysRole.PostThemeManageEdit, "主题贴管理", "主题贴编辑");
            //addSystemRole(SysRole.PostThemeManageDelete, "主题贴管理", "主题贴删除");

            //addSystemRole(SysRole.FeedbackManageRead, "用户反馈", "用户反馈查看");

            //addSystemRole(SysRole.SiteMessageManageSend, "系统消息", "系统消息发送");

            //addSystemRole(SysRole.ThirdPartyTicketRead, "第三方优惠", "第三方优惠查看");
            //addSystemRole(SysRole.ThirdPartyTicketCreate, "第三方优惠", "第三方优惠创建");
            //addSystemRole(SysRole.ThirdPartyTicketDelete, "第三方优惠", "第三方优惠删除");

            //addSystemRole(SysRole.ThirdPartyFoodTicketRead, "快餐券管理", "快餐券管理查看");
            //addSystemRole(SysRole.ThirdPartyFoodTicketCreate, "快餐券管理", "快餐券管理创建");
            //addSystemRole(SysRole.ThirdPartyFoodTicketEdit, "快餐券管理", "快餐券管理编辑");
            //addSystemRole(SysRole.ThirdPartyFoodTicketDelete, "快餐券管理", "快餐券管理删除");

            //addSystemRole(SysRole.BannerManageRead, "Banner管理", "Banner管理查看");
            //addSystemRole(SysRole.BannerManageCreate, "Banner管理", "Banner管理创建");
            //addSystemRole(SysRole.BannerManageEdit, "Banner管理", "Banner管理编辑");
            //addSystemRole(SysRole.BannerManageDelete, "Banner管理", "Banner管理删除");

            //addSystemRole(SysRole.WeChatManage, "微信设置", "微信设置");


            #endregion


            foreach (var item in roles)
            {
                if (_appRoleManager.FindByName(item.Name) == null)
                {
                    _appRoleManager.Create(item);
                }
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
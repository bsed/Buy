using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Enums
{
    #region 公共枚举
    public enum DebugLog
    {
        /// <summary>
        /// 所有
        /// </summary>
        All,
        /// <summary>
        /// 不输出
        /// </summary>
        No,
        /// <summary>
        /// 警告以上
        /// </summary>
        Warning,
        /// <summary>
        /// 错误以上
        /// </summary>
        Error
    }

    public enum DebugLogLevel
    {
        /// <summary>
        /// 普通记录
        /// </summary>
        Normal,
        /// <summary>
        /// 警告级别
        /// </summary>
        Warning,
        /// <summary>
        /// 错误级别
        /// </summary>
        Error
    }

    /// <summary>
    /// 占位图
    /// </summary>
    public enum DummyImage
    {
        [Display(Name = "默认")]
        Default,
        [Display(Name = "头像")]
        Avatar
    }

    public enum ResizerMode
    {
        Pad,
        Crop,
        Max,
    }

    public enum ReszieScale
    {
        Down,
        Both,
        Canvas
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    [Flags]
    public enum DriveType
    {
        Windows = 1,
        IPhone = 2,
        IPad = 4,
        Android = 8,
        WindowsPhone = 16,
    }

    public enum RoleType
    {
        System,
        User,

    }

    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 视频
        /// </summary>
        Video,
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 音频
        /// </summary>
        Audio,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
    #endregion

    public enum UserTicketState
    {
        [Display(Name = "未集齐")]
        NoComplete,
        [Display(Name = "已集齐")]
        Completed,
        [Display(Name = "已使用")]
        Used,
        [Display(Name = "已赠送")]
        Given,
        [Display(Name = "赠送中")]
        Giving,
        [Display(Name = "已过期")]
        Expired
    }

    public enum UserTicketLogState
    {
        [Display(Name = "获取")]
        Get,
        [Display(Name = "使用")]
        Use,
        [Display(Name = "已赠送")]
        Given,
        [Display(Name = "赠送中")]
        Giving,
        [Display(Name = "从转赠获取")]
        GetFormTrade,
        [Display(Name = "奖励")]
        Reward,
        [Display(Name = "从分享获得")]
        GetFormShare
    }


    public enum TicketType
    {
        /// <summary>
        /// 收集券
        /// </summary>
        [Display(Name = "鱼章卡")]
        Collect,
        /// <summary>
        /// 打折券
        /// </summary>
        [Display(Name = "打折券")]
        Discount,
        /// <summary>
        /// 代金券
        /// </summary>
        [Display(Name = "抵用券")]
        Coupon,
        /// <summary>
        /// 礼品券
        /// </summary>
        [Display(Name = "礼品券")]
        Present

    }

    public enum TicketSort
    {
        DateTimeDisc,
        EndDateTimeDisc,

    }

    public enum ActionType
    {
        None,
        Browser,
        ThirdPartyTicketDetail,
        ThirdPartyFootTicketDetail,
        LocalCouponDetail,
        CouponClass
    }

    public enum SiteMessageType
    {
        [Display(Name = "默认")]
        Default,
        [Display(Name = "成功")]
        Success,
        [Display(Name = "警告")]
        Warming,
        [Display(Name = "错误")]
        Error,
    }

    public enum UserType
    {
        [Display(Name = "普通用户")]
        Normal,
        [Display(Name = "管理员")]
        System,
        [Display(Name = "代理")]
        Proxy,
        [Display(Name = "二级代理")]
        ProxySec
    }

    public enum ShareLogType
    {
        /// <summary>
        /// 优惠券分享
        /// </summary>
        TicketShare,
        /// <summary>
        /// 转增
        /// </summary>
        Trade,
        /// <summary>
        /// 鱼章卡分享
        /// </summary>
        CollectShare
    }

    public enum LikeLogType
    {
        //帖子
        [Display(Name = "帖子")]
        Post,
        Comment
    }

    public enum CommentType
    {
        /// <summary>
        /// 帖子
        /// </summary>
        [Display(Name = "帖子")]
        Post,
    }

    public enum PostType
    {
        /// <summary>
        /// 我发的贴
        /// </summary>
        [Display(Name = "")]
        MyPost,
        /// <summary>
        /// 其他用在我这里的贴
        /// </summary>
        [Display(Name = "用户发帖")]
        FanPost,
    }

    public enum Head
    {
        /// <summary>
        /// 商家首页头部
        /// </summary>
        CompanyHomePageHead,
        /// <summary>
        /// 商家首页可用券
        /// </summary>
        CompanyHomePageTicket,
        /// <summary>
        /// 用户中心首页头部
        /// </summary>
        UserHomePageHead,
    }

    /// <summary>
    /// Cell样式
    /// </summary>
    public enum CellStyle
    {
        AvatarCell,
        BaseCell,
        ThirdPartyTicket,
        ThirdPartyFootTicketCell
    }

    /// <summary>
    /// 关注类型
    /// </summary>
    public enum FriendType
    {
        All,
        User,
        Company
    }

    /// <summary>
    /// 商品的标签
    /// </summary>
    [Flags]
    public enum ProductTag
    {
        [Display(Name = "新品")]
        New = 1,
        [Display(Name = "推荐")]
        Top = 2,
        [Display(Name = "热门")]
        Hot = 4,
        [Display(Name = "促销")]
        Sale = 8
    }

    /// <summary>
    /// 商品状态
    /// </summary>
    public enum ProductState
    {
        Off,
        On,
        SoldOut
    }

    public enum ProductSort
    {
        Sort,
        PriceAsc,
        PriceDesc,
        Name,
    }

    public enum AccessLogType
    {
        [Display(Name = "惠券搜索")]
        CouponSearch,
    }

    public enum SystemSettingType
    {
        BannerSetting,
        ClassifySetting,
        CustomerService
    }

    public enum CompanyRankCellTag
    {
        Ticket,
        MembershipCard
    }

    public enum UpdateLogType
    {
        [Display(Name = "Android")]
        Android,
        [Display(Name = "IOS")]
        IOS
    }

    /// <summary>
    /// 链接匹配搜索类别
    /// </summary>
    public enum UrlMatchType
    {
        [Display(Name = "淘宝天猫")]
        Taobao,
        [Display(Name = "蘑菇街")]
        MoGuJie,
        [Display(Name = "本地券")]
        LocationCoupon
    }

    public enum UserTitleType
    {
        /// <summary>
        /// 商家
        /// </summary>
        [Display(Name = "商家")]
        Store,
    }

    public enum CouponPlatform
    {
        [Display(Name = "淘宝")]
        TaoBao = 0,
        [Display(Name = "天猫")]
        TMall = 1,
        [Display(Name = "京东")]
        Jd = 2,
        //[Display(Name = "唯品会")]
        //Vip,
        [Display(Name = "蘑菇街")]
        MoGuJie = 4
    }

    public enum CouponSort
    {
        [Display(Name = "综合排序")]
        Default,
        [Display(Name = "人气热销")]
        Sales,
        [Display(Name = "最新上架")]
        CreateTime,
        /// <summary>
        /// 券额
        /// </summary>
        [Display(Name = "券额")]
        CouponValue,
        /// <summary>
        /// 券后价
        /// </summary>
        [Display(Name = "券后价")]
        CouponPrice,
    }


    public enum BannerShowPlatform
    {
        [Display(Name = "Android")]
        Android,
        [Display(Name = "IOS")]
        IOS,
        [Display(Name = "小程序")]
        WechatLiteapp,
        [Display(Name = "网页")]
        Web
    }

    public enum TimeInterval
    {
        [Display(Name = "日")]
        Day,
        [Display(Name = "周")]
        Week,
        [Display(Name = "月")]
        Month,
        [Display(Name = "年")]
        Year
    }
}
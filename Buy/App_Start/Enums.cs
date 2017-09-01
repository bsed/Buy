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
        UserHomePage,
        UserTicket,
        Ticket,
        TicketIndex,
        PostDetails,
        CompanyHomePage,
        Map,
        PostTheme,
        PostThemeDetail,
        ThirdPartyTicketDetail,
        ThirdPartyFootTicketDetail
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
        [Display(Name = "普通")]
        Normal,
        [Display(Name = "系统")]
        System,
        [Display(Name = "子账号")]
        Child
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
        UserTicketCell,
        TicketCell,
        MessageCell,
        PostCell,
        CompanyHomePageTicketCell,
        CommentCell,
        AddressCell,
        ProductCell,
        CompanyRankCell,
        TicketWithAvatar,
        SystemMessageCell,
        PostThemeCell,
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
        [Display(Name = "商家主页")]
        CompanyIndex,
        [Display(Name = "商家商品")]
        CompanyProductIndex,
        [Display(Name = "商家顾客留言")]
        CompanyCustomerPostIndex,
        [Display(Name = "商家定位")]
        CompanyMap,
        [Display(Name = "商家电话")]
        CompanyPhone,
        [Display(Name = "商家优惠券")]
        CompanyTicket,
        [Display(Name = "商家wifi")]
        CompanyWifi,
        [Display(Name = "商家帖子详情")]
        CompanyPostDetail,
        [Display(Name = "商家帖子分享")]
        CompanyPostShare,
        [Display(Name = "第三方优惠券首页")]
        ThirdPartyTicketIndex,
        [Display(Name = "第三方优惠券详情页")]
        ThirdPartyTicketDetail,
        [Display(Name = "第三方优惠券平台类型")]
        ThirdPartyTicketPlatform,
        [Display(Name = "第三方优惠券分类")]
        ThirdPartyTicketType,
        [Display(Name = "第三方优惠券排序方式")]
        ThirdPartyTicketSort,
        [Display(Name = "第三方优惠券搜索")]
        ThirdPartyTicketSearch,
        [Display(Name = "第三方优惠券进入主页")]
        ThirdPartyTicketToIndex,
        [Display(Name = "第三方优惠券进入另一张券")]
        ThirdPartyTicketToOrderTicket,
        [Display(Name = "第三方优惠券App首页")]
        ThirdPartyTicketAppIndex,
    }

    public enum SystemSettingType
    {

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
        /// <summary>
        /// 商家
        /// </summary>
        [Display(Name = "商家")]
        Company,
        /// <summary>
        /// 券
        /// </summary>
        [Display(Name = "券")]
        Ticket,
        /// <summary>
        /// 主题帖
        /// </summary>
        [Display(Name = "主题帖")]
        PostTheme,
        /// <summary>
        /// 帮助
        /// </summary>
        [Display(Name = "帮助")]
        Help,
        /// <summary>
        /// 第三方优惠券
        /// </summary>
        [Display(Name = "第三方优惠券")]
        ThirdPartyTicket,
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
        TaoBao,
        [Display(Name = "天猫")]
        TMall,
        [Display(Name = "京东")]
        Jd,
        [Display(Name = "唯品会")]
        Vip,
        [Display(Name = "蘑菇街")]
        MoGuJie
    }

    public enum CouponSort
    {
        [Display(Name = "默认")]
        Default,
        [Display(Name = "销量")]
        Sales,
        [Display(Name = "价格升序")]
        PriceAsc,
        [Display(Name = "价格降序")]
        PriceDesc,
        [Display(Name = "优惠券金额")]
        Discount,
        [Display(Name = "优惠比例")]
        DiscountRate
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
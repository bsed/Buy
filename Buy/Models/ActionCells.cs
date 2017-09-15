using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using Buy.Enums;

namespace Buy.Models.ActionCell
{
    public abstract class Cell
    {
        public Cell() { }

        public Cell(string id = null, string title = null, string image = null, string url = null)
        {
            UrlToAction(url);
            if (id != null)
            {
                ID = id;
            }
            if (title != null)
            {
                Title = title;
            }
            if (image != null)
            {
                Image = image;
            }

        }


        public abstract Enums.CellStyle Style { get; }

        public void UrlToAction(string originallyUrl)
        {
            Enums.ActionType type;
            if (string.IsNullOrWhiteSpace(originallyUrl))
            {
                type = Enums.ActionType.None;
            }
            else
            {
                var url = originallyUrl.ToLower();
                type = Enums.ActionType.None;
                Uri uri;
                Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);
                if (!uri.IsAbsoluteUri)
                {
                    string temp = url.Remove(0, url.IndexOf('?') + 1);
                    temp.SplitToArray<string>('&').ForEach(s =>
                    {
                        var kv = s.SplitToArray<string>('=').ToArray();
                        if (kv.Length == 2)
                        {
                            Action.Add(new KeyValuePair<string, string>(kv[0], kv[1]));
                        }
                    });

                    if (url.Contains("/u?") || url.Contains("/u/index") || url.Contains("/user/index"))
                    {
                        
                    }
                    else if (url.Contains("/coupon/details"))
                    {
                        type = ActionType.ThirdPartyTicketDetail;
                    }
                    else
                    {
                        type = Enums.ActionType.Browser;
                        Action.Add(new KeyValuePair<string, string>("url", originallyUrl));
                        Action.Add(new KeyValuePair<string, string>("title", Title));
                    }
                }
                else
                {
                    type = Enums.ActionType.Browser;
                    Action.Add(new KeyValuePair<string, string>("url", originallyUrl));
                    Action.Add(new KeyValuePair<string, string>("title", Title));
                }
            }
            Type = type;
        }


        public string ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        public Enums.ActionType Type { get; set; }

        public List<KeyValuePair<string, string>> Action { get; set; } = new List<KeyValuePair<string, string>>();


    }

    public class BaseCell : Cell
    {

        public override CellStyle Style
        {
            get
            {
                return CellStyle.BaseCell;
            }
        }
    }

    /// <summary>
    /// 头像Cell
    /// </summary>
    public class AvatarCell : Cell
    {
        public AvatarCell() { }

        public AvatarCell(UserViewModel user)
        {

            ID = user.ID;
            Title = user.NickName;

            Image = user.AvatarFull;
            UserName = user.UserName;
            QRCode = user.QRCode;

        }

        public string UserName { get; set; }

        public string QRCode { get; set; }

        public override CellStyle Style
        {
            get
            {
                return Enums.CellStyle.AvatarCell;
            }
        }

        public List<Enums.UserTitleType> Titles { get; set; }
    }

    public class SystemAvatarCell : AvatarCell
    {
        public SystemAvatarCell()
        {
            Title = "系统";
            Type = Enums.ActionType.None;
            Image = Comm.ResizeImage("~/Content/Images/SystemAvatar_x144.jpg", image: null);
            UserName = null;
            QRCode = null;
        }
    }




    public class CouponCell : Cell
    {
        public CouponCell(Coupon tp)
        {
            Title = tp.Name;
            Image = Comm.ResizeImage(tp.Image, image: null);
            Action.Add(new KeyValuePair<string, string>("id", tp.ID.ToString()));
            Type = ActionType.ThirdPartyTicketDetail;
            Platform = tp.Platform;
            Price = tp.Price;
            OriginalPrice = tp.OriginalPrice;
            Sale = tp.OriginalPrice - tp.Price;
            Link = tp.Link;
            Sales = tp.Sales;
            Values = Bll.ThirdPartyTickets.GetValues(tp);
        }

        public override CellStyle Style
        {
            get
            {
                return CellStyle.ThirdPartyTicket;
            }
        }

        public Enums.CouponPlatform Platform { get; set; }

        public decimal Price { get; set; }

        public string Link { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal Sale { get; set; }

        public int Sales { get; set; }

        public List<ThirdPartyTicketValue> Values { get; set; }
    }


    public class ThirdPartyFootTicketCell : Cell
    {
        public ThirdPartyFootTicketCell(FoodCoupon tp)
        {
            ID = tp.ID.ToString();
            Title = tp.Name;
            Image = Comm.ResizeImage(tp.Image, image: null);
            Action.Add(new KeyValuePair<string, string>("id", tp.ID.ToString()));
            Type = ActionType.ThirdPartyFootTicketDetail;
            Price = tp.Price;
            TypeImage = Comm.ResizeImage(tp.Type.Image, image: null);
            EndDateTime = tp.EndDateTime.ToString("yyyy-MM-dd");
            StartDateTime = tp.StartDateTime.ToString("yyyy-MM-dd");
        }

        public override CellStyle Style
        {
            get
            {
                return CellStyle.ThirdPartyFootTicketCell;
            }
        }

        public string EndDateTime { get; set; }

        public string StartDateTime { get; set; }

        public string TypeImage { get; set; }

        public decimal Price { get; set; }
    }

    public class LocalCouponCell : Cell
    {
        public LocalCouponCell(LocalCoupon lc)
        {
            ID = lc.ID.ToString();
            Title = lc.Name;
            Image = Comm.ResizeImage(lc.Image, image: null);
            Action.Add(new KeyValuePair<string, string>("id", lc.ID.ToString()));
            Type = ActionType.LocalCouponDetail;
            Price = lc.Price;
            EndDateTime = lc.EndDateTime.ToString("yyyy-MM-dd");
            CreateDateTime = lc.CreateDateTime.ToString("yyyy-MM-dd");
            Remark = lc.Remark;
            ShopID = lc.ShopID;
            ShopName = lc.Shop.Name;
            ShopLogo= Comm.ResizeImage(lc.Shop.Logo, image: null);
        }

        public override CellStyle Style
        {
            get
            {
                return CellStyle.ThirdPartyFootTicketCell;
            }
        }

        public string EndDateTime { get; set; }

        public string CreateDateTime { get; set; }

        public decimal Price { get; set; }

        public string Remark { get; set; }
        
        public int ShopID { get; set; }

        public string ShopName { get; set; }

        public string ShopLogo { get; set; }
    }
}

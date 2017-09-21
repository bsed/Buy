using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class SystemSetting
    {
        public int ID { get; set; }

        public Enums.SystemSettingType Key { get; set; }

        public string Value { get; set; }
    }

    public class CouponSetting
    {
        [Display(Name = "页大小")]
        public int PageSize { get; set; }

        [Display(Name = "时间间隔")]
        public int Minute { get; set; }
    }

    public class BannerSetting
    {
        [Required]
        public int ID { get; set; }

        [Display(Name = "编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "图片")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "链接")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Display(Name = "显示平台")]
        public Enums.CouponPlatform Platform { get; set; }

    }

    public class BannerSettingCreateEditViewModel
    {
        public BannerSettingCreateEditViewModel()
        {

        }

        public BannerSettingCreateEditViewModel(BannerSetting setting)
        {
            ID = setting.ID;
            Title = setting.Title;
            Code = setting.Code;
            Image.Images = new string[] { setting.Image };
            Link = setting.Link;
            Sort = setting.Sort;
            Platform = setting.Platform;
        }

        [Required]

        public int ID { get; set; }

        [Display(Name = "编号")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "图片")]
        public FileUpload Image { get; set; } = new FileUpload { Max = 1, Name = "Image" };

        [Required]
        [Display(Name = "链接")]
        public string Link { get; set; }

        [Required]
        [Display(Name = "排序")]
        public int Sort { get; set; }

        [Required]
        [Display(Name = "显示平台")]
        public Enums.CouponPlatform Platform { get; set; }

    }

    public class NewsMessageViewModel : Buy.WeChat.NewsMessage
    {
        public NewsMessageViewModel() { }

        public NewsMessageViewModel(Buy.WeChat.NewsMessage model)
        {
            this.CreateDateTime = model.CreateDateTime;
            this.Description = model.Description;
            this.Event = model.Event;
            if (!string.IsNullOrWhiteSpace(model.PicUrl))
            {
                Files.Images = new string[] { model.PicUrl };
            }
            this.Sort = model.Sort;
            this.Title = model.Title;
            this.Url = model.Url;

        }

        [Display(Name = "图片")]
        public FileUpload Files { get; set; } = new FileUpload { Max = 1 };

        [Display(Name = "图片")]
        public override string PicUrl { get { return Files.Images.Length > 0 ? Files.Images[0] : null; } }
    }

    public class TempMessageViewModel : WeChat.TempMessage
    {
        public TempMessageViewModel(WeChat.TempMessage model)
        {
            ID = model.ID;
            Title = model.Title;
            PrimaryID = model.PrimaryID;
            DeputyID = model.DeputyID;
            Content = model.Content;

        }

        [Display(Name = "模版ID")]
        public override string ID { get; set; }

        [Display(Name = "名称")]
        public override string Title { get; set; }

        [Display(Name = "一级行业")]
        public override string PrimaryID { get; set; }

        [Display(Name = "二级行业")]
        public override string DeputyID { get; set; }

        [Display(Name = "内容")]
        public override string Content { get; set; }

        public List<string> Key
        {
            get
            {
                var s = Content.AllIndexesOf("{{");
                var e = Content.AllIndexesOf("}}");
                List<string> key = new List<string>();
                for (int i = 0; i < s.Count; i++)
                {
                    key.Add(new string(Content.Skip(s[i] + 2).Take(e[i] - s[i] - 2).ToArray()).Replace(".DATA", ""));
                }

                return key;
            }
        }
    }

    public class HongBaoEditViewModel
    {

        [Display(Name = "二维码")]
        public FileUpload File { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class SystemMessage
    {
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "图片")]
        public string Img { get; set; }

        [Display(Name = "连接")]
        public string Url { get; set; }

        [Display(Name = "按钮文字")]
        public string ButtonText { get; set; }
    }
    public class SystemMessageViewModel : SystemMessage
    {
        public SystemMessageViewModel() { }

        public SystemMessageViewModel(SystemMessage s)
        {
            CreateTime = s.CreateTime;
            Title = s.Title;
            Img = s.Img;
            Url = s.Url;
            FileUpload = new FileUpload()
            {
                Max = 1,
                Images = string.IsNullOrWhiteSpace(s.Img) ? new string[0] : new string[] { s.Img },
                Type = FileType.Image,
                Name = "SystemMessage",
            };
            ButtonText = s.ButtonText;
        }

        public FileUpload FileUpload { get; set; }
    }



}
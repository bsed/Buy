using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Buy.Models
{
    public class UpdateLog
    {
        public int ID { get; set; }

        [Display(Name = "类别")]
        public Enums.UpdateLogType Type { get; set; }

        [Display(Name = "更新内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "更新日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        [Display(Name = "下载链接")]
        public string Url { get; set; }

        [Display(Name = "版本号")]
        public string Ver { get; set; }
    }

    [NotMapped]
    public class UpdateLogViewModel :UpdateLog
    {
        public UpdateLogViewModel() { }

        public UpdateLogViewModel(UpdateLog updateLog)
        {
            ID = updateLog.ID;
            Type = updateLog.Type;
            Content = updateLog.Content;
            CreateDateTime = updateLog.CreateDateTime;
            Url = updateLog.Url;
            Ver = updateLog.Ver;
            FileUpload = new FileUpload();
            if (updateLog.Type == Enums.UpdateLogType.Android)
            {
                FileUpload = new FileUpload() {
                    FilePath = "~/download",
                    IsResetName = true,
                    Name = "AndroidUrl",
                    Type = FileType.File,
                    Images = new string[] { updateLog.Url }
                };
            }
        }

        public FileUpload FileUpload { get; set; }
    }
}
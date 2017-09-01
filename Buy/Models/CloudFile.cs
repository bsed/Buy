using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Buy.Models
{
    public class CloudFile
    {
        public CloudFile() { }

        public CloudFile(string userID, HttpPostedFileBase file, string url)
        {
            var fileInfo = new System.IO.FileInfo(file.FileName);
            CreateDateTime = DateTime.Now;
            Extension = fileInfo.Extension.ToLower();
            Name = fileInfo.Name;
            Size = file.ContentLength;
            Url = url;
            UserID = userID;
            var contentType = file.ContentType.ToLower();
            if (contentType.Contains("image"))
            {
                Type = Enums.FileType.Image;
            }
            else if (contentType.Contains("video"))
            {
                Type = Enums.FileType.Video;
            }
            else if (contentType.Contains("text"))
            {
                Type = Enums.FileType.Text;
            }
            else if (contentType.Contains("audio"))
            {
                Type = Enums.FileType.Audio;
            }
            else
            {
                Type = Enums.FileType.Other;
            }
        }

        public int ID { get; set; }

        [Display(Name = "用户")]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        [Display(Name = "文件名")]
        public string Name { get; set; }

        [Display(Name = "链接")]
        public string Url { get; set; }

        [Display(Name = "类型")]
        public Enums.FileType Type { get; set; }

        [Display(Name = "扩展名")]
        public string Extension { get; set; }

        [Display(Name = "大小")]
        public long Size { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateDateTime { get; set; }
    }
}
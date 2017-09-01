using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Buy.Models
{
    public class Help
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    public class HelpViewModel
    {

        public int ID { get; set; }

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "编号")]
        public string Code { get; set; }

        [AllowHtml]
        [Display(Name = "正文")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "发布时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }
        
    }
}
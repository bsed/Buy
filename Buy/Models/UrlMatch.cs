using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Buy.Models
{
    public class UrlMatch
    {
        public UrlMatch() { }
        
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string URL { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [Display(Name = "标签")]
        public string Tag { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        public Enums.UrlMatchType Type { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称;
        /// </summary>
        [Display(Name = "用户昵称")]
        public string UserNickName { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [Display(Name = "封面")]
        public string Image { get; set; }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class UrlMatchModel
    {
        public string ID { get; set; }

        public string URL { get; set; }
    }

}
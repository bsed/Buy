using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace Buy.Models
{
    [NotMapped]
    public class ChildProxyApplyViewModel : ChildProxyApply
    {
        public string Avatar { get; set; }

        public string NickName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
        
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;

namespace Buy.Bll
{
    public static class Accounts
    {
        

        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public static VerCode VerCode(string phone, string code)
        {
            VerCode verCode = new VerCode();
            verCode.IsSuccess = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var verifyCode = db.VerificationCodes.FirstOrDefault(s => s.To == phone && s.Code == code);
                if (verifyCode == null)
                {
                    verCode.Message = "验证码有误";
                }
                else if (verifyCode.CreateDate.AddMinutes(10) <= DateTime.Now)
                {
                    verCode.Message = "验证码已过期";
                }
                else
                {
                    verCode.IsSuccess = true;
                }

            }
            return verCode;
        }
        
    }
}
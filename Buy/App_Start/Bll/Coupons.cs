using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;

namespace Buy.Bll
{
    public class ThirdPartyTickets
    {
        public static int? CheckType(string keyword)
        {
            return Bll.SystemSettings.CouponType.FirstOrDefault(s =>
             {
                 var keys = s.Keyword.SplitToArray<string>();
                 return keys.Any(x => keyword.Contains(x));
             })?.ID;
        }

        public static List<ThirdPartyTicketValue> GetValues(Coupon t)
        {
            List<ThirdPartyTicketValue> values = new List<ThirdPartyTicketValue>();


            foreach (var item in t.Value.SplitToArray<string>())
            {
                if (t.Platform == Enums.CouponPlatform.Jd)
                {
                    try
                    {
                        if (item.Contains("-"))
                        {
                            var temp = item.SplitToArray<string>('-');
                            values.Add(new ThirdPartyTicketValue { Type = "券", Value = $"满{temp[0]}减{temp[1]}" });
                        }
                        else if (item.Contains("满"))
                        {
                            values.Add(new ThirdPartyTicketValue { Type = "减", Value = item });
                        }
                        else
                        {
                            values.Add(new ThirdPartyTicketValue { Type = "专享价", Value = $"{item.Remove(0, 4)}元" });
                        }
                    }
                    catch (Exception)
                    {
                        values.Add(new ThirdPartyTicketValue { Type = "券", Value = item });
                    }

                }
                else
                {
                    values.Add(new ThirdPartyTicketValue { Type = "券", Value = $"{(t.OriginalPrice - t.Price):##.##}元" });
                }

            }
            return values;
        }

        public static void DbAdd(List<Coupon> models)
        {
            if (models?.Count == 0)
            {
                return;
            }
            DateTime dtStart = DateTime.Now.Date;

            var afterFilter = new List<Coupon>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //分段添加到数据库
                int pageSize = 200;
                var totalPage = models.Count / pageSize + (models.Count % pageSize > 0 ? 1 : 0);

                var dtTS = DateTime.Now;
                for (int i = 0; i < totalPage; i++)
                {
                    int pageIndex = i * pageSize;
                    var addTemp = models.Skip(pageIndex).Take(pageSize).ToList();

                    //判断是否有重复添加
                    var links = addTemp.Select(s => s.Link).ToList();
                    var dbLinks = db.Coupons.Where(s => links.Contains(s.Link)).Select(s => s.Link).ToList();

                    if (dbLinks.Count > 0)
                    {
                        addTemp = addTemp.Where(s => !dbLinks.Contains(s.Link)).ToList();
                    }
                    afterFilter.AddRange(addTemp);
                    //db.ThirdPartyTickets.AddRange(addTemp);
                    //db.SaveChanges();
                }
                var dtTS2 = DateTime.Now;
                var perSec = (60 * 60 * 24) / Convert.ToDouble(afterFilter.Count);
                var ii = 0;
                totalPage = afterFilter.Count / pageSize + (afterFilter.Count % pageSize > 0 ? 1 : 0);
                for (int i = 0; i < totalPage; i++)
                {
                    int pageIndex = i * pageSize;
                    var addTemp = afterFilter.Skip(pageIndex).Take(pageSize).ToList();
                    foreach (var item in addTemp)
                    {
                        item.CreateDateTime = dtStart.AddSeconds(ii * perSec);
                        ii++;
                    }
                    db.Coupons.AddRange(addTemp);
                    db.SaveChanges();
                }
                Comm.WriteLog("testTime", $"重复时间：{(DateTime.Now - dtTS).TotalSeconds}，添加用时：{(DateTime.Now - dtTS2).TotalSeconds}，导入数量{models.Count}，添加数量{afterFilter.Count}，重复数{models.Count - afterFilter.Count}", Enums.DebugLogLevel.Normal);

            }

        }
    }
}
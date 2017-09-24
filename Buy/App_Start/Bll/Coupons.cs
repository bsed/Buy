using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;

namespace Buy.Bll
{
    public class Coupons
    {
        public static int? CheckType(string keyword, Enums.CouponPlatform platform = Enums.CouponPlatform.TaoBao)
        {
            return Bll.SystemSettings.CouponType
                .Where(s => s.Platform == platform && !string.IsNullOrWhiteSpace(s.Keyword))
                .FirstOrDefault(s =>
                {

                    var keys = s.Keyword.SplitToArray<string>();
                    return keys.Contains(keyword);
                })?.ID;
        }

        public static List<CouponValue> GetValues(Coupon t)
        {
            List<CouponValue> values = new List<CouponValue>();


            foreach (var item in t.Value.SplitToArray<string>())
            {
                if (t.Platform == Enums.CouponPlatform.Jd)
                {
                    try
                    {
                        if (item.Contains("-"))
                        {
                            var temp = item.SplitToArray<string>('-');
                            values.Add(new CouponValue { Type = "券", Value = $"满{temp[0]}减{temp[1]}" });
                        }
                        else if (item.Contains("满"))
                        {
                            values.Add(new CouponValue { Type = "减", Value = item });
                        }
                        else
                        {
                            values.Add(new CouponValue { Type = "专享价", Value = $"{item.Remove(0, 4)}元" });
                        }
                    }
                    catch (Exception)
                    {
                        values.Add(new CouponValue { Type = "券", Value = item });
                    }

                }
                else
                {
                    values.Add(new CouponValue { Type = "券", Value = $"{(t.OriginalPrice - t.Price):##.##}元" });
                }

            }
            return values;
        }

        public static void DbAdd(List<CouponUserViewModel> models)
        {
            if (models?.Count == 0)
            {
                return;
            }

            var userID = models.FirstOrDefault().UserID;
            var afterFilter = new List<Coupon>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //分段添加到数据库
                int pageSize = 200;
                var totalPage = models.Count / pageSize + (models.Count % pageSize > 0 ? 1 : 0);

                var dtTS = DateTime.Now;
                //这里只导入券，不含链接，链接在导入完商品有导入
                for (int i = 0; i < totalPage; i++)
                {
                    int pageIndex = i * pageSize;
                    var addTemp = models.Skip(pageIndex).Take(pageSize).Select(s => s.ToCoupon()).ToList();
                    var platforms = models.GroupBy(s => s.Platform).Select(s => s.Key).ToList();
                    //判断是否有重复添加
                    var pLinks = addTemp.Select(s => s.PLink).ToList();
                    List<string> dbLinks = new List<string>();
                    if (platforms.Count == 1)//如果非淘宝联盟
                    {
                        var p = platforms[0];
                        dbLinks = db.Coupons.Where(s => s.Platform == p && pLinks.Contains(s.PLink)).Select(s => s.PLink).ToList();
                    }
                    else
                    {
                        dbLinks = db.Coupons.Where(s => platforms.Contains(s.Platform) && pLinks.Contains(s.PLink)).Select(s => s.PLink).ToList();

                    }
                    if (dbLinks.Count > 0)
                    {
                        addTemp = addTemp.Where(s => !dbLinks.Contains(s.PLink)).ToList();
                    }
                    afterFilter.AddRange(addTemp);
                }

                //伪造添加时间
                //从每天8点开始总时间为16小时，分组商品数量大于100才使用该算法
                DateTime dtStart = DateTime.Now.Date.AddHours(8);
                var dtTS2 = DateTime.Now;
                var addDbCount = 0;
                var groupTypes = afterFilter.GroupBy(s => s.TypeID)
                    .Select(s => new { TypeID = s.Key, Count = s.Count() })
                    .Where(s => s.Count > 50)
                    .ToList();
                //重写时间的添加算法，按分类分组去算
                foreach (var type in groupTypes)
                {
                    var items = afterFilter.Where(s => s.TypeID == type.TypeID).ToList();
                    var perSec = (60 * 60 * 16) / Convert.ToDouble(type.Count);
                    var ii = 0;
                    foreach (var item in items)
                    {
                        item.CreateDateTime = dtStart.AddSeconds(ii * perSec);
                        ii++;
                    }
                }
                //分页导入数据数据
                var afterTotalPage = afterFilter.Count / pageSize + (afterFilter.Count % pageSize > 0 ? 1 : 0);
                for (int i = 0; i < afterTotalPage; i++)
                {
                    try
                    {
                        int pageIndex = i * pageSize;
                        var addTemp = afterFilter.Skip(pageIndex).Take(pageSize).ToList();

                        db.Coupons.AddRange(addTemp);
                        addDbCount += db.SaveChanges();
                        //将所有的标题压缩成一串哪去关键字分析
                        var titles = string.Join(",", addTemp.Select(s => s.Name).ToList());
                        Bll.Keywords.Add(titles);
                    }
                    catch (Exception ex)
                    {
                        Comm.WriteLog("ImportLog", ex.Message, Enums.DebugLogLevel.Error);
                    }

                }

                Comm.WriteLog("testTime", $"重复时间：{(dtTS2 - dtTS).TotalSeconds}，添加用时：{(DateTime.Now - dtTS2).TotalSeconds}，导入数量{models.Count}，添加数量{addDbCount}，重复数{models.Count - afterFilter.Count},添加失败数{afterFilter.Count - addDbCount}", Enums.DebugLogLevel.Normal);


                //导入用户用户券
                for (int i = 0; i < totalPage; i++)
                {
                    int pageIndex = i * pageSize;
                    var addTemp = models.Skip(pageIndex).Take(pageSize).ToList();
                    var platforms = models.GroupBy(s => s.Platform).Select(s => s.Key).ToList();
                    //判断是否有重复添加
                    var links = addTemp.Select(s => s.Link).ToList();
                    List<string> dbLinks;//重复的券
                    if (platforms.Count == 1)//如果非淘宝联盟
                    {
                        var p = platforms[0];
                        dbLinks = db.CouponUsers
                            .Where(s => s.UserID == userID
                            && s.Platform == p
                            && links.Contains(s.Link))
                            .Select(s => s.Link)
                            .ToList();
                    }
                    else
                    {
                        dbLinks = db.CouponUsers
                           .Where(s => s.UserID == userID
                           && platforms.Contains(s.Platform)
                           && links.Contains(s.Link))
                           .Select(s => s.Link)
                           .ToList();
                    }

                    if (dbLinks.Count > 0)
                    {
                        addTemp = addTemp.Where(s => !dbLinks.Contains(s.Link)).ToList();
                        links = addTemp.Select(s => s.Link).ToList();
                    }
                    //通过pLink反查CountID
                    var pLinks = addTemp.Select(s => s.PLink);
                    var queryCoupon = db.Coupons.AsQueryable();
                    if (platforms.Count == 1)
                    {
                        var p = platforms[0];
                        queryCoupon = queryCoupon.Where(s => s.Platform == p && pLinks.Contains(s.PLink));

                    }
                    else
                    {
                        queryCoupon = queryCoupon.Where(s => platforms.Contains(s.Platform)
                            && pLinks.Contains(s.PLink));
                    }
                    var couponIDs = queryCoupon
                        .Select(s => new { s.ID, s.PLink })
                        .ToList();
                    var userCoupons = (
                                       from t in addTemp
                                       join ci in couponIDs on t.PLink equals ci.PLink into cig
                                       from k in cig.DefaultIfEmpty()
                                       where k != null
                                       select new CouponUser
                                       {
                                           CouponID = k.ID,
                                           Link = t.Link,
                                           PCouponID = t.PCouponID,
                                           UserID = userID,
                                           Platform = t.Platform,
                                           ProductID = t.ProductID
                                       }).ToList();
                    db.CouponUsers.AddRange(userCoupons);
                    db.SaveChanges();
                }

            }

        }



    }
}
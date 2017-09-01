using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buy.Models;


namespace Buy.Interface
{
    interface IThirdPartyTicketExcel
    {
        void Set(string path);
    }

    public class TaobaoExcel : IThirdPartyTicketExcel
    {


        public void Set(string path)
        {
            List<Models.Coupon> models = new List<Models.Coupon>();
            try
            {
                var dtable = new ExcelHelper(path).ExcelToDataTable(null, true);

                foreach (System.Data.DataRow item in dtable.Rows)
                {
                    var index = dtable.Rows.IndexOf(item);
                    try
                    {
                        var model = new Models.Coupon
                        {
                            EndDateTime = Convert.ToDateTime(item["优惠券结束时间"]).AddDays(1).AddSeconds(-1),
                            ProductID = item["商品id"].ToString(),
                            TypeID = Bll.ThirdPartyTickets.CheckType(item["商品一级类目"].ToString()),
                            Image = item["商品主图"].ToString(),
                            Link = item["商品优惠券推广链接"].ToString(),
                            Name = item["商品名称"].ToString(),
                            OriginalPrice = Convert.ToDecimal(item["商品价格(单位：元)"]),
                            ProductType = item["商品一级类目"].ToString(),
                            ShopName = item["店铺名称"].ToString(),
                            StartDateTime = Convert.ToDateTime(item["优惠券开始时间"].ToString()),
                            Subtitle = null,
                            Value = item["优惠券面额"].ToString(),
                            Platform = item["平台类型"].ToString() == "淘宝" ? Enums.CouponPlatform.TaoBao : Enums.CouponPlatform.TMall,
                            Sales = Convert.ToInt32(item["商品月销量"]),
                            Commission = Convert.ToDecimal(item["佣金"]),
                            CommissionRate = Convert.ToDecimal(item["收入比率(%)"]),
                            Left = Convert.ToInt32(item["优惠券剩余量"]),
                            Total = Convert.ToInt32(item["优惠券总量"])

                        };
                        try
                        {
                            model.Price = Taobao.GetAfterCouponPrice(model.OriginalPrice, model.Value);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        if (model.EndDateTime < DateTime.Now || model.OriginalPrice <= model.Price)
                        {
                            continue;
                        }
                        if (model.Price < 0)
                        {
                            continue;
                        }
                        models.Add(model);

                    }
                    catch (Exception ex)//有异常的数据忽略
                    {

                    }
                }
                //分段添加到数据库
                Bll.ThirdPartyTickets.DbAdd(models);

            }
            catch (Exception ex)
            {
                //Comm.WriteLog("excel", $"{ex.Message}", Enums.DebugLogLevel.Error);
            }
        }
    }
}

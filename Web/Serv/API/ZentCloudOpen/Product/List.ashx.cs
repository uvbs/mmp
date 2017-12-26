using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product
{
    /// <summary>
    /// 商品列表
    /// </summary>
    public class List : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {


            int totalCount = 0;
            var sourceData = bllMall.GetProductList(context, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           product_sn = p.ProductCode,
                           title = p.PName,
                           quote_price = p.PreviousPrice,
                           price = p.Price,
                           category_id = p.CategoryId,
                           category_name = bllMall.GetWXMallCategoryName(p.CategoryId),
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           pv = p.PV,
                           tags = p.Tags,
                           sale_count = p.SaleCount,
                           create_time = p.InsertDate.ToString(),
                           last_update_time = p.LastUpdate == null ? "" : ((DateTime)p.LastUpdate).ToString("yyyy-MM-dd HH:mm:ss"),
                           stock = bllMall.GetProductTotalStock(int.Parse(p.PID))
                           //skus = from s in bllMall.GetProductSkuList(int.Parse(p.PID))
                           //       select new
                           //       {
                           //           sku_id = s.SkuId,
                           //           properties = bllMall.GetProductProperties(s.SkuId),
                           //           price = bllMall.GetSkuPrice(s),
                           //           count = bllMall.IsPromotionTime(s) ? p.PromotionStock : p.Stock
                           //       }

                       };
            resp.status = true;
            resp.msg = "ok";
            resp.result = new
            {
                totalcount = totalCount,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}
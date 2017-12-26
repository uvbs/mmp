using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Product
{
    /// <summary>
    /// 商品详情
    /// </summary>
    public class Get : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string productId = context.Request["product_id"];
            string productSn=context.Request["product_sn"];
            string storeId=context.Request["store_id"];
            if (string.IsNullOrEmpty(productId) && string.IsNullOrEmpty(productSn))
            {
                resp.msg = "product_id product_sn不能同时为空";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var productInfo = bllMall.GetProduct(productId);
            if (productInfo==null)
            {
                productInfo = bllMall.GetProductByProductCode(productSn);
            }

            if (productInfo == null)
            {
                resp.msg = "商品不存在";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var productSkuList = bllMall.GetProductSkuList(int.Parse(productInfo.PID),true,storeId);//源SKU 
            var skus = from p in productSkuList
                       select new
                       {
                           sku_id = p.SkuId,
                           properties = bllMall.GetProductProperties(p.SkuId),
                           price = bllMall.GetSkuPrice(p),
                           count = bllMall.IsPromotionTime(p) ? p.PromotionStock : p.Stock
                       };

            resp.result= new
            {
                product_id = productInfo.PID,
                product_sn = productInfo.ProductCode,
                title = productInfo.PName,
                quote_price = productInfo.PreviousPrice,
                price = productInfo.Price,
                category_id = productInfo.CategoryId,
                category_name = bllMall.GetWXMallCategoryName(productInfo.CategoryId),
                img_url = bllMall.GetImgUrl(productInfo.RecommendImg),
                is_onsale = (!string.IsNullOrEmpty(productInfo.IsOnSale) && productInfo.IsOnSale == "1") ? 1 : 0,
                pv = productInfo.PV,
                sale_count = productInfo.SaleCount,
                create_time =productInfo.InsertDate.ToString(),
                last_update_time = productInfo.LastUpdate == null ? "" :((DateTime)productInfo.LastUpdate).ToString("yyyy-MM-dd HH:mm:ss"),
                stock = bllMall.GetProductTotalStock(int.Parse(productInfo.PID)),
                tags = productInfo.Tags,
                skus=skus,
                product_desc=productInfo.PDescription



            };
            resp.status = true;
            resp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }



    }
}
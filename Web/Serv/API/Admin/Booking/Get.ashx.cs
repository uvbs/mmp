using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Booking
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string productId = context.Request["product_id"];
            WXMallProductInfo productInfo = bllMall.GetByKey<WXMallProductInfo>("PID", productId,true);
            if (productInfo == null)
            {
                apiResp.msg = "数据未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllMall.ContextResponse(context, apiResp);
                return;
            }

            List<WXMallProductInfo> relList = new List<WXMallProductInfo>();
            if (!string.IsNullOrWhiteSpace(productInfo.RelationProductId))
            {
                string pIDStrings = "'" + productInfo.RelationProductId.Replace(",", "','") + "'";
                relList = bllMall.GetColMultListByKey<WXMallProductInfo>(int.MaxValue, 1, "PID", pIDStrings, "PID,PName,Price,Unit",true);
            }
            if (relList.Count == 0)
            {
                productInfo.RelationProductId = "";
            }
            else
            {
                relList = relList.Distinct().ToList();
                productInfo.RelationProductId = ZentCloud.Common.MyStringHelper.ListToStr(relList.Select(p => p.PID).ToList(), "", ",");
            }
            List<ProductSku> skuList = bllMall.GetColList<ProductSku>(int.MaxValue, 1, string.Format("ProductId={0}", productInfo.PID), "SkuId,ProductId,PropValueIdEx1,PropValueIdEx2,PropValueIdEx3,Price");
            apiResp.result = new
            {
                product_id = productInfo.PID,
                product_title = productInfo.PName,
                category_id = productInfo.CategoryId,
                product_summary = productInfo.Summary,
                product_desc = productInfo.PDescription,
                price = productInfo.Price,
                unit = productInfo.Unit,
                is_onsale = productInfo.IsOnSale,
                totalcount = productInfo.Stock,
                sort = productInfo.Sort,
                access_Level = productInfo.AccessLevel,
                show_imgs = productInfo.ShowImage,
                relation_products = from p in relList
                                    select new
                                    {
                                        product_id = p.PID,
                                        title = p.PName,
                                        price = p.Price,
                                        unit = p.Unit
                                    },
                sku_list = from p in skuList
                           select new
                           {
                               id = p.SkuId,
                               price = p.Price,
                               start = p.PropValueIdEx1,
                               end = p.PropValueIdEx2,
                               week = p.PropValueIdEx3
                           }
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }
    }
}
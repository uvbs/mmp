using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 限时特卖商品管理 无用
    /// </summary>
    public class PromotionProduct : BaseHandlerNeedLoginAdmin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            int totalCount = 0;
            var sourceData = bllMall.GetPromotionProductList(context, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           product_title = p.PName,
                           price = p.Price,
                           promotion_price = p.PromotionPrice,
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           promotion_start_timestamp = p.PromotionStartTime,
                           promotion_stop_timestamp = p.PromotionStopTime,
                           sale_count = bllMall.GetProductSaleCount(int.Parse(p.PID)),
                           promotion_count = p.PromotionStock,
                           totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID))

                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            string productId = context.Request["product_id"];//商品编号
            decimal promotionPrice = decimal.Parse(context.Request["promotion_price"]);//特卖价格
            int promotionCount = int.Parse(context.Request["promotion_count"]);//特卖库存
            double promotionStartTimeStamp = double.Parse(context.Request["promotion_start_timestamp"]);//特卖开始时间
            double promotionStopTimeStamp = double.Parse(context.Request["promotion_stop_timestamp"]);//特卖结束时间
            WXMallProductInfo productInfo = bllMall.GetProduct(productId);
            if (productInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "商品不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (productInfo.IsPromotionProduct == 1)
            {
                resp.errcode = 1;
                resp.errmsg = "此商品已经加入限时特卖";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (productInfo.IsOnSale == "0")
            {
                resp.errcode = 1;
                resp.errmsg = "此商品已经下架";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (promotionPrice <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "特卖价不能小于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            productInfo.IsPromotionProduct = 1;
            productInfo.PromotionPrice = promotionPrice;
            productInfo.PromotionStartTime = promotionStartTimeStamp;
            productInfo.PromotionStopTime = promotionStopTimeStamp;
            productInfo.PromotionStock = promotionCount;

            if (bllMall.Update(productInfo))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加限时特卖商品失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 编辑限时特卖商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {

            string productId = context.Request["product_id"];//商品编号
            decimal promotionPrice = decimal.Parse(context.Request["promotion_price"]);//特卖价格
            int promotionCount = int.Parse(context.Request["promotion_count"]);//特卖库存
            double promotionStartTimeStamp = double.Parse(context.Request["promotion_start_timestamp"]);//特卖库存
            double promotionStopTimeStamp = double.Parse(context.Request["promotion_stop_timestamp"]);//特卖库存
            WXMallProductInfo productInfo = bllMall.GetProduct(productId);
            if (productInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "商品不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            if (productInfo.IsOnSale == "0")
            {
                resp.errcode = 1;
                resp.errmsg = "此商品已经下架";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (promotionPrice <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "特卖价不能小于0";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            productInfo.IsPromotionProduct = 1;
            productInfo.PromotionPrice = promotionPrice;
            productInfo.PromotionStartTime = promotionStartTimeStamp;
            productInfo.PromotionStopTime = promotionStopTimeStamp;
            productInfo.PromotionStock = promotionCount;
            if (bllMall.Update(productInfo))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "添加限时特卖商品失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 删除限时特卖商品
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string productIds = context.Request["product_ids"];//商品id
            int resultCount = bllMall.Update(new WXMallProductInfo(), " IsPromotionProduct=0,PromotionStartTime=0,PromotionStopTime=0,PromotionPrice=0,PromotionStock=0", string.Format(" PID in ({0})", productIds));
            if (resultCount == productIds.Split(',').Length)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除限时特卖商品失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


    }
}
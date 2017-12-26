using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Booking.Order
{
    /// <summary>
    /// 预约订单详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string order_id = context.Request["order_id"];//订单状态 中文字符串
            WXMallOrderInfo orderInfo = bllMall.GetByKey<WXMallOrderInfo>("OrderID", order_id, true);
            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            var orderMainDetailList = orderDetailList.Where(p => p.ArticleCategoryType == orderInfo.ArticleCategoryType).ToList();
            var orderAddedDetailList = orderDetailList.Where(p => p.ArticleCategoryType != orderInfo.ArticleCategoryType).ToList();
            var mainProduct = bllMall.GetByKey<WXMallProductInfo>("PID", orderMainDetailList[0].PID, true);
            apiResp.result = new
            {
                order_id = orderInfo.OrderID,
                order_time = orderInfo.InsertDate.ToString("yyyy-MM-dd HH:mm:ss"),
                product_name = orderMainDetailList[0].ProductName,
                price = orderMainDetailList[0].OrderPrice,
                unit = orderMainDetailList[0].Unit,
                img = mainProduct.RecommendImg,
                show_imgs = mainProduct.ShowImage,
                total_amount = orderInfo.TotalAmount,
                order_status = orderInfo.Status,
                is_pay = orderInfo.PaymentStatus,
                pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY",
                use_score=orderInfo.UseScore,
                use_amount=orderInfo.UseAmount,
                order_details = from p in orderMainDetailList
                                select new
                                {
                                    start_date = p.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    end_date = p.EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                                },
                added_details = from p in orderAddedDetailList
                                select new
                                {
                                    product_name = p.ProductName,
                                    price = p.OrderPrice,
                                    count = p.TotalCount,
                                    unit = p.Unit
                                }

            };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }


    }
}
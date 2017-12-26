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
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {

        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            int page = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int rows = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string status = context.Request["order_status"];//订单状态 中文字符串
            string orderFromTime = context.Request["order_from_time"];
            string orderToTime = context.Request["order_to_time"];
            string orderType = context.Request["order_type"];//订单类型 0 普通订单 1礼品订单
            string giftOrderType = context.Request["gift_order_type"];//礼品订单类型 0收到的礼品订单 1 发出的礼品订单
            string type = context.Request["type"];//订单大类型 空普通订单 MeetingRoom会议室预订 BookingTutor导师预约

            int totalCount = 0;
            var orderList = bllMall.GetOrderList(rows, page, "", out totalCount, status, CurrentUserInfo.UserID, orderFromTime, orderToTime, 
                orderType, giftOrderType, null, null, null, null, null, type);
            List<dynamic> list = new List<dynamic>();
            foreach (var orderInfo in orderList)
            {
                var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                var orderMainDetailList = orderDetailList.Where(p => p.ArticleCategoryType == orderInfo.ArticleCategoryType).ToList();
                var orderAddedDetailList = orderDetailList.Where(p => p.ArticleCategoryType != orderInfo.ArticleCategoryType).ToList();
                var mainProduct = bllMall.GetByKey<WXMallProductInfo>("PID", orderMainDetailList[0].PID, true);
                list.Add(new
                {
                    order_id = orderInfo.OrderID,
                    order_time = orderInfo.InsertDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    product_name = orderMainDetailList[0].ProductName,
                    price = orderMainDetailList[0].OrderPrice,
                    unit = orderMainDetailList[0].Unit,
                    img = mainProduct.RecommendImg,
                    show_imgs =  mainProduct.ShowImage,
                    total_amount = orderInfo.TotalAmount,
                    order_status = orderInfo.Status,
                    is_pay = orderInfo.PaymentStatus,
                    pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY",
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

                });
            }
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
            };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }
    }
}
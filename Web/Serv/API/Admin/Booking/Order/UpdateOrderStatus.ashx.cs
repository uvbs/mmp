using MySpider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Booking.Order
{
    /// <summary>
    /// UpdateOrderStatus 的摘要说明
    /// </summary>
    public class UpdateOrderStatus : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string order_status = context.Request["order_status"];
            bool actionStatus = false;
            if (order_status == "预约成功")
            {
                List<string> successIds = new List<string>();
                List<string> failIds = new List<string>();
                List<string> orderIds = ids.Split(',').ToList();
                string hasOrderIDs = "";
                int maxCount = 1;
                foreach (string orderId in orderIds)
                {
                    //已有订单详情
                    WXMallOrderInfo tOrder = bllMall.GetByKey<WXMallOrderInfo>("OrderID",orderId);
                    List<WXMallOrderDetailsInfo> tDetailList = bllMall.GetOrderDetailsList(orderId, null, tOrder.ArticleCategoryType, null, null);
                    List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, tDetailList[0].PID, tOrder.ArticleCategoryType, tDetailList.Min(p => p.StartDate), tDetailList.Max(p => p.EndDate));
                    WXMallProductInfo tProductInfo = bllMall.GetByKey<WXMallProductInfo>("PID", tDetailList[0].PID);
                    maxCount = tProductInfo.Stock;

                    List<string> hasOrderID_List = new List<string>();
                    foreach (var item in tDetailList)
                    {
                        List<WXMallOrderDetailsInfo> hasOrderDetailList = oDetailList.Where(p => !((item.StartDate >= p.EndDate && item.EndDate > p.EndDate) || (item.StartDate < p.StartDate && item.EndDate <= p.StartDate))).ToList();
                        if (hasOrderDetailList.Count > 0)
                        {
                            hasOrderID_List.AddRange(hasOrderDetailList.Select(p => p.OrderID).Distinct());
                        }
                    }
                    hasOrderID_List = hasOrderID_List.Where(p => !p.Contains(orderId)).ToList();
                    if (hasOrderID_List.Count > 0)
                    {
                        hasOrderID_List = hasOrderID_List.Distinct().ToList();
                        hasOrderIDs = MyStringHelper.ListToStr(hasOrderID_List, "'", ",");
                        int tempCount = 0;
                        List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "预约成功", null, null, null,
                                null, null, null, null, null, null, null, tOrder.ArticleCategoryType, hasOrderIDs);
                        if (tempCount >= maxCount)
                        {
                            failIds.Add(orderId);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(hasOrderIDs)) hasOrderIDs = "'0'";
                        if(bllMall.Update(new WXMallOrderInfo(),
                            string.Format("PaymentStatus={0},PayTime=GetDate(),Status='{1}'", 1, "预约成功"),
                            string.Format("OrderID={0} AND WebsiteOwner='{4}' and (select count(1) from [ZCJ_WXMallOrderInfo] where Status='{3}' and WebsiteOwner='{4}' and  OrderID IN({1}))<{2}",
                                tOrder.OrderID, hasOrderIDs, maxCount, "预约成功",bllMall.WebsiteOwner)
                            ) > 0)
                        {
                            hasOrderIDs = string.Format("{0},'{1}'", hasOrderIDs,tOrder.OrderID);
                            successIds.Add(orderId);
                            #region 修改其他预约订单为预约失败
                                bllMall.Update(new WXMallOrderInfo(),
                                    string.Format("Status='{0}'", "预约失败"),
                                    string.Format("OrderID In ({0}) AND Status Not In ({1}) and WebsiteOwner='{4}' AND (select count(1) from [ZCJ_WXMallOrderInfo] where Status='{5}' and OrderID IN({2}) and WebsiteOwner='{4}' )>={3}",
                                        hasOrderIDs, "'预约失败','预约成功','已取消'", hasOrderIDs, maxCount, bllMall.WebsiteOwner, "预约成功"));
                            #endregion
                        }
                        else
                        {
                            failIds.Add(orderId);
                        }
                    }
                    else
                    {
                        tOrder.Status = order_status;
                        if (bllMall.Update(tOrder))
                        {
                            successIds.Add(orderId);
                        }
                        else
                        {
                            failIds.Add(orderId);
                        }
                    }
                }
                if (orderIds.Count == 0)
                {
                    apiResp.msg = "修改完成";
                }
                else
                {
                    if(failIds.Count == 0){
                        apiResp.msg = "全部修改成功";
                    }
                    else if (successIds.Count == 0){
                        apiResp.msg = "全部订单所选时间已有预约成功的订单";
                    }
                    else{
                        apiResp.msg = "订单[" + MyStringHelper.ListToStr(successIds, "", ",") + "]修改成功，订单[" + MyStringHelper.ListToStr(failIds, "", ",") + "]所选时间已有预约成功的订单";
                    }
                }
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                bllMall.ContextResponse(context, apiResp);
            }
            else
            {
                List<string> orderIds = ids.Split(',').ToList();
                ids = MyStringHelper.ListToStr(orderIds, "'", ",");
                if (bllMall.UpdateMultByKey<WXMallOrderInfo>("OrderID", ids, "Status", order_status, null, true) > 0)
                {
                    apiResp.status = true;
                    apiResp.msg = "修改完成";
                    apiResp.code = (int)APIErrCode.IsSuccess;
                }
                else
                {
                    apiResp.msg = "修改失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                }
            }
            bllMall.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
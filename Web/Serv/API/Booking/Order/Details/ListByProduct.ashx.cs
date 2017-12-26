using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Booking.Order.Details
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class ListByProduct : BaseHandlerNeedLoginNoAction
    {
        BLLMall bllMall = new BLLMall();

        public void ProcessRequest(HttpContext context)
        {
            string productId = context.Request["product"];
            DateTime start = Convert.ToDateTime(context.Request["start"]);
            DateTime end = Convert.ToDateTime(context.Request["end"]);
            string type = context.Request["type"];
            List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, productId, type, start, end);
            List<OrderDetailsGroupBy> rReusltList = new List<OrderDetailsGroupBy>();
            if (oDetailList.Count > 0)
            {
                WXMallProductInfo tProductInfo = bllMall.GetByKey<WXMallProductInfo>("PID", productId);
                int maxCount = tProductInfo.Stock;
                string oOrderIds = ZentCloud.Common.MyStringHelper.ListToStr(oDetailList.Select(p => p.OrderID).Distinct().ToList(), "'", ",");
                List<WXMallOrderInfo> orderList = bllMall.GetMultListByKey<WXMallOrderInfo>("OrderID", oOrderIds,true);
                orderList = orderList.Where(p => p.Status == "预约成功").ToList();
                if (orderList.Count >= maxCount)
                {
                    List<string> sOrderIdList =  orderList.Select(p => p.OrderID).Distinct().ToList();
                    oDetailList = oDetailList.Where(p => sOrderIdList.Contains(p.OrderID)).ToList();
                    rReusltList = oDetailList.GroupBy(p => new
                    {
                        p.StartDate,
                        p.EndDate
                    }).Select(g => new OrderDetailsGroupBy
                    {
                        StartDate = g.Key.StartDate,
                        EndDate = g.Key.EndDate,
                        GroupByCount = g.Count()
                    }).Where(p => p.GroupByCount >= maxCount).OrderBy(x => x.StartDate).ToList();
                }
            }
            var result = from p in rReusltList
                         select new
                         {
                             start = p.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                             end = p.EndDate.ToString("yyyy-MM-dd HH:mm:ss")
                         };
            apiResp.status = true;
            apiResp.result = result;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }
        public class OrderDetailsGroupBy
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime StartDate { set; get; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime EndDate { set; get; }
            /// <summary>
            /// 记录数
            /// </summary>
            public int GroupByCount { set; get; }
        }
    }
}
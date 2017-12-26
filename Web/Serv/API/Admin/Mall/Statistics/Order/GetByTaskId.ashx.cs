using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Order
{
    /// <summary>
    /// 订单统计
    /// </summary>
    public class GetByTaskId : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string taskId = context.Request["task_id"];

            List<WXMallStatisticsOrder> list=new List<WXMallStatisticsOrder>();
            BLLJIMP.Model.WXMallStatisticsOrder model = bllMall.Get<BLLJIMP.Model.WXMallStatisticsOrder>(string.Format("TaskId='{0}' And WebsiteOwner='{1}'",taskId,bllMall.WebsiteOwner));
            list.Add(model);
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = 1,
                list = from p in list
                       select new {

                           insert_date = p.InsertDate.ToString(),
                           from_date = p.FromDate != null ? ((DateTime)p.FromDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                           to_date = p.ToDate != null ? ((DateTime)p.ToDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                           total_count=p.TotalCount,
                           total_amount=p.TotalAmount,
                           base_total_amount = p.BaseTotalAmount,
                           total_transport_fee=p.TotalTransportFee,
                           profit = p.Profit,
                           total_coupon_exchang_amount=p.TotalCouponExchangAmount,
                           total_score_exchang_amount=p.TotalScoreExchangAmount,
                           total_accountamount_exchang_amount=p.TotalAccountAmountExchangAmount,
                           total_storecard_exchang_amount = p.TotalStorecardExchangAmount,
                           total_product_count=p.TotalProductCount,
                           total_product_fee=p.TotalProductFee,
                           total_refund_amount=p.TotalRefundAmount,
                           task_id=p.TaskId,
                           should_commission_order_count = p.ShouldCommissionOrderCount,
                           real_commission_order_count = p.RealCommissionOrderCount,
                           last_receiving_time = p.LastReceivingTime,
                           refund_order_count = p.RefundOrderCount,
                           wait_process_order_count = p.WaitProcessOrderCount
                       }


            };
            bllMall.ContextResponse(context, apiResp);



        }


    }
}
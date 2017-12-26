using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Order
{
    /// <summary>
    /// 订单统计明细
    /// </summary>
    public class OrderDetail : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string taskId = context.Request["task_id"];
            int pageIndex=int.Parse(context.Request["page"]);
            int pageSize=int.Parse(context.Request["rows"]);
            string type=context.Request["type"];
            int totalCount=0;
            List<WXMallStatisticsOrderDetail> list = bllMall.GetWXMallStatisticsOrderDetail(pageSize,pageIndex,taskId,type,out totalCount);
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount =totalCount,
                list = from p in list
                       select new
                       {
                           order_id=p.OrderId

                       }


            };
            bllMall.ContextResponse(context, apiResp);



        }
    }
}
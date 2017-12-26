using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerOrder
{
    /// <summary>
    /// 获取我的服务订单点评记录
    /// </summary>
    public class MyAssessmentList : CarBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                var pageSize = Convert.ToInt32(context.Request["page_size"]);
                var pageIndex = Convert.ToInt32(context.Request["page_index"]);

                int totalCount = 0;

                pageSize = pageSize == 0 ? 20 : pageSize;
                pageIndex = pageIndex == 0 ? 1 : pageIndex;

                var list = bllCommRelation.GetRelationList(out totalCount, BLLJIMP.Enums.CommRelationType.CarServerOrderRateScore, bll.GetCurrUserID(), "", pageIndex, pageSize,"","","","","","","");

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    //查询订单 获取商家名称
                    var orderId = Convert.ToInt32( item.RelationId);

                    var order = bll.GetCarServerOrderDetail(orderId);

                    returnList.Add(new
                    {
                        saller = order.Saller,
                        order_id = orderId,
                        createTime = item.RelationTime.ToString("yyyy-MM-dd HH:mm"),
                        comment = item.Ex2
                    });
                }

                resp.returnObj = new
                {
                    total_count = totalCount,
                    list = returnList
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

    }
}
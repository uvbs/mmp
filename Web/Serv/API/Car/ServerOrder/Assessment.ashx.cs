using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerOrder
{
    /// <summary>
    /// 评价订单：
    /// 每个订单原则上只允许评价一次
    /// </summary>
    public class Assessment : CarBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    resp.errmsg = "未登录";
                    bll.ContextResponse(context, resp);
                    return;
                }

                var orderId = Convert.ToInt32(context.Request["orderId"]);
                var score = Convert.ToInt32(context.Request["score"]);
                var comment = context.Request["comment"];
                //reputationScore serviceAttitudeScore comment

                var order = bll.GetCarServerOrderDetail(orderId);

                if (order == null)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "订单不存在";
                    bll.ContextResponse(context, resp);
                    return;
                }

                resp.isSuccess = bll.RateCarServerOrder(bll.GetCurrUserID(), orderId, score, comment);

                if (resp.isSuccess)
                {
                    //更改订单评价状态
                    order.CommentStatus = 1;
                    bll.Update(order);
                }

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
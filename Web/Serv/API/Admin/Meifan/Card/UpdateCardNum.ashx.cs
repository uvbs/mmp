using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Card
{
    /// <summary>
    /// UpdateEnable 的摘要说明
    /// </summary>
    public class UpdateCardNum : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 订单
        /// </summary>
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string cardNum = context.Request["card_num"];

            var orderPay = bllOrder.GetOrderPay(orderId);

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();


            if (bll.Update(new MeifanMyCard(), string.Format(" CardNum='{0}'", cardNum), string.Format("  UserId='{0}' And CardId='{1}' And CardNum!='{2}'", orderPay.UserId, orderPay.RelationId, cardNum), tran) > 0)
            {
                orderPay.Ex3 = cardNum;
                if (bll.Update(orderPay, tran))
                {
                    tran.Commit();
                    apiResp.status = true;
                    apiResp.msg = "ok";
                }
                else
                {
                    tran.Rollback();
                }

            }
            else
            {
                tran.Rollback();
                apiResp.msg = "操作失败";
            }

            bll.ContextResponse(context, apiResp);


        }

    }
}
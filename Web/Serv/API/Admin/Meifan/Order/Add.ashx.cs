using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Order
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        public void ProcessRequest(HttpContext context)
        {
            string cardId = context.Request["card_id"];
            var userId=context.Request["user_id"];
            if (string.IsNullOrEmpty(cardId))
            {
                apiResp.msg = "card_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }

            var orderModel=bll.Get<OrderPay>(string.Format(" UserId='{0}' And Status=1 And Type=8 And RelationId='{1}'", userId, cardId));

            if (orderModel != null)
            {
                if (DateTime.Now< DateTime.Parse(orderModel.Ex4))
                {
                    apiResp.msg = "重复订单";
                    bll.ContextResponse(context, apiResp);
                    return;
                }

                
            }

            MeifanCard card = bll.GetCard(cardId);
            if (card == null)
            {
                apiResp.msg = "card_id 错误";
                bll.ContextResponse(context, apiResp);
                return;
            }
            OrderPay order = new OrderPay();
            order.WebsiteOwner = bll.WebsiteOwner;
            order.InsertDate = DateTime.Now;
            order.UserId = userId;
            order.Type = "8";
            order.OrderId = bll.GetGUID(BLLJIMP.TransacType.CommAdd);
            order.Total_Fee = card.Amount + card.ServerAmount;
            order.Subject = card.CardName;
            order.PayType = 0;
            order.Status = 1;
            order.RelationId = card.CardId;
            order.Ex1 = "0";
            order.Ex5 = "1";
            if (bll.Add(order))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
                //apiResp.result = new
                //{
                //    order_id = order.OrderId
                //};
            }
            else
            {
                apiResp.msg = "生成订单失败";
            }
            bll.ContextResponse(context, apiResp);


        }
        
    }
}
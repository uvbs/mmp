using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Order
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                apiResp.msg = "order_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var data = bllOrder.GetOrderPay(orderId);
            if (data == null || data.UserId != CurrentUserInfo.UserID)
            {
                apiResp.msg = "order_id 错误";
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            var order = GetOrderInfo(data);
            apiResp.result = new
            {
                order_id=data.OrderId,
                img_url = order.ImgUrl,
                title = order.Title,
                summary = order.Summary,
                time = data.InsertDate.ToString("yyyy-MM-dd HH:mm"),
                amount = data.Total_Fee,
                pre_amount=0,
                is_pay =data.Status,
                pay_type = "weixin",
                type = "applycard"
            };

            bll.ContextResponse(context, apiResp);

        }
        /// <summary>
        /// 订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private Order GetOrderInfo(OrderPay order)
        {
            Order model = new Order();
            switch (order.Type)
            {
                case "8":
                    MeifanCard card = bll.GetCard(order.RelationId);
                    if (card != null)
                    {
                        model.Title = card.CardName;
                        model.ImgUrl = card.CardImg;
                        model.Summary = card.CardNameEn;
                    }
                    break;
                default:
                    break;
            }
            return model;



        }
        /// <summary>
        /// 订单信息转换
        /// </summary>
        public class Order
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string ImgUrl { get; set; }
            /// <summary>
            /// 概要
            /// </summary>
            public string Summary { get; set; }

        }


    }
}
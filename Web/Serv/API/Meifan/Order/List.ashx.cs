using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Order
{
    /// <summary>
    /// 我的订单列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
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

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount;
            var data = bllOrder.GetOrderPayList(pageSize, pageIndex, "1", "8", "", "", out totalCount, bllOrder.WebsiteOwner, CurrentUserInfo.UserID,"");
            var list = from p in data
                       select new
                       {
                           order_id=p.OrderId,
                           img_url = GetOrderInfo(p).ImgUrl,
                           title = GetOrderInfo(p).Title,
                           summary = GetOrderInfo(p).Summary,
                           time = p.InsertDate.ToString("yyyy-MM-dd HH:mm"),
                           amount = p.Total_Fee,
                           pre_amount=0,
                           type="applycard"

                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
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
                    if (card!=null)
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
        public class Order {
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
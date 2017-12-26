using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Order
{
    /// <summary>
    /// 订单列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();
        public void ProcessRequest(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string status=context.Request["status"];
            string keyWord=context.Request["keyword"];
            string channel=context.Request["channel"];
            int totalCount;
            var data = bllOrder.GetOrderPayList(pageSize, pageIndex, "1", "8", status, "", out totalCount, bllOrder.WebsiteOwner, "",keyWord,channel);

            List<OrderModel> list = new List<OrderModel>();
            foreach (var item in data)
            {
                OrderModel model = new OrderModel();
                model.order_id = item.OrderId;
                model.time = item.InsertDate.ToString("yyyy-MM-dd HH:mm");
                model.status = item.Status;
                model.amount = item.Total_Fee;
                if (item.Type == "8")
                {
                    BLLJIMP.Model.MeifanCard card = bll.GetCard(item.RelationId);
                    if (card != null)
                    {
                        model.title = card.CardName;
                        model.img_url = card.CardImg;
                        model.summary = card.CardNameEn;
                        model.type = "applycard";
                        model.card_type = card.CardType;
                        model.card_number = item.Ex3;
                        model.channel = item.Ex5;
                        //Ex1 开卡状态
                        //Ex2 过期时间
                        //Ex3 主卡Id
                        //Ex4 我的卡券编号



                    }
                }
                model.apply_card_status = item.Ex1;
                model.valid_date = item.Ex2;
                model.valid_to_date = item.Ex4;

                model.relation_id = item.RelationId;
                model.user_id = item.UserId;
                var userInfo = bllUser.GetUserInfo(item.UserId);
                if (userInfo!=null)
                {
                    model.show_name = userInfo.TrueName;
                    model.show_phone = userInfo.Phone;
                }
                list.Add(model);

            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
            };
            bll.ContextResponse(context, apiResp);

        }

        public class OrderModel
        {
            /// <summary>
            /// 订单号
            /// </summary>
            public string order_id { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string img_url { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 概要
            /// </summary>
            public string summary { get; set; }
            /// <summary>
            /// 时间
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public decimal amount { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 支付状态
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 申请开卡状态
            /// 0 未开卡
            /// 1 已开卡
            /// </summary>
            public string apply_card_status { get; set; }
            /// <summary>
            /// 生效日期
            /// </summary>
            public string valid_date { get; set; }
            /// <summary>
            /// 用户账号
            /// </summary>
            public string user_id { get; set; }
            /// <summary>
            /// 用户账号
            /// </summary>
            public string relation_id { get; set; }
            /// <summary>
            /// 会员卡类型
            /// </summary>
            public string card_type { get; set; }
            /// <summary>
            /// 会员卡号
            /// </summary>
            public string card_number { get; set; }
            /// <summary>
            /// 关联会员
            /// </summary>
            public string show_name { get; set; }
            /// <summary>
            /// 关联手机号
            /// </summary>
            public string show_phone { get; set; }
            /// <summary>
            /// 到期日期
            /// </summary>
            public string valid_to_date { get; set; }
            /// <summary>
            /// 购买渠道
            /// 线上
            /// 线下
            /// </summary>
            public string channel { get; set; }

        }
    }
}
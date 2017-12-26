using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Gift.Order
{

        /// <summary>
        /// 获取礼品订单信息
        /// </summary>
        public class Get : BaseHandlerNeedLoginNoAction
        {
            /// 商城BLL
            /// </summary>
            BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
            public void ProcessRequest(HttpContext context)
            {
                context.Response.ContentType = "application/json";
                string orderId=context.Request["order_id"];
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
                if (orderInfo==null)
                {
                    resp.errcode =1;
                    resp.errmsg = "订单号不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (orderInfo.OrderType!=1)
                {
                    resp.errcode =1;
                    resp.errmsg = "不是礼品订单";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (orderInfo.PaymentStatus==0)
                {
                    resp.errcode =1;
                    resp.errmsg = "礼品订单未付款";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (!string.IsNullOrEmpty(orderInfo.ParentOrderId))
                {
                    //resp.errcode =1;
                    //resp.errmsg = "不是父订单";
                    //context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    orderInfo = bllMall.GetOrderInfo(orderInfo.ParentOrderId);
                }

                WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(orderInfo.OrderID).First();
                WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                ProductSku skuInfo = bllMall.GetProductSku((int)orderDetail.SkuId);
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok",
                    order_id = orderInfo.OrderID,
                    product_name=productInfo.PName,
                    product_img_url=bllMall.GetImgUrl(productInfo.RecommendImg),
                    product_count=orderDetail.TotalCount,
                    //product_properties = bllMall.GetProductProperties(skuInfo.SkuId),
                    product_properties = "",
                    is_get_gift=IsGetGift(orderInfo.OrderID),
                    receiver_list = GetReceiverList(orderInfo.OrderID),
                    send_user_info = GiftOrderUserInfo(orderInfo.OrderUserID)
                }));


            }

            /// <summary>
            /// 是否已经领取过
            /// </summary>
            /// <param name="orderId"></param>
            /// <returns></returns>
            public int IsGetGift(string orderId) {

                if (CurrentUserInfo!=null)
                {
                    return bllMall.GetCount<WXMallOrderInfo>(string.Format(" OrderUserID='{0}' And ParentOrderId='{1}'", CurrentUserInfo.UserID, orderId));
                    
                }
                return 0;
            
            
            }

            /// <summary>
            /// 获取收货人列表
            /// </summary>
            /// <param name="orderId"></param>
            /// <returns></returns>
            public object GetReceiverList(string orderId)
            {
                List<object> list = new List<object>();
                var orderList=bllMall.GetList<WXMallOrderInfo>(string.Format("ParentOrderId='{0}'  And OrderType=1 And WebsiteOwner='{1}'", orderId,bllMall.WebsiteOwner));
                foreach (var order in orderList)
                {
                    UserInfo userInfo = bllUser.GetUserInfo(order.OrderUserID);
                    list.Add(new
                    {
                        head_img_url=userInfo.WXHeadimgurl,
                        receiver_name=order.Consignee


                    });
                }
                return list;


            }
            /// <summary>
            /// 礼品下单人信息
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public object GiftOrderUserInfo(string userId) {
                UserInfo userInfo = bllUser.GetUserInfo(userId);
                return new
                {
                    head_img_url = userInfo.WXHeadimgurl,
                    nick_name=userInfo.WXNickname
                };
            
            }


        }

    
}
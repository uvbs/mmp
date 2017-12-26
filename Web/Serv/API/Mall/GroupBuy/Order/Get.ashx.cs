using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.GroupBuy.Order
{


    /// <summary>
    /// 拼团订单信息
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
            {
                string orderId = context.Request["order_id"];
                if (string.IsNullOrEmpty(orderId))
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "order_id 必传";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
                if (orderInfo == null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "订单号不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (orderInfo.OrderType != 2)
                {

                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "不是拼团订单";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (!string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
                {
                    orderInfo = bllMall.GetOrderInfo(orderInfo.GroupBuyParentOrderId);
                }
                if (orderInfo.PaymentStatus == 0)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "团长订单未付款";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(orderInfo.OrderID).First();
                WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                ProductSku skuInfo = bllMall.GetProductSku((int)orderDetail.SkuId);
                decimal kk = Math.Round((decimal)productInfo.Price * (decimal)(orderInfo.MemberDiscount / 10), 2);
                apiResp.result=
                 new
                {
                    order_id = orderInfo.OrderID,
                    product_id=productInfo.PID,
                    product_name = productInfo.PName,
                    summary = productInfo.Summary,
                    priduct_price=productInfo.Price,
                    product_price = productInfo.Price,
                    product_img_url = bllMall.GetImgUrl(productInfo.RecommendImg),
                    product_properties = bllMall.GetProductProperties(skuInfo.SkuId),
                    product_desc = productInfo.PDescription,
                    is_join= IsJoin(orderInfo),
                    people_list = GetPeopleList(orderInfo),
                    head_discount = orderInfo.HeadDiscount,
                    head_price = Math.Round((decimal)productInfo.Price * (decimal)(orderInfo.HeadDiscount / 10), 2),
                    member_discount = orderInfo.MemberDiscount,
                    member_price = Math.Round((decimal)productInfo.Price * (decimal)(orderInfo.MemberDiscount / 10), 2),
                    people_count = orderInfo.PeopleCount,
                    pay_people_count = GetPayPeopleCount(orderInfo),
                    expire_day = orderInfo.ExpireDay,
                    is_head=IsHead(orderInfo),
                    group_buy_status=GetGroupBuyStatus(orderInfo),
                    end_time = bllMall.GetTimeStamp(((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay)),
                    pay_order_id=GetNeedPayOrderId(orderInfo),
                    score = productInfo.Score,
                    is_cashpay_only = productInfo.IsCashPayOnly,
                    is_no_express = productInfo.IsNoExpress,
                    sku_id = skuInfo.SkuId,
                    ex10=orderInfo.Ex10
                };
                apiResp.status=true;
                apiResp.msg="ok";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

               

            }

        /// <summary>
        /// 是否已经参加
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int IsJoin(WXMallOrderInfo orderInfo)
        {
            if (IsHead(orderInfo)==1)
            {
                return 1;
            }
            else
            {
                if (bllMall.GetCount<WXMallOrderInfo>(string.Format(" OrderUserID='{0}' And GroupBuyParentOrderId='{1}'", CurrentUserInfo.UserID, orderInfo.OrderID))>0)
                {
                    return 1;
                }
               
            }
            return 0;
            
              

            
          


        }
        /// <summary>
        /// 是否团长
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public int IsHead(WXMallOrderInfo orderInfo) {

            if (CurrentUserInfo.UserID==orderInfo.OrderUserID)
            {
                return 1;
            }
            return 0;
        
        }

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<object> GetPeopleList(WXMallOrderInfo orderInfo)
        {

            List<object> list = new List<object>();
            List<WXMallOrderInfo> orderList = new List<WXMallOrderInfo>();
            if (orderInfo != null && orderInfo.Ex10 == "1")//系统开团
            {
                orderList = bllMall.GetList<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}'  And OrderType=2 And WebsiteOwner='{1}'  And PaymentStatus=1 order by InsertDate ASC", orderInfo.OrderID, bllMall.WebsiteOwner));
            }
            else
            {
                orderList = bllMall.GetList<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}'  And OrderType=2 And WebsiteOwner='{1}'  And PaymentStatus=1 Or OrderId='{0}' order by InsertDate ASC", orderInfo.OrderID, bllMall.WebsiteOwner));
            }
            
            foreach (var order in orderList)
            {
                UserInfo userInfo = bllUser.GetUserInfo(order.OrderUserID);
                list.Add(new
                {
                    head_img_url = bllUser.GetUserDispalyAvatar(userInfo),
                    show_name = userInfo.WXNickname!=null?userInfo.WXNickname:"",
                    pay_time = bllMall.GetTimeStamp((DateTime)order.PayTime),
                    user_id = userInfo.UserID,

                });
            }
            return list;


        }

        /// <summary>
        /// 获取已经参加拼团的人数
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetPayPeopleCount(WXMallOrderInfo order)
        {
            if (order.Ex10 == "1")
            {
                return bllMall.GetCount<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}'  And OrderType=2 And WebsiteOwner='{1}'  And PaymentStatus=1  ", order.OrderID, bllMall.WebsiteOwner));
            }
            else
            {
                return bllMall.GetCount<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}'  And OrderType=2 And WebsiteOwner='{1}'  And PaymentStatus=1 Or (OrderId='{0}' And PaymentStatus=1) ", order.OrderID, bllMall.WebsiteOwner));
            }
            
            
        }
      


        /// <summary>
        /// 获取拼团状态
        /// 0 拼团中
        /// 1 拼团成功
        /// 2 拼团过期失败
        /// </summary>
        /// <param name="orderInfo">团长订单</param>
        /// <returns></returns>
        public int GetGroupBuyStatus(WXMallOrderInfo orderInfo) {

            if (orderInfo.GroupBuyStatus=="1")
            {
                return 1;
            }
            int status = -1;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("  PaymentStatus=1  ");
            if (orderInfo.Ex10 == "1")
            {
                sbSql.AppendFormat(" And GroupBuyParentOrderId='{0}' ", orderInfo.OrderID);
            }
            else
            {
                sbSql.AppendFormat(" And (GroupBuyParentOrderId='{0}' Or OrderId='{0}') ", orderInfo.OrderID);
            }
            if ((bllMall.GetCount<WXMallOrderInfo>(sbSql.ToString()) < orderInfo.PeopleCount) && (DateTime.Now < ((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay)))
            {
                return 0;//拼团中
            }
            else if (bllMall.GetCount<WXMallOrderInfo>(sbSql.ToString())>=orderInfo.PeopleCount)
            {
                return 1;//拼团成功
            }
            if (DateTime.Now>=((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay))
            {
                return 2;//拼团过期失败
            }
            return status;
        }

       
        /// <summary>
        /// 是否需要继续支付
        /// </summary>
        /// <param name="orderInfo">团长订单</param>
        /// <returns>需要支付的订单号</returns>
        private string GetNeedPayOrderId(WXMallOrderInfo orderInfo)
        {
           
            if (GetGroupBuyStatus(orderInfo)==0)//拼团中
            {
                var myOrderInfo = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And PaymentStatus=0 And GroupBuyParentOrderId='{1}' And OrderUserId='{2}'",bllMall.WebsiteOwner,orderInfo.OrderID,CurrentUserInfo.UserID));
                if (myOrderInfo!=null)
                {
                    return myOrderInfo.OrderID;
                }

            }
            return "";
        
        
        }


    }




}
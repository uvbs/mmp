using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model.API.Mall;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Enums;


namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Gift.Giving
{
    /// <summary>
    /// 接收礼品
    /// </summary>
    public class ReceiveGift : BaseHandlerNeedLoginNoAction
    {
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
            Open.HongWareSDK.MemberInfo hongWeiWareMemberInfo = null;
            if (websiteInfo.IsUnionHongware == 1)
            {
                
                hongWeiWareMemberInfo = hongWareClient.GetMemberInfo(CurrentUserInfo.WXOpenId);
            }
            string data = context.Request["data"];
            decimal productFee = 0;//商品总价格 不包含邮费
            OrderRequestModel orderRequestModel = new OrderRequestModel();//订单模型
            try
            {
                orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderRequestModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            //父订单
            WXMallOrderInfo parentOrderInfo = bllMall.GetOrderInfo(orderRequestModel.order_id);
            if (parentOrderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "父订单不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //检查是否可以生成订单
            if (parentOrderInfo.PaymentStatus == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "父订单未付款";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllMall.GetCount<WXMallOrderInfo>(string.Format("  ParentOrderId='{0}'  And OrderType=1 And OrderUserId='{1}'", parentOrderInfo.OrderID, CurrentUserInfo.UserID)) > 0)
            {
                resp.errcode = 1;
                resp.errmsg = "您已经领取过了";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var parentOrderDetail = bllMall.Get<WXMallOrderDetailsInfo>(string.Format(" OrderID='{0}'", parentOrderInfo.OrderID));
            if (bllMall.GetCount<WXMallOrderInfo>(string.Format(" ParentOrderId='{0}' And OrderID !='{0}' And OrderType=1", parentOrderInfo.OrderID)) >= parentOrderDetail.TotalCount)
            {
                resp.errcode = 1;
                resp.errmsg = "礼品已经领完";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;

            }

            #region 格式检查
            //相关检查
            if (string.IsNullOrEmpty(orderRequestModel.receiver_name))
            {
                resp.errcode = 1;
                resp.errmsg = "收货人姓名不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_phone))
            {
                resp.errcode = 1;
                resp.errmsg = "收货人联系电话不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_province))
            {
                resp.errcode = 1;
                resp.errmsg = "省份名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_province_code))
            {
                resp.errcode = 1;
                resp.errmsg = "省份代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_city))
            {
                resp.errcode = 1;
                resp.errmsg = "城市名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_city_code))
            {
                resp.errcode = 1;
                resp.errmsg = "城市代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_dist))
            {
                resp.errcode = 1;
                resp.errmsg = "城市区域名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_dist_code))
            {
                resp.errcode = 1;
                resp.errmsg = "城市区域代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_address))
            {
                resp.errcode = 1;
                resp.errmsg = "收货地址不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
 
            #endregion


            //检查是否可以生成订单

            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.Address = orderRequestModel.receiver_address;
            orderInfo.Consignee = orderRequestModel.receiver_name;
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = CurrentUserInfo.UserID;
            orderInfo.Phone = orderRequestModel.receiver_phone;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.Transport_Fee = 0;
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            if (bllMall.WebsiteOwner != "mixblu")
            {
                orderInfo.OutOrderId = orderInfo.OrderID;
            }
            orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            orderInfo.ReceiverProvince = orderRequestModel.receiver_province;
            orderInfo.ReceiverProvinceCode = orderRequestModel.receiver_province_code.ToString();
            orderInfo.ReceiverCity = orderRequestModel.receiver_city;
            orderInfo.ReceiverCityCode = orderRequestModel.receiver_city_code.ToString();
            orderInfo.ReceiverDist = orderRequestModel.receiver_dist;
            orderInfo.ReceiverDistCode = orderRequestModel.receiver_dist_code.ToString();
            orderInfo.ZipCode = orderRequestModel.receiver_zip;
            orderInfo.Status = "待发货";
            orderInfo.PaymentStatus = 1;
            orderInfo.PayTime = DateTime.Now;
            orderInfo.LastUpdateTime = DateTime.Now;
            orderInfo.ParentOrderId = parentOrderInfo.OrderID;
            orderInfo.OrderType = 1;
            orderInfo.PaymentType = parentOrderInfo.PaymentType;
            orderInfo.LastUpdateTime = DateTime.Now;
            orderInfo.DistributionStatus = 1;
            #region 订单详情生成
            ///订单详情
            WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
            detailModel.OrderID = orderInfo.OrderID;
            detailModel.PID = parentOrderDetail.PID;
            detailModel.TotalCount = 1;
            detailModel.OrderPrice = parentOrderDetail.OrderPrice;
            detailModel.ProductName = parentOrderDetail.ProductName;
            detailModel.SkuId = parentOrderDetail.SkuId;
            detailModel.SkuShowProp = parentOrderDetail.SkuShowProp;
            
            #endregion

            productFee = (decimal)detailModel.OrderPrice;

            //物流费用


            //合计计算
            orderInfo.Product_Fee = productFee;
            orderInfo.TotalAmount = orderInfo.Product_Fee;
            detailModel.MaxRefundAmount = orderInfo.TotalAmount;
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderInfo, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "提交失败";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                #region 插入订单详情
                if (!this.bllMall.Add(detailModel, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "提交失败";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                #endregion
                tran.Commit();//提交订单事务

                #region 宏巍通知
                if (websiteInfo.IsUnionHongware == 1)
                {
                    hongWareClient.OrderNotice(CurrentUserInfo.WXOpenId, orderInfo.OrderID);

                }
                #endregion

                #region 微信模板消息
                try
                {

                    UserInfo parentUserInfo = bllUser.GetUserInfo(parentOrderInfo.OrderUserID);
                    string name = CurrentUserInfo.WXNickname;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = orderInfo.Consignee;
                    }
                    bllWeixin.SendTemplateMessageNotifyComm(parentUserInfo, "礼品领取通知", string.Format("{0}已领取了您赠送的礼品{1}", name, parentOrderDetail.ProductName));

                }
                catch
                {


                }
                #endregion

            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                resp.errcode = 1;
                resp.errmsg = ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                errmsg = "ok"
            }));


        }


        /// <summary>
        /// 订单模型
        /// </summary>
        private class OrderRequestModel
        {
            /// <summary>
            /// 订单号
            /// </summary>
            public string order_id { get; set; }
            /// <summary>
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }
            /// <summary>
            /// 收货人省份
            /// </summary>
            public string receiver_province { get; set; }
            /// <summary>
            /// 收货人省份代码
            /// </summary>
            public string receiver_province_code { get; set; }
            /// <summary>
            /// 收货人城市名称
            /// </summary>
            public string receiver_city { get; set; }
            /// <summary>
            /// 收货人城市代码
            /// </summary>
            public string receiver_city_code { get; set; }

            /// <summary>
            /// 收货人区域名称
            /// </summary>
            public string receiver_dist { get; set; }
            /// <summary>
            /// 收货人区域代码
            /// </summary>
            public string receiver_dist_code { get; set; }
            /// <summary>
            /// 街道地址
            /// </summary>
            public string receiver_address { get; set; }
            /// <summary>
            /// 收货人姓名
            /// </summary>
            public string receiver_name { get; set; }
            /// <summary>
            /// 收货人邮编
            /// </summary>
            public string receiver_zip { get; set; }
            /// <summary>
            /// 收货人电话
            /// </summary>
            public string receiver_phone { get; set; }

        }
    }

}
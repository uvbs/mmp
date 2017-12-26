using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Order
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class Get : BaseHanderOpen
    {
        /// <summary>
        /// 商城逻辑BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        public void ProcessRequest(HttpContext context)
        {

            string orderSn = context.Request["order_sn"];
            if (string.IsNullOrEmpty(orderSn))
            {
                resp.msg = "order_sn 参数必传";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo == null)
            {
                resp.msg = "order_sn 不存在";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            OrderListModel model = new OrderListModel();
            model.order_sn = orderInfo.OutOrderId;

            var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            if (orderUserInfo != null)
            {
                model.wx_openid = orderUserInfo.WXOpenId;
            }
            model.user_id = orderInfo.OrderUserID;
            model.user_name = orderInfo.Consignee;
            if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
            {
                var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderInfo.OrderUserID);
                if (myCardCoupon != null)
                {
                    var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
                    if (cardCoupon != null)
                    {
                        model.cardcoupon_main_id = myCardCoupon.CardId;
                        model.cardcoupon_no = myCardCoupon.CardCouponNumber;
                        model.cardcoupon_name = cardCoupon.Name;
                        model.cardcoupon_amount = orderInfo.CardcouponDisAmount;
                    }
                }


            }
            model.use_score = orderInfo.UseScore;
            model.score_exchang_amount = orderInfo.ScoreExchangAmount;
            model.use_amount = orderInfo.UseAmount;
            model.payable_amount = orderInfo.PayableAmount;
            model.total_amount = orderInfo.TotalAmount;
            model.is_appeal = orderInfo.Ex8 == "1" ? 1 : 0;
            model.appeal_content = orderInfo.Ex9;
            model.is_apply_deposit = orderInfo.Ex10 == "1" ? 1 : 0;
            model.is_apply_refund = orderInfo.Ex11 == "1" ? 1 : 0;
            if (orderInfo.IsRefund == 1)
            {
                model.is_apply_refund = 1;
            }
            model.order_status = orderInfo.Status;
            model.shipping_type = orderInfo.DeliveryType;
            model.get_address_id = orderInfo.Ex3;
            model.get_address_name = orderInfo.Ex4;
            model.get_address = orderInfo.Ex5;
            model.departure_date = orderInfo.Ex1;
            model.backhome_date = orderInfo.Ex2;
            model.order_time = orderInfo.InsertDate.ToString();
            model.receiver_name = orderInfo.Consignee;
            model.receiver_phone = orderInfo.Phone;
            model.receiver_tel = orderInfo.Tel;
            model.receiver_email = orderInfo.Email;
            model.receiver_province = orderInfo.ReceiverProvince;
            model.receiver_city = orderInfo.ReceiverCity;
            model.receiver_dist = orderInfo.ReceiverDist;
            model.receiver_address = orderInfo.Address;
            model.receiver_zip = orderInfo.ZipCode;
            model.is_pay = orderInfo.PaymentStatus;
            model.express_company_code = orderInfo.ExpressCompanyCode;
            model.express_company_name = orderInfo.ExpressCompanyName;
            model.express_number = orderInfo.ExpressNumber;
            model.delivery_time = (orderInfo.DeliveryTime != null) ? ((DateTime)orderInfo.DeliveryTime).ToString("yyyy-MM-dd HH:mm:ss") : "";
            model.last_update_time = (orderInfo.LastUpdateTime != null) ? ((DateTime)orderInfo.LastUpdateTime).ToString("yyyy-MM-dd HH:mm:ss") : "";
            model.pay_time = (orderInfo.PayTime != null) ? ((DateTime)orderInfo.PayTime).ToString("yyyy-MM-dd HH:mm:ss") : "";
            model.order_type = orderInfo.OrderType;
            switch (model.order_type)
            {
                case 1://礼品订单
                    model.parent_order_sn = orderInfo.ParentOrderId;
                    break;
                case 2://拼团订单
                    model.parent_order_sn = orderInfo.GroupBuyParentOrderId;
                    break;
                default:
                    break;
            }
            model.freight = orderInfo.Transport_Fee;
            model.pay_tran_number = orderInfo.PayTranNo;
            model.store_name = !string.IsNullOrEmpty(orderInfo.SupplierName) ? orderInfo.SupplierName : "";
            model.store_address = orderInfo.StoreAddress;
            model.claim_arrival_time = !string.IsNullOrEmpty(orderInfo.ClaimArrivalTime) ? orderInfo.ClaimArrivalTime : "";
            model.product_list = new List<OrderProductModel>();
            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            foreach (var orderDetail in orderDetailList)
            {

                WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                OrderProductModel myProductModel = new OrderProductModel();
                myProductModel.product_id = productInfo.PID;
                myProductModel.product_sn = productInfo.ProductCode;
                myProductModel.product_name = productInfo.PName;
                myProductModel.count = orderDetail.TotalCount;
                myProductModel.price = (decimal)orderDetail.OrderPrice;
                myProductModel.quote_price = productInfo.PreviousPrice;
                try
                {
                    if ((!string.IsNullOrEmpty(orderInfo.Ex2)) && (!string.IsNullOrEmpty(orderInfo.Ex1)))
                    {
                        myProductModel.day = (DateTime.Parse(orderInfo.Ex2) - DateTime.Parse(orderInfo.Ex1)).TotalDays + 1;
                    }

                }
                catch (Exception)
                {

                    break;
                }
                myProductModel.total_price =myProductModel.price * myProductModel.count;
                myProductModel.sku_id = orderDetail.SkuId;
                myProductModel.show_property = orderDetail.SkuShowProp;
                myProductModel.refund_status = bllMall.GetRefundStatus(orderDetail);
                model.product_list.Add(myProductModel);
            }

            var userInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            if (userInfo!=null)
            {
                model.weixin_open_id = userInfo.WXOpenId;
            }
            

            resp.status = true;
            resp.msg = "ok";
            resp.result = model;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        /// <summary>
        /// 订单列表模型
        /// </summary>
        public class OrderListModel
        {
            /// <summary>
            ///订单号
            /// </summary>
            public string order_sn { get; set; }
            /// <summary>
            /// 用户id
            /// </summary>
            public string user_id { get; set; }
            /// <summary>
            /// openid
            /// </summary>
            public string wx_openid { get; set; }
            /// <summary>
            /// 用户姓名
            /// </summary>
            public string user_name { get; set; }
            /// <summary>
            /// 主卡券id
            /// </summary>
            public int cardcoupon_main_id { get; set; }
            /// <summary>
            /// 卡券编号
            /// </summary>
            public string cardcoupon_no { get; set; }
            /// <summary>
            /// 优惠券名称
            /// </summary>
            public string cardcoupon_name { get; set; }
            /// <summary>
            /// 优惠金额
            /// </summary>
            public decimal cardcoupon_amount { get; set; }
            /// <summary>
            /// 使用积分
            /// </summary>
            public int use_score { get; set; }
            /// <summary>
            /// 积分兑换的金额
            /// </summary>
            public decimal score_exchang_amount { get; set; }
            /// <summary>
            /// 使用余额
            /// </summary>
            public decimal use_amount { get; set; }
            /// <summary>
            /// 运费
            /// </summary>
            public decimal freight { get; set; }

            /// <summary>
            /// 应付金额 (商品总金额+邮费)
            /// </summary>
            public decimal payable_amount { get; set; }

            /// <summary>
            /// 实付总金额
            /// </summary>
            public decimal total_amount { get; set; }

            /// <summary>
            /// 是否已申述（待退押金状态才能申诉）0 未申诉1 已申诉
            /// </summary>
            public int is_appeal { get; set; }
            /// <summary>
            /// 申诉内容
            /// </summary>
            public string appeal_content { get; set; }

            /// <summary>
            /// 是否申请退押金0 未申请1 已申请
            /// </summary>
            public int is_apply_deposit { get; set; }

            /// <summary>
            /// 是否申请退款
            /// </summary>
            public int is_apply_refund { get; set; }

            /// <summary>
            /// 订单状态
            /// </summary>
            public string order_status { get; set; }
            /// <summary>
            /// 配送方式0免费快递1上门自提
            /// </summary>
            public int shipping_type { get; set; }

            /// <summary>
            /// 自提点ID
            /// </summary>
            public string get_address_id { get; set; }

            /// <summary>
            /// 自提点名称
            /// </summary>
            public string get_address_name { get; set; }

            /// <summary>
            /// 自提点地址
            /// </summary>
            public string get_address { get; set; }

            /// <summary>
            /// 国内出发日期(大于当天时间) 示例 2015-12-01 10:00:00
            /// </summary>
            public string departure_date { get; set; }

            /// <summary>
            /// 离境回国日期(不需要时间，大于出发时间) 示例2015-12-04  10:00:00
            /// </summary>
            public string backhome_date { get; set; }

            /// <summary>
            /// 下单时间
            /// </summary>
            public string order_time { get; set; }

            /// <summary>
            /// 收货人姓名
            /// </summary>
            public string receiver_name { get; set; }

            /// <summary>
            /// 收货人电话
            /// </summary>
            public string receiver_phone { get; set; }

            /// <summary>
            /// 收货人固话
            /// </summary>
            public string receiver_tel { get; set; }

            /// <summary>
            /// 收货人邮箱
            /// </summary>
            public string receiver_email { get; set; }

            /// <summary>
            /// 收货人省份
            /// </summary>
            public string receiver_province { get; set; }

            /// <summary>
            /// 收货人城市
            /// </summary>
            public string receiver_city { get; set; }


            /// <summary>
            /// 收货人区域
            /// </summary>
            public string receiver_dist { get; set; }


            /// <summary>
            /// 街道地址
            /// </summary>
            public string receiver_address { get; set; }

            /// <summary>
            /// 收货人邮编
            /// </summary>
            public string receiver_zip { get; set; }


            /// <summary>
            /// 是否已经付款 0未支付 1已经支付
            /// </summary>
            public int is_pay { get; set; }

            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string express_company_code { get; set; }
            /// <summary>
            /// 快递公司名称
            /// </summary>
            public string express_company_name { get; set; }
            /// <summary>
            /// 快递单号
            /// </summary>
            public string express_number { get; set; }

            /// <summary>
            /// 发货时间
            /// </summary>
            public string delivery_time { get; set; }

            /// <summary>
            /// 付款时间
            /// </summary>
            public string last_update_time { get; set; }

            /// <summary>
            /// 支付时间 
            /// </summary>
            public string pay_time { get; set; }
            /// <summary>
            /// 订单类型
            /// </summary>
            public int order_type { get; set; }
            /// <summary>
            ///父订单
            /// </summary>
            public string parent_order_sn { get; set; }
            /// <summary>
            /// 商品清单
            /// </summary>
            public List<OrderProductModel> product_list { get; set; }
            /// <summary>
            /// 微信openId
            /// </summary>
            public string weixin_open_id { get; set; }
            /// <summary>
            /// 交易流水号
            /// </summary>
            public string pay_tran_number { get; set; }
            /// <summary>
            /// 送货时间 自提时间
            /// </summary>
            public string claim_arrival_time { get; set; }
            /// <summary>
            /// 门店名称
            /// </summary>
            public string store_name { get; set; }
            /// <summary>
            /// 门店地址
            /// </summary>
            public string store_address { get; set; }


        }

        /// <summary>
        /// 商品清单模型
        /// </summary>
        public class OrderProductModel
        {
            /// <summary>
            /// 商品ID
            /// </summary>
            public string product_id { get; set; }
            /// <summary>
            /// 商品编码
            /// </summary>
            public string product_sn { get; set; }

            /// <summary>
            /// 商品名称
            /// </summary>
            public string product_name { get; set; }

            /// <summary>
            /// 原价
            /// </summary>
            public decimal quote_price { get; set; }

            /// <summary>
            /// 实价
            /// </summary>
            public decimal price { get; set; }

            /// <summary>
            /// 购买数量
            /// </summary>
            public int count { get; set; }
            /// <summary>
            /// 租用天数
            /// </summary>
            public double day { get; set; }
            /// <summary>
            /// 此件商品总金额 商品单价*数量*天数
            /// </summary>
            public decimal total_price { get; set; }
            /// <summary>
            /// SkuId
            /// </summary>
            public int? sku_id { get; set; }
            /// <summary>
            /// 示例 尺码:S;颜色:蓝色
            /// </summary>
            public string show_property { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string refund_status { get; set; }


        }
    }
}
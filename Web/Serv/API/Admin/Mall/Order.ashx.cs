using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : BaseHandlerNeedLoginAdmin
    {

        /// <summary>
        /// 商城逻辑BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();

        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// yike 
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();

        /// <summary>
        /// Efast
        /// </summary>
        BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();
        /// <summary>
        /// 快递100
        /// </summary>
        BLLJIMP.BllKuaidi100 bllKuaidi100 = new BLLJIMP.BllKuaidi100();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeiXin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 商城分销
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();

        /// <summary>
        /// 菜单权限BLL
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");

        /// <summary>
        /// 站点 BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string msg = "";
            apiResp.status = bllMall.AddOrder(context,out msg);
            apiResp.msg = msg;
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
        }


        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            string orderStatus = context.Request["order_status"];
            string keyword = context.Request["keyword"];
            string orderFromTime = context.Request["order_from_time"];
            string orderToTime = context.Request["order_to_time"];
            string orderType = context.Request["order_type"];//订单类型 0 普通订单 1礼品订单
            string giftOrderType = context.Request["gift_order_type"];//礼品订单类型 0收到的礼品订单 1 发出的礼品订单          
            //string isHideReciveGiftOrder=context.Request["is_hide_recive_gift_order"];//是否隐藏接收到的礼品订单
            string groupBuyStatus = context.Request["group_buy_status"];//
            string isShowMain = context.Request["is_show_main"];

            string groupType = context.Request["group_buy_type"];//区分团购订单中的系统开团或者用户开团
            string supplierUserId = context.Request["supplier_userid"];//供应商账户
            string keyType = context.Request["key_type"];//搜素key 
            string sort = context.Request["column_sort"];   //排序值 

            string channelUserId = context.Request["channel_user_id"];//渠道账户
            string distributionOwner = context.Request["distribution_owner"];//分销上级账户
            string userAutoId = context.Request["user_auto_id"];//用户Id

            string refundStatus=context.Request["refund_status"];//退款状态
            if (currentUserInfo.UserType == 7)
            {
                supplierUserId = currentUserInfo.UserID;
            }
            int totalCount = 0;
            var orderList = bllMall.GetOrderList(pageSize, pageIndex, keyword, out totalCount, orderStatus, "",
                orderFromTime, orderToTime, orderType, giftOrderType,
                "", "", "1", "1", groupBuyStatus, "", "", "", "", "", false, isShowMain, supplierUserId, groupType, "", keyType, sort, channelUserId, distributionOwner,userAutoId,refundStatus);
            List<OrderListModel> list = new List<OrderListModel>();
            foreach (var orderInfo in orderList)
            {
                OrderListModel model = new OrderListModel();
                model.out_order_id = orderInfo.OutOrderId;
                model.order_id = orderInfo.OrderID;
                model.order_time = bllMall.GetTimeStamp(orderInfo.InsertDate);
                model.pay_time = orderInfo.PayTime.HasValue ? bllMall.GetTimeStamp(orderInfo.PayTime) : 0;
                model.product_count = orderInfo.ProductCount;
                model.total_amount = orderInfo.TotalAmount;
                model.order_status = orderInfo.Status;
                model.is_pay = orderInfo.PaymentStatus;
                model.pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
                model.express_company_code = orderInfo.ExpressCompanyCode;
                model.express_company_name = orderInfo.ExpressCompanyName;
                model.express_number = orderInfo.ExpressNumber;
                model.order_type = orderInfo.OrderType;
                model.delivery_type = orderInfo.DeliveryType;
                model.is_refund = orderInfo.IsRefund.ToString();
                model.receiver_name = orderInfo.Consignee;
                model.receiver_phone = orderInfo.Phone;
                model.receiver_address = orderInfo.ReceiverProvince + orderInfo.ReceiverCity + orderInfo.ReceiverDist + orderInfo.Address;
                model.receiver_remark = orderInfo.OrderMemo;
                model.supplier_name = orderInfo.SupplierName;
                model.people_count = orderInfo.PeopleCount;
                model.parent_order_id = orderInfo.ParentOrderId;
                model.take_out_type = orderInfo.TakeOutType;
                if (orderInfo.TakeOutType == "Eleme")
                {
                    model.ex7 = orderInfo.Ex7;
                }
                else
                {
                    if (string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
                    {
                        if (orderInfo.Ex10 == "1")
                        {
                            model.ex7 = bllMall.GetCount<WXMallOrderInfo>(string.Format(" GroupBuyParentOrderId='{0}' AND PaymentStatus=1 ", orderInfo.OrderID)).ToString();
                        }
                        else
                        {
                            model.ex7 = bllMall.GetCount<WXMallOrderInfo>(string.Format(" (GroupBuyParentOrderId='{0}' OR OrderID='{0}')  AND PaymentStatus=1 ", orderInfo.OrderID)).ToString();
                        }

                    }
                }

                model.ex9 = orderInfo.Ex9;
                model.ex10 = orderInfo.Ex10;
                model.ex21 = orderInfo.Ex21;
                var userInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                if (userInfo != null)
                {
                    model.user_aid = userInfo.AutoID.ToString();
                }
                if (orderInfo.TakeOutType == "Eleme")
                {
                    model.user_aid = orderInfo.OrderUserID;
                }
                model.is_no_express = orderInfo.IsNoExpress;
                model.product_list = new List<OrderProductModel>();
                var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                foreach (var orderDetail in orderDetailList)
                {
                    WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                    OrderProductModel myProductModel = new OrderProductModel();
                    myProductModel.order_detail_id = orderDetail.AutoID;
                    myProductModel.count = orderDetail.TotalCount;
                    myProductModel.price = (decimal)orderDetail.OrderPrice;
                    myProductModel.score = orderDetail.OrderScore;
                    myProductModel.img_url = orderDetail.ProductImage;
                    if (string.IsNullOrEmpty(myProductModel.img_url))
                    {
                        if (orderInfo.TakeOutType == "Eleme")
                        {
                            myProductModel.img_url = "http://open-files.comeoncloud.net/img/ico/eleme_logo.jpg";
                        }
                        else
                        {
                            myProductModel.img_url = bllMall.GetImgUrl(productInfo.RecommendImg);
                        }
                    }
                    myProductModel.product_name = orderDetail.ProductName;
                    if (string.IsNullOrEmpty(myProductModel.product_name))
                    {
                        myProductModel.product_name = productInfo.PName;
                    }
                    if (orderInfo.TakeOutType != "Eleme") myProductModel.quote_price = productInfo.PreviousPrice;
                    if (orderDetail.SkuId != null)
                    {
                        myProductModel.show_property = orderDetail.SkuShowProp;
                    }
                    model.product_list.Add(myProductModel);
                }
                model.group_buy_info = new { group_buy_status = GetGroupBuyStatus(orderInfo, orderInfo.Ex10) };

                if (orderInfo.OrderType == 7 && orderInfo.PaymentStatus == 1)
                {

                    model.exam_info = bllMall.GetExamInfo(orderInfo);

                }

                list.Add(model);

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list

            });


        }

        

        /// <summary>
        /// 获取订单详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string orderId = context.Request["order_id"];
            string childOrderId=context.Request["child_order_id"];
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null || (orderInfo.WebsiteOwner != bllMall.WebsiteOwner))
            {
                resp.errcode = 1;
                resp.errmsg = "无效订单";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            OrderDetailModel model = new OrderDetailModel();
            model.buyer_memo = orderInfo.OrderMemo;
            if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
            {
                var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderInfo.OrderUserID);
                if (myCardCoupon != null && orderInfo.CouponType == 0)
                {
                    var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
                    if (cardCoupon != null)
                    {
                        model.cardcoupon = cardCoupon.Name;
                        model.cardcoupon_number = myCardCoupon.CardCouponNumber;
                    }
                }
                else
                {
                    var record = bllMall.Get<StoredValueCardRecord>(string.Format(" AutoId={0} ", orderInfo.MyCouponCardId));
                    if (record != null)
                    {
                        StoredValueCard storeValueCard = bllMall.Get<StoredValueCard>(string.Format(" AutoId={0}", record.CardId));
                        if (storeValueCard != null)
                        {
                            model.cardcoupon = storeValueCard.Name;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(model.cardcoupon))
            {
                model.cardcoupon = orderInfo.Ex1;
            }
            model.order_id = orderInfo.OrderID;
            model.out_order_id = orderInfo.OutOrderId;
            model.order_time = bllMall.GetTimeStamp(orderInfo.InsertDate);
            model.order_time_str = orderInfo.InsertDate.ToString();
            model.product_count = orderInfo.ProductCount;
            model.total_amount = orderInfo.TotalAmount;
            model.freight = orderInfo.Transport_Fee;
            model.order_status = orderInfo.Status;
            //model.pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY";

            switch (orderInfo.PaymentType)
            {
                case 1:
                    model.pay_type = "ALIPAY";
                    break;
                case 2:
                    model.pay_type = "WEIXIN";
                    break;
                case 3:
                    model.pay_type = "JDPAY";
                    break;
                default:
                    model.pay_type = "OTHER";
                    break;
            }

            model.product_count = orderInfo.ProductCount;
            model.receiver_province_code = orderInfo.ReceiverProvinceCode;
            model.receiver_province = orderInfo.ReceiverProvince;
            model.receiver_city_code = orderInfo.ReceiverCityCode;
            model.receiver_city = orderInfo.ReceiverCity;
            model.receiver_dist_code = orderInfo.ReceiverDistCode;
            model.receiver_dist = orderInfo.ReceiverDist;
            model.receiver_address = orderInfo.Address;
            model.receiver_name = orderInfo.Consignee;
            model.receiver_phone = orderInfo.Phone;
            model.use_score = orderInfo.UseScore;
            model.use_amount = orderInfo.UseAmount;
            model.is_pay = orderInfo.PaymentStatus;
            model.express_company_code = orderInfo.ExpressCompanyCode;
            model.express_company_name = orderInfo.ExpressCompanyName;
            model.express_number = orderInfo.ExpressNumber;
            model.order_type = orderInfo.OrderType;
            model.email = orderInfo.Email;
            model.delivery_type = orderInfo.DeliveryType;
            model.pay_time_str = orderInfo.PayTime != null ? ((DateTime)orderInfo.PayTime).ToString() : "";
            model.ex1 = orderInfo.Ex1;
            model.ex2 = orderInfo.Ex2;
            model.ex3 = orderInfo.Ex3;
            model.ex4 = orderInfo.Ex4;
            model.ex5 = orderInfo.Ex5;
            model.ex6 = orderInfo.Ex6;
            model.ex7 = orderInfo.Ex7;
            model.ex8 = orderInfo.Ex8;
            model.ex9 = orderInfo.Ex9;
            model.ex10 = orderInfo.Ex10;
            model.ex11 = orderInfo.Ex11;
            model.ex12 = orderInfo.Ex12;
            model.ex13 = orderInfo.Ex13;
            model.ex14 = orderInfo.Ex14;
            model.ex15 = orderInfo.Ex15;
            model.ex16 = orderInfo.Ex16;
            model.ex17 = orderInfo.Ex17;
            model.ex18 = orderInfo.Ex18;
            model.ex19 = orderInfo.Ex19;
            model.ex20 = orderInfo.Ex20;
            model.ex21 = orderInfo.Ex21;
            model.ex22 = orderInfo.Ex22;
            model.take_out_type = orderInfo.TakeOutType;
            model.score_exchang_amount = orderInfo.ScoreExchangAmount;
            model.cardcoupon_exchang_amount = orderInfo.CardcouponDisAmount;
            model.weixin_refund_status = bllMall.GetWeiXinRefundStatus(model.out_order_id, model.ex17);
            model.supplier_name = !string.IsNullOrEmpty(orderInfo.SupplierName)?orderInfo.SupplierName:"";
            model.store_address = orderInfo.StoreAddress;
            model.product_list = new List<OrderProductModel>();
            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            model.group_buy_info = new { group_buy_status = GetGroupBuyStatus(orderInfo) };
            model.distribution_offline_status = GetDistributionStatus(orderInfo);
            model.is_no_express = orderInfo.IsNoExpress;
            model.custom_creater_name = orderInfo.CustomCreaterName;
            model.custom_creater_phone = orderInfo.CustomCreaterPhone;
            if (!string.IsNullOrEmpty(orderInfo.OrderUserID))
            {
                UserInfo user = bllUser.GetUserInfo(orderInfo.OrderUserID);
                model.order_user_info = new
                {
                    true_name = user!=null?user.TrueName:"",
                    phone=user!=null?user.Phone:""
                };
            }

            if (!string.IsNullOrEmpty(orderInfo.OtherUserId))
            {
                UserInfo user = bllUser.GetUserInfo(orderInfo.OtherUserId);
                model.pay_user_info = new
                {
                    avatar = user.WXHeadimgurl,
                    nickname = user.WXNickname,
                    pay_time = bllMall.GetTimeStamp(orderInfo.PayTime)
                };
            }
            model.other_use_amount = orderInfo.OtherUseAmount;
            model.other_card_coupon_dis_amount = orderInfo.OtherCardcouponDisAmount;
            var webSite = bllWebsite.GetWebsiteInfoModelFromDataBase();

            if (!string.IsNullOrEmpty(webSite.NeedMallOrderCreaterNamePhoneRName))
            {
                model.custom_rname = webSite.NeedMallOrderCreaterNamePhoneRName;
            }
            else
            {
                model.custom_rname = "下单人";
            }
            foreach (var orderDetail in orderDetailList)
            {

                WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                OrderProductModel myProductModel = new OrderProductModel();
                myProductModel.product_id = orderDetail.PID;
                myProductModel.count = orderDetail.TotalCount;
                myProductModel.price = (decimal)orderDetail.OrderPrice;
                myProductModel.score = orderDetail.OrderScore;
                myProductModel.product_name = orderDetail.ProductName;
                if (string.IsNullOrEmpty(myProductModel.product_name))
                {
                    myProductModel.product_name = productInfo.PName;
                }
                myProductModel.img_url = orderDetail.ProductImage;
                if (string.IsNullOrEmpty(myProductModel.img_url))
                {
                    if (orderInfo.TakeOutType == "Eleme")
                    {
                        myProductModel.img_url = "http://open-files.comeoncloud.net/img/ico/eleme_logo.jpg";
                    }
                    else
                    {
                        if (productInfo!=null)
                        {
                            myProductModel.img_url = bllMall.GetImgUrl(productInfo.RecommendImg);
                        }
                        
                    }
                }
                if (orderInfo.TakeOutType != "Eleme") myProductModel.quote_price = productInfo.PreviousPrice;
                
                myProductModel.show_property = orderDetail.SkuShowProp;
                myProductModel.order_detail_id = orderDetail.AutoID;
                myProductModel.refund_status = bllMall.GetRefundStatus(orderDetail);
                myProductModel.ex1 = orderDetail.Ex1;
                myProductModel.ex2 = orderDetail.Ex2;
                myProductModel.ex3 = orderDetail.Ex3;
                myProductModel.ex4 = orderDetail.Ex4;
                if (!string.IsNullOrWhiteSpace(orderDetail.Ex5))
                {
                    List<TakeOutNotify.Model.OGroupItemSpec> spacList = ZentCloud.Common.JSONHelper.JsonToObjectList<TakeOutNotify.Model.OGroupItemSpec>(orderDetail.Ex5);
                   string spacStr = string.Empty;
                   for (int i = 0; i < spacList.Count; i++)
                   {
                       if (i == 0)
                           spacStr += spacList[i].name + "：" + spacList[i].value;
                       else
                           spacStr += ";" + spacList[i].name + "：" + spacList[i].value;
                   }
                   myProductModel.ex5 = spacStr;
                   
                }
                if (!string.IsNullOrWhiteSpace(orderDetail.Ex6))
                {
                    List<TakeOutNotify.Model.OGroupItemSpec> attrList = ZentCloud.Common.JSONHelper.JsonToObjectList<TakeOutNotify.Model.OGroupItemSpec>(orderDetail.Ex6);
                    string attrStr = string.Empty;
                    for (int i = 0; i < attrList.Count; i++)
                    {
                        if (i == 0)
                            attrStr += attrList[i].name + "：" + attrList[i].value;
                        else
                            attrStr += ";" + attrList[i].name + "：" + attrList[i].value;
                    }
                    myProductModel.ex5 = attrStr;
                }
                myProductModel.ex7 = orderDetail.Ex7;
                myProductModel.ex8 = orderDetail.Ex8;
                myProductModel.ex9 = orderDetail.Ex9;
                myProductModel.ex10 = orderDetail.Ex10;
                myProductModel.ex11 = orderDetail.Ex11;

                model.product_list.Add(myProductModel);
            }

            #region 查询礼品子订单
            if (orderInfo.OrderType == 1 && (string.IsNullOrEmpty(orderInfo.ParentOrderId)))//查询子订单
            {
                model.child_order_list = new List<OrderDetailModel>();
                foreach (var childOrder in bllMall.GetList<WXMallOrderInfo>(string.Format(" ParentOrderId='{0}' order by  OrderId ASC", orderInfo.OrderID)))
                {
                    OrderDetailModel childOrderDetail = new OrderDetailModel();
                    childOrderDetail.buyer_memo = childOrder.OrderMemo;
                    childOrderDetail.order_id = childOrder.OrderID;
                    childOrderDetail.out_order_id = childOrder.OutOrderId;
                    childOrderDetail.order_time = bllMall.GetTimeStamp(childOrder.InsertDate);
                    childOrderDetail.pay_time = bllMall.GetTimeStamp(childOrder.PayTime);
                    childOrderDetail.product_count = childOrder.ProductCount;
                    childOrderDetail.total_amount = childOrder.TotalAmount;
                    childOrderDetail.freight = childOrder.Transport_Fee;
                    childOrderDetail.order_status = childOrder.Status;
                    childOrderDetail.pay_type = childOrder.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
                    childOrderDetail.product_count = childOrder.ProductCount;
                    childOrderDetail.receiver_province_code = childOrder.ReceiverProvinceCode;
                    childOrderDetail.receiver_province = childOrder.ReceiverProvince;
                    childOrderDetail.receiver_city_code = childOrder.ReceiverCityCode;
                    childOrderDetail.receiver_city = childOrder.ReceiverCity;
                    childOrderDetail.receiver_dist_code = childOrder.ReceiverDistCode;
                    childOrderDetail.receiver_dist = childOrder.ReceiverDist;
                    childOrderDetail.receiver_address = childOrder.Address;
                    childOrderDetail.receiver_name = childOrder.Consignee;
                    childOrderDetail.receiver_phone = childOrder.Phone;
                    childOrderDetail.use_score = childOrder.UseScore;
                    childOrderDetail.is_pay = childOrder.PaymentStatus;
                    childOrderDetail.express_company_code = childOrder.ExpressCompanyCode;
                    childOrderDetail.express_company_name = childOrder.ExpressCompanyName;
                    childOrderDetail.express_number = childOrder.ExpressNumber;
                    childOrderDetail.order_type = childOrder.OrderType;
                    childOrderDetail.email = childOrder.Email;
                    childOrderDetail.delivery_type = orderInfo.DeliveryType;
                    childOrderDetail.ex1 = childOrder.Ex1;
                    childOrderDetail.ex2 = childOrder.Ex2;
                    childOrderDetail.ex3 = childOrder.Ex3;
                    childOrderDetail.ex4 = childOrder.Ex4;
                    childOrderDetail.ex5 = childOrder.Ex5;
                    childOrderDetail.ex6 = childOrder.Ex6;
                    childOrderDetail.ex7 = childOrder.Ex7;
                    childOrderDetail.ex8 = childOrder.Ex8;
                    childOrderDetail.ex9 = childOrder.Ex9;
                    childOrderDetail.ex10 = childOrder.Ex10;
                    childOrderDetail.ex11 = childOrder.Ex11;
                    childOrderDetail.ex12 = childOrder.Ex12;
                    childOrderDetail.ex13 = childOrder.Ex13;
                    childOrderDetail.ex14 = childOrder.Ex14;
                    childOrderDetail.ex15 = childOrder.Ex15;
                    childOrderDetail.ex16 = childOrder.Ex16;
                    childOrderDetail.ex17 = childOrder.Ex17;
                    childOrderDetail.weixin_refund_status = bllMall.GetWeiXinRefundStatus(model.out_order_id, model.ex17);
                    childOrderDetail.product_list = new List<OrderProductModel>();
                    var orderDetailListChild = bllMall.GetOrderDetailsList(childOrder.OrderID);
                    foreach (var orderDetail in orderDetailListChild)
                    {

                        WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                        OrderProductModel myProductModel = new OrderProductModel();
                        myProductModel.count = orderDetail.TotalCount;
                        myProductModel.product_name = orderDetail.ProductName;
                        if (string.IsNullOrEmpty(myProductModel.product_name))
                        {
                            myProductModel.product_name = productInfo.PName;
                        }
                        myProductModel.img_url = orderDetail.ProductImage;
                        if (string.IsNullOrEmpty(myProductModel.img_url))
                        {
                            myProductModel.img_url = productInfo.RecommendImg;
                        }
                        myProductModel.price = (decimal)orderDetail.OrderPrice;
                        myProductModel.quote_price = productInfo.PreviousPrice;
                        myProductModel.show_property = orderDetail.SkuShowProp;
                        myProductModel.order_detail_id = orderDetail.AutoID;
                        myProductModel.refund_status = bllMall.GetRefundStatus(orderDetail);
                        childOrderDetail.product_list.Add(myProductModel);
                    }
                    model.child_order_list.Add(childOrderDetail);

                }




            }
            #endregion

            #region 查询拼团子订单
            if (orderInfo.OrderType == 2 && (string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId)))//查询子订单  
            {
                model.group_buy_child_order_list = new List<OrderDetailModel>();
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" GroupBuyParentOrderId='{0}' And PaymentStatus=1", orderInfo.OrderID);
                if (!string.IsNullOrEmpty(childOrderId)) sbSql.AppendFormat(" AND OrderId='{0}' ", childOrderId);
                sbSql.AppendFormat("  order by  Paytime ASC ");
                foreach (var childOrder in bllMall.GetList<WXMallOrderInfo>(sbSql.ToString()))
                {
                    OrderDetailModel childOrderDetail = new OrderDetailModel();
                    childOrderDetail.buyer_memo = childOrder.OrderMemo;
                    childOrderDetail.order_id = childOrder.OrderID;
                    childOrderDetail.out_order_id = childOrder.OutOrderId;
                    childOrderDetail.order_time = bllMall.GetTimeStamp(childOrder.InsertDate);
                    childOrderDetail.pay_time = bllMall.GetTimeStamp(childOrder.PayTime);
                    childOrderDetail.product_count = childOrder.ProductCount;
                    childOrderDetail.total_amount = childOrder.TotalAmount;
                    childOrderDetail.freight = childOrder.Transport_Fee;
                    childOrderDetail.order_status = childOrder.Status;
                    childOrderDetail.pay_type = childOrder.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
                    childOrderDetail.product_count = childOrder.ProductCount;
                    childOrderDetail.receiver_province_code = childOrder.ReceiverProvinceCode;
                    childOrderDetail.receiver_province = childOrder.ReceiverProvince;
                    childOrderDetail.receiver_city_code = childOrder.ReceiverCityCode;
                    childOrderDetail.receiver_city = childOrder.ReceiverCity;
                    childOrderDetail.receiver_dist_code = childOrder.ReceiverDistCode;
                    childOrderDetail.receiver_dist = childOrder.ReceiverDist;
                    childOrderDetail.receiver_address = childOrder.Address;
                    childOrderDetail.receiver_name = childOrder.Consignee;
                    childOrderDetail.receiver_phone = childOrder.Phone;
                    childOrderDetail.use_score = childOrder.UseScore;
                    childOrderDetail.use_amount = childOrder.UseAmount;
                    childOrderDetail.is_pay = childOrder.PaymentStatus;
                    childOrderDetail.express_company_code = childOrder.ExpressCompanyCode;
                    childOrderDetail.express_company_name = childOrder.ExpressCompanyName;
                    childOrderDetail.express_number = childOrder.ExpressNumber;
                    childOrderDetail.order_type = childOrder.OrderType;
                    childOrderDetail.email = childOrder.Email;
                    childOrderDetail.delivery_type = orderInfo.DeliveryType;
                    childOrderDetail.ex1 = childOrder.Ex1;
                    childOrderDetail.ex2 = childOrder.Ex2;
                    childOrderDetail.ex3 = childOrder.Ex3;
                    childOrderDetail.ex4 = childOrder.Ex4;
                    childOrderDetail.ex5 = childOrder.Ex5;
                    childOrderDetail.ex6 = childOrder.Ex6;
                    childOrderDetail.ex7 = childOrder.Ex7;
                    childOrderDetail.ex8 = childOrder.Ex8;
                    childOrderDetail.ex9 = childOrder.Ex9;
                    childOrderDetail.ex10 = childOrder.Ex10;
                    childOrderDetail.ex11 = childOrder.Ex11;
                    childOrderDetail.ex12 = childOrder.Ex12;
                    childOrderDetail.ex13 = childOrder.Ex13;
                    childOrderDetail.ex14 = childOrder.Ex14;
                    childOrderDetail.ex15 = childOrder.Ex15;
                    childOrderDetail.ex16 = childOrder.Ex16;
                    childOrderDetail.ex17 = childOrder.Ex17;
                    childOrderDetail.weixin_refund_status = bllMall.GetWeiXinRefundStatus(model.out_order_id, model.ex17);
                    childOrderDetail.coupon_type = childOrder.CouponType;
                    childOrderDetail.product_list = new List<OrderProductModel>();
                    var orderDetailListChild = bllMall.GetOrderDetailsList(childOrder.OrderID);
                    foreach (var orderDetail in orderDetailListChild)
                    {

                        WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                        OrderProductModel myProductModel = new OrderProductModel();
                        myProductModel.product_name = orderDetail.ProductName;
                        if (string.IsNullOrEmpty(myProductModel.product_name))
                        {
                            myProductModel.product_name = productInfo.PName;
                        }
                        myProductModel.img_url = orderDetail.ProductImage;
                        if (string.IsNullOrEmpty(myProductModel.img_url))
                        {
                            myProductModel.img_url = productInfo.RecommendImg;
                        }
                        myProductModel.count = orderDetail.TotalCount;
                        myProductModel.price = (decimal)orderDetail.OrderPrice;
                        myProductModel.quote_price = productInfo.PreviousPrice;
                        //if (orderDetail.SkuId != null)
                        //{
                        myProductModel.show_property = orderDetail.SkuShowProp;
                        //myProductModel.show_property = bllMall.GetProductShowProperties((int)orderDetail.SkuId);
                        //}
                        myProductModel.order_detail_id = orderDetail.AutoID;
                        myProductModel.refund_status = bllMall.GetRefundStatus(orderDetail);
                        childOrderDetail.product_list.Add(myProductModel);
                    }
                    model.group_buy_child_order_list.Add(childOrderDetail);

                }




            }
            #endregion


            if (orderInfo.OrderType == 7 && orderInfo.PaymentStatus == 1)
            {

                model.exam_info = bllMall.GetExamInfo(orderInfo);

            }
            model.claim_arrival_time = !string.IsNullOrEmpty(orderInfo.ClaimArrivalTime) ? orderInfo.ClaimArrivalTime : "";
            return ZentCloud.Common.JSONHelper.ObjectToJson(model);


        }

        public class newSpecs
        {
            public string name { get; set; }

            public string value { get; set; }
        }


        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string orderStatus = context.Request["order_status"];
            if (orderStatus == "请选择订单状态")
            {
                resp.errcode = 1;
                resp.errmsg = "请选择订单状态";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo.PaymentStatus == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "未付款的不能更新订单状态";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            bool isSuccess = bllMall.UpdateOrderStatus(orderId, orderStatus);
            if (isSuccess)
            {
                resp.errmsg = "更新订单状态成功";
                #region 驿氪同步
                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                {

                    if (!string.IsNullOrEmpty(orderUserInfo.Phone))
                    {

                        Open.EZRproSDK.Client client = new Open.EZRproSDK.Client();
                        client.ChangeStatus(orderInfo.OrderID, orderStatus);

                    }


                }
                #endregion
                try
                {
                    string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
                    bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "订单状态更新", string.Format("订单号:{0}\\n订单状态:{1}", orderInfo.OrderID, orderStatus), url);
                }
                catch
                {


                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新订单状态失败";
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 批量更新订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateBatch(HttpContext context)
        {
            string orderIds = context.Request["order_ids"];//订单号
            string orderStatus = context.Request["order_status"];
            if (orderStatus == "请选择订单状态")
            {
                resp.errcode = 1;
                resp.errmsg = "请选择订单状态";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderStatus == "已发货")
            {
                resp.errcode = 1;
                resp.errmsg = "不能批量设置订单状态为已发货";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            foreach (var orderId in orderIds.Split(','))
            {
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
                if (orderInfo.PaymentStatus == 0) continue;
                if (orderInfo.Status == orderStatus) continue;
                if (orderInfo.WebsiteOwner != bllMall.WebsiteOwner) continue;

                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                if (bllMall.Update(orderInfo, string.Format(" Status='{0}'", orderStatus), string.Format(" OrderId='{0}'", orderId)) > 0)
                {
                    bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "订单状态更新", string.Format("订单号:{0}\\n订单状态:{1}", orderInfo.OrderID, orderStatus));
                    if (orderStatus == "交易成功")//交易成功打佣金
                    {
                        #region 分销自动打佣金
                        if (bllMenuPermission.CheckUserAndPmsKey(bllMall.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
                        {
                            string msg = "";
                            if (bllDis.Transfers(orderInfo, out msg))
                            {
                                orderInfo.DistributionStatus = 3;
                                bllMall.Update(orderInfo, string.Format(" DistributionStatus=3"), string.Format(" OrderID='{1}'", orderInfo.OrderID));
                            }

                        }
                        #endregion


                    }



                }
            }

            resp.errcode = 0;
            resp.errmsg = "ok";
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDistributionOffLine(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string orderStatus = context.Request["order_status"];
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo.DistributionStatus.ToString() == orderStatus)
            {
                resp.errcode = 1;
                resp.errmsg = "订单状态与原订单状态不能相同";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var oldDisStatus = orderInfo.DistributionStatus;
            int updateResultCount = bllMall.Update(new WXMallOrderInfo(), string.Format(" DistributionStatus={0}", orderStatus), string.Format(" OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllMall.WebsiteOwner));
            if (updateResultCount > 0)
            {

                if (orderStatus.Equals("3"))//已审核 ,给上级用户账户打款
                {
                    string msg = "";
                    if (bllDis.Transfers(orderInfo, out msg))
                    {

                        resp.errmsg = "ok";
                    }
                    else
                    {
                        bllMall.Update(new WXMallOrderInfo(), string.Format(" DistributionStatus={0}", oldDisStatus), string.Format(" OrderID='{0}' And WebsiteOwner='{1}'", orderId, bllMall.WebsiteOwner));
                        resp.errcode = 1;
                        resp.errmsg = msg;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                    }

                }

                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新分销订单状态失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //try
            //{

            //    //bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo.WXOpenId, "订单状态更新", string.Format("订单号:{0}\\n订单状态:{1}", orderInfo.OrderID, orderStatus));
            //}
            //catch
            //{


            //}


            //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新团购订单状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateGroupBuyStatus(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string groupBuyStatus = context.Request["groupbuy_status"];

            if (string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(groupBuyStatus))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "订单id和状态都必须传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //id转成字符串格式，不然某些id会报溢出


            var ids = ZentCloud.Common.StringHelper.ListToStr<string>(orderId.Split(',').ToList(), "'", ",");

            var count = bllMall.Update(
                    new WXMallOrderInfo(),
                    string.Format(" GroupBuyStatus='{0}' ", groupBuyStatus),
                    string.Format(" OrderID IN ({0}) And WebsiteOwner = '{1}' ",
                        ids,
                        bllMall.WebsiteOwner
                    )
                );

            if (count > 0)
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
                resp.result = count;
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新团购订单失败";
            }

            //WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderId);
            //orderInfo.GroupBuyStatus = groupBuyStatus;
            //if (bllMall.Update(orderInfo))
            //{
            //    resp.errcode =0;
            //    resp.errmsg = "ok";
            //}
            //else
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "更新团购订单失败";
            //}

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateExpress(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string expressCompanyCode = context.Request["express_company_code"];
            string expressCompanyName = context.Request["express_company_name"];
            string expressNumber = context.Request["express_number"];
            //if (string.IsNullOrEmpty(expressCompanyCode))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "快递代码不能为空";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}
            //if (string.IsNullOrEmpty(expressNumber))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "快递单号不能为空";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}
            //if (string.IsNullOrEmpty(expressCompanyName))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "快递公司名称不能为空";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}

            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.WebsiteOwner != bllMall.WebsiteOwner)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.PaymentStatus == 0)
            {
                resp.errcode = 1;
                resp.errmsg = "未付款的不能发货";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            orderInfo.ExpressCompanyCode = expressCompanyCode;
            orderInfo.ExpressCompanyName = expressCompanyName;
            orderInfo.ExpressNumber = expressNumber;
            orderInfo.DeliveryTime = DateTime.Now;
            orderInfo.Status = "已发货";
            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {

                resp.errmsg = "ok";
                //string msg = "";
                //bllKuaidi100.Poll(orderInfo.ExpressCompanyCode, orderInfo.ExpressNumber, out  msg);
                try
                {
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                    string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
                    bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "订单已发货", string.Format("订单号:{0}\\n订单金额:{1}", orderInfo.OrderID, orderInfo.TotalAmount), url);
                }
                catch
                {


                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新物流信息失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 批量发货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchUpdateExpress(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                apiResp.status = false;
                apiResp.msg = "请选择后缀名为.xls格式的文件";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            var postFile = context.Request.Files[0];
            String fileExt = System.IO.Path.GetExtension(postFile.FileName).ToLower();
            if (String.IsNullOrEmpty(fileExt) || (fileExt != ".xls"))
            {
                apiResp.status = false;
                apiResp.msg = "请选择后缀名为.xls格式的文件";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);
            }
            if (postFile.InputStream.Length >= 10 * 1024 * 1024)
            {
                apiResp.status = false;
                apiResp.msg = "请选择10M以下的excel文件";
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            String fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
            String savePath = "/FileUpload/BatchUpdateExpress/" + bllMall.WebsiteOwner + "/" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "/";
            String serverPath = context.Server.MapPath(savePath);
            if (!System.IO.Directory.Exists(serverPath))
            {
                System.IO.Directory.CreateDirectory(serverPath);
            }
            String serverFile = serverPath + fileName;
            try
            {
                postFile.SaveAs(serverFile);

            }
            catch (Exception ex)
            {
                apiResp.status = false;
                apiResp.msg = ex.ToString();
                return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

            }
            int successCount = 0;
            int failCount = 0;
            string msg = "";
            System.Data.DataTable dt = DataLoadTool.NPOIHelper.Import(serverFile);
            bllMall.BatchDeliver(dt, out successCount, out failCount, out msg);
            apiResp.status = true;
            apiResp.msg = msg;
            apiResp.result = new
            {
                total_count = dt.Rows.Count,
                success_count = successCount,
                fail_count = failCount
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(apiResp);

        }

        /// <summary>
        /// 开团
        /// </summary>
        /// <returns></returns>
        private string OpenGroup(HttpContext context)
        {
            string ruleIds = context.Request["rule_ids"];
            string productId = context.Request["product_id"];
            if (string.IsNullOrEmpty(productId))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "product_id 参数必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            WXMallProductInfo productInfo = bllMall.GetProduct(productId);

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            WXMallOrderInfo orderInfo = new WXMallOrderInfo();

            orderInfo = new WXMallOrderInfo();
            ProductGroupBuyRule ruleModel = bllMall.GetProductGroupBuyRule(int.Parse(ruleIds));
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            orderInfo.OrderUserID = currentUserInfo.UserID;
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.OrderType = 2;//团购订单
            orderInfo.OrderMemo = "系统开团";
            orderInfo.Ex10 = "1";//区分用户开团和系统开团  1系统开团
            orderInfo.Ex9 = productId;//
            orderInfo.Ex11 = ruleIds;//规则id
            orderInfo.PayTime = DateTime.Now;//支付时间
            orderInfo.PaymentStatus = 1;
            orderInfo.Status = "待发货";
            orderInfo.ExpireDay = ruleModel != null ? ruleModel.ExpireDay : 0;
            orderInfo.PeopleCount = ruleModel != null ? ruleModel.PeopleCount : 0;
            orderInfo.MemberDiscount = ruleModel != null ? ruleModel.MemberDiscount : 0;

            orderInfo.Ex12 = ruleModel.RuleName;//规则名称

            orderInfo.Ex13 = Math.Round((decimal)productInfo.Price * (decimal)(orderInfo.MemberDiscount / 10), 2).ToString();//成团价

            List<ProductSku> skuList = bllMall.GetProductSkuList(int.Parse(productId));

            WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
            detailModel.OrderID = orderInfo.OrderID;
            detailModel.PID = productId;
            detailModel.SkuId = skuList[0].SkuId;
            detailModel.OrderPrice = skuList[0].Price;
            detailModel.TotalCount = 1;
            detailModel.ProductName = productInfo.PName;
            if (!bllMall.Add(detailModel))
            {
                tran.Rollback();
                resp.errmsg = "插入订单详情表失败";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllMall.Add(orderInfo, tran))
            {
                tran.Commit();
                resp.errmsg = "开团成功";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "开团失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加订单备注,只有后台可以看到
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateRemark(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string remark = context.Request["remark"];
            if (string.IsNullOrEmpty(remark))
            {
                resp.errcode = 1;
                resp.errmsg = "请填写备注";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.WebsiteOwner != bllMall.WebsiteOwner)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            orderInfo.Ex21 = remark;
            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {

                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新订单备注失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 分配订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AssisnOrder(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            string supplierUserId = context.Request["supplier_user_id"];
            if (string.IsNullOrEmpty(supplierUserId))
            {
                resp.errcode = 1;
                resp.errmsg = "supplier_user_id 必传";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.WebsiteOwner != bllMall.WebsiteOwner)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var supplierUserInfo = bllUser.GetUserInfo(supplierUserId, bllUser.WebsiteOwner);
            orderInfo.SupplierUserId = supplierUserInfo.UserID;
            orderInfo.SupplierName = supplierUserInfo.Company;
            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {

                resp.errmsg = "ok";

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新订单备注失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 商品销量排序
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaleCountSort(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageIndex"]) ? int.Parse(context.Request["pageIndex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pageSize"]) ? int.Parse(context.Request["pageSize"]) : 50;
            string startDate = context.Request["start_date"];
            string stopDate = context.Request["stop_date"];


            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat(" WebsiteOwner='{0}' ", bllMall.WebsiteOwner);

            sbSql.AppendFormat(" AND Status!='已取消' ");

            sbSql.AppendFormat(" AND PaymentStatus=1 ");

            if (!string.IsNullOrEmpty(startDate))
            {
                sbSql.AppendFormat(" AND InsertDate>='{0}' ", DateTime.Parse(startDate));
            }
            if (!string.IsNullOrEmpty(stopDate))
            {
                sbSql.AppendFormat(" AND InsertDate<'{0}' ", DateTime.Parse(stopDate).AddDays(1));
            }

            string strSql = string.Format("  OrderID in (select OrderID from ZCJ_WXMallOrderInfo where {1} ) and IsComplete=1 ", pageSize, sbSql.ToString());

            List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetList<WXMallOrderDetailsInfo>(strSql);

            List<WXMallOrderDetailsInfo> disDetailList = orderDetailList.DistinctBy(p => p.PID).ToList();
            List<dynamic> returnList = new List<dynamic>();
            for (int i = 0; i < disDetailList.Count; i++)
            {
                disDetailList[i].TotalCount = orderDetailList.Where(p => p.PID == disDetailList[i].PID).Sum(p => p.TotalCount);
            }
            disDetailList = disDetailList.OrderByDescending(p => p.TotalCount).ToList();

            for (int j = 0; j < disDetailList.Count; j++)
            {
                returnList.Add(new
               {
                   sale_count = disDetailList[j].TotalCount,
                   product_name = bllMall.GetProduct(disDetailList[j].PID).PName
               });
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(returnList);
        }

        /// <summary>
        /// 获取拼团状态
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public int GetGroupBuyStatus(WXMallOrderInfo orderInfo, string groupbuyType = "")
        {
            int status = -1;
            if (orderInfo.OrderType != 2)
            {
                return status;
            }
            if (orderInfo.PayTime == null)
            {
                return status;
            }
            if (orderInfo.GroupBuyStatus == "1")
            {
                return 1;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("  PaymentStatus=1 ");
            if (!string.IsNullOrEmpty(groupbuyType) && groupbuyType == "1")
            {
                strSql.AppendFormat(" AND GroupBuyParentOrderId='{0}' ", orderInfo.OrderID);
            }
            else
            {
                strSql.AppendFormat("  And (GroupBuyParentOrderId='{0}' Or OrderId='{0}') ", orderInfo.OrderID);
            }



            if ((bllMall.GetCount<WXMallOrderInfo>(strSql.ToString()) < orderInfo.PeopleCount) && (DateTime.Now < ((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay)))
            {
                return 0;
            }
            else if (bllMall.GetCount<WXMallOrderInfo>(strSql.ToString()) >= orderInfo.PeopleCount)
            {
                if (orderInfo.GroupBuyStatus != "1")
                {
                    orderInfo.GroupBuyStatus = "1";
                    bllMall.Update(orderInfo);

                }
                return 1;//拼团成功
            }
            if (DateTime.Now >= ((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay))//拼团失败
            {
                if (orderInfo.GroupBuyStatus != "2")
                {
                    orderInfo.GroupBuyStatus = "2";
                    bllMall.Update(orderInfo);

                }
                return 2;//拼团过期失败
            }
            return status;
        }



        /// <summary>
        /// 获取分销订单状态
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public int GetDistributionStatus(WXMallOrderInfo orderInfo)
        {


            if (orderInfo.OrderType != 0)
            {
                return -1;
            }
            if (!bllMenuPermission.CheckUserAndPmsKey(bllMall.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
            {
                return -1;
            }

            return orderInfo.DistributionStatus;

        }


    }
}
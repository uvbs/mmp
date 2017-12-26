using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    ///订单
    /// </summary>
    public class Order : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// Efast
        /// </summary>
        BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 评论
        /// </summary>
        BLLJIMP.BLLReview bllReview = new BLLJIMP.BLLReview();

        /// <summary>
        /// 驿氪  
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// Efast
        /// </summary>
        Open.EfastSDK.Client efastClient = new Open.EfastSDK.Client();
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
        /// 
        /// </summary>
        BLLJIMP.BllOrder bllOrder = new BLLJIMP.BllOrder();


        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            var result = bllOrder.Add(context);

            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 代付订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            var result = bllOrder.Update(context);
            return JsonConvert.SerializeObject(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CancelUpdate(HttpContext context)
        {
            var result = bllOrder.CancelUpdate(context);
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 我的订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string status = context.Request["order_status"];//订单状态 中文字符串
            string orderFromTime = context.Request["order_from_time"];
            string orderToTime = context.Request["order_to_time"];
            string orderType = context.Request["order_type"];//订单类型 0 普通订单 1礼品订单
            string giftOrderType = context.Request["gift_order_type"];//礼品订单类型 0收到的礼品订单 1 发出的礼品订单
            string isAppeal = context.Request["is_appeal"];//是否是申诉中的订单 1是
            string isRefund = context.Request["is_refund"];//是否退款中的订单   1是
            string hasReview = context.Request["has_review"];//是否有评价
            string isShowMain=context.Request["is_show_main"];//是否显示主订单
            string groupbuyStatus=context.Request["group_buy_status"];//团购状态
            string groupBuyType = context.Request["group_buy_type"];//区分用户开团和系统开团
            string isDisableCurrUser = context.Request["is_disable_curr_user"];//是否禁用当前登录人作为条件
            int totalCount = 0;
            var orderList = bllMall.GetOrderList(pageSize, pageIndex, "", out totalCount, status, 
                currentUserInfo.UserID, orderFromTime, orderToTime, orderType,giftOrderType,
                "", "", "", "", groupbuyStatus, "", "", isAppeal, isRefund, hasReview, true, isShowMain, "", groupBuyType, isDisableCurrUser);
            List<OrderListModel> list = new List<OrderListModel>();
            foreach (var orderInfo in orderList)
            {
                OrderListModel model = new OrderListModel();
                model.order_id = orderInfo.OrderID;
                model.out_order_id = orderInfo.OutOrderId;
                model.order_time = bllMall.GetTimeStamp(orderInfo.InsertDate);
                model.order_time_str = orderInfo.InsertDate.ToString();
                if (orderInfo.DeliveryTime.HasValue)
                {
                    model.delivery_time = bllMall.GetTimeStamp((DateTime)orderInfo.DeliveryTime); 
                } 
                model.product_count = orderInfo.ProductCount;
                model.total_amount = orderInfo.TotalAmount;
                model.order_status = orderInfo.Status;
                model.is_pay = orderInfo.PaymentStatus;
                model.pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
                model.express_company_code = orderInfo.ExpressCompanyCode;
                model.express_company_name = orderInfo.ExpressCompanyName;
                model.express_number = orderInfo.ExpressNumber;
                model.order_type = orderInfo.OrderType;
                model.gift_order_type = giftOrderType;
                model.is_cansendgift = bllMall.IsCanShareGift(orderInfo);
                model.delivery_type = orderInfo.DeliveryType;
                model.review_score = orderInfo.ReviewScore;
                model.is_appointment = orderInfo.IsAppointment;
                model.is_main = orderInfo.IsMain;
                model.parent_order_id = !string.IsNullOrEmpty(orderInfo.ParentOrderId) ? orderInfo.ParentOrderId : "";
                model.supplier_name = orderInfo.SupplierName;
                model.ex9 = orderInfo.Ex9;
                model.ex10 = orderInfo.Ex10;
                model.ex11 = orderInfo.Ex11;
                model.ex12 = orderInfo.Ex12;
                model.ex13 = orderInfo.Ex13;
                model.is_refund = orderInfo.IsRefund.ToString();
                model.people_count = orderInfo.PeopleCount;
                model.member_discount = orderInfo.MemberDiscount;


                //if (orderInfo.Status != "已取消" && (orderInfo.Ex11 == "1" || !string.IsNullOrWhiteSpace(orderInfo.Ex18))){
                //    model.is_refund = "1";
                //}
                #region efast
                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                {
                    if (string.IsNullOrEmpty(model.express_company_code) && (string.IsNullOrEmpty(model.express_number)))
                    {
                        string expressCompanyCode = "";
                        string expressNumber = "";
                        var isSuccuss = bllEfast.GetOrderExpressInfo(model.order_id.ToString(), out expressCompanyCode, out expressNumber);
                        if (isSuccuss)
                        {
                            model.express_company_code = expressCompanyCode;
                            model.express_number = expressNumber;
                            if (!string.IsNullOrEmpty(model.express_company_code) && (!string.IsNullOrEmpty(model.express_number)))
                            {
                                model.order_status = "已发货";
                                var expressCompanyCodeMap = bllKuaidi100.expressCompanyMap.SingleOrDefault(p => p.Key == model.express_company_code).Value;
                                if (expressCompanyCodeMap != null)
                                {
                                    model.express_company_code = expressCompanyCodeMap;
                                }
                                else
                                {
                                    model.express_company_code = expressCompanyCode;
                                }

                                var expressInfo = bllMall.Get<ZentCloud.BLLJIMP.ModelGen.ExpressInfo>(string.Format(" ExpressCompanyCode='{0}'", model.express_company_code));
                                if (expressInfo != null)
                                {
                                    model.express_company_name = expressInfo.ExpressCompanyName;
                                }
                                bllMall.Update(orderInfo, string.Format(" Status='已发货',ExpressCompanyCode='{0}',ExpressNumber='{1}',ExpressCompanyName='{2}',DeliveryTime=GETDATE()", model.express_company_code, model.express_number, model.express_company_name), string.Format(" OrderId='{0}'", orderInfo.OrderID));

                                #region 快递订阅
                                //var map = bllKuaidi100.expressCompanyMap.SingleOrDefault(p => p.Key == expressCompanyCode);
                                //if (!string.IsNullOrEmpty(map.Key))
                                //{
                                //    expressCompanyCode = map.Value;

                                //}
                                string expressMsg = "";
                                if (!bllKuaidi100.Poll(model.express_company_code, model.express_number, out expressMsg))
                                {
                                    //订阅失败，记录日志
                                    using (StreamWriter sw = new StreamWriter(@"D:\kuaidi100log.txt", true, Encoding.GetEncoding("GB2312")))
                                    {
                                        sw.WriteLine(DateTime.Now.ToString() + "\t" + expressMsg + "\tnumber:" + model.express_number + "\tcompanycode:" + expressCompanyCode);
                                    }


                                }
                                #endregion


                                #region 驿氪同步
                                if ((!string.IsNullOrEmpty(currentUserInfo.Phone)))
                                {

                                    //驿氪同步
                                    yiKeClient.ChangeStatus(orderInfo.OrderID, model.order_status);
                                    //驿氪同步
                                }

                                #endregion

                            }


                        }
                    }

                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                    {
                        if (orderInfo.Status != "已取消" && (!string.IsNullOrEmpty(orderInfo.OutOrderId)))
                        {
                            var tradeDetail = efastClient.GetTradeDetail(orderInfo.OutOrderId);
                            if (tradeDetail != null)
                            {
                                if (tradeDetail.order_status == "3")//efast 订单作废
                                {

                                    orderInfo.Status = "已取消";
                                    model.order_status = "已取消";
                                    bllMall.Update(orderInfo, string.Format(" Status='已取消'"), string.Format(" OrderId='{0}'", orderInfo.OrderID));

                                    #region 驿氪同步
                                    if ((!string.IsNullOrEmpty(currentUserInfo.Phone)))
                                    {

                                        //驿氪同步
                                        yiKeClient.ChangeStatus(orderInfo.OrderID, model.order_status);
                                        //驿氪同步
                                    }

                                    #endregion


                                }
                            }


                        }
                    }


                }
                #endregion

                model.product_list = new List<OrderProductModel>();
                var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                foreach (var orderDetail in orderDetailList)
                {
                    WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                    OrderProductModel orderProductModel = new OrderProductModel();
                    orderProductModel.count = orderDetail.TotalCount;
                    orderProductModel.img_url = orderDetail.ProductImage;
                    if (string.IsNullOrEmpty(orderProductModel.img_url))
                    {
                        orderProductModel.img_url = bllMall.GetImgUrl(productInfo.RecommendImg);
                    }
                    orderProductModel.product_name = orderDetail.ProductName;
                    if (string.IsNullOrEmpty(orderProductModel.product_name))
                    {
                        orderProductModel.product_name = productInfo.PName;
                    }
                    orderProductModel.price = (decimal)orderDetail.OrderPrice;

                   
                    orderProductModel.category_name = bllMall.GetWXMallCategoryName(productInfo.CategoryId);
                    orderProductModel.quote_price = productInfo.PreviousPrice;
                    orderProductModel.show_property = orderDetail.SkuShowProp;
                    orderProductModel.parent_product_id = orderDetail.ParentProductId;
                    orderProductModel.order_detail_id = orderDetail.AutoID;
                    model.product_list.Add(orderProductModel);
                }

                model.group_buy_info = GetGroupBuyInfo(orderInfo);
                list.Add(model);

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list

            });
        }

         /// <summary>
        /// 获取退款订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RefundList(HttpContext context)
        {
            var refundList = bllMall.GetList<WXMallRefund>(string.Format(" UserId='{0}'", currentUserInfo.UserID));
            List<object> resultList = new List<object>();
            foreach (var item in refundList)
            {
                var orderInfo = bllMall.GetOrderInfo(item.OrderId);
                var orderDetail=bllMall.GetOrderDetail(item.OrderDetailId);
                if (orderInfo!=null&&orderDetail != null)
                {
                    var model = new
                    {
                        order_id = item.OrderId,
                        order_time =orderInfo.InsertDate.ToString(),
                        apply_time = item.InsertDate.ToString(),
                        order_detail_id = item.OrderDetailId,
                        refund_status = item.Status,
                        img_url = orderDetail.ProductImage,
                        product_name = orderDetail.ProductName,
                        show_property = orderDetail.SkuShowProp,
                        is_return_product=item.IsReturnProduct

                    };
                    resultList.Add(model);
                }

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = resultList.Count,
                list = resultList

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
            int hasReviewInfo = Convert.ToInt32(context.Request["has_review_info"]);
            string isReplacePay=context.Request["is_replace_pay"];
            var orderInfo = bllMall.GetOrderInfo(orderId);

            if (string.IsNullOrEmpty(isReplacePay))//代付
            {
                if (orderInfo.OrderUserID != currentUserInfo.UserID)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权查看此订单";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
           
            

            OrderDetailModel model = new OrderDetailModel();
            model.buyer_memo = orderInfo.OrderMemo;
            if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
            {
                var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderInfo.OrderUserID);
                if (myCardCoupon != null && orderInfo.CouponType==0)
                {
                    var cardCoupon = bllCardCoupon.GetCardCoupon(myCardCoupon.CardId);
                    if (cardCoupon != null)
                    {
                        model.cardcoupon = cardCoupon.Name;
                    }
                }
                else
                {
                    var record = bllMall.Get<StoredValueCardRecord>(string.Format(" AutoId={0} ", orderInfo.MyCouponCardId));
                   if (record!=null)
                   {
                       StoredValueCard storeValueCard=bllMall.Get<StoredValueCard>(string.Format(" AutoId={0}",record.CardId));
                       if (storeValueCard!=null)
                       {
                           model.cardcoupon = storeValueCard.Name;
                       }
                   }
                }


            }
            model.order_id = orderInfo.OrderID;
            model.out_order_id = orderInfo.OutOrderId;
            model.order_time = bllMall.GetTimeStamp(orderInfo.InsertDate);
            model.order_time_str = orderInfo.InsertDate.ToString();
            model.product_count = orderInfo.ProductCount;
            model.total_amount = orderInfo.TotalAmount;
            model.freight = orderInfo.Transport_Fee;
            model.order_status = orderInfo.Status;
            model.pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
            model.product_count = orderInfo.ProductCount;
            model.receiver_province_code = orderInfo.ReceiverProvinceCode;
            model.receiver_province = orderInfo.ReceiverProvince;
            model.receiver_city_code = orderInfo.ReceiverCityCode;
            model.receiver_city = orderInfo.ReceiverCity;
            model.receiver_dist_code = orderInfo.ReceiverDistCode;
            model.receiver_dist = orderInfo.ReceiverDist;
            model.receiver_address = orderInfo.Address;
            model.receiver_zip = orderInfo.ZipCode;
            model.receiver_name = orderInfo.Consignee;
            model.receiver_phone = orderInfo.Phone;
            model.use_score = orderInfo.UseScore;
            model.is_pay = orderInfo.PaymentStatus;
            model.express_company_code = orderInfo.ExpressCompanyCode;
            model.express_company_name = orderInfo.ExpressCompanyName;
            model.express_number = orderInfo.ExpressNumber;
            model.freight = orderInfo.Transport_Fee;
            model.order_type = orderInfo.OrderType;
            model.is_cansendgift = bllMall.IsCanShareGift(orderInfo);
            model.delivery_type = orderInfo.DeliveryType;
            model.review_score = orderInfo.ReviewScore;
            model.ex1 = orderInfo.Ex1;
            model.is_main = orderInfo.IsMain;
            model.parent_order_id = !string.IsNullOrEmpty(orderInfo.ParentOrderId) ? orderInfo.ParentOrderId : "";
            model.supplier_name = orderInfo.SupplierName;
            if (!string.IsNullOrEmpty(model.ex1))
            {
                model.ex1 = bllMall.GetTimeStamp(DateTime.Parse(model.ex1)).ToString();
            }
            model.ex2 = orderInfo.Ex2;
            if (!string.IsNullOrEmpty(model.ex2))
            {
                model.ex2 = bllMall.GetTimeStamp(DateTime.Parse(model.ex2)).ToString();
            }
            if (orderInfo.DeliveryTime != null)
            {
                model.delivery_time = bllMall.GetTimeStamp((DateTime)orderInfo.DeliveryTime);
            }
            model.ex3 = orderInfo.Ex3;
            model.ex4 = orderInfo.Ex4;
            model.ex5 = orderInfo.Ex5;
            model.ex6 = orderInfo.Ex6;
            model.ex7 = orderInfo.Ex7;
            model.ex8 = orderInfo.Ex8 == "1" ? "1" : "0";
            model.ex9 = orderInfo.Ex9;
            model.ex10 = orderInfo.Ex10 == "1" ? "1" : "0";
            model.ex11 = orderInfo.Ex11 == "1" ? "1" : "0";
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
            if (!string.IsNullOrEmpty(orderInfo.OrderUserID))
            {
                UserInfo user = bllUser.GetUserInfo(orderInfo.OrderUserID);

                model.curr_user_info = new
                {
                    avatar = user.WXHeadimgurl,
                    nickname = user.WXNickname,
                    is_order_user = user.UserID == currentUserInfo.UserID ? true : false,
                };
            }
              
            if (!string.IsNullOrEmpty(orderInfo.OtherUserId))
            {
                UserInfo user = bllUser.GetUserInfo(orderInfo.OtherUserId);
                model.pay_user_info = new
                {
                    avatar = user.WXHeadimgurl,
                    nickname = user.WXNickname,
                    pay_time = bllMall.GetTimeStamp(orderInfo.PayTime),
                    is_pay_user = user.UserID == currentUserInfo.UserID ? true : false
                };
            }
            model.dai_pay_total_amount = bllOrder.GetOrderDaiPayAmount(orderInfo);
            model.other_use_amount = orderInfo.OtherUseAmount;
            model.other_card_coupon_dis_amount = orderInfo.OtherCardcouponDisAmount;
            model.gift_order_type = GetGiftOrderType(orderInfo);
            model.weixin_refund_status = bllMall.GetWeiXinRefundStatus(model.out_order_id, model.ex17);
            model.use_amount = orderInfo.UseAmount;
            model.score_exchang_amount = orderInfo.ScoreExchangAmount;
            model.cardcoupon_exchang_amount = orderInfo.CardcouponDisAmount;
            //orderInfo.ParentOrderId;
            model.groupbuy_parent_order_id = orderInfo.GroupBuyParentOrderId;

            model.product_fee = orderInfo.Product_Fee;

            //if (orderInfo.OrderType == 2 && string.IsNullOrWhiteSpace(model.groupbuy_parent_order_id) )
            //{
            //    model.groupbuy_parent_order_id = orderInfo.OrderID;
            //}

            model.is_no_express = orderInfo.IsNoExpress;
            model.is_need_name_phone = orderInfo.IsNeedNamePhone;
            if (orderInfo.OrderType==7&&orderInfo.PaymentStatus==1)
            {
                model.exam_info = bllMall.GetExamInfo(orderInfo);
            }
           
            model.product_list = new List<OrderProductModel>();
            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            foreach (var orderDetail in orderDetailList)
            {
                WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                OrderProductModel orderProductModel = new OrderProductModel();
                orderProductModel.count = orderDetail.TotalCount;
                orderProductModel.product_name = orderDetail.ProductName;
                if (string.IsNullOrEmpty(orderProductModel.product_name))
                {
                    orderProductModel.product_name = productInfo.PName;
                }
                orderProductModel.img_url = orderDetail.ProductImage;
                if (string.IsNullOrEmpty(orderProductModel.img_url))
                {
                    orderProductModel.img_url = bllMall.GetImgUrl(productInfo.RecommendImg);
                }
                orderProductModel.price = (decimal)orderDetail.OrderPrice;
                orderProductModel.score = orderDetail.OrderScore;
                orderProductModel.category_name = bllMall.GetWXMallCategoryName(productInfo.CategoryId);
                orderProductModel.quote_price = productInfo.PreviousPrice;
                orderProductModel.show_property = orderDetail.SkuShowProp;
                orderProductModel.refund_status = bllMall.GetRefundStatus(orderDetail);
                orderProductModel.order_detail_id = orderDetail.AutoID;
                orderProductModel.max_refund_amount = orderDetail.MaxRefundAmount;
                orderProductModel.product_id = orderDetail.PID;
                orderProductModel.parent_product_id = orderDetail.ParentProductId;                
                orderProductModel.review_score = 0;
                orderProductModel.review_content = "";
                orderProductModel.comment_img = "";//晒图（评论图片）
                if (hasReviewInfo == 1)
                {
                    var reviewInfo = bllReview.GetOrderProductReviewInfo(orderDetail.OrderID, orderDetail.PID,orderDetail.AutoID.ToString());
                    if (reviewInfo != null)
                    {
                        orderProductModel.review_score = reviewInfo.ReviewScore;
                        orderProductModel.review_content = reviewInfo.ReviewContent;
                        orderProductModel.comment_img = reviewInfo.CommentImg;
                    }
                }
                if (productInfo.IsAppointment==1)
                {
                    var appointmentInfo = bllMall.GetProductAppointmentInfo(productInfo);
                    orderProductModel.show_property += string.Format(" [预购]{0}前发货",appointmentInfo.appointment_delivery_time);


                }

                model.product_list.Add(orderProductModel);
            }
            string msg = "";
            if (!string.IsNullOrEmpty(orderInfo.ExpressCompanyCode)&&!string.IsNullOrEmpty(orderInfo.ExpressNumber))
            {
                if (bllKuaidi100.GetExpressResult(orderInfo.ExpressNumber,orderInfo.ExpressCompanyCode)==null)
                {
                   bllKuaidi100.Poll(orderInfo.ExpressCompanyCode, orderInfo.ExpressNumber, out msg);
                }

            }
            model.claim_arrival_time = !string.IsNullOrEmpty(orderInfo.ClaimArrivalTime)?orderInfo.ClaimArrivalTime:"";
            model.store_address = orderInfo.StoreAddress;
            return ZentCloud.Common.JSONHelper.ObjectToJson(model);


        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReceiptConfirm(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.OrderUserID != currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "无权查看此订单";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            string msg = "";
            if (bllMall.OrderSuccess(orderInfo,out msg))
            {
                resp.errmsg ="ok";
                resp.errcode = 0;
            }
            else
            {
                resp.errmsg = msg;
                resp.errcode = 1;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //if (orderInfo.PaymentStatus.Equals(0))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "订单未付款";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}
            //if (orderInfo.Status.Equals("交易成功"))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "已经收过货了，请不要重复收货";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}
            //if (!orderInfo.Status.Equals("已发货"))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "只有状态是已发货的订单才能确认收货";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}

            ////下单用户
            //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
            //ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            //try
            //{


            //    orderInfo.Status = "交易成功";
            //    orderInfo.ReceivingTime = DateTime.Now;
            //    orderInfo.DistributionStatus = 2;
            //    if (!bllMall.Update(orderInfo, tran))
            //    {
            //        tran.Rollback();
            //        resp.errcode = 1;
            //        resp.errmsg = "更新订单状态失败";
            //    }

            //    #region 交易成功加积分


            //    //TODO:这边改成增加的是增加冻结积分，到了分佣的时候才真正把积分加上，mixblu按照原规则走：确认收货就发积分、积分按应付金额来算，其他的则按新规则来
            //    if (websiteInfo.WebsiteOwner == "mixblu")
            //    {
            //        #region mixblu加积分
            //        //增加积分
            //        ScoreConfig scoreConfig = bllScore.GetScoreConfig();
            //        int addScore = 0;
            //        if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
            //        {
            //            addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
            //        }
            //        if (addScore > 0)
            //        {
            //            CurrentUserInfo.TotalScore += addScore;

            //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //            scoreRecord.AddTime = DateTime.Now;
            //            scoreRecord.Score = addScore;
            //            scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
            //            scoreRecord.ScoreType = "OrderSuccess";
            //            scoreRecord.UserID = CurrentUserInfo.UserID;
            //            scoreRecord.AddNote = "微商城-交易成功获得积分";
            //            scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
            //            if (!bllMall.Add(scoreRecord, tran))
            //            {
            //                tran.Rollback();
            //                resp.errcode = 1;
            //                resp.errmsg = "插入积分记录表失败";
            //            }
            //            if (bllUser.Update(CurrentUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 1)
            //            {
            //                tran.Rollback();
            //                resp.errcode = 1;
            //                resp.errmsg = "更新用户积分失败";
            //            }

            //            #region yike 加积分
            //            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //            {

            //                if ((!string.IsNullOrEmpty(CurrentUserInfo.Ex2)) && (!string.IsNullOrEmpty(CurrentUserInfo.Phone)))
            //                {
            //                    try
            //                    {
            //                        //驿氪同步
            //                        yiKeClient.BonusUpdate(CurrentUserInfo.Ex2, addScore, string.Format("订单交易成功获得{0}积分", addScore));
            //                        //驿氪同步
            //                        yiKeClient.ChangeStatus(orderInfo.OrderID, orderInfo.Status);

            //                    }
            //                    catch (Exception)
            //                    {


            //                    }

            //                }


            //            }
            //            #endregion

            //            #region 宏巍加积分
            //            if (websiteInfo.IsUnionHongware == 1)
            //            {
            //                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
            //                var hongWareMemberInfo = hongWareClient.GetMemberInfo(CurrentUserInfo.WXOpenId);
            //                if (hongWareMemberInfo.member != null)
            //                {
            //                    if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, CurrentUserInfo.WXOpenId, addScore))
            //                    {
            //                        tran.Rollback();
            //                        resp.errcode = 1;
            //                        resp.errmsg = "更新宏巍积分失败";


            //                    }


            //                }


            //            }
            //            #endregion

            //        }
            //        #endregion
            //    }
            //    else
            //    {
                    
            //    }

            //    #endregion

            //    //

            //    //更新订单明细表状态
            //    orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            //    foreach (var orderDetail in orderDetailList)
            //    {
            //        orderDetail.IsComplete = 1;
            //        orderDetail.CompleteTime = DateTime.Now;
            //        if (!bllMall.Update(orderDetail))
            //        {
            //            tran.Rollback();
            //            resp.errcode = 1;
            //            resp.errmsg = "更新订单明细表失败";
            //        }

            //    }




            //}
            //catch (Exception ex)
            //{
            //    tran.Rollback();
            //    resp.errcode = 1;
            //    resp.errmsg = ex.ToString();
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //}

            //tran.Commit();
            //#region 更新销量
            //foreach (var orderDetail in orderDetailList)
            //{
            //    int saleCount = bllMall.GetProductSaleCount(int.Parse(orderDetail.PID));
            //    bllMall.Update(new WXMallProductInfo(), string.Format("SaleCount={0}", saleCount), string.Format(" PID='{0}'", orderDetail.PID));

            //} 
            //#endregion
           

            ////#region 分销佣金
            ////if (bllMenuPermission.CheckUserAndPmsKey(bllMall.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
            ////{
            ////    if (orderInfo.WebsiteOwner == "comeoncloud" || orderInfo.WebsiteOwner == "study")
            ////    {
            ////        string msg = "";
            ////        if (bllDis.Transfers(orderInfo, out msg))
            ////        {
            ////            //orderInfo.DistributionStatus = 3;
            ////            bllMall.Update(orderInfo, string.Format(" DistributionStatus=3"), string.Format(" OrderID='{0}'", orderInfo.OrderID));
            ////        }
            ////    }


            ////}
            ////#endregion

            //resp.errmsg = "ok";
            //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 计算订单可获积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcOrderRebateScore(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            var orderInfo = bllMall.GetOrderInfo(orderId);

            //计算获得积分
            var addScore = bllScore.CalcOrderRebateScore(orderInfo);

            //如获得积分则存入冻结表
            if (addScore > 0)
            {
                resp.result = addScore;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 计算传入的sku列表可获得的积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string CalcSkuListRebateScore(HttpContext context)
        {
            string skuListReq = context.Request["sku_list"];
            int useScore = Convert.ToInt32(context.Request["use_score"]);
            decimal useAmount = Convert.ToDecimal(context.Request["use_amount"]);
            int cardcouponId = Convert.ToInt32(context.Request["cardcoupon_id"]);
            decimal totalPay = Convert.ToDecimal(context.Request["total_pay"]);
            int order_type = Convert.ToInt32(context.Request["order_type"]);
            string groupbuy_type = context.Request["groupbuy_type"];
            
            var skuList = JsonConvert.DeserializeObject< List<SkuModel>>(skuListReq);
            var isAllCash = useScore == 0 && useAmount == 0 && cardcouponId == 0;
            var rate = bllScore.GetUserRebateScoreRate(currentUserInfo.UserID, currentUserInfo.WebsiteOwner);

            //判断订单类型是否支持返积分
            var currOrderType = OrderType.Normal;
            try
            {
                currOrderType = (OrderType)order_type;
            }
            catch (Exception ex)
            {
                
            }

            //判断订单类型是否支持返积分
            if (!bllScore.IsCanRebateScoreByOrderType(currOrderType, groupbuy_type))
            {
                resp.result = new
                {
                    score = 0,
                    rate = 0
                };
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.msg = "";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //计算均摊的时候才需要
            decimal productFee = 0;
            
            if (!isAllCash)
            {
                var websiteInfo = bllScore.GetWebsiteInfoModelFromDataBase(bllScore.WebsiteOwner);

                if (websiteInfo.IsRebateScoreMustAllCash == 1)
                {
                    //如果没有使用优惠券、积分、余额抵扣则为全额付款
                    resp.result = new
                    {
                        score = 0,
                        rate = rate
                    };
                    resp.errcode = (int)APIErrCode.IsMustAllCash;
                    resp.msg = "全额付款才可获得积分";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //没有全额付款 则需要计算出均摊价格
                for (int i = 0; i < skuList.Count; i++)
                {
                    var sku = bllMall.GetProductSku(skuList[i].sku_id);
                    skuList[i].price = sku.Price;
                }

                productFee = skuList.Sum(p => p.price * p.count).Value;
            }

            if (totalPay == 0 || rate == 0)
            {
                resp.result = new
                {
                    score = 0,
                    rate = rate
                };
                resp.msg = "";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            //计算获得积分
            decimal addScore = 0;

            foreach (var item in skuList)
            {
                decimal payRate = 1;

                if (!isAllCash)
                {
                    //计算返积分均摊价
                    payRate = totalPay / productFee;
                }

                addScore += bllScore.CalcProductSkuRebateScore(item.sku_id, rate, payRate, item.count);
            }
            
            resp.result = new
            {
                score = bllScore.RebateScoreGetInt(addScore),
                rate = rate
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Cancel(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            var orderInfo = bllMall.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "订单不存在";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (orderInfo.OrderUserID != currentUserInfo.UserID)
            {
                resp.errcode = 1;
                resp.errmsg = "无权操作";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrWhiteSpace(orderInfo.ParentOrderId))
            {
                resp.errcode = 1;
                resp.errmsg = "无权操作";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }

            string msg = "";
            if (bllMall.CancelOrder(orderInfo,out msg))
            {
                resp.errcode = 0;
                resp.errmsg ="ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


            //List<string> limitStatus = new List<string>() { "已发货", "交易成功", "已取消", "预约成功", "预约失败" };
            //if (limitStatus.Contains(orderInfo.Status))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "只有状态为待付款,待发货的订单才可以取消";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //}

            //ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            //try
            //{
                
            //    if (bllMall.Update(orderInfo, " Status='已取消'", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
            //    {
            //        tran.Rollback();
            //        resp.errcode = 1;
            //        resp.errmsg = "更新订单状态失败";
            //        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //    }

            //    //返还库存 BLLMall 中也有对应的方法返还SKU
            //    if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
            //    {
            //        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            //        foreach (var orderDetail in orderDetailList)
            //        {

            //            if (orderDetail.SkuId != null)
            //            {
            //                ProductSku sku = bllMall.GetProductSku((int)orderDetail.SkuId);
            //                if (sku != null)
            //                {
            //                    if (bllMall.Update(sku, string.Format(" Stock+={0}", orderDetail.TotalCount), string.Format(" SkuId={0}", sku.SkuId), tran) == 0)
            //                    {
            //                        tran.Rollback();
            //                        resp.errcode = 1;
            //                        resp.errmsg = "修改sku库存失败";
            //                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //                    }
            //                }
            //            }


            //        }
            //        if (bllMall.Update(new WXMallOrderDetailsInfo(), "IsComplete=0", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
            //        {
            //            tran.Rollback();
            //            resp.errcode = 1;
            //            resp.errmsg = "更新订单详情失败";
            //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //        }


            //    }
            //    #region 积分返还
            //    if (orderInfo.UseScore > 0)//使用积分 积分返还
            //    {
            //        CurrentUserInfo.TotalScore += orderInfo.UseScore;
            //        if (bllUser.Update(CurrentUserInfo,
            //            string.Format(" TotalScore+={0}", orderInfo.UseScore),
            //            string.Format(" AutoID={0}", CurrentUserInfo.AutoID),
            //            tran
            //            ) < 0)
            //        {
            //            tran.Rollback();
            //            resp.errcode = 1;
            //            resp.errmsg = "积分返还失败";
            //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //        }
            //        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //        scoreRecord.AddTime = DateTime.Now;
            //        scoreRecord.Score = orderInfo.UseScore;
            //        scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
            //        scoreRecord.ScoreType = "OrderCancel";
            //        scoreRecord.UserID = CurrentUserInfo.UserID;
            //        scoreRecord.RelationID = orderInfo.OrderID;
            //        scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
            //        if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
            //        {
            //            scoreRecord.AddNote = "微商城-订单取消返还积分";
            //        }
            //        else
            //        {
            //            scoreRecord.AddNote = "预约-订单取消返还积分";
            //        }
            //        bllMall.Add(scoreRecord, tran);

                   
                   
            //        if (websiteInfo.IsUnionHongware == 1)
            //        {
            //            hongWareMemberInfo = hongWareClient.GetMemberInfo(CurrentUserInfo.WXOpenId);
            //        }

            //        #region 宏巍加积分

            //        if (websiteInfo.IsUnionHongware == 1)
            //        {
            //            if (hongWareMemberInfo.member != null)
            //            {

            //                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, orderInfo.UseScore))
            //                {
            //                    tran.Rollback();
            //                    resp.errcode = 1;
            //                    resp.errmsg = "更新宏巍积分失败";


            //                }


            //            }


            //        }
            //        #endregion


            //    }
            //    #endregion

            //    #region 优惠券储值卡返还
            //    if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
            //    {

            //        var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), CurrentUserInfo.UserID);
            //        if (myCardCoupon != null && myCardCoupon.Status == 1)
            //        {
            //            myCardCoupon.Status = 0;
            //            if (!bllCardCoupon.Update(myCardCoupon, tran))
            //            {

            //                tran.Rollback();
            //                resp.errcode = 1;
            //                resp.errmsg = "优惠券更新失败";
            //                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //            }

            //        }
            //        else
            //        {
            //            bllMall.Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'",orderInfo.OrderID), tran);
            //        }

            //    }
            //    #endregion

            //    #region 账户余额返还

            //    if (orderInfo.UseAmount > 0)
            //    {
            //        CurrentUserInfo.AccountAmount += orderInfo.UseAmount;
            //        if (bllMall.Update(
            //            CurrentUserInfo, 
            //            string.Format(" AccountAmount+={0}", orderInfo.UseAmount), 
            //            string.Format(" AutoID={0}", CurrentUserInfo.AutoID),tran) < 0)
            //        {
            //            tran.Rollback();
            //            resp.errcode = 1;
            //            resp.errmsg = "更新用户余额失败";
            //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //        }


            //        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //        scoreRecord.AddTime = DateTime.Now;
            //        scoreRecord.Score = (double)orderInfo.UseAmount;
            //        scoreRecord.TotalScore = (double)CurrentUserInfo.AccountAmount;
            //        scoreRecord.ScoreType = "AccountAmount";
            //        scoreRecord.UserID = CurrentUserInfo.UserID;
            //        scoreRecord.RelationID = orderInfo.OrderID;
            //        scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
            //        scoreRecord.AddNote = "微商城-订单取消返还余额";
            //        if (!bllMall.Add(scoreRecord,tran))
            //        {
            //            tran.Rollback();
            //            resp.errcode = 1;
            //            resp.errmsg = "插入余额记录失败";
            //            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //        }

            //        #region 宏巍加余额

            //        if (websiteInfo.IsUnionHongware == 1)
            //        {
            //            if (hongWareMemberInfo.member != null)
            //            {
            //                if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, (float)orderInfo.UseAmount))
            //                {
            //                    tran.Rollback();
            //                    resp.errcode = 1;
            //                    resp.errmsg = "更新宏巍余额失败";


            //                }


            //            }


            //        }
            //        #endregion




            //    }


            //    #endregion

            //    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //    {
            //        if (orderInfo.UseScore > 0)
            //        {
            //            yiKeClient.BonusUpdate(CurrentUserInfo.Ex2, orderInfo.UseScore, string.Format("订单:{0}取消返还{1}积分", orderInfo.OrderID, orderInfo.UseScore));

            //        }
            //        yiKeClient.ChangeStatus(orderId, "已取消");

            //    }

            //    //冻结积分取消
            //    bllScore.CancelLockScoreByOrder(orderId, "取消订单，积分取消",tran);

            //    tran.Commit();
            //    bllMall.Update(orderInfo, string.Format("Status='己取消'"), string.Format(" ParentOrderId='{0}'", orderInfo.OrderID));
            //    resp.errcode = 0;
            //    resp.errmsg = "ok";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


            //}
            //catch (Exception ex)
            //{
            //    tran.Rollback();
            //    resp.errcode = 1;
            //    resp.errmsg = ex.Message;
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //}


        }


        /// <summary>
        /// 订单模型
        /// </summary>
        private class OrderModel
        {
            /// <summary>
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }
            /// <summary>
            /// 支付类型 WEIXIN 或 ALIPAY
            /// </summary>
            public string pay_type { get; set; }
            /// <summary>
            /// 物流方式 暂未用到
            /// </summary>
            public string shipping_type { get; set; }
            /// <summary>
            /// 收货人省份
            /// </summary>
            public string receiver_province { get; set; }
            /// <summary>
            /// 收货人省份代码
            /// </summary>
            public int receiver_province_code { get; set; }
            /// <summary>
            /// 收货人城市名称
            /// </summary>
            public string receiver_city { get; set; }
            /// <summary>
            /// 收货人城市代码
            /// </summary>
            public int receiver_city_code { get; set; }

            /// <summary>
            /// 收货人区域名称
            /// </summary>
            public string receiver_dist { get; set; }
            /// <summary>
            /// 收货人区域代码
            /// </summary>
            public int receiver_dist_code { get; set; }
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
            /// <summary>
            /// 快递公司名称
            /// </summary>
            public string express_company { get; set; }
            /// <summary>
            /// 商品SKU列表
            /// </summary>
            public List<SkuModel> skus { get; set; }
            /// <summary>
            /// 优惠券编号 我的优惠券编号
            /// </summary>
            public int cardcoupon_id { get; set; }
            /// <summary>
            /// 使用积分
            /// </summary>
            public int use_score { get; set; }

            /// <summary>
            /// 导购用户ID
            /// </summary>
            public string sale_id { get; set; }
            /// <summary>
            /// 使用账户金额
            /// </summary>
            public decimal use_amount { get; set; }



        }


        ///// <summary>
        ///// 快递公司代码对应
        ///// </summary>
        //Dictionary<string, string> expressCompanyMap = new Dictionary<string, string>()
        //{
        //   {"zto","中通快递"}


        //};
        
        /// <summary>
        /// 获取礼品订单类型
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        private string GetGiftOrderType(WXMallOrderInfo orderInfo)
        {


            if (orderInfo.OrderType == 1)
            {
                if (!string.IsNullOrEmpty(orderInfo.ParentOrderId))
                {
                    return "0";
                }
                else
                {
                    return "1";
                }
            }
            return "";

        }
        /// <summary>
        /// 获取拼团信息
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public object GetGroupBuyInfo(WXMallOrderInfo orderInfo)
        {
            if (orderInfo.OrderType != 2)
            {
                return null;

            }
            if (!string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
            {
                orderInfo = bllMall.GetOrderInfo(orderInfo.GroupBuyParentOrderId);
            }
            double endTime = 0;
            if (orderInfo.PayTime != null)
            {
                endTime = bllMall.GetTimeStamp(((DateTime)orderInfo.PayTime).AddDays(orderInfo.ExpireDay));
            }
            return new
           {
               head_discount = orderInfo.HeadDiscount,
               member_discount = orderInfo.MemberDiscount,
               people_count = orderInfo.PeopleCount,
               expire_day = orderInfo.ExpireDay,
               group_buy_status = GetGroupBuyStatus(orderInfo),
               end_time = endTime,
               pay_people_count = GetPayPeopleCount(orderInfo.OrderID),
               head_price =orderInfo.Ex10=="1"?Convert.ToDecimal(orderInfo.Ex13):orderInfo.Product_Fee
           };

        }
        /// <summary>
        /// 获取拼团状态
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public int GetGroupBuyStatus(WXMallOrderInfo orderInfo,string groupbuyType="")
        {
            int status = -1;

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("  PaymentStatus=1 ");
            if (!string.IsNullOrEmpty(groupbuyType) && groupbuyType == "1")//系统开团的订单
            {
                sbSql.AppendFormat(" And GroupBuyParentOrderId='{0}' ",orderInfo.OrderID);
            }
            else
            {
                sbSql.AppendFormat(" And GroupBuyParentOrderId='{0}' Or (OrderId='{0}' And PaymentStatus=1) ", orderInfo.OrderID);
            }


            if ((bllMall.GetCount<WXMallOrderInfo>(sbSql.ToString()) < orderInfo.PeopleCount) && (DateTime.Now < orderInfo.InsertDate.AddDays(orderInfo.ExpireDay)))
            {
                return 0;
            }
            else if (bllMall.GetCount<WXMallOrderInfo>(sbSql.ToString()) >= orderInfo.PeopleCount)
            {
                return 1;//拼团成功
            }
            if (DateTime.Now >= orderInfo.InsertDate.AddDays(orderInfo.ExpireDay))
            {
                return 2;//拼团过期失败
            }
            return status;



        }
        /// <summary>
        /// 获取已经参加拼团的人数
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetPayPeopleCount(string orderId)
        {
            return bllMall.GetCount<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}'  And OrderType=2 And WebsiteOwner='{1}'  And PaymentStatus=1 Or (OrderId='{0}' And PaymentStatus=1) ", orderId, bllMall.WebsiteOwner));

        }

        //private void ToLog(string log)
        //{
        //    try
        //    {
        //        using (StreamWriter sw = new StreamWriter(@"D:\logSandBox.txt", true, Encoding.GetEncoding("gb2312")))
        //        {
        //            sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
        //        }
        //    }
        //    catch { }
        //}
    }
}
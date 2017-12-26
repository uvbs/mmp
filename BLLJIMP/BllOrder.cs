using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;
using System.Web;
using ZentCloud.BLLJIMP.Model.API;
using Newtonsoft.Json.Linq;
using ZentCloud.ZCBLLEngine;
using ZentCloud.BLLJIMP.ModelGen.API.Mall;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    ///订单 逻辑
    /// </summary>
    public class BllOrder : BLL
    {


        /// <summary>
        /// 驿氪  
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// Efast
        /// </summary>
        Open.EfastSDK.Client efastClient = new Open.EfastSDK.Client();

        public BllOrder()
            : base()
        {

        }
        /// <summary>
        /// 获取支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderPay GetOrderPay(string orderId, string status = "", string websiteOwner = "", int? payType = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" OrderId ='{0}' ", orderId);
            if (!string.IsNullOrWhiteSpace(status)) sb.AppendFormat(" AND Status={0} ", status);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sb.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            if (payType.HasValue) sb.AppendFormat(" AND PayType={0} ", payType.Value);
            return Get<OrderPay>(sb.ToString());
        }

        /// <summary>
        /// 获取充值订单
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="ex1"></param>
        /// <param name="ex2"></param>
        /// <param name="total"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<OrderPay> GetOrderPayList(int pageSize, int pageIndex, string status, string type, string ex1, string ex2, out int total, string websiteOwner, string userId = "", string keyWord = "", string ex5 = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" Status In ({0}) ", status);
            sb.AppendFormat(" AND Type='{0}' ", type);
            if (type == "2" || type == "8")
            {
                if (!string.IsNullOrWhiteSpace(ex1)) sb.AppendFormat(" AND Ex1='{0}' ", ex1);
            }
            if (!string.IsNullOrWhiteSpace(ex2)) sb.AppendFormat(" AND Ex2 LIKE '%{0}%' ", ex2);
            sb.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);

            if (!string.IsNullOrEmpty(userId))
            {
                sb.AppendFormat(" And UserId='{0}'", userId);
            }
            if (type == "8" && !string.IsNullOrEmpty(keyWord))
            {

                sb.AppendFormat(" And UserId In(Select UserId from ZCJ_UserInfo where Websiteowner='{0}' And (TrueName like '%{1}%' or Phone ='{1}'))", websiteOwner, keyWord);

            }
            if (type == "8")
            {
                if (!string.IsNullOrEmpty(ex5))
                {
                    if (ex5 == "1")
                    {
                        sb.AppendFormat(" And Ex5='{0}'", ex5);
                    }
                    else
                    {
                        sb.AppendFormat(" And (Ex5='' Or Ex5 IS NULL)");
                    }

                }

            }


            total = GetCount<OrderPay>(sb.ToString());
            return GetLit<OrderPay>(pageSize, pageIndex, sb.ToString(), " AutoID Desc");
        }

        /// <summary>
        /// 获取用户所有订单数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserAllOrderCount(string userId)
        {
            return GetCount<Model.WXMallOrderInfo>(string.Format(" OrderUserID = '{0}' And WebsiteOwner = '{1}' ", userId, WebsiteOwner));
        }

        /// <summary>
        /// 计算均摊价
        /// </summary>
        /// <param name="totalAmount">总金额</param>
        /// <param name="detailList">订单详情</param>
        /// <returns></returns>
        public List<WXMallOrderDetailsInfo> CalcPaymentFt(decimal totalAmount, List<WXMallOrderDetailsInfo> detailList)
        {

            BLLMall bllMall = new BLLMall();

            try
            {
                List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//不重复的商品列表
                foreach (var orderDetail in detailList)
                {
                    var productInfo = bllMall.GetProduct(orderDetail.PID);
                    //if (productList.Where(p => p.PID == productInfo.PID).Count() == 0)
                    //{
                    productList.Add(productInfo);
                    //}
                }
                foreach (var orderDetail in detailList)
                {
                    var productQuotePrice = productList.First(p => p.PID == orderDetail.PID).PreviousPrice;
                    decimal rate = (decimal)productQuotePrice / productList.Sum(p => p.PreviousPrice);//此sku所占的比例
                    orderDetail.PaymentFt = (totalAmount * rate);
                    if (orderDetail.PaymentFt < 0)
                    {
                        orderDetail.PaymentFt = 0;
                    }
                }

                decimal cha = totalAmount - detailList.Sum(p => p.PaymentFt); //100 99.99 100 100.01  //差值补到最后一种商品
                if (cha != 0)
                {
                    detailList[detailList.Count - 1].PaymentFt += cha;
                    if (detailList[detailList.Count - 1].PaymentFt < 0)
                    {
                        detailList[detailList.Count - 1].PaymentFt = 0;
                    }
                }


            }
            catch (Exception)
            {


            }
            return detailList;
        }

        /// <summary>
        /// 计算最大退款金额
        /// </summary>
        /// <param name="amount">总金额</param>
        /// <param name="detailList">订单详情</param>
        /// <returns></returns>
        public List<WXMallOrderDetailsInfo> CalcMaxRefundAmount(decimal amount, List<WXMallOrderDetailsInfo> detailList)
        {

            try
            {
                if (amount < 0)
                {
                    amount = 0;
                }
                var productTotalPrice = ((decimal)detailList.Sum(p => p.OrderPrice * p.TotalCount));
                foreach (var orderDetail in detailList)
                {

                    decimal rate = 0;//此sku所占的比例
                    if (productTotalPrice > 0)
                    {
                        rate = ((decimal)orderDetail.OrderPrice * orderDetail.TotalCount) / productTotalPrice;//此sku所占的比例
                    }

                    orderDetail.MaxRefundAmount = Math.Round((amount * rate), 2, MidpointRounding.AwayFromZero);

                }
                decimal cha = amount - detailList.Sum(p => p.MaxRefundAmount); //100 99.99 100 100.01  //差值补到最后一种商品
                if (cha != 0)
                {
                    detailList[detailList.Count - 1].MaxRefundAmount += cha;
                }


            }
            catch (Exception)
            {


            }
            return detailList;
        }

        /// <summary>
        /// 判断是否是团长下单
        /// </summary>
        /// <param name="orderRequestModel"></param>
        /// <returns></returns>
        public bool IsGroupByLeader(OrderRequestModel orderRequestModel)
        {
            if (orderRequestModel.groupbuy_type == "leader" && orderRequestModel.order_type == 2)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否是团购团员下单
        /// </summary>
        /// <param name="orderRequestModel"></param>
        /// <returns></returns>
        public bool IsGroupByMember(OrderRequestModel orderRequestModel)
        {
            if (orderRequestModel.groupbuy_type == "member" && orderRequestModel.order_type == 2)
            {
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 提交订单备份20170116
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public AddOrderResp Add(HttpContext context)
        //{

        //    BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        //    BLLUser bllUser = new BLLUser();
        //    BLLDistribution bllDis = new BLLDistribution();
        //    BLLMall bllMall = new BLLMall();
        //    BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
        //    BLLCommRelation bllCommRelation = new BLLCommRelation();
        //    BllScore bllScore = new BllScore();
        //    BLLWeixin bllWeiXin = new BLLWeixin();
        //    BLLEfast bllEfast = new BLLEfast();
        //    BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();

        //    AddOrderResp resp = new AddOrderResp();
        //    var currentUserInfo = GetCurrentUserInfo();

        //    var currOrderType = Enums.OrderType.Normal;

        //    /// <summary>
        //    /// 宏巍接口
        //    /// </summary>
        //    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(WebsiteOwner);

        //    string data = context.Request["data"];
        //    decimal productFee = 0;//商品总价格 不包含邮费
        //    OrderRequestModel orderRequestModel;//订单模型
        //    try
        //    {
        //        orderRequestModel = Common.JSONHelper.JsonToModel<OrderRequestModel>(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Code = 1;
        //        resp.Msg = "JSON格式错误,请检查。错误信息:" + ex.Message;
        //        return resp;
        //    }
        //    WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase();
        //    ///宏巍会员信息
        //    Open.HongWareSDK.MemberInfo hongWeiWareMemberInfo = null;
        //    if (websiteInfo.IsUnionHongware == 1)
        //    {
        //        hongWeiWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
        //        if (hongWeiWareMemberInfo.member == null)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "您尚未绑定宏巍账号,请先绑定";
        //            return resp;
        //        }
        //    }

        //    WXMallOrderInfo parentOrderInfo = new WXMallOrderInfo();

        //    #region 参团前置判断
        //    if (IsGroupByMember(orderRequestModel))
        //    {
        //        if (string.IsNullOrEmpty(orderRequestModel.groupbuy_parent_orderid))
        //        {
        //            resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
        //            resp.Msg = "groupbuy_parent_orderid 必传";
        //            return resp;
        //        }

        //        parentOrderInfo = bllMall.GetOrderInfo(orderRequestModel.groupbuy_parent_orderid);//父订单
        //        if (parentOrderInfo == null)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "订单不存在";
        //            return resp;
        //        }
        //        //if (parentOrderInfo.OrderUserID == currentUserInfo.UserID)
        //        //{
        //        //    resp.Code = (int)Enums.APIErrCode.OperateFail;
        //        //    resp.Msg = "团长不可以参加";
        //        //    return resp;
        //        //}
        //        if (parentOrderInfo.OrderType != 2)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "不是拼团订单";
        //            return resp;
        //        }
        //        //if (!string.IsNullOrEmpty(parentOrderInfo.GroupBuyParentOrderId))
        //        //{
        //        //    resp.Code = (int)Enums.APIErrCode.OperateFail;
        //        //    resp.Msg = "订单无效";
        //        //    return resp;
        //        //}
        //        if (parentOrderInfo.PaymentStatus == 0)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "团长订单未付款";
        //            return resp;
        //        }
        //        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' Or OrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
        //        {

        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "团购人数已满";
        //            return resp;

        //        }
        //        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}' And OrderUserId='{1}' And PaymentStatus=0", parentOrderInfo.OrderID, currentUserInfo.UserID)) > 0)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "您还有未支付的订单,请先支付";
        //            return resp;
        //        }
        //        if (DateTime.Now >= (((DateTime)parentOrderInfo.PayTime).AddDays(parentOrderInfo.ExpireDay)))
        //        {
        //            parentOrderInfo.GroupBuyStatus = "2";
        //            bllMall.Update(parentOrderInfo);
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "拼团已过期";
        //            return resp;
        //        }


        //        #region 团长分销关系建立
        //        if (websiteInfo.DistributionRelationBuildMallOrder == 1)
        //        {
        //            if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner) && parentOrderInfo.OrderUserID != currentUserInfo.UserID)
        //            {
        //                currentUserInfo.DistributionOwner = parentOrderInfo.OrderUserID;
        //                bllUser.Update(currentUserInfo, string.Format(
        //                        " DistributionOwner='{0}' ",
        //                        currentUserInfo.DistributionOwner
        //                    ),
        //                    string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
        //            }
        //        }
        //        #endregion



        //        #region 参团订单的skuid构造
        //        List<SkuModel> orderSkuList = new List<SkuModel>();

        //        var parentOrderDetailList = bllMall.GetOrderDetailsList(parentOrderInfo.OrderID);

        //        orderSkuList.Add(new SkuModel()
        //        {
        //            sku_id = parentOrderDetailList[0].SkuId.Value,
        //            count = 1
        //        });

        //        orderRequestModel.skus = orderSkuList;
        //        #endregion

        //    }

        //    #endregion

        //    WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表

        //    //确定订单类型
        //    orderInfo.OrderType = orderRequestModel.order_type;

        //    try
        //    {
        //        currOrderType = (Enums.OrderType)orderInfo.OrderType;
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Code = (int)Enums.APIErrCode.MallOrderTypeNotExsit;
        //        resp.Msg = "订单类型不存在";
        //        return resp;

        //    }

        //    orderInfo.Address = orderRequestModel.receiver_address;
        //    orderInfo.Consignee = orderRequestModel.receiver_name;
        //    orderInfo.ExpressCompanyName = orderRequestModel.express_company;
        //    orderInfo.InsertDate = DateTime.Now;
        //    orderInfo.OrderUserID = currentUserInfo.UserID;
        //    orderInfo.Phone = orderRequestModel.receiver_phone;
        //    orderInfo.WebsiteOwner = WebsiteOwner;
        //    orderInfo.Transport_Fee = 0;
        //    orderInfo.OrderID = GetGUID(BLLJIMP.TransacType.AddMallOrder);

        //    if (string.IsNullOrWhiteSpace(currentUserInfo.TrueName))
        //    {
        //        currentUserInfo.TrueName = orderInfo.Consignee;
        //        Update(currentUserInfo, string.Format(" TrueName = '{0}' ", orderInfo.Consignee), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
        //    }

        //    if (string.IsNullOrWhiteSpace(currentUserInfo.Phone))
        //    {
        //        currentUserInfo.Phone = orderInfo.Phone;
        //        Update(currentUserInfo, string.Format(" Phone = '{0}' ", orderInfo.Phone), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
        //    }

        //    if (WebsiteOwner != "mixblu")
        //    {
        //        orderInfo.OutOrderId = orderInfo.OrderID;
        //    }
        //    orderInfo.OrderMemo = orderRequestModel.buyer_memo;
        //    orderInfo.ReceiverProvince = orderRequestModel.receiver_province;
        //    orderInfo.ReceiverProvinceCode = orderRequestModel.receiver_province_code.ToString();
        //    orderInfo.ReceiverCity = orderRequestModel.receiver_city;
        //    orderInfo.ReceiverCityCode = orderRequestModel.receiver_city_code.ToString();
        //    orderInfo.ReceiverDist = orderRequestModel.receiver_dist;
        //    orderInfo.ReceiverDistCode = orderRequestModel.receiver_dist_code.ToString();
        //    orderInfo.ZipCode = orderRequestModel.receiver_zip;
        //    orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();

        //    if (IsGroupByMember(orderRequestModel))
        //    {
        //        orderInfo.GroupBuyParentOrderId = parentOrderInfo.OrderID;
        //    }

        //    orderInfo.UseScore = orderRequestModel.use_score;
        //    orderInfo.Status = "待付款";
        //    orderInfo.ArticleCategoryType = "Mall";
        //    orderInfo.UseAmount = orderRequestModel.use_amount;
        //    orderInfo.LastUpdateTime = DateTime.Now;
        //    if (!string.IsNullOrEmpty(orderRequestModel.sale_id))
        //    {
        //        long saleId = 0;
        //        if (long.TryParse(orderRequestModel.sale_id, out saleId))
        //        {
        //            orderInfo.SellerId = saleId;

        //        }
        //        else
        //        {

        //        }
        //    }



        //    #region 分销关系建立
        //    if (bllMenuPermission.CheckUserAndPmsKey(WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
        //    {

        //        if (string.IsNullOrWhiteSpace(currentUserInfo.DistributionOwner) && (!string.IsNullOrEmpty(orderRequestModel.sale_id)))
        //        {

        //            if (websiteInfo.DistributionRelationBuildMallOrder == 1)
        //            {
        //                var preUserInfo = bllUser.GetUserInfo(orderRequestModel.sale_id);//推荐人
        //                if (preUserInfo != null)
        //                {
        //                    if (bllUser.IsDistributionMember(preUserInfo) || (preUserInfo.UserID == websiteInfo.WebsiteOwner))//上级符合分销员标准
        //                    {
        //                        currentUserInfo.DistributionOwner = preUserInfo.UserID;
        //                        Update(currentUserInfo);
        //                        bllDis.UpdateUpUserCount(currentUserInfo);
        //                        bllWeiXin.SendTemplateMessageNotifyComm(preUserInfo, string.Format("新会员通知"), string.Format("恭喜 {0} 成为您的第{1}号会员", currentUserInfo.WXNickname, preUserInfo.DistributionDownUserCountLevel1 + 1));

        //                    }

        //                }
        //            }

        //        }

        //        bllDis.SendMessageToPreUser(orderInfo);


        //    }
        //    #endregion

        //    if (orderRequestModel.pay_type == "WEIXIN")//微信支付
        //    {
        //        orderInfo.PaymentType = 2;
        //    }
        //    else if (orderRequestModel.pay_type == "ALIPAY")//支付宝支付
        //    {
        //        orderInfo.PaymentType = 1;
        //    }


        //    //判断商品是否无需物流
        //    int isNoExpress = 1;

        //    int isNeedNamePhone = 0;

        //    #region 格式检查
        //    if (orderRequestModel.skus == null)
        //    {
        //        resp.Code = 1;
        //        resp.Msg = "参数skus 不能为空";
        //        return resp;
        //    }
        //    if (currOrderType == Enums.OrderType.Gift)
        //    {
        //        if (orderRequestModel.skus.Count > 1)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "只能购买一个商品";
        //            return resp;
        //        }
        //    }

        //    if (IsGroupByLeader(orderRequestModel))
        //    {

        //        if (orderRequestModel.skus.Count > 1)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "只能购买一个商品";
        //            return resp;
        //        }

        //        if (string.IsNullOrEmpty(orderRequestModel.rule_id))
        //        {
        //            resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
        //            resp.Msg = "rule_id 必传";
        //            return resp;
        //        }

        //        if (orderRequestModel.skus.Sum(p => p.count) > 1)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
        //            resp.Msg = "团购商品只能购买一件";
        //            return resp;
        //        }

        //    }

        //    #endregion

        //    orderRequestModel.skus = orderRequestModel.skus.DistinctBy(p => p.sku_id).ToList();

        //    List<ProductGroupBuyRule> groupBuyList = new List<ProductGroupBuyRule>();//此商品的团购规则列表

        //    #region 购买的商品

        //    List<WXMallProductInfo> productList = new List<WXMallProductInfo>();


        //    foreach (var sku in orderRequestModel.skus)
        //    {
        //        ProductSku productSku = bllMall.GetProductSku(sku.sku_id);

        //        if (productSku == null) continue;
        //        if (productList.Count(p => p.PID == productSku.ProductId.ToString()) > 0) continue;

        //        WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());

        //        if (productInfo.IsNoExpress == 0)
        //        {
        //            isNoExpress = 0;
        //        }

        //        if (productInfo.IsNeedNamePhone == 1)
        //        {
        //            isNeedNamePhone = 1;
        //        }

        //        productList.Add(productInfo);
        //        groupBuyList = bllMall.GetProductGroupBuyRuleList(productSku.ProductId.ToString());
        //    }

        //    orderInfo.IsNoExpress = isNoExpress;
        //    orderInfo.IsNeedNamePhone = isNeedNamePhone;

        //    //productList = productList.Distinct().ToList();
        //    #endregion

        //    ProductGroupBuyRule groupBuyRule = new ProductGroupBuyRule();
        //    if (IsGroupByLeader(orderRequestModel))
        //    {
        //        //团购、团长下单
        //        if (groupBuyList.Count == 0)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "此商品不能团购";
        //            return resp;
        //        }
        //        if (groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id)) == null)
        //        {
        //            resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            resp.Msg = "rule_id 错误,请检查";
        //            return resp;
        //        }
        //        groupBuyRule = groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id));//团购规则
        //    }


        //    #region 收货人信息格式检查



        //    //相关检查

        //    if (isNoExpress == 0)
        //    {

        //        if (string.IsNullOrEmpty(orderInfo.Consignee))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "姓名不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.Phone))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "手机不能为空";
        //            return resp;
        //        }
        //        if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(orderInfo.Phone))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "手机格式不正确";
        //            return resp;
        //        }

        //        if (string.IsNullOrEmpty(orderInfo.ReceiverProvince))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "省份名称不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.ReceiverProvinceCode))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "省份代码不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.ReceiverCity))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "城市名称不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "城市代码不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.ReceiverDist))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "城市区域名称不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "城市区域代码不能为空";
        //            return resp;
        //        }
        //        if (string.IsNullOrEmpty(orderInfo.Address))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "收货地址不能为空";
        //            return resp;
        //        }

        //    }
        //    else
        //    {
        //        if (isNeedNamePhone == 1)
        //        {
        //            if (string.IsNullOrEmpty(orderInfo.Consignee))
        //            {
        //                resp.Code = 1;
        //                resp.Msg = "姓名不能为空";
        //                return resp;
        //            }
        //            if (string.IsNullOrEmpty(orderInfo.Phone))
        //            {
        //                resp.Code = 1;
        //                resp.Msg = "手机不能为空";
        //                return resp;
        //            }
        //            if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(orderInfo.Phone))
        //            {
        //                resp.Code = 1;
        //                resp.Msg = "手机格式不正确";
        //                return resp;
        //            }
        //        }
        //    }

        //    //相关检查 
        //    #endregion


        //    #region 商品检查 订单详情生成
        //    ///订单详情

        //    string cardCouponType = "";//优惠券类型
        //    MyCardCoupons mycardCoupon = new MyCardCoupons();
        //    #region 检查优惠券储值卡是否可用
        //    if (orderRequestModel.cardcoupon_id > 0)
        //    {

        //        mycardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, currentUserInfo.UserID);
        //        if (mycardCoupon != null)//优惠券
        //        {
        //            #region 优惠券

        //            if (mycardCoupon == null)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = "无效的优惠券";
        //                return resp;
        //            }
        //            var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
        //            if (cardCoupon == null)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = "无效的优惠券";
        //                return resp;
        //            }
        //            cardCouponType = cardCoupon.CardCouponType;
        //            #region 需要购买指定商品
        //            if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
        //            {

        //                if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
        //                {
        //                    var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
        //                    resp.Code = 1;
        //                    resp.Msg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
        //                    return resp;
        //                }


        //            }
        //            #endregion

        //            //#region 需要购买指定标签商品
        //            //if (!string.IsNullOrEmpty(cardCoupon.Ex8))
        //            //{
        //            //    if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
        //            //    {
        //            //        resp.Code = 1;
        //            //        resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
        //            //        return resp;

        //            //    }

        //            //    bool checkResult = true;
        //            //    foreach (var product in productList)
        //            //    {
        //            //        if (!string.IsNullOrEmpty(product.Tags))
        //            //        {
        //            //            bool tempResult = false;
        //            //            foreach (string tag in product.Tags.Split(','))
        //            //            {
        //            //                if (cardCoupon.Ex8.Contains(tag))
        //            //                {
        //            //                    tempResult = true;
        //            //                    break;
        //            //                }
        //            //            }
        //            //            if (!tempResult)
        //            //            {
        //            //                checkResult = false;
        //            //                break;
        //            //            }


        //            //        }
        //            //        else//商品不包含标签
        //            //        {

        //            //            checkResult = false;
        //            //            break;
        //            //        }

        //            //    }
        //            //    if (!checkResult)
        //            //    {
        //            //        resp.Code = 1;
        //            //        resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
        //            //        return resp;
        //            //    }

        //            //}
        //            //#endregion
        //            #region 需要购买指定标签商品 只要购买一种符合标签的商品即可
        //            if (!string.IsNullOrEmpty(cardCoupon.Ex8))
        //            {
        //                if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
        //                {
        //                    resp.Code = 1;
        //                    resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
        //                    return resp;

        //                }

        //                bool checkResult = false;
        //                foreach (var product in productList)
        //                {
        //                    if (!string.IsNullOrEmpty(product.Tags))
        //                    {

        //                        foreach (string tag in product.Tags.Split(','))
        //                        {
        //                            if (cardCoupon.Ex8.Contains(tag))
        //                            {
        //                                checkResult = true;
        //                                break;
        //                            }
        //                        }


        //                    }
        //                    //else
        //                    //{

        //                    //    checkResult = false;
        //                    //    break;
        //                    //}

        //                }
        //                if (!checkResult)
        //                {
        //                    resp.Code = 1;
        //                    resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
        //                    return resp;
        //                }

        //            }
        //            #endregion
        //            #endregion
        //        }
        //        else//储值卡
        //        {
        //            //#region 储值卡
        //            ////检查是否使用了积分余额
        //            //if (orderRequestModel.use_amount > 0 || orderRequestModel.use_score > 0)
        //            //{
        //            //    resp.Code = (int)Enums.APIErrCode.OperateFail;
        //            //    resp.Msg = "使用储值卡时,不可再用积分或余额抵扣,请取消使用积分或余额抵扣";
        //            //    return resp;

        //            //}
        //            //#endregion
        //        }



        //    }
        //    #endregion

        //    var needUseScore = 0;//必须使用的积分
        //    decimal needUseCash = 0;//必须使用的现金

        //    List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情

        //    foreach (var sku in orderRequestModel.skus)
        //    {
        //        //先检查库存
        //        ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
        //        if (productSku == null)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "SKU不存在";
        //            return resp;
        //        }

        //        //WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
        //        WXMallProductInfo productInfo = productList.Single(p => p.PID == productSku.ProductId.ToString());
        //        if (productInfo.IsOnSale == "0")
        //        {
        //            resp.Code = 1;
        //            resp.Msg = string.Format("{0}已下架", productInfo.PName);
        //            return resp;
        //        }
        //        if (productInfo.IsDelete == 1)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = string.Format("{0}已下架", productInfo.PName);
        //            return resp;
        //        }
        //        if (bllMall.GetSkuCount(productSku) < sku.count)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = string.Format("{0}{1}库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.Stock);
        //            return resp;
        //        }
        //        int promotionActivityId = 0;
        //        if (bllMall.IsPromotionTime(productSku))
        //        {
        //            //

        //            promotionActivityId = productList.First(p => p.PID == productSku.ProductId.ToString()).PromotionActivityId;

        //            int historyCount = bllMall.GetList<WXMallOrderDetailsInfo>(String.Format("PromotionActivityId={0} And PID={1} And OrderId In (Select OrderId From ZCJ_WXMallOrderInfo where OrderUserId='{2}')", promotionActivityId, productSku.ProductId, currentUserInfo.UserID)).Sum(p => p.TotalCount);
        //            BLLJIMP.Model.PromotionActivity activity = bllMall.GetPromotionActivity(promotionActivityId);

        //            int buyCount = 0;
        //            List<ProductSku> currProductSkuList = bllMall.GetProductSkuList(productSku.ProductId);
        //            foreach (var item in currProductSkuList)
        //            {
        //                if (orderRequestModel.skus.SingleOrDefault(p => p.sku_id == item.SkuId) != null)
        //                {
        //                    buyCount += sku.count;
        //                }
        //            }


        //            if (((historyCount + buyCount) > activity.LimitBuyProductCount) && activity.LimitBuyProductCount > 0)
        //            {
        //                string msgF = string.Format("{0}最多还可购买{1}件", productInfo.PName, activity.LimitBuyProductCount - historyCount);
        //                if (activity.LimitBuyProductCount - historyCount == 0)
        //                {
        //                    msgF = string.Format("{0}已经达到最多购买数量", productInfo.PName);
        //                }
        //                resp.Code = 1;
        //                resp.Msg = msgF;
        //                return resp;

        //            }


        //            //

        //            if (sku.count > productSku.PromotionStock)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = string.Format("{0}{1}特卖库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.PromotionStock);
        //                return resp;

        //            }


        //        }
        //        if (bllMall.IsLimitProductTime(productInfo, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = string.Format("{0} 暂时不能购买", productInfo.PName);
        //            return resp;
        //        }


        //        if (productInfo.IsAppointment == 1)
        //        {
        //            var appointmentInfo = bllMall.GetProductAppointmentInfo(productInfo);
        //            if (appointmentInfo.status == 0)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = string.Format("{0} 暂未到预购时间", productInfo.PName);
        //                return resp;
        //            }
        //            if (appointmentInfo.status == 2)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = string.Format("{0} 已经超过预购时间", productInfo.PName);
        //                return resp;
        //            }
        //            orderInfo.IsAppointment = 1;
        //        }



        //        if (productInfo.Score > 0)//必须使用的积分
        //        {
        //            needUseScore += sku.count * productInfo.Score;
        //        }


        //        WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
        //        detailModel.OrderID = orderInfo.OrderID;
        //        detailModel.PID = productInfo.PID;
        //        detailModel.TotalCount = sku.count;

        //        detailModel.OrderScore = productInfo.Score;
        //        detailModel.ProductName = productInfo.PName;
        //        detailModel.SkuId = productSku.SkuId;
        //        detailModel.SkuShowProp = bllMall.GetProductShowProperties(productSku.SkuId);
        //        detailModel.ArticleCategoryType = "Mall";
        //        detailModel.PromotionActivityId = promotionActivityId.ToString();
        //        detailModel.ExQuestionnaireID = productSku.ExQuestionnaireID;
        //        var productPrice = productInfo.Price;
        //        var skuPrice = productSku.Price;

        //        if (IsGroupByLeader(orderRequestModel))
        //        {
        //            //团长折扣
        //            productPrice = Math.Round((decimal)(productInfo.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2);
        //            skuPrice = Math.Round((decimal)(productSku.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2);
        //        }
        //        else if (IsGroupByMember(orderRequestModel))
        //        {
        //            //团员折扣
        //            productPrice = Math.Round((decimal)(productInfo.Price * (decimal)(parentOrderInfo.MemberDiscount / 10)), 2);
        //            skuPrice = Math.Round((decimal)(productSku.Price * (decimal)(parentOrderInfo.MemberDiscount / 10)), 2);
        //        }
        //        else
        //        {
        //            //通用商城下单（包括限时特卖）
        //            skuPrice = bllMall.GetSkuPrice(productSku);
        //        }

        //        if (productInfo.IsCashPayOnly == 1)//必须使用的现金
        //        {
        //            needUseCash += (productInfo.Score > 0 ? productPrice : skuPrice) * sku.count;
        //        }
        //        detailModel.OrderPrice = productInfo.Score > 0 ? productPrice : skuPrice;
        //        detailModel.IsComplete = 1;
        //        detailList.Add(detailModel);

        //    }
        //    #endregion

        //    #region 纯积分购买
        //    if (needUseScore > 0)
        //    {
        //        if ((currentUserInfo.TotalScore < needUseScore) || (orderRequestModel.use_score < needUseScore))
        //        {

        //            resp.Code = 1;
        //            resp.Msg = string.Format("您需要使用{0}积分来兑换， 可用积分不足", needUseScore);
        //            return resp;

        //        }

        //    }
        //    #endregion

        //    productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
        //    //物流费用
        //    #region 运费计算
        //    FreightModel freightModel = new FreightModel();
        //    freightModel.receiver_province_code = orderRequestModel.receiver_province_code;
        //    freightModel.receiver_city_code = orderRequestModel.receiver_city_code;
        //    freightModel.receiver_dist_code = orderRequestModel.receiver_dist_code;
        //    freightModel.skus = orderRequestModel.skus;
        //    decimal freight = 0;//运费
        //    string freightMsg = "";

        //    if (isNoExpress == 0)//判断是否无需物流
        //    {
        //        if (!bllMall.CalcFreight(freightModel, out freight, out freightMsg))
        //        {
        //            resp.Code = 1;
        //            resp.Msg = freightMsg;
        //            return resp;

        //        }
        //    }
        //    orderInfo.Transport_Fee = freight;
        //    #endregion

        //    #region 优惠券或储值卡优惠计算
        //    StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
        //    decimal discountAmount = 0;//优惠金额
        //    bool canUseCardCoupon = false;
        //    string msg = "";
        //    if (orderRequestModel.cardcoupon_id > 0)//有优惠券
        //    {
        //        #region 优惠券
        //        if (mycardCoupon != null)
        //        {
        //            discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, currentUserInfo.UserID, out canUseCardCoupon, out msg, currOrderType, orderRequestModel.groupbuy_type, productFee);
        //            if (!canUseCardCoupon)
        //            {
        //                resp.Code = 1;
        //                resp.Msg = msg;
        //                return resp;
        //            }
        //            //if (discountAmount > productFee + orderInfo.Transport_Fee)
        //            //{
        //            //    resp.Code = 1;
        //            //    resp.Msg = "优惠券可优惠金额超过了订单总金额";
        //            //    return resp;

        //            //}


        //            if (cardCouponType == "MallCardCoupon_FreeFreight")//免邮券
        //            {
        //                orderInfo.Transport_Fee = 0;
        //            }
        //        }
        //        #endregion

        //        #region 储值卡
        //        else
        //        {
        //            storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", orderRequestModel.cardcoupon_id));
        //            discountAmount = bllMall.CalcDiscountAmountStoreValue(productFee + freight, storeValueCardRecord.UserId, orderRequestModel.cardcoupon_id.ToString());

        //        }
        //        #endregion




        //    }
        //    //优惠券计算 


        //    #endregion

        //    #region 积分计算
        //    decimal scoreExchangeAmount = 0;///积分抵扣的金额
        //    //积分计算
        //    if (orderRequestModel.use_score > 0)
        //    {

        //        #region 使用驿氪积分
        //        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
        //        {

        //            if ((!string.IsNullOrEmpty(currentUserInfo.Phone)))
        //            {
        //                Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus(currentUserInfo.Ex1, currentUserInfo.Ex2, currentUserInfo.Phone);
        //                if (yikeUser != null)
        //                {

        //                    currentUserInfo.TotalScore = yikeUser.Bonus;

        //                }
        //                else//查不到yike用户
        //                {
        //                    resp.Code = 1;
        //                    resp.Msg = "您的账号尚未注册,请先注册";
        //                    return resp;
        //                }

        //            }

        //        }
        //        #endregion

        //        #region 使用宏巍积分
        //        if (websiteInfo.IsUnionHongware == 1)
        //        {

        //            currentUserInfo.TotalScore = hongWeiWareMemberInfo.member.point;

        //        }
        //        #endregion



        //        if (currentUserInfo.TotalScore < orderRequestModel.use_score)
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "积分不足";
        //            return resp;
        //        }


        //        ScoreConfig scoreConfig = bllScore.GetScoreConfig();
        //        if (scoreConfig != null && scoreConfig.ExchangeAmount > 0)
        //        {
        //            var tmpUseScore = orderRequestModel.use_score;

        //            if (detailList != null)
        //            {
        //                tmpUseScore -= detailList.Sum(p => p.TotalCount * p.OrderScore);
        //            }

        //            scoreExchangeAmount = Math.Round(tmpUseScore / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);

        //            var totalProductAmount = detailList.Sum(p => p.TotalCount * p.OrderPrice);

        //            if (scoreExchangeAmount > totalProductAmount)
        //            {
        //                scoreExchangeAmount = totalProductAmount.Value;
        //            }


        //        }

        //    }



        //    //积分计算 
        //    #endregion

        //    #region 使用账户余额
        //    if (orderRequestModel.use_amount > 0)
        //    {
        //        if (!bllMall.IsEnableAccountAmountPay())
        //        {
        //            resp.Code = 1;
        //            resp.Msg = "尚未启用余额支付功能";
        //            return resp;
        //        }
        //        #region 使用宏巍余额
        //        if (websiteInfo.IsUnionHongware == 1)
        //        {
        //            currentUserInfo.AccountAmount = (decimal)hongWeiWareMemberInfo.member.balance;

        //        }
        //        #endregion


        //        if (currentUserInfo.AccountAmount < orderRequestModel.use_amount)
        //        {

        //            resp.Code = 1;
        //            resp.Msg = "您的账户余额不足";
        //            return resp;
        //        }
        //    }
        //    #endregion

        //    //合计计算
        //    orderInfo.Product_Fee = productFee;
        //    orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
        //    if (cardCouponType != "MallCardCoupon_FreeFreight")//免邮券不算
        //    {
        //        orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额

        //    }
        //    orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
        //    orderInfo.TotalAmount -= orderRequestModel.use_amount;//使用余额
        //    orderInfo.PayableAmount = orderInfo.TotalAmount - freight;//应付金额
        //    if (orderInfo.TotalAmount < 0)
        //    {
        //        orderInfo.TotalAmount = 0;
        //    }
        //    if (orderInfo.PayableAmount < 0)
        //    {
        //        orderInfo.PayableAmount = 0;
        //    }
        //    orderInfo.ScoreExchangAmount = scoreExchangeAmount;//积分抵扣金额
        //    orderInfo.CardcouponDisAmount = discountAmount;//卡券抵扣金额

        //    if (IsGroupByLeader(orderRequestModel))
        //    {
        //        orderInfo.HeadDiscount = groupBuyRule.HeadDiscount;
        //        orderInfo.MemberDiscount = groupBuyRule.MemberDiscount;
        //        orderInfo.PeopleCount = groupBuyRule.PeopleCount;
        //        orderInfo.ExpireDay = groupBuyRule.ExpireDay;

        //    }

        //    if (IsGroupByMember(orderRequestModel))
        //    {
        //        orderInfo.HeadDiscount = parentOrderInfo.HeadDiscount;
        //        orderInfo.MemberDiscount = parentOrderInfo.MemberDiscount;
        //        orderInfo.PeopleCount = parentOrderInfo.PeopleCount;
        //        orderInfo.ExpireDay = parentOrderInfo.ExpireDay;
        //        orderInfo.GroupBuyParentOrderId = parentOrderInfo.OrderID;
        //    }

        //    //if ((productFee + orderInfo.Transport_Fee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
        //    //{
        //    //    resp.Code = 1;
        //    //    resp.Msg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
        //    //    return resp;

        //    //}


        //    #region 均摊价计算
        //    detailList = CalcPaymentFt(orderInfo.TotalAmount - orderInfo.Transport_Fee, detailList);
        //    #endregion

        //    #region 最大退款金额计算
        //    detailList = CalcMaxRefundAmount(orderInfo.TotalAmount - orderInfo.Transport_Fee, detailList);
        //    #endregion

        //    if (orderInfo.TotalAmount == 0)
        //    {
        //        orderInfo.PaymentStatus = 1;
        //        orderInfo.PayTime = DateTime.Now;
        //        orderInfo.Status = "待发货";
        //        if (orderInfo.OrderType == 2)
        //        {
        //            orderInfo.GroupBuyStatus = "0";
        //            orderInfo.DistributionStatus = 1;

        //        }
        //    }
        //    if (orderInfo.TotalAmount + orderRequestModel.use_amount < needUseCash)
        //    {
        //        resp.Code = 1;
        //        resp.Msg = string.Format("最少需要支付{0}元", needUseCash);
        //        return resp;
        //    }

        //    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

        //    try
        //    {

        //        if (!Add(orderInfo, tran))
        //        {
        //            tran.Rollback();
        //            resp.Code = 1;
        //            resp.Msg = "提交失败";
        //            return resp;
        //        }

        //        #region 更新优惠券储值卡使用状态

        //        #region 优惠券
        //        if (orderRequestModel.cardcoupon_id > 0 && (canUseCardCoupon == true) && mycardCoupon != null)//有优惠券且已经成功使用
        //        {

        //            //mycardCoupon= bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, currentUserInfo.UserID);
        //            mycardCoupon.UseDate = DateTime.Now;
        //            mycardCoupon.Status = 1;
        //            if (!bllCardCoupon.Update(mycardCoupon, tran))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "更新优惠券状态失败";
        //                return resp;
        //            }

        //        }
        //        #endregion

        //        #region 储值卡
        //        if (orderRequestModel.cardcoupon_id > 0 && mycardCoupon == null)
        //        {
        //            StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And (UserId='{1}' Or ToUserId='{1}')", orderRequestModel.cardcoupon_id, currentUserInfo.UserID));
        //            myStoredValueCardRecord.UseDate = DateTime.Now;
        //            myStoredValueCardRecord.Status = 9;
        //            if (!bllStoreValue.Update(myStoredValueCardRecord, tran))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "更新储值卡状态失败";
        //                return resp;
        //            }
        //            StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
        //            storeValueCardUseRecord.UseAmount = discountAmount;
        //            storeValueCardUseRecord.CardId = myStoredValueCardRecord.CardId;
        //            storeValueCardUseRecord.Remark = string.Format("商城下单,订单号:{0}使用{1}元", orderInfo.OrderID, Math.Round(discountAmount, 2));
        //            storeValueCardUseRecord.UseDate = DateTime.Now;
        //            storeValueCardUseRecord.UserId = storeValueCardRecord.UserId;
        //            storeValueCardUseRecord.UseUserId = currentUserInfo.UserID;
        //            storeValueCardUseRecord.WebsiteOwner = orderInfo.WebsiteOwner;
        //            storeValueCardUseRecord.MyCardId = orderRequestModel.cardcoupon_id;
        //            storeValueCardUseRecord.OrderId = orderInfo.OrderID;
        //            if (!bllStoreValue.Add(storeValueCardUseRecord, tran))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "更新储值卡状态失败";
        //                return resp;
        //            }



        //        }
        //        #endregion

        //        #endregion

        //        #region 积分抵扣
        //        //积分扣除
        //        if (orderRequestModel.use_score > 0)
        //        {
        //            currentUserInfo.TotalScore -= orderRequestModel.use_score;
        //            if (Update(currentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "更新用户积分失败";
        //                return resp;
        //            }

        //            //积分记录
        //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
        //            scoreRecord.AddTime = DateTime.Now;
        //            scoreRecord.Score = -orderRequestModel.use_score;
        //            scoreRecord.TotalScore = currentUserInfo.TotalScore;
        //            scoreRecord.ScoreType = "OrderSubmit";
        //            scoreRecord.UserID = currentUserInfo.UserID;
        //            scoreRecord.AddNote = "微商城-下单使用积分";
        //            scoreRecord.RelationID = orderInfo.OrderID;
        //            scoreRecord.WebSiteOwner = WebsiteOwner;
        //            if (!bllMall.Add(scoreRecord))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "插入积分记录失败";
        //                return resp;
        //            }

        //            #region 驿氪积分同步

        //            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
        //            {


        //                yiKeClient.BonusUpdate(currentUserInfo.Ex2, -(orderRequestModel.use_score), "商城下单使用积分" + orderRequestModel.use_score);


        //            }

        //            #endregion

        //            #region 更新宏巍积分
        //            if (websiteInfo.IsUnionHongware == 1)
        //            {
        //                if (!hongWareClient.UpdateMemberScore(hongWeiWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, -orderRequestModel.use_score))
        //                {
        //                    tran.Rollback();
        //                    resp.Code = 1;
        //                    resp.Msg = "更新宏巍积分失败";
        //                    return resp;
        //                }

        //            }
        //            #endregion



        //        }

        //        //积分扣除 
        //        #endregion

        //        #region 余额抵扣

        //        if (orderRequestModel.use_amount > 0 && bllMall.IsEnableAccountAmountPay())
        //        {
        //            currentUserInfo.AccountAmount -= orderRequestModel.use_amount;
        //            if (Update(currentUserInfo, string.Format(" AccountAmount={0}", currentUserInfo.AccountAmount), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "更新用户余额失败";
        //                return resp;
        //            }

        //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
        //            scoreRecord.AddTime = DateTime.Now;
        //            scoreRecord.Score = -(double)orderRequestModel.use_amount;
        //            scoreRecord.TotalScore = (double)currentUserInfo.AccountAmount;
        //            scoreRecord.UserID = currentUserInfo.UserID;
        //            scoreRecord.AddNote = "微商城-下单使用余额";
        //            scoreRecord.RelationID = orderInfo.OrderID;
        //            scoreRecord.WebSiteOwner = WebsiteOwner;
        //            scoreRecord.ScoreType = "AccountAmount";
        //            if (!bllMall.Add(scoreRecord))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "插入余额记录失败";
        //                return resp;
        //            }

        //            #region 更新宏巍余额
        //            if (websiteInfo.IsUnionHongware == 1)
        //            {
        //                if (!hongWareClient.UpdateMemberBlance(hongWeiWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, -(float)orderRequestModel.use_amount))
        //                {
        //                    tran.Rollback();
        //                    resp.Code = 1;
        //                    resp.Msg = "更新宏巍余额失败";
        //                    return resp;
        //                }

        //            }
        //            #endregion





        //        }


        //        #endregion

        //        #region 插入订单详情表及更新库存
        //        foreach (var item in detailList)
        //        {
        //            ProductSku productSku = bllMall.GetProductSku((int)(item.SkuId));
        //            WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
        //            if (!Add(item, tran))
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "提交失败";
        //                return resp;
        //            }
        //            //更新 SKU库存 
        //            System.Text.StringBuilder sbUpdateStock = new StringBuilder();//更新库存sql
        //            sbUpdateStock.AppendFormat(" Update ZCJ_ProductSku Set Stock-={0} ", item.TotalCount);
        //            if (bllMall.IsPromotionTime(productSku))
        //            {
        //                sbUpdateStock.AppendFormat(",PromotionStock-={0} ", item.TotalCount);
        //            }
        //            sbUpdateStock.AppendFormat(" Where SkuId={0} And Stock>0 ", productSku.SkuId);
        //            if (bllMall.IsPromotionTime(productSku))
        //            {
        //                sbUpdateStock.AppendFormat(" And PromotionStock>0 ", item.TotalCount);
        //            }
        //            if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbUpdateStock.ToString(), tran) <= 0)
        //            {
        //                tran.Rollback();
        //                resp.Code = 1;
        //                resp.Msg = "提交订单失败,库存不足";
        //                return resp;
        //            }




        //        }
        //        #endregion

        //        bllMall.DeleteShoppingCart(currentUserInfo.UserID, orderRequestModel.skus);

        //        tran.Commit();//提交订单事务



        //        #region 宏巍通知
        //        if (websiteInfo.IsUnionHongware == 1)
        //        {
        //            hongWareClient.OrderNotice(currentUserInfo.WXOpenId, orderInfo.OrderID);

        //        }
        //        #endregion

        //        try
        //        {

        //            #region 团购完成取消其它未付款订单
        //            if (orderInfo.OrderType == 2)
        //            {
        //                //团购完成取消其它未付款订单
        //                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And  (GroupBuyParentOrderId='{0}' Or OrderId='{0}')", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
        //                {
        //                    bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
        //                    parentOrderInfo.GroupBuyStatus = "1";
        //                    bllMall.Update(parentOrderInfo);

        //                }
        //            }
        //            #endregion

        //            #region 微信模板消息

        //            if (orderInfo.TotalAmount > 0)
        //            {
        //                var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;

        //                string title = "订单已成功提交";
        //                if (orderInfo.TotalAmount > 0)
        //                {
        //                    title += ",请尽快付款";
        //                }
        //                string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
        //                bllWeiXin.SendTemplateMessageNotifyComm(currentUserInfo, title, string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品：{4}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName), url);

        //            }
        //            else
        //            {
        //                var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;
        //                string remark = "";
        //                if (!string.IsNullOrEmpty(orderInfo.OrderMemo))
        //                {
        //                    remark = string.Format("客户留言:{0}", orderInfo.OrderMemo);
        //                }
        //                bllWeiXin.SendTemplateMessageToKefu("有新的订单", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}\\n{5}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName, remark));
        //            }

        //            #endregion

        //            if (websiteInfo.WebsiteOwner != "mixblu")
        //            {
        //                //bllScore.ToLog("判断订单类型是否可获得积分", "D://DALlog.txt");
        //                //判断订单类型是否可获得积分                      
        //                if (bllScore.IsCanRebateScoreByOrderType(currOrderType, orderRequestModel.groupbuy_type))
        //                {
        //                    //bllScore.ToLog("订单类型支持获得积分", "D://DALlog.txt");
        //                    //即将到账积分计算，并存入即将到账积分表，若取消、退款、删除订单，积分到账应该打上移除标志并追加有备注内容            
        //                    bllScore.AddLockScoreByOrder(orderInfo);
        //                }
        //            }
        //        }
        //        catch { }

        //        if (orderInfo.TotalAmount == 0)
        //        {
        //            #region Efast同步
        //            //判读当前站点是否需要同步到驿氪和efast
        //            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
        //            {
        //                try
        //                {

        //                    string outOrderId = string.Empty, msg1 = string.Empty;
        //                    var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out msg1);
        //                    if (syncResult)
        //                    {
        //                        orderInfo.OutOrderId = outOrderId;
        //                        Update(orderInfo);
        //                    }

        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            #endregion

        //            #region 驿氪同步
        //            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
        //            {
        //                try
        //                {

        //                    var uploadOrderResult = yiKeClient.OrderUpload(orderInfo);

        //                }
        //                catch (Exception ex)
        //                {


        //                }
        //            }
        //            #endregion

        //            #region 更新销量
        //            bllMall.UpdateProductSaleCount(orderInfo);
        //            #endregion

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //回滚事物
        //        tran.Rollback();
        //        resp.Code = 1;
        //        resp.Msg = "提交订单失败";
        //        return resp;
        //    }

        //    resp.Code = 0;
        //    resp.Msg = "ok";
        //    resp.OrderId = orderInfo.OrderID;

        //    return resp;

        //}

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public AddOrderResp Add(HttpContext context)
        {
            try
            {



                BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");//菜单权限
                BLLUser bllUser = new BLLUser();//用户
                BLLDistribution bllDis = new BLLDistribution();//分销
                BLLMall bllMall = new BLLMall();//商城
                BLLCardCoupon bllCardCoupon = new BLLCardCoupon();//卡券
                BLLCommRelation bllCommRelation = new BLLCommRelation();//公共
                BllScore bllScore = new BllScore();//积分
                BLLWeixin bllWeiXin = new BLLWeixin();//微信
                BLLEfast bllEfast = new BLLEfast();//Efast
                BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();//储值卡
                BLLWeixinCard bllWeixinCard = new BLLWeixinCard();//微信卡包
                BLLJuActivity bllJuactivity = new BLLJuActivity();
                BLLWebSite bllCompanyWebsite = new BLLWebSite();
                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(WebsiteOwner);//宏巍接口
                AddOrderResp resp = new AddOrderResp();//响应模型
                var currentUserInfo = GetCurrentUserInfo();//当前用户
                var currOrderType = Enums.OrderType.Normal;//订单类型
                string data = context.Request["data"];//订单数据json字符串
                decimal productFee = 0;//商品总价格(应付)
                OrderRequestModel orderRequestModel;//订单请求模型
                try
                {
                    orderRequestModel = Common.JSONHelper.JsonToModel<OrderRequestModel>(data);
                    if (string.IsNullOrEmpty(orderRequestModel.supplier_id))
                    {
                        orderRequestModel.supplier_id = "";
                    }
                }
                catch (Exception ex)
                {
                    resp.Code = 1;
                    resp.Msg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                    return resp;
                }
                WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase();//当前站点信息
                CompanyWebsite_Config companyConfig = bllCompanyWebsite.GetCompanyWebsiteConfig();

                Open.HongWareSDK.MemberInfo hongWeiWareMemberInfo = null; ///宏巍会员信息
                if (websiteInfo.IsUnionHongware == 1)
                {
                    hongWeiWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
                    if (hongWeiWareMemberInfo.member == null)
                    {
                        resp.Code = 1;
                        resp.Msg = "您尚未绑定宏巍账号,请先绑定";
                        return resp;
                    }
                }

                WXMallOrderInfo parentOrderInfo = new WXMallOrderInfo();

                #region 参团前置判断
                if (IsGroupByMember(orderRequestModel))
                {
                    if (string.IsNullOrEmpty(orderRequestModel.groupbuy_parent_orderid))
                    {
                        resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
                        resp.Msg = "groupbuy_parent_orderid 必传";
                        return resp;
                    }

                    parentOrderInfo = bllMall.GetOrderInfo(orderRequestModel.groupbuy_parent_orderid);//父订单
                    if (parentOrderInfo == null)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "订单不存在";
                        return resp;
                    }
                    //if (parentOrderInfo.OrderUserID == currentUserInfo.UserID)
                    //{
                    //    resp.Code = (int)Enums.APIErrCode.OperateFail;
                    //    resp.Msg = "团长不可以参加";
                    //    return resp;
                    //}
                    if (parentOrderInfo.OrderType != 2)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "不是拼团订单";
                        return resp;
                    }
                    //if (!string.IsNullOrEmpty(parentOrderInfo.GroupBuyParentOrderId))
                    //{
                    //    resp.Code = (int)Enums.APIErrCode.OperateFail;
                    //    resp.Msg = "订单无效";
                    //    return resp;
                    //}
                    if (parentOrderInfo.PaymentStatus == 0)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "团长订单未付款";
                        return resp;
                    }
                    if (parentOrderInfo.Ex10 == "1")
                    {
                        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                        {

                            resp.Code = (int)Enums.APIErrCode.OperateFail;
                            resp.Msg = "团购人数已满";
                            return resp;

                        }
                    }
                    else
                    {
                        if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' Or OrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                        {

                            resp.Code = (int)Enums.APIErrCode.OperateFail;
                            resp.Msg = "团购人数已满";
                            return resp;

                        }
                    }

                    if (bllMall.GetCount<WXMallOrderInfo>(string.Format("GroupBuyParentOrderId='{0}' And OrderUserId='{1}' And PaymentStatus=0", parentOrderInfo.OrderID, currentUserInfo.UserID)) > 0)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "您还有未支付的订单,请先支付";
                        return resp;
                    }
                    if (DateTime.Now >= (((DateTime)parentOrderInfo.PayTime).AddDays(parentOrderInfo.ExpireDay)))
                    {
                        parentOrderInfo.GroupBuyStatus = "2";
                        bllMall.Update(parentOrderInfo);
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "拼团已过期";
                        return resp;
                    }


                    #region 团长分销关系建立
                    if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                    {
                        if (string.IsNullOrEmpty(currentUserInfo.DistributionOwner) && parentOrderInfo.OrderUserID != currentUserInfo.UserID)
                        {
                            //设置用户上级
                            var setUserDistributionOwnerResult =
                                bllDis.SetUserDistributionOwner(currentUserInfo.UserID, parentOrderInfo.OrderUserID, currentUserInfo.WebsiteOwner);

                            if (setUserDistributionOwnerResult)
                            {
                                currentUserInfo.DistributionOwner = parentOrderInfo.OrderUserID;
                            }

                        }
                    }
                    #endregion



                    #region 参团订单的skuid构造
                    List<SkuModel> orderSkuList = new List<SkuModel>();

                    var parentOrderDetailList = bllMall.GetOrderDetailsList(parentOrderInfo.OrderID);

                    orderSkuList.Add(new SkuModel()
                    {
                        sku_id = parentOrderDetailList[0].SkuId.Value,
                        count = 1
                    });

                    orderRequestModel.skus = orderSkuList;
                    #endregion

                }

                #endregion

                WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表

                //确定订单类型
                orderInfo.OrderType = orderRequestModel.order_type;

                try
                {
                    currOrderType = (Enums.OrderType)orderInfo.OrderType;
                }
                catch (Exception ex)
                {
                    resp.Code = (int)Enums.APIErrCode.MallOrderTypeNotExsit;
                    resp.Msg = "订单类型不存在";
                    return resp;

                }

                orderInfo.ClaimArrivalTime = orderRequestModel.claim_arrival_time;

                orderInfo.CustomCreaterName = orderRequestModel.custom_creater_name;
                orderInfo.CustomCreaterPhone = orderRequestModel.custom_creater_phone;

                orderInfo.Address = orderRequestModel.receiver_address;
                orderInfo.Consignee = orderRequestModel.receiver_name;
                orderInfo.ExpressCompanyName = orderRequestModel.express_company;
                orderInfo.InsertDate = DateTime.Now;
                orderInfo.OrderUserID = currentUserInfo.UserID;
                orderInfo.Phone = orderRequestModel.receiver_phone;
                orderInfo.WebsiteOwner = WebsiteOwner;
                orderInfo.Transport_Fee = 0;
                orderInfo.OrderID = GetGUID(BLLJIMP.TransacType.AddMallOrder);
                orderInfo.DeliveryType = Convert.ToInt32(orderRequestModel.delivery_type);
                //Ex21已作为后台备注字段，后面尽量不要用，系统级别字段应该直接建立新的不应该用ex
                //orderInfo.Ex21 = orderRequestModel.freight_remark;
                #region 有供应商传入
                if (!string.IsNullOrEmpty(orderRequestModel.supplier_id))
                {
                    var supplierInfo = bllMall.GetSuppLierByAutoId(int.Parse(orderRequestModel.supplier_id));
                    if (supplierInfo != null)
                    {
                        orderInfo.SupplierUserId = supplierInfo.UserID;
                        orderInfo.SupplierName = supplierInfo.Company;

                        if (orderInfo.DeliveryType == 1)
                        {
                            var storeInfo = bllJuactivity.GetStoreById(supplierInfo.AutoID.ToString());
                            if (storeInfo != null)
                            {
                                orderInfo.StoreAddress = storeInfo.Province + storeInfo.City + storeInfo.District + storeInfo.ActivityAddress;
                            }
                        }

                    }

                }
                #endregion


                #region 同步姓名和手机

                if (websiteInfo.IsSynchronizationData == 1)
                {
                    if (string.IsNullOrWhiteSpace(currentUserInfo.TrueName))
                    {
                        if (websiteInfo.IsNeedMallOrderCreaterNamePhone == 1)
                        {
                            currentUserInfo.TrueName = orderInfo.CustomCreaterName;
                        }
                        else
                        {
                            currentUserInfo.TrueName = orderInfo.Consignee;
                        }
                        Update(currentUserInfo, string.Format(" TrueName = '{0}' ", currentUserInfo.TrueName), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                    }


                    if (string.IsNullOrWhiteSpace(currentUserInfo.Phone))
                    {
                        if (websiteInfo.IsNeedMallOrderCreaterNamePhone == 1)
                        {
                            currentUserInfo.Phone = orderInfo.CustomCreaterPhone;
                        }
                        else
                        {
                            currentUserInfo.Phone = orderInfo.Phone;
                        }

                        Update(currentUserInfo, string.Format(" Phone = '{0}' ", currentUserInfo.Phone), string.Format(" AutoID = {0} ", currentUserInfo.AutoID));
                    }

                }

                #endregion



                if (WebsiteOwner != "mixblu")
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
                orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();

                if (IsGroupByMember(orderRequestModel))
                {
                    orderInfo.GroupBuyParentOrderId = parentOrderInfo.OrderID;
                }


                orderInfo.UseScore = orderRequestModel.use_score;
                orderInfo.Status = "待付款";
                orderInfo.ArticleCategoryType = "Mall";
                orderInfo.UseAmount = orderRequestModel.use_amount;
                orderInfo.LastUpdateTime = DateTime.Now;

                if (!string.IsNullOrEmpty(orderRequestModel.sale_id))
                {
                    long saleId = 0;
                    if (long.TryParse(orderRequestModel.sale_id, out saleId))
                    {
                        orderInfo.SellerId = saleId;

                    }
                    else
                    {

                    }
                }



                #region 分销关系建立
                if (bllMenuPermission.CheckUserAndPmsKey(WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
                {

                    if (string.IsNullOrWhiteSpace(currentUserInfo.DistributionOwner) && (!string.IsNullOrEmpty(orderRequestModel.sale_id)))
                    {

                        if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                        {
                            var preUserInfo = bllUser.GetUserInfo(orderRequestModel.sale_id);//推荐人
                            if (preUserInfo != null)
                            {
                                if (bllUser.IsDistributionMember(preUserInfo) || (preUserInfo.UserID == websiteInfo.WebsiteOwner))//上级符合分销员标准
                                {

                                    var setUserDistributionOwnerResult =
                                        bllDis.SetUserDistributionOwner(currentUserInfo.UserID, preUserInfo.UserID, currentUserInfo.WebsiteOwner);

                                    if (setUserDistributionOwnerResult)
                                    {
                                        currentUserInfo.DistributionOwner = preUserInfo.UserID;
                                        currentUserInfo.Channel = preUserInfo.Channel;
                                    }

                                }

                            }
                        }

                    }

                    //bllDis.SendMessageToUser(orderInfo);
                    orderInfo.DistributionOwner = currentUserInfo.DistributionOwner;
                    orderInfo.ChannelUserId = bllDis.GetUserChannel(currentUserInfo);

                }
                #endregion

                if (orderRequestModel.pay_type == "WEIXIN")//微信支付
                {
                    orderInfo.PaymentType = 2;
                }
                else if (orderRequestModel.pay_type == "ALIPAY")//支付宝支付
                {
                    orderInfo.PaymentType = 1;
                }


                //判断商品是否无需物流
                int isNoExpress = 1;

                if (!string.IsNullOrEmpty(orderRequestModel.is_no_express))
                {
                    isNoExpress = int.Parse(orderRequestModel.is_no_express);

                }
                int isNeedNamePhone = 0;

                #region 格式检查
                if (orderRequestModel.skus == null)
                {
                    resp.Code = 1;
                    resp.Msg = "参数skus 不能为空";
                    return resp;
                }
                if (currOrderType == Enums.OrderType.Gift)
                {
                    if (orderRequestModel.skus.Count > 1)
                    {
                        resp.Code = 1;
                        resp.Msg = "只能购买一个商品";
                        return resp;
                    }
                }

                if (IsGroupByLeader(orderRequestModel))
                {

                    if (orderRequestModel.skus.Count > 1)
                    {
                        resp.Code = 1;
                        resp.Msg = "只能购买一个商品";
                        return resp;
                    }

                    if (string.IsNullOrEmpty(orderRequestModel.rule_id))
                    {
                        resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
                        resp.Msg = "rule_id 必传";
                        return resp;
                    }

                    if (orderRequestModel.skus.Sum(p => p.count) > 1)
                    {
                        resp.Code = (int)Enums.APIErrCode.PrimaryKeyIncomplete;
                        resp.Msg = "团购商品只能购买一件";
                        return resp;
                    }

                }

                #endregion




                orderRequestModel.skus = orderRequestModel.skus.DistinctBy(p => p.sku_id).ToList();

                List<ProductGroupBuyRule> groupBuyList = new List<ProductGroupBuyRule>();//此商品的团购规则列表

                #region 购买的商品

                List<WXMallProductInfo> productList = new List<WXMallProductInfo>();//此次订单包含的商品
                List<ProductSku> skuList = new List<ProductSku>();//此次订单包含的sku
                foreach (var sku in orderRequestModel.skus)
                {
                    ProductSku productSku = bllMall.GetProductSku(sku.sku_id, true, orderRequestModel.supplier_id);//保证库存准确性，下单库存读取数据库
                    if (productSku == null) continue;
                    skuList.Add(productSku);
                    if (productList.Count(p => p.PID == productSku.ProductId.ToString()) > 0) continue;

                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());

                    if (productInfo.IsNoExpress == 0 && (string.IsNullOrEmpty(orderRequestModel.is_no_express)))
                    {
                        isNoExpress = 0;
                    }

                    if (productInfo.IsNeedNamePhone == 1)
                    {
                        isNeedNamePhone = 1;
                    }
                    if (!string.IsNullOrEmpty(productInfo.CategoryId))
                    {
                        var category = bllMall.Get<WXMallCategory>(string.Format(" AutoID={0} ", productInfo.CategoryId));
                        if (category != null)
                        {
                            productInfo.CategoryName = category.CategoryName;
                        }
                    }
                    productList.Add(productInfo);

                    groupBuyList = bllMall.GetProductGroupBuyRuleList(productSku.ProductId.ToString());
                }

                orderInfo.IsNoExpress = isNoExpress;
                orderInfo.IsNeedNamePhone = isNeedNamePhone;
                #endregion

                ProductGroupBuyRule groupBuyRule = new ProductGroupBuyRule();
                if (IsGroupByLeader(orderRequestModel))
                {
                    //团购、团长下单
                    if (groupBuyList.Count == 0)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "此商品不能团购";
                        return resp;
                    }
                    if (groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id)) == null)
                    {
                        resp.Code = (int)Enums.APIErrCode.OperateFail;
                        resp.Msg = "rule_id 错误,请检查";
                        return resp;
                    }
                    groupBuyRule = groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id));//团购规则
                }


                #region 收货人信息格式检查



                //相关检查

                if (isNoExpress == 0 && orderInfo.DeliveryType == 2)
                {

                    if (string.IsNullOrEmpty(orderInfo.Consignee))
                    {
                        resp.Code = 1;
                        resp.Msg = "姓名不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.Phone))
                    {
                        resp.Code = 1;
                        resp.Msg = "手机不能为空";
                        return resp;
                    }
                    //if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(orderInfo.Phone))
                    //{
                    //    resp.Code = 1;
                    //    resp.Msg = "手机格式不正确";
                    //    return resp;
                    //}

                    if (string.IsNullOrEmpty(orderInfo.ReceiverProvince))
                    {
                        resp.Code = 1;
                        resp.Msg = "省份名称不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.ReceiverProvinceCode))
                    {
                        resp.Code = 1;
                        resp.Msg = "省份代码不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.ReceiverCity))
                    {
                        resp.Code = 1;
                        resp.Msg = "城市名称不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
                    {
                        resp.Code = 1;
                        resp.Msg = "城市代码不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.ReceiverDist))
                    {
                        resp.Code = 1;
                        resp.Msg = "城市区域名称不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
                    {
                        resp.Code = 1;
                        resp.Msg = "城市区域代码不能为空";
                        return resp;
                    }
                    if (string.IsNullOrEmpty(orderInfo.Address))
                    {
                        resp.Code = 1;
                        resp.Msg = "收货地址不能为空";
                        return resp;
                    }

                }
                else
                {
                    if (isNeedNamePhone == 1)
                    {
                        if (string.IsNullOrEmpty(orderInfo.Consignee))
                        {
                            resp.Code = 1;
                            resp.Msg = "姓名不能为空";
                            return resp;
                        }
                        if (string.IsNullOrEmpty(orderInfo.Phone))
                        {
                            resp.Code = 1;
                            resp.Msg = "手机不能为空";
                            return resp;
                        }
                        if (!ZentCloud.Common.MyRegex.PhoneNumLogicJudge(orderInfo.Phone))
                        {
                            resp.Code = 1;
                            resp.Msg = "手机格式不正确";
                            return resp;
                        }
                    }
                }

                //相关检查 
                #endregion


                #region 门店自提 或快递配送范围检查
                if (orderInfo.DeliveryType == 0 || orderInfo.DeliveryType == 2)
                {
                    if (companyConfig.StoreExpressRange > 0 || companyConfig.ExpressRange > 0)
                    {
                        //if (string.IsNullOrEmpty(orderRequestModel.supplier_id))
                        //{
                        //   resp.Code = (int)Enums.APIErrCode.OperateFail;
                        //   resp.Msg = "请选择门店";
                        //  return resp;
                        //}
                        var storeInfo = bllMall.Get<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' And ArticleType='Outlets' And K5='{1}'", bllMall.WebsiteOwner, orderRequestModel.supplier_id));
                        if (storeInfo==null)
                        {
                            storeInfo = new JuActivityInfo();
                        }

                        var geoResult = ZentCloud.Common.AMapHelper.GetGeoByAddress(orderInfo.ReceiverProvince + orderInfo.ReceiverCity + orderInfo.ReceiverDist);
                        double distance = 0;
                        if (geoResult.status)
                        {
                            distance = ZentCloud.Common.GeolocationHelper.ComputeDistance(geoResult.longitude, geoResult.latitude, Double.Parse(storeInfo.UserLongitude), Double.Parse(storeInfo.UserLatitude));

                            if (orderInfo.DeliveryType == 2)//送货上门
                            {
                                if (distance > companyConfig.StoreExpressRange)
                                {
                                    resp.Code = 1;
                                    resp.Msg = string.Format("超过门店配送范围,请选择其它配送方式");
                                    return resp;
                                }

                            }
                            if (orderInfo.DeliveryType == 0)//送货上门
                            {
                                if (distance < companyConfig.ExpressRange)
                                {
                                    resp.Code = 1;
                                    resp.Msg = string.Format("小于最小快递配送范围,请选择其它配送方式");
                                    return resp;
                                }

                            }

                        }



                    }
                }

                #endregion

                #region 商品检查 订单详情生成
                ///订单详情

                string cardCouponType = "";//优惠券类型
                MyCardCoupons mycardCoupon = new MyCardCoupons();
                #region 检查优惠券储值卡是否可用
                if (orderRequestModel.cardcoupon_id > 0)
                {

                    mycardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, currentUserInfo.UserID);
                    if (mycardCoupon != null)//优惠券
                    {
                        #region 优惠券
                        var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
                        if (cardCoupon == null)
                        {
                            resp.Code = 1;
                            resp.Msg = "无效的优惠券";
                            return resp;
                        }
                        cardCouponType = cardCoupon.CardCouponType;
                        #region 需要购买指定商品
                        if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
                        {

                            if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
                            {
                                var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
                                resp.Code = 1;
                                resp.Msg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
                                return resp;
                            }


                        }
                        #endregion

                        //#region 需要购买指定标签商品
                        //if (!string.IsNullOrEmpty(cardCoupon.Ex8))
                        //{
                        //    if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
                        //    {
                        //        resp.Code = 1;
                        //        resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        //        return resp;

                        //    }

                        //    bool checkResult = true;
                        //    foreach (var product in productList)
                        //    {
                        //        if (!string.IsNullOrEmpty(product.Tags))
                        //        {
                        //            bool tempResult = false;
                        //            foreach (string tag in product.Tags.Split(','))
                        //            {
                        //                if (cardCoupon.Ex8.Contains(tag))
                        //                {
                        //                    tempResult = true;
                        //                    break;
                        //                }
                        //            }
                        //            if (!tempResult)
                        //            {
                        //                checkResult = false;
                        //                break;
                        //            }


                        //        }
                        //        else//商品不包含标签
                        //        {

                        //            checkResult = false;
                        //            break;
                        //        }

                        //    }
                        //    if (!checkResult)
                        //    {
                        //        resp.Code = 1;
                        //        resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        //        return resp;
                        //    }

                        //}
                        //#endregion
                        #region 需要购买指定标签商品 只要购买一种符合标签的商品即可
                        if (!string.IsNullOrEmpty(cardCoupon.Ex8))
                        {
                            if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                                return resp;

                            }

                            bool checkResult = false;
                            foreach (var product in productList)
                            {
                                if (!string.IsNullOrEmpty(product.Tags))
                                {

                                    foreach (string tag in product.Tags.Split(','))
                                    {
                                        if (cardCoupon.Ex8.Contains(tag))
                                        {
                                            checkResult = true;
                                            break;
                                        }
                                    }


                                }
                                //else
                                //{

                                //    checkResult = false;
                                //    break;
                                //}

                            }
                            if (!checkResult)
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                                return resp;
                            }

                        }
                        #endregion


                        if (cardCoupon.LimitCount > 0)//限制购买数量
                        {
                            if (orderRequestModel.skus.Sum(p => p.count) < cardCoupon.LimitCount)
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("使用此优惠券需要购买{0}件以上商品", cardCoupon.LimitCount);
                                return resp;
                            }
                        }
                        if (cardCoupon.LimitAmount > 0)//限制购买金额
                        {
                            if (skuList.Sum(p => p.Price) < cardCoupon.LimitAmount)
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("使用此优惠券需要购买{0}元以上商品", cardCoupon.LimitAmount);
                                return resp;
                            }

                        }
                        if (!string.IsNullOrEmpty(cardCoupon.Categorys))//需要购买指定分类
                        {
                            bool cateCheckResult = false;
                            foreach (var item in productList)
                            {
                                foreach (var category in cardCoupon.Categorys.Split(','))
                                {
                                    if (!string.IsNullOrEmpty(item.CategoryName) && (item.CategoryName == category))
                                    {
                                        cateCheckResult = true;
                                        break;
                                    }

                                }
                            }
                            if (!cateCheckResult)
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("使用此优惠券需要购买分类{0}的商品", cardCoupon.LimitAmount);
                                return resp;
                            }

                        }
                        if (cardCoupon.IsPrePrice == 1)//正价
                        {

                        }
                        if (!string.IsNullOrEmpty(cardCoupon.StoreIds))
                        {
                            if (orderRequestModel.supplier_id != cardCoupon.StoreIds)
                            {
                                resp.Code = 1;
                                resp.Msg = string.Format("指定门店才可使用");
                                return resp;

                            }


                        }

                        #endregion
                    }
                    else//储值卡
                    {
                        //#region 储值卡
                        ////检查是否使用了积分余额
                        //if (orderRequestModel.use_amount > 0 || orderRequestModel.use_score > 0)
                        //{
                        //    resp.Code = (int)Enums.APIErrCode.OperateFail;
                        //    resp.Msg = "使用储值卡时,不可再用积分或余额抵扣,请取消使用积分或余额抵扣";
                        //    return resp;

                        //}
                        //#endregion
                    }



                }
                #endregion

                var needUseScore = 0;//必须使用的积分
                decimal needUseCash = 0;//必须使用的现金

                List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情

                List<BLLJIMP.Model.API.Mall.SkuModel> skuLisHongwei = new List<SkuModel>();
                foreach (var sku in orderRequestModel.skus)
                {
                    BLLJIMP.Model.API.Mall.SkuModel model = new SkuModel();
                    model.sku_id = sku.sku_id;
                    model.count = sku.count;
                    model.price = skuList.Single(p => p.SkuId == sku.sku_id).Price;
                    model.sku_sn = skuList.Single(p => p.SkuId == sku.sku_id).SkuSN;
                    skuLisHongwei.Add(model);

                }
                //var memberRight = hongWareClient.MemberRight(skuLisHongwei);
                // Open.HongWareSDK.Client cl = new Open.HongWareSDK.Client("hf");
                foreach (var sku in orderRequestModel.skus)
                {
                    //先检查库存
                    //ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                    //if (productSku == null)
                    //{
                    //    resp.Code = 1;
                    //    resp.Msg = "SKU不存在";
                    //    return resp;
                    //}

                    ProductSku productSku = skuList.Single(p => p.SkuId == sku.sku_id);
                    //WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    WXMallProductInfo productInfo = productList.Single(p => p.PID == productSku.ProductId.ToString());
                    if (productInfo.IsOnSale == "0")
                    {
                        resp.Code = 1;
                        resp.Msg = string.Format("{0}已下架", productInfo.PName);
                        return resp;
                    }
                    if (productInfo.IsDelete == 1)
                    {
                        resp.Code = 1;
                        resp.Msg = string.Format("{0}已下架", productInfo.PName);
                        return resp;
                    }
                    if (bllMall.GetSkuCount(productSku) < sku.count)
                    {
                        resp.Code = 1;
                        resp.Msg = string.Format("{0}{1}库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.Stock);
                        resp.Result = skuList;
                        return resp;
                    }
                    int promotionActivityId = 0;
                    #region 判断是否特卖商品
                    if (bllMall.IsPromotionTime(productSku))
                    {
                        //

                        promotionActivityId = productList.First(p => p.PID == productSku.ProductId.ToString()).PromotionActivityId;

                        int historyCount = bllMall.GetList<WXMallOrderDetailsInfo>(String.Format("PromotionActivityId={0} And PID={1} And OrderId In (Select OrderId From ZCJ_WXMallOrderInfo where OrderUserId='{2}')", promotionActivityId, productSku.ProductId, currentUserInfo.UserID)).Sum(p => p.TotalCount);

                        BLLJIMP.Model.PromotionActivity activity = bllMall.GetPromotionActivity(promotionActivityId);

                        int buyCount = 0;
                        List<ProductSku> currProductSkuList = bllMall.GetProductSkuList(productSku.ProductId);
                        foreach (var item in currProductSkuList)
                        {
                            if (orderRequestModel.skus.SingleOrDefault(p => p.sku_id == item.SkuId) != null)
                            {
                                buyCount += sku.count;
                            }
                        }


                        if (((historyCount + buyCount) > activity.LimitBuyProductCount) && activity.LimitBuyProductCount > 0)
                        {
                            string msgF = string.Format("{0}最多还可购买{1}件", productInfo.PName, activity.LimitBuyProductCount - historyCount);
                            if (activity.LimitBuyProductCount - historyCount == 0)
                            {
                                msgF = string.Format("{0}已经达到最多购买数量", productInfo.PName);
                            }
                            resp.Code = 1;
                            resp.Msg = msgF;
                            return resp;

                        }


                        //

                        if (sku.count > productSku.PromotionStock)
                        {
                            resp.Code = 1;
                            resp.Msg = string.Format("{0}{1}特卖库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.PromotionStock);
                            return resp;

                        }


                    }
                    #endregion



                    if (bllMall.IsLimitProductTime(productInfo, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        resp.Code = 1;
                        resp.Msg = string.Format("{0} 暂时不能购买", productInfo.PName);
                        return resp;
                    }


                    #region 预购商品判断

                    if (productInfo.IsAppointment == 1)
                    {
                        var appointmentInfo = bllMall.GetProductAppointmentInfo(productInfo);
                        if (appointmentInfo.status == 0)
                        {
                            resp.Code = 1;
                            resp.Msg = string.Format("{0} 暂未到预购时间", productInfo.PName);
                            return resp;
                        }
                        if (appointmentInfo.status == 2)
                        {
                            resp.Code = 1;
                            resp.Msg = string.Format("{0} 已经超过预购时间", productInfo.PName);
                            return resp;
                        }
                        orderInfo.IsAppointment = 1;
                    }
                    #endregion


                    if (productInfo.Score > 0)//必须使用的积分
                    {
                        needUseScore += sku.count * productInfo.Score;
                    }


                    WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                    detailModel.OrderID = orderInfo.OrderID;
                    detailModel.PID = productInfo.PID;
                    detailModel.TotalCount = sku.count;
                    detailModel.OrderScore = productInfo.Score;
                    detailModel.ProductName = productInfo.PName;
                    detailModel.SkuId = productSku.SkuId;
                    detailModel.SkuShowProp = bllMall.GetProductShowProperties(productSku.SkuId);
                    detailModel.ArticleCategoryType = "Mall";
                    detailModel.PromotionActivityId = promotionActivityId.ToString();
                    detailModel.ExQuestionnaireID = productSku.ExQuestionnaireID;
                    detailModel.ProductName = productInfo.PName;
                    detailModel.ProductImage = productInfo.RecommendImg;

                    if (string.IsNullOrEmpty(orderRequestModel.supplier_id))
                    {
                        orderInfo.SupplierUserId = productInfo.SupplierUserId;
                        var supplierInfo = bllUser.GetUserInfo(orderInfo.SupplierUserId, orderInfo.WebsiteOwner);
                        if (supplierInfo != null)
                        {
                            orderInfo.SupplierName = supplierInfo.Company;
                        }


                    }


                    var productPrice = productInfo.Price;
                    var skuPrice = productSku.Price;

                    if (IsGroupByLeader(orderRequestModel))
                    {
                        //团长折扣
                        productPrice = Math.Round((decimal)(productInfo.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2);
                        skuPrice = Math.Round((decimal)(productSku.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2);
                    }
                    else if (IsGroupByMember(orderRequestModel))
                    {
                        //团员折扣
                        productPrice = Math.Round((decimal)(productInfo.Price * (decimal)(parentOrderInfo.MemberDiscount / 10)), 2);
                        skuPrice = Math.Round((decimal)(productSku.Price * (decimal)(parentOrderInfo.MemberDiscount / 10)), 2);
                    }
                    else
                    {
                        //通用商城下单（包括限时特卖）
                        skuPrice = bllMall.GetSkuPrice(productSku);

                        //if (memberRight.isSuccess)
                        //{

                        //    foreach (var orderItem in memberRight.orders[0].orderItems)
                        //    {
                        //        if (orderItem.sku==productSku.SkuSN)
                        //        {
                        //            switch (orderRequestModel.discount_type)
                        //            {
                        //                case "0"://商品折扣
                        //                    skuPrice = Convert.ToDecimal(orderItem.pro_RealPrice);
                        //                    break;
                        //                case "1"://会员折扣
                        //                    skuPrice = Convert.ToDecimal(orderItem.mem_realPrice);
                        //                    break;
                        //                case "2"://生日折扣
                        //                    skuPrice = Convert.ToDecimal(orderItem.birth_realPrice);
                        //                    break;
                        //                default:
                        //                    break;
                        //            }
                        //        }

                        //    }
                        //}
                    }
                    if (productInfo.IsCashPayOnly == 1)//必须使用的现金
                    {
                        needUseCash += (productInfo.Score > 0 ? productPrice : skuPrice) * sku.count;
                    }
                    detailModel.OrderPrice = productInfo.Score > 0 ? productPrice : skuPrice;

                    detailModel.BasePrice = productSku.BasePrice > 0 ? productSku.BasePrice : productInfo.BasePrice;

                    detailModel.IsComplete = 0;

                    detailList.Add(detailModel);

                }
                #endregion

                #region 纯积分购买
                if (needUseScore > 0)
                {
                    if ((currentUserInfo.TotalScore < needUseScore) || (orderRequestModel.use_score < needUseScore))
                    {

                        resp.Code = 1;
                        resp.Msg = string.Format("您需要使用{0}积分来兑换， 可用积分不足", needUseScore);
                        return resp;

                    }

                }
                #endregion

                productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用
                //物流费用
                #region 运费计算
                FreightModel freightModel = new FreightModel();
                freightModel.receiver_province_code = orderRequestModel.receiver_province_code;
                freightModel.receiver_city_code = orderRequestModel.receiver_city_code;
                freightModel.receiver_dist_code = orderRequestModel.receiver_dist_code;
                freightModel.skus = orderRequestModel.skus;
                decimal freight = 0;//运费
                string freightMsg = "";

                if (isNoExpress == 0)//判断是否无需物流
                {
                    if (!bllMall.CalcFreight(freightModel, out freight, out freightMsg))
                    {
                        resp.Code = 1;
                        resp.Msg = freightMsg;
                        return resp;

                    }
                }
                orderInfo.Transport_Fee = freight;

                #endregion

                #region 优惠券或储值卡优惠计算
                StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
                decimal discountAmount = 0;//优惠金额
                bool canUseCardCoupon = false;
                string msg = "";
                if (orderRequestModel.cardcoupon_id > 0)//有优惠券
                {


                    if (!string.IsNullOrEmpty(orderRequestModel.coupon_type))
                    {
                        orderInfo.CouponType = int.Parse(orderRequestModel.coupon_type);
                        #region 优惠券
                        if (orderInfo.CouponType == 0)
                        {

                            if (mycardCoupon != null)
                            {
                                discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, currentUserInfo.UserID, out canUseCardCoupon, out msg, currOrderType, orderRequestModel.groupbuy_type, productFee);
                                if (!canUseCardCoupon)
                                {
                                    resp.Code = 1;
                                    resp.Msg = msg;
                                    return resp;
                                }
                                //if (discountAmount > productFee + orderInfo.Transport_Fee)
                                //{
                                //    resp.Code = 1;
                                //    resp.Msg = "优惠券可优惠金额超过了订单总金额";
                                //    return resp;

                                //}


                                if (cardCouponType == "MallCardCoupon_FreeFreight")//免邮券
                                {
                                    orderInfo.Transport_Fee = 0;
                                }
                            }

                        }
                        #endregion

                        #region 储值卡
                        else if (orderInfo.CouponType == 1)
                        {
                            storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", orderRequestModel.cardcoupon_id));
                            discountAmount = bllMall.CalcDiscountAmountStoreValue(productFee + freight, storeValueCardRecord.UserId, orderRequestModel.cardcoupon_id.ToString());


                        }
                        #endregion
                    }
                    else
                    {
                        #region 优惠券
                        if (mycardCoupon != null)
                        {
                            discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, currentUserInfo.UserID, out canUseCardCoupon, out msg, currOrderType, orderRequestModel.groupbuy_type, productFee);
                            if (!canUseCardCoupon)
                            {
                                resp.Code = 1;
                                resp.Msg = msg;
                                return resp;
                            }
                            //if (discountAmount > productFee + orderInfo.Transport_Fee)
                            //{
                            //    resp.Code = 1;
                            //    resp.Msg = "优惠券可优惠金额超过了订单总金额";
                            //    return resp;

                            //}


                            if (cardCouponType == "MallCardCoupon_FreeFreight")//免邮券
                            {
                                orderInfo.Transport_Fee = 0;
                            }
                        }
                        #endregion

                        #region 储值卡
                        else
                        {
                            storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", orderRequestModel.cardcoupon_id));
                            discountAmount = bllMall.CalcDiscountAmountStoreValue(productFee + freight, storeValueCardRecord.UserId, orderRequestModel.cardcoupon_id.ToString());
                            orderInfo.CouponType = 1;
                        }
                        #endregion


                    }



                }
                //优惠券计算 


                #endregion

                #region 积分计算
                decimal scoreExchangeAmount = 0;///积分抵扣的金额
                //积分计算
                if (orderRequestModel.use_score > 0)
                {

                    #region 使用驿氪积分
                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                    {

                        if ((!string.IsNullOrEmpty(currentUserInfo.Phone)))
                        {
                            Open.EZRproSDK.Entity.BonusGetResp yikeUser = yiKeClient.GetBonus(currentUserInfo.Ex1, currentUserInfo.Ex2, currentUserInfo.Phone);
                            if (yikeUser != null)
                            {

                                currentUserInfo.TotalScore = yikeUser.Bonus;

                            }
                            else//查不到yike用户
                            {
                                resp.Code = 1;
                                resp.Msg = "您的账号尚未注册,请先注册";
                                return resp;
                            }

                        }

                    }
                    #endregion

                    #region 使用宏巍积分
                    if (websiteInfo.IsUnionHongware == 1)
                    {

                        currentUserInfo.TotalScore = hongWeiWareMemberInfo.member.point;

                    }
                    #endregion



                    if (currentUserInfo.TotalScore < orderRequestModel.use_score)
                    {
                        resp.Code = 1;
                        resp.Msg = "积分不足";
                        return resp;
                    }


                    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                    if (scoreConfig != null && scoreConfig.ExchangeAmount > 0)
                    {
                        var tmpUseScore = orderRequestModel.use_score;

                        if (detailList != null)
                        {
                            tmpUseScore -= detailList.Sum(p => p.TotalCount * p.OrderScore);
                        }

                        scoreExchangeAmount = Math.Round(tmpUseScore / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);

                        var totalProductAmount = detailList.Sum(p => p.TotalCount * p.OrderPrice);

                        if (scoreExchangeAmount > totalProductAmount)
                        {
                            scoreExchangeAmount = totalProductAmount.Value;
                        }


                    }

                }



                //积分计算 
                #endregion

                #region 使用账户余额
                if (orderRequestModel.use_amount > 0)
                {
                    if (!bllMall.IsEnableAccountAmountPay())
                    {
                        resp.Code = 1;
                        resp.Msg = "尚未启用余额支付功能";
                        return resp;
                    }
                    #region 使用宏巍余额
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        currentUserInfo.AccountAmount = (decimal)hongWeiWareMemberInfo.member.balance;

                    }
                    #endregion


                    if (currentUserInfo.AccountAmount < orderRequestModel.use_amount)
                    {

                        resp.Code = 1;
                        resp.Msg = "您的账户余额不足";
                        return resp;
                    }
                }
                #endregion

                //实付金额=(商品总金额-优惠券优惠金额-积分抵扣-余额抵扣)+运费
                //运费只有使用了免邮券才能抵
                orderInfo.Product_Fee = productFee;
                //orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
                orderInfo.TotalAmount = orderInfo.Product_Fee;
                if (cardCouponType != "MallCardCoupon_FreeFreight")//免邮券不算
                {
                    orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额

                }
                orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
                orderInfo.TotalAmount -= orderRequestModel.use_amount;//使用余额
                //if (orderInfo.TotalAmount < 0)
                //{
                //    orderInfo.TotalAmount = 0;
                //}
                orderInfo.TotalAmount += orderInfo.Transport_Fee;
                orderInfo.PayableAmount = orderInfo.Product_Fee + freight;//应付金额
                if (orderInfo.TotalAmount < 0)
                {
                    orderInfo.TotalAmount = 0;
                }
                if (orderInfo.PayableAmount < 0)
                {
                    orderInfo.PayableAmount = 0;
                }
                if (orderInfo.DeliveryType == 1)
                {
                    orderInfo.Transport_Fee = 0;
                    orderInfo.TotalAmount -= companyConfig.StoreSinceDiscount;
                }
                orderInfo.ScoreExchangAmount = scoreExchangeAmount;//积分抵扣金额
                orderInfo.CardcouponDisAmount = discountAmount;//卡券抵扣金额
                if (IsGroupByLeader(orderRequestModel))
                {
                    orderInfo.HeadDiscount = groupBuyRule.HeadDiscount;
                    orderInfo.MemberDiscount = groupBuyRule.MemberDiscount;
                    orderInfo.PeopleCount = groupBuyRule.PeopleCount;
                    orderInfo.ExpireDay = groupBuyRule.ExpireDay;

                }

                if (IsGroupByMember(orderRequestModel))
                {
                    orderInfo.HeadDiscount = parentOrderInfo.HeadDiscount;
                    orderInfo.MemberDiscount = parentOrderInfo.MemberDiscount;
                    orderInfo.PeopleCount = parentOrderInfo.PeopleCount;
                    orderInfo.ExpireDay = parentOrderInfo.ExpireDay;
                    orderInfo.GroupBuyParentOrderId = parentOrderInfo.OrderID;
                }

                //if ((productFee + orderInfo.Transport_Fee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
                //{
                //    resp.Code = 1;
                //    resp.Msg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
                //    return resp;

                //}


                #region 均摊价计算
                detailList = CalcPaymentFt(orderInfo.TotalAmount - orderInfo.Transport_Fee, detailList);
                #endregion

                #region 最大退款金额计算
                detailList = CalcMaxRefundAmount(orderInfo.TotalAmount - orderInfo.Transport_Fee, detailList);
                #endregion


                if (orderInfo.TotalAmount == 0)
                {
                    orderInfo.PaymentStatus = 1;
                    orderInfo.PayTime = DateTime.Now;
                    orderInfo.Status = "待发货";
                    if (orderInfo.OrderType == 2)
                    {
                        orderInfo.GroupBuyStatus = "0";
                        orderInfo.DistributionStatus = 1;

                    }
                    if (orderInfo.DeliveryType == 1)
                    {
                        orderInfo.Status = "待自提";
                    }
                }
                if (orderInfo.TotalAmount + orderRequestModel.use_amount < needUseCash)
                {
                    resp.Code = 1;
                    resp.Msg = string.Format("最少需要支付{0}元", needUseCash);
                    return resp;
                }

                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {

                    if (!Add(orderInfo, tran))
                    {
                        tran.Rollback();
                        resp.Code = 1;
                        resp.Msg = "提交失败";
                        return resp;
                    }

                    #region 更新优惠券储值卡使用状态

                    #region 优惠券
                    if (orderRequestModel.cardcoupon_id > 0 && (canUseCardCoupon == true) && mycardCoupon != null)//有优惠券且已经成功使用
                    {

                        //mycardCoupon= bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, currentUserInfo.UserID);
                        mycardCoupon.UseDate = DateTime.Now;
                        mycardCoupon.Status = 1;
                        if (!bllCardCoupon.Update(mycardCoupon, tran))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "更新优惠券状态失败";
                            return resp;
                        }
                        #region 同时核销掉微信卡包
                        if (!string.IsNullOrEmpty(mycardCoupon.WeixinHexiaoCode))
                        {
                            bllWeixinCard.Consume(mycardCoupon.WeixinHexiaoCode);
                        }
                        #endregion

                        //if (WebsiteOwner.Contains("hailan"))
                        //{
                        //    if (!hongWareClient.YimaCardHexiao(mycardCoupon.YimaCardCode, out msg))
                        //    {
                        //        tran.Rollback();
                        //        resp.Code = 1;
                        //        resp.Msg = "核销翼码卡券失败";
                        //        return resp;
                        //    }

                        //}

                    }
                    #endregion

                    #region 储值卡
                    if (orderRequestModel.cardcoupon_id > 0 && mycardCoupon == null)
                    {
                        StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And (UserId='{1}' Or ToUserId='{1}')", orderRequestModel.cardcoupon_id, currentUserInfo.UserID));
                        myStoredValueCardRecord.UseDate = DateTime.Now;
                        myStoredValueCardRecord.Status = 9;
                        if (!bllStoreValue.Update(myStoredValueCardRecord, tran))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "更新储值卡状态失败";
                            return resp;
                        }
                        StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
                        storeValueCardUseRecord.UseAmount = discountAmount;
                        storeValueCardUseRecord.CardId = myStoredValueCardRecord.CardId;
                        storeValueCardUseRecord.Remark = string.Format("商城下单,订单号:{0}使用{1}元", orderInfo.OrderID, Math.Round(discountAmount, 2));
                        storeValueCardUseRecord.UseDate = DateTime.Now;
                        storeValueCardUseRecord.UserId = storeValueCardRecord.UserId;
                        storeValueCardUseRecord.UseUserId = currentUserInfo.UserID;
                        storeValueCardUseRecord.WebsiteOwner = orderInfo.WebsiteOwner;
                        storeValueCardUseRecord.MyCardId = orderRequestModel.cardcoupon_id;
                        storeValueCardUseRecord.OrderId = orderInfo.OrderID;
                        if (!bllStoreValue.Add(storeValueCardUseRecord, tran))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "更新储值卡状态失败";
                            return resp;
                        }



                    }
                    #endregion

                    #endregion

                    #region 积分抵扣
                    //积分扣除
                    if (orderRequestModel.use_score > 0)
                    {
                        currentUserInfo.TotalScore -= orderRequestModel.use_score;
                        if (Update(currentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "更新用户积分失败";
                            return resp;
                        }

                        //积分记录
                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        scoreRecord.AddTime = DateTime.Now;
                        scoreRecord.Score = -orderRequestModel.use_score;
                        scoreRecord.TotalScore = currentUserInfo.TotalScore;
                        scoreRecord.ScoreType = "OrderSubmit";
                        scoreRecord.UserID = currentUserInfo.UserID;
                        scoreRecord.AddNote = "微商城-下单使用积分";
                        scoreRecord.RelationID = orderInfo.OrderID;
                        scoreRecord.WebSiteOwner = WebsiteOwner;
                        if (!bllMall.Add(scoreRecord))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "插入积分记录失败";
                            return resp;
                        }

                        #region 驿氪积分同步

                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {


                            yiKeClient.BonusUpdate(currentUserInfo.Ex2, -(orderRequestModel.use_score), "商城下单使用积分" + orderRequestModel.use_score);


                        }

                        #endregion

                        #region 更新宏巍积分
                        if (websiteInfo.IsUnionHongware == 1)
                        {
                            if (!hongWareClient.UpdateMemberScore(hongWeiWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, -orderRequestModel.use_score))
                            {
                                tran.Rollback();
                                resp.Code = 1;
                                resp.Msg = "更新宏巍积分失败";
                                return resp;
                            }

                        }
                        #endregion



                    }

                    //积分扣除 
                    #endregion

                    #region 余额抵扣

                    if (orderRequestModel.use_amount > 0 && bllMall.IsEnableAccountAmountPay())
                    {
                        currentUserInfo.AccountAmount -= orderRequestModel.use_amount;
                        if (Update(currentUserInfo, string.Format(" AccountAmount={0}", currentUserInfo.AccountAmount), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "更新用户余额失败";
                            return resp;
                        }

                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        scoreRecord.AddTime = DateTime.Now;
                        scoreRecord.Score = -(double)orderRequestModel.use_amount;
                        scoreRecord.TotalScore = (double)currentUserInfo.AccountAmount;
                        scoreRecord.UserID = currentUserInfo.UserID;
                        scoreRecord.AddNote = "微商城-下单使用余额";
                        scoreRecord.RelationID = orderInfo.OrderID;
                        scoreRecord.WebSiteOwner = WebsiteOwner;
                        scoreRecord.ScoreType = "AccountAmount";
                        if (!bllMall.Add(scoreRecord))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "插入余额记录失败";
                            return resp;
                        }

                        #region 更新宏巍余额
                        if (websiteInfo.IsUnionHongware == 1)
                        {
                            if (!hongWareClient.UpdateMemberBlance(hongWeiWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, -(float)orderRequestModel.use_amount))
                            {
                                tran.Rollback();
                                resp.Code = 1;
                                resp.Msg = "更新宏巍余额失败";
                                return resp;
                            }

                        }
                        #endregion





                    }


                    #endregion

                    #region 插入订单详情表及更新库存
                    foreach (var item in detailList)
                    {
                        //ProductSku productSku = bllMall.GetProductSku((int)(item.SkuId));
                        ProductSku productSku = skuList.Single(p => p.SkuId == ((int)(item.SkuId)));
                        // WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                        if (!Add(item, tran))
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "提交失败";
                            resp.Result = "detail fail";
                            return resp;
                        }


                        //更新详情表IsComplete状态
                        if (orderInfo.TotalAmount == 0)
                        {
                            if (Update(new WXMallOrderDetailsInfo(), string.Format(" IsComplete=1"), string.Format(" OrderID='{0}' AND PID='{1}'", item.OrderID, item.PID), tran) <= 0)
                            {
                                tran.Rollback();
                                resp.Code = 1;
                                resp.Msg = "更新IsComplete状态失败";
                                return resp;
                            }
                        }

                        //更新 SKU库存 
                        System.Text.StringBuilder sbUpdateStock = new StringBuilder();//更新库存sql

                        if (string.IsNullOrEmpty(orderRequestModel.supplier_id))
                        {

                            sbUpdateStock.AppendFormat(" Update ZCJ_ProductSku Set Stock-={0} ", item.TotalCount);
                            if (bllMall.IsPromotionTime(productSku))
                            {
                                sbUpdateStock.AppendFormat(",PromotionStock-={0} ", item.TotalCount);
                            }
                            sbUpdateStock.AppendFormat(" Where SkuId={0} And Stock>0 ", productSku.SkuId);
                            if (bllMall.IsPromotionTime(productSku))
                            {
                                sbUpdateStock.AppendFormat(" And PromotionStock>0 ", item.TotalCount);
                            }
                        }
                        else
                        {
                            #region 扣门店库存
                            sbUpdateStock.AppendFormat(" Update ZCJ_ProductSkuSupplier Set Stock-={0} ", item.TotalCount);
                            //if (bllMall.IsPromotionTime(productSku))
                            //{
                            //    sbUpdateStock.AppendFormat(",PromotionStock-={0} ", item.TotalCount);
                            //}
                            sbUpdateStock.AppendFormat(" Where SkuId={0} And Stock>0 And SupplierId={1}", productSku.SkuId, orderRequestModel.supplier_id);
                            //if (bllMall.IsPromotionTime(productSku))
                            //{
                            //    sbUpdateStock.AppendFormat(" And PromotionStock>0 ", item.TotalCount);
                            //}
                            #endregion
                        }
                        if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbUpdateStock.ToString(), tran) <= 0)
                        {
                            tran.Rollback();
                            resp.Code = 1;
                            resp.Msg = "提交订单失败,库存不足";
                            resp.Result = sbUpdateStock.ToString();
                            return resp;
                        }




                    }
                    #endregion

                    bllMall.DeleteShoppingCart(currentUserInfo.UserID, orderRequestModel.skus);


                    tran.Commit();//提交订单事务

                    #region 检查是否需要拆单
                    if (orderInfo.OrderType == 0 && productList.Count(p => !string.IsNullOrEmpty(p.SupplierUserId)) > 0 && (string.IsNullOrEmpty(orderRequestModel.supplier_id)))//有供应商,拆单
                    {
                        try
                        {



                            #region 生成本次订单共有多少供应商
                            //检查有几个供应商
                            List<string> supplierUserIdList = new List<string>();//供应商列表
                            foreach (var item in productList)
                            {
                                if (string.IsNullOrEmpty(item.SupplierUserId))
                                {
                                    item.SupplierUserId = "";
                                }
                                supplierUserIdList.Add(item.SupplierUserId);
                            }
                            supplierUserIdList = supplierUserIdList.Distinct().ToList();
                            #endregion

                            #region 生成供应商对应sku
                            Dictionary<string, List<SkuModel>> dic = new Dictionary<string, List<SkuModel>>(); //key 供应商 value Sku集合
                            foreach (var suppLierUserId in supplierUserIdList)
                            {
                                var productListChild = productList.Where(p => p.SupplierUserId == suppLierUserId).ToList();
                                foreach (var item in productListChild)
                                {
                                    List<SkuModel> list = new List<SkuModel>();
                                    foreach (var detail in detailList.Where(p => p.PID == item.PID))
                                    {
                                        SkuModel model = new SkuModel();
                                        model.sku_id = (int)detail.SkuId;
                                        model.count = detail.TotalCount;
                                        list.Add(model);
                                    }
                                    if (!dic.ContainsKey(suppLierUserId))
                                    {
                                        dic.Add(suppLierUserId, list);
                                    }
                                    else
                                    {
                                        dic[suppLierUserId].AddRange(list);

                                    }

                                }



                            }
                            #endregion

                            #region 生成供应商对应订单
                            foreach (var item in dic)
                            {
                                ZentCloud.ZCBLLEngine.BLLTransaction tranChild = new ZCBLLEngine.BLLTransaction();
                                string childOrderId = GetGUID(TransacType.AddWXMallOrderInfo);
                                decimal childProductFee = 0;
                                List<WXMallOrderDetailsInfo> childOrderDetailList = new List<WXMallOrderDetailsInfo>();
                                foreach (var sku in item.Value)
                                {

                                    var detail = detailList.Single(p => p.SkuId == sku.sku_id);
                                    WXMallOrderDetailsInfo childOrderDetail = new WXMallOrderDetailsInfo();
                                    childOrderDetail.OrderID = childOrderId;
                                    childOrderDetail.PID = detail.PID;
                                    childOrderDetail.TotalCount = detail.TotalCount;
                                    childOrderDetail.OrderPrice = detail.OrderPrice;
                                    childOrderDetail.OrderScore = detail.OrderScore;
                                    childOrderDetail.SkuId = detail.SkuId;
                                    childOrderDetail.SkuShowProp = detail.SkuShowProp;
                                    childOrderDetail.ProductName = detail.ProductName;
                                    //childOrderDetail.IsComplete = detail.IsComplete;
                                    childOrderDetail.PaymentFt = detail.PaymentFt;
                                    childOrderDetail.MaxRefundAmount = detail.MaxRefundAmount;
                                    childOrderDetail.ArticleCategoryType = detail.ArticleCategoryType;
                                    childOrderDetail.ProductName = detail.ProductName;
                                    childOrderDetail.ProductImage = detail.ProductImage;
                                    childOrderDetail.BasePrice = detail.BasePrice;
                                    childOrderDetailList.Add(childOrderDetail);

                                }

                                childProductFee = (decimal)childOrderDetailList.Sum(p => p.OrderPrice * p.TotalCount);

                                WXMallOrderInfo childOrder = new WXMallOrderInfo();
                                childOrder.OrderID = childOrderId;
                                childOrder.OrderUserID = orderInfo.OrderUserID;
                                childOrder.Address = orderInfo.Address;
                                childOrder.Phone = orderInfo.Phone;
                                childOrder.TotalAmount = childProductFee;
                                childOrder.InsertDate = orderInfo.InsertDate;
                                childOrder.OrderMemo = orderInfo.OrderMemo;
                                childOrder.Consignee = orderInfo.Consignee;
                                childOrder.WebsiteOwner = orderInfo.WebsiteOwner;
                                childOrder.Status = orderInfo.Status;
                                childOrder.DeliveryTime = orderInfo.DeliveryTime;
                                childOrder.PaymentType = orderInfo.PaymentType;
                                childOrder.PaymentStatus = orderInfo.PaymentStatus;
                                childOrder.Product_Fee = childProductFee;
                                childOrder.ClaimArrivalTime = orderRequestModel.claim_arrival_time;
                                childOrder.CustomCreaterName = orderRequestModel.custom_creater_name;
                                childOrder.CustomCreaterPhone = orderRequestModel.custom_creater_phone;
                                #region 供应商运费
                                if (isNoExpress == 0)
                                {

                                    FreightModel fre = new FreightModel();
                                    fre.receiver_province_code = orderRequestModel.receiver_province_code;
                                    fre.receiver_city_code = orderRequestModel.receiver_city_code;
                                    fre.receiver_dist_code = orderRequestModel.receiver_dist_code;
                                    fre.skus = new List<SkuModel>();
                                    fre.skus = item.Value;
                                    decimal freightChild = 0;
                                    string msgChild = "";
                                    bllMall.CalcFreight(fre, out freightChild, out msgChild);
                                    childOrder.Transport_Fee = freightChild;
                                }

                                #endregion


                                childOrder.DistributionStatus = orderInfo.DistributionStatus;
                                childOrder.CouponNumber = orderInfo.CouponNumber;
                                childOrder.ReceiverProvince = orderInfo.ReceiverProvince;
                                childOrder.ReceiverProvinceCode = orderInfo.ReceiverProvinceCode;
                                childOrder.ReceiverCity = orderInfo.ReceiverCity;
                                childOrder.ReceiverCityCode = orderInfo.ReceiverCityCode;
                                childOrder.ReceiverDist = orderInfo.ReceiverDist;
                                childOrder.ReceiverDistCode = orderInfo.ReceiverDistCode;
                                childOrder.ZipCode = orderInfo.ZipCode;
                                childOrder.UseScore = orderInfo.UseScore;
                                childOrder.MyCouponCardId = orderInfo.MyCouponCardId;
                                childOrder.CouponType = orderInfo.CouponType;
                                childOrder.SellerId = orderInfo.SellerId;
                                childOrder.OrderType = orderInfo.OrderType;
                                childOrder.PayTime = orderInfo.PayTime;
                                childOrder.LastUpdateTime = orderInfo.LastUpdateTime;
                                childOrder.CardcouponDisAmount = orderInfo.CardcouponDisAmount;
                                childOrder.ArticleCategoryType = orderInfo.ArticleCategoryType;
                                childOrder.UseAmount = orderInfo.UseAmount;
                                childOrder.ScoreExchangAmount = orderInfo.ScoreExchangAmount;
                                childOrder.IsNoExpress = orderInfo.IsNoExpress;
                                childOrder.IsNeedNamePhone = orderInfo.IsNeedNamePhone;
                                childOrder.IsAppointment = orderInfo.IsAppointment;
                                childOrder.SupplierUserId = item.Key;
                                childOrder.PaymentType = orderInfo.PaymentType;
                                var supplierInfo = bllUser.GetUserInfo(childOrder.SupplierUserId, childOrder.WebsiteOwner);
                                if (supplierInfo != null)
                                {
                                    childOrder.SupplierName = supplierInfo.Company;
                                }
                                childOrder.ParentOrderId = orderInfo.OrderID;
                                decimal rate = childOrder.Product_Fee / orderInfo.Product_Fee;
                                if (orderInfo.UseScore > 0)//均摊积分
                                {

                                    childOrder.UseScore = (int)(orderInfo.UseScore * rate);
                                    childOrder.ScoreExchangAmount = orderInfo.ScoreExchangAmount * rate;

                                }
                                if (orderInfo.UseAmount > 0)//均摊余额
                                {

                                    childOrder.UseAmount = orderInfo.UseAmount * rate;


                                }
                                if (orderInfo.CardcouponDisAmount > 0)//均摊优惠券
                                {

                                    childOrder.CardcouponDisAmount = orderInfo.CardcouponDisAmount * rate;

                                }
                                //childOrder.TotalAmount += childOrder.Transport_Fee;
                                //childOrder.TotalAmount -= childOrder.UseAmount;
                                //childOrder.TotalAmount -= childOrder.CardcouponDisAmount;
                                //childOrder.TotalAmount -= childOrder.ScoreExchangAmount;
                                //childOrder.Transport_Fee = orderInfo.Transport_Fee * rate;
                                if (!orderInfo.IsAllCash)
                                {
                                    childOrder.TotalAmount = orderInfo.TotalAmount * rate;
                                }
                                else
                                {
                                    childOrder.TotalAmount = childOrder.Product_Fee + childOrder.Transport_Fee;
                                }
                                if (orderInfo.TotalAmount == 0)
                                {
                                    childOrder.PaymentStatus = 1;
                                    childOrder.PayTime = DateTime.Now;
                                    childOrder.Status = "待发货";
                                    if (orderInfo.DeliveryType == 1)
                                    {
                                        orderInfo.Status = "待自提";
                                    }

                                }
                                if (!Add(childOrder, tranChild))
                                {
                                    tranChild.Rollback();
                                    resp.Code = 1;
                                    resp.Msg = "拆分订单失败";
                                    return resp;
                                }
                                if (!AddList<WXMallOrderDetailsInfo>(childOrderDetailList))
                                {
                                    tranChild.Rollback();
                                    resp.Code = 1;
                                    resp.Msg = "拆分订单失败";
                                    return resp;
                                }
                                tranChild.Commit();

                            }
                            #endregion
                            orderInfo.IsMain = 1;
                            Update(orderInfo, string.Format("IsMain=1,SupplierUserId='',SupplierName=''"), string.Format("OrderId='{0}'", orderInfo.OrderID));
                        }
                        catch (Exception ex)
                        {

                            resp.Code = 1;
                            resp.Msg = ex.ToString();
                            return resp;
                        }
                    }
                    #endregion

                    #region 宏巍通知
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        hongWareClient.OrderNotice(currentUserInfo.WXOpenId, orderInfo.OrderID);

                    }
                    #endregion

                    try
                    {

                        #region 团购完成取消其它未付款订单
                        if (orderInfo.OrderType == 2)
                        {
                            //团购完成取消其它未付款订单
                            //系统开团
                            if (parentOrderInfo.Ex10 == "1")
                            {
                                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And  (GroupBuyParentOrderId='{0}' )", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                {
                                    bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                    parentOrderInfo.GroupBuyStatus = "1";
                                    bllMall.Update(parentOrderInfo);
                                }
                            }
                            else
                            {
                                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And  (GroupBuyParentOrderId='{0}' Or OrderId='{0}')", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                {
                                    bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                    parentOrderInfo.GroupBuyStatus = "1";
                                    bllMall.Update(parentOrderInfo);
                                }
                            }

                        }
                        #endregion

                        #region 微信模板消息

                        if (orderInfo.TotalAmount > 0)
                        {
                            var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;

                            string title = "订单已成功提交";
                            if (orderInfo.TotalAmount > 0)
                            {
                                title += ",请尽快付款";
                            }
                            string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
                            if (orderInfo.IsMain == 1)
                            {
                                url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderList#/orderList", context.Request.Url.Host);
                            }
                            bllWeiXin.SendTemplateMessageNotifyComm(currentUserInfo, title, string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品：{4}", orderInfo.OrderID, Math.Round(orderInfo.TotalAmount, 2), orderInfo.Consignee, orderInfo.Phone, productName), url);

                        }
                        else
                        {
                            var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;
                            string remark = "";
                            if (!string.IsNullOrEmpty(orderInfo.OrderMemo))
                            {
                                remark = string.Format("客户留言:{0}", orderInfo.OrderMemo);
                            }
                            bllWeiXin.SendTemplateMessageToKefu("有新的订单", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}\\n{5}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName, remark));
                        }

                        #endregion

                        if (websiteInfo.WebsiteOwner != "mixblu")
                        {
                            //bllScore.ToLog("判断订单类型是否可获得积分", "D://DALlog.txt");
                            //判断订单类型是否可获得积分                      
                            if (bllScore.IsCanRebateScoreByOrderType(currOrderType, orderRequestModel.groupbuy_type))
                            {
                                //bllScore.ToLog("订单类型支持获得积分", "D://DALlog.txt");
                                //即将到账积分计算，并存入即将到账积分表，若取消、退款、删除订单，积分到账应该打上移除标志并追加有备注内容            
                                bllScore.AddLockScoreByOrder(orderInfo);
                            }
                        }


                        if (orderInfo.TotalAmount == 0)
                        {
                            #region Efast同步
                            //判读当前站点是否需要同步到驿氪和efast
                            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                            {
                                try
                                {

                                    string outOrderId = string.Empty, msg1 = string.Empty;
                                    var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out msg1);
                                    if (syncResult)
                                    {
                                        orderInfo.OutOrderId = outOrderId;
                                        Update(orderInfo);
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            #endregion

                            #region 驿氪同步
                            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                            {
                                try
                                {

                                    var uploadOrderResult = yiKeClient.OrderUpload(orderInfo);

                                }
                                catch (Exception ex)
                                {


                                }
                            }
                            #endregion

                            #region 购买指定商品发送指定的优惠券
                            string v1ProductId = Common.ConfigHelper.GetConfigString("YGBV1ProductId");
                            string v2ProductId = Common.ConfigHelper.GetConfigString("YGBV2ProductId");
                            string v1CouponId = Common.ConfigHelper.GetConfigString("YGBV1CouponId");
                            string v2CouponId = Common.ConfigHelper.GetConfigString("YGBV2CouponId");
                            foreach (var item in orderRequestModel.skus)
                            {
                                ProductSku skuModel = bllMall.GetProductSku(item.sku_id);
                                if (skuModel == null) continue;
                                if (!string.IsNullOrEmpty(v1ProductId) && !string.IsNullOrEmpty(v1CouponId) && skuModel.ProductId.ToString() == v1ProductId)
                                {
                                    //发送v1优惠券
                                    bllCardCoupon.SendCardCouponsByCurrUserInfo(currentUserInfo, v1CouponId);
                                }
                                if (!string.IsNullOrEmpty(v2ProductId) && !string.IsNullOrEmpty(v2CouponId) && skuModel.ProductId.ToString() == v2ProductId)
                                {
                                    //发送v2优惠券
                                    bllCardCoupon.SendCardCouponsByCurrUserInfo(currentUserInfo, v2CouponId);
                                }
                            }

                            #endregion

                            #region 更新销量
                            bllMall.UpdateProductSaleCount(orderInfo);
                            #endregion

                        }

                        bllMall.ClearProductListCacheByOrder(orderInfo);
                    }
                    catch { }


                    #region 自动分单
                    try
                    {

                        //var companyConfig = bllMall.Get<CompanyWebsite_Config>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
                        bool isNeedAssignOrder = false;//是否需要自动分单
                        if (companyConfig != null && companyConfig.IsAutoAssignOrder == 1 && string.IsNullOrEmpty(orderRequestModel.supplier_id))
                        {
                            if (orderInfo.DeliveryType == 2)
                            {
                                isNeedAssignOrder = true;
                            }
                        }
                        if (isNeedAssignOrder)//
                        {
                            var geoResult = ZentCloud.Common.AMapHelper.GetGeoByAddress(orderInfo.ReceiverProvince + orderInfo.ReceiverCity + orderInfo.ReceiverDist);
                            if (geoResult.status)
                            {
                                BLLJuActivity bllJuActivity = new BLLJuActivity();
                                int outLetTotal = 0;
                                List<JuActivityInfo> storeList = bllJuActivity.GetOutletsList(10000, 1, "", "", "", false,
                            "JuActivityID,ActivityName,ActivityAddress,ThumbnailsPath,Sort,K4,K5,UserLongitude,UserLatitude", out outLetTotal,
                            geoResult.longitude.ToString(), geoResult.latitude.ToString(), int.Parse(companyConfig.AutoAssignOrderRange), "range", bllJuActivity.WebsiteOwner, "", "", "1", "");
                                if (storeList.Count > 0)
                                {

                                    storeList = storeList.Where(p => !string.IsNullOrEmpty(p.K5)).ToList();//没绑定过供应商Id的排除
                                    storeList = storeList.OrderBy(p => p.Distance).ToList();//距离从近到远
                                    foreach (var item in storeList)
                                    {
                                        #region 检查是否可以把订单分配给当前门店
                                        var canAssignOrder = true;//是否可以把订单分给当前门店
                                        foreach (var detail in detailList)
                                        {
                                            var supplierSku = Get<ProductSkuSupplier>(string.Format(" SkuId={0} And SupplierId={1}", detail.SkuId, item.K5));
                                            if (supplierSku == null)
                                            {
                                                canAssignOrder = false;
                                                break;
                                            }
                                            if (supplierSku.Stock < detail.TotalCount)
                                            {
                                                canAssignOrder = false;
                                                break;
                                            }


                                        }
                                        #endregion

                                        #region 可以把订单分配给当前门店
                                        if (canAssignOrder)
                                        {
                                            var supplierUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(item.K5));
                                            if (supplierUserInfo != null)
                                            {
                                                string storeAddress = "";
                                                if (orderInfo.DeliveryType == 1)
                                                {
                                                    var storeInfo = bllJuactivity.GetStoreById(supplierUserInfo.AutoID.ToString());
                                                    if (storeInfo != null)
                                                    {
                                                        storeAddress = storeInfo.Province + storeInfo.City + storeInfo.District + storeInfo.ActivityAddress;
                                                    }
                                                }
                                                if (Update(orderInfo, string.Format("SupplierUserId='{0}',SupplierName='{1}',StoreAddress='{2}'", supplierUserInfo.UserID, supplierUserInfo.Company, storeAddress), string.Format(" OrderId='{0}'", orderInfo.OrderID)) > 0)
                                                {
                                                    break;//分单成功,跳出循环
                                                }
                                                else
                                                {
                                                    resp.Code = 1;
                                                    resp.Msg = "自动分单失败,店铺名称:" + item.ActivityName;
                                                    return resp;
                                                }

                                            }



                                        }
                                        #endregion


                                    }


                                }
                            }



                        }
                    }
                    catch (Exception ex)
                    {
                        resp.Code = 1;
                        resp.Msg = ex.ToString();
                        return resp;

                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    //回滚事物
                    tran.Rollback();
                    resp.Code = 1;
                    resp.Msg = "提交订单失败";
                    return resp;
                }

                resp.Code = 0;
                resp.Msg = "ok";
                resp.OrderId = orderInfo.OrderID;
                //resp.Result = new
                //{
                //    is_main=orderInfo.IsMain
                //};
                return resp;


            }
            catch (Exception ex)
            {
                AddOrderResp resp = new AddOrderResp();
                resp.Code = -1;
                resp.Result = ex.ToString();
                return resp;
            }

        }

        /// <summary>
        /// 代付订单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public BaseResponse Update(HttpContext context)
        {

            //提交json示例:
            //data=
            //{ 
            //  "order_id":"45846",//订单号
            //  "use_amount":"1",//余额
            //  "cardcoupon_id":"15687"//储值卡id
            //}
            string data = context.Request["data"];//订单数据json字符串
            OrderRequestModel orderRequestModel;//订单请求模型
            BLLMall bllMall = new BLLMall();//商城
            BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();//储值卡
            BaseResponse resp = new BaseResponse();//响应模型
            var currentUserInfo = GetCurrentUserInfo();//当前用户
            try
            {
                orderRequestModel = Common.JSONHelper.JsonToModel<OrderRequestModel>(data);
            }
            catch (Exception ex)
            {
                resp.Code = 1;
                resp.Msg = "JSON格式错误,请检查。错误信息:" + ex.ToString();
                return resp;
            }


            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderRequestModel.order_id);
            if (orderInfo == null)
            {
                resp.Code = 1;
                resp.Msg = "订单号不存在";
                return resp;
            }
            if (orderInfo.PaymentStatus == 1)
            {
                resp.Code = 1;
                resp.Msg = "该订单己支付";
                return resp;
            }
            if (orderInfo.Status == "已取消")
            {
                resp.Code = 1;
                resp.Msg = "该订单己取消";
                return resp;
            }

            #region 余额代付
            if (orderRequestModel.use_amount > 0)
            {
                if (!bllMall.IsEnableAccountAmountPay())
                {
                    resp.Code = 1;
                    resp.Msg = "尚未启用余额支付功能";
                    return resp;
                }

                if (currentUserInfo.AccountAmount < orderRequestModel.use_amount)
                {

                    resp.Code = 1;
                    resp.Msg = "您的账户余额不足";
                    return resp;
                }
                currentUserInfo.AccountAmount -= orderRequestModel.use_amount;
                if (Update(currentUserInfo, string.Format(" AccountAmount={0}", currentUserInfo.AccountAmount), string.Format(" AutoID={0}", currentUserInfo.AutoID), tran) < 0)
                {
                    tran.Rollback();
                    resp.Code = 1;
                    resp.Msg = "更新用户余额失败";
                    return resp;
                }

                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.AddTime = DateTime.Now;
                scoreRecord.Score = -(double)orderRequestModel.use_amount;
                scoreRecord.TotalScore = (double)currentUserInfo.AccountAmount;
                scoreRecord.UserID = currentUserInfo.UserID;
                scoreRecord.AddNote = "微商城-代付订单使用余额";
                scoreRecord.RelationID = orderInfo.OrderID;
                scoreRecord.WebSiteOwner = WebsiteOwner;
                scoreRecord.ScoreType = "AccountAmount";
                if (!bllMall.Add(scoreRecord, tran))
                {
                    tran.Rollback();
                    resp.Code = 1;
                    resp.Msg = "插入余额记录失败";
                    return resp;
                }

                orderInfo.OtherUseAmount += orderRequestModel.use_amount;
                orderInfo.TotalAmount -= orderInfo.OtherUseAmount;
            }
            #endregion

            #region 储值卡代付
            StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
            decimal discountAmount = 0;//优惠金额
            if (orderRequestModel.cardcoupon_id > 0)//有优惠券
            {
                storeValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0}", orderRequestModel.cardcoupon_id));
                discountAmount = bllMall.CalcDiscountAmountStoreValue(orderInfo.TotalAmount, storeValueCardRecord.UserId, orderRequestModel.cardcoupon_id.ToString());
                orderInfo.OtherMyCouponCardId = orderRequestModel.cardcoupon_id.ToString();

                StoredValueCardRecord myStoredValueCardRecord = bllStoreValue.Get<StoredValueCardRecord>(string.Format("AutoId={0} And (UserId='{1}' Or ToUserId='{1}')", orderRequestModel.cardcoupon_id, currentUserInfo.UserID));
                myStoredValueCardRecord.UseDate = DateTime.Now;
                myStoredValueCardRecord.Status = 9;
                if (!bllStoreValue.Update(myStoredValueCardRecord, tran))
                {
                    tran.Rollback();
                    resp.Code = 1;
                    resp.Msg = "更新储值卡状态失败";
                    return resp;
                }
                StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
                storeValueCardUseRecord.UseAmount = discountAmount;
                storeValueCardUseRecord.CardId = myStoredValueCardRecord.CardId;
                storeValueCardUseRecord.Remark = string.Format("商城下单,订单号:{0}使用{1}元", orderInfo.OrderID, Math.Round(discountAmount, 2));
                storeValueCardUseRecord.UseDate = DateTime.Now;
                storeValueCardUseRecord.UserId = storeValueCardRecord.UserId;
                storeValueCardUseRecord.UseUserId = currentUserInfo.UserID;
                storeValueCardUseRecord.WebsiteOwner = orderInfo.WebsiteOwner;
                storeValueCardUseRecord.MyCardId = orderRequestModel.cardcoupon_id;
                storeValueCardUseRecord.OrderId = orderInfo.OrderID;
                if (!bllStoreValue.Add(storeValueCardUseRecord, tran))
                {
                    tran.Rollback();
                    resp.Code = 1;
                    resp.Msg = "更新储值卡状态失败";
                    return resp;
                }



                orderInfo.OtherCouponType = 1;
                orderInfo.OtherCardcouponDisAmount += discountAmount;
                orderInfo.TotalAmount -= orderInfo.OtherCardcouponDisAmount;

            }
            #endregion

            orderInfo.OtherUserId = currentUserInfo.UserID;
            if (orderInfo.TotalAmount <= 0)
            {
                orderInfo.TotalAmount = 0;
                orderInfo.PaymentStatus = 1;
                orderInfo.Status = "待发货";
                orderInfo.PayTime = DateTime.Now;
                if (orderInfo.DeliveryType == 1)
                {
                    orderInfo.Status = "待自提";
                }
            }

            #region 最大退款金额
            decimal refundTotalAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;
            if (refundTotalAmount < 0)
            {
                refundTotalAmount = 0;
            }
            var detailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            var productTotalPrice = ((decimal)detailList.Sum(p => p.OrderPrice * p.TotalCount));
            foreach (var orderDetail in detailList)
            {
                decimal rate = 0;//此sku所占的比例
                if (productTotalPrice > 0)
                {
                    rate = ((decimal)orderDetail.OrderPrice * orderDetail.TotalCount) / productTotalPrice;//此sku所占的比例
                }
                orderDetail.MaxRefundAmount = Math.Round((refundTotalAmount * rate), 2, MidpointRounding.AwayFromZero);

            }
            decimal cha = refundTotalAmount - detailList.Sum(p => p.MaxRefundAmount); //100 99.99 100 100.01  //差值补到最后一种商品
            if (cha != 0)
            {
                detailList[detailList.Count - 1].MaxRefundAmount += cha;
            }
            foreach (var orderDetail in detailList)
            {
                bllMall.Update(orderDetail);
            }



            #endregion


            if (Update(orderInfo, tran))
            {
                tran.Commit();
                resp.Status = true;
                resp.Msg = "ok";
                resp.Result = new { total_amount = orderInfo.TotalAmount };
            }
            else
            {
                tran.Rollback();
                resp.Code = 1;
                resp.Msg = "代付失败";
                return resp;
            }
            return resp;

        }

        /// <summary>
        /// 取消代付
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public BaseResponse CancelUpdate(HttpContext context)
        {

            BaseResponse resp = new BaseResponse();//响应模型
            BLLMall bllMall = new BLLMall();
            BLLTransaction tran = new BLLTransaction();
            BLLStoredValueCard bllStoreValue = new BLLStoredValueCard();//储值卡
            BLLUser bllUser = new BLLUser();
            string orderId = context.Request["order_id"];

            var orderInfo = bllMall.GetOrderInfo(orderId);


            var otherUserInfo = bllUser.GetUserInfo(orderInfo.OtherUserId);//当前用户

            var curUser = GetCurrentUserInfo();

            if (orderInfo == null)
            {
                resp.Msg = "订单号不存在";
                resp.Code = 1;
                return resp;
            }
            if (curUser.UserID != orderInfo.OrderUserID && curUser.UserID != orderInfo.OtherUserId)
            {
                resp.Code = 1;
                resp.Msg = "无权取消代付订单";
                return resp;
            }

            if (orderInfo.PaymentStatus == 1)
            {
                resp.Code = 1;
                resp.Msg = "该订单己支付";
                return resp;
            }
            if (orderInfo.OtherUseAmount > 0 || orderInfo.OtherCardcouponDisAmount > 0)
            {
                resp.Code = 1;
                resp.Msg = "使用余额或储值卡不能取消订单";
                return resp;
            }
            if (orderInfo.Status == "已取消")
            {
                resp.Code = 1;
                resp.Msg = "该订单己取消";
                return resp;
            }

            #region 退余额(取消代付)
            //if (orderInfo.OtherUseAmount > 0)
            //{
            //    otherUserInfo.AccountAmount += orderInfo.OtherUseAmount;
            //    if (Update(otherUserInfo, string.Format(" AccountAmount={0}", otherUserInfo.AccountAmount), string.Format(" AutoID={0}", otherUserInfo.AutoID), tran) < 0)
            //    {
            //        tran.Rollback();
            //        resp.Code = 1;
            //        resp.Msg = "更新用户余额失败";
            //        return resp;
            //    }

            //    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //    scoreRecord.AddTime = DateTime.Now;
            //    scoreRecord.Score = (double)orderInfo.OtherUseAmount;
            //    scoreRecord.TotalScore = (double)otherUserInfo.AccountAmount;
            //    scoreRecord.UserID = otherUserInfo.UserID;
            //    scoreRecord.AddNote = "微商城-取消代付订单";
            //    scoreRecord.RelationID = orderInfo.OrderID;
            //    scoreRecord.WebSiteOwner = WebsiteOwner;
            //    scoreRecord.ScoreType = "AccountAmount";
            //    if (!bllMall.Add(scoreRecord, tran))
            //    {
            //        tran.Rollback();
            //        resp.Code = 1;
            //        resp.Msg = "插入余额记录失败";
            //        return resp;
            //    }
            //    orderInfo.TotalAmount += orderInfo.OtherUseAmount;
            //}
            #endregion

            #region 储值卡（取消代付）
            //if (!string.IsNullOrEmpty(orderInfo.OtherMyCouponCardId))//有优惠券
            //{
            //    StoredValueCardUseRecord useCard = bllMall.Get<StoredValueCardUseRecord>(string.Format("  WebsiteOwner='{0}' AND OrderId='{1}' AND UseUserId='{2}'   ", WebsiteOwner, orderId, otherUserInfo.UserID));

            //    if (bllMall.Delete(useCard, tran)<=0)
            //    {
            //        tran.Rollback();
            //        resp.Code = 1;
            //        resp.Msg = "删除储值卡使用记录出错";
            //        return resp;
            //    }
            //    orderInfo.TotalAmount += orderInfo.OtherCardcouponDisAmount;
            //}



            #endregion

            orderInfo.OtherUserId = "";
            orderInfo.OtherUseAmount = 0;
            orderInfo.OtherCardcouponDisAmount = 0;
            orderInfo.OtherCouponType = 0;
            orderInfo.OtherMyCouponCardId = "";

            if (Update(orderInfo, tran))
            {
                tran.Commit();
                resp.Status = true;
                resp.Msg = "取消代付成功";
            }
            else
            {
                tran.Rollback();
                resp.Code = 1;
                resp.Msg = "取消代付失败";
            }

            return resp;
        }
        /// <summary>
        /// 获取代付金额
        /// </summary>
        /// <returns></returns>
        public decimal GetOrderDaiPayAmount(WXMallOrderInfo orderInfo)
        {

            decimal daiPayTotal = 0;//代付总金额

            StoredValueCardRecord storeValueCardRecord = new StoredValueCardRecord();
            if (!string.IsNullOrEmpty(orderInfo.OtherMyCouponCardId))
            {
                daiPayTotal += orderInfo.OtherCardcouponDisAmount;
            }
            if (orderInfo.OtherUseAmount > 0)
            {
                daiPayTotal += orderInfo.OtherUseAmount;
            }
            if (orderInfo.TotalAmount > 0)
            {
                daiPayTotal += orderInfo.TotalAmount;
            }

            return daiPayTotal;

        }

        /// <summary>
        /// 获取订单正在退款或者已经退款成功的笔数
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetOrderHasRefundCount(string orderId)
        {
            BLLMall bllMall = new BLLMall();
            var result = 0;

            var orderDetailList = bllMall.GetOrderDetailsList(orderId);

            foreach (var item in orderDetailList)
            {
                if (!string.IsNullOrWhiteSpace(item.RefundStatus) && item.RefundStatus != "7")
                {
                    result++;
                }
            }

            return result;
        }



    }
}









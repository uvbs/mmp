using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;
using System.Text;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.GroupBuy.Order
{
    /// <summary>
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
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
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeiXin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 分销
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        /// <summary>
        /// 微信
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
                if (hongWeiWareMemberInfo.member == null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "您尚未绑定宏巍账号,请先绑定";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
            }
            string data = context.Request["data"];
            if (string.IsNullOrEmpty(data))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "data 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            decimal productFee = 0;//商品总价格 不包含邮费
            OrderModel orderRequestModel;//订单模型
            try
            {
                orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderModel>(data);

            }
            catch (Exception ex)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;


            }
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.Address = orderRequestModel.receiver_address;
            orderInfo.Consignee = orderRequestModel.receiver_name;
            //orderInfo.ExpressCompanyName = orderRequestModel.express_company;
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
            orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();
            orderInfo.UseScore = orderRequestModel.use_score;
            orderInfo.UseAmount = orderRequestModel.use_amount;
            orderInfo.Status = "待付款";
            orderInfo.OrderType = 2;
            orderInfo.LastUpdateTime = DateTime.Now;

            //if (!string.IsNullOrEmpty(orderRequestModel.sale_id))
            //{
            //    long saleId = 0;
            //    if (long.TryParse(orderRequestModel.sale_id, out saleId))
            //    {
            //        orderInfo.SellerId = saleId;
            //    }
            //    else
            //    {

            //    }


            //}
            if (orderRequestModel.pay_type == "WEIXIN")//微信支付
            {
                orderInfo.PaymentType = 2;
            }
            else if (orderRequestModel.pay_type == "ALIPAY")//支付宝支付
            {
                orderInfo.PaymentType = 1;
            }

            #region 格式检查


            //相关检查
            if (string.IsNullOrEmpty(orderRequestModel.rule_id))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "rule_id 必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (string.IsNullOrEmpty(orderInfo.Consignee))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "收货人姓名不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            if (string.IsNullOrEmpty(orderInfo.Phone))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "收货人联系电话不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverProvince))
            {

                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "省份名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverProvinceCode))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "省份代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverCity))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "城市名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
            {

                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "城市代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverDist))
            {

                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "城市区域名称不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.ReceiverCityCode))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "城市区域代码不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderInfo.Address))
            {

                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "收货地址不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderRequestModel.skus == null)
            {

                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "参数skus 不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }


            //相关检查 
            #endregion

            #region 分销关系建立
            if (websiteInfo.DistributionRelationBuildMallOrder == 1)
            {

                if (!string.IsNullOrEmpty(orderRequestModel.sale_id))
                {
                    int saleId = 0;
                    if (int.TryParse(orderRequestModel.sale_id, out saleId))
                    {
                        orderInfo.SellerId = saleId;
                        if (string.IsNullOrEmpty(CurrentUserInfo.DistributionOwner))
                        {
                            var recommendUserInfo = bllUser.GetUserInfoByAutoID(saleId);
                            if (recommendUserInfo != null)
                            {
                                var setUserDistributionOwnerResult =  bllDis.SetUserDistributionOwner(CurrentUserInfo.UserID, recommendUserInfo.UserID, CurrentUserInfo.WebsiteOwner);

                                if (setUserDistributionOwnerResult)
                                {
                                    CurrentUserInfo.DistributionOwner = recommendUserInfo.UserID;
                                    CurrentUserInfo.Channel = recommendUserInfo.Channel;
                                }
                               
                            }

                        }


                    }
                    else
                    {

                    }


                }
                else
                {
                    if (string.IsNullOrEmpty(CurrentUserInfo.DistributionOwner))
                    {

                        CurrentUserInfo.DistributionOwner = bllUser.WebsiteOwner;
                        bllUser.Update(CurrentUserInfo);

                    }

                }

            }
            #endregion

            if (orderRequestModel.skus.Count > 1)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "只能购买一种商品";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderRequestModel.skus.Sum(p => p.count) > 1)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "团购商品只能购买一件";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }


            List<ProductGroupBuyRule> groupBuyList = new List<ProductGroupBuyRule>();//此商品的团购规则列表

            #region 购买的商品
            List<WXMallProductInfo> productList = new List<WXMallProductInfo>();
            foreach (var sku in orderRequestModel.skus)
            {
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                if (productSku == null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "sku_id 无效";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
               // WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                //productList.Add(productInfo);

                //ProductSku productSku = bllMall.GetProductSku(sku.sku_id);

                //if (productSku == null) continue;
                if (productList.Count(p => p.PID == productSku.ProductId.ToString()) > 0) continue;

                WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                productList.Add(productInfo);
                groupBuyList = bllMall.GetProductGroupBuyRuleList(productSku.ProductId.ToString());


            }

            if (groupBuyList.Count == 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "此商品不能团购";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id)) == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "rule_id 错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            ProductGroupBuyRule groupBuyRule = groupBuyList.SingleOrDefault(p => p.RuleId == int.Parse(orderRequestModel.rule_id));//团购规则

           // productList = productList.Distinct().ToList();
            #endregion

            string cardCouponType = "";//优惠券类型
            #region 检查优惠券是否可用
            if (orderRequestModel.cardcoupon_id > 0)
            {
                var mycardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, CurrentUserInfo.UserID);
                if (mycardCoupon == null)
                {
                    apiResp.code = 1;
                    apiResp.msg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
                if (cardCoupon == null)
                {
                    apiResp.code = 1;
                    apiResp.msg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                cardCouponType = cardCoupon.CardCouponType;
                #region 需要购买指定商品
                if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
                {

                    if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
                    {
                        var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
                        apiResp.code = 1;
                        apiResp.msg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }


                }
                #endregion

                #region 需要购买指定标签商品
                if (!string.IsNullOrEmpty(cardCoupon.Ex8))
                {
                    if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
                    {
                        apiResp.code = 1;
                        apiResp.msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;

                    }
                    bool checkResult = true;
                    foreach (var product in productList)
                    {
                        if (!string.IsNullOrEmpty(product.Tags))
                        {
                            bool tempResult = false;
                            foreach (string tag in product.Tags.Split(','))
                            {
                                if (cardCoupon.Ex8.Contains(tag))
                                {
                                    tempResult = true;
                                    break;
                                }
                            }
                            if (!tempResult)
                            {
                                checkResult = false;
                                break;
                            }


                        }
                        else//商品不包含标签
                        {

                            checkResult = false;
                            break;
                        }

                    }
                    if (!checkResult)
                    {
                        apiResp.code = 1;
                        apiResp.msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                }
                #endregion


            }
            #endregion

            var needUseScore = 0;//必须使用的积分
            decimal needUseCash = 0;//必须使用的现金
            #region 商品检查 订单详情生成
            ///订单详情
            List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情
            orderRequestModel.skus = orderRequestModel.skus.Distinct().ToList();
            foreach (var sku in orderRequestModel.skus)
            {
                //先检查库存
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                if (productSku == null)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "SKU不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

                WXMallProductInfo productInfo = productList.Single(p => p.PID == productSku.ProductId.ToString());
                if (productInfo.IsOnSale == "0")
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = string.Format("{0}已下架", productInfo.PName);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (productSku.Stock < sku.count)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = string.Format("{0}{1}库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.Stock);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (productInfo.Score > 0)//必须使用的积分
                {
                    needUseScore += sku.count * productInfo.Score;
                }
                if (productInfo.IsCashPayOnly == 1)//必须使用的现金
                {
                    needUseCash += (Math.Round((decimal)(productSku.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2)) * sku.count;
                }
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderInfo.OrderID;
                detailModel.PID = productInfo.PID;
                detailModel.TotalCount = sku.count;
                detailModel.OrderPrice = Math.Round((decimal)(productSku.Price * (decimal)(groupBuyRule.HeadDiscount / 10)), 2);//四舍五入
                detailModel.ProductName = productInfo.PName;
                detailModel.SkuId = productSku.SkuId;
                detailModel.SkuShowProp = bllMall.GetProductShowProperties(productSku.SkuId);
                detailModel.IsComplete = 1;//拼团的只要下单就算销量
                detailList.Add(detailModel);


            }
            #endregion

            #region 纯积分购买
            if (needUseScore > 0)
            {
                if ((CurrentUserInfo.TotalScore < needUseScore) || (orderRequestModel.use_score < needUseScore))
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = string.Format("您需要使用{0}积分来兑换， 可用积分不足", needUseScore);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;

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
            if (!bllMall.CalcFreight(freightModel, out freight, out  freightMsg))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = freightMsg;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            orderInfo.Transport_Fee = freight;
            #endregion

            #region 优惠券计算

            decimal discountAmount = 0;//优惠金额
            bool canUseCardCoupon = false;
            string msg = "";
            if (orderRequestModel.cardcoupon_id > 0)//有优惠券
            {


                discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, CurrentUserInfo.UserID, out canUseCardCoupon, out msg);
                if (!canUseCardCoupon)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = msg;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (cardCouponType == "MallCardCoupon_FreeFreight")//免邮券
                {
                    orderInfo.Transport_Fee = 0;
                }

            }
            //优惠券计算 
            #endregion

            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (orderRequestModel.use_score > 0)
            {

                #region 使用宏巍积分
                if (websiteInfo.IsUnionHongware == 1)
                {
                    CurrentUserInfo.TotalScore = hongWeiWareMemberInfo.member.point;

                }
                #endregion
                if (CurrentUserInfo.TotalScore < orderRequestModel.use_score)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "积分不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }

                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                if (scoreConfig != null && scoreConfig.ExchangeAmount > 0)
                {
                    scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);

                }
                //scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);


            }



            //积分计算 
            #endregion

            #region 使用账户余额
            if (orderRequestModel.use_amount > 0)
            {
                if (!bllMall.IsEnableAccountAmountPay())
                {

                    apiResp.msg = "尚未启用余额支付功能";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                #region 使用宏巍余额
                if (websiteInfo.IsUnionHongware == 1)
                {

                    CurrentUserInfo.AccountAmount = (decimal)hongWeiWareMemberInfo.member.balance;

                }
                #endregion

                if (CurrentUserInfo.AccountAmount < orderRequestModel.use_amount)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "您的账户余额不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
            }
            #endregion

            //合计计算
            orderInfo.Product_Fee = productFee;
            orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
            orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
            orderInfo.TotalAmount -= orderRequestModel.use_amount;//余额
            orderInfo.PayableAmount = orderInfo.TotalAmount - freight;//应付金额
            orderInfo.ScoreExchangAmount = scoreExchangeAmount;//优惠券抵扣金额
            orderInfo.CardcouponDisAmount = discountAmount;//卡券抵扣金额
            if ((productFee + orderInfo.Transport_Fee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
            {


                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }

            if (orderInfo.TotalAmount < 0)
            {
                orderInfo.TotalAmount = 0;

            }
            if (orderInfo.TotalAmount == 0)
            {
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "待发货";
            }
            orderInfo.HeadDiscount = groupBuyRule.HeadDiscount;
            orderInfo.MemberDiscount = groupBuyRule.MemberDiscount;
            orderInfo.PeopleCount = groupBuyRule.PeopleCount;
            orderInfo.ExpireDay = groupBuyRule.ExpireDay;

            if (orderInfo.TotalAmount < needUseCash)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = string.Format("最少需要支付{0}元" + needUseCash);
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {

                if (!this.bllMall.Add(orderInfo, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "提交失败";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

                #region 更新优惠券使用状态
                //优惠券
                if (orderRequestModel.cardcoupon_id > 0 && (canUseCardCoupon == true))//有优惠券且已经成功使用
                {
                    MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, CurrentUserInfo.UserID);
                    myCardCoupon.UseDate = DateTime.Now;
                    myCardCoupon.Status = 1;
                    if (!bllCardCoupon.Update(myCardCoupon, tran))
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "更新优惠券状态失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                }
                //优惠券 
                #endregion

                #region 积分抵扣
                //积分扣除
                if (orderRequestModel.use_score > 0)
                {
                    CurrentUserInfo.TotalScore -= orderRequestModel.use_score;
                    if (bllMall.Update(CurrentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", CurrentUserInfo.AutoID), tran) < 0)
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "更新用户积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                    //积分记录
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -orderRequestModel.use_score;
                    scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderSubmit";
                    scoreRecord.UserID = CurrentUserInfo.UserID;
                    scoreRecord.AddNote = "微商城-开团使用积分";
                    scoreRecord.WebSiteOwner = CurrentUserInfo.WebsiteOwner;
                    if (!bllMall.Add(scoreRecord, tran))
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "插入积分记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                    #region 更新宏巍积分
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (!hongWareClient.UpdateMemberScore(hongWeiWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, -orderRequestModel.use_score))
                        {
                            tran.Rollback();
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = "更新宏巍积分失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                            return;
                        }

                    }
                    #endregion



                }

                //积分扣除 
                #endregion

                #region 余额抵扣

                if (orderRequestModel.use_amount > 0 && bllMall.IsEnableAccountAmountPay())
                {
                    CurrentUserInfo.AccountAmount -= orderRequestModel.use_amount;
                    if (bllMall.Update(CurrentUserInfo, string.Format(" AccountAmount={0}", CurrentUserInfo.AccountAmount), string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "更新用户余额失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }


                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -(double)orderRequestModel.use_amount;
                    scoreRecord.TotalScore = (double)CurrentUserInfo.AccountAmount;
                    scoreRecord.UserID = CurrentUserInfo.UserID;
                    scoreRecord.AddNote = "拼团-开团使用余额";
                    scoreRecord.RelationID = orderInfo.OrderID;
                    scoreRecord.WebSiteOwner = bllUser.WebsiteOwner;
                    scoreRecord.ScoreType = "AccountAmount";
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "插入余额记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                    UserCreditAcountDetails record = new UserCreditAcountDetails();
                    record.WebsiteOwner = bllUser.WebsiteOwner;
                    record.Operator = CurrentUserInfo.UserID;
                    record.UserID = CurrentUserInfo.UserID;
                    record.CreditAcount = -orderRequestModel.use_amount;
                    record.SysType = "AccountAmount";
                    record.AddTime = DateTime.Now;
                    record.AddNote = "账户余额变动-" + orderRequestModel.use_amount;
                    if (!bllMall.Add(record))
                    {
                        tran.Rollback();
                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "插入余额记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }

                    #region 更新宏巍余额
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (!hongWareClient.UpdateMemberBlance(hongWeiWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, -(float)orderRequestModel.use_amount))
                        {
                            tran.Rollback();
                            apiResp.code = (int)APIErrCode.OperateFail;
                            apiResp.msg = "更新宏巍余额失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }

                    }
                    #endregion


                }


                #endregion

                #region 插入订单详情表及更新库存
                foreach (var item in detailList)
                {
                    ProductSku productSku = bllMall.GetProductSku((int)(item.SkuId));
                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();

                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "提交失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }
                    //更新 SKU库存 
                    System.Text.StringBuilder sbUpdateStock = new StringBuilder();//更新库存sql
                    sbUpdateStock.AppendFormat(" Update ZCJ_ProductSku Set Stock-={0} ", item.TotalCount);

                    sbUpdateStock.AppendFormat(" Where SkuId={0} And Stock>0 ", productSku.SkuId);

                    if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbUpdateStock.ToString(), tran) <= 0)
                    {
                        tran.Rollback();

                        apiResp.code = (int)APIErrCode.OperateFail;
                        apiResp.msg = "提交订单失败,库存不足";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }




                }
                #endregion

               

                bllMall.DeleteShoppingCart(CurrentUserInfo.UserID, orderRequestModel.skus);
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

                    string title = "订单已成功提交";
                    if (orderInfo.TotalAmount > 0)
                    {
                        title += ",请尽快付款";
                    }
                    bllWeiXin.SendTemplateMessageNotifyComm(CurrentUserInfo, title, string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone));

                    

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
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "提交订单失败";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                order_id = orderInfo.OrderID
            };

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        /// <summary>
        /// 订单模型
        /// </summary>
        private class OrderModel
        {
            /// <summary>
            /// 团购规则ID
            /// </summary>
            public string rule_id { get; set; }
            /// <summary>
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }
            /// <summary>
            /// 支付类型 WEIXIN 或 ALIPAY
            /// </summary>
            public string pay_type { get; set; }
            ///// <summary>
            ///// 物流方式 暂未用到
            ///// </summary>
            //public string shipping_type { get; set; }
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
            ///// <summary>
            ///// 快递公司名称
            ///// </summary>
            //public string express_company { get; set; }
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
            /// 使用余额
            /// </summary>
            public decimal use_amount { get; set; }
            /// <summary>
            /// 推广人ID
            /// </summary>
            public string sale_id { get; set; }


        }

    }
}
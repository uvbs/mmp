using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model.API.Mall;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Enums;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Gift.Order
{
    /// <summary>
    /// 提交礼品订单
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
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
                    resp.errcode = 1;
                    resp.errmsg = "您尚未绑定宏巍账号,请先绑定";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
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
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = CurrentUserInfo.UserID;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.Transport_Fee = 0;
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            if (bllMall.WebsiteOwner != "mixblu")
            {
                orderInfo.OutOrderId = orderInfo.OrderID;
            }
            orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();
            orderInfo.UseScore = orderRequestModel.use_score;
            orderInfo.UseAmount = orderRequestModel.use_amount; 
            orderInfo.Status = "待付款";
            orderInfo.OrderType = 1;
            orderInfo.LastUpdateTime = DateTime.Now;
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
            if (orderRequestModel.skus == null)
            {
                resp.errcode = 1;
                resp.errmsg = "参数skus 不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (orderRequestModel.skus.Count > 1)
            {
                resp.errcode = 1;
                resp.errmsg = "只能购买一个商品";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            //相关检查 
            #endregion

            #region 商品检查 订单详情生成
            ///订单详情
            List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情
            orderRequestModel.skus = orderRequestModel.skus.Distinct().ToList();

            #region 购买的商品
            List<WXMallProductInfo> productList = new List<WXMallProductInfo>();
            foreach (var sku in orderRequestModel.skus)
            {
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);

                if (productSku == null) continue;
                if (productList.Count(p => p.PID == productSku.ProductId.ToString()) > 0) continue;

                WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                productList.Add(productInfo);

            }
            //productList = productList.Distinct().ToList();
            #endregion


            #region 检查优惠券是否可用
            if (orderRequestModel.cardcoupon_id > 0)
            {
                var mycardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, CurrentUserInfo.UserID);
                if (mycardCoupon == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
                if (cardCoupon == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                #region 需要购买指定商品
                if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
                {

                    if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
                    {
                        var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
                        resp.errcode = 1;
                        resp.errmsg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }


                }
                #endregion

                #region 需要购买指定标签商品
                if (!string.IsNullOrEmpty(cardCoupon.Ex8))
                {
                    if (productList.Where(p => p.Tags == "" || p.Tags == null).Count() == productList.Count)//全部商品都没有标签
                    {
                        resp.errcode = 1;
                        resp.errmsg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
                        resp.errcode = 1;
                        resp.errmsg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                }
                #endregion


            }
            #endregion

            var needUseScore = 0;//必须使用的积分
            decimal needUseCash = 0;//必须使用的现金
            foreach (var sku in orderRequestModel.skus)
            {
                //先检查库存
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                if (productSku == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "SKU不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                //WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                WXMallProductInfo productInfo = productList.Single(p => p.PID == productSku.ProductId.ToString());
                if (productInfo.IsOnSale == "0")
                {
                    resp.errcode = 1;
                    resp.errmsg = string.Format("{0}已下架", productInfo.PName);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (bllMall.GetSkuCount(productSku) < sku.count)
                {
                    resp.errcode = 1;
                    resp.errmsg = string.Format("{0}{1}库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.Stock);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (bllMall.IsPromotionTime(productSku))
                {
                    if (sku.count > productSku.PromotionStock)
                    {
                        resp.errcode = 1;
                        resp.errmsg = string.Format("{0}{1}特卖库存余量为{2},库存不足,请减少购买数量", productInfo.PName, bllMall.GetProductShowProperties(productSku.SkuId), productSku.PromotionStock);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }


                }
                if (productInfo.Score > 0)//必须使用的积分
                {
                    needUseScore += sku.count * productInfo.Score;
                }
                if (productInfo.IsCashPayOnly == 1)//必须使用的现金
                {
                    needUseCash += bllMall.GetSkuPrice(productSku)*sku.count;
                }
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderInfo.OrderID;
                detailModel.PID = productInfo.PID;
                detailModel.TotalCount = sku.count;
                detailModel.OrderPrice = bllMall.GetSkuPrice(productSku);
                detailModel.ProductName = productInfo.PName;
                detailModel.SkuId = productSku.SkuId;
                detailModel.SkuShowProp = bllMall.GetProductShowProperties(productSku.SkuId);
                detailList.Add(detailModel);

            }
            #endregion

            #region 纯积分购买
            if (needUseScore > 0)
            {
                if ((CurrentUserInfo.TotalScore < needUseScore) || (orderRequestModel.use_score < needUseScore))
                {

                    resp.errcode = 1;
                    resp.errmsg = string.Format("您需要使用{0}积分来兑换， 可用积分不足", needUseScore);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

            }
            #endregion
            productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用

            //物流费用
            //#region 运费计算
            //FreightModel freightModel = new FreightModel();
            //freightModel.skus = orderRequestModel.skus;
            //decimal freight = 0;//运费
            //string freightMsg = "";
            //if (!bllMall.CalcFreight(freightModel, out freight, out  freightMsg))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = freightMsg;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;

            //}
            //orderInfo.Transport_Fee = freight;
            //#endregion

            #region 优惠券计算
            decimal discountAmount = 0;//优惠金额
            bool canUseCardCoupon = false;
            string msg = "";
            if (orderRequestModel.cardcoupon_id > 0)//有优惠券
            {


                discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, CurrentUserInfo.UserID, out canUseCardCoupon, out msg);
                if (!canUseCardCoupon)
                {
                    resp.errcode = 1;
                    resp.errmsg = msg;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                //if (discountAmount > productFee + orderInfo.Transport_Fee)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "优惠券可优惠金额超过了订单总金额";
                //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                //    return;

                //}




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
                    resp.errcode = 1;
                    resp.errmsg = "积分不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }



                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                if (scoreConfig==null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "未配置积分兑换";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (scoreConfig.ExchangeScore==0||scoreConfig.ExchangeAmount==0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "未配置积分兑换";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);



            }



            //积分计算 
            #endregion

            #region 使用账户余额
            if (orderRequestModel.use_amount > 0)
            {
                if (!bllMall.IsEnableAccountAmountPay())
                {

                    resp.errcode = 1;
                    resp.errmsg = "尚未启用余额支付功能";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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

                    resp.errcode = 1;
                    resp.errmsg = "您的账户余额不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
            orderInfo.PayableAmount = orderInfo.TotalAmount;//应付金额
            orderInfo.ScoreExchangAmount = scoreExchangeAmount;//优惠券抵扣金额
            orderInfo.CardcouponDisAmount = discountAmount;//卡券抵扣金额
            if ((productFee + orderInfo.Transport_Fee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
            {
                resp.errcode = 1;
                resp.errmsg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
            if (orderInfo.TotalAmount < needUseCash)
            {
                resp.errcode = 1;
                resp.errmsg = string.Format("最少需要支付{0}元" + needUseCash);
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
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
                        resp.errcode = 1;
                        resp.errmsg = "更新优惠券状态失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
                    if (

                        bllMall.Update(CurrentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "更新用户积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    //积分记录
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -orderRequestModel.use_score;
                    scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
                    scoreRecord.ScoreType = "OrderSubmit";
                    scoreRecord.UserID = CurrentUserInfo.UserID;
                    scoreRecord.AddNote = "微商城-下单使用积分";
                    scoreRecord.WebSiteOwner = CurrentUserInfo.WebsiteOwner;
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "插入积分记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    #region 更新宏巍积分
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (!hongWareClient.UpdateMemberScore(hongWeiWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, -orderRequestModel.use_score))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "更新宏巍积分失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }

                    }
                    #endregion

                    //积分记录


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
                        resp.errcode = 1;
                        resp.errmsg = "更新用户余额失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }


                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -(double)orderRequestModel.use_amount;
                    scoreRecord.TotalScore = (double)CurrentUserInfo.AccountAmount;
                    scoreRecord.UserID = CurrentUserInfo.UserID;
                    scoreRecord.AddNote = "账户余额变动-下单使用余额";
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
                        resp.errcode = 1;
                        resp.errmsg = "插入余额记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    #region 更新宏巍余额
                    if (websiteInfo.IsUnionHongware == 1)
                    {
                        if (!hongWareClient.UpdateMemberBlance(hongWeiWareMemberInfo.member.mobile,CurrentUserInfo.WXOpenId, -(float)orderRequestModel.use_amount))
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "更新宏巍余额失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }

                    }
                    #endregion



                }


                #endregion

                #region 插入订单详情页及更新库存
                foreach (var item in detailList)
                {
                    ProductSku productSku = bllMall.GetProductSku((int)(item.SkuId));
                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "提交失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    //productSku.Stock -= item.TotalCount;
                    //if (bllMall.IsPromotionTime(productSku))
                    //{
                    //    productSku.PromotionStock -= item.TotalCount;
                    //}
                    //if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("update ZCJ_ProductSku set Stock={0},PromotionStock={1} where SkuId={2}", productSku.Stock, productSku.PromotionStock, productSku.SkuId), tran) <= 0)
                    //{
                    //    tran.Rollback();
                    //    resp.errcode = 1;
                    //    resp.errmsg = "更新SKU失败";
                    //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //    return;

                    //}
                    //更新 SKU库存 
                    System.Text.StringBuilder sbUpdateStock = new StringBuilder();//更新库存sql
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
                    if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbUpdateStock.ToString(), tran) <= 0)
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "提交订单失败,库存不足";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                }
                #endregion
                tran.Commit();//提交订单事务

                #region 宏巍通知
                if (websiteInfo.IsUnionHongware == 1)
                {
                    hongWareClient.OrderNotice(CurrentUserInfo.WXOpenId, orderInfo.OrderID);

                }
                #endregion

            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                resp.errcode = 1;
                resp.errmsg = ex.ToString();
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                errmsg = "ok",
                order_id = orderInfo.OrderID
            }));


        }

        /// <summary>
        /// 订单模型
        /// </summary>
        private class OrderRequestModel
        {
            /// <summary>
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }

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
            /// 支付方式
            /// WEIXIN 微信
            /// ALIPAY 支付宝
            /// </summary>
            public string pay_type { get; set; }



        }


    }
}
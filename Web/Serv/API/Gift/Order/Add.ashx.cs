using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model.API.Mall;
using ZentCloud.BLLJIMP.Model;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv.API.Gift.Order
{
    /// <summary>
    /// 没有使用
    /// </summary>
    public class Add : IHttpHandler, IReadOnlySessionState
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
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        /// <summary>
        /// 默认响应模型
        /// </summary>
        protected DefaultResponse resp = new DefaultResponse();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentUserInfo = bllUser.GetCurrentUserInfo();
            string data = context.Request["data"];
            decimal productFee = 0;//商品总价格 不包含邮费
            OrderRequestModel orderRequestModel=new OrderRequestModel();//订单模型
            try
            {
                orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderRequestModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

            }
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = currentUserInfo.UserID;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.Transport_Fee = 0;
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();
            orderInfo.UseScore = orderRequestModel.use_score;
            orderInfo.Status = "待付款";
            orderInfo.OrderType = 1;
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
            //相关检查 
            #endregion


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
                    resp.errcode = 1;
                    resp.errmsg = "SKU不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                      return;
                }

                WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
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

            productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;//商品费用

            //物流费用
            #region 运费计算
            FreightModel freightModel = new FreightModel();
            freightModel.skus = orderRequestModel.skus;
            decimal freight = 0;//运费
            string freightMsg = "";
            if (!bllMall.CalcFreight(freightModel, out freight, out  freightMsg))
            {
                resp.errcode = 1;
                resp.errmsg = freightMsg;
                context.Response.Write( ZentCloud.Common.JSONHelper.ObjectToJson(resp));
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
                discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, currentUserInfo.UserID, out canUseCardCoupon, out msg);
                if (!canUseCardCoupon)
                {
                    resp.errcode = 1;
                    resp.errmsg = msg;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (discountAmount > productFee + orderInfo.Transport_Fee)
                {
                    resp.errcode = 1;
                    resp.errmsg = "优惠券可优惠金额超过了订单总金额";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;

                }




            }
            //优惠券计算 
            #endregion


            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (orderRequestModel.use_score > 0)
            {

                if (currentUserInfo.TotalScore < orderRequestModel.use_score)
                {
                    resp.errcode = 1;
                    resp.errmsg = "积分不足";
                   context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);



            }
            #region 驿氪积分同步






            #endregion


            //积分计算 
            #endregion


            //合计计算
            orderInfo.Product_Fee = productFee;
            orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
            orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
            orderInfo.PayableAmount = orderInfo.TotalAmount - freight;//应付金额
            if ((productFee + orderInfo.Transport_Fee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
            {
                resp.errcode = 1;
                resp.errmsg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
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
                    MyCardCoupons myCardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, currentUserInfo.UserID);
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
                    currentUserInfo.TotalScore -= orderRequestModel.use_score;
                    if (
                        
                        bllMall.Update(currentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", currentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "更新用户积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    //积分记录
                    WXMallScoreRecord scoreRecord = new WXMallScoreRecord();
                    scoreRecord.InsertDate = DateTime.Now;
                    scoreRecord.OrderID = orderInfo.OrderID;
                    scoreRecord.Score = orderRequestModel.use_score;
                    scoreRecord.Type = 1;
                    scoreRecord.UserId = currentUserInfo.UserID;
                    scoreRecord.WebsiteOwner = bllMall.WebsiteOwner;
                    scoreRecord.Remark = "积分抵扣";
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "插入积分记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }

                    //积分记录


                }

                //积分扣除 
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
                    //更新 SKU库存 
                    productSku.Stock -= item.TotalCount;
                    if (bllMall.IsPromotionTime(productSku))
                    {
                        productSku.PromotionStock -= item.TotalCount;
                    }
                    if (!bllMall.Update(productSku, tran))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "更新SKU失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }



                }
                #endregion
                tran.Commit();//提交订单事务


            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                resp.errcode = 1;
                resp.errmsg = ex.Message;
                context.Response.Write( ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            context.Response.Write( ZentCloud.Common.JSONHelper.ObjectToJson(new
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
            /// 支付方式
            /// WEIXIN 微信
            /// ALIPAY 支付宝
            /// </summary>
            public string pay_type { get; set; }

        }

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
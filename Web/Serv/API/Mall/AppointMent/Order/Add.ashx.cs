using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.AppointMent.Order
{
    /// <summary>
    /// 预约提交订单
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            decimal productFee = 0;//商品总价格
            OrderModel orderRequestModel;//订单模型
            try
            {
                orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderModel>(data);
            }
            catch (Exception ex)
            {
                apiResp.code = 1;
                apiResp.msg = "JSON格式错误,请检查.错误信息:" + ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            #region 检查是否可以下单
            if (string.IsNullOrEmpty(orderRequestModel.select_time_type))
            {
                apiResp.code = 1;
                apiResp.msg = "请选择一种时间预约方式";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            int totalHours = 0;//预约小时数
            if (orderRequestModel.select_time_type == "0")//直接选择开始时间结束时间方式
            {
                if (string.IsNullOrEmpty(orderRequestModel.start_time) || string.IsNullOrEmpty(orderRequestModel.stop_time))
                {
                    apiResp.code = 1;
                    apiResp.msg = "请选择开始时间与结束时间";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;


                }
                if (DateTime.Parse(orderRequestModel.start_time)<=DateTime.Now)
                {
                    apiResp.code = 1;
                    apiResp.msg = "开始时间需要晚于当前时间";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                if (DateTime.Parse(orderRequestModel.stop_time) <= DateTime.Parse(orderRequestModel.start_time))
                {
                    apiResp.code = 1;
                    apiResp.msg = "结束时间需要晚于开始时间";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

                totalHours = (int)(DateTime.Parse(orderRequestModel.stop_time) - DateTime.Parse(orderRequestModel.start_time)).TotalHours;

            }
            else if (orderRequestModel.select_time_type == "1")//选择某一天的多个时间段的方式
            {
                if (string.IsNullOrEmpty(orderRequestModel.date) || string.IsNullOrEmpty(orderRequestModel.date_time_ranges))
                {
                    apiResp.code = 1;
                    apiResp.msg = "请选择预约时间段";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;


                }
                if (DateTime.Parse(orderRequestModel.date) <= DateTime.Now)
                {
                    apiResp.code = 1;
                    apiResp.msg = "开始时间需要晚于当前时间";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }
                totalHours = orderRequestModel.date_time_ranges.Split(';').Count();

            }


            if (!Check(orderRequestModel))
            {
                apiResp.code = 1;
                apiResp.msg = "您所选的时间段已经被占用,请选择别的时间段";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            #endregion



            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            orderInfo.Consignee = orderRequestModel.receiver_name;
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = CurrentUserInfo.UserID;
            orderInfo.Phone = orderRequestModel.receiver_phone;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.Transport_Fee = 0;
            orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();
            orderInfo.UseScore = orderRequestModel.use_score;
            orderInfo.Status = "待付款";
            orderInfo.OrderType = 3;
            orderInfo.Ex3 = orderRequestModel.select_time_type;
            orderInfo.Ex4 = orderRequestModel.start_time;
            orderInfo.Ex5 = orderRequestModel.stop_time;
            orderInfo.Ex6 = orderRequestModel.date;
            orderInfo.Ex7 = orderRequestModel.date_time_ranges;
            orderInfo.LastUpdateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(orderRequestModel.sale_id))//分销ID
            {
                long saleId = 0;
                if (long.TryParse(orderRequestModel.sale_id, out saleId))
                {
                    orderInfo.SellerId = saleId;
                }
            }
            if (orderRequestModel.pay_type == "WEIXIN")//微信支付
            {
                orderInfo.PaymentType = 2;
            }
            else if (orderRequestModel.pay_type == "ALIPAY")//支付宝支付
            {
                orderInfo.PaymentType = 1;
            }

            #region 格式检查
            if (string.IsNullOrEmpty(orderInfo.Consignee))
            {
                apiResp.code = 1;
                apiResp.msg = "收货人姓名不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(orderRequestModel.receiver_phone))
            {
                apiResp.code = 1;
                apiResp.msg = "联系手机号不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }


            if (orderRequestModel.skus == null)
            {
                apiResp.code = 1;
                apiResp.msg = "skus 参数不能为空";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            //相关检查 
            #endregion

            #region 商品检查 订单详情生成
            ///订单详情
            List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情
            #region 购买的商品
            List<WXMallProductInfo> productList = new List<WXMallProductInfo>();
            foreach (var sku in orderRequestModel.skus)
            {
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                productList.Add(productInfo);

            }
            productList = productList.Distinct().ToList();
            #endregion
            #region 检查优惠券是否可用
            if (orderRequestModel.cardcoupon_id > 0)
            {
                var mycardCoupon = bllCardCoupon.GetMyCardCoupon(orderRequestModel.cardcoupon_id, CurrentUserInfo.UserID);
                if (mycardCoupon == null)
                {
                    apiResp.code = 1;
                    apiResp.msg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }
                var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
                if (cardCoupon == null)
                {
                    apiResp.code = 1;
                    apiResp.msg = "无效的优惠券";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }
                #region 需要购买指定商品
                if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
                {

                    if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
                    {
                        var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
                        apiResp.code = 1;
                        apiResp.msg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
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
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;

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

                    }
                    if (!checkResult)
                    {
                        apiResp.code = 1;
                        apiResp.msg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                    }

                }
                #endregion


            }
            #endregion

            foreach (var sku in orderRequestModel.skus)
            {
                //先检查库存
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                if (productSku == null)
                {
                    apiResp.code = 1;
                    apiResp.msg = "SKU不存在";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }
                WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                if (productInfo.IsOnSale == "0")
                {
                    apiResp.code = 1;
                    apiResp.msg = string.Format("{0}已下架", productInfo.PName);
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.OrderID = orderInfo.OrderID;
                detailModel.PID = productInfo.PID;
                detailModel.TotalCount = sku.count;
                detailModel.OrderPrice = bllMall.GetSkuPrice(productSku) * totalHours;
                detailModel.ProductName = productInfo.PName;
                detailModel.SkuId = productSku.SkuId;
                detailList.Add(detailModel);


            }
            #endregion
            productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;   //商品费用
            orderInfo.Transport_Fee = 0;
            orderInfo.Product_Fee = productFee;
            orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
            #region 优惠券计算
            decimal discountAmount = 0;//优惠金额
            bool canUseCardCoupon = false;
            string msg = "";
            if (orderRequestModel.cardcoupon_id > 0)//有优惠券
            {
                discountAmount = bllMall.CalcDiscountAmount(orderRequestModel.cardcoupon_id.ToString(), data, CurrentUserInfo.UserID, out canUseCardCoupon, out msg);
                if (!canUseCardCoupon)
                {
                    apiResp.code = 1;
                    apiResp.msg = msg;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }
                if (discountAmount > productFee)
                {
                    apiResp.code = 1;
                    apiResp.msg = "优惠券可优惠金额超过了订单总金额";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;

                }
                orderInfo.CardcouponDisAmount = discountAmount;




            }
            #endregion

            #region 积分计算
            decimal scoreExchangeAmount = 0;///积分抵扣的金额
            //积分计算
            if (orderRequestModel.use_score > 0)
            {
                if (CurrentUserInfo.TotalScore < orderRequestModel.use_score)
                {
                    apiResp.code = 1;
                    apiResp.msg = "积分不足";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                }
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);
            }



            //积分计算 
            #endregion

            #region 合计计算

            orderInfo.TotalAmount -= discountAmount;//优惠券优惠金额
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
            orderInfo.PayableAmount = orderInfo.TotalAmount;//应付金额
            if ((productFee - discountAmount - scoreExchangeAmount) < orderInfo.TotalAmount)
            {
                apiResp.code = 1;
                apiResp.msg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            #endregion
            if (orderInfo.TotalAmount < 0)
            {
                apiResp.code = 1;
                apiResp.msg = "付款金额不能小于0";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (orderInfo.TotalAmount == 0)
            {
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "待发货";
            }
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (!this.bllMall.Add(orderInfo, tran))
                {
                    tran.Rollback();
                    apiResp.code = 1;
                    apiResp.msg = "提交失败";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
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
                        apiResp.code = 1;
                        apiResp.msg = "更新优惠券状态失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                    }

                }
                //优惠券 
                #endregion

                #region 积分抵扣
                //积分扣除
                if (orderRequestModel.use_score > 0)
                {
                    CurrentUserInfo.TotalScore -= orderRequestModel.use_score;
                    if (bllMall.Update(CurrentUserInfo, string.Format(" TotalScore-={0}", orderRequestModel.use_score), string.Format(" AutoID={0}", CurrentUserInfo.AutoID)) < 0)
                    {
                        tran.Rollback();
                        apiResp.code = 1;
                        apiResp.msg = "更新用户积分失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                    }
                    //积分记录
                    UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                    scoreRecord.AddTime = DateTime.Now;
                    scoreRecord.Score = -orderRequestModel.use_score;
                    scoreRecord.ScoreType = "OrderSubmit";
                    scoreRecord.UserID = CurrentUserInfo.UserID;
                    scoreRecord.AddNote = "预约使用积分";
                    if (!bllMall.Add(scoreRecord))
                    {
                        tran.Rollback();
                        apiResp.code = 1;
                        apiResp.msg = "插入积分记录失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }
                    //积分记录
                }

                //积分扣除 
                #endregion

                #region 插入订单详情页
                foreach (var item in detailList)
                {
                    ProductSku productSku = bllMall.GetProductSku((int)(item.SkuId));
                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    if (!this.bllMall.Add(item, tran))
                    {
                        tran.Rollback();
                        apiResp.code = 1;
                        apiResp.msg = "提交失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp)); return;
                    }

                }
                #endregion

                tran.Commit();//提交订单事务

            }
            catch (Exception ex)
            {
                //回滚事物
                tran.Rollback();
                apiResp.code = 1;
                apiResp.msg = "提交订单失败,内部错误";
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
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }
            /// <summary>
            /// 支付类型 WEIXIN 或 ALIPAY
            /// </summary>
            public string pay_type { get; set; }

            /// <summary>
            /// 收货人姓名
            /// </summary>
            public string receiver_name { get; set; }

            /// <summary>
            /// 收货人手机
            /// </summary>
            public string receiver_phone { get; set; }

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
            /// 预约时间类型 
            /// 0 直接选择开始时间和结束时间
            /// 1 选择某一天的多个时间段
            /// </summary>
            public string select_time_type { get; set; }

            /// <summary>
            /// 预约开始时间（选开始结束时间方式）（二选一）
            /// </summary>
            public string start_time { get; set; }
            /// <summary>
            /// 结束时间（选开始结束时间方式）(二选一)
            /// </summary>
            public string stop_time { get; set; }

            /// <summary>
            /// 预约日期(选时间段方式)（二选一）
            /// </summary>
            public string date { get; set; }
            /// <summary>
            ///时间段范围 (选时间段方式)（二选一）
            /// </summary>
            public string date_time_ranges { get; set; }

            /// <summary>
            /// 商品SKU列表
            /// </summary>
            public List<SkuModel> skus { get; set; }



        }
        /// <summary>
        /// SKU 模型
        /// </summary>
        public class SkuModel
        {
            /// <summary>
            /// SKU  编号
            /// </summary>
            public int sku_id { get; set; }
            /// <summary>
            /// 数量
            /// </summary>
            public int count { get; set; }




        }

        /// <summary>
        /// 检查预约是否已经被占用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool Check(OrderModel model)
        {
            if (model.select_time_type == "0")//直接用选择的时间段的方式
            {
                //orderInfo.Ex3 = orderRequestModel.select_time_type;
                //orderInfo.Ex4 = orderRequestModel.start_time;
                //orderInfo.Ex5 = orderRequestModel.stop_time;
                //orderInfo.Ex6 = orderRequestModel.date;
                //orderInfo.Ex7 = orderRequestModel.date_time_ranges;

                //2016-04-01 10:00  2016-04-01 12:00
                var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}'   And Status!='已取消' And OrderId in (select OrderId from ZCJ_WXMallOrderDetailsInfo where SkuId={1})", bllMall.WebsiteOwner, model.skus[0].sku_id));
                if (orderList.Count > 0)
                {
                    foreach (var order in orderList)
                    {

                        if ((!(string.IsNullOrEmpty(order.Ex4)))&&(!string.IsNullOrEmpty(order.Ex5)))
                        {
                            if (((DateTime.Parse(model.stop_time)) < DateTime.Parse(order.Ex4)) || (DateTime.Parse(model.stop_time) > DateTime.Parse(order.Ex5)))
                            {
                                //无交集，即不冲突
                            }
                            else
                            {
                                //有交集，即冲突
                                return false;
                            }

                        }


                    }


                }


            }
            if (model.select_time_type == "1")//选择日期，再选择时间段
            {
                var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}'   And Status!='已取消' And OrderId in (select OrderId from ZCJ_WXMallOrderDetailsInfo where SkuId={1}) And Ex6='{2}'", bllMall.WebsiteOwner, model.skus[0].sku_id,model.date));
                if (orderList.Count > 0)
                {
                    foreach (var order in orderList)
                    {
                        foreach (var item in model.date_time_ranges.Split(';'))
                        {
                            if (order.Ex7.Contains(item))
                            {
                                return false;
                            }


                        }


                    }


                }


            }


            return true;

        }



    }
}
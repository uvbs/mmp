using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Booking.Order
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLMall bllMall = new BLLMall();
        BLLUser bllUser = new BLLUser();
        BLLWeixin bllWeiXin = new BLLWeixin();
        BllScore bllScore = new BllScore();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            OrderModel orderRequestModel;//订单模型
            try
            {
                orderRequestModel = JSONHelper.JsonToModel<OrderModel>(data);
            }
            catch (Exception ex)
            {
                apiResp.msg = "提交格式错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }

            WXMallProductInfo productInfo = bllMall.GetProduct(orderRequestModel.product_id.ToString());
            if (productInfo == null)
            {
                apiResp.msg = "记录没有找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
            orderInfo.ArticleCategoryType = productInfo.ArticleCategoryType;
            orderInfo.InsertDate = DateTime.Now;
            orderInfo.OrderUserID = CurrentUserInfo.UserID;
            orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
            orderInfo.OrderMemo = orderRequestModel.buyer_memo;
            orderInfo.UseAmount = orderRequestModel.use_amount;
            if (orderRequestModel.receiver_id == 0)
            {
                orderInfo.Consignee = CurrentUserInfo.TrueName;
                orderInfo.Phone = CurrentUserInfo.Phone;
            }
            else
            {
                WXConsigneeAddress nUserAddress = bllMall.GetByKey<WXConsigneeAddress>("AutoID", orderRequestModel.receiver_id.ToString());
                orderInfo.Consignee = nUserAddress.ConsigneeName;
                orderInfo.Phone = nUserAddress.Phone;
                orderInfo.Address = nUserAddress.Address;
                orderInfo.ZipCode = nUserAddress.ZipCode;
                orderInfo.ReceiverProvince = nUserAddress.Province;
                orderInfo.ReceiverProvinceCode = nUserAddress.ProvinceCode;
                orderInfo.ReceiverCity = nUserAddress.City;
                orderInfo.ReceiverCityCode = nUserAddress.CityCode;
                orderInfo.ReceiverDist = nUserAddress.Dist;
                orderInfo.ReceiverDistCode = nUserAddress.DistCode;
            }

            orderInfo.Transport_Fee = 0;
            orderInfo.Status = "待付款";
            if (orderRequestModel.pay_type == "WEIXIN")//微信支付
            {
                orderInfo.PaymentType = 2;
            }
            else if (orderRequestModel.pay_type == "ALIPAY")//支付宝支付
            {
                orderInfo.PaymentType = 1;
            }
            if (orderRequestModel.skus == null || orderRequestModel.skus.Count == 0)
            {
                apiResp.msg = "Skus不能为空";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }

            #region 商品检查 订单详情生成
            //订单详情
            List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//主商品订单明细
            List<WXMallOrderDetailsInfo> detailAddedList = new List<WXMallOrderDetailsInfo>();//增值服务订单明细
            orderRequestModel.skus = orderRequestModel.skus.Distinct().ToList();
            #region 购买的商品
            foreach (var sku in orderRequestModel.skus)
            {
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                detailModel.TotalCount = sku.count;
                detailModel.OrderPrice = bllMall.GetSkuPrice(productSku);
                detailModel.SkuId = productSku.SkuId;
                detailModel.ArticleCategoryType = productSku.ArticleCategoryType;
                if (productSku.ArticleCategoryType.Contains("Added"))
                {
                    WXMallProductInfo rproductInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    detailModel.PID = rproductInfo.PID;
                    detailModel.ProductName = rproductInfo.PName;
                    detailModel.Unit = rproductInfo.Unit;
                    detailAddedList.Add(detailModel);
                }
                else
                {
                    detailModel.PID = productInfo.PID;
                    detailModel.ProductName = productInfo.PName;
                    detailModel.StartDate = sku.start_date;
                    detailModel.EndDate = sku.end_date;
                    detailModel.Unit = productInfo.Unit;
                    detailList.Add(detailModel);
                }
            }
            #endregion
            if (detailList.Count ==0)
            {
                apiResp.msg = "请选择预约时间";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            //已有订单详情
            List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, productInfo.PID, productInfo.ArticleCategoryType, detailList.Min(p => p.StartDate), detailList.Max(p => p.EndDate));
            List<string> hasOrderID_List = new List<string>();
            string hasOrderIDs = "";
            int maxCount = productInfo.Stock;
            foreach (var item in detailList)
            { 
                List<WXMallOrderDetailsInfo> hasOrderDetailList = oDetailList.Where(p => !((item.StartDate >= p.EndDate && item.EndDate > p.EndDate)|| (item.StartDate < p.StartDate && item.EndDate <= p.StartDate))).ToList();
                if (hasOrderDetailList.Count >= maxCount)
                {
                    hasOrderID_List.AddRange(hasOrderDetailList.Select(p=>p.OrderID).Distinct());
                }
            }
            if (hasOrderID_List.Count > 0)
            {
                hasOrderID_List = hasOrderID_List.Distinct().ToList();
                hasOrderIDs = MyStringHelper.ListToStr(hasOrderID_List, "'", ",");
                int tempCount = 0;
                List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "预约成功", null, null, null,
                        null, null, null, null, null, null, null, orderInfo.ArticleCategoryType, hasOrderIDs);
                if (tempCount >= maxCount)
                {
                    apiResp.msg = "所选时间已有成功的预约";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            //增值服务合并到主订单明细列表
            detailList.AddRange(detailAddedList);
            //合计计算
            orderInfo.Product_Fee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;

            #region 积分计算

            decimal scoreExchangeAmount = 0;//积分抵扣的金额                       
            //积分计算
            if (orderRequestModel.use_score > 0 && orderInfo.Product_Fee>0)
            {
                orderInfo.UseScore = orderRequestModel.use_score;    
                if (CurrentUserInfo.TotalScore < orderRequestModel.use_score)
                {
                    apiResp.msg = "积分不足";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);
            }
            //积分计算 
            #endregion

            #region 使用账户余额
            if (orderRequestModel.use_amount > 0)
            {
                if (!bllMall.IsEnableAccountAmountPay())
                {
                    apiResp.msg = "未开启余额支付";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
                if (CurrentUserInfo.AccountAmount < orderRequestModel.use_amount)
                {

                    apiResp.msg = "您的账户余额不足";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            #endregion

            orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
            orderInfo.TotalAmount -= scoreExchangeAmount;//积分优惠金额
            orderInfo.TotalAmount -= orderRequestModel.use_amount;//余额抵扣金额
            orderInfo.PayableAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;//应付金额
            if ((orderInfo.Product_Fee + orderInfo.Transport_Fee - scoreExchangeAmount) < orderInfo.TotalAmount)
            {
                apiResp.msg = "积分兑换金额不能大于订单总金额,请减少积分兑换";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;

            }

            if (orderInfo.TotalAmount < 0)
            {
                orderInfo.TotalAmount = 0;
            }
            if (orderInfo.TotalAmount == 0 && orderInfo.UseScore==0)
            {
                orderInfo.Status = "待审核";
            }
            else if (orderInfo.TotalAmount == 0 && (orderInfo.UseAmount>0 || orderInfo.UseScore > 0))
            {
                orderInfo.PaymentStatus = 1;
                orderInfo.PayTime = DateTime.Now;
                orderInfo.Status = "预约成功";
            }

            #endregion

            //生成订单ID
            orderInfo.OrderID = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
            BLLTransaction tran = new BLLTransaction();
            if (!this.bllMall.Add(orderInfo, tran))
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            foreach (var item in detailList)
            {
                item.OrderID = orderInfo.OrderID;
                if (!this.bllMall.Add(item, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "提交失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }

            #region 积分抵扣
            //积分扣除
            if (orderRequestModel.use_score > 0)
            {
                CurrentUserInfo.TotalScore -= orderRequestModel.use_score;
                if (bllMall.Update(CurrentUserInfo, 
                    string.Format(" TotalScore-={0}", orderRequestModel.use_score), 
                    string.Format(" AutoID={0}", CurrentUserInfo.AutoID)
                    ,tran) < 0)
                {
                    tran.Rollback();
                    apiResp.msg = "更新用户积分失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }

                //积分记录
                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.AddTime = DateTime.Now;
                scoreRecord.Score = -orderRequestModel.use_score;
                scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
                scoreRecord.ScoreType = "OrderSubmit";
                scoreRecord.UserID = CurrentUserInfo.UserID;
                scoreRecord.AddNote = "预约-下单使用积分";
                scoreRecord.RelationID = orderInfo.OrderID;
                scoreRecord.WebSiteOwner = CurrentUserInfo.WebsiteOwner;
                if (!bllMall.Add(scoreRecord))
                {
                    tran.Rollback();
                    apiResp.msg = "插入积分记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
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
                    apiResp.msg = "更新用户余额失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }


                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                scoreRecord.AddTime = DateTime.Now;
                scoreRecord.Score = -(double)orderRequestModel.use_amount;
                scoreRecord.TotalScore = (double)CurrentUserInfo.AccountAmount;
                scoreRecord.UserID = CurrentUserInfo.UserID;
                scoreRecord.AddNote = "账户余额变动-下单使用余额";
                scoreRecord.RelationID = orderInfo.OrderID;
                scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
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
                record.WebsiteOwner = bllMall.WebsiteOwner;
                record.Operator = CurrentUserInfo.UserID;
                record.UserID = CurrentUserInfo.UserID;
                record.CreditAcount = -orderRequestModel.use_amount;
                record.SysType = "AccountAmount";
                record.AddTime = DateTime.Now;
                record.AddNote = "账户余额变动-" + orderRequestModel.use_amount;
                if (!bllMall.Add(record))
                {
                    tran.Rollback();

                    apiResp.msg = "插入余额记录失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }



            }


            #endregion
            tran.Commit();//提交订单事务

            if(orderInfo.Status=="预约成功"){
                int tempCount = 0;
                if (string.IsNullOrWhiteSpace(hasOrderIDs)) hasOrderIDs = "'0'";
                List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "预约成功", null, null, null,
                        null, null, null, null, null, null, null, orderInfo.ArticleCategoryType, hasOrderIDs);
                tempCount = tempCount + 1; //加上当前订单的数量
                if (tempCount >= maxCount)
                {
                    tempList = bllMall.GetColOrderListInStatus("'待付款','待审核'", hasOrderIDs, "OrderID,OrderUserID,UseScore",bllMall.WebsiteOwner);
                    if (tempList.Count > 0)
                    {
                        string stopOrderIds = MyStringHelper.ListToStr(tempList.Select(p => p.OrderID).ToList(), "'", ",");
                        tempList = tempList.Where(p => p.UseScore > 0).ToList();
                        foreach (var item in tempList)
                        {
                            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID,bllMall.WebsiteOwner);//下单用户信息
                            if (orderUserInfo == null) continue;
                            orderUserInfo.TotalScore += item.UseScore;
                            if (bllMall.Update(orderUserInfo, string.Format(" TotalScore+={0}", item.UseScore),
                                string.Format(" UserID='{0}'", item.OrderUserID)) > 0)
                            {
                                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                scoreRecord.AddTime = DateTime.Now;
                                scoreRecord.Score = item.UseScore;
                                scoreRecord.TotalScore = orderUserInfo.TotalScore;
                                scoreRecord.ScoreType = "OrderCancel";
                                scoreRecord.UserID = item.OrderUserID;
                                scoreRecord.RelationID = item.OrderID;
                                scoreRecord.AddNote = "预约-订单失败返还积分";
                                scoreRecord.WebSiteOwner = item.WebsiteOwner;
                                bllMall.Add(scoreRecord);
                            }
                        }
                        bllMall.Update(new WXMallOrderInfo(),
                            string.Format("Status='{0}'", "预约失败"),
                            string.Format("OrderID In ({0}) and WebsiteOwner='{1}'", stopOrderIds,bllMall.WebsiteOwner));
                    }
                }
            }
            

            //预约通知
            bllWeiXin.SendTemplateMessageNotifyComm(CurrentUserInfo, orderInfo.Status, string.Format("预约:{2}\\n订单号:{0}\\n订单金额:{1}元", orderInfo.OrderID, orderInfo.TotalAmount, productInfo.PName));

            apiResp.result = new
            {
                order_id = orderInfo.OrderID
            };
            apiResp.msg = "提交完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            bllMall.ContextResponse(context, apiResp);
        }
        /// <summary>
        /// 订单模型
        /// </summary>
        private class OrderModel
        {
            /// <summary>
            /// 支付类型 WEIXIN 或 ALIPAY
            /// </summary>
            public string pay_type { get; set; }
            /// <summary>
            /// 商品SKU列表
            /// </summary>
            public List<SkuModel> skus { get; set; }
            /// <summary>
            /// 客户留言
            /// </summary>
            public string buyer_memo { get; set; }
            /// <summary>
            /// 客户省份
            /// </summary>
            public string receiver_province { get; set; }
            /// <summary>
            /// 客户省份代码
            /// </summary>
            public string receiver_province_code { get; set; }
            /// <summary>
            /// 客户城市名称
            /// </summary>
            public string receiver_city { get; set; }
            /// <summary>
            /// 客户城市代码
            /// </summary>
            public string receiver_city_code { get; set; }

            /// <summary>
            /// 客户区域名称
            /// </summary>
            public string receiver_dist { get; set; }
            /// <summary>
            /// 客户区域代码
            /// </summary>
            public string receiver_dist_code { get; set; }
            /// <summary>
            /// 客户地址ID
            /// </summary>
            public int receiver_id { get; set; }
            /// <summary>
            /// 商品ID
            /// </summary>
            public int product_id { get; set; }
            /// <summary>
            /// 最小开始时间
            /// </summary>
            public DateTime min_start_date { get; set; }
            /// <summary>
            /// 最大结束时间
            /// </summary>
            public DateTime max_end_date { get; set; }
            /// <summary>
            /// 使用积分
            /// </summary>
            public int use_score { get; set; }
            /// <summary>
            /// 使用账户金额
            /// </summary>
            public decimal use_amount { get; set; }

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
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime start_date { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime end_date { get; set; }

        }
    }
}
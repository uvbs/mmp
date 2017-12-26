using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall.Wifi.Order
{
    /// <summary>
    /// 提交订单
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
        /// <summary>
        /// 模块BLL
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {

            try
            {
                string data = context.Request["data"];
                decimal productFee = 0;//商品总价格 不包含邮费
                decimal deviceFee = 0;//租金总金额,不包含押金
                OrderModel orderRequestModel;//订单模型
                try
                {
                    orderRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<OrderModel>(data);
                }
                catch (Exception ex)
                {
                    resp.errcode = 1;
                    resp.errmsg = "JSON格式错误,请检查.错误信息:" + ex.Message;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (string.IsNullOrEmpty(orderRequestModel.departure_date))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请选择出发日期";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (string.IsNullOrEmpty(orderRequestModel.backhome_date))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请选择回国日期";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                WXMallOrderInfo orderInfo = new WXMallOrderInfo();//订单表
                string orderId = bllMall.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                orderInfo.OrderID = CreateOrderId(orderId);//内部订单号
                orderInfo.OutOrderId = CreateOrderId(orderId);//外部订单号
                orderInfo.Consignee = orderRequestModel.receiver_name;
                orderInfo.InsertDate = DateTime.Now;
                orderInfo.OrderUserID = CurrentUserInfo.UserID;
                orderInfo.Phone = orderRequestModel.receiver_phone;
                orderInfo.WebsiteOwner = bllMall.WebsiteOwner;
                orderInfo.Transport_Fee = 0;
                orderInfo.OrderMemo = orderRequestModel.buyer_memo;
                orderInfo.ZipCode = orderRequestModel.receiver_zip;
                orderInfo.MyCouponCardId = orderRequestModel.cardcoupon_id.ToString();
                orderInfo.UseScore = orderRequestModel.use_score;
                orderInfo.Status = "待付款";
                orderInfo.Email = orderRequestModel.email;
                orderInfo.Tel = orderRequestModel.receiver_tel;
                orderInfo.DeliveryType = orderRequestModel.delivery_type;
                orderInfo.Ex1 = bllMall.GetTime(long.Parse(orderRequestModel.departure_date)).ToString();//出国时间
                orderInfo.Ex2 = bllMall.GetTime(long.Parse(orderRequestModel.backhome_date)).ToString();//回国时间
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
                    resp.errcode = 1;
                    resp.errmsg = "收货人姓名不能为空";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                if (string.IsNullOrEmpty(orderRequestModel.receiver_phone))
                {
                    resp.errcode = 1;
                    resp.errmsg = "收货人联系手机号不能为空";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                #region 快递
                if (orderRequestModel.delivery_type == 0)//快递
                {
                    //相关检查
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
                        resp.errmsg = "街道地址不能为空";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    orderInfo.ReceiverProvince = orderRequestModel.receiver_province;
                    orderInfo.ReceiverProvinceCode = orderRequestModel.receiver_province_code;
                    orderInfo.ReceiverCity = orderRequestModel.receiver_city;
                    orderInfo.ReceiverCityCode = orderRequestModel.receiver_city_code;
                    orderInfo.ReceiverDist = orderRequestModel.receiver_dist;
                    orderInfo.ReceiverDistCode = orderRequestModel.receiver_dist_code;
                    orderInfo.Address = orderRequestModel.receiver_address;
                    orderInfo.ZipCode = orderRequestModel.receiver_zip;
                }
                #endregion

                #region 上门自提
                if (orderRequestModel.delivery_type == 1)//自提点
                {
                    if (string.IsNullOrEmpty(orderRequestModel.get_address_id))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "自提点ID为必填项,请检查";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (string.IsNullOrEmpty(orderRequestModel.get_address_name))
                    {
                        resp.errcode = 1;
                        resp.errmsg = "自提点名称为必填项,请检查";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    //if (string.IsNullOrEmpty(orderRequestModel.ex7))
                    //{
                    //    resp.errcode = 1;
                    //    resp.errmsg = "自提时间为必填项,请检查";
                    //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //    return;
                    //}
                    orderInfo.Ex3 = orderRequestModel.get_address_id;
                    orderInfo.Ex4 = orderRequestModel.get_address_name;
                    orderInfo.Ex5 = bllMall.GetGetAddress(orderRequestModel.get_address_id).GetAddressLocation;
                    //orderInfo.Ex7 = orderRequestModel.ex7;//自提时间

                }
                #endregion

                if (orderRequestModel.skus == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "skus 参数不能为空";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                DateTime startTime =DateTime.Parse(bllMall.GetTime(long.Parse(orderRequestModel.departure_date)).ToString("yyyy/MM/dd"));
                DateTime returnTime = bllMall.GetTime(long.Parse(orderRequestModel.backhome_date));
                if (returnTime <= startTime)
                {
                    resp.errcode = 1;
                    resp.errmsg = "回国日期不能晚于或等于出发日期";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                int day = (int)(returnTime - startTime).TotalDays + 1;
                if (day < 3)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "起租最低为3天";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }


                //相关检查 
                #endregion

                #region 商品检查 订单详情生成
                ///订单详情
                List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();//订单详情
                //orderRequestModel.skus = orderRequestModel.skus.Distinct().ToList();
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
                        resp.errcode = 1;
                        resp.errmsg = "无效的优惠券";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                    }
                    var cardCoupon = bllCardCoupon.GetCardCoupon(mycardCoupon.CardId);
                    if (cardCoupon == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "无效的优惠券";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                    }
                    #region 需要购买指定商品
                    if ((!string.IsNullOrEmpty(cardCoupon.Ex2)) && (cardCoupon.Ex2 != "0"))
                    {

                        if (productList.Count(p => p.PID == cardCoupon.Ex2) == 0)
                        {
                            var productInfo = bllMall.GetProduct(cardCoupon.Ex2);
                            resp.errcode = 1;
                            resp.errmsg = string.Format("此优惠券需要购买{0}时才可以使用", productInfo.PName);
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
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
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;

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
                            resp.errcode = 1;
                            resp.errmsg = string.Format("使用此优惠券需要购买标签为{0}的商品", cardCoupon.Ex8);
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                        }

                    }
                    #endregion


                }
                #endregion

                List<int> relationSkuList = new List<int>();//关联的SKU
                foreach (var sku in orderRequestModel.skus)
                {
                    //先检查库存
                    ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                    if (productSku == null)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "SKU不存在";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                    }
                    WXMallProductInfo productInfo = bllMall.GetProduct(productSku.ProductId.ToString());
                    if (productInfo.IsDelete == 1)
                    {
                        resp.errcode = 1;
                        resp.errmsg = string.Format("{0}已下架", productInfo.PName);
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
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

                    if (!string.IsNullOrEmpty(productInfo.RelationProductId))
                    {
                        WXMallProductInfo relationProductInfo = bllMall.GetProduct(productInfo.RelationProductId);
                        var relationProductSkuList = bllMall.GetProductSkuList(int.Parse(relationProductInfo.PID));
                        if (orderRequestModel.skus.Where(p => p.sku_id == relationProductSkuList[0].SkuId).Count() == 0)
                        {
                            resp.errcode = 1;
                            resp.errmsg = string.Format("{0}必须有关联商品下单", productInfo.PName);
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                        else
                        {
                            relationSkuList.Add(relationProductSkuList[0].SkuId);//关联的Sku商品不参与运费计算
                        }




                    }
                    //if (bllMall.IsLimitProductTime(productInfo, orderInfo.Ex1, orderInfo.Ex2))
                    //{
                    //    resp.errcode = 1;
                    //    resp.errmsg = string.Format("{0} 暂时不能购买", productInfo.PName);
                    //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //    return;
                    //}


                    WXMallOrderDetailsInfo detailModel = new WXMallOrderDetailsInfo();
                    detailModel.OrderID = orderInfo.OrderID;
                    detailModel.PID = productInfo.PID;
                    detailModel.TotalCount = sku.count;
                    if (!string.IsNullOrEmpty(productInfo.RelationProductId))//
                    {
                        //detailModel.OrderPrice = bllMall.GetSkuPrice(productSku) * day;
                        //List<string> dateRange = new List<string>();//时间范围
                        //startTime = DateTime.Parse(startTime.ToString("yyyy/MM/dd"));
                        //returnTime = DateTime.Parse(returnTime.ToString("yyyy/MM/dd"));
                        //for (int i = 1; i < (returnTime - startTime).TotalDays; i++)
                        //{
                        //    dateRange.Add(startTime.AddDays(i).ToString("yyyy/MM/dd"));
                        //}
                        //dateRange.Add(startTime.ToString("yyyy/MM/dd"));
                        //dateRange.Add(returnTime.ToString("yyyy/MM/dd"));
                        //dateRange = dateRange.Distinct().ToList();
                        //detailModel.OrderPrice = 0;
                        //foreach (var date in dateRange)//
                        //{
                        detailModel.OrderPrice= bllMall.GetSkuPriceByDate(productSku, startTime.ToString("yyyy/MM/dd"))*day;
                        //}
                        deviceFee += (decimal)detailModel.OrderPrice * detailModel.TotalCount;


                    }
                    else//设备租金
                    {
                        detailModel.OrderPrice = bllMall.GetSkuPrice(productSku);
                    }
                    detailModel.ProductName = productInfo.PName;
                    detailModel.SkuId = productSku.SkuId;
                    detailModel.ParentProductId = sku.parent_product_id;
                    detailList.Add(detailModel);


                }
                #endregion
                productFee = detailList.Sum(p => p.OrderPrice * p.TotalCount).Value;   //商品费用
                #region 运费计算

                List<ZentCloud.BLLJIMP.Model.API.Mall.SkuModel> skuList = new List<ZentCloud.BLLJIMP.Model.API.Mall.SkuModel>();
                foreach (var item in orderRequestModel.skus)
                {
                    ZentCloud.BLLJIMP.Model.API.Mall.SkuModel sku = new BLLJIMP.Model.API.Mall.SkuModel();
                    sku.sku_id = item.sku_id;
                    sku.count = item.count;
                    skuList.Add(sku);
                }
                decimal freight = 0;//运费
                string freightMsg = "";
                if (orderRequestModel.delivery_type == 0)//配送方式为快递时才计算运费
                {

                    FreightModel freightModel = new FreightModel();
                    freightModel.receiver_province_code = int.Parse(orderRequestModel.receiver_province_code);
                    freightModel.receiver_city_code = int.Parse(orderRequestModel.receiver_city_code);
                    freightModel.receiver_dist_code = int.Parse(orderRequestModel.receiver_dist_code);
                    freightModel.skus = skuList;
                    foreach (int relationSku in relationSkuList)
                    {
                        //关联SKU不参与运费计算
                        freightModel.skus = freightModel.skus.Where(p => p.sku_id != relationSku).ToList();
                    }
                    if (!bllMall.CalcFreight(freightModel, out freight, out  freightMsg))
                    {
                        resp.errcode = 1;
                        resp.errmsg = freightMsg;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                }



                orderInfo.Transport_Fee = freight;
                #endregion

                orderInfo.Product_Fee = productFee;
                orderInfo.TotalAmount = orderInfo.Product_Fee + orderInfo.Transport_Fee;
                #region 优惠券计算
                decimal discountAmount = 0;//优惠金额
                bool canUseCardCoupon = false;
                string msg = "";
                if (orderRequestModel.cardcoupon_id > 0)//有优惠券
                {
                    discountAmount = bllMall.CalcDiscountAmountWifi(orderRequestModel.cardcoupon_id.ToString(), data, CurrentUserInfo.UserID, out canUseCardCoupon, deviceFee, out msg);
                    if (!canUseCardCoupon)
                    {
                        resp.errcode = 1;
                        resp.errmsg = msg;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                    }
                    if (discountAmount > productFee + orderInfo.Transport_Fee)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "优惠券可优惠金额超过了订单总金额";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;

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
                        resp.errcode = 1;
                        resp.errmsg = "积分不足";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                    }
                    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                    scoreExchangeAmount = Math.Round(orderRequestModel.use_score / (scoreConfig.ExchangeScore / scoreConfig.ExchangeAmount), 2);
                }



                //积分计算 
                #endregion

                #region 合计计算

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
                #endregion
                if (orderInfo.TotalAmount <= 0)
                {
                    //resp.errcode = 1;
                    //resp.errmsg = "付款金额不能小于0";
                    //context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    //return;
                    orderInfo.TotalAmount = 0;
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
                        resp.errcode = 1;
                        resp.errmsg = "提交失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
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
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
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
                            resp.errcode = 1;
                            resp.errmsg = "更新用户积分失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                        }
                        //积分记录
                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        scoreRecord.AddTime = DateTime.Now;
                        scoreRecord.Score = -orderRequestModel.use_score;
                        scoreRecord.TotalScore = CurrentUserInfo.TotalScore;
                        scoreRecord.ScoreType = "OrderSubmit";
                        scoreRecord.UserID = CurrentUserInfo.UserID;
                        scoreRecord.AddNote = "微商城-下单使用积分";
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
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp)); return;
                        }
                        //更新 SKU库存 
                        if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("update ZCJ_ProductSku set Stock-={0} where SkuId={1} And Stock>0", item.TotalCount, productSku.SkuId), tran) <= 0)
                        {
                            tran.Rollback();
                            resp.errcode = 1;
                            resp.errmsg = "提交订单失败,库存不足";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                    }
                    #endregion
                    bllMall.DeleteShoppingCart(CurrentUserInfo.UserID, skuList);
                    bllLog.Add(BLLJIMP.Enums.EnumLogType.Mall, BLLJIMP.Enums.EnumLogTypeAction.Add, bllLog.GetCurrUserID(), "提交订单", orderInfo.OrderID);
                    tran.Commit();//提交订单事务

                }
                catch (Exception ex)
                {
                    //回滚事物
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "提交订单失败,内部错误";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok",
                    order_id = orderInfo.OrderID,
                    total_amount = orderInfo.TotalAmount
                }));
            }
            catch (Exception ex)
            {
                
                resp.errcode = 1;
                resp.errmsg = ex.ToString();
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

           


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
            /// 配送方式
            /// 0 快递
            /// 1 上门自提
            /// </summary>
            public int delivery_type { get; set; }

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

            /// <summary>
            /// 收货人固话
            /// </summary>
            public string receiver_tel { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            public string email { get; set; }

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
            /// 出发日期
            /// </summary>
            public string departure_date { get; set; }

            /// <summary>
            /// 回国日期
            /// </summary>
            public string backhome_date { get; set; }

            /// <summary>
            /// 自提点ID 配送方式为上门自提时有值
            /// </summary>
            public string get_address_id { get; set; }

            /// <summary>
            /// 自提点 配送方式为上门自提时有值
            /// </summary>
            public string get_address_name { get; set; }

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

            /// <summary>
            /// 父商品ID
            /// </summary>
            public string parent_product_id { get; set; }

        }


        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        private string CreateOrderId(string orderId)
        {
            return string.Format("1{0}", orderId.PadLeft(11, '0'));

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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using ZentCloud.BLLJIMP;
using Payment.JDPay.Model;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using ZentCloud.Common;
using System.Web.SessionState;
using System.Xml;
using System.Text.RegularExpressions;

namespace ZentCloud.JubitIMP.Web.JDPayNotify
{
    /// <summary>
    /// 京东支付通知地址
    /// </summary>
    public class MallNotify : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public WXMallOrderInfo orderInfo = new WXMallOrderInfo();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 支付BLL
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 通用关系BLL
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeiXin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 菜单权限BLL
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// Efast BLL
        /// </summary>
        BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();
        /// <summary>
        /// 驿氪BLL
        /// </summary>
        Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        /// 商城分销BLL
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCard = new BLLJIMP.BLLCardCoupon();
        /// <summary>
        /// 成功
        /// </summary>
        private string successStr = "ok";
        /// <summary>
        /// 失败
        /// </summary>
        private string failStr = "fail";
        public void ProcessRequest(HttpContext context)
        {

            try
            {
                Tolog("京东支付通知start");
                var payConfig = bllPay.GetPayConfig();
                byte[] byts = new byte[context.Request.InputStream.Length];
                context.Request.InputStream.Read(byts, 0, byts.Length);
                string req = Encoding.UTF8.GetString(byts);
                Tolog("通知参数"+req);

                //var jdPubKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCKE5N2xm3NIrXON8Zj19GNtLZ8xwEQ6uDIyrS3S03UhgBJMkGl4msfq4Xuxv6XUAN7oU1XhV3/xtabr9rXto4Ke3d6WwNbxwXnK5LSgsQc1BhT5NcXHXpGBdt7P8NMez5qGieOKqHGvT0qvjyYnYA29a8Z4wzNR7vAVHp36uD5RwIDAQAB";

                //req = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><jdpay><version>V2.0</version><merchant>110226843002</merchant><result><code>000000</code><desc>success</desc></result><encrypt>MDIxZjNkNjI1YjU1NzQ4MWE5N2ExMTg3YjkxNmQ4NzkxYzQyMmFjZjM4YTM1MjZjY2JjNzM1ZDkxY2Q5ZDNmMTMzMGFjYTBkNTI0MzYzNjc3MTVhODI3OWUzZTAxMmY5ODEyOTVmNDNiNWY5MDZhZGJiYTcwYTYyOTFlYzVmYTU2Y2EyN2U1YzhkNzllMGE3ZTUyNDE4NWU4OWMxNjIwNDFkODcyYzJiZDA4ZWY4YWEyMmY5ZWUxNDk3YTg3MTI3YmU1NTMxNjc5NWJiOTlmN2ZmNGU1MTc1YWJhNjNkYjUzYWUwZDQzOTk4ZjIzMjBiYmVkNGJkMDcxOWUzOThjZjU1ODUwNzM4Y2RiNzM2Njc1N2U2ZTcxN2Q0N2ZiYTZmN2M1YzBiYzRmMjc4ZThiZDNhYTkxZTExNzBiYjg2ZDNjNmQ3OWUyZTBlYjUzZWNlZjFjODQ2MzdiN2E5MTQxN2Y3NmRhZmNkNDdiMzMwNjc1MGRhYjhiYzg0NTFkOWNiY2QyMDZhOTJiYzU2YTFiOWEwMjRmNTZhMWZhNTVhNjlmYTA1ZDFlMmI0YTI4MGE1YTU0N2NlMjc3ZWMwM2QyNWE4ODdlOTA0ZGM3YTY2MDViY2I1OTI5MDBlYWU4MGU0Mzc4MmEwOWY3ZjEwZTk5MGZkOGUzYTA4MzNkNGMyZGZkNWM3MDhkOGU4N2NhMmQyZGM1MDgwYzUyOTg3OGNkMzFhZWRkMTE1NDM5ZjExYTM2OGM2OGE4MGZjZTYyMjJkNzlmODcwNGYyNDMzMTYyZThhZTBkODM2ODBjYzg5ZGRjMWY3ZGVmYzQzYjc4MDZiMDNhMTBmZTc2YjI3MThjYjQ0YjQwZDkyY2E3OGUzYmYzZmFlNjBlOTI4OGU1ODVkNjBjMWZiNDBmMjFjNzVmNDkxYmRkYWFlNzQ0YjZkYmU1ODNkOWQ4OGYwN2EyMDViOWQ5MGNjMzViMTE3MDQ4NWVlNTdlN2Y5MTRhZDM3YzFlODY1NDFiZmQyNDg1MzhlZGZiNDNiZWZmZjY3YmE3NWQ5YjI0MzE4ZDMzMDE5NTE3YWM4ZTJiMDZhZWYyM2NhNjMwODc3MDhkNTdkZWI3MWVhMmY2MzA2ZDliZjBmZGFlNzQ3ODgzODg0ZjVhOWFkODIxYWM0NGQ1ZDlmMGRlNDhkMjBiYTJjYmQ4NTlkMmU3NDMxZDExZmRjMzkxMDU3ZmE3NGE0Yzg2OThkMmY0ZWQ1ZjE3MjIwOWQ1ZTBmZGIzZjFhNGYwOTllZWY5YzRiMWYyZDAxZjlhNzhlYWY5ZjU0YmIyZjczNmUxMjJkNWY2NzhlMDFmYjU0YjY3NWRlYTc1ZWZkNmMwNTJhZmY3ZGVhMGM5NjAyMWQwMGQyMjI2NzdhM2RlNDdhMTdkMWI4ZWQxYmEyYWZlZDg1ZjI4NDk2ZmI2MGVjOTc5OTc4MjgzZTEyNzY2ZmI1OWUzZjY1MWI4OTVlMmQ4OGNlNGRhODg0NzJiNTFiN2RmZDc5NzdkNDk4NTY0NGU4ZjBmNGZjMTM3MmUzMGNhMTUwOTFkNDFhODIzMjZiODU0YjMzNmI1N2EwZDVjMjZjOWNjMzBlMjFjZjNkMDA0NjQ4Zjk0NjQzZWRhOTU1MmIyZjJkYzZjNGJmOTU5MDIzNTBlODlmOTNhZDRhYmEyMzZiY2E1OWE4NzY5Mjc0ZTcwN2MyNGFjNGJmOTU5MDIzNTBlODlmNDNlZGE5NTUyYjJmMmRjNjU0MTNlZjYxZTNlODc4MDk3MmNiYjg3NTVjNmU1NWNlZDljNWU0ZDE5ZTRmZjJjZTAxNmJiMGIzYWFlOTdlYTQzYmM5NWVmMGU0ZjUyNzY4NDNlZGE5NTUyYjJmMmRjNmMyOWRiYWZkYTNjYzQ1M2E3ZGJhZWNjZWJmNGIxZmQ5MTMxMmRiMzliOTU2YzBmNmNiODMxMDQ1ZDBiYjM1ZTNmMzlmMGE0ODNiN2M3ODYyZjNjMTFiN2ZiODljNDNkMjE4NGFlNzU5M2JhMmQ2YTJkOWMwMmYxYTBlNTg2MzhkYzU0NzE0NjExNzkyNGU4ZjQ4NDgzMTAyNDY5OGRlNGZiMDVmNGQzNzE3NGMwOGI2NGU2NjkxMmU2NGY5M2I0YjNiMThmMzZiZmY0NTgwN2FjMDAxOWRlY2ZkYTcyOGFmNzIyZTQwNjhlMTViM2UwMmQzMmRiYjJkOTE2MmQzNWMzZWM2OGRkZjJjMjdmOTRhZGNmYzEzOTdlOWY0NjQwODFiYWU0Y2E3Y2NjZDY0NjQzNGFmZWU4ODExYWRiZTBlY2MwY2JlOThmNDliMDZkYjE2YjNjNTZhOTRiOGZkMDU=</encrypt></jdpay>";

                //req = Regex.Replace(req, @"[\t\n\r]", "", RegexOptions.IgnoreCase);

                req = req.Replace("\r","").Replace("\n", "").Replace("\t", "");

                AsynNotifyResponse anyResponse = Payment.JDPay.XMLUtil.decryptResXml<AsynNotifyResponse>(Payment.JDPay.Config.JDPubKey, payConfig.JDPayDESKey, req);
                // System.Diagnostics.Debug.WriteLine("异步通知订单号：" + anyResponse.tradeNum + ",状态：" + anyResponse.status);
                Tolog("异步通知订单号：" + anyResponse.tradeNum + ",状态：" + anyResponse.status);
                var orderId = anyResponse.tradeNum;
                var orderInfo = bllMall.GetOrderInfo(orderId);
                if (orderInfo == null)
                {
                    //context.Response.Write("订单未找到");
                    context.Response.StatusCode = 500;
                    context.Response.Write(failStr);
                    return;
                }
                if (orderInfo.PaymentStatus.Equals(1))
                {
                    //Tolog("已支付");
                   // context.Response.Write("订单已支付");
                    context.Response.StatusCode = 200;
                    context.Response.Write(successStr);
                    return;
                }



                if (anyResponse.status == "2")
                {
                    orderInfo.PaymentType = 3;

                    //支付成功
                    WXMallProductInfo tProductInfo = new WXMallProductInfo();
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);//下单用户信息
                    string hasOrderIDs = "";
                    int maxCount = 1;
                    //Tolog("准备检查更新订单状态");
                    if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                    {
                        //Tolog("预订类型");
                        #region 预约订单修改状态
                        orderInfo.PaymentStatus = 1;
                        orderInfo.PayTime = DateTime.Now;
                        orderInfo.Status = "预约成功";

                        #region 检查是否有预约成功的订单
                        List<WXMallOrderDetailsInfo> tDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID, null, orderInfo.ArticleCategoryType, null, null);
                        List<WXMallOrderDetailsInfo> oDetailList = bllMall.GetOrderDetailsList(null, tDetailList[0].PID, orderInfo.ArticleCategoryType, tDetailList.Min(p => p.StartDate), tDetailList.Max(p => p.EndDate));
                        tProductInfo = bllMall.GetByKey<WXMallProductInfo>("PID", tDetailList[0].PID);
                        maxCount = tProductInfo.Stock;
                        List<string> hasOrderIDList = new List<string>();
                        foreach (var item in tDetailList)
                        {
                            List<WXMallOrderDetailsInfo> hasOrderDetailList = oDetailList.Where(p => !((item.StartDate >= p.EndDate && item.EndDate > p.EndDate) || (item.StartDate < p.StartDate && item.EndDate <= p.StartDate))).ToList();
                            if (hasOrderDetailList.Count >= maxCount)
                            {
                                hasOrderIDList.AddRange(hasOrderDetailList.Select(p => p.OrderID).Distinct());
                            }
                        }
                        hasOrderIDList = hasOrderIDList.Where(p => !p.Contains(orderInfo.OrderID)).ToList();
                        if (hasOrderIDList.Count > 0)
                        {
                            hasOrderIDList = hasOrderIDList.Distinct().ToList();
                            hasOrderIDs = MyStringHelper.ListToStr(hasOrderIDList, "'", ",");
                        }
                        #endregion 检查是否有预约成功的订单

                        #endregion 预约订单修改状态
                    }
                    else
                    {
                        //Tolog("普通类型");
                        #region 原订单修改状态
                        orderInfo.PaymentStatus = 1;
                        orderInfo.Status = "待发货";
                        orderInfo.PayTime = DateTime.Now;
                        Tolog("更改状态start");
                        //if (bllMall.GetWebsiteInfoModelFromDataBase().IsDistributionMall.Equals(1))
                        //{
                        orderInfo.GroupBuyStatus = "0";
                        orderInfo.DistributionStatus = 1;

                        #region 拼团订单
                        if (orderInfo.OrderType == 2)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(orderInfo.GroupBuyParentOrderId))
                                {
                                    var parentOrderInfo = bllMall.GetOrderInfo(orderInfo.GroupBuyParentOrderId);

                                    if (bllMall.GetCount<WXMallOrderInfo>(string.Format("PaymentStatus=1 And GroupBuyParentOrderId='{0}' Or OrderId='{0}'", parentOrderInfo.OrderID)) >= parentOrderInfo.PeopleCount)
                                    {
                                        bllMall.Update(new WXMallOrderInfo(), string.Format("Status='已取消'"), string.Format("  GroupBuyParentOrderId='{0}' And PaymentStatus=0", parentOrderInfo.OrderID));
                                        parentOrderInfo.GroupBuyStatus = "1";
                                        bllMall.Update(parentOrderInfo);

                                    }

                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region 活动订单
                        if (orderInfo.OrderType == 4)
                        {
                            ActivityDataInfo data = bllMall.Get<ActivityDataInfo>(string.Format(" OrderId='{0}'", orderInfo.OrderID));
                            if (data != null)
                            {
                                bllMall.Update(data, string.Format(" PaymentStatus=1"), string.Format("  OrderId='{0}'", orderInfo.OrderID));
                            }
                        }
                        #endregion

                        bllMall.Update(orderInfo, string.Format("PaymentStatus=1,Status='待发货',PayTime=GETDATE(),DistributionStatus=1"), string.Format("ParentOrderId='{0}'", orderInfo.OrderID));

                        //}
                        #endregion 原订单修改状态

                        //orderInfo.PayTranNo = tradeNo;
                    }
                    bool result = false;

                    if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                    {
                        if (string.IsNullOrWhiteSpace(hasOrderIDs)) hasOrderIDs = "'0'";
                        result = bllMall.Update(new WXMallOrderInfo(),
                            string.Format("PaymentStatus={0},PayTime=GetDate(),Status='{1}'", 1, "预约成功"),
                            string.Format("OrderID='{0}' and WebsiteOwner='{4}' AND (select count(1) from [ZCJ_WXMallOrderInfo] where Status='{3}' and WebsiteOwner='{4}' and  OrderID IN({1}))<{2}",
                                orderInfo.OrderID, hasOrderIDs, maxCount, "预约成功", bllMall.WebsiteOwner)
                            ) > 0;
                        if (result)
                        {
                            // #region 交易成功加积分
                            //增加积分 （慧聚不需要）
                            //if (orderInfo.TotalAmount > 0)
                            //{
                            //    ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                            //    int addScore = 0;
                            //    if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                            //    {
                            //        addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                            //    }
                            //    if (addScore > 0)
                            //    {
                            //        if (bllUser.Update(new UserInfo(), 
                            //            string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore),
                            //            string.Format(" UserID='{0}'", orderInfo.OrderUserID)) > 0)
                            //        {
                            //            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            //            scoreRecord.AddTime = DateTime.Now;
                            //            scoreRecord.Score = addScore;
                            //            scoreRecord.ScoreType = "OrderSuccess";
                            //            scoreRecord.UserID = orderInfo.OrderUserID;
                            //            scoreRecord.AddNote = "预约-交易成功获得积分";
                            //            bllMall.Add(scoreRecord);
                            //        }
                            //    }
                            //}
                            // #endregion

                            #region 修改其他预约订单为预约失败 返还积分

                            if (BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType) && !string.IsNullOrWhiteSpace(hasOrderIDs))
                            {
                                int tempCount = 0;
                                List<WXMallOrderInfo> tempList = bllMall.GetOrderList(0, 1, "", out tempCount, "预约成功", null, null, null,
                                        null, null, null, null, null, null, null, orderInfo.ArticleCategoryType, hasOrderIDs);
                                tempCount = tempCount + 1; //加上当前订单的数量
                                if (tempCount >= maxCount)
                                {
                                    tempList = bllMall.GetColOrderListInStatus("'待付款','待审核'", hasOrderIDs, "OrderID,OrderUserID,UseScore", bllMall.WebsiteOwner);
                                    if (tempList.Count > 0)
                                    {
                                        string stopOrderIds = MyStringHelper.ListToStr(tempList.Select(p => p.OrderID).ToList(), "'", ",");
                                        tempList = tempList.Where(p => p.UseScore > 0).ToList();
                                        foreach (var item in tempList)
                                        {
                                            orderUserInfo.TotalScore += item.UseScore;
                                            if (bllUser.Update(new UserInfo(), string.Format(" TotalScore+={0}", item.UseScore),
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
                                            string.Format("OrderID In ({0}) and WebsiteOwner='{1}'", stopOrderIds, bllMall.WebsiteOwner));
                                    }
                                }
                                //Tolog("更改修改其他预约为预约失败");
                            }
                            #endregion

                        }
                    }
                    else
                    {
                        result = bllMall.Update(orderInfo);
                    }
                    if (result)
                    {
                        Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);

                        //Tolog("更改状态true");

                        #region Efast同步
                        //判读当前站点是否需要同步到驿氪和efast
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncEfast, bllCommRelation.WebsiteOwner, ""))
                        {
                            try
                            {
                                Tolog("开始同步efast");

                                string outOrderId = string.Empty, msg = string.Empty;
                                var syncResult = bllEfast.CreateOrder(orderInfo.OrderID, out outOrderId, out msg);
                                if (syncResult)
                                {
                                    orderInfo.OutOrderId = outOrderId;
                                    bllMall.Update(orderInfo);
                                }
                                Tolog(string.Format("efast订单同步结果:{0},订单号：{1}，提示信息：{2}", syncResult, outOrderId, msg));

                            }
                            catch (Exception ex)
                            {
                                Tolog("efast订单同步异常：" + ex.Message);

                            }
                        }
                        #endregion

                        #region 驿氪同步
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {
                            try
                            {

                                Tolog("开始同步驿氪");
                                //同步成功订单到驿氪

                                //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                                //if ((!string.IsNullOrEmpty(orderUserInfo.Ex1)) && (!string.IsNullOrEmpty(orderUserInfo.Ex2)) && (!string.IsNullOrEmpty(orderUserInfo.Phone)))
                                //{
                                //    client.BonusUpdate(orderUserInfo.Ex2, -(orderInfo.UseScore), "商城下单使用积分" + orderInfo.UseScore);

                                //}
                                var uploadOrderResult = yikeClient.OrderUpload(orderInfo);

                                Tolog(string.Format("驿氪订单同步结果：{0}", Common.JSONHelper.ObjectToJson(uploadOrderResult)));
                            }
                            catch (Exception ex)
                            {
                                Tolog("驿氪订单同步异常：" + ex.Message);

                            }
                        }
                        #endregion

                        #region 付款加积分
                        try
                        {
                            bllUser.AddUserScoreDetail(orderInfo.OrderUserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.OrderPay), bllMall.WebsiteOwner, null, null);
                        }
                        catch (Exception)
                        { }

                        #endregion


                        #region 消息通知
                        if (!BLLJIMP.BLLMall.bookingList.Contains(orderInfo.ArticleCategoryType))
                        {
                            try
                            {
                                var productName = bllMall.GetOrderDetailsList(orderInfo.OrderID)[0].ProductName;
                                string remark = "";
                                if (!string.IsNullOrEmpty(orderInfo.OrderMemo))
                                {
                                    remark = string.Format("客户留言:{0}", orderInfo.OrderMemo);
                                }
                                bllWeiXin.SendTemplateMessageToKefu("有新的订单", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}\\n{5}", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName, remark));
                                if (orderInfo.OrderType != 4)//付费的活动不发消息
                                {
                                    if (orderInfo.WebsiteOwner != "jikuwifi")
                                    {
                                        string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", context.Request.Url.Host, orderInfo.OrderID);
                                        bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, "订单已成功支付，我们将尽快发货，请保持手机畅通等待物流送达！", string.Format("订单号:{0}\\n订单金额:{1}元\\n收货人:{2}\\n电话:{3}\\n商品:{4}...\\n查看详情", orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.Consignee, orderInfo.Phone, productName), url);

                                    }

                                }

                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            try
                            {
                                bllWeiXin.SendTemplateMessageToKefu(orderInfo.Status, string.Format("预约:{2}\\n订单号:{0}\\n订单金额:{1}元\\n预约人:{3}\\n预约人手机:{4}", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName, orderUserInfo.TrueName, orderUserInfo.Phone));
                                bllWeiXin.SendTemplateMessageNotifyComm(orderUserInfo, orderInfo.Status, string.Format("预约:{2}\\n订单号:{0}\\n订单金额:{1}元", orderInfo.OrderID, orderInfo.TotalAmount, tProductInfo.PName));
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion
                        WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                        #region 分销相关
                        try
                        {
                            if (bllMenuPermission.CheckUserAndPmsKey(websiteInfo.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution, websiteInfo.WebsiteOwner))
                            {

                                if (string.IsNullOrWhiteSpace(orderUserInfo.DistributionOwner))
                                {

                                    if (websiteInfo.DistributionRelationBuildMallOrder == 1)
                                    {
                                        orderUserInfo.DistributionOwner = bllMall.WebsiteOwner;
                                        bllMall.Update(orderUserInfo);

                                    }

                                }

                                bllDis.AutoUpdateLevel(orderInfo);
                                bllDis.TransfersEstimate(orderInfo);
                                bllDis.SendMessageToUser(orderInfo);
                                bllDis.UpdateDistributionSaleAmountUp(orderInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Tolog("设置分销员异常：" + ex.Message + " 用户id：" + orderUserInfo.UserID);
                        }
                        #endregion


                        #region 宏巍通知
                        try
                        {
                            if (websiteInfo.IsUnionHongware == 1)
                            {
                                hongWareClient.OrderNotice(orderUserInfo.WXOpenId, orderInfo.OrderID);

                            }
                        }
                        catch (Exception)
                        {


                        }

                        #endregion

                        bllCard.Give(orderInfo.TotalAmount, orderUserInfo);


                        string v1ProductId = Common.ConfigHelper.GetConfigString("YGBV1ProductId");
                        string v2ProductId = Common.ConfigHelper.GetConfigString("YGBV2ProductId");
                        string v1CouponId = Common.ConfigHelper.GetConfigString("YGBV1CouponId");
                        string v2CouponId = Common.ConfigHelper.GetConfigString("YGBV2CouponId");

                        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        foreach (var item in orderDetailList)
                        {
                            item.IsComplete = 1;
                            bllMall.Update(item);
                            #region 购买指定商品发送指定的优惠券
                            if (!string.IsNullOrEmpty(v1ProductId) && !string.IsNullOrEmpty(v1CouponId) && item.PID == v1ProductId)
                            {
                                bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v1CouponId);
                            }

                            if (!string.IsNullOrEmpty(v2ProductId) && !string.IsNullOrEmpty(v2CouponId) && item.PID == v2ProductId)
                            {
                                bllCard.SendCardCouponsByCurrUserInfo(orderUserInfo, v2CouponId);
                            }
                            #endregion
                        }
                        //更新销量
                        bllMall.UpdateProductSaleCount(orderInfo);
                        
                        context.Response.StatusCode = 200;
                        context.Response.Write(successStr);
                        Tolog("支付成功"+orderInfo.OrderID);
                        return;
                    }
                    else
                    {
                        Tolog("更改状态false");
                        context.Response.StatusCode = 500;
                        context.Response.Write(failStr);
                        return;
                    }




                }

            }
            catch(Exception ex)
            {
                //error = "fail";
                Tolog("京东支付异常:"+ex.ToString());
            }

            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 500;
            context.Response.Write(failStr);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        public void Tolog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\log\\JDPayMallNotify.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                }

            }
            catch { }
        }

    }
}
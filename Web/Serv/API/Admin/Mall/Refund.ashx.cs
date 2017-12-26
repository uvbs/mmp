using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 退款
    /// </summary>
    public class Refund : BaseHandlerNeedLoginAdmin
    {


        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 支付逻辑
        /// </summary>
        BllPay bllPay = new BllPay();
        /// <summary>
        /// 用户逻辑
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 积分逻辑
        /// </summary>
        BllScore bllScore = new BllScore();
        /// <summary>
        /// yike 
        /// </summary>
        Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// 微信
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 菜单权限BLL
        /// </summary>
        BLLPermission.BLLMenuPermission bllMenuPermission = new BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// 分销
        /// </summary>
        BLLDistribution bllDistribution = new BLLDistribution();
        /// <summary>
        /// 
        /// </summary>
        BLLTransfersAudit bllTran = new BLLTransfersAudit();
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLPermission bllPer = new BLLPermission.BLLPermission();
        /// <summary>
        /// 获取退款详细信息接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string orderDetailId = context.Request["order_detail_id"];
            var sourceData = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            if (sourceData == null || (sourceData.WebSiteOwner != bllMall.WebsiteOwner))
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = "暂无退款申请"

                });
            }
            var data = new
            {
                refund_id = sourceData.RefundId,
                time = bllMall.GetTimeStamp(sourceData.InsertDate),
                order_detail_id = sourceData.OrderDetailId,
                refund_reason = sourceData.RefundReason,
                refund_status = sourceData.Status,
                refund_amount = sourceData.RefundAmount,
                refund_account_amount = sourceData.RefundAccountAmount,
                refund_score = sourceData.RefundScore,
                is_contain_transportfee = sourceData.IsContainTransportFee,
                phone = sourceData.Phone,
                remark = sourceData.Remark,
                imagelist = !string.IsNullOrEmpty(sourceData.ImageList) ? sourceData.ImageList.Split(',') : new string[0],
                refund_address = sourceData.RefundAddress,
                express_company_name = sourceData.ExpressCompanyName,
                express_number = sourceData.ExpressNumber,
                is_return_product = sourceData.IsReturnProduct,
                product_status = sourceData.ProductStatus
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 同意退款申请,提交退货地址接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AgreeAndSubmitRefundAddress(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            string refundAddress = context.Request["refund_address"];

            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(refundAddress))
            {
                resp.errcode = 1;
                resp.errmsg = "退货地址不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }


            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.Status = 1;
            model.RefundAddress = refundAddress;
            WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
            orderDetail.RefundStatus = model.Status.ToString();
            if (!bllMall.Update(orderDetail))
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.Update(model))
            {
                resp.errmsg = "ok";
                //插入维权记录
                WXMallRefundLog log = new WXMallRefundLog();
                log.OrderDetailId = model.OrderDetailId;
                log.Role = "商家";
                log.Title = "同意退款申请";
                log.LogContent = string.Format("退货地址:{0}", model.RefundAddress);
                log.InsertDate = DateTime.Now;
                log.WebSiteOwner = bllMall.WebsiteOwner;
                bllMall.Add(log);
                //插入维权记录
                try
                {
                    var result = yikeClient.UpdateRefundStatus(model);
                }
                catch (Exception)
                {


                }
                //if (bllPer.CheckPermissionKey(bllMall.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.PMS_TRANSFERSAUDIT))
                //{
                //    string tranInfo = string.Format("订单号:{0}<br/>退款金额:{1}<br/>{2}", model.OrderId, model.RefundAmount, log.LogContent);
                //    if (bllTran.Add("MallRefund", model.RefundId, tranInfo, model.RefundAmount))
                //    {
                //        string title = string.Format("收到退款申请");
                //        string content = string.Format("订单号:{0}退款金额:{1}", model.OrderId, model.RefundAmount);
                //        string url = string.Format("http://{0}/app/transfersaudit/list.aspx", System.Web.HttpContext.Current.Request.Url.Host);
                //        //发送微信模板消息
                //        bllWeixin.SendTemplateMessageToKefuTranAuditPer(title, content, url);
                //    }
                //    else
                //    {
                //        resp.errcode = 1;
                //        resp.errmsg = "操作失败";
                //        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //    }

                //}



            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        /// 拒绝退款申请
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RefuseApply(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            string refuseReason = context.Request["refuse_reason"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(refuseReason))
            {
                resp.errcode = 1;
                resp.errmsg = "拒绝理由 不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.Status = 2;
            model.RefuseReason = refuseReason;
            model.UpdateTime = DateTime.Now;
            WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
            orderDetail.RefundStatus = "2";
            if (!bllMall.Update(orderDetail))
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.Update(model))
            {
                resp.errmsg = "ok";
                //插入维权记录
                WXMallRefundLog log = new WXMallRefundLog();
                log.OrderDetailId = model.OrderDetailId;
                log.Role = "商家";
                log.Title = "拒绝退款申请";
                log.LogContent = string.Format("拒绝原因:{0}", model.RefuseReason);
                log.InsertDate = DateTime.Now;
                log.WebSiteOwner = bllMall.WebsiteOwner;
                bllMall.Add(log);
                //插入维权记录
                try
                {
                    var result = yikeClient.UpdateRefundStatus(model);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 拒绝确认收货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RefuseReceiptConfirm(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.Status = 5;
            model.RefuseReason = "未收到货，拒绝退款";
            WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
            orderDetail.RefundStatus = model.Status.ToString();
            if (!bllMall.Update(orderDetail))
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.Update(model))
            {
                resp.errmsg = "ok";
                //插入维权记录
                WXMallRefundLog log = new WXMallRefundLog();
                log.OrderDetailId = model.OrderDetailId;
                log.Role = "商家";
                log.Title = "拒绝确认收货";
                log.LogContent = string.Format("拒绝原因:{0}", model.RefuseReason);
                log.InsertDate = DateTime.Now;
                log.WebSiteOwner = bllMall.WebsiteOwner;
                bllMall.Add(log);
                //插入维权记录
                try
                {
                    var result = yikeClient.UpdateRefundStatus(model);
                }
                catch (Exception)
                {


                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 商家确认收货并退款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReceiptConfirm(HttpContext context)
        {
            string msg = "";
            if (bllMall.Refund(int.Parse(context.Request["order_detail_id"]), out msg))
            {
                resp.errcode = 0;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;

            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
            //Open.HongWareSDK.MemberInfo hongWareMemberInfo = null;
            //Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);

            //string aliBatchNo = string.Empty;
                        //if ((!string.IsNullOrEmpty(msg)) && (msg.Contains("请使用可用余额退款")))
                        //{
                        //    isSuccess = bllPay.WeixinRefundYuEr(orderInfo.OrderID, model.OrderDetailId.ToString(), orderInfo.TotalAmount, model.RefundAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId);
                        //}

            //#region 检查
            //string orderDetailId = context.Request["order_detail_id"];
            //if (string.IsNullOrEmpty(orderDetailId))
            //{
            //    resp.errcode = 1;
            //    resp.errmsg = "order_detail_id 参数不能为空";
            //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //}
            ////if (currentUserInfo.UserID != bllMall.WebsiteOwner && currentUserInfo.UserType != 1)
            ////{
            ////    resp.errcode = 1;
            ////    resp.errmsg = "无权操作";
            ////    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            ////}
            //#endregion

            //UserInfo orderUserInfo = new UserInfo();//下单用户信息
            //WXMallOrderInfo orderInfo = new WXMallOrderInfo();//主订单
            //ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            //try
            //{
            //    WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            //    if (model.Status == 6)
            //    {
            //        resp.errcode = 1;
            //        resp.errmsg = "已经退过款了";
            //        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //    }
            //    model.Status = 6;//商家退款
            //    WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
            //    orderDetail.RefundStatus = "6";
            //    orderDetail.IsComplete = 0;
            //    if (!bllMall.Update(orderDetail, tran))
            //    {
            //        tran.Rollback();
            //        resp.errcode = 1;
            //        resp.errmsg = "操作失败";
            //        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //    }
            //    if (bllMall.Update(model, tran))
            //    {
            //        resp.errmsg = "ok";
            //        //微信退款业务逻辑
            //        ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
            //        orderInfo = bllMall.GetOrderInfo(model.OrderId);
            //        orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
            //        string msg = "";
            //        string weixinRefundId = "";
            //        if (!string.IsNullOrEmpty(orderInfo.ParentOrderId))//礼品订单退款
            //        {
            //            orderInfo = bllMall.GetOrderInfo(orderInfo.ParentOrderId);

            //        }
            //        #region 退款
            //        if (model.RefundAmount > 0)//需要退款
            //        {
            //            bool isSuccess = false;

            //            #region 微信退款
            //            if (orderInfo.PaymentType == 2)
            //            {
            //                //微信支付的退款
            //                isSuccess = bllPay.WeixinRefund(orderInfo.OrderID, model.OrderDetailId.ToString(), orderInfo.TotalAmount, model.RefundAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId);

            //                if ((!string.IsNullOrEmpty(msg)) && (msg.Contains("请使用可用余额退款")))
            //                {
            //                    isSuccess = bllPay.WeixinRefundYuEr(orderInfo.OrderID, model.OrderDetailId.ToString(), orderInfo.TotalAmount, model.RefundAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId);
            //                }
            //            } 
            //            #endregion
            //            #region 支付宝退款
            //            else if (orderInfo.PaymentType == 1)
            //            {
            //                string batchNo = DateTime.Now.ToString("yyyyMMdd") + ((int)(bllMall.GetTimeStamp(DateTime.Now) / 1000)).ToString();
            //                string notifyUrl = string.Format("http://{0}/Alipay/NotifyRefund.aspx", context.Request.Url.Host);
            //                string remark = string.Format("订单号{0}", orderInfo.OrderID);
            //                //支付宝支付的退款
            //                isSuccess = bllPay.AlipayRefund(orderInfo.PayTranNo, batchNo, model.RefundAmount, notifyUrl, out msg, remark);
            //                if (isSuccess)
            //                {
            //                    aliBatchNo = batchNo;                                
            //                }

            //            } 
            //            #endregion

            //            if (!isSuccess)
            //            {
            //                tran.Rollback();
            //                resp.errcode = 1;
            //                resp.errmsg = msg;
            //                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            //            }
            //        }
            //        #endregion

            //        if (websiteInfo.IsUnionHongware == 1)
            //        {
            //            hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
            //        }
            //        #region 退积分
            //        if (model.RefundScore > 0)//需要退还积分
            //        {
            //            orderUserInfo.TotalScore += (double)model.RefundScore;
            //            if (bllUser.Update(orderUserInfo, string.Format(" TotalScore={0}", orderUserInfo.TotalScore), string.Format(" AutoID={0}", orderUserInfo.AutoID)) > 0)
            //            {
            //                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //                scoreRecord.AddTime = DateTime.Now;
            //                scoreRecord.Score = (double)model.RefundScore;
            //                scoreRecord.TotalScore = orderUserInfo.TotalScore;
            //                scoreRecord.ScoreType = "OrderRefund";
            //                scoreRecord.UserID = orderUserInfo.UserID;
            //                scoreRecord.AddNote = "微商城-退款返还积分";
            //                scoreRecord.RelationID = orderInfo.OrderID;
            //                scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
            //                bllMall.Add(scoreRecord);
            //            }
            //            if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //            {
            //                yikeClient.BonusUpdate(orderUserInfo.Ex2, (int)model.RefundScore, string.Format("订单{0}退款退还{1}积分", orderInfo.OrderID, model.RefundScore));
            //            }

            //            #region 宏巍加积分

            //            if (websiteInfo.IsUnionHongware == 1)
            //            {

            //                if (hongWareMemberInfo.member != null)
            //                {

            //                    if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)model.RefundScore))
            //                    {
            //                        tran.Rollback();
            //                        resp.errcode = 1;
            //                        resp.errmsg = "更新宏巍积分失败";


            //                    }


            //                }
            //                else
            //                {
            //                    tran.Rollback();
            //                    resp.errcode = 1;
            //                    resp.errmsg = "更新宏巍积分失败";
            //                }


            //            }
            //            #endregion


            //        }
            //        #endregion

            //        #region 退余额
            //        if (model.RefundAccountAmount > 0)//需要退还余额
            //        {
            //            orderUserInfo.AccountAmount += model.RefundAccountAmount;
            //            if (bllUser.Update(orderUserInfo, string.Format(" AccountAmount={0}", orderUserInfo.AccountAmount), string.Format(" AutoID={0}", orderUserInfo.AutoID)) > 0)
            //            {
            //                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
            //                scoreRecord.AddTime = DateTime.Now;
            //                scoreRecord.Score = (double)model.RefundAccountAmount;
            //                scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
            //                scoreRecord.UserID = orderUserInfo.UserID;
            //                scoreRecord.AddNote = "微商城-退款余额返还";
            //                scoreRecord.RelationID = orderInfo.OrderID;
            //                scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
            //                scoreRecord.ScoreType = "AccountAmount";
            //                bllMall.Add(scoreRecord);
            //            }
            //            #region 宏巍加余额

            //            if (websiteInfo.IsUnionHongware == 1)
            //            {
            //                if (hongWareMemberInfo.member != null)
            //                {
            //                    if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)model.RefundAccountAmount))
            //                    {
            //                        tran.Rollback();
            //                        resp.errcode = 1;
            //                        resp.errmsg = "更新宏巍余额失败";


            //                    }


            //                }
            //                else
            //                {
            //                    tran.Rollback();
            //                    resp.errcode = 1;
            //                    resp.errmsg = "更新宏巍余额失败";
            //                }


            //            }
            //            #endregion


            //        }
            //        #endregion

            //        resp.errmsg = "ok";
            //        //插入维权记录
            //        WXMallRefundLog log = new WXMallRefundLog();
            //        log.OrderDetailId = model.OrderDetailId;
            //        log.Role = "商家";
            //        log.Title = "确认收货并退款";
            //        log.LogContent = string.Format("确认收货并退款");
            //        log.InsertDate = DateTime.Now;
            //        log.WebSiteOwner = bllMall.WebsiteOwner;
            //        bllMall.Add(log);
            //        //插入维权记录
            //        tran.Commit();

            //        if (!string.IsNullOrWhiteSpace(aliBatchNo))
            //        {
            //            bllMall.Update(model, string.Format("OutRefundId='{0}'", aliBatchNo), string.Format("RefundId='{0}'", model.RefundId));
            //        }

            //        var orderDetailList = bllMall.GetOrderDetailsList(model.OrderId);


            //        #region 交易成功再退款扣除积分
            //        if (orderInfo.Status == "交易成功")//交易成功以后退款
            //        {
            //            try
            //            {

            //                ScoreConfig scoreConfig = bllScore.GetScoreConfig();
            //                if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
            //                {
            //                    int score = (int)(model.RefundAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
            //                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //                    {
            //                        yikeClient.BonusUpdate(orderUserInfo.Ex2, -score, string.Format("退款扣除{0}积分", score));
            //                    }
            //                }


            //            }
            //            catch (Exception)
            //            {


            //            }

            //        }
            //        #endregion

            //        var totalCount = orderDetailList.Count(p => p.RefundStatus == "0")
            //            + orderDetailList.Count(p => p.RefundStatus == "1")
            //            + orderDetailList.Count(p => p.RefundStatus == "2")
            //            + orderDetailList.Count(p => p.RefundStatus == "3")
            //            + orderDetailList.Count(p => p.RefundStatus == "4")
            //            + orderDetailList.Count(p => p.RefundStatus == "5");//退款中的数量
            //        if (totalCount == 0)
            //        {

            //            orderInfo.IsRefund = 0;//此订单再无退款申请
            //            bllMall.Update(orderInfo);
            //        }

            //        #region 所有商品都退款 订单变成已取消
            //        if (orderDetailList.Count(p => p.RefundStatus == "6") == orderDetailList.Count)
            //        {

            //            //修改订单为已经取消
            //            orderInfo.Status = "已取消";
            //            orderInfo.IsRefund = 0;
            //            if (!bllMall.Update(orderInfo))
            //            {

            //                resp.errcode = 1;
            //                resp.errmsg = "修改订单状态失败";
            //                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            //            }
            //            model.WeiXinRefundId = weixinRefundId;
            //            bllMall.Update(model);


            //            #region 驿氪订单状态同步
            //            try
            //            {


            //                if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //                {

            //                    //if (!string.IsNullOrEmpty(orderUserInfo.Phone))
            //                    //{


            //                    yikeClient.ChangeStatus(orderInfo.OrderID, orderInfo.Status);//更新yike订单状态
            //                    yikeClient.UpdateRefundStatus(model);//更新yike 退款状态
            //                    //}


            //                }

            //            }
            //            catch (Exception)
            //            {


            //            }
            //            #endregion






            //        }
            //        #endregion


            //        //try
            //        //{
            //        //    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
            //        //    {
            //        //        var result = yikeClient.UpdateRefundStatus(model);
            //        //    }
            //        //}
            //        //catch (Exception)
            //        //{


            //        //}


            //        //}
            //        List<WXMallOrderDetailsInfo> detailList = new List<WXMallOrderDetailsInfo>();
            //        detailList.Add(orderDetail);
            //        bllMall.ReturnProductSku(detailList);


            //        #region 通知
            //        bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo, "退款成功", string.Format("您的订单:{0}已成功退款\\n退款金额：{1}元\\n退还积分:{2}\\n退还余额:{3}", orderInfo.OrderID, model.RefundAmount, model.RefundScore, model.RefundAccountAmount));

            //        try
            //        {
            //            #region 给商城分销上级通知
            //            //给上级通知
            //            if (bllMenuPermission.CheckUserAndPmsKey(bllMall.WebsiteOwner, BLLPermission.Enums.PermissionSysKey.OnlineDistribution))
            //            {


            //                UserInfo upUserLevel1 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 1);//上一级用户
            //                //UserInfo upUserLevel2 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 2);//上二级用户
            //                //UserInfo upUserLevel3 = bllDistribution.GetUpUser(orderInfo.OrderUserID, 3);//上三级用户                           
            //                if (upUserLevel1 != null)
            //                {
            //                    if (!string.IsNullOrEmpty(upUserLevel1.WXOpenId))
            //                    {
            //                        string disName = bllUser.GetUserDispalyName(orderUserInfo);
            //                        if (string.IsNullOrEmpty(disName))
            //                        {
            //                            disName = "";
            //                        }
            //                        bllWeixin.SendTemplateMessageNotifyComm(upUserLevel1, "佣金取消通知", string.Format("您的代言人{0}的订单:{1}已退款,佣金取消", disName, orderInfo.OrderID));

            //                    }
            //                }

            //                #region 所有已经退款,取消
            //                if (orderDetailList.Count(p => p.RefundStatus == "6") == orderDetailList.Count)
            //                {
            //                    #region 预估佣金取消
            //                    List<ProjectCommissionEstimate> list = bllMall.GetList<ProjectCommissionEstimate>(string.Format("ProjectId='{0}'", orderInfo.OrderID));
            //                    foreach (var item in list)
            //                    {


            //                        if (item.CommissionLevel == "0")
            //                        {
            //                            bllMall.Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0}", item.Amount), string.Format(" UserId='{0}'", item.UserId));

            //                        }
            //                        //    else if (item.CommissionLevel == "1")
            //                        //{
            //                        //    bllMall.Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0},DistributionSaleAmountLevel1-={1}", item.Amount,item.ProjectAmount), string.Format(" UserId='{0}'", item.UserId));

            //                    //}
            //                        else//渠道
            //                        {
            //                            bllMall.Update(new UserInfo(), string.Format(" HistoryDistributionOnLineTotalAmountEstimate-={0},DistributionSaleAmountLevel1-={1}", item.Amount, item.ProjectAmount), string.Format(" UserId='{0}'", item.UserId));

            //                        }


            //                    }
            //                    #endregion

            //                    #region 累计销售额取消
            //                    orderUserInfo.DistributionSaleAmountLevel0 -= orderInfo.TotalAmount;
            //                    if (orderUserInfo.DistributionSaleAmountLevel0 <= 0)
            //                    {
            //                        orderUserInfo.DistributionSaleAmountLevel0 = 0;
            //                    }
            //                    bllMall.Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel0={0}", orderUserInfo.DistributionSaleAmountLevel0), string.Format(" UserId='{0}'", orderInfo.OrderUserID));
            //                    if (upUserLevel1 != null)
            //                    {

            //                        upUserLevel1.DistributionSaleAmountLevel0 -= orderInfo.TotalAmount;
            //                        if (upUserLevel1.DistributionSaleAmountLevel0 <= 0)
            //                        {
            //                            upUserLevel1.DistributionSaleAmountLevel0 = 0;
            //                        }
            //                        bllMall.Update(new UserInfo(), string.Format(" DistributionSaleAmountLevel0={0}", upUserLevel1.DistributionSaleAmountLevel0), string.Format(" UserId='{0}'", upUserLevel1.UserID));


            //                    }
            //                    #endregion


            //                }
            //                else
            //                {
            //                    bllCommRelation.ToLog("未配置分销" + orderInfo.WebsiteOwner);
            //                }
            //                //给上级通知 
            //                #endregion
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            bllCommRelation.ToLog("分销退款异常" + ex.ToString());

            //        }

            //            #endregion
            //        #endregion



            //    }
            //    else
            //    {
            //        tran.Rollback();
            //        resp.errcode = 1;
            //        resp.errmsg = "操作失败";
            //    }


            //}
            //catch (Exception ex)
            //{

            //    tran.Rollback();
            //    resp.errcode = 1;
            //    resp.errmsg = ex.ToString();

            //}




            //return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 查询付款状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWeixinTransferInfo(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
            bool isSuccess = bllPay.GetWeixinTransferInfo(model.OrderId, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                is_pay_success = isSuccess,
            });


        }


        /// <summary>
        /// 查询退款状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWeixinRefundInfo(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
            string msg = "";
            bool isSuccess = bllPay.QueryWeixinRefund(model.OrderDetailId.ToString(), model.WeiXinRefundId, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg);
            if (isSuccess)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    refund_status = msg,
                });
            }
            else
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    refund_status = msg,
                });
            }



        }

        /// <summary>
        /// 协商记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Log(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            List<WXMallRefundLog> sourceData = bllMall.GetList<WXMallRefundLog>(string.Format(" OrderDetailId={0} order by AutoID DESC", orderDetailId));
            var list = from p in sourceData
                       select new
                       {
                           role = p.Role,
                           time = bllMall.GetTimeStamp(p.InsertDate),
                           title = p.Title,
                           content = p.LogContent

                       };

            var data = new
            {
                totalcount = sourceData.Count,
                list = list,//列表

            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);


        }


        /// <summary>
        ///修改退款状态为重新打开
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReOpen(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.Status = 0;
            if (bllMall.Update(model))
            {
                resp.errmsg = "ok";
                WXMallRefundLog log = new WXMallRefundLog();
                log.OrderDetailId = model.OrderDetailId;
                log.Role = "商家";
                log.Title = "修改退款状态为重新打开";
                log.LogContent = string.Format("修改退款状态为重新打开");
                log.InsertDate = DateTime.Now;
                log.WebSiteOwner = model.WebSiteOwner;
                bllMall.Add(log);
                WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
                orderDetail.RefundStatus = "0";
                bllMall.Update(orderDetail);
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderDetail.OrderID);
                if (orderInfo.IsRefund == 0)
                {
                    bllMall.Update(orderInfo, string.Format(" IsRefund=1"), string.Format(" OrderID='{0}'", orderInfo.OrderID));
                }

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }

        /// <summary>
        ///客服帮忙填写物流单号
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SubmitExpressInfo(HttpContext context)
        {

            string orderDetailId = context.Request["order_detail_id"];
            string expressCompanyName = context.Request["express_company_name"];
            string expressNumber = context.Request["express_number"];

            if (string.IsNullOrEmpty(orderDetailId))
            {
                resp.errcode = 1;
                resp.errmsg = "order_detail_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(expressCompanyName))
            {
                resp.errcode = 1;
                resp.errmsg = "express_company_name 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrEmpty(expressNumber))
            {
                resp.errcode = 1;
                resp.errmsg = "express_number 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.ExpressCompanyName = expressCompanyName;
            model.ExpressNumber = expressNumber;
            model.Status = 3;
            WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
            orderDetail.RefundStatus = model.Status.ToString();
            if (!bllMall.Update(orderDetail))
            {
                resp.errcode = 1;
                resp.errmsg = "操作失败";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.Update(model))
            {
                resp.errmsg = "ok";
                //插入维权记录
                WXMallRefundLog log = new WXMallRefundLog();
                log.OrderDetailId = model.OrderDetailId;
                log.Role = "客服";
                log.Title = "已经发货";
                log.LogContent = string.Format("物流公司:{0}<br/>物流单号:{1}", model.ExpressCompanyName, model.ExpressNumber);
                log.InsertDate = DateTime.Now;
                log.WebSiteOwner = bllMall.WebsiteOwner;
                bllMall.Add(log);
                //插入维权记录
                try
                {
                    if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                    {
                        var result = yikeClient.UpdateRefundStatus(model);
                    }



                }
                catch (Exception)
                {


                }
                if (bllPer.CheckPermissionKey(bllMall.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.PMS_TRANSFERSAUDIT))
                {
                    string tranInfo = string.Format("订单号:{0}<br/>退款金额:{1}<br/>{2}", model.OrderId, model.RefundAmount, log.LogContent);
                    if (bllTran.Add("MallRefund", model.RefundId, tranInfo, model.RefundAmount))
                    {
                        string title = string.Format("收到退款申请");
                        string content = string.Format("订单号:{0}退款金额:{1}", model.OrderId, model.RefundAmount);
                        string url = string.Format("http://{0}/app/transfersaudit/list.aspx", System.Web.HttpContext.Current.Request.Url.Host);
                        //发送微信模板消息
                        bllWeixin.SendTemplateMessageToKefuTranAuditPer(title, content, url);
                    }
                    else
                    {
                        resp.errcode = 1;
                        resp.errmsg = "操作失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "发货失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        ///打款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Transfers(HttpContext context)
        {
            int successCount = 0;
            string orderIds = context.Request["order_ids"];//订单
            orderIds = orderIds.TrimEnd(',');
            ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.GetPayConfig();
            #region 团购-要先给团员退款
            if (orderIds.Split(',').Count() == 1)
            {
                var order = bllMall.GetOrderInfo(orderIds.Split(',')[0]);
                if (order.OrderType == 2 && (string.IsNullOrEmpty(order.GroupBuyParentOrderId)))
                {
                    //团长订单不能先退款,先退完团员才能退团长
                    if (bllMall.GetCount<WXMallOrderInfo>(string.Format(" GroupBuyParentOrderId='{0}' And PaymentStatus=1 And (Ex1 IS NULL Or Ex1 ='') And TotalAmount>0", order.OrderID)) > 0)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "请先给团员订单退款,然后给团长订单退款";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                }


            }
            #endregion

            foreach (string orderId in orderIds.Split(','))
            {

                var orderInfo = bllMall.GetOrderInfo(orderId);
                if (orderInfo.WebsiteOwner != bllMall.WebsiteOwner)
                {
                    continue;
                }

                if (orderInfo.TotalAmount > 0)
                {
                    string msg = "";
                    #region 微信退款
                    if (orderInfo.PaymentType == 2)
                    {

                        string weixinRefundId = "";
                        if (bllPay.WeixinRefund(orderInfo.OrderID, orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.TotalAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId, bllMall.WebsiteOwner))
                        {
                            successCount++;
                            orderInfo.Ex1 = weixinRefundId;
                            orderInfo.Ex2 = msg;
                            bllMall.Update(orderInfo);
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(msg)) && (msg.Contains("请使用可用余额退款")))//尝试使用余额退款
                            {
                                if (bllPay.WeixinRefundYuEr(orderInfo.OrderID, orderInfo.OrderID, orderInfo.TotalAmount, orderInfo.TotalAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId, bllMall.WebsiteOwner))
                                {
                                    successCount++;
                                    orderInfo.Ex1 = weixinRefundId;
                                    orderInfo.Ex2 = msg;
                                    bllMall.Update(orderInfo);
                                }
                                else
                                {
                                    resp.errcode = 1;
                                    resp.errmsg = msg;
                                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                                }

                            }
                            else
                            {
                                resp.errcode = 1;
                                resp.errmsg = msg;
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }



                        }
                    }
                    #endregion

                    #region 支付宝退款
                    else if (orderInfo.PaymentType == 1)
                    {
                        string batchNo = DateTime.Now.ToString("yyyyMMdd") + ((int)(bllMall.GetTimeStamp(DateTime.Now) / 1000)).ToString();
                        string notifyUrl = string.Format("http://{0}/Alipay/NotifyRefund.aspx", context.Request.Url.Host);
                        string remark = string.Format("订单号{0}", orderInfo.OrderID);
                        //支付宝支付的退款
                        bool isSuccess = bllPay.AlipayRefund(orderInfo.PayTranNo, batchNo, orderInfo.TotalAmount, notifyUrl, out msg, remark);
                        if (!isSuccess)
                        {
                            resp.errcode = 1;
                            resp.errmsg = msg;
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                    }
                    #endregion

                    else if (orderInfo.PaymentType == 3)
                    {
                        if (!bllPay.JDPayRefund(orderInfo.OrderID, orderInfo.OrderID, orderInfo.TotalAmount, "", "", out msg))
                        {
                            resp.errcode = 1;
                            resp.errmsg = msg;
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                    }




                }
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

    }
}
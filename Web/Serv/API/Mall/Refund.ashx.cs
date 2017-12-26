using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    ///退款
    /// </summary>
    public class Refund : BaseHandlerNeedLogin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        /// <summary>
        /// 支付逻辑
        /// </summary>
        BllPay bllPay = new BllPay();


        BllScore bllScore = new BllScore();

        BllOrder bllOrder = new BllOrder();

        /// <summary>
        /// yike 
        /// </summary>
        Open.EZRproSDK.Client yikeClient = new Open.EZRproSDK.Client();
        /// <summary>
        /// 通用关系表
        /// </summary>
        BLLJIMP.BLLCommRelation bllCommRelation = new BLLJIMP.BLLCommRelation();
        /// <summary>
        /// 
        /// </summary>
        BLLTransfersAudit bllTran = new BLLTransfersAudit();
        /// <summary>
        /// 
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLPermission bllPer = new BLLPermission.BLLPermission();
        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string data = context.Request["data"];
            RefundRequestModel refundRequest;//订单模型
            try
            {
                refundRequest = ZentCloud.Common.JSONHelper.JsonToModel<RefundRequestModel>(data);


                //检查
                if (refundRequest.order_detail_id == 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "order_detail_id 参数不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                if (string.IsNullOrEmpty(refundRequest.refund_reason))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请填写理由";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                //if (refundRequest.refund_amount <= 0)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "refund_amount 参数不能小于等于0";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                //}
                if (string.IsNullOrEmpty(refundRequest.phone))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请填写手机号";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                if (bllMall.GetRefundInfoByOrderDetailId(refundRequest.order_detail_id) != null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "重复提交";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                //检查
                WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(refundRequest.order_detail_id);
                if (orderDetail == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "订单详情不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }


                var maxRefundInfo = GetMaxRefundInfoModel(refundRequest.order_detail_id.ToString());
                if (refundRequest.refund_amount > maxRefundInfo.max_refund_amount)
                {
                    resp.errcode = 1;
                    resp.errmsg = "最大退款金额不能大于" + maxRefundInfo.max_refund_amount + "元";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderDetail.OrderID);

                if (orderInfo.Status == "已取消"||orderInfo.Status=="待付款")
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权访问";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                if (orderInfo.OrderUserID != currentUserInfo.UserID)
                {
                    resp.errcode = 1;
                    resp.errmsg = "无权访问";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                if (bllMall.GetCount<WXMallOrderInfo>(string.Format("OrderType=0 And ParentOrderId='{0}'",orderInfo.OrderID))>0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "主订单不能申请退款";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }

                //判断是否已参与分佣金，已分佣的订单不能申请退款
                if (bllMall.HasOrderCommission(orderInfo.OrderID))
                {
                    resp.errcode = 1;
                    resp.errmsg = "订单已参与分佣，不能退款";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                //判断是否已经分积分，已经分积分的不能申请退款
                var lockScoreModel = bllScore.GetLockScoreByOrder(orderInfo.OrderID);
                if (lockScoreModel != null)
                {
                    if (lockScoreModel.LockStatus == 1)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "订单已返积分到账，不能退款";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                }

                //拆单的判断父订单是否已经分积分，已经分积分的不能申请退款

                if (!string.IsNullOrWhiteSpace(orderInfo.ParentOrderId))
                {
                    var lockScoreParentModel = bllScore.GetLockScoreByOrder(orderInfo.ParentOrderId);
                    if (lockScoreParentModel != null)
                    {
                        if (lockScoreParentModel.LockStatus == 1)
                        {
                            resp.errcode = 1;
                            resp.errmsg = "订单已返积分到账，不能退款";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }
                    }
                }

                #region 礼品订单
                if (orderInfo.OrderType == 1)
                {
                    if (string.IsNullOrEmpty(orderInfo.ParentOrderId))
                    {
                        //子订单已经发货,不能退款
                        if (bllMall.GetCount<WXMallOrderInfo>(string.Format(" ParentOrderId='{0}' And ExpressNumber!=''", orderInfo.OrderID)) > 0)
                        {
                            resp.errcode = 1;
                            resp.errmsg = "礼品已经发货,不能退款";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }


                    }
                    else//子订单不能退款
                    {
                        resp.errcode = 1;
                        resp.errmsg = "礼品订单不能退款";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }
                #endregion



                WXMallRefund model = new BLLJIMP.Model.WXMallRefund();
                model.RefundId = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmm"), bllMall.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.OrderDetailId = refundRequest.order_detail_id;
                model.UserId = currentUserInfo.UserID;
                model.RefundReason = refundRequest.refund_reason;
                model.RefundAmount = refundRequest.refund_amount;
                model.Phone = refundRequest.phone;
                model.Remark = refundRequest.remark;
                model.IsReturnProduct = refundRequest.is_return_product;
                model.ProductStatus = refundRequest.product_status;
                model.ProductName = orderDetail.ProductName;
                model.UpdateTime = DateTime.Now;
                //ProductSku productSku = bllMall.GetProductSku((int)orderDetail.SkuId);
                //if (productSku == null)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "sku已被删除";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //}
                //model.SkuSn = productSku.SkuSN;
                if (refundRequest.imagelist != null && refundRequest.imagelist.Length > 0)
                {
                    model.ImageList = string.Join(",", refundRequest.imagelist);
                }
                model.Status = 0;
                model.InsertDate = DateTime.Now;
                model.WebSiteOwner = bllMall.WebsiteOwner;
                model.OrderId = orderDetail.OrderID;
                model.OpenId = currentUserInfo.WXOpenId;
                model.RefundScore = refundRequest.refund_score;
                model.RefundAccountAmount = refundRequest.refund_account_amount;
                orderDetail.RefundStatus = model.Status.ToString();

                if (refundRequest.refund_score > 0 || refundRequest.refund_account_amount > 0)
                {

                    if (refundRequest.refund_score > maxRefundInfo.max_refund_score)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "最多退还积分不能大于" + maxRefundInfo.max_refund_score;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (refundRequest.refund_account_amount > maxRefundInfo.max_refund_account_amount)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "最多退还余额不能大于" + maxRefundInfo.max_refund_account_amount;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }
                model.IsContainTransportFee = maxRefundInfo.is_contain_transport_fee;
                if (!bllMall.Update(orderDetail))
                {
                    resp.errcode = 1;
                    resp.errmsg = "申请失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (bllMall.Add(model))
                {
                    string isReturnProduct = "并退货";
                    if (model.IsReturnProduct == 0)
                    {
                        isReturnProduct = "不退货";
                    }
                    resp.errmsg = "ok";
                    //插入维权记录
                    WXMallRefundLog log = new WXMallRefundLog();
                    log.OrderDetailId = model.OrderDetailId;
                    log.Role = "买家";
                    log.Title = "发起了退款申请，等待商家处理";
                    log.LogContent = string.Format("退款原因:{0}<br/>期望结果:我要退款,{1}<br/>退款金额:{2}元<br/>退还积分:{3}<br/>退还余额:{4}<br/>退款说明:{5}<br/>联系电话:{6},货物状态:{7}", model.RefundReason, isReturnProduct, model.RefundAmount, model.RefundScore, model.RefundAccountAmount, model.Remark, model.Phone, model.ProductStatus);
                    log.InsertDate = DateTime.Now;
                    log.WebSiteOwner = bllMall.WebsiteOwner;
                    bllMall.Add(log);
                    //插入维权记录

                    if (orderInfo.IsRefund == 0)
                    {
                        bllMall.Update(orderInfo, string.Format(" IsRefund=1"), string.Format(" OrderID='{0}'", orderInfo.OrderID));

                    }
                    var websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
                    if (websiteInfo.IsHaveUnReadRefundOrder == 0)
                    {
                        websiteInfo.IsHaveUnReadRefundOrder = 1;
                        bllMall.Update(websiteInfo);
                    }
                    try
                    {
                        if (bllCommRelation.ExistRelation(BLLJIMP.Enums.CommRelationType.SyncYike, bllCommRelation.WebsiteOwner, ""))
                        {
                            var result = yikeClient.Refund(model);
                        }
                    }
                    catch (Exception)
                    {


                    }


                    //暂时取消积分
                    if (lockScoreModel != null)
                    {
                        if (lockScoreModel.LockStatus == 0)
                        {
                            bllScore.CancelLockScoreByOrder(orderInfo.OrderID, DateTime.Now.ToString() + " 申请退款暂时取消积分");
                        }

                    }

                    if (model.IsReturnProduct == 0)//不退货
                    {
                        if (bllPer.CheckPermissionKey(orderInfo.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.PMS_TRANSFERSAUDIT))
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
                                resp.errmsg = "申请失败";
                                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                            }
                        }




                    }


                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "申请失败";
                }



            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取退款详细信息接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string orderDetailId = context.Request["order_detail_id"];
            var sourceData = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            if (sourceData == null)
            {
                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 1,
                    errmsg = "暂无退款申请"

                });
            }
            var data = new
            {
                order_id=sourceData.OrderId,
                refund_id = sourceData.RefundId,
                time = bllMall.GetTimeStamp(sourceData.InsertDate),
                refund_reason = sourceData.RefundReason,
                refund_status = sourceData.Status,
                refund_amount = sourceData.RefundAmount,
                refund_account_amount = sourceData.RefundAccountAmount,
                refund_score = sourceData.RefundScore,
                is_contain_transportfee = sourceData.IsContainTransportFee,
                refund_refusereason = sourceData.RefuseReason,
                refund_address = sourceData.RefundAddress,
                is_return_product = sourceData.IsReturnProduct,
                product_status = sourceData.ProductStatus,
                product_name=sourceData.ProductName
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

        }

        /// <summary>
        /// 获取最大退款金额
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMaxRefundAmount(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            //WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(int.Parse(orderDetailId));
            MaxRefundInfo model = GetMaxRefundInfoModel(orderDetailId);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                model.max_refund_amount,
                model.max_refund_score,
                model.max_refund_account_amount,
                model.is_contain_transport_fee
            });
            //return ZentCloud.Common.JSONHelper.ObjectToJson(new
            //{
            //    errcode = 0,
            //    errmsg = "ok",
            //    max_refund_amount = orderDetail.MaxRefundAmount

            //});

        }

        /// <summary>
        /// 修改退款信息(重新提交退款申请)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string data = context.Request["data"];
            RefundRequestModel refundRequest;//订单模型
            try
            {
                refundRequest = ZentCloud.Common.JSONHelper.JsonToModel<RefundRequestModel>(data);


                //检查
                if (refundRequest.order_detail_id == 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = "order_detail_id 参数不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                if (string.IsNullOrEmpty(refundRequest.refund_reason))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请填写理由";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                //if (refundRequest.refund_amount <= 0)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "refund_amount 参数不能小于等于0";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                //}
                if (string.IsNullOrEmpty(refundRequest.phone))
                {
                    resp.errcode = 1;
                    resp.errmsg = "请填写手机号";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                var maxRefundInfo = GetMaxRefundInfoModel(refundRequest.order_detail_id.ToString());
                if (refundRequest.refund_amount > maxRefundInfo.max_refund_amount)
                {
                    resp.errcode = 1;
                    resp.errmsg = "最大退款金额不能大于" + maxRefundInfo.max_refund_amount + "元";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (refundRequest.refund_score > 0 || refundRequest.refund_account_amount > 0)
                {

                    if (refundRequest.refund_score > maxRefundInfo.max_refund_score)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "最多退还积分不能大于" + maxRefundInfo.max_refund_score;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }
                    if (refundRequest.refund_account_amount > maxRefundInfo.max_refund_account_amount)
                    {
                        resp.errcode = 1;
                        resp.errmsg = "最多退还余额不能大于" + maxRefundInfo.max_refund_account_amount;
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }

                }

                //检查
                WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(refundRequest.order_detail_id);
                model.OrderDetailId = refundRequest.order_detail_id;
                model.UserId = currentUserInfo.UserID;
                model.RefundReason = refundRequest.refund_reason;
                model.RefundAmount = refundRequest.refund_amount;
                model.RefundScore = refundRequest.refund_score;
                model.RefundAccountAmount = refundRequest.refund_account_amount;
                model.Phone = refundRequest.phone;
                model.Remark = refundRequest.remark;
                model.IsReturnProduct = refundRequest.is_return_product;
                if (refundRequest.imagelist != null && refundRequest.imagelist.Length > 0)
                {
                    model.ImageList = string.Join(",", refundRequest.imagelist);
                }

                model.Status = 0;
                model.InsertDate = DateTime.Now;
                model.RefundAddress = "";
                model.RefuseReason = "";
                model.ExpressCompanyCode = "";
                model.ExpressCompanyName = "";
                model.ExpressNumber = "";
                model.ProductStatus = refundRequest.product_status;
                model.UpdateTime = DateTime.Now;
                WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(model.OrderDetailId);
                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderDetail.OrderID);
                orderDetail.RefundStatus = model.Status.ToString();
                if (!bllMall.Update(orderDetail))
                {
                    resp.errcode = 1;
                    resp.errmsg = "操作失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (bllMall.Update(model))
                {
                    string isReturnProduct = "并退货";
                    if (model.IsReturnProduct == 0)
                    {
                        isReturnProduct = "不退货";
                    }
                    resp.errmsg = "ok";
                    //插入维权记录
                    WXMallRefundLog log = new WXMallRefundLog();
                    log.OrderDetailId = model.OrderDetailId;
                    log.Role = "买家";
                    log.Title = "修改了退款申请，等待商家处理";
                    log.LogContent = string.Format("退款原因:{0}<br/>期望结果:我要退款,{1}<br/>退款金额:{2}元<br/>退还积分:{3}<br/>退还余额:{4}<br/>退款说明:{5}<br/>联系电话:{6},货物状态:{7}", model.RefundReason, isReturnProduct, model.RefundAmount, model.RefundScore, model.RefundAccountAmount, model.Remark, model.Phone, model.ProductStatus);
                    log.InsertDate = DateTime.Now;
                    log.WebSiteOwner = bllMall.WebsiteOwner;
                    bllMall.Add(log);
                    if (orderInfo.IsRefund == 0)
                    {
                        bllMall.Update(orderInfo, string.Format(" IsRefund=1"), string.Format(" OrderID='{0}'", orderInfo.OrderID));

                    }
                    var websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
                    if (websiteInfo.IsHaveUnReadRefundOrder == 0)
                    {
                        websiteInfo.IsHaveUnReadRefundOrder = 1;
                        bllMall.Update(websiteInfo);
                    }
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
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "申请失败";
                }



            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


        }


        /// <summary>
        /// 买家发货
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
                log.Role = "买家";
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
                            resp.errmsg = "发货失败";
                            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        }

                    }


                }
                catch (Exception)
                {


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
        /// 关闭退款申请
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Close(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            WXMallRefund model = bllMall.GetRefundInfoByOrderDetailId(int.Parse(orderDetailId));
            model.Status = 7;
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
                log.Role = "买家";
                log.Title = "关闭退款申请";
                log.LogContent = string.Format("买家已经关闭退款申请");
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

                WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(model.OrderId);
                orderInfo.IsRefund = 0;
                bllMall.Update(orderInfo);

                //查询订单是否还有退款，如果没有退款了，而且还有暂时取消的冻结积分记录，则反取消积分
                var lockScoreModel = bllScore.GetLockScoreByOrder(orderInfo.OrderID);
                if (lockScoreModel != null)
                {
                    if (lockScoreModel.LockStatus == 2 && bllOrder.GetOrderHasRefundCount(orderInfo.OrderID) == 0)
                    {
                        bllScore.UnCancelLockScoreByOrder(orderInfo.OrderID, DateTime.Now.ToString() + " 订单退款已全部关闭，恢复取消的即将到账积分 ");
                    }
                }

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "";
            }


            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);


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
        /// 查询退款状态
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
            PayConfig payConfig = bllPay.GetPayConfig();
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
            PayConfig payConfig = bllPay.GetPayConfig();
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
        /// 获取最多的退款信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMaxRefundInfo(HttpContext context)
        {
            string orderDetailId = context.Request["order_detail_id"];
            MaxRefundInfo model = GetMaxRefundInfoModel(orderDetailId);
            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                errcode = 0,
                model.max_refund_amount,
                model.max_refund_score,
                model.max_refund_account_amount,
                model.is_contain_transport_fee
            });
        }

        /// <summary>
        /// 获取最多的退款信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private MaxRefundInfo GetMaxRefundInfoModel(string orderDetailId)
        {
            MaxRefundInfo model = new MaxRefundInfo();
            WXMallOrderDetailsInfo orderDetail = bllMall.GetOrderDetail(int.Parse(orderDetailId));
            WXMallOrderInfo orderInfo = bllMall.GetOrderInfo(orderDetail.OrderID);
            List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            List<WXMallRefund> refundList = bllMall.GetList<WXMallRefund>(string.Format(" OrderId='{0}'", orderInfo.OrderID));

            var productTotalProce = ((decimal)orderDetailList.Sum(p => p.OrderPrice * p.TotalCount));
            decimal rate = 0;
            if (productTotalProce > 0)
                rate = ((decimal)orderDetail.OrderPrice * orderDetail.TotalCount) / ((decimal)orderDetailList.Sum(p => p.OrderPrice * p.TotalCount));//此商品所占的比例
            model.max_refund_amount = orderDetail.MaxRefundAmount;

            //商品单独需要的积分不在均摊计算范围内
            int singelProductTotalScore = orderDetail.OrderScore * orderDetail.TotalCount,
                allProductTotalScore = orderDetailList.Sum(p => p.OrderScore * p.TotalCount);

            //foreach (var item in orderDetailList)
            //{
            //    allProductTotalScore += item.OrderScore * item.TotalCount;
            //}

            orderInfo.UseScore -= allProductTotalScore;

            if (orderInfo.UseScore > 0)
            {
                model.max_refund_score = (int)(orderInfo.UseScore * rate);//可退积分
            }
            else
            {
                orderInfo.UseScore = 0;
            }

            if (orderInfo.UseAmount > 0)
            {
                model.max_refund_account_amount = Math.Round(orderInfo.UseAmount * rate, 2, MidpointRounding.AwayFromZero);//可退余额
            }
            if (orderDetailList.Count == 1 && (string.IsNullOrEmpty(orderInfo.ExpressNumber)))//只买了一种商品且未发货
            {
                //model.max_refund_amount += orderInfo.Transport_Fee;
                DealTransportFeeRefund(ref model, orderInfo, refundList);

                model.max_refund_score = orderInfo.UseScore;

                //if (orderInfo.UseScore > 0)
                //{
                //    if (refundList != null && refundList.Count > 0)
                //    {
                //        model.max_refund_score = orderInfo.UseScore - (refundList.Sum(p => p.RefundScore));
                //        if (model.max_refund_score < 0)
                //        {
                //            model.max_refund_score = 0;
                //        }
                //    }
                //}

                if ((bllMall.GetCount<WXMallRefund>(string.Format(" OrderId='{0}'", orderInfo.OrderID)) == orderDetailList.Count - 1) && (string.IsNullOrEmpty(orderInfo.ExpressNumber)))//只有一种商品且未发货
                {
                    model.is_contain_transport_fee = 1;
                }
            }
            else
            {
                #region 处理最后一种商品的情况
                if ((refundList != null) && (refundList.Count == (orderDetailList.Count - 1)))//最后一种商品
                {
                    if (string.IsNullOrEmpty(orderInfo.ExpressNumber))//最后一件商品未发货
                    {
                        //model.max_refund_amount += orderInfo.Transport_Fee;
                        DealTransportFeeRefund(ref model, orderInfo, refundList);

                        model.is_contain_transport_fee = 1;

                    }

                    #region 最后一种商品的退还积分
                    if (orderInfo.UseScore > 0)
                    {

                        if (refundList != null && refundList.Count > 0)
                        {

                            model.max_refund_score = orderInfo.UseScore - (refundList.Sum(p => p.RefundScore));
                            if (model.max_refund_score < 0)
                            {
                                model.max_refund_score = 0;
                            }

                        }
                    }
                    #endregion

                    #region 最后一种商品的余额退款
                    if (orderInfo.UseAmount > 0)
                    {

                        if (refundList != null && refundList.Count > 0)
                        {
                            model.max_refund_account_amount = orderInfo.UseAmount - (refundList.Sum(p => p.RefundAccountAmount));
                            if (model.max_refund_account_amount < 0)
                            {
                                model.max_refund_account_amount = 0;
                            }

                        }

                    }
                    #endregion

                }
                #endregion
            }

            if (singelProductTotalScore > 0)
            {
                model.max_refund_score += singelProductTotalScore;
            }

            //如果最大退款金额比实付金额还大，则说明有问题，最大退款金额重置为 0，在处理老的订单运费逻辑，重置为0是正确的，其他情况如有不对根据具体情况再分析

            if (model.max_refund_amount > orderInfo.TotalAmount + orderInfo.Transport_Fee)
            {
                model.max_refund_amount = 0;
            }

            return model;

        }

        /// <summary>
        /// 处理最后一件运费退款情况
        /// </summary>
        /// <param name="model"></param>
        /// <param name="orderInfo"></param>
        /// <param name="refundList"></param>
        /// <param name="overTransportFee"></param>
        /// <param name="canRefundAmount"></param>
        /// <param name="canRefundAccountAmount"></param>
        private void DealTransportFeeRefund(
                ref MaxRefundInfo model,
                WXMallOrderInfo orderInfo,
                List<WXMallRefund> refundList

            )
        {
            var overTransportFee = orderInfo.Transport_Fee;//当前已计算抵掉的运费
            var canRefundAmount = orderInfo.TotalAmount - refundList.Sum(p => p.RefundAmount);//当前可退最大金额
            var canRefundAccountAmount = orderInfo.UseAmount - (refundList.Sum(p => p.RefundAccountAmount));//当前可退最大余额

            //把金额、余额、积分都退掉，并且保证可退金额跟余额不大于邮费，剩余的积分则可以全部都退掉
            if (overTransportFee > 0)
            {
                if (canRefundAmount >= overTransportFee)
                {
                    model.max_refund_amount += overTransportFee;
                    overTransportFee = 0;
                }
                else
                {
                    model.max_refund_amount += canRefundAmount;
                    overTransportFee -= canRefundAmount;
                }
            }
            if (overTransportFee > 0)
            {
                if (canRefundAccountAmount >= overTransportFee)
                {
                    model.max_refund_account_amount += overTransportFee;
                    overTransportFee = 0;
                }
                else
                {
                    model.max_refund_account_amount += canRefundAccountAmount;
                    overTransportFee -= canRefundAccountAmount;
                }
            }

        }

        /// <summary>
        /// 请求退款模型
        /// </summary>
        public class RefundRequestModel
        {
            /// <summary>
            /// 订单详情Id
            /// </summary>
            public int order_detail_id { get; set; }
            /// <summary>
            /// 退款原因
            /// </summary>
            public string refund_reason { get; set; }
            /// <summary>
            /// 退款金额
            /// </summary>
            public decimal refund_amount { get; set; }
            /// <summary>
            ///是否退货 1退款并退货0退款不退货 
            /// </summary>
            public int is_return_product { get; set; }
            /// <summary>
            /// 手机号码
            /// </summary>
            public string phone { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 图片例证列表
            /// </summary>
            public string[] imagelist { get; set; }
            /// <summary>
            /// 货物状态
            /// 未收到货
            /// 已收到货
            /// </summary>
            public string product_status { get; set; }
            /// <summary>
            /// 退还积分
            /// </summary>
            public decimal refund_score { get; set; }
            /// <summary>
            /// 退还余额
            /// </summary>
            public decimal refund_account_amount { get; set; }


        }

        /// <summary>
        /// 最大退款信息模型
        /// </summary>
        public class MaxRefundInfo
        {

            /// <summary>
            /// 最大退款金额
            /// </summary>
            public decimal max_refund_amount { get; set; }
            /// <summary>
            /// 最大退还余额
            /// </summary>
            public decimal max_refund_account_amount { get; set; }
            /// <summary>
            /// 最大退还积分
            /// </summary>
            public decimal max_refund_score { get; set; }
            /// <summary>
            /// 是否包含运费
            /// </summary>
            public int is_contain_transport_fee { get; set; }


        }







    }
}
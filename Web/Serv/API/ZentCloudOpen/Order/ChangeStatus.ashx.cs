using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Order
{
    /// <summary>
    /// 修改订单状态
    /// </summary>
    public class ChangeStatus : BaseHanderOpen
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
        ///微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        public void ProcessRequest(HttpContext context)
        {

            string orderSn = context.Request["order_sn"];//订单号
            string status = context.Request["status"];//订单状态 1001 确认收货 1002取消订单
            if (string.IsNullOrEmpty(orderSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(status))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "status 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, bllUser.WebsiteOwner);
            switch (status)
            {


                #region 确认收货

                case "1001"://确认收货
                    if (orderInfo.Status == "交易成功")
                    {
                        resp.code = (int)APIErrCode.OperateFail;
                        resp.msg = "重复操作";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (orderInfo.PaymentStatus == 0)
                    {
                        resp.code = (int)APIErrCode.OperateFail;
                        resp.msg = "订单未付款";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    if (orderInfo.Status=="已取消")
                    {
                        resp.code = (int)APIErrCode.OperateFail;
                        resp.msg = "订单已取消";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                    //交易成功处理
                    try
                    {
                        orderInfo.Status = "交易成功";
                        orderInfo.ReceivingTime = DateTime.Now;
                        if (!bllMall.Update(orderInfo, tran))
                        {
                            tran.Rollback();
                            resp.code = (int)APIErrCode.OperateFail;
                            resp.msg = "更新订单状态失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }

                        #region 交易成功加积分
                        //增加积分
                        ScoreConfig scoreConfig = bllScore.GetScoreConfig();
                        int addScore = 0;
                        if (scoreConfig != null && scoreConfig.OrderAmount > 0 && scoreConfig.OrderScore > 0)
                        {
                            addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                        }
                        if (addScore > 0)
                        {
                            orderUserInfo.TotalScore += addScore;

                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = addScore;
                            scoreRecord.TotalScore = orderUserInfo.TotalScore;
                            scoreRecord.ScoreType = "OrderSuccess";
                            scoreRecord.UserID = orderUserInfo.UserID;
                            scoreRecord.AddNote = "微商城-交易成功获得积分";
                            scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                            if (!bllMall.Add(scoreRecord, tran))
                            {
                                tran.Rollback();
                                resp.code = (int)APIErrCode.OperateFail;
                                resp.msg = "插入积分记录表失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }
                            if (bllUser.Update(orderUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 1)
                            {
                                tran.Rollback();
                                resp.code = (int)APIErrCode.OperateFail;
                                resp.msg = "更新用户积分失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }




                        }
                        #endregion

                        //

                        #region 更新订单明细表状态
                        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        foreach (var orderDetail in orderDetailList)
                        {
                            orderDetail.IsComplete = 1;
                            if (!bllMall.Update(orderDetail))
                            {
                                tran.Rollback();
                                resp.code = (int)APIErrCode.OperateFail;
                                resp.msg = "更新订单明细表失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }

                        }
                        #endregion




                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        resp.code = (int)APIErrCode.OperateFail;
                        resp.msg = ex.Message;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }

                    tran.Commit();
                    resp.code = 0;
                    resp.msg = "ok";
                    resp.status = true;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                //交易成功处理
                #endregion

                #region 取消订单
                case "1002"://取消订单
                    if (orderInfo.Status == "已取消")
                    {
                        resp.code = (int)APIErrCode.OperateFail;
                        resp.msg = "重复操作";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;
                    }
                    ZentCloud.ZCBLLEngine.BLLTransaction tranCancel = new ZCBLLEngine.BLLTransaction();
                    try
                    {

                        orderInfo.Status = "已取消";
                        if (!bllMall.Update(orderInfo, tranCancel))
                        {
                            tranCancel.Rollback();
                            resp.code = (int)APIErrCode.OperateFail; ;
                            resp.msg = "更新订单状态失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }

                        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        foreach (var orderDetail in orderDetailList)
                        {

                            if (orderDetail.SkuId != null)
                            {
                                ProductSku sku = bllMall.GetProductSku((int)orderDetail.SkuId);
                                if (sku != null)
                                {
                                    if (bllMall.Update(sku, string.Format(" Stock+={0}", orderDetail.TotalCount), string.Format(" SkuId={0}", sku.SkuId), tranCancel) == 0)
                                    {
                                        tranCancel.Rollback();
                                        resp.code = (int)APIErrCode.OperateFail; ;
                                        resp.msg = "修改sku库存失败";
                                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                        return;
                                    }
                                }
                            }


                        }
                        if (bllMall.Update(new WXMallOrderDetailsInfo(), "IsComplete=0", string.Format(" OrderId='{0}'", orderInfo.OrderID), tranCancel) <= 0)
                        {
                            tranCancel.Rollback();
                            resp.code = (int)APIErrCode.OperateFail; ;
                            resp.msg = "更新订单详情失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }



                        #region 积分返还
                        if (orderInfo.UseScore > 0)//使用积分 积分返还
                        {
                            orderUserInfo.TotalScore += orderInfo.UseScore;
                            if (bllUser.Update(orderUserInfo,
                                string.Format(" TotalScore+={0}", orderInfo.UseScore),
                                string.Format(" AutoID={0}", orderUserInfo.AutoID) ) < 0)
                             
                            {
                                tranCancel.Rollback();
                                resp.code = (int)APIErrCode.OperateFail; ;
                                resp.msg = "积分返还失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = orderInfo.UseScore;
                            scoreRecord.TotalScore = orderUserInfo.TotalScore;
                            scoreRecord.ScoreType = "OrderCancel";
                            scoreRecord.UserID = orderUserInfo.UserID;
                            scoreRecord.RelationID = orderInfo.OrderID;
                            scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                            scoreRecord.AddNote = "微商城-订单取消返还积分";
                            if (!bllMall.Add(scoreRecord))
                            {
                                tranCancel.Rollback();
                                resp.code = (int)APIErrCode.OperateFail; ;
                                resp.msg = "插入积分记录失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }
                        }
                        #endregion

                        #region 优惠券返还
                        if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
                        {

                            var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderUserInfo.UserID);
                            if (myCardCoupon != null && myCardCoupon.Status == 1)
                            {
                                myCardCoupon.Status = 0;
                                if (!bllCardCoupon.Update(myCardCoupon))
                                {

                                    tranCancel.Rollback();
                                    resp.code = (int)APIErrCode.OperateFail; ;
                                    resp.msg = "优惠券更新失败";
                                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                }

                            }

                        }
                        #endregion

                        #region 账户余额返还

                        if (orderInfo.UseAmount > 0)
                        {
                            orderUserInfo.AccountAmount += orderInfo.UseAmount;
                            if (bllMall.Update(orderUserInfo, string.Format(" AccountAmount={0}", orderUserInfo.AccountAmount), string.Format(" AutoID={0}", orderUserInfo.AutoID)) < 0)
                            {
                                tranCancel.Rollback();
                                resp.code = (int)APIErrCode.OperateFail; ;
                                resp.msg = "更新用户余额失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }

                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.Score = (double)orderInfo.UseAmount;
                            scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
                            scoreRecord.ScoreType = "AccountAmount";
                            scoreRecord.UserID = orderUserInfo.UserID;
                            scoreRecord.RelationID = orderInfo.OrderID;
                            scoreRecord.WebSiteOwner = bllMall.WebsiteOwner;
                            scoreRecord.AddNote = "微商城-订单取消返还余额";
                            if (!bllMall.Add(scoreRecord))
                            {
                                tranCancel.Rollback();
                                resp.code = (int)APIErrCode.OperateFail; ;
                                resp.msg = "插入余额记录失败";
                                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                                return;
                            }
                        }


                        #endregion

                        tranCancel.Commit();
                        resp.code = 0;
                        resp.msg = "ok";
                        resp.status = true;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }
                    catch (Exception ex)
                    {
                        tranCancel.Rollback();
                        resp.code = (int)APIErrCode.OperateFail; ;
                        resp.msg = ex.Message;
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                        return;

                    }

                #endregion

                default:
                    resp.code = (int)APIErrCode.OperateFail;
                    resp.msg = "不合法的 status 值";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
            }



        }




    }
}
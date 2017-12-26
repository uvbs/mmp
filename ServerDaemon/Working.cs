using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLPermission;
using Newtonsoft.Json.Linq;

namespace ServerDaemon
{
    public class Working
    {
        /// <summary>
        /// 主窗体
        /// </summary>
        FormMain formMain = new FormMain();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="form"></param>
        public Working(FormMain form)
        {
            formMain = form;
        }
        /// <summary>
        /// 是否正在启动
        /// </summary>
        public static bool IsWorking = false;
        /// <summary>
        ///处理线程
        /// </summary>
        Thread thWork;
        /// <summary>
        /// 商城任务线程
        /// </summary>
        Thread thWorkMall;
        /// <summary>
        ///同步微信粉丝线程
        /// </summary>
        Thread thSynWeixinFans;
        /// <summary>
        ///同步所有站点微信粉丝线程
        /// </summary>
        //Thread thSynAllWebsiteWeixinFans;
        /// <summary>
        ///同步分销会员线程
        /// </summary>
        Thread thSynDistributionMember;
        /// <summary>
        ///同步分销销售额线程
        /// </summary>
        Thread thSynDistributionSaleAmount;
        /// <summary>
        /// 更新渠道数据 线程
        /// </summary>
        Thread thFlashChannelData;
        /// <summary>
        ///会员清洗线程
        /// </summary>
        Thread thCleanUser;
        /// <summary>
        ///获取我的分销二维码
        /// </summary>
        Thread thGetDistributionImage;
        /// <summary>
        /// 同步商品销量
        /// </summary>
        Thread thSynProductSaleCount;
        /// <summary>
        /// 商城统计
        /// </summary>
        Thread thMallStatistics;
        /// <summary>
        /// 商城统计 订单及商品
        /// </summary>
        Thread thMallStatisticsOrderProduct;
        /// <summary>
        /// 商城统计 
        /// 供应商月结算任务
        /// 渠道供应月结算任务
        /// 添加任务
        /// </summary>
        Thread thSupplierSettlementTask;
        /// <summary>
        /// 商城统计 供应商月结算
        /// </summary>
        Thread thSupplierSettlement;
        /// <summary>
        /// 商城统计 供应商渠道月结算
        /// </summary>
        Thread thSupplierChannelSettlement;
        /// <summary>
        /// 定时发送短息
        /// </summary>
        Thread thTimingSendSms;
        /// <summary>
        /// 自动好评
        /// </summary>
        Thread thAutoComment;

        /// <summary>
        /// 积分统计任务
        /// </summary>
        Thread thScoreStatis;
        /// <summary>
        /// 线程间隔
        /// </summary>
        public static int WorkInterval = 5000;
        ///// <summary>
        ///// 商城任务线程间隔 2小时执行一次
        ///// </summary>
        //public static int WorkIntervalMall = 3600000;

        public static int WorkIntervalMall = 5000;

        /// <summary>
        /// 系统日志
        /// </summary>
        public static string AppLog = "";
        /// <summary>
        /// 打印日志标识
        /// </summary>
        public static bool isToLog = true;


        /// <summary>
        /// 开始处理
        /// </summary>
        public void StartWork()
        {
            ZentCloud.ZCDALEngine.DALEngine.GetMetas();//获取表结构
            IsWorking = true;

            ToLog("开启任务...");
            thWork = new Thread(new ThreadStart(ExecuteTask));
            thWork.Start();

            ToLog("开启商城任务...");
            thWorkMall = new Thread(new ThreadStart(ExecuteMallTask));
            thWorkMall.Start();

            ToLog("开启同步微信粉丝任务...");
            thSynWeixinFans = new Thread(new ThreadStart(ExecuteSynWeixinFansTask));
            thSynWeixinFans.Start();

            //ToLog("开启同步微信粉丝任务...");
            //thSynAllWebsiteWeixinFans = new Thread(new ThreadStart(ExecuteSynAllWebsiteWeixinFansTask));
            //thSynAllWebsiteWeixinFans.Start();


            ToLog("开启同步分销会员任务...");
            thSynDistributionMember = new Thread(new ThreadStart(ExecuteSynDistributionMemberTask));
            thSynDistributionMember.Start();

            ToLog("开启同步分销销售额任务...");
            thSynDistributionSaleAmount = new Thread(new ThreadStart(ExecuteSynDistributionSaleAmountTask));
            thSynDistributionSaleAmount.Start();

            ToLog("开启更新渠道数据任务...");
            thFlashChannelData = new Thread(new ThreadStart(ExecuteFlashChannelDataTask));
            thFlashChannelData.Start();

            ToLog("开启会员清洗任务...");
            thCleanUser = new Thread(new ThreadStart(ExecuteCleanUserTask));
            thCleanUser.Start();

            ToLog("开启分销二维码任务...");
            thGetDistributionImage = new Thread(new ThreadStart(ExecuteGetDistributionImageTask));
            thGetDistributionImage.Start();

            ToLog("开启同步销量任务...");
            thSynProductSaleCount = new Thread(new ThreadStart(ExecutethSynProductSaleCountTask));
            thSynProductSaleCount.Start();

            ToLog("开启商城统计任务...");
            thMallStatistics = new Thread(new ThreadStart(ExecutethMallStatisticsTask));
            thMallStatistics.Start();

            ToLog("开启商城统计任务...");
            thMallStatisticsOrderProduct = new Thread(new ThreadStart(ExecuteStatisticsOrderProductTask));
            thMallStatisticsOrderProduct.Start();


            ToLog("开启供应商结算任务...");
            thSupplierSettlementTask = new Thread(new ThreadStart(AddSupplierSettlementTask));
            thSupplierSettlementTask.Start();

            ToLog("供应商结算任务...");
            thSupplierSettlement = new Thread(new ThreadStart(ExecuteSupplierSettlementTask));
            thSupplierSettlement.Start();

            ToLog("供应商渠道结算任务...");
            thSupplierChannelSettlement = new Thread(new ThreadStart(ExecuteSupplierChannelSettlementTask));
            thSupplierChannelSettlement.Start();

            ToLog("开启发送短信");
            thTimingSendSms = new Thread(new ThreadStart(TimingSandSms));
            thTimingSendSms.Start();

            ToLog("开启自动好评");
            thAutoComment = new Thread(new ThreadStart(OrderAutoComment));
            thAutoComment.Start();

            ToLog("开启积分统计");
            thScoreStatis = new Thread(new ThreadStart(ExecuteScoreStatisTask));
            thScoreStatis.Start();

        }

        /// <summary>
        /// 停止处理
        /// </summary>
        public void StopWork()
        {
            try
            {
                ToLog("结束定时任务...");
                IsWorking = false;
                thWork.Abort();
                thWorkMall.Abort();
                thSynWeixinFans.Abort();
                thSynDistributionMember.Abort();
                thSynDistributionSaleAmount.Abort();
                thCleanUser.Abort();
                thGetDistributionImage.Abort();
                thSynProductSaleCount.Abort();
                thMallStatistics.Abort();
                thMallStatisticsOrderProduct.Abort();

            }
            catch (Exception ex)
            {
                WriteLog("异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        private void ExecuteTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            BLLWeixin bllWeiXin = new BLLWeixin();
            BLLUser bllUser = new BLLUser();
            while (IsWorking)
            {
                try
                {



                    #region 积分通知 已注释
                    ////执行定时积分账户通知业务
                    //task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SendTMUserScoreNotify, BLLTimingTask.TaskStatus.Waiting);
                    //if (task != null && task.ScheduleDate <= DateTime.Now)
                    //{
                    //    task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                    //    bllTimingTask.Update(task);

                    //    bllTimingTask.ExecuteTimingTask(task);

                    //    task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                    //    task.FinishDate = DateTime.Now;
                    //    bllTimingTask.Update(task);
                    //}
                    #endregion

                    #region 消息群发每月4次
                    //执行定时群发接口发消息
                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SendMassMessage, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                    }
                    #endregion


                    ToLog("处理客服消息");
                    #region 客服消息

                    bllWeiXin.SendMessageToKeFuTask();
                    bllWeiXin.SendMessageToUserTask();
                    #endregion


                    #region 群发图文
                    task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SendImageTextMessage, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        ToLog(string.Format("获取到图文群发任务" + task.TaskInfo));
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("开始群发图文任务" + task.TaskInfo));
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("群发图文任务完成" + task.TaskInfo));

                    }
                    #endregion

                    ToLog("发送模板消息(批量)");
                    #region 发送模板消息
                    task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SendTemplateMessage, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        bllWeiXin.SendTemplateMessageTask(task);
                    }
                    #endregion

                    ToLog("发送模板消息(全部)");
                    #region 发送模板消息
                    task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SendTemplateMessageAll, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        bllWeiXin.SendTemplateMessageTask(task);
                    }
                    #endregion
                    //执行定时微信客服接口群发任务
                    ToLog("获取任务...");

                    #region 分佣返利解冻

                    #endregion

                }
                catch (Exception ex)
                {
                    ToLog(ex.Message);
                    WriteLog("异常" + ex.ToString());
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }

        /// <summary>
        /// 执行商城相关任务
        /// </summary>
        private void ExecuteMallTask()
        {

            BLLMall bllMall = new BLLMall();
            BLLUser bllUser = new BLLUser();
            BllScore bllScore = new BllScore();
            BLLCardCoupon bllCardCoupon = new BLLCardCoupon();
            // BllPay bllPay = new BllPay();
            BLLMenuPermission bllMenuPermission = new BLLMenuPermission("");
            BLLDistribution bllDistribution = new BLLDistribution();
            BLLWeixin bllWeixin = new BLLWeixin();
            BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();

            BLLWebSite bllWebsite = new BLLWebSite();

            //string msg1 = "";
            //var orderInfo1=bllMall.GetOrderInfo("2250143");
            //if (orderInfo1.Status=="待付款")
            //{
            //    if (bllMall.CancelOrder(orderInfo1, out msg1))
            //    {
            //        UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo1.OrderUserID, orderInfo1.WebsiteOwner);
            //        string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", bllWebsiteDomain.GetWebsiteDoMain(orderInfo1.WebsiteOwner), orderInfo1.OrderID);
            //        bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "订单超过时间未支付，已自动取消", string.Format("订单号:{0}\\n点击详情查看", orderInfo1.OrderID), url, "", "", "", orderInfo1.WebsiteOwner);

            //        WriteLog(string.Format("取消订单成功,订单号:{0}", orderInfo1.OrderID));
            //    }
            //    else
            //    {
            //        WriteLog(string.Format("取消订单失败,订单号:{0},原因:{1}", orderInfo1.OrderID, msg1));
            //    }
            //}

            while (IsWorking)
            {
                try
                {


                    #region 普通订单自动收货
                    ToLog("微商城自动收货...");
                    var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" Status='已发货' And PayMentStatus=1 And DATEDIFF(day,DeliveryTime,GetDate())>=7 And WebsiteOwner!='mixblu'   And WebsiteOwner!='jikuwifi' And OrderType in(0,1,2) "));
                    ToLog(string.Format("共获取到{0}笔订单,开始自动收货...", orderList.Count));
                    DateTime dtNow = DateTime.Now;
                    int index = 1;
                    int successCount = 0;
                    foreach (var orderInfo in orderList)
                    {


                        ToLog(string.Format("正在处理第{0}笔订单,共{1}笔...", index, orderList.Count));
                        UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                        if (orderUserInfo == null)
                        {
                            continue;
                        }
                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                        try
                        {
                            //WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                            if (bllMall.Update(orderInfo, " Status='交易成功',ReceivingTime=GetDate(),DistributionStatus=2", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
                            {
                                tran.Rollback();
                                continue;
                            }

                            //下单立即增加冻结积分
                            ////增加冻结积分
                            //if (!bllScore.AddLockScoreByOrder(orderInfo, tran))
                            //{
                            //    tran.Rollback();
                            //    continue;
                            //}

                            //增加积分
                            //ScoreConfig scoreConfig = bllScore.Get<ScoreConfig>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));

                            //if (scoreConfig != null && scoreConfig.OrderScore>0)
                            //{
                            //    if (scoreConfig.OrderAmount == 0)
                            //    {
                            //        scoreConfig.OrderAmount = 1;
                            //    }
                            //    int addScore = (int)(orderInfo.TotalAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                            //    if (addScore > 0)
                            //    {

                            //        orderUserInfo.TotalScore += addScore;

                            //        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            //        scoreRecord.AddTime = DateTime.Now;
                            //        scoreRecord.Score = addScore;
                            //        scoreRecord.TotalScore = orderUserInfo.TotalScore;
                            //        scoreRecord.ScoreType = "OrderSuccess";
                            //        scoreRecord.UserID = orderInfo.OrderUserID;
                            //        scoreRecord.AddNote = "微商城-交易成功获得积分";
                            //        scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;

                            //        if (!bllMall.Add(scoreRecord, tran))
                            //        {
                            //            tran.Rollback();
                            //            continue;
                            //        }
                            //        if (bllUser.Update(orderUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) <= 0)
                            //        {
                            //            tran.Rollback();
                            //            continue;
                            //        }

                            //        #region 宏巍加积分
                            //        if (websiteInfo.IsUnionHongware == 1)
                            //        {
                            //            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);
                            //            var hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                            //            if (hongWareMemberInfo.member != null)
                            //            {
                            //                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile,orderUserInfo.WXOpenId, addScore))
                            //                {
                            //                    tran.Rollback();
                            //                    continue;

                            //                }


                            //            }

                            //        }
                            //        #endregion


                            //    }
                            //}

                            //更新订单明细表状态
                            //List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                            //foreach (var orderDetail in orderDetailList)
                            //{
                            //orderDetail.IsComplete = 1;

                            if (bllMall.Update(new WXMallOrderDetailsInfo(), "IsComplete=1,CompleteTime=GetDate()", string.Format(" OrderId='{0}'", orderInfo.OrderID)) <= 0)
                            {
                                tran.Rollback();
                                continue;
                            }

                            //}


                        }
                        catch (Exception ex)
                        {
                            WriteLog("异常" + ex.ToString());
                            tran.Rollback();
                            continue;

                        }
                        tran.Commit();
                        //#region 更新销量
                        //foreach (var orderDetail in bllMall.GetOrderDetailsList(orderInfo.OrderID))
                        //{
                        //    int saleCount = bllMall.GetProductSaleCount(int.Parse(orderDetail.PID));
                        //    bllMall.Update(new WXMallProductInfo(), string.Format("SaleCount={0}", saleCount), string.Format(" PID='{0}'", orderDetail.PID));

                        //}
                        //#endregion

                        #region 微信通知
                        try
                        {

                            string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", bllWebsiteDomain.GetWebsiteDoMain(orderInfo.WebsiteOwner), orderInfo.OrderID);
                            bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "订单已自动确认收货", string.Format("订单号:{0}\\n点击详情查看", orderInfo.OrderID), url, "", "", "", orderInfo.WebsiteOwner);
                        }
                        catch
                        {


                        }
                        #endregion

                        successCount++;
                        WriteLog(string.Format("自动收货完成订单号:{0} WebsiteOwner:{1}", orderInfo.OrderID, orderInfo.WebsiteOwner));


                    }
                    #endregion

                    #region 普通订单自动取消
                    ToLog("微商城订单自动取消");
                    foreach (var orderInfo in bllMall.GetList<WXMallOrderInfo>(string.Format(" Status='待付款' And PayMentStatus=0 And DATEDIFF(minute,InsertDate,GetDate())>=2  And WebsiteOwner!='jikuwifi' And WebsiteOwner!='mixblu'  And OrderType in(0,1) And IsNull(IsMain,0)=1")))
                    {
                        WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                        if (websiteInfo == null)
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(websiteInfo.OrderCancelMinute) && (websiteInfo.OrderCancelMinute != "0"))
                        {

                            if (orderInfo.InsertDate.AddMinutes(int.Parse(websiteInfo.OrderCancelMinute)) > dtNow)
                            {
                                continue;
                            }

                        }
                        if (orderInfo.InsertDate.AddHours(2) > dtNow)//默认2小时
                        {
                            continue;
                        }


                        orderInfo.Status = "己取消";
                        bllMall.Update(orderInfo);
                    }
                    var orderListCancel = bllMall.GetList<WXMallOrderInfo>(string.Format(" Status='待付款' And PayMentStatus=0 And DATEDIFF(minute,InsertDate,GetDate())>=2  And WebsiteOwner!='jikuwifi' And WebsiteOwner!='mixblu'  And OrderType in(0,1) And IsNull(IsMain,0)=0"));
                    ToLog(string.Format("共获取到{0}笔订单,开始自动取消...", orderListCancel.Count));
                    //WriteLog(string.Format("共获取到{0}笔订单,开始自动取消...", orderListCancel.Count));
                    index = 1;
                    foreach (var orderInfo in orderListCancel)
                    {

                        try
                        {
                            WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", orderInfo.WebsiteOwner));
                            if (websiteInfo == null)
                            {
                                continue;
                            }
                            if (!string.IsNullOrEmpty(websiteInfo.OrderCancelMinute) && (websiteInfo.OrderCancelMinute != "0"))
                            {

                                if (orderInfo.InsertDate.AddMinutes(int.Parse(websiteInfo.OrderCancelMinute)) > dtNow)
                                {
                                    continue;
                                }

                            }
                            if (orderInfo.InsertDate.AddHours(2) > dtNow)//默认2小时
                            {
                                //10   12>    11
                                continue;
                            }
                            //WriteLog(string.Format("正在处理第{0}笔订单,共{1}笔...", index, orderListCancel.Count));
                            ToLog(string.Format("正在处理第{0}笔订单,共{1}笔...", index, orderListCancel.Count));

                            string msg = "";
                            if (bllMall.CancelOrder(orderInfo, out msg))
                            {
                                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                                string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", bllWebsiteDomain.GetWebsiteDoMain(orderInfo.WebsiteOwner), orderInfo.OrderID);
                                bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "订单超过时间未支付，已自动取消", string.Format("订单号:{0}\\n点击详情查看", orderInfo.OrderID), url, "", "", "", orderInfo.WebsiteOwner);

                                WriteLog(string.Format("取消订单成功,订单号:{0}", orderInfo.OrderID));
                            }
                            else
                            {
                                WriteLog(string.Format("取消订单失败,订单号:{0},原因:{1}", orderInfo.OrderID, msg));
                            }
                        }
                        catch (Exception ex)
                        {

                            WriteLog("取消订单异常:" + ex.ToString());
                            continue;
                        }


                        //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                        //if (orderUserInfo == null)
                        //{
                        //    continue;
                        //}
                        //ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                        //try
                        //{

                        //    if (bllMall.Update(orderInfo, " Status='已取消'", string.Format(" OrderId='{0}'", orderInfo.OrderID), tran) <= 0)
                        //    {

                        //        tran.Rollback();
                        //        continue;
                        //    }

                        //    #region 库存返还
                        //    List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        //    foreach (var orderDetail in orderDetailList)
                        //    {

                        //        if (orderDetail.SkuId != null)
                        //        {
                        //            ProductSku sku = bllMall.GetProductSku((int)orderDetail.SkuId);
                        //            if (sku != null)
                        //            {
                        //                if (bllMall.Update(sku, string.Format(" Stock+={0}", orderDetail.TotalCount), string.Format(" SkuId={0}", sku.SkuId), tran) == 0)
                        //                {
                        //                    tran.Rollback();
                        //                    continue;

                        //                }
                        //            }
                        //        }
                        //    }
                        //    #endregion

                        //    #region 积分返还
                        //    if (orderInfo.UseScore > 0)//使用积分 积分返还
                        //    {
                        //        orderUserInfo.TotalScore += orderInfo.UseScore;
                        //        if (bllUser.Update(orderUserInfo,
                        //            string.Format(" TotalScore+={0}", orderInfo.UseScore),
                        //            string.Format(" AutoID={0}", orderUserInfo.AutoID),
                        //            tran) < 0)
                        //        {
                        //            tran.Rollback();
                        //            continue;
                        //        }
                        //        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        //        scoreRecord.AddTime = DateTime.Now;
                        //        scoreRecord.Score = orderInfo.UseScore;
                        //        scoreRecord.TotalScore = orderUserInfo.TotalScore;
                        //        scoreRecord.ScoreType = "OrderCancel";
                        //        scoreRecord.UserID = orderUserInfo.UserID;
                        //        scoreRecord.RelationID = orderInfo.OrderID;
                        //        scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                        //        scoreRecord.AddNote = "微商城-订单取消返还积分";
                        //        bllMall.Add(scoreRecord, tran);

                        //        #region 宏巍加积分
                        //        if (websiteInfo.IsUnionHongware == 1)
                        //        {
                        //            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);

                        //            var hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                        //            if (hongWareMemberInfo.member != null)
                        //            {
                        //                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, orderInfo.UseScore))
                        //                {
                        //                    tran.Rollback();
                        //                    continue;

                        //                }


                        //            }

                        //        }
                        //        #endregion

                        //        if (orderInfo.WebsiteOwner == "mixblu")
                        //        {
                        //            /// <summary>
                        //            /// 驿氪  
                        //            /// </summary>
                        //            Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
                        //            if (orderInfo.UseScore > 0)
                        //            {
                        //                var result = yiKeClient.BonusUpdate(orderUserInfo.Ex2, orderInfo.UseScore, string.Format("订单:{0}取消返还{1}积分", orderInfo.OrderID, orderInfo.UseScore));
                        //                WriteLog("退还驿氪积分: " + ZentCloud.Common.JSONHelper.ObjectToJson(result));

                        //            }
                        //            var orderResult = yiKeClient.ChangeStatus(orderInfo.OrderID, "已取消");
                        //            WriteLog("更新驿氪订单状态: " + ZentCloud.Common.JSONHelper.ObjectToJson(orderResult));
                        //        }

                        //    }
                        //    #endregion

                        //    //冻结积分取消
                        //    bllScore.CancelLockScoreByOrder(orderInfo.OrderID, "取消订单，积分取消", tran);

                        //    #region 优惠券储值卡返还
                        //    if (!string.IsNullOrEmpty(orderInfo.MyCouponCardId))
                        //    {

                        //        var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(orderInfo.MyCouponCardId), orderUserInfo.UserID);
                        //        if (myCardCoupon != null && myCardCoupon.Status == 1)
                        //        {
                        //            myCardCoupon.Status = 0;
                        //            if (!bllCardCoupon.Update(myCardCoupon, tran))
                        //            {

                        //                tran.Rollback();
                        //                continue;

                        //            }

                        //        }
                        //        else
                        //        {
                        //            bllMall.Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'", orderInfo.OrderID, tran));
                        //        }

                        //    }
                        //    #endregion

                        //    #region 账户余额返还

                        //    if (orderInfo.UseAmount > 0)
                        //    {
                        //        orderUserInfo.AccountAmount += orderInfo.UseAmount;
                        //        if (bllMall.Update(
                        //            orderUserInfo,
                        //            string.Format(" AccountAmount+={0}", orderInfo.UseAmount),
                        //            string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) < 0
                        //            )
                        //        {
                        //            tran.Rollback();
                        //            continue;
                        //        }

                        //        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        //        scoreRecord.AddTime = DateTime.Now;
                        //        scoreRecord.Score = (double)orderInfo.UseAmount;
                        //        scoreRecord.TotalScore = (double)orderUserInfo.AccountAmount;
                        //        scoreRecord.UserID = orderUserInfo.UserID;
                        //        scoreRecord.RelationID = orderInfo.OrderID;
                        //        scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                        //        scoreRecord.AddNote = "微商城-订单取消返还余额";
                        //        scoreRecord.ScoreType = "AccountAmount";
                        //        if (!bllMall.Add(scoreRecord, tran))
                        //        {
                        //            tran.Rollback();
                        //            continue;
                        //        }
                        //        #region 宏巍加余额
                        //        if (websiteInfo.IsUnionHongware == 1)
                        //        {
                        //            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(orderInfo.WebsiteOwner);

                        //            var hongWareMemberInfo = hongWareClient.GetMemberInfo(orderUserInfo.WXOpenId);
                        //            if (hongWareMemberInfo.member != null)
                        //            {
                        //                if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile, orderUserInfo.WXOpenId, (float)orderInfo.UseAmount))
                        //                {
                        //                    tran.Rollback();
                        //                    continue;

                        //                }


                        //            }

                        //        }
                        //        #endregion



                        //    }


                        //    #endregion

                        //    //

                        //    #region 微信通知
                        //    try
                        //    {

                        //        string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", bllWebsiteDomain.GetWebsiteDoMain(orderInfo.WebsiteOwner), orderInfo.OrderID);
                        //        bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "订单超过时间未支付，已自动取消", string.Format("订单号:{0}\\n点击详情查看", orderInfo.OrderID), url, "", "", "", orderInfo.WebsiteOwner);

                        //    }
                        //    catch
                        //    {


                        //    }
                        //    #endregion
                        //    WriteLog(string.Format("自动取消订单完成订单号:{0} WebsiteOwner:{1}", orderInfo.OrderID, orderInfo.WebsiteOwner));
                        //    index++;



                        //}
                        //catch (Exception ex)
                        //{
                        //    WriteLog("异常" + ex.ToString());
                        //    tran.Rollback();
                        //    continue;

                        //}
                        //tran.Commit();

                        //#region 代付返还
                        //if (!string.IsNullOrEmpty(orderInfo.OtherInfo))
                        //{
                        //    foreach (var item in orderInfo.OtherInfo.Split('|'))
                        //    {
                        //        if (!string.IsNullOrEmpty(item))
                        //        {
                        //            //var otherInfo = new
                        //            //{
                        //            //    use_amount = orderRequestModel.use_amount,
                        //            //    cardcoupon_id = orderRequestModel.cardcoupon_id,
                        //            //    cardcoupon_disamount = discountAmount,
                        //            //    user_id = currentUserInfo.UserID

                        //            //};
                        //            JToken token = JToken.Parse(item);
                        //            if (decimal.Parse(token["use_amount"].ToString()) > 0)
                        //            {
                        //                UserInfo userInfo = bllUser.GetUserInfo(token["user_id"].ToString(), orderInfo.WebsiteOwner);
                        //                if (bllMall.Update(
                        //                    userInfo,
                        //                    string.Format(" AccountAmount+={0}", decimal.Parse(token["use_amount"].ToString())),
                        //                    string.Format(" AutoID={0}", userInfo.AutoID), tran) >0
                        //                    )
                        //                {


                        //                }
                        //                UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                        //                scoreRecord.AddTime = DateTime.Now;
                        //                scoreRecord.Score = double.Parse(token["use_amount"].ToString());
                        //                scoreRecord.TotalScore = (double)userInfo.AccountAmount;
                        //                scoreRecord.UserID = userInfo.UserID;
                        //                scoreRecord.RelationID = orderInfo.OrderID;
                        //                scoreRecord.WebSiteOwner = orderInfo.WebsiteOwner;
                        //                scoreRecord.AddNote = "微商城-订单取消返还余额";
                        //                scoreRecord.ScoreType = "AccountAmount";
                        //                if (bllMall.Add(scoreRecord))
                        //                {


                        //                }
                        //            }
                        //            if (int.Parse(token["cardcoupon_id"].ToString()) > 0)
                        //            {
                        //                bllMall.Delete(new StoredValueCardUseRecord(), string.Format("OrderId='{0}'", orderInfo.OrderID));

                        //            }


                        //        }
                        //    }


                        //}
                        //#endregion

                    }
                    #endregion

                    #region 商城订单自动分佣
                    ToLog("微商城自动分佣...");
                    var orderListFenxiao = bllMall.GetList<WXMallOrderInfo>(string.Format("  PaymentStatus=1 And DATEDIFF(day,ReceivingTime,GetDate())>=7 And WebsiteOwner!='mixblu'     And WebsiteOwner!='jikuwifi' And OrderType in(0,1,2) And Status='交易成功' And DistributionStatus!=3 And IsNull(IsMain,0)=0"));
                    foreach (var orderInfo in orderListFenxiao)
                    {

                        ToLog(string.Format("正在处理第{0}笔订单,共{1}笔...", index, orderListFenxiao.Count));

                        if (bllMenuPermission.CheckUserAndPmsKey(orderInfo.WebsiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.OnlineDistribution, orderInfo.WebsiteOwner))
                        {
                            WebsiteInfo websiteInfo = bllMall.GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);
                            if (websiteInfo.IsDisabledCommission == 1)
                            {
                                WriteLog(string.Format("站点已禁用分佣,WebsiteOwner:{0}:订单号{1}", orderInfo.WebsiteOwner, orderInfo.OrderID));
                                continue;
                            }
                            string msg = "";
                            try
                            {
                                if (bllDistribution.Transfers(orderInfo, out msg))
                                {
                                    orderInfo.DistributionStatus = 3;
                                    if (bllMall.Update(orderInfo))
                                    {
                                        WriteLog(string.Format("自动分佣完成。订单号:{0} WebsiteOwner:{1}", orderInfo.OrderID, orderInfo.WebsiteOwner));
                                    }

                                }
                                else
                                {
                                    WriteLog(string.Format("自动分佣失败!订单号:{0}原因{1}", orderInfo.OrderID, msg));
                                }
                            }
                            catch (Exception ex)
                            {

                                WriteLog(string.Format("自动分佣失败!订单号:{0}原因{1}", orderInfo.OrderID, ex.ToString()));
                                continue;
                            }



                        }

                    }
                    #endregion

                    #region 处理未拆单的即将到账积分
                    //处理即将到账积分：七天后到账，积分到账后不允许退款，解冻积分，积分入账
                    ToLog("处理即将到账积分...");
                    var orderListLockScore = bllMall.GetList<WXMallOrderInfo>(string.Format(" DATEDIFF(day,ReceivingTime,GetDate())>=7 And WebsiteOwner!='mixblu' And WebsiteOwner!='jikuwifi' And OrderType in(0,1,2,3) And Status='交易成功' AND  OrderID in ( select ForeignkeyId from ZCJ_ScoreLockInfo  where LockStatus = 0 and LockType = 1 ) "));
                    var orderListLockScoreIndex = 1;

                    foreach (var orderInfo in orderListLockScore)
                    {
                        string msg = "";

                        bllScore.SettlementOrderLockScore(orderInfo, out msg);

                        WriteLog("处理即将到账积分:" + msg);

                        orderListLockScoreIndex++;

                    }
                    #endregion

                    #region 处理拆单的即将到账积分

                    /* 处理拆单的即将到账积分
                    
                     1.处理成功到账：确认收货7天后，没有取消的子订单订单，没有退款处理中或者退款成功的订单；
                    
                     2.处理可以取消积分的订单：有取消的子订单，有退款成功的子订单
                     
                     */


                    //1.处理成功到账：确认收货7天后，没有取消的子订单订单，没有退款处理中或者退款成功的订单；

                    //获取可以到账的订单
                    StringBuilder strChildSuccessOrderList = new StringBuilder();
                    strChildSuccessOrderList.Append("select * from");
                    strChildSuccessOrderList.Append("(");
                    strChildSuccessOrderList.Append(" select orderid,");
                    strChildSuccessOrderList.Append("  (");
                    strChildSuccessOrderList.Append("  select COUNT(*) from ZCJ_WXMallOrderInfo b where b.ParentOrderId=a.OrderID ");
                    strChildSuccessOrderList.Append("  and b.Status='交易成功' ");
                    strChildSuccessOrderList.Append("  and DATEDIFF(day,ReceivingTime,GetDate())>=7 ");
                    strChildSuccessOrderList.Append("  and not exists (select 1 from ZCJ_WXMallRefund r where r.orderid=b.OrderID ");
                    strChildSuccessOrderList.Append("  and r.Status not in (7,8))");
                    strChildSuccessOrderList.Append("  ) as childordercount");
                    strChildSuccessOrderList.Append("  from ZCJ_WXMallOrderInfo a");
                    strChildSuccessOrderList.Append("  where a.ismain=1 ");
                    strChildSuccessOrderList.Append(")");
                    strChildSuccessOrderList.Append("as #tmp ");
                    strChildSuccessOrderList.Append("where childordercount > 0");//查出交易成功七天并且没有退款中和退款成功的订单

                    //查出存在冻结积分的订单
                    strChildSuccessOrderList.Append(" AND  OrderID in ( select ForeignkeyId from ZCJ_ScoreLockInfo  where LockStatus = 0 and LockType = 1 ) ");

                    foreach (var item in BLLMall.Query<Model.ChildOrderCountModel>(strChildSuccessOrderList.ToString()))
                    {
                        //判断交易成功七天而且没有退款的子订单等于该订单的所有子订单，则可以结算积分
                        var totalChildCount = bllMall.GetCount<WXMallOrderInfo>(string.Format(" ParentOrderId='{0}' ", item.OrderId));

                        if (totalChildCount == item.ChildOrderCount)
                        {
                            string msg = "";
                            var orderInfo = bllMall.GetOrderInfo(item.OrderId);
                            bllScore.SettlementOrderLockScore(orderInfo, out msg);
                            WriteLog("处理拆单的即将到账积分:" + msg);
                        }
                    }


                    //2.处理可以取消积分的订单：有取消的子订单，有退款成功的子订单则直接取消积分
                    StringBuilder strChildCancelOrderList = new StringBuilder();
                    strChildCancelOrderList.Append("select * from");
                    strChildCancelOrderList.Append("(");
                    strChildCancelOrderList.Append("  select orderid,");
                    strChildCancelOrderList.Append("  (");
                    strChildCancelOrderList.Append("  select COUNT(*) from ZCJ_WXMallOrderInfo b where b.ParentOrderId=a.OrderID");
                    strChildCancelOrderList.Append("  and ( b.Status='已取消'");
                    strChildCancelOrderList.Append("  or exists (select 1 from ZCJ_WXMallRefund r where r.orderid=b.OrderID ");
                    strChildCancelOrderList.Append("  and r.Status in (6)");
                    strChildCancelOrderList.Append("  )) ");
                    strChildCancelOrderList.Append("  ) as childordercount");
                    strChildCancelOrderList.Append("  from [ZCJ_WXMallOrderInfo] a");
                    strChildCancelOrderList.Append("  where a.ismain=1");
                    strChildCancelOrderList.Append(")");
                    strChildCancelOrderList.Append("as #tmp1 where childordercount > 0");//查出有已取消的或者有退款成功的子订单

                    //查出存在冻结积分的订单
                    strChildCancelOrderList.Append(" AND  OrderID in ( select ForeignkeyId from ZCJ_ScoreLockInfo  where LockStatus = 0 and LockType = 1 ) ");

                    foreach (var item in BLLMall.Query<Model.ChildOrderCountModel>(strChildCancelOrderList.ToString()))
                    {
                        bllScore.CancelLockScoreByOrder(item.OrderId, "有已取消的或者有退款成功的子订单");
                    }

                    #endregion

                    #region 团购订单自动取消
                    var orderListGroupBuyParent = bllMall.GetList<WXMallOrderInfo>(string.Format(" OrderType=2 And Paymentstatus=1     And ( GroupBuyStatus=0 or GroupBuyStatus = '' or GroupBuyStatus is null ) And (GroupBuyParentOrderId='' or GroupBuyParentOrderId  Is Null Or GroupBuyParentOrderId=OrderId) And (DATEADD(day,ExpireDay,[PayTime] )<=GetDate())"));//团长订单
                    ToLog(string.Format("共获取到{0}条数据,开始处理...", orderListGroupBuyParent.Count));
                    foreach (var headOrder in orderListGroupBuyParent)
                    {
                        //ZentCloud.BLLJIMP.Model.PayConfig payConfig = bllPay.Get<PayConfig>(string.Format(" WebsiteOwner='{0}'", headOrder.WebsiteOwner));
                        //if (DateTime.Now >= ((DateTime)headOrder.PayTime).AddDays(headOrder.ExpireDay))
                        //{

                        #region 取消团购订单
                        foreach (var order in bllMall.GetList<WXMallOrderInfo>(string.Format(" OrderType='2' And Paymentstatus=1 And (GroupBuyParentOrderId='{0}' Or OrderId='{0}')", headOrder.OrderID)))//团长订单
                        {
                            UserInfo userInfo = bllUser.GetUserInfo(order.OrderUserID, order.WebsiteOwner);
                            //string msg = "";
                            //string weixinRefundId = "";
                            //bool isSuccess;
                            //WriteLog(string.Format(" 开始处理订单:订单号{0}", order.OrderID));
                            //if (order.TotalAmount > 0)
                            //{
                            //    bllPay.WeixinRefund(order.OrderID, order.OrderID, order.TotalAmount, order.TotalAmount, payConfig.WXAppId, payConfig.WXMCH_ID, payConfig.WXPartnerKey, out msg, out weixinRefundId, headOrder.WebsiteOwner);
                            //}
                            //if (isSuccess)
                            //{
                            order.GroupBuyStatus = "2";//2为失败
                            order.Status = "团购失败";
                            if (bllMall.Update(order))
                            {

                                //
                                #region 积分返还
                                if (order.UseScore > 0)//使用积分 积分返还
                                {
                                    userInfo.TotalScore += order.UseScore;
                                    if (bllUser.Update(userInfo,
                                        string.Format(" TotalScore+={0}", order.UseScore),
                                        string.Format(" AutoID={0}", userInfo.AutoID)) > 0)
                                    {
                                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                        scoreRecord.AddTime = DateTime.Now;
                                        scoreRecord.Score = order.UseScore;
                                        scoreRecord.TotalScore = userInfo.TotalScore;
                                        scoreRecord.ScoreType = "OrderCancel";
                                        scoreRecord.UserID = userInfo.UserID;
                                        scoreRecord.RelationID = order.OrderID;
                                        scoreRecord.WebSiteOwner = order.WebsiteOwner;
                                        scoreRecord.AddNote = "团购-团购失败返还积分";
                                        bllMall.Add(scoreRecord);
                                    }

                                    #region 宏巍加积分
                                    WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", order.WebsiteOwner));
                                    if (websiteInfo.IsUnionHongware == 1)
                                    {
                                        Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(headOrder.WebsiteOwner);

                                        var hongWareMemberInfo = hongWareClient.GetMemberInfo(userInfo.WXOpenId);
                                        if (hongWareMemberInfo.member != null)
                                        {
                                            if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, userInfo.WXOpenId, order.UseScore))
                                            {



                                            }


                                        }

                                    }
                                    #endregion


                                }
                                #endregion

                                //冻结积分取消
                                bllScore.CancelLockScoreByOrder(order.OrderID, "取消订单，积分取消");

                                #region 优惠券返还
                                if (!string.IsNullOrEmpty(order.MyCouponCardId))
                                {

                                    var myCardCoupon = bllCardCoupon.GetMyCardCoupon(int.Parse(order.MyCouponCardId), userInfo.UserID);
                                    if (myCardCoupon != null && myCardCoupon.Status == 1)
                                    {
                                        myCardCoupon.Status = 0;
                                        if (bllCardCoupon.Update(myCardCoupon))
                                        {


                                        }

                                    }

                                }
                                #endregion

                                #region 账户余额返还

                                if (order.UseAmount > 0)
                                {
                                    userInfo.AccountAmount += order.UseAmount;
                                    if (bllMall.Update(userInfo, string.Format(" AccountAmount={0}", userInfo.AccountAmount), string.Format(" AutoID={0}", userInfo.AutoID)) < 0)
                                    {
                                        UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                                        scoreRecord.AddTime = DateTime.Now;
                                        scoreRecord.Score = (double)order.UseAmount;
                                        scoreRecord.TotalScore = (double)userInfo.AccountAmount;
                                        scoreRecord.UserID = userInfo.UserID;
                                        scoreRecord.RelationID = order.OrderID;
                                        scoreRecord.WebSiteOwner = order.WebsiteOwner;
                                        scoreRecord.AddNote = "团购-团购失败返余额";
                                        scoreRecord.ScoreType = "AccountAmount";
                                        if (!bllMall.Add(scoreRecord))
                                        {

                                        }
                                        #region 宏巍加余额
                                        WebsiteInfo websiteInfo = bllMall.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", order.WebsiteOwner));
                                        if (websiteInfo.IsUnionHongware == 1)
                                        {
                                            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(headOrder.WebsiteOwner);

                                            var hongWareMemberInfo = hongWareClient.GetMemberInfo(userInfo.WXOpenId);
                                            if (hongWareMemberInfo.member != null)
                                            {
                                                if (!hongWareClient.UpdateMemberBlance(hongWareMemberInfo.member.mobile, userInfo.WXOpenId, (float)order.UseAmount))
                                                {


                                                }


                                            }

                                        }
                                        #endregion

                                    }





                                }


                                #endregion
                                //
                                #region 微信通知
                                try
                                {

                                    var orderUserInfo = bllUser.GetUserInfo(order.OrderUserID, order.WebsiteOwner);
                                    string url = string.Format("http://{0}/customize/shop/?v=1.0&ngroute=/orderDetail/{1}#/orderDetail/{1}", bllWebsiteDomain.GetWebsiteDoMain(order.WebsiteOwner), order.OrderID);
                                    bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "团购订单失败", string.Format("订单号:{0}\\n点击详情查看", order.OrderID), url, "", "", "", order.WebsiteOwner);

                                }
                                catch
                                {


                                }
                                #endregion
                                WriteLog(string.Format(" 取消团购订单成功,订单号：{0}WebsiteOwner:{1}", order.OrderID, order.WebsiteOwner));
                            }
                            else
                            {
                                WriteLog(string.Format(" 取消团购订单失败,订单号：{0}", order.OrderID));
                            }
                            //}
                            //else
                            //{

                            //    WriteLog(string.Format(" 团购订单退款失败:订单号{0} 原因:{1}", order.OrderID, msg));
                            //}

                            bllMall.Update(new WXMallOrderDetailsInfo(), "IsComplete=0", string.Format(" OrderId='{0}'", order.OrderID));

                        }
                        #endregion


                        //}

                    }
                    #endregion

                    #region 自动关闭退款
                    foreach (var item in bllMall.GetList<WXMallRefund>(string.Format(" Status=2")))
                    {
                        var companyConfig = bllWebsite.Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", item.WebSiteOwner));
                        if (companyConfig != null)
                        {
                            if (companyConfig.IsAutoCloseRefund == 1 && companyConfig.AutoCloseRefundDay > 0)
                            {
                                if (item.UpdateTime != null)
                                {
                                    if (Convert.ToDateTime(item.UpdateTime).AddDays(companyConfig.AutoCloseRefundDay)<= DateTime.Now)
                                    {

                                        ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                                        try
                                        {
                                            item.Status = 7;
                                            if (!bllMall.Update(item, tran))
                                            {
                                                tran.Rollback();
                                                WriteLog("更新退款单号失败,退款单号:" + item.RefundId);
                                            }

                                            var orderDetail = bllMall.GetOrderDetail(item.OrderDetailId);
                                            orderDetail.RefundStatus = "7";
                                            if (!bllMall.Update(orderDetail, tran))
                                            {
                                                tran.Rollback();
                                                WriteLog("更新订单详情失败,订单详情Id:" + orderDetail.AutoID);
                                            }
                                            tran.Commit();

                                            List<WXMallRefund> refundList = bllMall.GetList<WXMallRefund>(string.Format(" OrderId='{0}'", item.OrderId));

                                            var totalCount = refundList.Count(p => p.Status == 0)
                                            + refundList.Count(p => p.Status == 1)
                                                //+ refundList.Count(p => p.Status == 2)
                                            + refundList.Count(p => p.Status == 3)
                                            + refundList.Count(p => p.Status == 4)
                                            + refundList.Count(p => p.Status == 5);//退款中的数量
                                            if (totalCount == 0)
                                            {
                                                //此订单再无退款申请
                                                if (bllMall.Update(new WXMallOrderInfo(), string.Format(" IsRefund=0"), string.Format(" OrderId='{0}'", item.OrderId)) <= 0)
                                                {
                                                    WriteLog(string.Format("更新订单退款状态失败:订单号" + item.OrderId));
                                                }

                                            }


                                        }
                                        catch (Exception ex)
                                        {

                                            tran.Rollback();
                                            WriteLog(ex.ToString());
                                        }


                                    }

                                }


                            }

                        }


                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    ToLog(ex.ToString());
                    WriteLog("异常" + ex.ToString());
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkIntervalMall / 1000));
                Thread.Sleep(WorkIntervalMall);
            }
        }


        /// <summary>
        /// 执行同步微信粉丝任务
        /// </summary>
        private void ExecuteSynWeixinFansTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            BLLWeixin bllWeiXin = new BLLWeixin();
            while (IsWorking)
            {
                try
                {

                    ToLog("同步微信粉丝");
                    #region 同步微信粉丝
                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SynFans, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        //bllWeiXin.UpdateAllFollowersInfoTask(task);
                        var th = new Thread(new ParameterizedThreadStart(ExecuteSynWeixinFans));
                        th.Start(task);

                    }



                    #endregion


                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }

        /// <summary>
        /// 执行同步所有站点微信粉丝任务
        /// </summary>
        private void ExecuteSynAllWebsiteWeixinFansTask()
        {

            while (IsWorking)
            {
                try
                {
                    if (DateTime.Now.Hour == 3)
                    {
                        BLLWeixin bllWeiXin = new BLLWeixin();
                        bllWeiXin.UpdateAllWesiteAllFollowersInfoTask();

                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());

                }

                Thread.Sleep(1000 * WorkInterval);
            }
        }

        /// <summary>
        /// 执行同步分销会员任务
        /// </summary>
        private void ExecuteSynDistributionMemberTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();

            while (IsWorking)
            {
                try
                {

                    ToLog("同步分销会员");
                    #region 同步分销会员
                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SynDistributionMember, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        ToLog(string.Format("同步分销会员任务" + task.TaskInfo));
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步分销会员任务" + task.TaskInfo));
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步分销会员任务" + task.TaskInfo));

                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }

        /// <summary>
        /// 执行同步分销销售额任务
        /// </summary>
        private void ExecuteSynDistributionSaleAmountTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();

            while (IsWorking)
            {
                try
                {

                    ToLog("同步分销销售额");

                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SynDistributionSaleAmount, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        ToLog(string.Format("同步分销销售额" + task.TaskInfo));
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步分销销售额" + task.TaskInfo));
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步分销销售额" + task.TaskInfo));

                    }





                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }


        /// <summary>
        /// 执行更新渠道数据任务
        /// </summary>
        private void ExecuteFlashChannelDataTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();

            while (IsWorking)
            {
                try
                {

                    ToLog("同步渠道数据");

                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.FlashChannelData, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        ToLog(string.Format("同步渠道数据" + task.TaskInfo));
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步渠道数据" + task.TaskInfo));
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("同步渠道数据" + task.TaskInfo));

                    }





                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }
        /// <summary>
        /// 执行会员清洗任务
        /// </summary>
        private void ExecuteCleanUserTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                try
                {

                    ToLog("开始会员清洗");
                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.CleanUser, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        try
                        {
                            ToLog(string.Format("会员清洗" + task.TaskInfo));
                            task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                            bllTimingTask.Update(task);
                            ToLog(string.Format("会员清洗" + task.TaskInfo));
                            bllTimingTask.ExecuteTimingTask(task);

                        }
                        catch (Exception ex)
                        {
                            WriteLog("异常" + ex.ToString());
                            task.TaskInfo += ex.Message;

                        }
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                        ToLog(string.Format("会员清洗" + task.TaskInfo));

                    }





                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }


        /// <summary>
        /// 获取分销二维码图片任务
        /// </summary>
        private void ExecuteGetDistributionImageTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                try
                {

                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.GetDistributionImage, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        try
                        {
                            task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                            bllTimingTask.Update(task);
                            bllTimingTask.ExecuteTimingTask(task);

                        }
                        catch (Exception ex)
                        {
                            WriteLog("异常" + ex.ToString());
                            task.TaskInfo += ex.Message;

                        }
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);

                    }



                }
                catch (Exception ex)
                {
                    WriteLog("异常" + ex.ToString());
                    ToLog(ex.Message);
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }

        /// <summary>
        /// 同步销量
        /// </summary>
        private void ExecutethSynProductSaleCountTask()
        {
            while (IsWorking)
            {
                try
                {


                    if (DateTime.Now.Hour == 3)
                    {
                        StringBuilder sbSql = new StringBuilder();

                        //更新总销量
                        sbSql.Append(" Update [dbo].[ZCJ_WXMallProductInfo] ");
                        sbSql.Append(" Set [SaleCount]=isnull(");
                        sbSql.Append(" (select sum(TotalCount) from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1)");
                        sbSql.Append(" ,0)");
                        sbSql.Append(" FROM [dbo].[ZCJ_WXMallProductInfo]");
                        sbSql.Append(" where exists  (select 1 from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 );");
                        //更新总销量

                        //更新最近一个月销量
                        sbSql.Append(" Update [dbo].[ZCJ_WXMallProductInfo] ");
                        sbSql.Append(" Set [SaleCountOneMonth]=isnull(");
                        sbSql.Append(" (select sum(TotalCount) from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 And (CompleteTime Between  dateadd(mm,-1,GetDate()) And GetDate()))");
                        sbSql.Append(" ,0)");
                        sbSql.Append(" FROM [dbo].[ZCJ_WXMallProductInfo]");
                        sbSql.Append(" where exists  (select 1 from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 );");
                        //更新最近一个月销量

                        //更新最近三个月销量
                        sbSql.Append(" Update [dbo].[ZCJ_WXMallProductInfo] ");
                        sbSql.Append(" Set [SaleCountThreeMonth]=isnull(");
                        sbSql.Append(" (select sum(TotalCount) from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 And (CompleteTime Between  dateadd(mm,-3,GetDate()) And GetDate()))");
                        sbSql.Append(" ,0)");
                        sbSql.Append(" FROM [dbo].[ZCJ_WXMallProductInfo]");
                        sbSql.Append(" where exists  (select 1 from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 );");
                        //更新最近三个月销量

                        //更新最近六个月销量
                        sbSql.Append(" Update [dbo].[ZCJ_WXMallProductInfo] ");
                        sbSql.Append(" Set [SaleCountHalfYear]=isnull(");
                        sbSql.Append(" (select sum(TotalCount) from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 And (CompleteTime Between  dateadd(mm,-6,GetDate()) And GetDate()))");
                        sbSql.Append(" ,0)");
                        sbSql.Append(" FROM [dbo].[ZCJ_WXMallProductInfo]");
                        sbSql.Append(" where exists  (select 1 from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 );");
                        //更新最近六个月销量

                        //更新最近1年销量
                        sbSql.Append(" Update [dbo].[ZCJ_WXMallProductInfo] ");
                        sbSql.Append(" Set [SaleCountOneYear]=isnull(");
                        sbSql.Append(" (select sum(TotalCount) from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 And (CompleteTime Between  dateadd(yy,-1,GetDate()) And GetDate()))");
                        sbSql.Append(" ,0)");
                        sbSql.Append(" FROM [dbo].[ZCJ_WXMallProductInfo]");
                        sbSql.Append(" where exists  (select 1 from [dbo].ZCJ_WXMallOrderDetailsInfo where PID=[dbo].[ZCJ_WXMallProductInfo].[PID] And IsComplete=1 );");
                        //更新最近1年销量

                        ZentCloud.ZCDALEngine.DALEngine.ExecuteSql(sbSql.ToString());

                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());

                }
                Thread.Sleep(20 * 60 * 1000);
            }
        }


        /// <summary>
        /// 商城统计
        /// </summary>
        private void ExecutethMallStatisticsTask()
        {
            BLLMall bllMall = new BLLMall();
            while (IsWorking)
            {
                try
                {
                    if (DateTime.Now.Hour == 3)
                    {

                        bllMall.Statistics();
                    }
                }
                catch (Exception ex)
                {

                    WriteLog(ex.ToString());
                }

                Thread.Sleep(20 * 60 * 1000);
            }
        }

        /// <summary>
        /// 同步粉丝
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSynWeixinFans(object obj)
        {
            try
            {
                TimingTask task = new TimingTask();
                task = (TimingTask)obj;
                new BLLWeixin().UpdateAllFollowersInfoTask(task);

            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());

            }



        }


        /// <summary>
        /// 统计订单及商品
        /// </summary>
        private void ExecuteStatisticsOrderProductTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {

                try
                {


                    var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.StatisticsOrderProduct, BLLTimingTask.TaskStatus.Waiting);
                    if (task != null && task.ScheduleDate <= DateTime.Now)
                    {
                        task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                        bllTimingTask.Update(task);
                        bllTimingTask.ExecuteTimingTask(task);
                        task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                        task.FinishDate = DateTime.Now;
                        bllTimingTask.Update(task);
                    }
                }
                catch (Exception ex)
                {


                }


                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }
        }

        /// <summary>
        /// 添加供应商结算任务
        /// 添加供应商渠道结算任务
        /// </summary>
        private void AddSupplierSettlementTask()
        {
            BLLMall bll = new BLLMall();
            while (IsWorking)
            {
                if (DateTime.Now.Day == 1)//每月1号生成
                {


                    DateTime dtLastMonthFirstDay = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1);
                    dtLastMonthFirstDay = Convert.ToDateTime(dtLastMonthFirstDay.ToString("yyyy-MM-dd"));  //上个月第一天


                    DateTime dtLastMonthLastDay = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                    dtLastMonthLastDay = Convert.ToDateTime(dtLastMonthLastDay.ToString("yyyy-MM-dd")).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);//上个月最后一天


                    string fromDate = dtLastMonthFirstDay.ToString("yyyy-MM-dd HH:mm:ss");
                    string toDate = dtLastMonthLastDay.ToString("yyyy-MM-dd HH:mm:ss");
                    if (bll.GetCount<TimingTask>(string.Format(" TaskType=13 And FromDate='{0}' And ToDate='{1}'", fromDate, toDate)) == 0)
                    {
                        TimingTask task = new TimingTask();
                        task.WebsiteOwner = "common";
                        task.InsertDate = DateTime.Now;
                        task.ScheduleDate = DateTime.Now;
                        task.FromDate = dtLastMonthFirstDay; ;
                        task.ToDate = dtLastMonthLastDay;
                        task.TaskType = 13;
                        task.Status = 1;
                        bll.Add(task);
                    }
                    if (bll.GetCount<TimingTask>(string.Format(" TaskType=14 And FromDate='{0}' And ToDate='{1}'", fromDate, toDate)) == 0)
                    {
                        TimingTask task = new TimingTask();
                        task.WebsiteOwner = "common";
                        task.InsertDate = DateTime.Now;
                        task.ScheduleDate = DateTime.Now;
                        task.FromDate = dtLastMonthFirstDay; ;
                        task.ToDate = dtLastMonthLastDay;
                        task.TaskType = 14;
                        task.Status = 1;
                        bll.Add(task);
                    }


                }

                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }




        }
        /// <summary>
        /// 执行供应商结算任务
        /// </summary>
        private void ExecuteSupplierSettlementTask()
        {
            //BLLMall bllMall = new BLLMall();
            //string msg = "";
            //bllMall.SupplierSettlement("songhe", "2017-04-01 00:00:00", "2017-04-30 23:59:59", out msg);
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                ToLog("查找任务..");
                var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SupplierSettlement, BLLTimingTask.TaskStatus.Waiting);
                if (task != null && task.ScheduleDate <= DateTime.Now)
                {
                    ToLog("有任务");
                    task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                    bllTimingTask.Update(task);
                    ToLog("更新任务状态完成");
                    try
                    {
                        bllTimingTask.ExecuteTimingTask(task);
                    }
                    catch (Exception ex)
                    {

                        ToLog(ex.ToString());
                        WriteLog(ex.ToString());
                    }
                    task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                    task.FinishDate = DateTime.Now;
                    bllTimingTask.Update(task);


                }
                else
                {
                    ToLog("无任务");
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }




        }


        /// <summary>
        /// 执行供应商渠道结算任务
        /// </summary>
        private void ExecuteSupplierChannelSettlementTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                ToLog("查找任务..");
                var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.SupplierChannelSettlement, BLLTimingTask.TaskStatus.Waiting);
                if (task != null && task.ScheduleDate <= DateTime.Now)
                {
                    ToLog("有任务");
                    task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                    bllTimingTask.Update(task);
                    ToLog("更新任务状态完成");
                    try
                    {
                        bllTimingTask.ExecuteTimingTask(task);
                    }
                    catch (Exception ex)
                    {

                        ToLog(ex.ToString());
                        WriteLog(ex.ToString());
                    }
                    task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                    task.FinishDate = DateTime.Now;
                    bllTimingTask.Update(task);


                }
                else
                {
                    ToLog("无任务");
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }




        }
        /// <summary>
        /// 
        /// </summary>
        private void TimingSandSms()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                BLLSMS bllSms = new BLLSMS("");

                try
                {
                    bllSms.SendTimingSms();
                }
                catch (Exception ex)
                {
                    ToLog("短信提示异常:" + ex.ToString());
                    WriteLog("短信提示异常:" + ex.ToString());
                }


                ToLog(string.Format("线程休眠{0}秒...", 24 * 60 * 60));

                Thread.Sleep(24 * 60 * 60 * 1000);
            }
        }

        /// <summary>
        /// 自动好评
        /// </summary>
        public void OrderAutoComment()
        {
            BLLMall bllMall = new BLLMall();
            while (IsWorking)
            {
                try
                {
                    bllMall.TimingOrderAutoComment();
                }
                catch (Exception ex)
                {

                    WriteLog(ex.ToString());
                }


                ToLog(string.Format("线程休眠{0}秒...", 60 * 60));

                Thread.Sleep(60 * 60 * 1000);
            }
        }

        /// <summary>
        /// 执行积分统计任务
        /// </summary>
        private void ExecuteScoreStatisTask()
        {
            BLLTimingTask bllTimingTask = new BLLTimingTask();
            while (IsWorking)
            {
                ToLog("查找任务..");
                var task = bllTimingTask.GetNextTimingTask(BLLTimingTask.TaskType.ScoreStatis, BLLTimingTask.TaskStatus.Waiting);
                if (task != null && task.ScheduleDate <= DateTime.Now)
                {
                    ToLog("有任务");
                    task.Status = (int)BLLTimingTask.TaskStatus.Executing;
                    bllTimingTask.Update(task);
                    ToLog("更新任务状态完成");
                    try
                    {
                        bllTimingTask.ExecuteTimingTask(task);
                    }
                    catch (Exception ex)
                    {

                        ToLog(ex.ToString());
                        WriteLog(ex.ToString());
                    }
                    task.Status = (int)BLLTimingTask.TaskStatus.Finished;
                    task.FinishDate = DateTime.Now;
                    bllTimingTask.Update(task);


                }
                else
                {
                    ToLog("无任务");
                }
                ToLog(string.Format("线程休眠{0}秒...", WorkInterval / 1000));
                Thread.Sleep(WorkInterval);
            }




        }
        /// <summary>
        /// 写日志
        /// </summary>
        private void WriteLog(string log)
        {

            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\log"))
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\log");

            string fileName = System.Windows.Forms.Application.StartupPath + string.Format(@"\log\{0}.txt", DateTime.Now.ToShortDateString().Replace("\\", "-").Replace("/", "-"));
            using (StreamWriter sw = new StreamWriter(fileName, true, Encoding.GetEncoding("GB2312")))
            {
                sw.WriteLine(DateTime.Now.ToString() + " " + log);
            }
        }
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="msg"></param>
        public void ToLog(string msg)
        {
            if (isToLog)
            {
                if (msg.Contains("正在终止线程"))
                {
                    return;
                }
                FormMain.UpdateLogDel update = new FormMain.UpdateLogDel(formMain.updateMessageLog);//定义委托
                formMain.Invoke(update, string.Format("{0}: {1}", DateTime.Now, msg));
            }
        }

    }
}

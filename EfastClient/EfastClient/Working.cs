using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace EfastClient
{
    public class Working
    {
        FormMain formMain = new FormMain();
        /// <summary>
        /// Efast
        /// </summary>
        Open.EfastSDK.Client efastClient = new Open.EfastSDK.Client();
        /// <summary>
        /// 店铺ID
        /// </summary>
        string shopId = System.Configuration.ConfigurationManager.AppSettings["eFastShopId"];
        /// <summary>
        /// 站点所有者
        /// </summary>
        string websiteOwner = System.Configuration.ConfigurationManager.AppSettings["MixBluWebSiteOwner"];
        /// <summary>
        /// 商城BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLMall bllMall = new ZentCloud.BLLJIMP.BLLMall();
        /// <summary>
        /// 用户BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
        /// <summary>
        /// 积分BLL
        /// </summary>
        ZentCloud.BLLJIMP.BllScore bllScore = new ZentCloud.BLLJIMP.BllScore();
        /// <summary>
        /// YIKE
        /// </summary>
        Open.EZRproSDK.Client yiKeClient = new Open.EZRproSDK.Client();
        public Working(FormMain form)
        {
            formMain = form;
        }
        public static bool IsWorking = false;

        /// <summary>
        ///处理线程
        /// </summary>
        Thread thWork;

        /// <summary>
        /// 线程间隔
        /// </summary>
        public static int WorkInterval = 1000;
        /// <summary>
        /// 系统日志
        /// </summary>
        public static string AppLog = "";
        /// <summary>
        /// 打印日志标识
        /// </summary>
        public static bool isToLog = true;
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


        /// <summary>
        /// 开始处理
        /// </summary>
        public void StartWork()
        {

            IsWorking = true;
            ToLog("开启程序...");
            thWork = new Thread(new ThreadStart(DaemonWorking));
            thWork.Start();
        }

        /// <summary>
        /// 停止处理
        /// </summary>
        public void StopWork()
        {
            try
            {
                IsWorking = false;
                thWork.Abort();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// 守护线程
        /// </summary>
        public void DaemonWorking()
        {
            //Efast 库存同步
            SynEfastStock();
            //自动收货
            ReceiptConfirm();
            System.Environment.Exit(0);

        }
        /// <summary>
        /// 同步Efast库存
        /// </summary>
        private void SynEfastStock()
        {

            try
            {
                var skuList = bllMall.GetList<ProductSku>(string.Format(" OutBarCode is not null And OutBarCode!='' And WebSiteOwner='{0}'", websiteOwner));
                ToLog(string.Format("共获取到{0}条数据,开始同步...", skuList.Count));
                DateTime dtNow = DateTime.Now;
                int index = 1;
                foreach (var skuInfo in skuList)
                {
                    //ToLog("刷新redis库存测试，skuid:" + skuInfo.SkuId);
                    ////刷新redis库存
                    //BLLRedis.ClearProductSkuSingle(skuInfo.WebSiteOwner, skuInfo.SkuId);
                    //ToLog("执行redis库存刷新测试完毕");

                    ToLog(string.Format("正在同步第{0}条数据,共{1}条...", index, skuList.Count));
                    var eFastSku = efastClient.GetSkuStock(int.Parse(shopId), skuInfo.OutBarCode);
                    if (eFastSku != null)
                    {
                        if (eFastSku.sl != skuInfo.Stock)
                        {
                            skuInfo.Stock = eFastSku.sl;

                            if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("update ZCJ_ProductSku set Stock={0} where SkuId={1}", skuInfo.Stock, skuInfo.SkuId)) >= 0)
                            {
                                ToLog("刷新redis库存，skuid:" + skuInfo.SkuId);
                                //刷新redis库存
                                BLLRedis.ClearProductSkuSingle(skuInfo.WebSiteOwner, skuInfo.SkuId);
                                ToLog("执行redis库存刷新完毕");
                                ToLog(string.Format("同步成功,条码:{0}库存:{1}", skuInfo.OutBarCode, skuInfo.Stock));
                            }

                        }
                        else
                        {
                            ToLog(string.Format("库存一致,跳过 条码:{0}库存:{1}", skuInfo.OutBarCode, skuInfo.Stock));
                        }

                    }
                    else
                    {

                        if (skuInfo.Stock > 0)
                        {
                            skuInfo.Stock = 0;
                            if (ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("update ZCJ_ProductSku set Stock=0 where SkuId={0}", skuInfo.SkuId)) >= 0)
                            {
                                ToLog("刷新redis库存，skuid:" + skuInfo.SkuId);
                                //刷新redis库存
                                BLLRedis.ClearProductSkuSingle(skuInfo.WebSiteOwner, skuInfo.SkuId);
                                ToLog("执行redis库存刷新完毕");
                                ToLog(string.Format("同步成功,条码:{0}库存:{1}", skuInfo.OutBarCode, skuInfo.Stock));
                            }


                        }
                        else
                        {
                            ToLog(string.Format("库存为已为0,条码:{0}", skuInfo.OutBarCode));
                        }
                       
                    }
                    index++;

                }
                ToLog("操作完成。耗时:" + (DateTime.Now - dtNow).TotalSeconds + "S");
                WriteLog(string.Format("Efast库存同步完成.耗时{0}", (DateTime.Now - dtNow).TotalSeconds + "秒"));


            }
            catch (Exception ex)
            {
                ToLog(ex.Message);
                WriteLog(ex.Message);
            }
            ToLog("线程休眠1秒...");
            Thread.Sleep(1000);
            
        }

        /// <summary>
        /// 收货
        /// </summary>
        private void ReceiptConfirm()
        {
            try
            {
                var orderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" Status='已发货' And PayMentStatus=1 And DATEDIFF(day,DeliveryTime,GETDATE())>=8 And  WebSiteOwner='{0}'", websiteOwner));
                ToLog(string.Format("共获取到{0}笔订单,开始自动收货...", orderList.Count));
                DateTime dtNow = DateTime.Now;
                int index = 1;
                int successCount = 0;
                foreach (var orderInfo in orderList)
                {
                    ToLog(string.Format("正在处理第{0}笔订单,共{1}笔...", index, orderList.Count));
                    UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                    ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                    try
                    {
                        orderInfo.Status = "交易成功";
                        if (bllMall.Update(orderInfo," Status='交易成功'",string.Format(" OrderId='{0}'",orderInfo.OrderID), tran)<=0)
                        {

                            tran.Rollback();
                            continue;
                        }

                        //增加积分
                        ScoreConfig scoreConfig = bllScore.Get<ScoreConfig>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
                        if (scoreConfig.OrderAmount == 0)
                        {
                            scoreConfig.OrderAmount = 1;
                        }
                        int addScore = (int)(orderInfo.PayableAmount / (scoreConfig.OrderAmount / scoreConfig.OrderScore));
                        if (addScore > 0)
                        {

                            WXMallScoreRecord scoreRecord = new WXMallScoreRecord();
                            scoreRecord.InsertDate = DateTime.Now;
                            scoreRecord.Remark = "微商城-交易完成获得积分";
                            scoreRecord.Score = addScore;
                            scoreRecord.UserId = orderInfo.OrderUserID;
                            scoreRecord.WebsiteOwner = websiteOwner;
                            scoreRecord.OrderID = orderInfo.OrderID;
                            scoreRecord.Type = 1;
                            if (!bllMall.Add(scoreRecord, tran))
                            {
                                tran.Rollback();
                                continue;
                            }
                            if (bllUser.Update(orderUserInfo, string.Format(" TotalScore+={0},HistoryTotalScore+={0}", addScore), string.Format(" AutoID={0}", orderUserInfo.AutoID), tran) <=0)
                            {
                                tran.Rollback();
                                continue;
                            }
                            try
                            {
                                //驿氪同步
                                yiKeClient.BonusUpdate(orderUserInfo.Ex2, addScore, string.Format("订单交易成功获得{0}积分", addScore));
                                //驿氪同步
                                yiKeClient.ChangeStatus(orderInfo.OrderID, orderInfo.Status);

                            }
                            catch (Exception)
                            {


                            }




                        }

                        //

                        //更新订单明细表状态
                        List<WXMallOrderDetailsInfo> orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                        foreach (var orderDetail in orderDetailList)
                        {
                            orderDetail.IsComplete = 1;
                            if (bllMall.Update(orderDetail, "IsComplete=1", string.Format(" OrderId='{0}'", orderInfo.OrderID))<=0)
                            {
                                tran.Rollback();
                                continue;
                            }

                        }


                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        continue;

                    }
                    tran.Commit();
                    successCount++;
                    //


                }

                ToLog("操作完成。耗时:" + (DateTime.Now - dtNow).TotalSeconds + "S");
                WriteLog(string.Format("自动收货完成.耗时{0}秒,自动收货订单数量{1}", (DateTime.Now - dtNow).TotalSeconds, successCount));

            }

            catch (Exception ex)
            {
                ToLog(ex.Message);
                WriteLog(ex.Message);
            }

        }




        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log"></param>
        private void WriteLog(string log)
        {

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\EfastClient\Log.txt", true, Encoding.GetEncoding("gb2312")))
            {
                sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
            }


        }
        //private void UnExit(string log)
        //{

        //    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\EfastClient\UnExistSkuLog.txt", true, Encoding.GetEncoding("gb2312")))
        //    {
        //        sw.WriteLine(string.Format("{0}", log));
        //    }


        //}

    }
}

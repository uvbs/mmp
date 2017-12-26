using EasyNetQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.MQ;

namespace ZentCloud.BLLJIMP
{
    public class BLLMQ : BLL
    {

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="messageInfo"></param>
        public void Publish(MessageInfo messageInfo)
        {
            messageInfo.MsgId = Guid.NewGuid().ToString();

            var historyInfo = GetMQHistoryByMsg(messageInfo);
            historyInfo.MsgActionType = "produce";
            historyInfo.StartTime = DateTime.Now;
            
            try
            {
                var connStr = Common.ConfigHelper.GetConfigString("MQConnectionString");
                using (var bus = RabbitHutch.CreateBus(connStr))
                {
                    if (bus.IsConnected)
                    {
                        bus.Publish(messageInfo);
                    }
                    else
                    {
                        historyInfo.ExceptionInfo = "发布失败:消息服务器连接失败，" + connStr;
                    }                    
                }
                
                historyInfo.EndTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                historyInfo.ExceptionInfo = ex.Message;
            }
            
            //出异常过滤掉，并存下原始信息
            try
            {
                Add(historyInfo);
            }
            catch (Exception ex)
            {
                string errLog = string.Format(" ex:{0}  historyInfo:{1}  ",
                        ex.Message,
                        JsonConvert.SerializeObject(historyInfo)
                    );
                ToLog(errLog, "D:\\log\\BLLMQ.txt");
            }

        }

        /// <summary>
        /// 客户端订阅消息
        /// </summary>
        public void Subscribe()
        {
            var connStr = Common.ConfigHelper.GetConfigString("MQConnectionString");

            using (var bus = RabbitHutch.CreateBus(connStr))
            {
                if (bus.IsConnected)
                {


                    bus.SubscribeAsync<MessageInfo>(Common.ConfigHelper.GetConfigString("MQSubscribeId"),
                    message => Task.Factory.StartNew(() =>
                    {

                        Console.WriteLine("收到新消息" + DateTime.Now.ToString());

                        var historyInfo = GetMQHistoryByMsg(message);
                        historyInfo.MsgActionType = "consume";
                        historyInfo.StartTime = DateTime.Now;
                        historyInfo.QueueId = Common.ConfigHelper.GetConfigString("MQSubscribeId");
                        try
                        {
                            Console.WriteLine("接收到的消息：" + message.Msg);

                            string reamrk = "", errMsg = "";

                        #region 业务逻辑处理
                        if (message.MsgType == CommonPlatform.Helper.EnumStringHelper.ToString(Enums.MQType.DistNewMemberNotice))
                            {
                                DistNewMemberNotice(message, out reamrk, out errMsg);
                            }
                            else if (message.MsgType == CommonPlatform.Helper.EnumStringHelper.ToString(Enums.MQType.QuestionnaireStatistics))
                            {
                                QuestionnaireStatistics(message, out reamrk, out errMsg);
                            }
                            else if (message.MsgType == CommonPlatform.Helper.EnumStringHelper.ToString(Enums.MQType.ShopDetailOpenStatistics))
                            {
                                ShopDetailOpenStatistics(message, out reamrk, out errMsg);
                            }
                            else
                            {
                            //未知类型消息，存入数据库待分析
                            reamrk = "未知类型消息";
                            }
                        #endregion

                        historyInfo.Remark = reamrk;
                            historyInfo.ExceptionInfo = errMsg;

                            historyInfo.EndTime = DateTime.Now;

                            Console.WriteLine("消息业务逻辑处理完毕");
                        }
                        catch (Exception ex)
                        {
                            historyInfo.ExceptionInfo += ex.Message;
                        }

                    //出异常过滤掉，并存下原始信息

                    try
                        {
                            Add(historyInfo);
                        }
                        catch (Exception ex)
                        {
                            string errLog = string.Format(" ex:{0}  historyInfo:{1}  ",
                                    ex.Message,
                                    JsonConvert.SerializeObject(historyInfo)
                                );
                            ToLog(errLog, "D:\\log\\BLLMQ.txt");
                        }



                    }).ContinueWith(task =>
                    {

                    //throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");

                    if (task.IsCompleted && !task.IsFaulted)
                        {
                            Console.WriteLine("消息处理完毕");
                        // Everything worked out ok
                    }
                        else
                        {
                            Console.WriteLine("消息处理异常");
                        // Dont catch this, it is caught further up the heirarchy and results in being sent to the default error queue
                        // on the broker
                        Console.WriteLine("Message processing exception - look in the default error queue (broker)");
                            throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                        }

                    }));

                    Console.WriteLine("Listening for messages. Hit <return> to quit.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("消息服务器连接失败:" + connStr);
                    Console.ReadLine();
                }
            }
        }
        
        public MQHistoryInfo GetMQHistoryByMsg(MessageInfo messageInfo)
        {
            MQHistoryInfo historyInfo = new MQHistoryInfo();
            historyInfo.MsgId = messageInfo.MsgId;
            historyInfo.Msg = messageInfo.Msg;
            historyInfo.MsgType = messageInfo.MsgType;
            historyInfo.WebsiteOwner = messageInfo.WebsiteOwner;

            historyInfo.ExceptionInfo = "";
            historyInfo.ClientId = Common.ConfigHelper.GetConfigString("MQClientId");

            return historyInfo;
        }

        /// <summary>
        /// 新分销会员通知
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <param name="ramark"></param>
        /// <param name="errMsg"></param>
        public void DistNewMemberNotice(MessageInfo messageInfo, out string ramark, out string errMsg)
        {
            Console.WriteLine("正在处理新分销会员通知");

            BLLUserDistributionMember bllUserDistributionMember = new BLLUserDistributionMember();
            BLLUser bllUser = new BLLUser();
            BLLWeixin bllWeixin = new BLLWeixin();

            ramark = "";
            errMsg = "";

            var msgBody = JsonConvert.DeserializeObject<Model.MQ.DistNewMemberNoticeInfo>(messageInfo.Msg);

            Console.WriteLine("转换完msgBody");

            //获取分销员和会员
            var distUser = bllUser.GetUserInfoByAutoID(int.Parse(msgBody.DistributionOwnerAutoId), messageInfo.WebsiteOwner);
            var member = bllUser.GetUserInfoByAutoID(int.Parse(msgBody.MemberAutoId), messageInfo.WebsiteOwner);

            //记录到会员表
            bllUserDistributionMember.SetUserDistributionOwnerInMember(new List<string>() { member.UserID }, distUser.UserID, member.WebsiteOwner);

            //获取当前是第几位会员
            var rowCount = bllUserDistributionMember.GetMemberRowCount(member.UserID, distUser.UserID, member.WebsiteOwner);

            Console.WriteLine("排名：" + rowCount);

            //获取会员昵称
            if (string.IsNullOrWhiteSpace(member.WXNickname) && !string.IsNullOrWhiteSpace(member.WXOpenId))
            {
                Console.WriteLine("无微信昵称，开始获取微信昵称");

                var accessToken = bllWeixin.GetAccessToken(messageInfo.WebsiteOwner);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("获取到accessToken，开始获取会员信息");
                    var wxUserInfo = bllWeixin.GetWeixinUserInfo(accessToken, member.WXOpenId);
                    Console.WriteLine("处理完获取会员信息");
                    if (wxUserInfo != null)
                    {
                        Console.WriteLine("获取到会员信息");
                        member.WXHeadimgurl = wxUserInfo.headimgurl;
                        member.WXNickname = string.IsNullOrWhiteSpace(wxUserInfo.nickname) ? "" : wxUserInfo.nickname.Replace("'", "");
                        member.WXProvince = wxUserInfo.province;
                        member.WXCity = wxUserInfo.city;

                        bllUser.Update(new UserInfo(), string.Format(" WXHeadimgurl='{0}',WXNickname='{1}',WXProvince='{2}',WXCity='{3}' ",
                                member.WXHeadimgurl,
                                member.WXNickname,
                                member.WXProvince,
                                member.WXCity
                            ), string.Format(" UserId = '{0}' AND WebsiteOwner = '{1}' ", member.UserID, member.WebsiteOwner));
                    }
                    else
                    {
                        Console.WriteLine("获取不到会员信息");
                    }

                }
                else
                {
                    Console.WriteLine("获取不到accessToken");
                }

            }

            var notice = string.Format("恭喜 {0} 成为您的第{1}号会员", member.WXNickname, rowCount);

            Console.WriteLine("开始发通知");
            //发送通知
            bllWeixin.SendTemplateMessageNotifyComm(distUser, string.Format("新会员通知"), notice);

            Console.WriteLine(notice);
        }

        /// <summary>
        /// 问卷统计及计数
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <param name="ramark"></param>
        /// <param name="errMsg"></param>
        public void QuestionnaireStatistics(MessageInfo messageInfo, out string ramark, out string errMsg)
        {
            Console.WriteLine("正在处理问卷统计及计数");
            ramark = "";
            errMsg = "";

            var msgBody = JsonConvert.DeserializeObject<Model.MQ.QuestionnaireStatisticsInfo>(messageInfo.Msg);

            BLL bll = new BLL();
            BLLUser bllUser = new BLLUser();

            var QuestionnaireModel = bll.Get<BLLJIMP.Model.Questionnaire>(string.Format("QuestionnaireID={0}", msgBody.QuestionnaireID));

            MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
            detailInfo.MonitorPlanID = msgBody.QuestionnaireID;
            detailInfo.EventType = 0;
            detailInfo.EventBrowser = msgBody.EventBrowser;
            detailInfo.EventBrowserID = msgBody.EventBrowserID;
            detailInfo.EventBrowserIsBata = msgBody.EventBrowserIsBata;

            detailInfo.EventBrowserVersion = msgBody.EventBrowserVersion;
            detailInfo.EventDate = DateTime.Now;
            detailInfo.EventSysByte = msgBody.EventSysByte;
            
            detailInfo.EventSysPlatform = msgBody.EventSysPlatform;
            detailInfo.SourceIP = msgBody.SourceIP;
            detailInfo.IPLocation = msgBody.IPLocation;
            detailInfo.SourceUrl = msgBody.SourceUrl;
            detailInfo.RequesSourcetUrl = msgBody.RequesSourcetUrl;
            detailInfo.WebsiteOwner = messageInfo.WebsiteOwner;
            detailInfo.ModuleType = "question";
            detailInfo.EventUserID = msgBody.EventUserID;
            detailInfo.ShareTimestamp = "1";

            int ipCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='1' ", messageInfo.WebsiteOwner, msgBody.QuestionnaireID));
            int uvCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" EventUserID is not null AND WebsiteOwner='{0}' AND MonitorPlanID={1} AND ShareTimestamp='1' ", messageInfo.WebsiteOwner, msgBody.QuestionnaireID));
            int pvCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" WebsiteOwner='{0}' AND MonitorPlanID={1} ", messageInfo.WebsiteOwner, msgBody.QuestionnaireID));

            QuestionnaireModel.IP = ipCount;
            QuestionnaireModel.PV = pvCount;
            QuestionnaireModel.UV = uvCount;
            bll.Update(QuestionnaireModel);

            var spreadUser = bllUser.GetUserInfo(msgBody.SpreadUserId,messageInfo.WebsiteOwner);

            if (spreadUser != null)
            {
                detailInfo.SpreadUserID = spreadUser.UserID;

                MonitorLinkInfo linkInfo = bll.Get<MonitorLinkInfo>(string.Format(" LinkName='{0}' And MonitorPlanID={1}", spreadUser.UserID, QuestionnaireModel.QuestionnaireID));
                if (linkInfo != null)
                {
                    linkInfo.ActivityName = QuestionnaireModel.QuestionnaireName;
                    linkInfo.ThumbnailsPath = QuestionnaireModel.QuestionnaireImage;
                    //已经为该用户建立推广链接
                    detailInfo.LinkID = linkInfo.LinkID;
                    //增加打开人数
                    linkInfo.OpenCount++;
                    int shareCount = bll.GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" LinkID ={0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", linkInfo.LinkID));
                    linkInfo.ShareCount = shareCount;
                    int iCount = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID='{0}' AND SpreadUserID='{1}' ", msgBody.QuestionnaireID, spreadUser.UserID));
                    linkInfo.DistinctOpenCount = ipCount;
                    int uCount = bll.GetCount<MonitorEventDetailsInfo>(" EventUserId ", string.Format(" MonitorPlanID='{0}' AND SpreadUserID='{1}' ", msgBody.QuestionnaireID, spreadUser.UserID));

                    int spreadCount = bll.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID='{0}' AND EventUserId='{1}' ", msgBody.QuestionnaireID, msgBody.EventUserID));
                    if (spreadCount == 0)
                    {
                        uCount = uCount + 1;
                    }

                    bll.Update(linkInfo, string.Format(" OpenCount={0},DistinctOpenCount={1},UV={2},ShareCount={3}", linkInfo.OpenCount, iCount, uCount, shareCount), string.Format("LinkID={0}", linkInfo.LinkID));
                }
                else
                {

                    //还没有为该用户建立推广链接
                    MonitorLinkInfo newLinkinfo = new MonitorLinkInfo();
                    newLinkinfo.LinkID = int.Parse(bll.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorLinkID));
                    newLinkinfo.MonitorPlanID = QuestionnaireModel.QuestionnaireID;
                    newLinkinfo.WXMemberID = 0;
                    newLinkinfo.LinkName = spreadUser.UserID;
                    newLinkinfo.RealLink = msgBody.CurrfilePath;
                    newLinkinfo.InsertDate = DateTime.Now;
                    newLinkinfo.OpenCount = 1;
                    newLinkinfo.ActivityName = QuestionnaireModel.QuestionnaireName;
                    newLinkinfo.ThumbnailsPath = QuestionnaireModel.QuestionnaireImage;
                    newLinkinfo.WebsiteOwner = messageInfo.WebsiteOwner;
                    newLinkinfo.DistinctOpenCount = 1;// ip
                    newLinkinfo.ShareCount = 0;//分享数
                    newLinkinfo.ForwardType = "questionnaire";
                    newLinkinfo.UV = 1;
                    newLinkinfo.ActivityId = QuestionnaireModel.QuestionnaireID;
                    bll.Add(newLinkinfo);
                }

            }
            bll.Add(detailInfo);


            //更新ip pv uv[Questionnaire]

            int countIp = bll.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID = {0} ", QuestionnaireModel.QuestionnaireID));
            int countPv = QuestionnaireModel.PV++;
            int countUv = bll.GetCount<MonitorEventDetailsInfo>(" EventUserId ", string.Format(" MonitorPlanID = {0} ", QuestionnaireModel.QuestionnaireID));
            bll.Update(QuestionnaireModel, string.Format(" IP={0},PV={1},UV={2} ", countIp, countPv, countUv), string.Format(" QuestionnaireID={0} ", QuestionnaireModel.QuestionnaireID));

            //更新 转发表
            bll.Update(new ActivityForwardInfo(), string.Format(" PV+=1,UV={0}", countUv), string.Format(" ActivityId='{0}'", QuestionnaireModel.QuestionnaireID));//更新转发表UV.pv
            
            Console.WriteLine("问卷统计及计数处理完毕");
        }

        /// <summary>
        /// 商品打开统计及计数
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <param name="ramark"></param>
        /// <param name="errMsg"></param>
        public void ShopDetailOpenStatistics(MessageInfo messageInfo, out string ramark, out string errMsg)
        {
            Console.WriteLine("正在处理商品打开统计及计数");
            ramark = "";
            errMsg = "";

            BLLMall bllMall = new BLLMall();

            var msgBody = JsonConvert.DeserializeObject<Model.MQ.ShopDetailOpenStatistics>(messageInfo.Msg);

            var detailInfo = msgBody.GetMonitorEventDetailsInfo();

            detailInfo.MonitorPlanID = msgBody.ProductId;

            bllMall.Add(detailInfo);

            //改成手动更新商品表
            ////随机机率去更新，不用每次都去更新商品详情

            //var rand = new Random().Next(0, 100);

            //if (rand > 10 && rand < 50 && rand % 2 == 0)
            //{
            //    //随机更新商品详情的访问统计
            //    Console.WriteLine("随机更新商品详情的访问统计");
            //    int uvCount = bllMall.GetCount<MonitorEventDetailsInfo>(" EventUserID ", string.Format(" MonitorPlanID={0} ", msgBody.ProductId));
            //    int ipCount = bllMall.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" MonitorPlanID={0} ", msgBody.ProductId));
            //    int pv = bllMall.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID={0} ", msgBody.ProductId));
            //    bllMall.Update(new WXMallProductInfo(), string.Format(" PV={2},UV={0},IP={1} ", uvCount, ipCount, pv), string.Format(" PID='{0}' ", msgBody.ProductId));
            //}
            Console.WriteLine("商品打开统计及计数处理完毕");
        }

    }
}

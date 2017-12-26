using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;
using CommonPlatform.Helper;
using System.Drawing;
using System.Threading;
using ZentCloud.BLLJIMP.Enums;
using Newtonsoft.Json;
using System.Web;
using System.IO;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 分销BLL
    /// </summary>
    public class BLLDistribution : BLL
    {
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 支付
        /// </summary>
        BllPay bllPay = new BllPay();
        /// <summary>
        /// 日志
        /// </summary>
        BLLLog bllLog = new BLLLog();
        /// <summary>
        /// 订单
        /// </summary>
        BllOrder bllOrder = new BllOrder();
        /// <summary>
        /// 
        /// </summary>
        BLLTransfersAudit bllTran = new BLLTransfersAudit();
        /// <summary>
        /// 
        /// </summary>
        BLLPermission.BLLPermission bllPer = new BLLPermission.BLLPermission();

        public void InitUserDistributionMember()
        {
            //初始化所有分销会员数据

            //查出所有分销员
            var disList = Query<InitUserDistributionMemberInfo>("select DistributionOwner from ZCJ_UserInfo where DistributionOwner is not null and DistributionOwner <> '' group by DistributionOwner");

            Console.WriteLine("获取到所有分销员：" + disList.Count + "个");

            for (int i = 0; i < disList.Count; i++)
            {
                Console.WriteLine("正在处理第：" + (i + 1) + "个分销员 " + disList[i].DistributionOwner + "；剩余：" + (disList.Count - (i + 1)));
                var userList = GetList<UserInfo>(string.Format(" UserID = '{0}' ", disList[i].DistributionOwner));

                for (int j = 0; j < userList.Count; j++)
                {
                    //分别查出每个分销员的一级会员列表
                    var memberList = GetList<UserInfo>(string.Format(" DistributionOwner = '{0}' ", userList[j].UserID));
                    Console.WriteLine("获取到所有会员：" + memberList.Count + "个");

                    for (int k = 0; k < memberList.Count; k++)
                    {
                        Console.WriteLine("正在处理弟：" + (k + 1) + "个会员：" + memberList[k].UserID);

                        //存入列表，数据保证不重复，存入前先查下是否存在
                        if (GetCount<UserDistributionMemberInfo>(string.Format(" UserId = '{0}' AND MemberId = '{1}' AND WebsiteOwner = '{2}'  ",
                                userList[j].UserID,
                                memberList[k].UserID,
                                memberList[k].WebsiteOwner
                            )) == 0)
                        {
                            UserDistributionMemberInfo data = new UserDistributionMemberInfo()
                            {
                                UserId = userList[j].UserID,
                                MemberId = memberList[k].UserID,
                                WebsiteOwner = memberList[k].WebsiteOwner,
                                InsertDate = DateTime.Now
                            };

                            if (Add(data))
                            {
                                Console.WriteLine("添加成功");
                            }
                            else
                            {
                                Console.WriteLine("添加失败");
                            }

                        }
                        else
                        {
                            Console.WriteLine("会员重复");
                        }

                    }

                }

            }

            Console.WriteLine("处理完成");

        }

        /// <summary>
        ///获取用户的下一级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserInfo> GetDownUserList(string userId)
        {
            return GetList<UserInfo>(string.Format("DistributionOwner='{0}'", userId));
        }

        public List<UserInfo> GetDownUserList(string userId, string websiteOwner)
        {
            return GetList<UserInfo>(string.Format(" DistributionOwner='{0}' AND WebsiteOwner = '{1}' ", userId, websiteOwner));
        }

        /// <summary>
        /// 获取用户的分销下级,第level级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public List<UserInfo> GetDownUserList(string userId, int level)
        {
            List<UserInfo> userList = new List<UserInfo>();
            userList.Add(Get<UserInfo>(string.Format("UserId='{0}'", userId)));

            for (int i = 0; i < level; ++i)
            {
                List<UserInfo> tempList = new List<UserInfo>();
                tempList.AddRange(userList);
                userList.Clear();
                foreach (UserInfo user in tempList)
                {
                    userList.AddRange(GetDownUserList(user.UserID));
                }
            }
            return userList;
        }

        public List<UserInfo> GetDownUserList(string userId, int level, string websiteOwner)
        {
            List<UserInfo> userList = new List<UserInfo>();
            userList.Add(Get<UserInfo>(string.Format(" UserId='{0}' AND WebsiteOwner = '{1}' ", userId, websiteOwner)));

            for (int i = 0; i < level; ++i)
            {
                List<UserInfo> tempList = new List<UserInfo>();
                tempList.AddRange(userList);
                userList.Clear();
                foreach (UserInfo user in tempList)
                {
                    userList.AddRange(GetDownUserList(user.UserID, websiteOwner));
                }
            }
            return userList;
        }

        /// <summary>
        /// 获取用户所有线下会员列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllDownUserList(string userId)
        {
            List<UserInfo> result = new List<UserInfo>();

            List<UserInfo> userList = GetDownUserList(userId);

            result.AddRange(userList);

            foreach (var item in userList)
            {
                result.AddRange(GetAllDownUserList(item.UserID));
            }

            return result;
        }


        /// <summary>
        ///获取下N级用户总和
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDownUserTotalCount(string userId, int level)
        {
            List<UserInfo> list = new List<UserInfo>();
            for (int i = 1; i <= level; i++)
            {
                var userList = GetDownUserList(userId, i);
                list.AddRange(userList);
            }
            return list.DistinctBy(p => p.UserID).ToList().Count;

        }

        /// <summary>
        ///获取下线第N级用户数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDownUserCount(string userId, int level)
        {
            return GetDownUserList(userId, level).Count;

        }
        /// <summary>
        /// 更新分销上级
        /// </summary>
        /// <param name="autoIds"></param>
        /// <param name="preUserId"></param>
        /// <returns></returns>
        public bool UpdatePreUserId(string autoIds, string preUserId, out string errorMsg)
        {
            errorMsg = "";
            UserInfo preUserInfo = bllUser.GetUserInfo(preUserId);

            foreach (var autoId in autoIds.Split(','))
            {
                UserInfo userInfo = bllUser.GetUserInfoByAutoID(int.Parse(autoId));

                if (IsInDownUserList(userInfo.UserID, preUserInfo.DistributionOwner))
                {
                    errorMsg = "不能互为上下级关系";
                    //不能互为上下级关系
                    return false;
                }


                //if (bllOrder.GetUserAllOrderCount(userInfo.UserID) > 0)
                //{

                //    errorMsg = "选择会员已有订单，不能修改上级";
                //    return false;
                //}


            }
            if (Update(new UserInfo(), string.Format("DistributionOwner='{0}'", preUserId), string.Format(" AutoId in({0}) And WebsiteOwner='{1}'", autoIds, WebsiteOwner)) == autoIds.Split(',').Count())
            {
                //修改关系
                new BLLUserDistributionMember().SetUserDistributionOwnerInMember(autoIds.Split(',').ToList(), preUserId, WebsiteOwner);
                ToLog("后台操作者 修改关系 userid:" + autoIds + "  DistributionOwner" + preUserId, "D:\\log\\BLLUserDistributionMember.txt");

                TimingTask task = new TimingTask();
                task.WebsiteOwner = WebsiteOwner;
                task.InsertDate = DateTime.Now;
                task.Status = 1;
                task.TaskInfo = "同步分销会员下级人数";
                task.TaskType = 5;
                task.ScheduleDate = DateTime.Now;
                Add(task);
                bllLog.Add(Enums.EnumLogType.DistributionOffLine, Enums.EnumLogTypeAction.Update, GetCurrUserID(), string.Format("商城分销设置上级，上级用户{0},用户ID({1})", preUserId, autoIds));
                return true;
            }
            return false;

        }

        /// <summary>
        /// 设置用户分销上级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool SetUserDistributionOwner(string userId, string distributionOwner, string websiteOwner)
        {
            BLLMQ bllMq = new BLLMQ();
            var userInfo = bllUser.GetUserInfo(userId, websiteOwner);

            //判断，如果上线是空、系统的当前用户，准备上级不是他自己也不是他下线的，则可以成为他的上线
            var websiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);
            bool canTobDistributionOwner = true;

            if (!string.IsNullOrWhiteSpace(userInfo.DistributionOwner) && userInfo.DistributionOwner != websiteOwner)
            {
                if (userInfo.DistributionOwner != websiteOwner)
                {
                    bllWeixin.ToLog(string.Format("上级不为空而且上级不是系统的，则不能设置上级  currSendMsgUser.DistributionOwner:{0} websiteOwner:{1}",
                    userInfo.DistributionOwner,
                    websiteOwner
                    ));
                    //上级不为空而且上级不是系统的，则不能设置上级
                    canTobDistributionOwner = false;
                }
                else
                {
                    //如果本身已经有上级是系统，而系统设置为不能替换上级，则不允许替换上级
                    if (websiteInfo.DisableReplaceDistributonOwner == 1)
                    {
                        canTobDistributionOwner = false;
                    }

                }
            }
            
            if (distributionOwner == userInfo.UserID)
            {
                bllWeixin.ToLog("上级是他自己，不能设置为上级 distributionOwner：" + distributionOwner + " currSendMsgUser.UserID: " + userInfo.UserID);
                //上级是他自己，不能设置为上级
                canTobDistributionOwner = false;
            }

            //memberlevel > 10 的，不能设置上级（基于月供宝大金额会员已经确定关系的人而来）
            if (userInfo.MemberLevel > 10)
            {
                canTobDistributionOwner = false;
            }

            var preUser = bllUser.GetUserInfo(distributionOwner, websiteOwner);//推荐人

            if (preUser == null)
            {
                bllWeixin.ToLog("没有取到推荐人信息，不能设置为上级 distributionOwner" + distributionOwner);
                //没有取到推荐人信息，不能设置为上级
                canTobDistributionOwner = false;
            }

            if (canTobDistributionOwner == true)
            {
                if (IsInDownUserList(userInfo.UserID, distributionOwner))
                {
                    bllWeixin.ToLog(string.Format("上级存在他的下级中的，不能设置上为级 currSendMsgUser.UserID:{0} distributionOwner:{1} ",
                            userInfo.UserID,
                            distributionOwner
                        ));
                    canTobDistributionOwner = false;
                }
            }

            //bllUser.Update(userInfo);
            bllWeixin.ToLog(string.Format("canTobDistributionOwner:{0} ,distributionOwner:{1},websiteInfo.WebsiteOwner:{2}，userId:{3}", canTobDistributionOwner, distributionOwner, websiteOwner, userId));
            if (canTobDistributionOwner)
            {

                if (bllUser.IsDistributionMember(preUser) || (distributionOwner == websiteOwner))//上级是分销员或者是根才给建立关系
                {
                    bllWeixin.ToLog("准备更新上级");
                    userInfo.DistributionOwner = preUser.UserID;
                    userInfo.Channel = preUser.Channel;

                    if (bllUser.Update(userInfo))
                    {
                        bllWeixin.ToLog("更新上级成功");

                        if (distributionOwner != websiteOwner)
                        {
                            //bllWeixin.SendTemplateMessageNotifyComm(preUser, string.Format("新会员通知"), string.Format("恭喜 {0} 成为您的第{1}号会员", userInfo.WXNickname, preUser.DistributionDownUserCountLevel1 + 1));

                            //提交到消息队列里面去发送通知
                            var distNewMemberNoticeInfo = new Model.MQ.DistNewMemberNoticeInfo()
                            {
                                DistributionOwnerAutoId = preUser.AutoID.ToString(),
                                MemberAutoId = userInfo.AutoID.ToString()
                            };
                            var mq = new Model.MQ.MessageInfo()
                            {
                                Msg = JsonConvert.SerializeObject(distNewMemberNoticeInfo),
                                MsgId = Guid.NewGuid().ToString(),
                                MsgType = EnumStringHelper.ToString(MQType.DistNewMemberNotice),
                                WebsiteOwner = userInfo.WebsiteOwner
                            };

                            bllMq.Publish(mq);

                        }

                        UpdateUpUserCount(userInfo);
                        bllUser.AddUserScoreDetail(preUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.RecommendWeixinSubscribeAddScore), websiteOwner, null, null);

                        bllWeixin.ToLog("更新上级后其他处理完成");

                    }
                    else
                    {
                        bllWeixin.ToLog("更新上级失败");
                    }
                }

            }

            return canTobDistributionOwner;
        }

        /// <summary>
        /// 根据临时关系设置
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool SetUserDistributionOwnerByTemp(string userId, string websiteOwner)
        {
            /*
             * 1.当前用户上级没有；
             * 2.当前用户有临时记录
             * 3.设置临时记录的上级；
             * 4.修改临时记录状态
             * 
             */

            var userInfo = bllUser.GetUserInfo(userId, websiteOwner);

            var tmpInfo = GetUserDistributionOwnerTempInfo(userInfo.WXOpenId, websiteOwner);

            if (tmpInfo != null)
            {
                if (SetUserDistributionOwner(userId, tmpInfo.DistributionOwner, websiteOwner))
                {

                    tmpInfo = GetUserDistributionOwnerTempInfo(userInfo.WXOpenId, websiteOwner);
                    if (tmpInfo.Status == 0)
                    {
                        tmpInfo.Status = 1;
                        Update(tmpInfo);

                        if (tmpInfo.FromSource == "qrcode")
                        {
                            bllWeixin.ToLog("开始更新带来新会员得积分");
                            string msg = "";
                            bllWeixin.ToLog("更新带来新会员得积分结果：" + bllUser.AddUserScoreDetail(tmpInfo.DistributionOwner, "DistQRcodeMember", websiteOwner, out msg, relationID: userId));
                        }

                    }
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public UserDistributionOwnerTempInfo GetUserDistributionOwnerTempInfo(string openId, string websiteOwner)
        {
            if (string.IsNullOrWhiteSpace(openId))
            {
                return null;
            }

            return Get<UserDistributionOwnerTempInfo>(string.Format(" OpenId = '{0}' AND WebsiteOwner = '{1}' ", openId, websiteOwner));
        }


        /// <summary>
        /// 判断指定用户是否在下级用户列表中
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="downUserId"></param>
        /// <returns></returns>
        public bool IsInDownUserList(string userId, string downUserId)
        {
            bool result = false;
            var currUserDownUserList = GetAllDownUserList(userId);
            if (currUserDownUserList != null)
            {
                if (currUserDownUserList.Count(p => p.UserID == downUserId) > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取用户加入分销系统时间
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public DateTime GetUserDistributionRegTime(UserInfo userInfo)
        {
            try
            {


                DateTime regTime = new DateTime();
                WXMallOrderInfo orderInfo = Get<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And WebsiteOwner='{1}' And PaymentStatus=1 And OrderType in(0,1,2)  Order by InsertDate ASC", userInfo.UserID, WebsiteOwner));
                if (orderInfo != null && orderInfo.PayTime != null)
                {
                    regTime = (DateTime)orderInfo.PayTime;
                }
                else
                {
                    if (userInfo.Regtime != null)
                    {
                        regTime = (DateTime)userInfo.Regtime;
                    }

                }
                return regTime;
            }
            catch (Exception)
            {

                return DateTime.Now;
            }
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="userInfo">提现用户信息</param>
        /// <param name="bankCardId">银行卡ID</param>
        /// <param name="amount">提现金额</param>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="type"> 到账类型 0银行卡1微信 2账户余额</param>
        /// <param name="msg">提示信息</param>
        /// <returns>成功或失败</returns>
        public bool ApplyWithrawCash(UserInfo userInfo, string bankCardId, string amount, string websiteOwner, int type, out string msg)
        {

            bool result = false;
            msg = "";
            decimal amountD = 0;
            if (!decimal.TryParse(amount, out amountD))
            {
                msg = "金额不正确";
                goto outoff;
            }
            if (amountD <= 0)
            {
                msg = "金额必须大于0";
                goto outoff;
            }
            if (GetUserCanUseAmount(userInfo) < amountD)
            {
                msg = "您的可用余额不足";
                goto outoff;
            }
            WithdrawCash model = new WithdrawCash();
            model.InsertDate = DateTime.Now;
            model.IsPublic = 1;
            model.Phone = userInfo.Phone;
            model.ServerFee = 0;
            model.RealAmount = amountD;
            model.Amount = amountD;
            model.Status = 0;
            model.TrueName = userInfo.TrueName;
            model.UserId = userInfo.UserID;
            model.LastUpdateDate = DateTime.Now;
            model.WebSiteOwner = websiteOwner;
            model.WithdrawCashType = "DistributionOnLine";
            model.TransfersType = type;
            model.TranId = bllTran.GetGUID(TransacType.CommAdd);
            switch (type)
            {
                case 0://银行卡提现
                    BindBankCard bankCard = Get<BindBankCard>(string.Format("AutoId={0} And UserId='{1}'", bankCardId, userInfo.UserID));
                    if (bankCard == null)
                    {
                        msg = "银行卡不存在";
                        goto outoff;

                    }

                    model.AccountBranchCity = bankCard.AccountBranchCity;
                    model.AccountBranchName = bankCard.AccountBranchName;
                    model.AccountBranchProvince = bankCard.AccountBranchProvince;
                    model.AccountName = bankCard.AccountName;
                    model.BankAccount = bankCard.BankAccount;
                    model.BankName = bankCard.BankName;
                    break;
                case 1://微信提现
                    model.AccountName = bllUser.GetUserDispalyName(userInfo);
                    break;
                case 2://账户余额
                    model.AccountName = bllUser.GetUserDispalyName(userInfo);
                    break;
                default:
                    break;
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();
            try
            {
                userInfo.FrozenAmount += amountD;
                if (Update(userInfo, string.Format(" FrozenAmount={0}", userInfo.FrozenAmount), string.Format(" AutoID={0}", userInfo.AutoID), tran) > 0)
                {
                    //插入申请提现记录表
                    if (Add(model, tran))
                    {

                        #region 审核记录表
                        if (bllPer.CheckPermissionKey(model.WebSiteOwner, ZentCloud.BLLPermission.Enums.PermissionSysKey.PMS_TRANSFERSAUDIT))
                        {
                            string tranInfo = string.Format("申请人:{0}<br/>申请金额:{1}<br/>手机:{2}", model.AccountName, model.Amount, model.Phone);
                            if (!bllTran.Add("DistributionWithdraw", model.TranId, tranInfo, model.Amount))
                            {
                                tran.Rollback();
                                msg = "申请失败";
                                return false;
                            }
                            else
                            {
                                string title = string.Format("收到提现申请");
                                string content = string.Format("申请金额:{0}", model.Amount);
                                string url = string.Format("http://{0}/app/transfersaudit/list.aspx", System.Web.HttpContext.Current.Request.Url.Host);

                                //发送微信模板消息
                                bllWeixin.SendTemplateMessageToKefuTranAuditPer(title, content, url);

                            }
                        }

                        #endregion

                        #region 颂和
                        if (userInfo.WebsiteOwner == "songhe")
                        {
                            UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            scoreRecord.UserID = userInfo.UserID;
                            scoreRecord.WebSiteOwner = userInfo.WebsiteOwner;
                            scoreRecord.Score = (double)(-amountD);
                            scoreRecord.AddTime = DateTime.Now;
                            scoreRecord.AddNote = "申请提现,金额:" + amountD;
                            scoreRecord.ScoreType = "ApplyWithrawCash";
                            scoreRecord.RelationID = model.TranId;

                            if (!Add(scoreRecord, tran))
                            {
                                tran.Rollback();
                                msg = "申请失败";
                                return false;
                            }

                        }
                        #endregion
                        tran.Commit();
                        msg = "您的提现申请已经成功提交!";
                        result = true;

                    }
                    else
                    {
                        msg = "插入提现申请失败";
                        tran.Rollback();
                        goto outoff;
                    }


                }
                else
                {
                    msg = "更新冻结金额失败";
                    tran.Rollback();
                    goto outoff;
                }



            }
            catch (Exception ex)
            {
                msg = ex.Message;
                tran.Rollback();
                goto outoff;

            }

        outoff:
            return result;

        }

        /// <summary>
        ///修改提现状态
        /// </summary>
        /// <param name="list"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateWithrawCashStatus(List<WithdrawCash> list, int status, out string msg)
        {

            msg = "";
            bool result = false;
            if (list.Count() <= 0)
            {
                msg = "修改记录数量不能为零";
                goto outoff;
            }
            if (status.Equals(0))
            {
                msg = "待审核状态不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(status)).Count() > 0)
            {
                msg = "新状态与旧状态相同，不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(2)).Count() > 0)
            {
                msg = "状态为成功的记录不能修改";
                goto outoff;
            }
            if (list.Where(p => p.Status.Equals(3)).Count() > 0)
            {
                msg = "状态为失败的记录不能修改";
                goto outoff;
            }


            foreach (var item in list)
            {
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
                try
                {

                    StringBuilder sbSQL = new StringBuilder();
                    UserInfo userInfo = bllUser.GetUserInfo(item.UserId);
                    if (userInfo.FrozenAmount - item.Amount < 0)
                    {
                        msg = string.Format("账户 {0} 冻结金额不足,请检查", userInfo.UserID);
                        goto outoff;
                    }
                    if (userInfo.FrozenAmount - item.Amount < 0)
                    {
                        msg = string.Format("账户 {0} 余额不足,请检查", userInfo.UserID);
                        goto outoff;
                    }
                    if (status.Equals(1))//修改为受理中
                    {

                    }
                    else if (status.Equals(2))//修改为成功
                    {
                        //扣除账户余额并解冻
                        sbSQL.AppendFormat(" UPDATE ZCJ_UserInfo SET TotalAmount-={0},FrozenAmount-={0} Where UserId='{1}';", item.Amount, userInfo.UserID);
                        if (item.TransfersType == 2)
                        {
                            sbSQL.AppendFormat(" UPDATE ZCJ_UserInfo SET AccountAmount+={0} Where UserId='{1}';", item.Amount, userInfo.UserID);

                        }

                    }
                    else if (status.Equals(3))//修改为失败
                    {
                        //解冻
                        sbSQL.AppendFormat(" UPDATE ZCJ_UserInfo SET FrozenAmount-={0} where UserId='{1}';", item.Amount, userInfo.UserID);
                        sbSQL.AppendFormat(" Delete From  ZCJ_TransfersAudit where TranId='{0}';", item.TranId);
                        if (item.WebSiteOwner == "songhe")
                        {
                            //UserScoreDetailsInfo scoreRecord = new UserScoreDetailsInfo();
                            //scoreRecord.UserID = userInfo.UserID;
                            //scoreRecord.WebSiteOwner = userInfo.WebsiteOwner;
                            //scoreRecord.Score = (double)(-amountD);
                            //scoreRecord.AddTime = DateTime.Now;
                            //scoreRecord.AddNote = "申请提现,金额:"+amountD;
                            //scoreRecord.ScoreType = "ApplyWithrawCash";
                            //scoreRecord.RelationID = model.TranId;
                            sbSQL.AppendFormat(" Delete From  ZCJ_UserScoreDetailsInfo where WebsiteOwner='{0}' And UserID='{1}' And ScoreType='ApplyWithrawCash' And RelationID='{2}' ;", item.WebSiteOwner, item.UserId, item.TranId);

                        }
                    }
                    sbSQL.AppendFormat("update ZCJ_WithdrawCash Set Status={0},LastUpdateDate=getdate() where AutoID ={1}", status, item.AutoID);//修改提现记录状态
                    int count = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSQL.ToString(), tran);
                    if (count > 0)
                    {
                        if (!string.IsNullOrEmpty(userInfo.MgrUserId))//管理员账号
                        {
                            userInfo = bllUser.GetUserInfo(userInfo.MgrUserId);//打钱给管理员账号
                            if (status.Equals(2) && (item.TransfersType == 2))//给管理账号充余额
                            {
                                int resInt = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_UserInfo SET AccountAmount+={0} Where UserId='{1}';", item.Amount, userInfo.UserID));
                                if (resInt <= 0)
                                {
                                    tran.Rollback();
                                    continue;
                                }


                            }


                        }
                        if (status.Equals(2) && (item.TransfersType == 1))//成功如果是微信方式则直接打款
                        {
                            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                            string orderId = (item.AutoID + userInfo.AutoID + new Random().Next(1, 100)).ToString();
                            if (bllPay.WeixinTransfers(orderId, item.RealAmount, userInfo.WXOpenId, ip, out msg, "提现"))
                            {
                                //发送微信模板消息
                                bllWeixin.SendTemplateMessageNotifyComm(userInfo, "您提现的佣金已经到账", string.Format("提现金额:{0}元。请查看微信钱包", item.Amount));
                                //发送微信模板消息
                            }
                            else//打款失败
                            {
                                tran.Rollback();
                                continue;
                            }

                        }


                        tran.Commit();
                        result = true;
                        msg = "操作成功!";

                    }
                    else
                    {
                        tran.Rollback();
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message;
                    goto outoff;
                }

            }
        outoff:
            return result;
        }


        /// <summary>
        /// 获取用户可提现金额
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public decimal GetUserCanUseAmount(UserInfo userInfo)
        {
            decimal result = userInfo.TotalAmount - userInfo.FrozenAmount;
            return result;
            //return result > 0 ? result : 0;
        }
        /// <summary>
        /// 获取用户已经提现的总金额
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public decimal GetUserWithdrawTotalAmount(UserInfo userInfo)
        {
            decimal result = 0;
            if (userInfo.WebsiteOwner == "songhe")
            {
                BLLUserScoreDetailsInfo bllUserScoreDetailsInfo = new BLLUserScoreDetailsInfo();
                result = -(decimal)bllUserScoreDetailsInfo.GetSumScore(userInfo.WebsiteOwner, "TotalAmount", userIDs: userInfo.UserID, scoreEvents: "申请提现", isPrint: "1");
            }
            else
            {
                //WithdrawCashType='DistributionOnLine' And
                string sql = string.Format("select Sum(Amount) From ZCJ_WithdrawCash Where  Status=2 And UserId='{0}' And WebsiteOwner='{1}' ", userInfo.UserID, WebsiteOwner);
                var dbData = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
                if (dbData != null) result = decimal.Parse(dbData.ToString());

            }
            return result;
        }

        /// <summary>
        /// 获取我的订单数
        /// </summary>
        /// <returns></returns>
        public int GetMyOrderCount(string paymentStatus = "1")
        {
            int result = 0;

            result = GetCount<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And PaymentStatus in ({1}) And OrderType in(0,1,2) And TotalAmount>0 And IsNull(IsMain,0)=0", GetCurrUserID(), paymentStatus));

            return result;
        }

        /// <summary>
        /// 获取userid用户的第level级订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetOrderList(string userId, int level, DistributionStatus status)
        {
            List<UserInfo> userList = GetDownUserList(userId, level);
            UserInfo userT = bllUser.GetUserInfo(userId);
            userList.Add(userT);
            StringBuilder sbUserIds = new StringBuilder("");
            foreach (var user in userList)
            {
                sbUserIds.AppendFormat("'{0}',", user.UserID);
            }
            return GetList<WXMallOrderInfo>(string.Format("OrderUserID in({0}) And DistributionStatus={1} And Status!='已取消' And IsRefund=0 And OrderType in(0,1,2) And TotalAmount>0 And IsNull(IsMain,0)=0", sbUserIds.ToString().TrimEnd(','), (int)status));
            //List<WXMallOrderInfo> orderList = new List<WXMallOrderInfo>();
            //foreach (UserInfo user in userList)
            //{
            //    orderList.AddRange(GetList<WXMallOrderInfo>(string.Format("OrderUserID='{0}' and DistributionStatus={1} And Status!='已取消' And IsRefund=0 ", user.UserID, (int)status)));
            //}

            //return orderList;
        }
        /// <summary>
        /// 获取用户的第level级订单 已付款状态及之后状态的订单 (已付款，已收货，已审核)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetOrderListPaied(string userId, int level)
        {
            List<WXMallOrderInfo> orderList = new List<WXMallOrderInfo>();
            List<UserInfo> userList = GetDownUserList(userId, level);
            foreach (UserInfo user in userList)
            {
                orderList.AddRange(GetList<WXMallOrderInfo>(string.Format("OrderUserID='{0}'  And IsRefund!=1 And Status!='已取消' And OrderType in(0,1,2) And TotalAmount>0 And IsNull(IsMain,0)=0", user.UserID)));
            }
            return orderList;
        }

        ///// <summary>
        ///// 获取用户第N级获得的佣金总和
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="level"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public decimal GetCommission(string userId, int level)
        //{
        //    decimal commission = 0;
        //    if (level < 1 || level > 10)
        //    {
        //        return commission;
        //    }
        //    UserInfo userInfo = Get<UserInfo>(string.Format("UserId='{0}'", userId));
        //    WebsiteInfo webSiteInfo=GetWebsiteInfoModelFromDataBase();
        //    if (userInfo == null)
        //    {
        //        throw new Exception(string.Format("用户 {0} 不存在！", userId));
        //    }
        //    List<WXMallOrderInfo> orderList = GetOrderListPaied(userId, level);
        //    //double rate = GetDistributionRate(level) / 100;
        //    foreach (WXMallOrderInfo order in orderList)
        //    {
        //        decimal rate = 0;
        //        if (level==1)
        //        {
        //            rate = order.DistributionRateLevel1/100;
        //        }
        //        if (level==2)
        //        {

        //        }
        //        if (level==3)
        //        {

        //        }
        //        commission += order.TotalAmount * (decimal)rate;
        //    }
        //    return commission;

        //}

        ///// <summary>
        ///// 获取用户获得的下N级佣金 总和
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="level"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public decimal GetCommissionTotal(string userId, int level)
        //{
        //    //decimal total = 0;
        //    //for (int i = 1; i <= level; i++)
        //    //{
        //    //    total += GetCommission(userId, i);
        //    //}
        //    //return total;
        //    //累计佣金
        //    string sql = string.Format("Select Sum(Amount) From ZCJ_ProjectCommission Where ProjectType='DistributionOnLine' And UserId='{0}' And 	CommissionLevel<={1}", userId, level);
        //    var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
        //    if (result != null)
        //    {
        //        return decimal.Parse(result.ToString());
        //    }
        //    return 0;



        //}

        /// <summary>
        /// 获取用户下N级订单(已付款)金额 总和
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public decimal GetUserOrderTotalAmount(string userId, int level)
        {
            decimal totalAmount = 0;
            for (int i = 1; i <= level; i++)
            {
                totalAmount += GetOrderListPaied(userId, i).Sum(p => p.TotalAmount);
            }
            return totalAmount;

        }

        /// <summary>
        /// 获取用户第N级销售额 总额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public decimal GetUserOrderTotalAmountByLevel(string userId, int level)
        {
            return GetOrderListPaied(userId, level).Sum(p => p.TotalAmount);

        }

        ///// <summary>
        ///// 获取用户贡献的佣金总额
        ///// </summary>
        ///// <param name="upUserInfo">上级用户</param>
        ///// <param name="downUserInfo">下级用户</param>
        ///// <returns></returns>
        //public decimal GetContributionAmount(UserInfo upUserInfo, UserInfo downUserInfo)
        //{

        //    //decimal total = 0;
        //    //int level = GetUserBetweenLevel(UpUserInfo, DownUserInfo);
        //    //total = GetList<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And DistributionStatus=3", DownUserInfo.UserID)).Sum(p => p.TotalAmount) * ((decimal)GetDistributionRate(level) / 100);
        //    //return total;
        //    decimal total = 0;
        //    foreach (var item in GetList<WXMallOrderInfo>(string.Format("OrderUserID='{0}' And DistributionStatus=3", downUserInfo.UserID)))
        //    {
        //        total+=item.TotalAmount*(item.DistributionRateLevel1/100);
        //    }
        //    return total;

        //}
        /// <summary>
        /// 获取上级用户跟下级用之间的级数
        /// </summary>
        /// <param name="upUserInfo"></param>
        /// <param name="downUserInfo"></param>
        /// <returns></returns>
        public int GetUserBetweenLevel(UserInfo upUserInfo, UserInfo downUserInfo)
        {
            int level = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (GetDownUserList(upUserInfo.UserID, i).Where(p => p.UserID.Equals(downUserInfo.UserID)).Count() > 0)
                {
                    level = i;
                    break;
                }

            }
            return level;

        }




        /// <summary>
        /// 获取用户下N级的订单数量 总和
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetOrderCount(string userId, int level, DistributionStatus status)
        {
            int total = 0;
            for (int i = 1; i <= level; i++)
            {
                total += GetOrderList(userId, i, status).Count;
            }

            return total;

        }

        /// <summary>
        /// 获取订单金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public decimal GetOrderTotalAmount(string userId, int level, DistributionStatus status)
        {
            decimal total = 0;
            for (int i = 1; i <= level; i++)
            {
                total += GetOrderList(userId, i, status).Where(p => p.Status != "已取消").Where(p => p.IsRefund == 0).Sum(p => p.TotalAmount);
            }

            return total;

        }

        ///// <summary>
        ///// 获取分级提成 百分比
        ///// </summary>
        ///// <param name="level"></param>
        ///// <returns></returns>
        //public double GetDistributionRate(int level)
        //{

        //    WebsiteInfo webSiteInfo = GetWebsiteInfoModelFromDataBase();
        //    switch (level)
        //    {
        //        case 1:
        //            return webSiteInfo.DistributionRateLevel1;
        //        case 2:
        //            return webSiteInfo.DistributionRateLevel2;
        //        case 3:
        //            return webSiteInfo.DistributionRateLevel3;
        //        case 4:
        //            return webSiteInfo.DistributionRateLevel4;
        //        case 5:
        //            return webSiteInfo.DistributionRateLevel5;
        //        case 6:
        //            return webSiteInfo.DistributionRateLevel6;
        //        case 7:
        //            return webSiteInfo.DistributionRateLevel7;
        //        case 8:
        //            return webSiteInfo.DistributionRateLevel8;
        //        case 9:
        //            return webSiteInfo.DistributionRateLevel9;
        //        case 10:
        //            return webSiteInfo.DistributionRateLevel10;
        //        default:
        //            return 0;
        //    }
        //}

        ///// <summary>
        ///// 获取成为会员提成 百分比
        ///// </summary>
        ///// <param name="level"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public double GetDistributionRateMember(int level)
        //{
        //    WebsiteInfo webSiteInfo = GetWebsiteInfoModelFromDataBase();
        //    switch (level)
        //    {
        //        case 1:
        //            return webSiteInfo.DistributionMemberRateLevel1;
        //        case 2:
        //            return webSiteInfo.DistributionMemberRateLevel2;
        //        case 3:
        //            return webSiteInfo.DistributionMemberRateLevel3;
        //        default:
        //            return 0;
        //    }
        //}

        /// <summary>
        /// 获取分销设置的级数 1-3 固定为三级
        /// </summary>
        /// <returns></returns>
        public int GetDistributionRateLevel()
        {
            return 1;
            //int level = 1;
            //WebsiteInfo webSiteInfo = GetWebsiteInfoModelFromDataBase();
            //if (webSiteInfo.DistributionRateLevel2.Equals(0))
            //{
            //    level = 1;
            //}
            //else if ((!webSiteInfo.DistributionRateLevel2.Equals(0)) && (webSiteInfo.DistributionRateLevel3.Equals(0)))
            //{
            //    level = 2;
            //}
            //else if ((!webSiteInfo.DistributionRateLevel2.Equals(0)) && (!webSiteInfo.DistributionRateLevel3.Equals(0)))
            //{
            //    level = 3;
            //}
            //return level;


        }

        /// <summary>
        /// 获取推广二维码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetDistributionWxQrcodeLimitUrl(string userId = "", string type = "")
        {
            string result = string.Empty;

            bllWeixin.GetDistributionWxQrcodeLimit(out result, userId, type);

            return result;
        }

        /// <summary>
        /// 获取用户的分销上一级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUpUser(string userId)
        {
            UserInfo preUserInfo = Get<UserInfo>(string.Format("UserId='{0}'", userId));
            if (preUserInfo == null)
            {
                return null;
            }
            else
            {
                return Get<UserInfo>(string.Format("UserID='{0}'", preUserInfo.DistributionOwner));
            }
        }


        /// <summary>
        ///获取用户的分销上级,第level级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public UserInfo GetUpUser(string userId, int level)
        {
            UserInfo preUser = new UserInfo();
            for (int i = 0; i < level; ++i)
            {
                preUser = GetUpUser(userId);
                if (preUser == null)
                {
                    return null;
                }
                else
                {
                    userId = preUser.UserID;
                }
            }
            return preUser;
        }
        /// <summary>
        /// 获取分销提成比例
        /// </summary>
        /// <param name="userInfo">用户</param>
        /// <param name="level">等级</param>
        /// <param name="isFirst">是否首单</param>
        /// <returns></returns>
        public double GetDistributionRate(UserInfo userInfo, int level, bool isFirst = false)
        {

            UserLevelConfig levelConfig = GetUserLevel(userInfo);

            double result = 0;

            if (isFirst)
            {
                switch (level)
                {
                    case 0:
                        double.TryParse(levelConfig.DistributionRateLevel0First, out result);
                        break;
                    case 1:
                        double.TryParse(levelConfig.DistributionRateLevel1First, out result);
                        break;

                    default:
                        break;
                }
            }
            else
            {

                switch (level)
                {
                    case 0:
                        //续单
                        double.TryParse(levelConfig.DistributionRateLevel0, out result);
                        break;
                    case 1:
                        //续单
                        double.TryParse(levelConfig.DistributionRateLevel1, out result);
                        break;
                    default:
                        break;
                }

            }
            return result;
        }


        /// <summary>
        /// 获取渠道提成比例
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public double GetDistributionRateChannel(UserInfo userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo.ChannelLevelId))
            {
                UserLevelConfig config = Get<UserLevelConfig>(string.Format("AutoId={0}", userInfo.ChannelLevelId));
                if (config != null)
                {
                    if (!string.IsNullOrEmpty(config.ChannelRate))
                    {
                        return double.Parse(config.ChannelRate);
                    }
                }
            }
            return 0;

        }


        /// <summary>
        /// 获取用户对应等级
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public UserLevelConfig GetUserLevel(UserInfo userInfo)
        {
            UserLevelConfig level = new UserLevelConfig();
            level.DistributionRateLevel0First = "0";
            level.DistributionRateLevel0 = "0";
            level.DistributionRateLevel1First = "0";
            level.DistributionRateLevel1 = "0";
            level.DistributionRateLevel2 = "0";
            level.DistributionRateLevel3 = "0";
            level.LevelString = "";
            if (userInfo == null)
            {
                return level;
            }

            WebsiteInfo websiteInfo = GetColByKey<WebsiteInfo>("WebsiteOwner", userInfo.WebsiteOwner, "WebsiteOwner,DistributionGetWay");

            if (websiteInfo == null)
            {
                return level;
            }

            if (websiteInfo.DistributionGetWay == 1)
            {
                var levelQuery = QueryUserLevel(userInfo.WebsiteOwner, "DistributionOnLine", userInfo.MemberLevel.ToString());
                if (levelQuery == null)
                {
                    return level;
                }
                else
                {
                    return levelQuery;
                }

            }

            foreach (var item in QueryUserLevelList(userInfo.WebsiteOwner))
            {
                if ((double)userInfo.HistoryDistributionOnLineTotalAmount >= item.FromHistoryScore && (double)userInfo.HistoryDistributionOnLineTotalAmount <= item.ToHistoryScore)
                {
                    return item;
                }
            }
            return level;
        }
        /// <summary>
        /// 查询用户等级
        /// </summary>
        /// <returns></returns>
        public UserLevelConfig QueryUserLevel(string websiteOwner, string levelType, string levelNumber)
        {
            return Get<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' And LevelType='{1}' And LevelNumber={2}", websiteOwner, levelType, levelNumber));
        }
        /// <summary>
        /// 查询用户等级
        /// </summary>
        /// <returns></returns>
        public UserLevelConfig QueryUserLevel(string websiteOwner, string levelId)
        {
            return Get<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' And AutoId='{1}'", websiteOwner, levelId));
        }
        /// <summary>
        /// 查询用户等级
        /// </summary>
        /// <returns></returns>
        public List<UserLevelConfig> QueryUserLevelList(string websiteOwner, string levelType = "DistributionOnLine",
            string levelNumber = "", string fromLevelNumber = "", bool showAll = false, string colName = "", string levelnumberSort = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}'", websiteOwner);
            sbSql.AppendFormat(" And LevelType='{0}'", levelType);
            if (!string.IsNullOrWhiteSpace(levelNumber)) sbSql.AppendFormat(" And LevelNumber={0}", levelNumber);
            if (!string.IsNullOrWhiteSpace(fromLevelNumber)) sbSql.AppendFormat(" And LevelNumber>={0}", fromLevelNumber);
            if (!showAll) sbSql.AppendFormat(" And IsNull(IsDisable,0)=0 ");
            if (!string.IsNullOrEmpty(levelnumberSort)) sbSql.AppendFormat(" Order by LevelNumber ASC ");
            if (!string.IsNullOrWhiteSpace(colName)) return GetColList<UserLevelConfig>(int.MaxValue, 1, sbSql.ToString(), colName);


            return GetList<UserLevelConfig>(sbSql.ToString());
        }
        /// <summary>
        /// 查询渠道用户等级
        /// </summary>
        /// <returns></returns>
        public List<UserLevelConfig> QueryUserLevelListChannel(string websiteOwner)
        {
            return GetList<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' And LevelType='DistributionChannel'", websiteOwner));


        }



        /// <summary>
        /// 分销打佣金
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool Transfers(WXMallOrderInfo orderInfo, out string msg)
        {
            BLLJIMP.BLLUser bllUser = new BLLUser();
            BLLMall bllMall = new BLLMall();
            msg = "";
            decimal totalAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;
            List<WXMallRefund> refundList = GetList<WXMallRefund>(string.Format(" OrderId='{0}'", orderInfo.OrderID));
            if (refundList.Count(p => p.Status != 6 && p.Status != 7) > 0)
            {
                msg = "该订单有退款未处理,不能分佣";
                return false;
            }
            totalAmount -= refundList.Where(p => p.Status == 6).Sum(p => p.RefundAmount);
            if (totalAmount <= 0)
            {

                totalAmount = 0;

            }
            //int disLevel =1;//设置的分销级别
            UserInfo upUserLevel1 = null;//分销上一级
            //UserInfo upUserLevel2 = null;//分销上二级
            //UserInfo upUserLevel3 = null;//分销上三级
            StringBuilder sbSql = new StringBuilder();
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();
            if (orderInfo.Status != "交易成功" || (orderInfo.PaymentStatus == 0))
            {
                msg = "该订单不是交易成功状态，或者没有付款，不可以分佣";
                return false;
            }
            if (GetCount<ProjectCommission>(string.Format("ProjectId={0}", orderInfo.OrderID)) > 0)
            {
                msg = "已经分过佣金了";
                if (orderInfo.DistributionStatus != 3)
                {
                    orderInfo.DistributionStatus = 3;
                    Update(orderInfo);
                }
                return false;
            }

            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            if (orderUserInfo == null)
            {
                msg = "下单用户不存在";
                return false;
            }
            bool isFirst = IsFirstOrder(orderInfo);

            #region 直销

            //double rate = GetDistributionRate(orderUserInfo, 0, isFirst);//直销提成比例

            //decimal amount = ((decimal)rate / 100) * totalAmount;//直销提成金额


            double rate = GetDistributionRate(orderUserInfo, 0, isFirst);//直销提成比例

            //decimal amount = ((decimal)rate / 100) * totalAmount;//直销提成金额

            decimal amount = GetUserCommission(orderInfo, orderUserInfo, 0);

            sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amount, orderUserInfo.UserID);

            sbSql.AppendFormat(" Update ZCJ_WXMallOrderInfo Set IsCanSettlement='1' Where OrderId='{0}' ;", orderInfo.OrderID);

            ProjectCommission model = new ProjectCommission();
            model.UserId = orderUserInfo.UserID;
            model.Amount = amount;
            model.CommissionUserId = orderUserInfo.UserID;
            model.InsertDate = DateTime.Now;
            model.ProjectAmount = totalAmount;
            model.ProjectId = int.Parse(orderInfo.OrderID);
            model.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
            model.Rate = rate;
            model.WebsiteOwner = orderInfo.WebsiteOwner;
            model.ProjectType = "DistributionOnLine";
            model.Remark = "直销分销佣金";
            if (isFirst)
            {
                model.Remark += "(首次购买)";
            }
            model.CommissionLevel = "0";
            if (!Add(model, tran))
            {
                tran.Rollback();
                msg = "分佣失败";
                return false;
            }
            if (amount != 0 && orderInfo.WebsiteOwner == "songhe")
            {
                if (bllUser.AddScoreDetail(orderUserInfo.UserID, orderInfo.WebsiteOwner, (double)amount,
                    string.Format("商城订单分佣 订单号：{0}", orderInfo.OrderID), "TotalAmount", (double)0,
                    orderInfo.OrderID, "商城订单分佣", orderUserInfo.WXOpenId, "", (double)amount, 0, "",
                    tran, ex5: "0") <= 0)
                {
                    msg = "直销分佣明细记录失败";
                    tran.Rollback();
                    return false;
                }
            }


            #endregion

            #region 分销上一级
            ProjectCommission modelLevel1 = new ProjectCommission();
            //if (disLevel >= 1)
            //{
            upUserLevel1 = GetUpUser(orderInfo.OrderUserID, 1);

            if (upUserLevel1 != null && (!IsChannel(upUserLevel1)))//一级分销打款
            {
                double rateLevel1 = GetDistributionRate(upUserLevel1, 1, isFirst);
                // decimal amountLevel1 = ((decimal)rateLevel1 / 100) * totalAmount;

                decimal amountLevel1 = GetUserCommission(orderInfo, orderUserInfo, 1);
                //upUserLevel1.TotalAmount += amountLevel1;
                //upUserLevel1.HistoryDistributionOnLineTotalAmount += amountLevel1;
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amountLevel1, upUserLevel1.UserID);
                //ProjectCommission modelLevel1 = new ProjectCommission();
                modelLevel1.UserId = upUserLevel1.UserID;
                modelLevel1.Amount = amountLevel1;
                modelLevel1.CommissionUserId = orderInfo.OrderUserID;
                modelLevel1.InsertDate = DateTime.Now;
                modelLevel1.ProjectAmount = totalAmount;
                modelLevel1.ProjectId = int.Parse(orderInfo.OrderID);
                modelLevel1.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                modelLevel1.Rate = rateLevel1;
                modelLevel1.WebsiteOwner = orderInfo.WebsiteOwner;
                modelLevel1.ProjectType = "DistributionOnLine";
                modelLevel1.Remark = "一级分销佣金";
                if (isFirst)
                {
                    modelLevel1.Remark += "(首次购买)";
                }
                modelLevel1.CommissionLevel = "1";
                if (!Add(modelLevel1, tran))
                {
                    tran.Rollback();
                    msg = "分佣失败";
                    return false;
                }

                if (amountLevel1 != 0 && orderInfo.WebsiteOwner == "songhe")
                {
                    if (bllUser.AddScoreDetail(upUserLevel1.UserID, orderInfo.WebsiteOwner, (double)amountLevel1,
                        string.Format("商城订单分佣 订单号：{0}", orderInfo.OrderID), "TotalAmount", (double)0,
                        orderInfo.OrderID, "商城订单分佣", upUserLevel1.WXOpenId, "", (double)amountLevel1, 0, "",
                        tran, ex5: "1") <= 0)
                    {
                        msg = "分佣明细记录失败";
                        tran.Rollback();
                        return false;
                    }
                }
            }

            // }
            #endregion



            string channelUserId = GetUserChannel(orderUserInfo);
            if (!string.IsNullOrEmpty(channelUserId))
            {
                #region 最底层渠道
                UserInfo channelUserInfo = bllUser.GetUserInfo(channelUserId, orderUserInfo.WebsiteOwner);
                if (channelUserInfo != null)//
                {
                    double channelRate = GetDistributionRateChannel(channelUserInfo);
                    //decimal channelAmount = ((decimal)channelRate / 100) * totalAmount;

                    decimal channelAmount = GetChannelCommissin(orderInfo, channelUserInfo);
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", channelAmount, channelUserInfo.UserID);
                    ProjectCommission projectCommission = new ProjectCommission();
                    projectCommission.UserId = channelUserInfo.UserID;
                    projectCommission.Amount = channelAmount;
                    projectCommission.CommissionUserId = orderInfo.OrderUserID;
                    projectCommission.InsertDate = DateTime.Now;
                    projectCommission.ProjectAmount = totalAmount;
                    projectCommission.ProjectId = int.Parse(orderInfo.OrderID);
                    projectCommission.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                    projectCommission.Rate = channelRate;
                    projectCommission.WebsiteOwner = orderInfo.WebsiteOwner;
                    projectCommission.ProjectType = "DistributionOnLineChannel";
                    projectCommission.Remark = "渠道佣金";
                    projectCommission.CommissionLevel = "";
                    if (!Add(projectCommission, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }
                #endregion


                #region 依次给上级渠道分佣
                while (channelUserInfo != null)
                {
                    if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
                    {
                        break;
                    }
                    channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, orderInfo.WebsiteOwner);
                    if (channelUserInfo != null)
                    {

                        double channelRate = GetDistributionRateChannel(channelUserInfo);
                        //decimal channelAmount = ((decimal)channelRate / 100) * totalAmount;
                        decimal channelAmount = GetChannelCommissin(orderInfo, channelUserInfo);
                        sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", channelAmount, channelUserInfo.UserID);
                        ProjectCommission projectCommission = new ProjectCommission();
                        projectCommission.UserId = channelUserInfo.UserID;
                        projectCommission.Amount = channelAmount;
                        projectCommission.CommissionUserId = orderInfo.OrderUserID;
                        projectCommission.InsertDate = DateTime.Now;
                        projectCommission.ProjectAmount = totalAmount;
                        projectCommission.ProjectId = int.Parse(orderInfo.OrderID);
                        projectCommission.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                        projectCommission.Rate = channelRate;
                        projectCommission.WebsiteOwner = orderInfo.WebsiteOwner;
                        projectCommission.ProjectType = "DistributionOnLineChannel";
                        projectCommission.Remark = "渠道佣金";
                        projectCommission.CommissionLevel = "";
                        if (!Add(projectCommission, tran))
                        {
                            tran.Rollback();
                            msg = "渠道分佣失败";
                            return false;
                        }



                    }


                }
                #endregion

            }

            #region 供应商渠道分佣
            //销售毛利=售价-下单时候的基价
            //供应商的渠道佣金=销售毛利 * 供应商渠道对应的等级比例
            if (!string.IsNullOrEmpty(orderInfo.SupplierUserId))
            {

                var suppLierInfo = bllUser.GetUserInfo(orderInfo.SupplierUserId, orderInfo.WebsiteOwner);
                if (suppLierInfo != null)
                {
                    var channelUserInfo = bllUser.GetUserInfo(suppLierInfo.ParentChannel, orderInfo.WebsiteOwner);
                    if (channelUserInfo != null)
                    {

                        UserLevelConfig channelLevel = bllUser.Get<UserLevelConfig>(string.Format("AutoId={0}", channelUserInfo.SupplierLevelId));
                        if (channelLevel != null)
                        {
                            List<WXMallOrderDetailsInfo> orderDetailListAll = bllMall.GetOrderDetail(orderInfo.OrderID);
                            List<WXMallOrderDetailsInfo> orderDetailList = new List<WXMallOrderDetailsInfo>();
                            foreach (var item in orderDetailListAll)//退款的不算
                            {
                                if (string.IsNullOrEmpty(item.RefundStatus) || (item.RefundStatus == "7"))
                                {
                                    orderDetailList.Add(item);
                                }
                            }

                            decimal totalOrderPrice = orderDetailList.Sum(p => (decimal)p.OrderPrice * p.TotalCount);//总售价
                            decimal totalBasePrice = orderDetailList.Sum(p => p.BasePrice * p.TotalCount);//总基价
                            decimal channelCommionAmount = Math.Round((totalOrderPrice - totalBasePrice) * (decimal.Parse(channelLevel.ChannelRate) / 100), 2);//供应商渠道佣金
                            if (channelCommionAmount < 0)
                            {
                                channelCommionAmount = 0;
                            }
                            sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", channelCommionAmount, channelUserInfo.UserID);
                            ProjectCommission projectCommission = new ProjectCommission();
                            projectCommission.UserId = channelUserInfo.UserID;
                            projectCommission.Amount = channelCommionAmount;
                            projectCommission.CommissionUserId = orderInfo.OrderUserID;
                            projectCommission.InsertDate = DateTime.Now;
                            projectCommission.ProjectAmount = totalAmount;
                            projectCommission.ProjectId = int.Parse(orderInfo.OrderID);
                            projectCommission.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                            projectCommission.Rate = double.Parse(channelLevel.ChannelRate);
                            projectCommission.WebsiteOwner = orderInfo.WebsiteOwner;
                            projectCommission.ProjectType = "DistributionOnLineSupplierChannel";
                            projectCommission.Remark = "供应商渠道佣金";
                            projectCommission.Remark += string.Format(" 售价:{0}基价:{1}:毛利:{2}:分佣比例{3}%", totalOrderPrice, totalBasePrice, (totalOrderPrice - totalBasePrice), projectCommission.Rate);
                            projectCommission.CommissionLevel = "";
                            if (!Add(projectCommission, tran))
                            {
                                tran.Rollback();
                                msg = "分佣失败";
                                return false;
                            }




                        }

                    }

                }

            }

            #endregion


            if (upUserLevel1 == null)//都没有上级
            {
                return true;
            }

            int result = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString(), tran);
            if (result > 0)
            {
                tran.Commit();

                #region 微信通知
                try
                {

                    BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
                    string url = string.Format("http://{0}/app/cation/wap/mall/distribution/index.aspx", bllWebsiteDomain.GetWebsiteDoMain(orderInfo.WebsiteOwner));
                    if (model.Amount > 0)//直销佣金
                    {
                        bllWeixin.SendTemplateMessageNotifyCommTask(orderUserInfo.WXOpenId, "订单已分佣", string.Format("订单号:{0}\\n金额:{1}\\n点击查看详情", orderInfo.OrderID, model.Amount), url, "", "", "", orderInfo.WebsiteOwner);

                    }
                    if (modelLevel1.Amount > 0)//一级佣金
                    {
                        bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "订单已分佣", string.Format("订单号:{0}\\n金额:{1}\\n点击查看详情", orderInfo.OrderID, modelLevel1.Amount), url, "", "", "", orderInfo.WebsiteOwner);

                    }
                }
                catch (Exception)
                {


                }
                #endregion
                return true;
            }
            else
            {
                msg = "操作失败";
                tran.Rollback();
                return false;
            }


        }



        /// <summary>
        /// 分销预估佣金
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool TransfersEstimate(WXMallOrderInfo orderInfo)
        {

            try
            {


                BLLJIMP.BLLUser bllUser = new BLLUser();
                decimal totalAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;
                if (totalAmount <= 0)
                {

                    totalAmount = 0;
                }
                UserInfo upUserLevel1 = null;//分销上一级
                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                bool isFirst = IsFirstOrder(orderInfo);
                #region 直销
                double rate = GetDistributionRate(orderUserInfo, 0, isFirst);//直销提成比例
                //decimal amount = ((decimal)rate / 100) * totalAmount;//直销提成金额

                decimal amount = GetUserCommission(orderInfo, orderUserInfo, 0);
                ProjectCommissionEstimate model = new ProjectCommissionEstimate();
                model.UserId = orderUserInfo.UserID;
                model.Amount = amount;
                model.CommissionUserId = orderUserInfo.UserID;
                model.InsertDate = DateTime.Now;
                model.ProjectAmount = orderInfo.TotalAmount;
                model.ProjectId = int.Parse(orderInfo.OrderID);
                model.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                model.Rate = rate;
                model.WebsiteOwner = orderInfo.WebsiteOwner;
                model.ProjectType = "DistributionOnLine";
                model.Remark = "直销分销佣金";
                if (isFirst)
                {
                    model.Remark += "(首次购买)";
                }
                model.CommissionLevel = "0";
                if (!Add(model))
                {


                    return false;
                }


                orderUserInfo.HistoryDistributionOnLineTotalAmountEstimate += amount;
                Update(orderUserInfo);



                //if (orderUserInfo.UserID=="WXUser35056729-1c43-4312-968e-4db531e94cfb")
                //{
                //    ToLog(string.Format("orderid:{0}totalamont:{1}isfirst:{2}rate:{3}amount:{4}HistoryDistributionOnLineTotalAmountEstimate{5}",orderInfo.OrderID,totalAmount,isFirst.ToString(),rate,amount,orderUserInfo.HistoryDistributionOnLineTotalAmountEstimate));

                //}

                #endregion

                #region 分销上一级
                //if (disLevel >= 1)
                //{
                upUserLevel1 = GetUpUser(orderInfo.OrderUserID, 1);
                if (upUserLevel1 != null && (!IsChannel(upUserLevel1)))//一级分销打款
                {
                    double rateLevel1 = GetDistributionRate(upUserLevel1, 1, isFirst);
                    //decimal amountLevel1 = ((decimal)rateLevel1 / 100) * totalAmount;
                    decimal amountLevel1 = GetUserCommission(orderInfo, orderUserInfo, 1);
                    ProjectCommissionEstimate modelLevel1 = new ProjectCommissionEstimate();
                    modelLevel1.UserId = upUserLevel1.UserID;
                    modelLevel1.Amount = amountLevel1;
                    modelLevel1.CommissionUserId = orderInfo.OrderUserID;
                    modelLevel1.InsertDate = DateTime.Now;
                    modelLevel1.ProjectAmount = orderInfo.TotalAmount;
                    modelLevel1.ProjectId = int.Parse(orderInfo.OrderID);
                    modelLevel1.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                    modelLevel1.Rate = rateLevel1;
                    modelLevel1.WebsiteOwner = orderInfo.WebsiteOwner;
                    modelLevel1.ProjectType = "DistributionOnLine";
                    modelLevel1.Remark = "一级分销佣金";
                    if (isFirst)
                    {
                        modelLevel1.Remark += "(首次购买)";
                    }
                    modelLevel1.CommissionLevel = "1";
                    if (!Add(modelLevel1))
                    {

                        return false;
                    }

                    upUserLevel1.HistoryDistributionOnLineTotalAmountEstimate += amountLevel1;
                    Update(upUserLevel1);



                    //if (upUserLevel1.UserID == "WXUser35056729-1c43-4312-968e-4db531e94cfb")
                    //{
                    //    ToLog(string.Format("orderid:{0}totalamont:{1}isfirst:{2}rate:{3}amount:{4}HistoryDistributionOnLineTotalAmountEstimate{5}", orderInfo.OrderID, totalAmount, isFirst.ToString(), rateLevel1, amountLevel1, upUserLevel1.HistoryDistributionOnLineTotalAmountEstimate));

                    //}

                }

                // }
                #endregion


                string channelUserId = GetUserChannel(orderUserInfo);
                if (!string.IsNullOrEmpty(channelUserId))
                {
                    #region 最底层渠道
                    UserInfo channelUserInfo = bllUser.GetUserInfo(channelUserId, orderUserInfo.WebsiteOwner);
                    if (channelUserInfo != null)//
                    {
                        double channelRate = GetDistributionRateChannel(channelUserInfo);
                        //decimal channelAmount = ((decimal)channelRate / 100) * totalAmount;
                        decimal channelAmount = GetChannelCommissin(orderInfo, channelUserInfo);
                        ProjectCommissionEstimate modelLevel1 = new ProjectCommissionEstimate();
                        modelLevel1.UserId = channelUserInfo.UserID;
                        modelLevel1.Amount = channelAmount;
                        modelLevel1.CommissionUserId = orderInfo.OrderUserID;
                        modelLevel1.InsertDate = DateTime.Now;
                        modelLevel1.ProjectAmount = totalAmount;
                        modelLevel1.ProjectId = int.Parse(orderInfo.OrderID);
                        modelLevel1.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                        modelLevel1.Rate = channelRate;
                        modelLevel1.WebsiteOwner = orderInfo.WebsiteOwner;
                        modelLevel1.ProjectType = "DistributionOnLineChannel";
                        modelLevel1.Remark = "渠道佣金";
                        modelLevel1.CommissionLevel = "";
                        if (!Add(modelLevel1))
                        {

                            return false;
                        }
                        channelUserInfo.HistoryDistributionOnLineTotalAmountEstimate += channelAmount;
                        Update(channelUserInfo);
                    }
                    #endregion



                    #region 依次给上级渠道增加预估佣金
                    while (channelUserInfo != null)
                    {
                        if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
                        {
                            break;
                        }
                        channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, orderInfo.WebsiteOwner);
                        if (channelUserInfo != null)
                        {

                            double channelRate = GetDistributionRateChannel(channelUserInfo);
                            //decimal channelAmount = ((decimal)channelRate / 100) * totalAmount;
                            decimal channelAmount = GetChannelCommissin(orderInfo, channelUserInfo);
                            ProjectCommissionEstimate projectCommission = new ProjectCommissionEstimate();
                            projectCommission.UserId = channelUserInfo.UserID;
                            projectCommission.Amount = channelAmount;
                            projectCommission.CommissionUserId = orderInfo.OrderUserID;
                            projectCommission.InsertDate = DateTime.Now;
                            projectCommission.ProjectAmount = totalAmount;
                            projectCommission.ProjectId = int.Parse(orderInfo.OrderID);
                            projectCommission.ProjectName = string.Format("订单号{0}", orderInfo.OrderID);
                            projectCommission.Rate = channelRate;
                            projectCommission.WebsiteOwner = orderInfo.WebsiteOwner;
                            projectCommission.ProjectType = "DistributionOnLineChannel";
                            projectCommission.Remark = "渠道佣金";
                            projectCommission.CommissionLevel = "";
                            if (!Add(projectCommission))
                            {

                                return false;
                            }
                            channelUserInfo.HistoryDistributionOnLineTotalAmountEstimate += channelAmount;
                            Update(channelUserInfo);


                        }


                    }
                    #endregion

                }

                return true;
            }
            catch (Exception ex)
            {

                ToLog(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 是否首次下单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsFirstOrder(WXMallOrderInfo orderInfo)
        {
            bool isFirst = false;//会员是否首次下单
            int count = GetCount<WXMallOrderInfo>(string.Format(" OrderUserId='{0}' And Paymentstatus=1 And Status!='已取消'", orderInfo.OrderUserID));
            if (count == 1)
            {
                return true;
            }
            else
            {
                //1 0918
                //2 0920
                //3 0921
                //4 0922
                //5 0923
                int count1 = GetCount<WXMallOrderInfo>(string.Format(" OrderUserId='{0}' And Paymentstatus=1 And Status!='已取消' And OrderId!='{1}' And InsertDate <='{2}'", orderInfo.OrderUserID, orderInfo.OrderID, orderInfo.InsertDate.ToString()));
                if (count1 >= 1)
                {
                    isFirst = false;
                }
                else
                {
                    isFirst = true;
                }

            }
            //if (GetCount<ProjectCommission>(string.Format(" CommissionUserId='{0}'", userId)) == 0)
            //{
            //    isFirst = true;
            //}
            return isFirst;

        }

        /// <summary>
        /// 分销打佣金 业务分销而来
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public bool Transfers(int projectId, out string msg)
        {
            /// <summary>
            /// 线下分销
            /// </summary>
            BLLDistributionOffLine blDisOffLine = new BLLDistributionOffLine();

            msg = "";
            Project projectInfo = blDisOffLine.GetProject(projectId);
            if (projectInfo.IsComplete == 1)
            {
                msg = "已经分过佣金了";
                return false;
            }
            if (projectInfo.Amount == 0)
            {
                msg = "项目金额为0不可以分佣";
                return false;
            }
            decimal totalAmount = projectInfo.Amount;
            int disLevel = 3;//设置的分销级别
            UserInfo upUserLevel1 = null;//分销上一级
            UserInfo upUserLevel2 = null;//分销上二级
            UserInfo upUserLevel3 = null;//分销上三级
            StringBuilder sbSql = new StringBuilder();
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();

            if (GetCount<ProjectCommission>(string.Format("ProjectId={0}", projectInfo.ProjectId)) > 0)
            {
                msg = "已经分过佣金了";
                return false;
            }

            #region 直销

            var userInfo = bllUser.GetUserInfo(projectInfo.UserId);
            if (userInfo != null)
            {
                double rate = GetDistributionRate(userInfo, 0);
                decimal amount = ((decimal)rate / 100) * totalAmount;
                sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amount, userInfo.UserID);
                ProjectCommission model = new ProjectCommission();
                model.UserId = userInfo.UserID;
                model.Amount = amount;
                model.CommissionUserId = projectInfo.UserId;
                model.InsertDate = DateTime.Now;
                model.ProjectAmount = projectInfo.Amount;
                model.ProjectId = projectInfo.ProjectId;
                model.ProjectName = string.Format("项目名称{0}", projectInfo.ProjectName);
                model.Rate = (int)rate;
                model.WebsiteOwner = projectInfo.WebsiteOwner;
                model.ProjectType = "DistributionOnLine";
                model.Remark = "直销分销佣金";
                model.CommissionLevel = "0";
                if (!Add(model, tran))
                {
                    tran.Rollback();
                    msg = "分佣失败";
                    return false;
                }
            }

            #endregion


            #region 分销上一级
            if (disLevel >= 1)
            {
                upUserLevel1 = GetUpUser(projectInfo.UserId, 1);
                if (upUserLevel1 != null)//一级分销打款
                {
                    double rate = GetDistributionRate(upUserLevel1, 1);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0},HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel1.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel1.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = string.Format("项目名称{0}", projectInfo.ProjectName);
                    model.Rate = (int)rate;
                    model.WebsiteOwner = projectInfo.WebsiteOwner;
                    model.ProjectType = "DistributionOnLine";
                    model.Remark = "一级分销佣金";
                    model.CommissionLevel = "1";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }

            }
            #endregion

            #region 分销上二级
            if (disLevel >= 2)
            {
                upUserLevel2 = GetUpUser(projectInfo.UserId, 2);
                if (upUserLevel2 != null)//二级分销打款
                {
                    double rate = GetDistributionRate(upUserLevel2, 2);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0} ,HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel2.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel2.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = projectInfo.ProjectName;
                    model.Rate = (int)rate;
                    model.WebsiteOwner = projectInfo.WebsiteOwner;
                    model.ProjectType = "DistributionOnLine";
                    model.Remark = "二级分销佣金";
                    model.CommissionLevel = "2";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }
            }
            #endregion

            #region 分销上三级
            if (disLevel >= 3)
            {
                upUserLevel3 = GetUpUser(projectInfo.UserId, 3);
                if (upUserLevel3 != null)//三级分销打款
                {
                    double rate = GetDistributionRate(upUserLevel3, 3);
                    decimal amount = ((decimal)rate / 100) * totalAmount;
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set TotalAmount+={0} ,HistoryDistributionOnLineTotalAmount+={0} Where UserId='{1}' ; ", amount, upUserLevel3.UserID);
                    ProjectCommission model = new ProjectCommission();
                    model.UserId = upUserLevel3.UserID;
                    model.Amount = amount;
                    model.CommissionUserId = projectInfo.UserId;
                    model.InsertDate = DateTime.Now;
                    model.ProjectAmount = projectInfo.Amount;
                    model.ProjectId = projectInfo.ProjectId;
                    model.ProjectName = projectInfo.ProjectName;
                    model.Rate = (int)rate;
                    model.WebsiteOwner = projectInfo.WebsiteOwner;
                    model.ProjectType = "DistributionOnLine";
                    model.Remark = "三级分销佣金";
                    model.CommissionLevel = "3";
                    if (!Add(model, tran))
                    {
                        tran.Rollback();
                        msg = "分佣失败";
                        return false;
                    }
                }

            }
            #endregion

            if (upUserLevel1 == null && upUserLevel2 == null && upUserLevel3 == null)//都没有上级
            {
                return true;
            }

            int result = ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sbSql.ToString(), tran);
            if (result > 0)
            {
                tran.Commit();
                return true;
            }
            else
            {
                msg = "操作失败";
                tran.Rollback();
                return false;
            }


        }

        ///// <summary>
        ///// 获取预计的佣金金额及比例
        ///// </summary>
        ///// <param name="userInfo">用户信息</param>
        ///// <param name="level">第三级</param>
        ///// <param name="totalAmount">金额</param>
        ///// <returns></returns>
        //public object GetPredictionCommission(UserInfo userInfo, int level, decimal totalAmount)
        //{

        //    PredictionCommission model = new PredictionCommission();
        //    UserLevelConfig userLevel = GetUserLevel(userInfo);
        //    switch (level)
        //    {
        //        case 1:
        //            model.Amount = decimal.Parse(userLevel.DistributionRateLevel1) * totalAmount;
        //            model.Rate = decimal.Parse(userLevel.DistributionRateLevel1);
        //            break;
        //        case 2:
        //            model.Amount = decimal.Parse(userLevel.DistributionRateLevel2) * totalAmount;
        //            model.Rate = decimal.Parse(userLevel.DistributionRateLevel2);
        //            break;
        //        case 3:
        //            model.Amount = decimal.Parse(userLevel.DistributionRateLevel3) * totalAmount;
        //            model.Rate = decimal.Parse(userLevel.DistributionRateLevel3);
        //            break;
        //        default:
        //            break;
        //    }

        //    return model;
        //}
        /// <summary>
        /// 给用户发送信息
        /// </summary>
        public void SendMessageToUser(WXMallOrderInfo orderInfo)
        {

            try
            {
                ToLog("OrderID:" + orderInfo.OrderID, "D:\\SendMessageToPreUser.txt");

                WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);

                ToLog("IsDisabledCommission:" + websiteInfo.IsDisabledCommission, "D:\\SendMessageToPreUser.txt");
                if (websiteInfo.IsDisabledCommission == 1)//不分佣
                {
                    return;
                }
                StringBuilder sbMsg = new StringBuilder();
                UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                UserInfo upUserLevel1 = GetUpUser(orderInfo.OrderUserID, 1);//上一级用户

                decimal amount;
                #region 分销直销
                if (orderUserInfo != null)
                {

                    amount = GetUserCommission(orderInfo, orderUserInfo, 0);
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    amount = Math.Round(amount, 2);
                    sbMsg.AppendFormat("您在{0} 下单,订单号为:{1} 订单金额为:{2}元;您将获得的奖励为:{3}元", orderInfo.PayTime.ToString(), orderInfo.OrderID, orderInfo.TotalAmount, amount);

                    if (amount > 0)
                    {
                        bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo, "下单通知", sbMsg.ToString());
                    }
                }
                #endregion

                #region 分销一级
                if (upUserLevel1 != null)
                {
                    //rate = GetDistributionRate(upUserLevel1, 1, IsFirstOrder(orderInfo));
                    ToLog("正在给上级发消息" + upUserLevel1.UserID, "D:\\SendMessageToPreUser.txt");
                    amount = GetUserCommission(orderInfo, upUserLevel1, 1);
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    amount = Math.Round(amount, 2);
                    sbMsg.Clear();
                    sbMsg.AppendFormat("您的会员 [{0}] 在{1} 下单,订单号为:{2} 订单金额为:{3}元;您将获得的奖励为:{4}元", bllUser.GetUserDispalyName(orderUserInfo), orderInfo.PayTime.ToString(), orderInfo.OrderID, orderInfo.TotalAmount, amount);

                    if (amount > 0)
                    {
                        ToLog("给上级发消息：" + sbMsg.ToString(), "D:\\SendMessageToPreUser.txt");
                        bllWeixin.SendTemplateMessageNotifyComm(upUserLevel1, "会员下单通知", sbMsg.ToString());
                    }
                }
                #endregion

                //#region 分销二级
                //if (upUserLevel2 != null)
                //{
                //    sbMsg.Clear();
                //    rate = GetDistributionRate(upUserLevel2, 2);
                //    amount = ((decimal)rate / 100) * orderInfo.TotalAmount;
                //    amount = Math.Round(amount, 2);
                //    sbMsg.AppendFormat("您的二级代言人 [{0}] 在{1} 下单,订单号为:{2} 订单金额为:{3}元;您将获得的提成为:{4}元", bllUser.GetUserDispalyName(orderUserInfo), orderInfo.PayTime.ToString(), orderInfo.OrderID, orderInfo.TotalAmount, amount);

                //    if (amount > 0)
                //    {
                //        bllWeixin.SendTemplateMessageNotifyComm(upUserLevel2.WXOpenId, "代言人下单通知", sbMsg.ToString());
                //    }
                //}
                //#endregion

                //#region 分销三级
                //if (upUserLevel3 != null)
                //{
                //    sbMsg.Clear();
                //    rate = GetDistributionRate(upUserLevel3, 3);
                //    amount = ((decimal)rate / 100) * orderInfo.TotalAmount;
                //    amount = Math.Round(amount, 2);

                //    sbMsg.AppendFormat("您的三级代言人 [{0}] 在{1} 下单,订单号为:{2} 订单金额为:{3}元;您将获得的提成为:{4}元", bllUser.GetUserDispalyName(orderUserInfo), orderInfo.PayTime.ToString(), orderInfo.OrderID, orderInfo.TotalAmount, amount);
                //    if (amount > 0)
                //    {
                //        bllWeixin.SendTemplateMessageNotifyComm(upUserLevel3.WXOpenId, "代言人下单通知", sbMsg.ToString());
                //    }
                //}
                //#endregion

            }
            catch (Exception ex)
            {
                ToLog("ex:" + ex.Message, "D:\\SendMessageToPreUser.txt");
            }


        }

        ///// <summary>
        ///// 更新上级用户的下级人数
        ///// </summary>
        ///// <param name="userId"></param>
        //public void UpdateUpUserCount(string userId)
        //{

        //    var upUserLevel1 = GetUpUser(userId, 1);//上一级用户
        //    //var upUserLevel2 = GetUpUser(userId, 2);//上二级用户
        //    //var upUserLevel3 = GetUpUser(userId, 3);//上三级用户
        //    if (upUserLevel1 != null)
        //    {
        //        upUserLevel1.DistributionDownUserCountLevel1 = GetDownUserCount(upUserLevel1.UserID, 1);
        //        //upUserLevel1.DistributionDownUserCountLevel2 = GetDownUserCount(upUserLevel1.UserID, 2);
        //        //upUserLevel1.DistributionDownUserCountLevel3 = GetDownUserCount(upUserLevel1.UserID, 3);
        //        //Update(upUserLevel1);

        //        Update(
        //            upUserLevel1,
        //            string.Format(" DistributionDownUserCountLevel1 = {0} ", upUserLevel1.DistributionDownUserCountLevel1),
        //            string.Format(" AutoID = {0} ", upUserLevel1.AutoID));

        //    }

        //if (upUserLevel2 != null)
        //{
        //    upUserLevel2.DistributionDownUserCountLevel1 = GetDownUserCount(upUserLevel2.UserID, 1);
        //    upUserLevel2.DistributionDownUserCountLevel2 = GetDownUserCount(upUserLevel2.UserID, 2);
        //    upUserLevel2.DistributionDownUserCountLevel3 = GetDownUserCount(upUserLevel2.UserID, 3);
        //    Update(upUserLevel2);
        //}
        //if (upUserLevel3 != null)
        //{
        //    upUserLevel3.DistributionDownUserCountLevel1 = GetDownUserCount(upUserLevel3.UserID, 1);
        //    upUserLevel3.DistributionDownUserCountLevel2 = GetDownUserCount(upUserLevel3.UserID, 2);
        //    upUserLevel3.DistributionDownUserCountLevel3 = GetDownUserCount(upUserLevel3.UserID, 3);
        //    Update(upUserLevel3);
        //}


        //}
        /// <summary>
        /// 更新上级用户的下级人数
        /// </summary>
        /// <param name="userId"></param>
        public void UpdateUpUserCount(UserInfo userInfo)
        {

            var upUserLevel1 = GetUpUser(userInfo.UserID, 1);//上一级用户
            if (upUserLevel1 != null)
            {
                upUserLevel1.DistributionDownUserCountLevel1 = GetDownUserCount(upUserLevel1.UserID, 1);
                upUserLevel1.DistributionDownUserCountAll = GetAllDownUsersList(upUserLevel1.UserID).Count() - 1;
                Update(upUserLevel1);
            }

            #region 给渠道更新
            UserInfo channelUserInfo;//渠道用户
            if (!string.IsNullOrEmpty(userInfo.Channel))
            {

                #region 先给最底层的渠道更新
                channelUserInfo = bllUser.GetUserInfo(userInfo.Channel, userInfo.WebsiteOwner);
                if (channelUserInfo != null)//
                {


                    channelUserInfo.DistributionDownUserCountLevel1 = GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Count;//直接会员数量
                    channelUserInfo.DistributionDownUserCountAll = GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
                    Update(channelUserInfo);



                }
                #endregion

                #region 依次给上级渠道更新
                while (channelUserInfo != null)
                {
                    if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
                    {
                        break;
                    }
                    channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, userInfo.WebsiteOwner);
                    if (channelUserInfo != null)
                    {
                        channelUserInfo.DistributionDownUserCountLevel1 = GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Count;//直接会员数量
                        channelUserInfo.DistributionDownUserCountAll = GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
                        Update(channelUserInfo);

                    }


                }
                #endregion



            }


            #endregion




            //if (!string.IsNullOrEmpty(upUserLevel1.IsFirstLevelDistribution))
            //{
            //    if (upUserLevel1.IsFirstLevelDistribution.Trim() == "1")
            //    {
            //        if (!string.IsNullOrEmpty(upUserLevel1.ParentChannel))
            //        {
            //            #region 给渠道更新
            //            #region 最底层渠道
            //            UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format(" UserId='{0}' And WebsiteOwner='{1}'", upUserLevel1.ParentChannel, userInfo.WebsiteOwner));//渠道用户
            //            channelUserInfo.DistributionDownUserCountLevel1 = GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Count;//直接会员数量
            //            channelUserInfo.DistributionDownUserCountAll = GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
            //            bllUser.Update(channelUserInfo);
            //            #endregion


            //            #region 依次给上级渠道更新
            //            while (channelUserInfo != null)
            //            {
            //                if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
            //                {
            //                    break;
            //                }
            //                channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, userInfo.WebsiteOwner);
            //                if (channelUserInfo != null)
            //                {
            //                    channelUserInfo.DistributionDownUserCountLevel1 = GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Count;//直接会员数量
            //                    channelUserInfo.DistributionDownUserCountAll = GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
            //                    Update(channelUserInfo);

            //                }


            //            }
            //            #endregion






            //            #endregion
            //        }


            //    }
            //}












        }

        ///// <summary>
        ///// 更新上级的销售额
        ///// </summary>
        ///// <param name="userId"></param>
        //public void UpdateUpUserSaleAmount(string userId)
        //{

        //    var upUserLevel1 = GetUpUser(userId, 1);//上一级用户
        //   // var upUserLevel2 = GetUpUser(userId, 2);//上二级用户
        //    //var upUserLevel3 = GetUpUser(userId, 3);//上三级用户
        //    if (upUserLevel1 != null)
        //    {
        //        upUserLevel1.DistributionSaleAmountLevel1 = GetUserOrderTotalAmountByLevel(upUserLevel1.UserID, 1);
        //        //upUserLevel1.DistributionSaleAmountLevel2 = GetUserOrderTotalAmountByLevel(upUserLevel1.UserID, 2);
        //       // upUserLevel1.DistributionSaleAmountLevel3 = GetUserOrderTotalAmountByLevel(upUserLevel1.UserID, 3);
        //        Update(upUserLevel1);



        //    }
        //    //if (upUserLevel2 != null)
        //    //{
        //    //    upUserLevel2.DistributionSaleAmountLevel1 = GetUserOrderTotalAmountByLevel(upUserLevel2.UserID, 1);
        //    //    upUserLevel2.DistributionSaleAmountLevel2 = GetUserOrderTotalAmountByLevel(upUserLevel2.UserID, 2);
        //    //    upUserLevel2.DistributionSaleAmountLevel3 = GetUserOrderTotalAmountByLevel(upUserLevel2.UserID, 3);
        //    //    Update(upUserLevel2);
        //    //}
        //    //if (upUserLevel3 != null)
        //    //{
        //    //    upUserLevel3.DistributionSaleAmountLevel1 = GetUserOrderTotalAmountByLevel(upUserLevel3.UserID, 1);
        //    //    upUserLevel3.DistributionSaleAmountLevel2 = GetUserOrderTotalAmountByLevel(upUserLevel3.UserID, 2);
        //    //    upUserLevel3.DistributionSaleAmountLevel3 = GetUserOrderTotalAmountByLevel(upUserLevel3.UserID, 3);
        //    //    Update(upUserLevel3);
        //    //}


        //}

        /// <summary>
        /// 更新会员分销上下级人数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool SynDistributionCount(string websiteOwner)
        {

            string strWhere = string.Format(" WebsiteOwner='{0}' And DistributionOwner!='' Or UserId='{0}'", websiteOwner);
            int totalCount = GetCount<UserInfo>(strWhere);
            int pageSize = 10;
            int totalPage = GetTotalPage(totalCount, pageSize);
            for (int i = 1; i <= totalPage; i++)
            {
                var userList = GetLit<UserInfo>(pageSize, i, strWhere);
                foreach (var user in userList)
                {
                    if (!IsChannel(user))
                    {
                        user.DistributionDownUserCountLevel1 = GetDownUserCount(user.UserID, 1);
                        //user.DistributionDownUserCountLevel2 = GetDownUserCount(user.UserID, 2);
                        //user.DistributionDownUserCountLevel3 = GetDownUserCount(user.UserID, 3);
                        Update(user);
                    }
                    else
                    {
                        user.DistributionDownUserCountLevel1 = GetChannelAllChildUser(user.UserID).Count;

                    }

                }

                System.Threading.Thread.Sleep(5000);

            }
            return true;


        }


        /// <summary>
        /// 更新会员分销销售额
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public bool SynDistributionSaleAmount(string websiteOwner)
        {

            string strWhere = string.Format(" WebsiteOwner='{0}' And DistributionOwner!='' Or UserId='{0}'", websiteOwner);
            int totalCount = GetCount<UserInfo>(strWhere);
            int pageSize = 100;
            int totalPage = GetTotalPage(totalCount, pageSize);
            for (int i = 1; i <= totalPage; i++)
            {
                var userList = GetLit<UserInfo>(pageSize, i, strWhere);
                foreach (var user in userList)
                {
                    if (!IsChannel(user))
                    {
                        user.DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(user);
                        user.DistributionSaleAmountLevel1 = GetUserOrderTotalAmountByLevel(user.UserID, 1);
                        //user.DistributionSaleAmountLevel2 = GetUserOrderTotalAmountByLevel(user.UserID, 2);
                        //user.DistributionSaleAmountLevel3 = GetUserOrderTotalAmountByLevel(user.UserID, 3);
                        Update(user);
                    }
                    else
                    {
                        user.DistributionSaleAmountLevel1 = GetChannelSaleAmount(user.UserID, user.WebsiteOwner);
                    }

                }

                System.Threading.Thread.Sleep(5000);

            }
            return true;


        }

        /// <summary>
        /// 更新上级销售额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateDistributionSaleAmountUp(WXMallOrderInfo orderInfo)
        {
            BLLMall bllMall = new BLLMall();
            if (orderInfo.TotalAmount < 0)
            {
                orderInfo.TotalAmount = 0;
            }
            if (orderInfo.OrderType == 0 || orderInfo.OrderType == 1 || orderInfo.OrderType == 2)
            {

                var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                if (orderUserInfo != null)
                {
                    orderUserInfo.DistributionSaleAmountLevel0 = GetDistributionSaleAmountLevel0(orderUserInfo);//更新自己销售额
                    bllUser.Update(orderUserInfo);
                }
                var upUserLevel1 = GetUpUser(orderInfo.OrderUserID);
                if (upUserLevel1 != null && (!IsChannel(upUserLevel1)))
                {
                    upUserLevel1.DistributionSaleAmountLevel1 = GetUserOrderTotalAmountByLevel(upUserLevel1.UserID, 1);
                    upUserLevel1.DistributionSaleAmountAll = GetDisSaleAmount(upUserLevel1.UserID);
                    Update(upUserLevel1);
                }


                //UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                #region 给渠道更新销售额
                UserInfo channelUserInfo;//渠道
                if (!string.IsNullOrEmpty(orderUserInfo.Channel))
                {

                    #region 先给最底层的渠道更新销售额
                    channelUserInfo = bllUser.GetUserInfo(orderUserInfo.Channel, orderInfo.WebsiteOwner);
                    if (channelUserInfo != null)//
                    {

                        channelUserInfo.DistributionSaleAmountLevel1 = GetChannelAllFirstLevelOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                        channelUserInfo.DistributionSaleAmountAll = GetChannelAllOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                        Update(channelUserInfo);
                    }
                    #endregion

                    #region 依次给上级渠道更新销售额
                    while (channelUserInfo != null)
                    {
                        if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
                        {
                            break;
                        }
                        channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, orderInfo.WebsiteOwner);
                        if (channelUserInfo != null)
                        {
                            channelUserInfo.DistributionSaleAmountLevel1 = GetChannelAllFirstLevelOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                            channelUserInfo.DistributionSaleAmountAll = GetChannelAllOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                            Update(channelUserInfo);

                        }


                    }
                    #endregion



                }


                #endregion

                //#region 渠道更新销售额
                //if (upUserLevel1 != null)
                //{
                //    if (!string.IsNullOrEmpty(upUserLevel1.IsFirstLevelDistribution))
                //    {
                //        if (upUserLevel1.IsFirstLevelDistribution.Trim() == "1")
                //        {
                //            if (!string.IsNullOrEmpty(upUserLevel1.ParentChannel))
                //            {

                //                #region 最底层渠道
                //                UserInfo channelUserInfo = bllUser.GetUserInfo(upUserLevel1.ParentChannel, orderInfo.WebsiteOwner);
                //                //.Get<UserInfo>(string.Format(" UserId='{0}' And WebsiteOwner='{1}'", upUserLevel1.DistributionOwner, orderInfo.WebsiteOwner));//渠道第一级用户
                //                if (channelUserInfo != null)//
                //                {
                //                    channelUserInfo.DistributionSaleAmountLevel1 = GetChannelAllFirstLevelOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                //                    channelUserInfo.DistributionSaleAmountAll = GetChannelAllOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);


                //                    Update(channelUserInfo);
                //                }
                //                #endregion


                //                #region 依次给上级渠道更新销售额
                //                while (channelUserInfo != null)
                //                {
                //                    if (string.IsNullOrEmpty(channelUserInfo.ParentChannel))
                //                    {
                //                        break;
                //                    }
                //                    channelUserInfo = bllUser.GetUserInfo(channelUserInfo.ParentChannel, orderInfo.WebsiteOwner);
                //                    if (channelUserInfo != null)
                //                    {
                //                        channelUserInfo.DistributionSaleAmountLevel1 = GetChannelAllFirstLevelOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                //                        channelUserInfo.DistributionSaleAmountAll = GetChannelAllOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                //                        Update(channelUserInfo);

                //                    }


                //                }
                //                #endregion

                //            }


                //        }
                //    }

                //}
                //#endregion


                #region 供应商渠道更新销售额




                if (!string.IsNullOrEmpty(orderInfo.SupplierUserId))
                {

                    var suppLierInfo = bllUser.GetUserInfo(orderInfo.SupplierUserId, orderInfo.WebsiteOwner);
                    if (suppLierInfo != null)
                    {
                        channelUserInfo = bllUser.GetUserInfo(suppLierInfo.ParentChannel, orderInfo.WebsiteOwner);
                        if (channelUserInfo != null)
                        {
                            channelUserInfo.DistributionSaleAmountAll = GetSupplierChannelSaleAmount(channelUserInfo.UserID);
                            Update(channelUserInfo, string.Format(" DistributionSaleAmountAll={0}", channelUserInfo.DistributionSaleAmountAll), string.Format(" AutoId={0}", channelUserInfo.AutoID));
                        }

                    }

                }
                if (orderInfo.IsMain == 1)
                {
                    List<WXMallOrderInfo> orderList = bllMall.GetList<WXMallOrderInfo>(string.Format("ParentOrderId='{0}'", orderInfo.OrderID));

                    foreach (var order in orderList)
                    {

                        if (!string.IsNullOrEmpty(order.SupplierUserId))
                        {

                            var suppLierInfo = bllUser.GetUserInfo(order.SupplierUserId, order.WebsiteOwner);
                            if (suppLierInfo != null)
                            {
                                channelUserInfo = bllUser.GetUserInfo(suppLierInfo.ParentChannel, order.WebsiteOwner);
                                if (channelUserInfo != null)
                                {
                                    channelUserInfo.DistributionSaleAmountAll = GetSupplierChannelSaleAmount(channelUserInfo.UserID);
                                    Update(channelUserInfo, string.Format(" DistributionSaleAmountAll={0}", channelUserInfo.DistributionSaleAmountAll), string.Format(" AutoId={0}", channelUserInfo.AutoID));
                                }

                            }

                        }

                    }
                }


                #endregion



            }




            return true;


        }
        /// <summary>
        /// 截图(截取页面)
        /// </summary>
        /// <param name="autoid"></param>
        /// <param name="host"></param>
        /// <returns>物理文件路径</returns>
        public string PageScreenshot(string openId, string host)
        {

            UserInfo userInfo = bllUser.GetUserInfoByOpenIdClient(openId);
            string url = "http://" + host + "/App/Cation/Wap/Mall/Distribution/QCodePage.aspx?autoid=" + userInfo.AutoID;
            string filePath = "/FileUpload/QRCodePage/" + Guid.NewGuid() + ".jpg";
            filePath = filePath.Replace("/", "\\");
            string websitePath = Common.ConfigHelper.GetConfigString("WebSitePath");

            if (string.IsNullOrWhiteSpace(websitePath))
            {
                websitePath = "D:\\WebSite\\CommonPlatform";
            }

            string fileServerPath = string.Format("{1}{0}", filePath, websitePath);//本地磁盘路径
            // string fileServerPath = System.Web.HttpContext.Current.Server.MapPath(filePath);
            WebsiteToImageHelper websiteToImage = new WebsiteToImageHelper(url);
            Bitmap bMap = websiteToImage.Generate();
            bMap.Save(fileServerPath);
            return fileServerPath;
        }

        /// <summary>
        /// 创建用户的个性二维码
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public string CreateUserDistributionImage(string openId, string websiteOwner)
        {

            ToLog("CreateUserDistributionImage", @"d:\CreateUserDistributionImage.txt");

            BLLWebSite bllWebSite = new BLLWebSite();
            BLLJuActivity bllJuActivity = new BLLJuActivity();
            BLLWebSite bllWebsite = new BLLWebSite();
            string result = string.Empty;

            ToLog("CreateUserDistributionImage 开始获取config", @"d:\CreateUserDistributionImage.txt");
            CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig(websiteOwner);
            /*
             * 获取背景图本地地址
             * 获取用户分销二维码原图
             * 获取用户头像
             * 获取用户昵称
             * 
             */
            ToLog("CreateUserDistributionImage 已获取config", @"d:\CreateUserDistributionImage.txt");


            string websitePath = Common.ConfigHelper.GetConfigString("WebSitePath");

            if (string.IsNullOrWhiteSpace(websitePath))
            {
                websitePath = "D:\\WebSite\\CommonPlatform";
            }

            ToLog("开始获取websiteInfo", @"d:\CreateUserDistributionImage.txt");
            var websiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);

            if (websiteInfo == null)
            {
                return "";
            }
            ToLog("已获取websiteInfo", @"d:\CreateUserDistributionImage.txt");

            ToLog("DistributionShareQrcodeBgImg:" + websiteInfo.DistributionShareQrcodeBgImg, @"d:\CreateUserDistributionImage.txt");
            if (string.IsNullOrWhiteSpace(websiteInfo.DistributionShareQrcodeBgImg))
            {
                websiteInfo.DistributionShareQrcodeBgImg = "http://files.comeoncloud.net/img/gxfc.png";
            }

            //背景图
            var bgImg = websitePath + bllJuActivity.DownLoadRemoteImageLocal(websiteInfo.DistributionShareQrcodeBgImg);
            ToLog("已下载背景图:" + bgImg, @"d:\CreateUserDistributionImage.txt");
            var userInfo = bllUser.GetUserInfoByOpenId(openId, websiteOwner);

            if (userInfo == null)
            {
                return "";
            }

            ToLog("CreateUserDistributionImage  开始处理图片", @"d:\CreateUserDistributionImage.txt");

            //复制一个背景图出来处理,图片拉伸到 650，高度1008
            var newBgImg = websitePath + @"\FileUpload\ImageMapping\" + Guid.NewGuid().ToString() + Path.GetExtension(bgImg);
            Common.ImageHelper.ImageClass.ZoomPicture(bgImg, newBgImg, 650, 1008);

            //用户头像、微信昵称 与二维码集合

            if (config.IsHideHeadImg == 0)
            {
                //头像固定位置：
                //获取头像
                var userAvatar = websitePath + bllJuActivity.DownLoadRemoteImageLocal(bllUser.GetUserDispalyAvatar(userInfo));

                //头像固定大小：88px*88px
                //Common.ImageHelper.ImageClass.ZoomPicture(userAvatar, userAvatar, 92, 92);
                result = Common.ImageHelper.ImageClass.ImageWatermark(newBgImg, userAvatar, 278, 30, 92, 92, true);
            }




            var nikNameFontColor = Color.DarkRed;
            if (!string.IsNullOrEmpty(config.WXNickNameFontColor) && config.WXNickNameFontColor.Contains("#"))
            {
                nikNameFontColor = System.Drawing.ColorTranslator.FromHtml(config.WXNickNameFontColor);
            }
            var nikNameFontRectangle = new Rectangle(0, -200, 0, 0);
            var nikNameFont = new Font("微软雅黑", 16, FontStyle.Bold);
            if (config.IsShowWXNickName == 0)
            {
                if (config.WXNickShowPosition == 1)
                {
                    nikNameFontRectangle = new Rectangle(0, 836, 640, 30);
                }
                else
                {
                    nikNameFontRectangle = new Rectangle(0, 142, 640, 30);
                }
            }
            //TODO:增加用户昵称颜色可配置
            //if (websiteOwner == "lanyueliang")
            //{
            //    nikNameFontColor = Color.White;
            //}



            result = Common.ImageHelper.ImageClass.LetterWatermark(
                    newBgImg,
                    nikNameFont,
                    "我是 " + bllUser.GetUserDispalyName(userInfo),
                    nikNameFontColor,
                    nikNameFontRectangle,
                    new StringFormat() { Alignment = StringAlignment.Center }
                );

            ToLog("CreateUserDistributionImage 分销二维码 开始", @"d:\CreateUserDistributionImage.txt");

            var distributionWxQrcodeLimitUrl = string.Empty;
            bllWeixin.GetDistributionWxQrcodeLimit(out distributionWxQrcodeLimitUrl, userInfo.UserID, "", websiteOwner);

            ToLog("CreateUserDistributionImage 获取到的distributionWxQrcodeLimitUrl：" + distributionWxQrcodeLimitUrl, @"d:\CreateUserDistributionImage.txt");

            //分销二维码
            var qrcondeUrl = websitePath + bllJuActivity.DownLoadRemoteImageLocal(distributionWxQrcodeLimitUrl);

            ToLog("qrcondeUrl：" + qrcondeUrl, @"d:\CreateUserDistributionImage.txt");

            if (!string.IsNullOrEmpty(config.DistributionQRCodeIcon) && !string.IsNullOrEmpty(qrcondeUrl))
            {
                qrcondeUrl = websitePath + bllWeixin.GetQRCodeImgLocal(qrcondeUrl, config.DistributionQRCodeIcon);
            }

            result = Common.ImageHelper.ImageClass.ImageWatermark(newBgImg, qrcondeUrl, 205, 585, 240, 240);
            ToLog("处理完毕，返回路径：" + result, @"d:\CreateUserDistributionImage.txt");
            return result;
        }

        /// <summary>
        /// 获取分销二维码
        /// </summary>
        /// <param name="task">任务信息
        /// Receivers 存储openid
        /// TaskInfo  存储域名
        /// </param>
        /// <returns></returns>
        public bool GetDistributionImage(TimingTask task)
        {

            string filePath = CreateUserDistributionImage(task.Receivers, task.WebsiteOwner);//PageScreenshot(task.Receivers, task.TaskInfo);

            if (!string.IsNullOrEmpty(filePath))
            {
                string mediaIdUpload = bllWeixin.UploadFileToWeixinModel(bllWeixin.GetAccessToken(task.WebsiteOwner), "image", filePath).media_id;
                if (!string.IsNullOrEmpty(mediaIdUpload))
                {
                    bllWeixin.SendKeFuMessageImage(bllWeixin.GetAccessToken(task.WebsiteOwner), task.Receivers, mediaIdUpload);

                }


            }
            return false;

        }

        /// <summary>
        /// 累计销售 自己加下级销售额
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public decimal GetUserSalesQuota(UserInfo userInfo)
        {
            decimal result = 0;

            result += userInfo.DistributionSaleAmountLevel1;

            result +=
                bllUser.GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(
                string.Format("OrderUserID='{0}' And DistributionStatus in(1,2,3) And OrderType in(0,1,2) And IsRefund!=1 And Status!='已取消' And TotalAmount>0 And IsNull(IsMain,0)=0",
                    userInfo.UserID)
                ).Sum(p => p.TotalAmount);

            return result;
        }
        /// <summary>
        /// 获取自己消费额
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public decimal GetDistributionSaleAmountLevel0(UserInfo userInfo)
        {


            string sql = string.Format("select Sum(TotalAmount) From ZCJ_WXMallOrderInfo Where  OrderUserID='{0}' And WebsiteOwner='{1}' And OrderType in(0,1,2) And IsRefund!=1 And Status!='已取消' And PaymentStatus=1 And IsNull(IsMain,0)=0", userInfo.UserID, userInfo.WebsiteOwner);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
            if (result != null)
            {
                return decimal.Parse(result.ToString());
            }
            return 0;



        }


        ///// <summary>
        ///// 设置上级分销员
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="distributionOwnerId"></param>
        ///// <param name="userInfo"></param>
        ///// <param name="distributionOwner"></param>
        ///// <returns></returns>
        //public bool SetUserDistributionOwner(string userId, string distributionOwnerId, UserInfo userInfo = null, UserInfo distributionOwner = null)
        //{
        //    bool result = false;

        //    if (userInfo == null) userInfo = bllUser.GetUserInfo(userId);

        //    if (distributionOwner == null) distributionOwner = bllUser.GetUserInfo(distributionOwnerId);

        //    if (userInfo == null || distributionOwner == null) return false;



        //    return result;
        //}

        /// <summary>
        /// 查询自己及所有子级渠道
        /// </summary>
        /// <param name="userId">渠道账号</param>
        /// <returns></returns>
        public List<UserInfo> GetAllChildChannel(string userId)
        {

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("with cte as");
            sbWhere.AppendFormat("(");
            sbWhere.AppendFormat(" select UserID,Channel from ZCJ_UserInfo where UserId = '{0}'", userId);
            sbWhere.AppendFormat(" union all");
            sbWhere.AppendFormat(" select a.UserID,a.Channel from ZCJ_UserInfo a join cte b on a.ParentChannel = b.UserId");
            sbWhere.AppendFormat(")");
            sbWhere.AppendFormat(" select * from cte");

            return ZentCloud.ZCBLLEngine.BLLBase.Query<UserInfo>(sbWhere.ToString());



        }

        /// <summary>
        /// 获取用户管理的渠道账号
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetChannelUserId(UserInfo userInfo)
        {

            var channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", userInfo.UserID));
            if (channelUserInfo != null)
            {
                return channelUserInfo.UserID;
            }
            return "";

        }




        /// <summary>
        /// 获取渠道的所有下线会员 包括二维码
        /// </summary>
        /// <param name="userId">渠道账号</param>
        /// <returns></returns>
        public List<UserInfo> GetChannelAllChildUser(string userId)
        {
            List<UserInfo> data = new List<UserInfo>();
            var childChannelList = GetAllChildChannel(userId);
            foreach (var item in childChannelList)
            {
                if (!string.IsNullOrEmpty(item.Channel))
                {
                    data.AddRange(GetChannelUser(item.Channel));
                }

            }
            data = data.Where(p => !p.PermissionGroupID.HasValue).ToList();//下级渠道的不算
            data = data.DistinctBy(p => p.UserID).ToList();
            return data;

        }
        /// <summary>
        /// 获取渠道会员
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetChannelUser(string channelUserId)
        {
            return GetList<UserInfo>(string.Format("Channel='{0}'", channelUserId));
        }
        /// <summary>
        /// 获取渠道下分销员一级会员数 二维码发展的下一级会员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserInfo> GetChannelAllFirstLevelChildUser(string userId, string websiteOwner)
        {
            List<UserInfo> data = new List<UserInfo>();
            var userList = bllUser.GetList<UserInfo>(string.Format("WebsiteOwner='{0}' And ParentChannel='{1}' And IsFirstLevelDistribution='1'", websiteOwner, userId));
            foreach (var item in userList)
            {
                data.AddRange(GetDownUserList(item.UserID));
            }
            data = data.Where(p => p.UserID != userId).Where(p => p.Channel == userId).ToList();
            data = data.DistinctBy(p => p.UserID).ToList();
            return data;
        }
        /// <summary>
        ///直接销售
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetChannelAllFirstLevelOrder(string userId, string websiteOwner)
        {

            List<UserInfo> userList = GetChannelAllFirstLevelChildUser(userId, websiteOwner);
            string userIds = "";
            foreach (var user in userList)
            {
                userIds += "'" + user.UserID + "',";

            }
            userIds = userIds.TrimEnd(',');
            if (string.IsNullOrEmpty(userIds.Trim()))
            {
                return new List<WXMallOrderInfo>();
            }
            return GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And OrderUserId in ({1}) And Status!='已取消' And IsRefund=0 And TotalAmount>0 And IsNull(IsMain,0)=0  Order by InsertDate DESC", websiteOwner, userIds));



        }
        /// <summary>
        /// 获取渠道的所有下线订单
        /// </summary>
        /// <param name="userId">渠道账号</param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetChannelAllOrder(string userId, string websiteOwner)
        {

            List<UserInfo> userList = GetChannelAllChildUser(userId);
            string userIds = "";
            foreach (var user in userList)
            {
                userIds += "'" + user.UserID + "',";

            }
            userIds = userIds.TrimEnd(',');
            if (string.IsNullOrEmpty(userIds.Trim()))
            {
                return new List<WXMallOrderInfo>();
            }
            return GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And OrderUserId in ({1}) And Status!='已取消' And IsRefund=0 And TotalAmount>0 And IsNull(IsMain,0)=0 Order by InsertDate DESC", websiteOwner, userIds));



        }
        /// <summary>
        /// 获取渠道销售额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetChannelSaleAmount(string userId, string websiteOwner)
        {

            return GetChannelAllOrder(userId, websiteOwner).Sum(p => p.TotalAmount);
        }

        /// <summary>
        /// 获取所有下线会员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllDownUsersList(string userId)
        {

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("with cte as");
            sbWhere.AppendFormat("(");
            sbWhere.AppendFormat(" select AutoId,UserID,PermissionGroupID,IsFirstLevelDistribution from ZCJ_UserInfo where UserId = '{0}'", userId);
            sbWhere.AppendFormat(" union all");
            sbWhere.AppendFormat(" select a.AutoId,a.UserID,a.PermissionGroupID,a.IsFirstLevelDistribution from ZCJ_UserInfo a join cte b on a.Distributionowner = b.UserId");
            sbWhere.AppendFormat(")");
            sbWhere.AppendFormat(" select * from cte");

            return ZentCloud.ZCBLLEngine.BLLBase.Query<UserInfo>(sbWhere.ToString());



        }

        /// <summary>
        /// 获取分销员的所有下线订单
        /// </summary>
        /// <param name="userId">渠道账号</param>
        /// <returns></returns>
        public List<WXMallOrderInfo> GetDisAllOrder(string userId)
        {

            List<UserInfo> userList = GetAllDownUsersList(userId);
            string userIds = "";
            foreach (var user in userList)
            {
                userIds += "'" + user.UserID + "',";

            }
            userIds = userIds.TrimEnd(',');
            if (string.IsNullOrEmpty(userIds.Trim()))
            {
                return new List<WXMallOrderInfo>();
            }
            return GetList<ZentCloud.BLLJIMP.Model.WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' And OrderUserId in ({1}) And Status!='已取消' And IsRefund=0 And TotalAmount>0 And IsNull(IsMain,0)=0 Order by InsertDate DESC", bllUser.WebsiteOwner, userIds));



        }
        /// <summary>
        /// 获取分销员累计销售额 自己及所有下线 无限级销售额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetDisSaleAmount(string userId)
        {
            return GetDisAllOrder(userId).Sum(p => p.TotalAmount);
        }

        /// <summary>
        /// 更新某一站点渠道数据
        /// </summary>
        public bool FlashChannelData(string websiteOwner)
        {


            int successCount = 0;
            var allChannelList = bllUser.GetList<UserInfo>(string.Format(" PermissionGroupID={0} And WebsiteOwner='{1}'", GetChannelPermissionGroupId(), websiteOwner));
            foreach (var channelUserInfo in allChannelList)
            {
                channelUserInfo.DistributionDownUserCountLevel1 = GetChannelAllFirstLevelChildUser(channelUserInfo.UserID, websiteOwner).Count;//直接会员数量
                channelUserInfo.DistributionDownUserCountAll = GetChannelAllChildUser(channelUserInfo.UserID).Count();//所有会员数量
                channelUserInfo.DistributionSaleAmountLevel1 = GetChannelAllFirstLevelOrder(channelUserInfo.UserID, websiteOwner).Sum(s => s.TotalAmount);
                channelUserInfo.DistributionSaleAmountAll = GetChannelAllOrder(channelUserInfo.UserID, channelUserInfo.WebsiteOwner).Sum(s => s.TotalAmount);
                if (Update(channelUserInfo))
                {
                    successCount++;
                }

            }

            return successCount > 0;



        }
        /// <summary>
        /// 获取用户所属渠道
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetUserChannel(UserInfo userInfo)
        {
            return userInfo.Channel;
            //string channelUserId = "";
            //try
            //{
            //    if (!string.IsNullOrEmpty(userInfo.IsFirstLevelDistribution) && !string.IsNullOrEmpty(userInfo.ParentChannel))
            //    {
            //        UserInfo channelUserInfo = bllUser.GetUserInfo(userInfo.ParentChannel);
            //        if (channelUserInfo != null)
            //        {
            //            return channelUserInfo.UserID;
            //        }
            //    }
            //    else
            //    {
            //        do
            //        {
            //            userInfo = GetUpUser(userInfo.UserID);
            //            if (userInfo != null && !string.IsNullOrEmpty(userInfo.IsFirstLevelDistribution) && !string.IsNullOrEmpty(userInfo.ParentChannel))
            //            {
            //                UserInfo channelUserInfo = bllUser.GetUserInfo(userInfo.ParentChannel);
            //                if (channelUserInfo != null)
            //                {
            //                    return channelUserInfo.UserID;
            //                }

            //            }


            //        } while (userInfo != null);
            //    }
            //}
            //catch (Exception ex)
            //{


            //}

            //return channelUserId;


        }


        /// <summary>
        /// 根据订单，获取用户分佣金额(分销员)
        /// </summary>
        /// <param name="orderInfo">订单信息</param>
        /// <param name="userInfo">用户信息(已弃用)</param>
        /// <param name="level">等级 0直销 1第一级</param>
        /// <returns></returns>
        public decimal GetUserCommission(WXMallOrderInfo orderInfo, UserInfo userInfo, int level)
        {
            BLLMall bllMall = new BLLMall();
            UserInfo orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
            bool isFirst = IsFirstOrder(orderInfo);
            decimal totalAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;
            List<WXMallRefund> refundList = GetList<WXMallRefund>(string.Format(" OrderId='{0}' And Status=6", orderInfo.OrderID));
            totalAmount -= refundList.Sum(p => p.RefundAmount);


            if (totalAmount <= 0)
            {
                //totalAmount = 0;
                return 0;
            }

            double rate = 0;//原始等级比例
            //decimal amount = 0;//分佣金额
            switch (level)
            {
                case 0://直销
                    rate = GetDistributionRate(orderUserInfo, 0, isFirst);//直销提成比例
                    //amount = ((decimal)rate / 100) * totalAmount;//直销提成金额
                    break;
                case 1://一级
                    //查询上级用户的一级分佣比例
                    var orderPreUserInfo = GetUpUser(orderUserInfo.UserID, 1);
                    rate = GetDistributionRate(orderPreUserInfo, 1, isFirst);//直销提成比例
                    //amount = ((decimal)rate / 100) * totalAmount;//直销提成金额
                    break;
                default:
                    break;
            }

            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);

            decimal basePriceSum = 0;//基础价总和
            foreach (var orderDetail in orderDetailList)
            {
                basePriceSum += orderDetail.BasePrice * orderDetail.TotalCount;
            }

            //分佣金额
            decimal commAmount = Math.Round((totalAmount / orderInfo.Product_Fee) * (orderInfo.Product_Fee - basePriceSum) * ((decimal)rate / 100), 2);
            return commAmount > 0 ? commAmount : 0;

            //#region 商品分佣按比例 计算分佣金额
            //foreach (var orderDetail in orderDetailList)
            //{
            //    var productSku = bllMall.GetProductSku((int)orderDetail.SkuId);
            //    var productInfo = bllMall.GetProduct(productSku.ProductId);
            //    if (IsSingleProduct(productInfo))
            //    {
            //        productSku.CommissionRate = productInfo.CommissionRate;
            //    }
            //    if (string.IsNullOrEmpty(productSku.CommissionRate))
            //    {
            //        productSku.CommissionRate = "100";
            //    }
            //    decimal amountItem = 0;
            //    decimal rate1 = ((decimal)orderDetail.OrderPrice * orderDetail.TotalCount) / (orderDetailList.Sum(p => (decimal)p.OrderPrice * p.TotalCount));//商品占总订单的百分比
            //    amountItem = totalAmount * rate1 * ((decimal)rate / 100) * (decimal.Parse(productSku.CommissionRate) / 100);
            //    amount += amountItem;

            //    //每种商品分佣金额=订单金额(扣掉运费)*等级比例*商品占应付的百分比*商品的分佣比例
            //    //100*0.5*0.5*1


            //}





            //#endregion

            //return amount;


        }

        /// <summary>
        /// 渠道供应商销售额
        /// </summary>
        /// <returns></returns>
        public decimal GetSupplierChannelSaleAmount(string userId)
        {



            string sqlTotalAmount = string.Format("Select Sum(TotalAmount) from ZCJ_WXMallOrderInfo where SupplierUserId in(select UserId from ZCJ_UserInfo where ParentChannel='{0}') And OrderType In(0,1,2) And PaymentStatus=1 And Status!='已取消' And IsNull(IsMain,0)=0", userId);
            var totalAmount = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlTotalAmount);
            if (totalAmount != null)
            {
                return decimal.Parse(totalAmount.ToString());

            }
            return 0;


        }
        /// <summary>
        /// 获取渠道分佣金额
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="rootUserInfo"></param>
        /// <returns></returns>
        public decimal GetChannelCommissin(WXMallOrderInfo orderInfo, UserInfo channelUserInfo)
        {


            BLLMall bllMall = new BLLMall();
            decimal totalAmount = orderInfo.TotalAmount - orderInfo.Transport_Fee;
            if (totalAmount <= 0)
            {
                //totalAmount = 0;
                return 0;
            }
            double channelRate = GetDistributionRateChannel(channelUserInfo);
            List<ProductSku> skuList = new List<ProductSku>();
            var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            foreach (var orderDetail in orderDetailList)
            {

                var productSku = bllMall.GetProductSku((int)orderDetail.SkuId);
                skuList.Add(productSku);

            }
            decimal basePriceSum = 0;//基础价总和
            foreach (var orderDetail in orderDetailList)
            {
                basePriceSum += skuList.Single(p => p.SkuId == (int)orderDetail.SkuId).BasePrice * orderDetail.TotalCount;

            }
            //分佣金额
            decimal commAmount = Math.Round((totalAmount / orderInfo.Product_Fee) * (orderInfo.Product_Fee - basePriceSum) * ((decimal)channelRate / 100), 2);
            return commAmount > 0 ? commAmount : 0;

            //decimal channelAmount = 0;

            //#region 商品分佣按比例 计算分佣金额
            //var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
            //foreach (var orderDetail in orderDetailList)
            //{
            //    var productSku = bllMall.GetProductSku((int)orderDetail.SkuId);
            //    var productInfo = bllMall.GetProduct(productSku.ProductId);
            //    if (IsSingleProduct(productInfo))
            //    {
            //        productSku.CommissionRate = productInfo.CommissionRate;
            //    }
            //    if (string.IsNullOrEmpty(productSku.CommissionRate))
            //    {
            //        productSku.CommissionRate = "100";
            //    }

            //    decimal amountItem = 0;
            //    decimal rate1 = (decimal)orderDetail.OrderPrice * orderDetail.TotalCount / (orderDetailList.Sum(p => (decimal)p.OrderPrice * p.TotalCount));//商品占应付百分比
            //    amountItem = totalAmount * rate1 * ((decimal)channelRate / 100) * (decimal.Parse(productSku.CommissionRate) / 100);
            //    channelAmount += amountItem;
            //    //每种商品分佣金额=订单金额(扣掉运费)*渠道等级比例*商品占应付的百分比*商品的分佣比例
            //}


            //#endregion

            //return channelAmount;

        }


        /// <summary>
        /// 自动设置会员等级
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool AutoUpdateLevel(WXMallOrderInfo order)
        {
            WXMallOrderInfo orderInfo = new WXMallOrderInfo();
            orderInfo.WebsiteOwner = order.WebsiteOwner;
            orderInfo.OrderUserID = order.OrderUserID;
            WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase(orderInfo.WebsiteOwner);
            ToLog("AutoUpdateLevel" + ZentCloud.Common.JSONHelper.ObjectToJson(websiteInfo));
            if (websiteInfo.DistributionGetWay == 1)
            {

                if (websiteInfo.AutoUpdateLevelMinAmout > 0 && (!string.IsNullOrEmpty(websiteInfo.AutoUpdateLevelId)))
                {

                    string sql = string.Format("select Sum(TotalAmount) From ZCJ_WXMallOrderInfo Where  OrderUserID='{0}' And WebsiteOwner='{1}' And OrderType in(0,1,2)  And PaymentStatus=1 And IsNull(IsMain,0)=0", orderInfo.OrderUserID, orderInfo.WebsiteOwner);

                    var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
                    if (result != null)
                    {
                        orderInfo.TotalAmount = decimal.Parse(result.ToString());
                    }
                    string sqlRefundAmount = string.Format("select Sum(RefundAmount) From ZCJ_WXMallRefund Where  UserId='{0}' And Status=6 ", orderInfo.OrderUserID);
                    var resultRefund = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundAmount);
                    if (resultRefund != null)
                    {
                        orderInfo.TotalAmount -= decimal.Parse(resultRefund.ToString());
                    }

                    if (orderInfo.TotalAmount >= websiteInfo.AutoUpdateLevelMinAmout)
                    {
                        var userInfo = bllUser.GetUserInfo(orderInfo.OrderUserID, orderInfo.WebsiteOwner);
                        if (userInfo.MemberLevel == 0)
                        {
                            UserLevelConfig level = QueryUserLevel(orderInfo.WebsiteOwner, websiteInfo.AutoUpdateLevelId);
                            if (Update(userInfo, string.Format("MemberLevel={0},Ex15=1", level.LevelNumber), string.Format("AutoId={0}", userInfo.AutoID)) > 0)
                            {
                                return true;
                            }

                        }
                    }

                }


            }
            return false;

        }

        /// <summary>
        /// 取消自动设置会员等级
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool CancelUpdateLevel(UserInfo userInfo)
        {
            if (!string.IsNullOrEmpty(userInfo.Ex15))
            {

                WebsiteInfo websiteInfo = GetWebsiteInfoModelFromDataBase(userInfo.WebsiteOwner);
                if (websiteInfo.DistributionGetWay == 1)
                {
                    if (websiteInfo.AutoUpdateLevelMinAmout > 0 && (!string.IsNullOrEmpty(websiteInfo.AutoUpdateLevelId)))
                    {

                        decimal totalAmount = 0;
                        string sql = string.Format("select Sum(TotalAmount) From ZCJ_WXMallOrderInfo Where  OrderUserID='{0}' And WebsiteOwner='{1}' And OrderType in(0,1,2) And IsRefund!=1 And Status!='已取消' And PaymentStatus=1 And IsNull(IsMain,0)=0", userInfo.UserID, userInfo.WebsiteOwner);

                        var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sql);
                        if (result != null)
                        {
                            totalAmount = decimal.Parse(result.ToString());
                        }
                        string sqlRefundAmount = string.Format("select Sum(RefundAmount) From ZCJ_WXMallRefund Where  UserId='{0}' And Status=6 ", userInfo.UserID);
                        var resultRefund = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sqlRefundAmount);
                        if (resultRefund != null)
                        {
                            totalAmount -= decimal.Parse(resultRefund.ToString());
                        }
                        if (totalAmount < websiteInfo.AutoUpdateLevelMinAmout)
                        {

                            if (Update(userInfo, string.Format("MemberLevel=0,Ex15=''"), string.Format("AutoId={0}", userInfo.AutoID)) > 0)
                            {
                                return true;
                            }


                        }

                    }


                }


            }


            return false;

        }
        ///// <summary>
        ///// 是否是单品
        ///// </summary>
        //public bool IsSingleProduct(WXMallProductInfo productInfo)
        //{

        //    BLLMall bllMall = new BLLMall();
        //    var skuList = bllMall.GetProductSkuList(int.Parse(productInfo.PID));
        //    if (skuList.Count > 0)
        //    {

        //        if (skuList.Where(p => p.Props == null || p.Props == "").ToList().Count == 0)
        //        {
        //            return false;//非单品
        //        }
        //        else
        //        {
        //            return true;//单品
        //        }

        //    }
        //    return true;//单品


        //}


        ///// <summary>
        ///// 预计分佣模型
        ///// </summary>
        //public class PredictionCommission
        //{
        //    /// <summary>
        //    /// 提成比例
        //    /// </summary>
        //    public decimal Rate { get; set; }
        //    /// <summary>
        //    /// 提成金额
        //    /// </summary>
        //    public decimal Amount { get; set; }

        //}

        /// <summary>
        /// 分销订单状态
        /// </summary>
        public enum DistributionStatus
        {
            /// <summary>
            /// 未付款
            /// </summary>
            NotPay = 0,
            /// <summary>
            /// 已付款
            /// </summary>
            Paied = 1,
            /// <summary>
            /// 已收货
            /// </summary>
            Received = 2,
            /// <summary>
            /// 已审核
            /// </summary>
            Verified = 3,
            /// <summary>
            /// 已提现
            /// </summary>
            Withdraw = 4
        }
        /// <summary>
        /// 获取分销渠道用户组Id
        /// </summary>
        /// <returns></returns>
        public string GetChannelPermissionGroupId()
        {

            BLLPermission.Model.PermissionGroupInfo model = Get<BLLPermission.Model.PermissionGroupInfo>(string.Format("GroupType=4"));
            if (model != null)
            {
                return model.GroupID.ToString();
            }
            return "";

        }
        /// <summary>
        /// 是否渠道
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true 是渠道</returns>
        public bool IsChannel(UserInfo userInfo)
        {

            var channelPerGroupId = GetChannelPermissionGroupId();
            if (userInfo.PermissionGroupID.HasValue && (!string.IsNullOrEmpty(channelPerGroupId)))
            {
                if (userInfo.PermissionGroupID.Value == long.Parse(channelPerGroupId))
                {
                    return true;
                }
            }
            if (GetCount<UserInfo>(string.Format(" ParentChannel='{0}'", userInfo.UserID)) > 0)//有下级渠道
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 是否渠道管理员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>true 是渠道</returns>
        public bool IsChannelMgr(UserInfo userInfo)
        {

            if (GetCount<UserInfo>(string.Format(" MgrUserId='{0}'", userInfo.UserID)) > 0)//有管理渠道
            {
                return true;
            }
            return false;

        }

        #region 返佣金相关（锁定）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disLevel"></param>
        /// <param name="orderUserInfo"></param>
        /// <param name="projectId"></param>
        /// <param name="amount"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="scoreEvent"></param>
        /// <param name="sbSql"></param>
        /// <param name="upUserLevel1"></param>
        /// <param name="upUserLevel2"></param>
        /// <param name="upUserLevel3"></param>
        /// <param name="levelConfig1"></param>
        /// <param name="levelConfig2"></param>
        /// <param name="levelConfig3"></param>
        /// <param name="modelLevel1"></param>
        /// <param name="scoreLockLevel1Info"></param>
        /// <param name="modelLevel1ex1"></param>
        /// <param name="scoreLockLevel1ex1Info"></param>
        /// <param name="modelLevel2"></param>
        /// <param name="scoreLockLevel2Info"></param>
        /// <param name="modelLevel3"></param>
        /// <param name="scoreLockLevel3Info"></param>
        /// <param name="toLevelString"></param>
        public void ComputeTransfers(int disLevel, UserInfo orderUserInfo, string projectId, decimal amount, string websiteOwner, string scoreEvent,
            ref StringBuilder sbSql, ref UserInfo upUserLevel1, ref UserInfo upUserLevel2, ref UserInfo upUserLevel3, ref UserLevelConfig levelConfig1,
            ref UserLevelConfig levelConfig2, ref UserLevelConfig levelConfig3, ref ProjectCommission modelLevel1, ref ScoreLockInfo scoreLockLevel1Info,
            ref ProjectCommission modelLevel1ex1, ref ScoreLockInfo scoreLockLevel1ex1Info, ref ProjectCommission modelLevel2,
            ref ScoreLockInfo scoreLockLevel2Info, ref ProjectCommission modelLevel3, ref ScoreLockInfo scoreLockLevel3Info, string toLevelString)
        {

            #region 计算分销上一级
            if (orderUserInfo.MemberApplyStatus == 9)
            {
                upUserLevel1 = bllUser.GetUserInfo(orderUserInfo.DistributionOwner, websiteOwner);
                if (upUserLevel1 != null && upUserLevel1.IsDisable != 1 && upUserLevel1.MemberLevel > 0) levelConfig1 = GetUserLevel(upUserLevel1);
            }
            if (levelConfig1 != null)//一级分销打款
            {
                if (string.IsNullOrEmpty(levelConfig1.RebateMemberRate))
                {
                    levelConfig1.RebateMemberRate = "0";
                }
                UserLevelConfig currUserConfig = GetUserLevel(orderUserInfo);

                //当配置奖励金额返利时 只按奖励金额返利 无会费比例无购房补助和公积金 ， 奖励金额读取的是 目标升级用户等级配置的奖励金额
                if (currUserConfig.AwardAmount > 0)
                {
                    levelConfig1.RebateMemberRate = "0";//会费奖励比例
                    levelConfig1.DistributionRateLevel1Ex1 = "0";//购房补助
                    levelConfig1.AccumulationFundRateLevel1 = "0";//公基金比例
                }
                double rateLevel1 = Convert.ToDouble(levelConfig1.RebateMemberRate);
                double rateLevel1ex1 = Convert.ToDouble(levelConfig1.DistributionRateLevel1Ex1);
                double accumulationFundRate = Convert.ToDouble(levelConfig1.AccumulationFundRateLevel1);

                double trueRate = (100 - accumulationFundRate) / 100 * rateLevel1;
                double trueRateex1 = (100 - accumulationFundRate) / 100 * rateLevel1ex1;
                double deductRate = accumulationFundRate / 100 * rateLevel1;
                double deductRateex1 = accumulationFundRate / 100 * rateLevel1ex1;

                decimal amountLevel1 = Math.Round(((decimal)rateLevel1 / 100) * amount, 2);
                decimal amountFundLevel1 = Math.Round(((decimal)deductRate / 100) * amount, 2);
                decimal amountFundLevel1True = Math.Round(((decimal)trueRate / 100) * amount, 2);


                decimal amountLevel1ex1 = Math.Round(((decimal)rateLevel1ex1 / 100) * amount, 2);
                decimal amountFundLevel1ex1 = Math.Round(((decimal)deductRateex1 / 100) * amount, 2);
                decimal amountFundLevel1ex1True = Math.Round(((decimal)trueRateex1 / 100) * amount, 2);

                if (currUserConfig.AwardAmount > 0)
                {
                    amountFundLevel1True += currUserConfig.AwardAmount;
                }

                if (amountFundLevel1True > 0)
                {
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0},AccumulationFund=ISNULL(AccumulationFund,0)+{3} Where UserId='{1}' And WebsiteOwner='{2}';",
                        amountFundLevel1True, upUserLevel1.UserID, websiteOwner, amountFundLevel1);

                    modelLevel1.UserId = upUserLevel1.UserID;
                    modelLevel1.Amount = amountFundLevel1True;
                    modelLevel1.CommissionUserId = orderUserInfo.UserID;
                    modelLevel1.InsertDate = DateTime.Now;
                    modelLevel1.ProjectAmount = amount;
                    modelLevel1.ProjectId = int.Parse(projectId);
                    modelLevel1.ProjectName = string.Format("{0}{1}", scoreEvent, "返利");
                    modelLevel1.Rate = trueRate;
                    modelLevel1.WebsiteOwner = websiteOwner;
                    modelLevel1.ProjectType = "MemberDistribution";
                    if (scoreEvent.Contains("注册"))
                    {
                        modelLevel1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "注册", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("升级"))
                    {
                        modelLevel1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "升级", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("空单填满"))
                    {
                        modelLevel1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "空单填满", toLevelString, orderUserInfo.Phone);
                    }
                    else
                    {
                        modelLevel1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, scoreEvent, amount, orderUserInfo.Phone);
                    }

                    modelLevel1.CommissionLevel = "1";
                    modelLevel1.SourceRate = rateLevel1;
                    modelLevel1.SourceAmount = amountLevel1;
                    modelLevel1.DeductRate = deductRate;
                    modelLevel1.DeductAmount = amountFundLevel1;

                    scoreLockLevel1Info = new ScoreLockInfo()
                    {
                        ForeignkeyId = projectId,
                        LockStatus = 0,
                        LockTime = DateTime.Now,
                        LockType = 2,
                        Score = amountFundLevel1True,
                        UserId = upUserLevel1.UserID,
                        FromUserId = orderUserInfo.UserID,
                        WebsiteOwner = websiteOwner,
                        Memo = modelLevel1.Remark + "，获返利"
                    };
                }

                if (amountFundLevel1ex1True > 0)
                {
                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0},AccumulationFund=ISNULL(AccumulationFund,0)+{3} Where UserId='{1}' And WebsiteOwner='{2}';",
                        amountFundLevel1ex1True, upUserLevel1.UserID, websiteOwner, amountFundLevel1ex1);

                    modelLevel1ex1.UserId = upUserLevel1.UserID;
                    modelLevel1ex1.Amount = amountFundLevel1ex1True;
                    modelLevel1ex1.CommissionUserId = orderUserInfo.UserID;
                    modelLevel1ex1.InsertDate = DateTime.Now;
                    modelLevel1ex1.ProjectAmount = amount;
                    modelLevel1ex1.ProjectId = int.Parse(projectId);
                    modelLevel1ex1.ProjectName = string.Format("{0}{1}", scoreEvent, "返购房补助");
                    modelLevel1ex1.Rate = trueRateex1;
                    modelLevel1ex1.WebsiteOwner = websiteOwner;
                    modelLevel1ex1.ProjectType = "MemberAccumulationFund";
                    if (scoreEvent.Contains("注册"))
                    {
                        modelLevel1ex1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "注册", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("升级"))
                    {
                        modelLevel1ex1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "升级", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("空单填满"))
                    {
                        modelLevel1ex1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "空单填满", toLevelString, orderUserInfo.Phone);
                    }
                    else
                    {
                        modelLevel1ex1.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, scoreEvent, amount, orderUserInfo.Phone);
                    }
                    modelLevel1ex1.CommissionLevel = "1";
                    modelLevel1ex1.SourceRate = rateLevel1ex1;
                    modelLevel1ex1.SourceAmount = amountLevel1ex1;
                    modelLevel1ex1.DeductRate = deductRateex1;
                    modelLevel1ex1.DeductAmount = amountFundLevel1ex1;

                    scoreLockLevel1ex1Info = new ScoreLockInfo()
                    {
                        ForeignkeyId = projectId,
                        LockStatus = 0,
                        LockTime = DateTime.Now,
                        LockType = 2,
                        Score = amountFundLevel1ex1True,
                        UserId = upUserLevel1.UserID,
                        FromUserId = orderUserInfo.UserID,
                        WebsiteOwner = websiteOwner,
                        Memo = modelLevel1ex1.Remark + "，获购房补助"
                    };
                }
            }
            #endregion

            #region 计算分销上二级
            if (disLevel >= 2 && upUserLevel1 != null && upUserLevel1.IsDisable != 1 && upUserLevel1.MemberApplyStatus == 9)
            {
                upUserLevel2 = bllUser.GetUserInfo(upUserLevel1.DistributionOwner, websiteOwner);
                if (upUserLevel2 != null && upUserLevel2.IsDisable != 1 && upUserLevel2.MemberLevel > 0) levelConfig2 = GetUserLevel(upUserLevel2);
            }
            if (levelConfig2 != null)//二级分销打款
            {
                double rateLevel2 = Convert.ToDouble(levelConfig2.DistributionRateLevel2);
                if (rateLevel2 > 0)
                {
                    double accumulationFundRate = Convert.ToDouble(levelConfig2.AccumulationFundRateLevel1);
                    double trueRate = (100 - accumulationFundRate) / 100 * rateLevel2;
                    double deductRate = accumulationFundRate / 100 * rateLevel2;

                    decimal amountLevel2 = Math.Round(((decimal)rateLevel2 / 100) * amount, 2);
                    decimal amountFundLevel2 = Math.Round(((decimal)deductRate / 100) * amount, 2);
                    decimal amountFundLevel2True = Math.Round(((decimal)trueRate / 100) * amount, 2);

                    sbSql.AppendFormat(" Update ZCJ_UserInfo Set AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0},AccumulationFund=ISNULL(AccumulationFund,0)+{3} Where UserId='{1}' And WebsiteOwner='{2}';",
                        amountFundLevel2True, upUserLevel2.UserID, websiteOwner, amountFundLevel2);


                    modelLevel2.UserId = upUserLevel2.UserID;
                    modelLevel2.Amount = amountFundLevel2True;
                    modelLevel2.CommissionUserId = orderUserInfo.UserID;
                    modelLevel2.InsertDate = DateTime.Now;
                    modelLevel2.ProjectAmount = amount;
                    modelLevel2.ProjectId = int.Parse(projectId);
                    modelLevel2.ProjectName = string.Format("{0}{1}", scoreEvent, "返利");
                    modelLevel2.Rate = trueRate;
                    modelLevel2.WebsiteOwner = websiteOwner;
                    modelLevel2.ProjectType = "MemberDistribution";
                    if (scoreEvent.Contains("注册"))
                    {
                        modelLevel2.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "注册", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("升级"))
                    {
                        modelLevel2.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "升级", toLevelString, orderUserInfo.Phone);
                    }
                    else if (scoreEvent.Contains("空单填满"))
                    {
                        modelLevel2.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "空单填满", toLevelString, orderUserInfo.Phone);
                    }
                    else
                    {
                        modelLevel2.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, scoreEvent, amount, orderUserInfo.Phone);
                    }
                    modelLevel2.CommissionLevel = "2";
                    modelLevel2.SourceRate = rateLevel2;
                    modelLevel2.SourceAmount = amountLevel2;
                    modelLevel2.DeductRate = deductRate;
                    modelLevel2.DeductAmount = amountFundLevel2;

                    scoreLockLevel2Info = new ScoreLockInfo()
                    {
                        ForeignkeyId = projectId,
                        LockStatus = 0,
                        LockTime = DateTime.Now,
                        LockType = 2,
                        Score = amountFundLevel2True,
                        UserId = upUserLevel2.UserID,
                        FromUserId = orderUserInfo.UserID,
                        WebsiteOwner = websiteOwner,
                        Memo = modelLevel2.Remark + "，获返利"
                    };
                }
            }
            #endregion

            #region 计算分销上三级
            if (disLevel >= 3 && upUserLevel2 != null && upUserLevel2.IsDisable != 1 && upUserLevel2.MemberApplyStatus == 9)
            {
                upUserLevel3 = bllUser.GetUserInfo(upUserLevel2.DistributionOwner, websiteOwner);
                if (upUserLevel3 != null && upUserLevel3.IsDisable != 1 && upUserLevel3.MemberLevel > 0) levelConfig3 = GetUserLevel(upUserLevel3);
            }
            if (levelConfig3 != null)//二级分销打款
            {
                double rateLevel3 = Convert.ToDouble(levelConfig3.DistributionRateLevel3);
                if (rateLevel3 > 0)
                {
                    double accumulationFundRate = Convert.ToDouble(levelConfig3.AccumulationFundRateLevel1);
                    double trueRate = (100 - accumulationFundRate) / 100 * rateLevel3;
                    double deductRate = accumulationFundRate / 100 * rateLevel3;

                    decimal amountLevel3 = Math.Round(((decimal)rateLevel3 / 100) * amount, 2);
                    decimal amountFundLevel3 = Math.Round(((decimal)deductRate / 100) * amount, 2);
                    decimal amountFundLevel3True = Math.Round(((decimal)trueRate / 100) * amount, 2);

                    if (amountFundLevel3True > 0)
                    {
                        sbSql.AppendFormat(" Update ZCJ_UserInfo Set AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0},AccumulationFund=ISNULL(AccumulationFund,0)+{3} Where UserId='{1}' And WebsiteOwner='{2}';",
                        amountFundLevel3True, upUserLevel3.UserID, websiteOwner, amountFundLevel3);


                        modelLevel3.UserId = upUserLevel3.UserID;
                        modelLevel3.Amount = amountFundLevel3True;
                        modelLevel3.CommissionUserId = orderUserInfo.UserID;
                        modelLevel3.InsertDate = DateTime.Now;
                        modelLevel3.ProjectAmount = amount;
                        modelLevel3.ProjectId = int.Parse(projectId);
                        modelLevel3.ProjectName = string.Format("{0}{1}", scoreEvent, "返利");
                        modelLevel3.Rate = trueRate;
                        modelLevel3.WebsiteOwner = websiteOwner;
                        modelLevel3.ProjectType = "MemberDistribution";
                        if (scoreEvent.Contains("注册"))
                        {
                            modelLevel3.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "注册", toLevelString, orderUserInfo.Phone);
                        }
                        else if (scoreEvent.Contains("升级"))
                        {
                            modelLevel3.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "升级", toLevelString, orderUserInfo.Phone);
                        }
                        else if (scoreEvent.Contains("空单填满"))
                        {
                            modelLevel3.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, "空单填满", toLevelString, orderUserInfo.Phone);
                        }
                        else
                        {
                            modelLevel3.Remark = string.Format("{0}[{3}]{1}{2}", orderUserInfo.TrueName, scoreEvent, amount, orderUserInfo.Phone);
                        }
                        modelLevel3.CommissionLevel = "3";
                        modelLevel3.SourceRate = rateLevel3;
                        modelLevel3.SourceAmount = amountLevel3;
                        modelLevel3.DeductRate = deductRate;
                        modelLevel3.DeductAmount = amountFundLevel3;

                        scoreLockLevel3Info = new ScoreLockInfo()
                        {
                            ForeignkeyId = projectId,
                            LockStatus = 0,
                            LockTime = DateTime.Now,
                            LockType = 2,
                            Score = amountFundLevel3True,
                            UserId = upUserLevel3.UserID,
                            FromUserId = orderUserInfo.UserID,
                            WebsiteOwner = websiteOwner,
                            Memo = modelLevel3.Remark + "，获返利"
                        };
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 线上支付注册分佣(冻结)
        /// </summary>
        /// <param name="regUser"></param>
        /// <param name="orderPay"></param>
        /// <param name="parametersAll"></param>
        /// <param name="levelConfig"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PayRegisterTransfers(UserInfo regUser, OrderPay orderPay, string openid, string trade_No, UserLevelConfig levelConfig, out string msg)
        {
            string scoreEvent = "线上注册";
            BLLUser bllUser = new BLLUser();
            WebsiteInfo website = GetColByKey<WebsiteInfo>("WebsiteOwner", orderPay.WebsiteOwner, "WebsiteOwner,TotalAmountShowName,DistributionLimitLevel");
            int disLevel = 1;
            if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;
            msg = "";
            if (orderPay.Total_Fee <= 0)
            {
                msg = "订单金额必须大于0";
                return false;
            }
            if (GetCount<ProjectCommission>(string.Format("ProjectId={0}", orderPay.OrderId)) > 0)
            {
                msg = "已经分过佣金了";
                return false;
            }
            StringBuilder sbSql = new StringBuilder();

            bool hasProjectCommission = false;//分佣是否存在
            UserInfo upUserLevel1 = null;//分销上一级
            UserInfo upUserLevel2 = null;//分销上二级
            UserInfo upUserLevel3 = null;//分销上三级

            UserLevelConfig levelConfig1 = null;//分销上一级规则
            UserLevelConfig levelConfig2 = null;//分销上二级规则
            UserLevelConfig levelConfig3 = null;//分销上三级规则

            ProjectCommission modelLevel1 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
            ProjectCommission modelLevel1ex1 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
            ProjectCommission modelLevel2 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
            ProjectCommission modelLevel3 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

            //计算分佣
            ComputeTransfers(disLevel, regUser, orderPay.OrderId, orderPay.Total_Fee, orderPay.WebsiteOwner, scoreEvent, ref sbSql, ref upUserLevel1,
                ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                levelConfig.LevelString);


            BLLTransaction tran = new BLLTransaction();

            try
            {

                #region 注册会员
                if (regUser.AutoID == 0)
                {
                    if (!bllUser.Add(regUser, tran))
                    {
                        msg = "添加账号失败";
                        tran.Rollback();
                        return false;
                    }
                }
                else
                {
                    if (!bllUser.Update(regUser, tran))
                    {
                        msg = "更新会员级别失败";
                        tran.Rollback();
                        return false;
                    }
                }

                new BLLUserDistributionMember().SetUserDistributionOwnerInMember(new List<string>() { regUser.UserID }, regUser.DistributionOwner, regUser.WebsiteOwner);

                #endregion
                #region 记录余额明细
                //自己的消费记录
                if (bllUser.AddScoreDetail(regUser.UserID, orderPay.WebsiteOwner, (double)(orderPay.Total_Fee),
                    string.Format("注册充值{0}元", orderPay.Total_Fee), "TotalAmount", (double)(regUser.TotalAmount + orderPay.Total_Fee),
                    orderPay.OrderId.ToString(), "线上注册充值", openid, trade_No, (double)orderPay.Total_Fee, 0, "",
                    tran, ex1: orderPay.PayTypeCnName, ex5: orderPay.PayTypeEnName, isPrint: 1) <= 0)
                {
                    msg = "充值明细失败";
                    tran.Rollback();
                    return false;
                }
                int mainDetailId = bllUser.AddScoreDetail(regUser.UserID, orderPay.WebsiteOwner, (double)(0 - orderPay.Total_Fee),
                    string.Format("{1}{0}", levelConfig.LevelString, scoreEvent), "TotalAmount", (double)regUser.TotalAmount,
                    "", "注册会员", "", "", (double)orderPay.Total_Fee, 0, "",
                    tran);
                if (mainDetailId <= 0)
                {
                    msg = "注册明细失败";
                    tran.Rollback();
                    return false;
                }
                #endregion

                #region 记录分佣信息
                if (modelLevel1.Amount > 0)
                {
                    hasProjectCommission = true;
                    int modelLevel1Id = Convert.ToInt32(AddReturnID(modelLevel1, tran));
                    if (modelLevel1Id <= 0)
                    {
                        msg = "一级返利失败";
                        tran.Rollback();
                        return false;
                    }
                    scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                    scoreLockLevel1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1Info, tran));
                    if (scoreLockLevel1Info.AutoId <= 0)
                    {
                        msg = "一级返利冻结失败";
                        tran.Rollback();
                        return false;
                    }
                    string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                    if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel1Info.Score,
                        scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                        scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                        modelLevel1.CommissionUserId, tran,
                        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                        ex5: modelLevel1.CommissionLevel) <= 0)
                    {
                        msg = "一级返返利明细记录失败";
                        tran.Rollback();
                        return false;
                    }
                }
                if (modelLevel1ex1.Amount > 0)
                {
                    hasProjectCommission = true;
                    int modelLevel1ex1Id = Convert.ToInt32(AddReturnID(modelLevel1ex1, tran));
                    if (modelLevel1ex1Id <= 0)
                    {
                        msg = "一级返购房补助失败";
                        tran.Rollback();
                        return false;
                    }
                    scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                    scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1ex1Info, tran));
                    if (scoreLockLevel1ex1Info.AutoId <= 0)
                    {
                        msg = "一级返购房补助冻结失败";
                        tran.Rollback();
                        return false;
                    }
                    string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                    if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel1ex1Info.Score,
                        scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                        scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                        modelLevel1ex1.CommissionUserId, tran,
                        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                        ex5: modelLevel1ex1.CommissionLevel) <= 0)
                    {
                        msg = "一级返购房补助明细记录失败";
                        tran.Rollback();
                        return false;
                    }
                }
                if (modelLevel2.Amount > 0)
                {
                    hasProjectCommission = true;
                    int modelLevel2Id = Convert.ToInt32(AddReturnID(modelLevel2, tran));
                    if (modelLevel2Id <= 0)
                    {
                        msg = "二级返利失败";
                        tran.Rollback();
                        return false;
                    }
                    scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                    scoreLockLevel2Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel2Info, tran));
                    if (scoreLockLevel2Info.AutoId <= 0)
                    {
                        msg = "二级返利冻结失败";
                        tran.Rollback();
                        return false;
                    }
                    string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                    if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel2Info.Score,
                        scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                        scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                        modelLevel2.CommissionUserId, tran,
                        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                        ex5: modelLevel2.CommissionLevel) <= 0)
                    {
                        msg = "二级返利明细记录失败";
                        tran.Rollback();
                        return false;
                    }
                }
                if (modelLevel3.Amount > 0)
                {
                    hasProjectCommission = true;
                    int modelLevel3Id = Convert.ToInt32(AddReturnID(modelLevel3, tran));
                    if (!Add(modelLevel3, tran))
                    {
                        msg = "三级返利失败";
                        tran.Rollback();
                        return false;
                    }
                    scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                    scoreLockLevel3Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel3Info, tran));
                    if (scoreLockLevel3Info.AutoId <= 0)
                    {
                        msg = "三级返利冻结失败";
                        tran.Rollback();
                        return false;
                    }
                    string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                    if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel3Info.Score,
                        scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                        scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                        modelLevel3.CommissionUserId, tran,
                        ex3: levelConfig.LevelNumber.ToString(), ex4: levelConfig.LevelString,
                        ex5: modelLevel3.CommissionLevel) <= 0)
                    {
                        msg = "三级返利明细记录失败";
                        tran.Rollback();
                        return false;
                    }
                }
                #endregion

                #region 记录业绩明细
                TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
                perDetail.AddType = "注册";
                perDetail.AddNote = "注册" + levelConfig.LevelString;
                perDetail.AddTime = DateTime.Now;
                perDetail.DistributionOwner = regUser.DistributionOwner;
                perDetail.UserId = regUser.UserID;
                perDetail.UserName = regUser.TrueName;
                perDetail.UserPhone = regUser.Phone;
                perDetail.Performance = orderPay.Total_Fee;
                string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
                int yearMonth = Convert.ToInt32(yearMonthString);
                perDetail.YearMonth = yearMonth;
                perDetail.WebsiteOwner = orderPay.WebsiteOwner;

                if (!Add(perDetail, tran))
                {
                    msg = "记录业绩明细失败";
                    tran.Rollback();
                    return false;
                }
                #endregion

                #region 更新订单状态

                int resultPay = BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_OrderPay SET Status={0},Trade_No='{4}' WHERE WebsiteOwner='{1}' And AutoID={2} And Status={3} ",
                    "1", orderPay.WebsiteOwner, orderPay.AutoID, "0", trade_No), tran);
                if (resultPay <= 0)
                {
                    msg = "更新支付状态失败";
                    tran.Rollback();
                    return false;
                }
                #endregion
                BLLSMS bllSms = new BLLSMS("");
                bool smsBool = false;
                if (hasProjectCommission)
                {
                    int result = BLLBase.ExecuteSql(sbSql.ToString(), tran);
                    if (result > 0)
                    {
                        tran.Commit();

                        //计算相关业绩
                        BuildCurMonthPerformanceByUserID(website.WebsiteOwner, regUser.UserID);

                        #region 短信发送密码
                        string smsString = string.Format("恭喜您成功注册为天下华商月供宝：{1}，您的初始密码为：{0}。您可关注公众号：songhetz，登录账户修改密码，并设置支付密码。", regUser.Password, levelConfig.LevelString);
                        bllSms.SendSmsMisson(regUser.Phone, smsString, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), website.SmsSignature, out smsBool, out msg);
                        #endregion
                        #region 微信通知
                        //try
                        //{

                        //    BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
                        //    string url = string.Format("http://{0}/App/Wap/MemberCenter.aspx", bllWebsiteDomain.GetWebsiteDoMain(orderPay.WebsiteOwner));

                        //    if (modelLevel1.Amount > 0)//一级佣金
                        //    {
                        //        bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", regUser.TrueName, levelConfig.LevelString, modelLevel1.Amount, modelLevel1.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                        //    }
                        //    if (modelLevel1ex1.Amount > 0)//一级住房补助
                        //    {
                        //        bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利购房补助", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", regUser.TrueName, levelConfig.LevelString, modelLevel1ex1.Amount, modelLevel1ex1.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                        //    }
                        //    if (modelLevel2.Amount > 0)//二级佣金
                        //    {
                        //        bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel2.WXOpenId, "二级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", regUser.TrueName, levelConfig.LevelString, modelLevel2.Amount, modelLevel2.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                        //    }
                        //    if (modelLevel3.Amount > 0)//三级佣金
                        //    {
                        //        bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel3.WXOpenId, "三级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", regUser.TrueName, levelConfig.LevelString, modelLevel3.Amount, modelLevel3.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                        //    }
                        //}
                        //catch (Exception)
                        //{


                        //}
                        #endregion
                        return true;
                    }
                    else
                    {
                        msg = "更新分佣账面金额";
                        tran.Rollback();
                        return false;
                    }
                }
                else
                {
                    tran.Commit();

                    string smsString = string.Format("恭喜您成功注册为天下华商月供宝：{1}，您的初始密码为：{0}。您可关注公众号：songhetz，登录账户修改密码，并设置支付密码。", regUser.Password, levelConfig.LevelString);
                    bllSms.SendSmsMisson(regUser.Phone, smsString, "", website.SmsSignature, out smsBool, out msg);
                    return true;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();

                throw ex;
            }

        }

        /// <summary>
        /// 充值余额
        /// </summary>
        /// <param name="orderPay"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PayRechargeTransfers(OrderPay orderPay, string openid, string trade_No, out string msg)
        {
            msg = "";
            string scoreEvent = "线上充值";
            BLLUser bllUser = new BLLUser();
            WebsiteInfo website = GetColByKey<WebsiteInfo>("WebsiteOwner", orderPay.WebsiteOwner, "WebsiteOwner,TotalAmountShowName,DistributionLimitLevel");
            int disLevel = 1;
            if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;


            UserInfo payUser = bllUser.GetUserInfo(orderPay.UserId, orderPay.WebsiteOwner);
            if (payUser == null)
            {
                msg = "用户未找到";
                return false;
            }
            UserLevelConfig levelConfig = QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", payUser.MemberLevel.ToString());
            string levelString = levelConfig == null ? "" : levelConfig.LevelString;

            BLLTransaction tran = new BLLTransaction();
            //修改金额 状态
            StringBuilder sbPaySql = new StringBuilder();
            sbPaySql.AppendFormat("UPDATE ZCJ_UserInfo SET TotalAmount=ISNULL(TotalAmount,0)+{1},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{1} FROM ZCJ_UserInfo WHERE UserID='{2}' And WebsiteOwner='{3}' And EXISTS( SELECT 1 FROM ZCJ_OrderPay WHERE AutoID={0} And Status=0);",
                orderPay.AutoID, orderPay.Total_Fee, orderPay.UserId, orderPay.WebsiteOwner);
            sbPaySql.AppendFormat("UPDATE ZCJ_OrderPay SET Status={0},Trade_No='{4}' WHERE WebsiteOwner='{1}' And AutoID={2} And Status={3} ",
                "1", orderPay.WebsiteOwner, orderPay.AutoID, "0", trade_No);

            #region 记录余额明细
            //自己的消费记录
            if (bllUser.AddScoreDetail(payUser.UserID, orderPay.WebsiteOwner, (double)(orderPay.Total_Fee),
                string.Format("线上充值{0}元", (double)orderPay.Total_Fee), "TotalAmount", (double)(payUser.TotalAmount + orderPay.Total_Fee),
                orderPay.OrderId.ToString(), "线上充值", openid, trade_No, (double)orderPay.Total_Fee, 0, "",
                tran, ex1: orderPay.PayTypeCnName, ex5: orderPay.PayTypeEnName, isPrint: 1) <= 0)
            {
                msg = "充值明细失败";
                tran.Rollback();
                return false;
            }
            #endregion
            #region 修改金额 更新支付状态
            int resultPay = BLLBase.ExecuteSql(sbPaySql.ToString(), tran);
            if (resultPay <= 0)
            {
                msg = "更新支付状态失败";
                tran.Rollback();
                return false;
            }
            #endregion

            tran.Commit();

            //异步计算金额
            Thread th1 = new Thread(delegate()
            {
                CheckTotalAmount(payUser.AutoID, orderPay.WebsiteOwner, 7);
            });
            th1.Start();
            #region 微信通知
            try
            {

                BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
                string url = string.Format("http://{0}/App/Wap/MemberCenter.aspx", bllWebsiteDomain.GetWebsiteDoMain(orderPay.WebsiteOwner));

                if (orderPay.Total_Fee > 0)//充值提示
                {
                    bllWeixin.SendTemplateMessageNotifyCommTask(payUser.WXOpenId, string.Format("充值{0}", website.TotalAmountShowName), string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{3}\\n{4}:{2}\\n点击查看详情", payUser.TrueName, levelString, orderPay.Total_Fee, scoreEvent, website.TotalAmountShowName), url, "", "", "", orderPay.WebsiteOwner);
                }
                //if (modelLevel1.Amount > 0)//一级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", payUser.TrueName, levelString, modelLevel1.Amount, modelLevel1.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel1ex1.Amount > 0)//一级住房补助
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利购房补助", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", payUser.TrueName, levelString, modelLevel1ex1.Amount, modelLevel1ex1.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel2.Amount > 0)//二级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel2.WXOpenId, "二级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", payUser.TrueName, levelString, modelLevel2.Amount, modelLevel2.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel3.Amount > 0)//三级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel3.WXOpenId, "三级返利佣金", string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", payUser.TrueName, levelString, modelLevel3.Amount, modelLevel3.DeductAmount, scoreEvent), url, "", "", "", orderPay.WebsiteOwner);
                //}
            }
            catch (Exception)
            {


            }
            #endregion

            return true;
        }
        /// <summary>
        /// 支付升级
        /// </summary>
        /// <param name="orderPay"></param>
        /// <param name="parametersAll"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PayUpgradeTransfers(OrderPay orderPay, Model.API.User.PayUpgrade payUpgrade, string openid, string trade_No, out string msg)
        {
            msg = "";
            string scoreEvent = "支付升级";
            msg = "";
            BLLUser bllUser = new BLLUser();
            WebsiteInfo website = GetColByKey<WebsiteInfo>("WebsiteOwner", orderPay.WebsiteOwner, "WebsiteOwner,TotalAmountShowName,DistributionLimitLevel");
            int disLevel = 1;
            if (website.DistributionLimitLevel > 1) disLevel = website.DistributionLimitLevel;
            UserInfo payUser = bllUser.GetUserInfo(orderPay.UserId, orderPay.WebsiteOwner);
            if (payUser == null)
            {
                msg = "用户未找到";
                return false;
            }
            if (payUpgrade.level != payUser.MemberLevel)
            {
                msg = "用户原级别有误";
                return false;
            }
            payUser.MemberLevel = payUpgrade.toLevel;
            UserLevelConfig levelConfig = QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", payUser.MemberLevel.ToString());
            UserLevelConfig toLevelConfig = QueryUserLevel(orderPay.WebsiteOwner, "DistributionOnLine", payUpgrade.toLevel.ToString());
            if (toLevelConfig == null)
            {
                msg = "会员等级未找到";
                return false;
            }
            string levelString = levelConfig == null ? "" : levelConfig.LevelString;
            string toLevelString = toLevelConfig == null ? "" : toLevelConfig.LevelString;

            BLLTransaction tran = new BLLTransaction();
            BLLDistribution bllDistribution = new BLLDistribution();

            StringBuilder sbSql = new StringBuilder();
            UserInfo upUserLevel1 = null;//分销上一级
            UserInfo upUserLevel2 = null;//分销上二级
            UserInfo upUserLevel3 = null;//分销上三级
            UserLevelConfig levelConfig1 = null;//分销上一级规则
            UserLevelConfig levelConfig2 = null;//分销上二级规则
            UserLevelConfig levelConfig3 = null;//分销上三级规则
            ProjectCommission modelLevel1 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel1Info = new ScoreLockInfo();
            ProjectCommission modelLevel1ex1 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel1ex1Info = new ScoreLockInfo();
            ProjectCommission modelLevel2 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel2Info = new ScoreLockInfo();
            ProjectCommission modelLevel3 = new ProjectCommission();
            ScoreLockInfo scoreLockLevel3Info = new ScoreLockInfo();

            //计算分佣
            ComputeTransfers(disLevel, payUser, orderPay.OrderId, orderPay.Total_Fee, orderPay.WebsiteOwner, scoreEvent, ref sbSql, ref upUserLevel1,
                ref upUserLevel2, ref upUserLevel3, ref levelConfig1, ref levelConfig2, ref levelConfig3, ref modelLevel1, ref scoreLockLevel1Info,
                ref modelLevel1ex1, ref scoreLockLevel1ex1Info, ref modelLevel2, ref scoreLockLevel2Info, ref modelLevel3, ref scoreLockLevel3Info,
                toLevelConfig.LevelString);

            #region 记录余额明细
            //自己的消费记录
            if (bllUser.AddScoreDetail(payUser.UserID, orderPay.WebsiteOwner, (double)(orderPay.Total_Fee),
                string.Format("升级充值{0}元", (double)orderPay.Total_Fee), "TotalAmount", (double)(payUser.TotalAmount + orderPay.Total_Fee),
                orderPay.OrderId.ToString(), "升级充值", openid, trade_No, (double)orderPay.Total_Fee, 0, "",
                tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                ex5: orderPay.PayTypeEnName, isPrint: 1) <= 0)
            {
                msg = "充值明细失败";
                tran.Rollback();
                return false;
            }
            if (bllUser.AddScoreDetail(payUser.UserID, orderPay.WebsiteOwner, (double)(0 - orderPay.Total_Fee),
                string.Format("{1}为{0}", toLevelConfig.LevelString, scoreEvent), "TotalAmount", (double)(payUser.TotalAmount),
                "", "升级会员", openid, trade_No, (double)orderPay.Total_Fee, 0, "",
                tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString) <= 0)
            {
                msg = "升级会员失败";
                tran.Rollback();
                return false;
            }

            #endregion

            bool hasProjectCommission = false;

            #region 记录分佣信息
            if (modelLevel1.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel1Id = Convert.ToInt32(AddReturnID(modelLevel1, tran));
                if (modelLevel1Id <= 0)
                {
                    msg = "一级返利失败";
                    tran.Rollback();
                    return false;
                }
                scoreLockLevel1Info.ForeignkeyId2 = modelLevel1Id.ToString();
                scoreLockLevel1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1Info, tran));
                if (scoreLockLevel1Info.AutoId <= 0)
                {
                    msg = "一级返利冻结失败";
                    tran.Rollback();
                    return false;
                }
                string scoreDetailEvent = modelLevel1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel1Info.Score,
                    scoreLockLevel1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1Info.Score),
                    scoreLockLevel1Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel1.SourceAmount, (double)modelLevel1.DeductAmount,
                    modelLevel1.CommissionUserId,
                    tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel1.CommissionLevel) <= 0)
                {
                    msg = "一级返利明细记录失败";
                    tran.Rollback();
                    return false;
                }
            }
            if (modelLevel1ex1.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel1ex1Id = Convert.ToInt32(AddReturnID(modelLevel1ex1, tran));
                if (modelLevel1ex1Id <= 0)
                {
                    msg = "一级返购房补助失败";
                    tran.Rollback();
                    return false;
                }
                scoreLockLevel1ex1Info.ForeignkeyId2 = modelLevel1ex1Id.ToString();
                scoreLockLevel1ex1Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel1ex1Info, tran));
                if (scoreLockLevel1ex1Info.AutoId <= 0)
                {
                    msg = "一级返购房补助冻结失败";
                    tran.Rollback();
                    return false;
                }
                string scoreDetailEvent = modelLevel1ex1.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel1ex1Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel1ex1Info.Score,
                    scoreLockLevel1ex1Info.Memo, "TotalAmount", (double)(upUserLevel1.TotalAmount + scoreLockLevel1ex1Info.Score),
                    scoreLockLevel1ex1Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel1ex1.SourceAmount, (double)modelLevel1ex1.DeductAmount,
                    modelLevel1ex1.CommissionUserId,
                    tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel1ex1.CommissionLevel) <= 0)
                {
                    msg = "一级返购房补助明细记录失败";
                    tran.Rollback();
                    return false;
                }
            }
            if (modelLevel2.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel2Id = Convert.ToInt32(AddReturnID(modelLevel2, tran));
                if (modelLevel2Id <= 0)
                {
                    msg = "二级返利失败";
                    tran.Rollback();
                    return false;
                }
                scoreLockLevel2Info.ForeignkeyId2 = modelLevel2Id.ToString();
                scoreLockLevel2Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel2Info, tran));
                if (scoreLockLevel2Info.AutoId <= 0)
                {
                    msg = "二级返利冻结失败";
                    tran.Rollback();
                    return false;
                }
                string scoreDetailEvent = modelLevel2.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel2Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel2Info.Score,
                    scoreLockLevel2Info.Memo, "TotalAmount", (double)(upUserLevel2.TotalAmount + scoreLockLevel2Info.Score),
                    scoreLockLevel2Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel2.SourceAmount, (double)modelLevel2.DeductAmount,
                    modelLevel2.CommissionUserId,
                    tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel2.CommissionLevel) <= 0)
                {
                    msg = "二级返利明细记录失败";
                    tran.Rollback();
                    return false;
                }
            }
            if (modelLevel3.Amount > 0)
            {
                hasProjectCommission = true;
                int modelLevel3Id = Convert.ToInt32(AddReturnID(modelLevel3, tran));
                if (!Add(modelLevel3, tran))
                {
                    msg = "三级返利失败";
                    tran.Rollback();
                    return false;
                }
                scoreLockLevel3Info.ForeignkeyId2 = modelLevel3Id.ToString();
                scoreLockLevel3Info.AutoId = Convert.ToInt32(AddReturnID(scoreLockLevel3Info, tran));
                if (scoreLockLevel3Info.AutoId <= 0)
                {
                    msg = "三级返利冻结失败";
                    tran.Rollback();
                    return false;
                }
                string scoreDetailEvent = modelLevel3.ProjectName.Contains("购房补助") ? "返购房补助" : "返利";
                if (bllUser.AddScoreDetail(scoreLockLevel3Info.UserId, orderPay.WebsiteOwner, (double)scoreLockLevel3Info.Score,
                    scoreLockLevel3Info.Memo, "TotalAmount", (double)(upUserLevel3.TotalAmount + scoreLockLevel3Info.Score),
                    scoreLockLevel3Info.AutoId.ToString(), scoreDetailEvent, "", orderPay.OrderId, (double)modelLevel3.SourceAmount, (double)modelLevel3.DeductAmount,
                    modelLevel3.CommissionUserId,
                    tran, ex1: levelConfig.LevelNumber.ToString(), ex2: levelConfig.LevelString,
                    ex3: toLevelConfig.LevelNumber.ToString(), ex4: toLevelConfig.LevelString,
                    ex5: modelLevel3.CommissionLevel) <= 0)
                {
                    msg = "三级返利明细记录失败";
                    tran.Rollback();
                    return false;
                }
            }
            #endregion

            #region 修改金额 更新支付状态

            if (BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_UserInfo SET MemberLevel={0},IsDisable=0 FROM ZCJ_UserInfo WHERE UserID='{1}' And WebsiteOwner='{2}';",
                payUpgrade.toLevel, orderPay.UserId, orderPay.WebsiteOwner),
                tran) <= 0)
            {
                msg = "更新会员等级失败";
                tran.Rollback();
                return false;
            }
            if (BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_OrderPay SET Status={0},Trade_No='{4}' WHERE WebsiteOwner='{1}' And AutoID={2} And Status={3} ",
                "1", orderPay.WebsiteOwner, orderPay.AutoID, "0", trade_No),
                tran) <= 0)
            {
                msg = "更新支付状态失败";
                tran.Rollback();
                return false;
            }
            #endregion

            #region 更新分佣账面金额
            if (hasProjectCommission)
            {
                if (BLLBase.ExecuteSql(sbSql.ToString(), tran) <= 0)
                {
                    tran.Rollback();
                    msg = string.Format("更新分佣账面{0}出错", website.TotalAmountShowName);
                    tran.Rollback();
                    return false;
                }
            }
            #endregion

            #region 记录业绩明细
            TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
            perDetail.AddType = "升级";
            perDetail.AddNote = "由" + levelConfig.LevelString + "升级" + toLevelConfig.LevelString;
            perDetail.AddTime = DateTime.Now;
            perDetail.DistributionOwner = payUser.DistributionOwner;
            perDetail.UserId = payUser.UserID;
            perDetail.UserName = payUser.TrueName;
            perDetail.UserPhone = payUser.Phone;
            perDetail.Performance = orderPay.Total_Fee;
            string yearMonthString = perDetail.AddTime.ToString("yyyyMM");
            int yearMonth = Convert.ToInt32(yearMonthString);
            perDetail.YearMonth = yearMonth;
            perDetail.WebsiteOwner = orderPay.WebsiteOwner;

            if (!Add(perDetail, tran))
            {
                msg = "记录业绩明细失败";
                tran.Rollback();
                return false;
            }
            #endregion

            tran.Commit();
            if (hasProjectCommission)
            {
                //计算相关业绩
                BuildCurMonthPerformanceByUserID(website.WebsiteOwner, payUser.UserID);
            }

            #region 微信通知
            try
            {

                BLLWebsiteDomainInfo bllWebsiteDomain = new BLLWebsiteDomainInfo();
                string url = string.Format("http://{0}/App/Wap/MemberCenter.aspx", bllWebsiteDomain.GetWebsiteDoMain(orderPay.WebsiteOwner));

                if (orderPay.Total_Fee > 0)//充值提示
                {
                    bllWeixin.SendTemplateMessageNotifyCommTask(payUser.WXOpenId, "升级会员",
                        string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{3}\\n原会员类型:{4}\\消耗{2}{5}\\n点击查看详情",
                        payUser.TrueName, toLevelString, orderPay.Total_Fee, scoreEvent, levelString, website.TotalAmountShowName),
                        url, "", "", "", orderPay.WebsiteOwner);
                }
                //if (modelLevel1.Amount > 0)//一级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利佣金", 
                //        string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n原会员类型:{5}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", 
                //        payUser.TrueName, toLevelString, modelLevel1.Amount, modelLevel1.DeductAmount, scoreEvent, levelString), 
                //        url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel1ex1.Amount > 0)//一级住房补助
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel1.WXOpenId, "一级返利购房补助", 
                //        string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n原会员类型:{5}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", 
                //        payUser.TrueName, toLevelString, modelLevel1ex1.Amount, modelLevel1ex1.DeductAmount, scoreEvent, levelString), 
                //        url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel2.Amount > 0)//二级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel2.WXOpenId, "二级返利佣金", 
                //        string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n原会员类型:{5}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", 
                //        payUser.TrueName, toLevelString, modelLevel2.Amount, modelLevel2.DeductAmount, scoreEvent, levelString), 
                //        url, "", "", "", orderPay.WebsiteOwner);
                //}
                //if (modelLevel3.Amount > 0)//三级佣金
                //{
                //    bllWeixin.SendTemplateMessageNotifyCommTask(upUserLevel3.WXOpenId, "三级返利佣金", 
                //        string.Format("姓名:{0}\\n会员类型:{1}\\n操作:{4}\\n原会员类型:{5}\\n返利到金额:{2}\\n返利到公积金:{3}\\n点击查看详情", 
                //        payUser.TrueName, toLevelString, modelLevel3.Amount, modelLevel3.DeductAmount, scoreEvent, levelString), 
                //        url, "", "", "", orderPay.WebsiteOwner);
                //}
            }
            catch (Exception)
            {


            }
            #endregion

            return true;
        }

        /// <summary>
        /// 100行一次 解锁余额
        /// </summary>
        public void UnLockTotalAmount()
        {
            List<ScoreLockInfo> lockList = GetList<ScoreLockInfo>(20,
                string.Format(" LockType=2 And LockStatus=0 And DATEADD(d,{0}, [LockTime])<getdate() ", 7), "AutoId Asc");
            BLLUser bllUser = new BLLUser();
            foreach (var lockItem in lockList)
            {
                UserInfo lockUser = bllUser.GetUserInfo(lockItem.UserId, lockItem.WebsiteOwner);
                if (lockUser == null)
                { //账号未找到
                    BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_ScoreLockInfo SET LockStatus=1 WHERE AutoId={0} And LockStatus=0;", lockItem.AutoId));
                    continue;
                }
                ProjectCommission project = GetByKey<ProjectCommission>("AutoId", lockItem.ForeignkeyId2, websiteOwner: lockItem.WebsiteOwner);
                BLLBase.ExecuteSql(string.Format("UPDATE ZCJ_ScoreLockInfo SET LockStatus=1,UnLockTime=GETDATE() WHERE AutoId={0} And LockStatus=0;", lockItem.AutoId));
                CheckTotalAmount(lockUser.AutoID, lockItem.WebsiteOwner, 7);
            }
        }
        /// <summary>
        /// 检查站点金额，计算账面金额，可用金额，公积金
        /// </summary>
        /// <param name="websiteOwner"></param>
        public void CheckTotalAmount(int userAutoId, string websiteOwner, int day)
        {
            UserInfo user = GetCol<UserInfo>(string.Format("MemberLevel>0 And WebsiteOwner='{0}' And AutoID={1} ", websiteOwner, userAutoId),
                "AutoID,UserID,TrueName,Phone,MemberLevel,MemberApplyStatus,EmptyBill");
            if (user == null) return;

            decimal accountAmountEstimate = 0;
            decimal accumulationFund = 0;
            //decimal winAmount = 0;
            //decimal totalLoseAmount = 0;
            decimal totalAmount = 0;
            BLLUserScoreDetailsInfo bllScore = new BLLUserScoreDetailsInfo();
            BLLDistribution bllDistribution = new BLLDistribution();

            //accumulationFund = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, scoreEvents: "返利,返购房补助,撤单扣返利,撤单扣购房补助", sumColName: "DeductScore");

            accumulationFund = (decimal)bllDistribution.GetUserDeductScore(user.UserID, websiteOwner);

            accountAmountEstimate = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, sumColName: "Score");

            //winAmount = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, sumColName: "Score", scoreWinStatus: "1");
            //totalLoseAmount = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, sumColName: "Score", scoreWinStatus: "2");

            string startTime = DateTime.Now.AddDays(0 - day).ToString("yyyy-MM-dd");

            List<UserScoreDetailsInfo> list = bllScore.GetScoreList(int.MaxValue, 1, websiteOwner, "TotalAmount", userIDs: user.UserID, colName: "AutoID,Score",
                scoreEvents: "返利,返购房补助,撤单扣返利,撤单扣购房补助,变更扣返利,变更扣购房补助", startTime: startTime);
            if (list.Count > 0)
            {
                decimal lockAmount = 0;
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    lockAmount = lockAmount + Convert.ToDecimal(list[i].Score);
                    if (lockAmount < 0) lockAmount = 0;
                }
                totalAmount = accountAmountEstimate - lockAmount;
            }
            else
            {
                totalAmount = accountAmountEstimate;
            }
            //decimal lockAmount = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, scoreEvents: "返利,返购房补助", sumColName: "Score",
            //    startTime: startTime);
            //decimal loseAmount = (decimal)bllScore.GetSumScore(websiteOwner, "TotalAmount", userIDs: user.UserID, scoreEvents: "撤单扣返利,撤单扣购房补助,变更扣返利,变更扣购房补助", sumColName: "Score",
            //    startTime: startTime);

            //if (loseAmount + lockAmount <0)
            //{
            //    totalAmount = accountAmountEstimate;
            //}
            //else
            //{
            //    totalAmount = accountAmountEstimate - (lockAmount + loseAmount);
            //}
            if (user.EmptyBill == 1 && user.MemberApplyStatus != 9 && totalAmount >= 0)
            {
                BLLFlow bllFlow = new BLLFlow();
                BLLJIMP.Model.Flow flow = bllFlow.GetFlowByKey("EmptyBilFill", websiteOwner);
                List<BLLJIMP.Model.FlowStep> steps = bllFlow.GetStepList(2, 1, websiteOwner, flow.AutoID);

                if (flow != null && steps.Count > 0 &&
                    !bllFlow.ExistsMemberPhoneAction(websiteOwner, "EmptyBilFill", "0", memberUserId: user.UserID))
                {
                    BLLJIMP.Model.FlowStep step1 = steps[0];
                    BLLJIMP.Model.FlowStep step2 = null;
                    if (steps.Count == 2) step2 = steps[1];
                    BLLJIMP.Model.FlowAction action = new BLLJIMP.Model.FlowAction();
                    action.CreateDate = DateTime.Now;
                    action.CreateUserID = user.UserID;
                    action.WebsiteOwner = websiteOwner;
                    action.StartStepID = step1.AutoID;
                    action.FlowID = flow.AutoID;
                    action.FlowKey = flow.FlowKey;
                    action.Amount = totalAmount;

                    action.MemberAutoID = user.AutoID;
                    action.MemberID = user.UserID;
                    action.MemberName = user.TrueName;
                    action.MemberPhone = user.Phone;
                    action.MemberLevel = user.MemberLevel;
                    UserLevelConfig levelConfig = QueryUserLevel(websiteOwner, "DistributionOnLine", user.MemberLevel.ToString());
                    action.MemberLevelName = levelConfig == null ? "" : levelConfig.LevelString;

                    action.FlowName = flow.FlowName;
                    if (step2 != null)
                    {
                        action.StepID = step2.AutoID;
                        action.StepName = step2.StepName;
                    }
                    else
                    {
                        action.Status = 9;
                        action.EndDate = DateTime.Now;
                    }

                    BLLJIMP.Model.FlowActionDetail actionDetail1 = new BLLJIMP.Model.FlowActionDetail();
                    actionDetail1.WebsiteOwner = websiteOwner;
                    actionDetail1.FlowID = flow.AutoID;
                    actionDetail1.StepID = step1.AutoID;
                    actionDetail1.StepName = step1.StepName;
                    actionDetail1.HandleUserID = user.UserID;
                    actionDetail1.HandleDate = DateTime.Now;

                    int rId = Convert.ToInt32(bllFlow.AddReturnID(action));
                    actionDetail1.ActionID = rId;
                    bllFlow.Add(actionDetail1);
                }
            }

            Update(user,
                string.Format("TotalAmount={0},AccountAmountEstimate={1},AccumulationFund={2}", totalAmount, accountAmountEstimate, accumulationFund),
                string.Format("WebsiteOwner='{0}' And AutoID={1}", websiteOwner, user.AutoID));
        }

        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="msg"></param>
        /// <param name="UnLockAmount">撤销返利</param>
        /// <returns></returns>
        public bool CancelLockAmount(UserInfo orderUserInfo, WebsiteInfo website, out string msg, out decimal UnLockAmount)
        {
            msg = "";
            UnLockAmount = 0;
            string orderUserName = bllUser.GetUserDispalyName(orderUserInfo);
            List<ScoreLockInfo> lockList = GetList<ScoreLockInfo>(string.Format(" LockType=2 And FromUserId='{0}' And WebsiteOwner='{1}'  ",
                orderUserInfo.UserID, website.WebsiteOwner));
            List<int> uIdList = new List<int>();
            uIdList.Add(orderUserInfo.AutoID);
            foreach (var item in lockList)
            {
                if (item.LockStatus == 0 || item.LockStatus == 1)
                {
                    string scoreDetailEvent = item.Memo.Contains("购房补助") ? "购房补助" : "返利";
                    UserInfo lockUserInfo = GetColByKey<UserInfo>("UserID", item.UserId, "AutoID,TotalAmount", websiteOwner: website.WebsiteOwner);
                    ProjectCommission project = GetCol<ProjectCommission>(string.Format("AutoId={0} And UserId='{1}' And WebsiteOwner='{2}' ", item.ForeignkeyId2, item.UserId, website.WebsiteOwner), "AutoId,DeductAmount");
                    decimal fund = project == null ? 0 : project.DeductAmount;
                    if (item.LockStatus == 0)
                    {
                        Update(new ScoreLockInfo(), "LockStatus=2,CancelTime=GETDATE()",
                            string.Format(" LockType=2 And LockStatus=0 And AutoId={0} And WebsiteOwner='{1}' ",
                            item.AutoId, website.WebsiteOwner));
                        if (lockUserInfo == null) continue;
                        bllUser.AddScoreDetail(item.UserId, website.WebsiteOwner, (double)(0 - item.Score),
                            string.Format("会员{0}[{1}]撤单扣除", orderUserName, orderUserInfo.Phone),
                            "TotalAmount", lockUserInfo == null ? 0 : (double)(lockUserInfo.TotalAmount - item.Score),
                            "", "撤单扣" + scoreDetailEvent, "", item.AutoId.ToString(), (double)(0 - (item.Score + fund)), (double)(0 - fund), item.FromUserId,
                            ex5: project.CommissionLevel);
                        uIdList.Add(lockUserInfo.AutoID);
                    }
                    else if (item.LockStatus == 1)
                    {
                        UnLockAmount += item.Score;
                        Update(new ScoreLockInfo(), "LockStatus=3,CancelTime=GETDATE()",
                            string.Format(" LockType=2 And LockStatus=1 And AutoId={0} And WebsiteOwner='{1}' ",
                            item.AutoId, website.WebsiteOwner));
                        if (lockUserInfo == null) continue;
                        bllUser.AddScoreDetail(item.UserId, website.WebsiteOwner, (double)(0 - item.Score),
                            string.Format("会员{0}[{1}]撤单扣除", orderUserName, orderUserInfo.Phone),
                            "TotalAmount", lockUserInfo == null ? 0 : (double)(lockUserInfo.TotalAmount - item.Score),
                            "", "撤单扣" + scoreDetailEvent, "", item.AutoId.ToString(), (double)(0 - (item.Score + fund)), (double)(0 - fund), item.FromUserId,
                            ex5: project.CommissionLevel);
                        uIdList.Add(lockUserInfo.AutoID);
                    }
                }
            }
            foreach (var uId in uIdList.Distinct())
            {
                CheckTotalAmount(uId, website.WebsiteOwner, 7);
            }
            //计算相关业绩
            BuildCurMonthPerformanceByUserID(website.WebsiteOwner, orderUserInfo.UserID);
            return true;
        }

        /// <summary>
        /// 获取用户的所有公积金
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public double GetUserDeductScore(string userId, string websiteOwner)
        {
            BLLUserScoreDetailsInfo bllUserScoreDetailsInfo = new BLLUserScoreDetailsInfo();

            var result = bllUserScoreDetailsInfo.GetSumScore(websiteOwner, "TotalAmount", userIDs: userId, scoreNotEvents: "提现退款,申请提现",
                        sumColName: "DeductScore", moreQuery: " and DeductScore <> 0 and DeductScore is not null ");

            return result;
        }

        /// <summary>
        /// 获取用户公积金列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<UserScoreDetailsInfo> GetUserDeductScoreList(string userId, string websiteOwner, int pageIndex, int pageSize)
        {
            BLLUserScoreDetailsInfo bllUserScoreDetailsInfo = new BLLUserScoreDetailsInfo();

            var list = bllUserScoreDetailsInfo.GetScoreList(pageSize, pageIndex, websiteOwner, "TotalAmount", "", userId, "", scoreNotEvents: "提现退款,申请提现", moreQuery: " and DeductScore <> 0 and DeductScore is not null ");

            return list;
        }

        /// <summary>
        /// 获取用户公积金列表总数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public int GetUserDeductScoreTotalCount(string userId, string websiteOwner)
        {
            BLLUserScoreDetailsInfo bllUserScoreDetailsInfo = new BLLUserScoreDetailsInfo();
            int result = bllUserScoreDetailsInfo.GetScoreRowCount(websiteOwner, "TotalAmount", "", userId, scoreNotEvents: "提现退款,申请提现", moreQuery: " and DeductScore <> 0 and DeductScore is not null ");

            return result;
        }

        #endregion
        #region 团队
        /// <summary>
        /// 获取上级Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="isMember"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public void GetParentIdList(ref List<string> idList, string userId, string websiteOwner)
        {
            UserInfo u = GetColByKey<UserInfo>("UserID", userId, "AutoID,UserID,DistributionOwner", websiteOwner: websiteOwner);
            if (u == null || string.IsNullOrWhiteSpace(u.DistributionOwner) || u.DistributionOwner == websiteOwner)
            {
                idList.Add(websiteOwner);
                return;
            }
            idList.Add(u.UserID);
            GetParentIdList(ref idList, u.DistributionOwner, websiteOwner);
        }
        /// <summary>
        /// 查询团队Id列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="isMember"></param>
        /// <returns></returns>
        public List<string> GetTeamIdList(string userId, string websiteOwner, bool isMember, int maxLevel = 100)
        {
            bool hasChild = true;
            List<string> nList = new List<string>();
            string tempIds = userId;
            int level = 0;
            while (hasChild)
            {
                level++;
                List<UserInfo> childs = GetChildrenIdList(tempIds, websiteOwner, isMember, "AutoID,UserID");
                if (childs.Count == 0 || level > maxLevel) break;
                List<string> tempIdList = childs.Select(p => p.UserID).ToList();
                tempIds = ZentCloud.Common.MyStringHelper.ListToStr(tempIdList, "", ",");
                nList.AddRange(tempIdList);
            }
            return nList;
        }
        /// <summary>
        /// 查询团队列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="isMember"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public List<UserInfo> GetTeamList(string userId, string websiteOwner, bool isMember, int maxLevel = 100, string colName = "")
        {
            List<string> idList = GetTeamIdList(userId, websiteOwner, isMember, maxLevel);
            if (idList.Count == 0) return new List<UserInfo>();
            string userIds = ZentCloud.Common.MyStringHelper.ListToStr(idList, "", ",");
            return GetChildrenIdList("", websiteOwner, isMember, colName, userIds);
        }

        /// <summary>
        /// 查询下级列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="isMember"></param>
        /// <returns></returns>
        public List<UserInfo> GetChildrenIdList(string parentIds, string websiteOwner, bool isMember, string colName, string userIds = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isMember) sbSql.AppendFormat(" And ( MemberLevel>=10 Or WebsiteOwner=UserID) ");
            if (!string.IsNullOrWhiteSpace(parentIds))
            {
                parentIds = "'" + parentIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And DistributionOwner In ({0}) ", parentIds);
            }
            if (!string.IsNullOrWhiteSpace(userIds))
            {
                userIds = "'" + userIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And UserID In ({0}) ", userIds);
            }
            if (string.IsNullOrWhiteSpace(colName))
            {
                return GetLit<UserInfo>(int.MaxValue, 1, sbSql.ToString(), "Regtime Desc");
            }
            return GetColList<UserInfo>(int.MaxValue, 1, sbSql.ToString(), "Regtime Desc", colName);
        }
        /// <summary>
        /// 查询下级列表
        /// </summary>
        /// <param name="parentIds"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="isMember"></param>
        /// <returns></returns>
        public List<UserInfo> GetChildrenList(int rows, int page, string Ids, string websiteOwner, bool isMember, string colName)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isMember) sbSql.AppendFormat(" And MemberLevel>=0");
            sbSql.AppendFormat(" And AutoID In ({0}) ", Ids);
            return GetColList<UserInfo>(rows, page, sbSql.ToString(), "AutoID Desc", colName);
        }

        /// <summary>
        /// 是否有下级
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="distributionOwner"></param>
        /// <returns></returns>
        public bool HaveChildrens(string websiteOwner, string distributionOwner, bool isMember)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isMember) sbSql.AppendFormat(" And MemberLevel>=0");
            sbSql.AppendFormat(" And DistributionOwner='{0}' ", distributionOwner);
            UserInfo u = GetCol<UserInfo>(sbSql.ToString(), "AutoID");
            return u != null ? true : false;
        }
        /// <summary>
        /// 下级数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="distributionOwner"></param>
        /// <returns></returns>
        public int GetChildrenCount(string websiteOwner, string distributionOwner, bool isMember)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isMember) sbSql.AppendFormat(" And MemberLevel>=0");
            sbSql.AppendFormat(" And DistributionOwner='{0}' ", distributionOwner);
            return GetCount<UserInfo>(sbSql.ToString());
        }
        #endregion 团队

        #region 账面金
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="lockStatus"></param>
        /// <returns></returns>
        public string GetLockProjectCommissionParamString(string userId, string websiteOwner, string lockStatus)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbSql.AppendFormat(" And UserId='{0}' ", userId);
            if (!string.IsNullOrWhiteSpace(lockStatus))
            {
                sbSql.AppendFormat(" And EXISTS( ");
                sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_ScoreLockInfo] ");
                sbSql.AppendFormat("    WHERE [ForeignkeyId2]=[ZCJ_ProjectCommission].[AutoId]");
                sbSql.AppendFormat("    AND [UserId]='{0}' ", userId);
                sbSql.AppendFormat("    AND [LockType]=2 ");
                sbSql.AppendFormat("    AND [WebsiteOwner]='{0}' ", websiteOwner);
                sbSql.AppendFormat("    AND [LockStatus] In ({0}) ", lockStatus);
                sbSql.AppendFormat(" ) ");
            }
            else
            {
                sbSql.AppendFormat(" And EXISTS( ");
                sbSql.AppendFormat("    SELECT 1 FROM [ZCJ_ScoreLockInfo] ");
                sbSql.AppendFormat("    WHERE [ForeignkeyId2]=[ZCJ_ProjectCommission].[AutoId]");
                sbSql.AppendFormat("    AND [UserId]='{0}' ", userId);
                sbSql.AppendFormat("    AND [LockType]=2 ");
                sbSql.AppendFormat("    AND [WebsiteOwner]='{0}' ", websiteOwner);
                sbSql.AppendFormat("    AND [LockStatus]=0 ", lockStatus);
                sbSql.AppendFormat(" ) ");
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="lockStatus"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<ProjectCommission> GetLockProjectCommissionList(int rows, int page, string userId, string websiteOwner, string lockStatus, string colName = "")
        {
            if (!string.IsNullOrWhiteSpace(colName)) return GetColList<ProjectCommission>(rows, page, GetLockProjectCommissionParamString(userId, websiteOwner, lockStatus), "AutoID Desc", colName);
            return GetLit<ProjectCommission>(rows, page, GetLockProjectCommissionParamString(userId, websiteOwner, lockStatus), "AutoID Desc");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="lockStatus"></param>
        /// <returns></returns>
        public int GetLockProjectCommissionCount(string userId, string websiteOwner, string lockStatus)
        {
            return GetCount<ProjectCommission>(GetLockProjectCommissionParamString(userId, websiteOwner, lockStatus));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="lockStatus"></param>
        /// <returns></returns>
        public decimal GetLockProjectCommissionSum(string userId, string websiteOwner, string lockStatus, string sumColName)
        {
            decimal result = 0;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" SELECT SUM({0}) FROM [ZCJ_ProjectCommission] WHERE ", sumColName);
            sbSql.AppendFormat(GetLockProjectCommissionParamString(userId, websiteOwner, lockStatus));
            var totalCount = GetSingle(sbSql.ToString());
            if (totalCount != null)
            {
                result = Convert.ToDecimal(totalCount);
            }
            return result;
        }
        #endregion 账面金

        #region 团队业绩
        /// <summary>
        /// 拼查询条件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public string GetPerformanceParamString(string userIds, string parentIds, string websiteOwner, int yearMonth)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner ='{0}' ", websiteOwner);
            if (yearMonth > 0) sbSql.AppendFormat(" And YearMonth ={0} ", yearMonth);
            if (!string.IsNullOrWhiteSpace(userIds))
            {
                userIds = "'" + userIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And UserId In ({0}) ", userIds);
            }
            if (!string.IsNullOrWhiteSpace(parentIds))
            {
                parentIds = "'" + parentIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And DistributionOwner In ({0}) ", parentIds);
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// 个人业绩
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public TeamPerformance GetMyPerformance(string userId, string websiteOwner, int yearMonth)
        {
            return Get<TeamPerformance>(GetPerformanceParamString(userId, "", websiteOwner, yearMonth));
        }
        /// <summary>
        /// 下级业绩
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public List<TeamPerformance> GetChildPerformanceList(int rows, int page, string parentId, string websiteOwner, int yearMonth,
            string userIds = "", string colName = "")
        {
            if (!string.IsNullOrWhiteSpace(colName))
            {
                return GetColList<TeamPerformance>(rows, page, GetPerformanceParamString(userIds, parentId, websiteOwner, yearMonth),
                    "UserName asc,UserId asc,YearMonth desc,Performance desc,AutoId desc", colName);
            }
            return GetLit<TeamPerformance>(rows, page, GetPerformanceParamString(userIds, parentId, websiteOwner, yearMonth),
                "UserName asc,UserId asc,YearMonth desc,Performance desc,AutoId desc");
        }
        /// <summary>
        /// 汇总金额
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public decimal GetPerformanceSum(string parentId, string websiteOwner, int yearMonth, string userIds = "")
        {
            decimal result = 0;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" SELECT SUM(Performance) FROM [ZCJ_TeamPerformance] WHERE ");
            sbSql.AppendFormat(GetPerformanceParamString(userIds, parentId, websiteOwner, yearMonth));
            var totalCount = GetSingle(sbSql.ToString());
            if (totalCount != null)
            {
                result = Convert.ToDecimal(totalCount);
            }
            return result;
        }

        /// <summary>
        /// 下级记录数
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public int GetChildPerformanceCount(string parentId, string websiteOwner, int yearMonth, string userIds = "")
        {
            return GetCount<TeamPerformance>(GetPerformanceParamString(userIds, parentId, websiteOwner, yearMonth));
        }

        /// <summary>
        /// 汇总某月业绩
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="websiteOwner"></param>
        public bool BuildMonthPerformance(int yearMonth, string websiteOwner)
        {
            #region 查询消费
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" SELECT A.AutoId,A.DistributionOwner [UserID],B.DistributionOwner,B.TrueName UserName,B.Phone UserPhone,A.[Performance] ");
            sbSql.AppendFormat(" FROM [ZCJ_TeamPerformanceDetails] A ");
            sbSql.AppendFormat(" Left join [ZCJ_UserInfo] B on  B.WebsiteOwner='{0}' and A.DistributionOwner=B.UserID ", websiteOwner);
            sbSql.AppendFormat(" where A.WebsiteOwner='{0}' AND A.YearMonth={1} ", websiteOwner, yearMonth);
            //sbSql.AppendFormat(" and A.[AutoId] In(1001345,1001349,1001393,1001413,1001296)");
            //     sbSql.AppendFormat(" and A.DistributionOwner In('ZYUser20161219411543F5168D4214858457E460DD5BC6','ZYUser2016123023B8BED80E224B519AD49F1A2D1AD933','ZYUser2016121908D9E76C79D9464F8BABB53D807824C1','ZYUser20161219B49D233753E349DC8A9090F563129570','ZYUser201612199F79C31DD261452AAFB71E6682E1AFBB','ZYUser20161228E43B4A56D3374991BBD88069B863CBE7','ZYUser20161228FDBB4BB480B845059D702B2261A591E4','ZYUser20170114AB8B3725DE7D4C2F9F487ADBCB02C7D8','ZYUser201701302195B58179DF4ECC91294EBCD4BC91B6','WXUser97c6a30e-48b3-4069-ab74-cf47196a023c','ZYUser201612199C272ED4EB42461F8DD5A17670DB2BB2','ZYUser20161228246734B04835441BA93E41E516F697D7','ZYUser20170130EEE2E4A50D6D4E55B9D16A166E8C977C','ZYUser201701306DE8D2781F6141C7BE92B96BBD2DDADD','ZYUser20161219411543F5168D4214858457E460DD5BC6') ");
            List<TeamPerformanceDetails> list = Query<TeamPerformanceDetails>(sbSql.ToString());
            #endregion 查询消费
            #region 构造数据
            if (list.Count > 0)
            {
                DateTime curDate = DateTime.Now;
                List<TeamPerformance> resultList = new List<TeamPerformance>();
                foreach (var item in list)
                {
                    BuildPerformanceList(resultList, websiteOwner, yearMonth, item.UserId, item.DistributionOwner,
                        item.UserName, item.UserPhone, item.Performance, item.AutoId);
                }
                List<TeamPerformance> oidList = GetChildPerformanceList(int.MaxValue, 1, "", websiteOwner, yearMonth);
                foreach (var item in resultList)
                {
                    TeamPerformance o = oidList.FirstOrDefault(p => p.UserId == item.UserId);
                    if (o == null)
                    {
                        item.DetailIds = ZentCloud.Common.MyStringHelper.ListToStr(item.DetailIDList, "", ",");
                        item.UpdateDate = curDate;
                        Add(item);
                    }
                    else
                    {
                        o.Performance = item.Performance;
                        o.DetailIds = ZentCloud.Common.MyStringHelper.ListToStr(item.DetailIDList, "", ",");
                        o.UpdateDate = curDate;
                        Update(o);
                    }
                }
                //删除多余的
                List<string> userIdList = resultList.Select(u => u.UserId).ToList();
                foreach (var item in oidList.Where(p => !userIdList.Contains(p.UserId)))
                {
                    Delete(item);
                }
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 计算某人本月业绩
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool BuildCurMonthPerformanceByUserID(string websiteOwner, string userId)
        {
            string yearMonthString = DateTime.Now.ToString("yyyyMM");
            int yearMonth = Convert.ToInt32(yearMonthString);
            return BuildMonthPerformance(yearMonth, websiteOwner);
        }
        public void BuildPerformanceList(List<TeamPerformance> resultList, string websiteOwner, int yearMonth,
            string userId, string distributionOwner, string userName, string userPhone, decimal performance, int detailID)
        {
            TeamPerformance oPerformance = resultList.FirstOrDefault(p => p.UserId == userId);
            if (oPerformance == null)
            {
                TeamPerformance item = new TeamPerformance(websiteOwner, yearMonth, userId, distributionOwner, userName, userPhone, performance, detailID);
                resultList.Add(item);
            }
            else
            {
                oPerformance.Performance += performance;
                if (detailID > 0) oPerformance.DetailIDList.Add(detailID);
            }
            if (string.IsNullOrWhiteSpace(distributionOwner)) return;
            TeamPerformance pPerformance = resultList.FirstOrDefault(p => p.UserId == distributionOwner);
            if (pPerformance == null)
            {
                UserInfo u = GetCol<UserInfo>(
                    string.Format("WebsiteOwner='{0}' And UserID ='{1}' ", websiteOwner, distributionOwner),
                    "AutoID,UserID,DistributionOwner,TrueName,Phone");
                string pDistributionOwner = u == null ? "" : u.DistributionOwner;
                string pUserName = u == null ? "" : u.TrueName;
                string pUserPhone = u == null ? "" : u.Phone;
                BuildPerformanceList(resultList, websiteOwner, yearMonth, distributionOwner, pDistributionOwner, pUserName, pUserPhone, performance, detailID);
            }
            else
            {
                BuildPerformanceList(resultList, websiteOwner, yearMonth, distributionOwner, pPerformance.DistributionOwner, pPerformance.UserName,
                    pPerformance.UserPhone, performance, detailID);
            }
        }

        /// <summary>
        /// 拼查询条件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public string GetPerformanceDetailsParamString(string userIds, string parentIds, string websiteOwner, int yearMonth,
            string start = "", string ids = "")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner ='{0}' ", websiteOwner);
            sbSql.AppendFormat(" And Performance !=0 ");
            if (yearMonth > 0) sbSql.AppendFormat(" And YearMonth ={0} ", yearMonth);
            if (!string.IsNullOrWhiteSpace(userIds))
            {
                userIds = "'" + userIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And UserId In ({0}) ", userIds);
            }
            if (!string.IsNullOrWhiteSpace(parentIds))
            {
                parentIds = "'" + parentIds.Replace(",", "','") + "'";
                sbSql.AppendFormat(" And DistributionOwner In ({0})  ", parentIds);
            }
            if (!string.IsNullOrWhiteSpace(start))
            {
                sbSql.AppendFormat(" And AddTime>='{0}' ", start);
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                sbSql.AppendFormat(" And AutoId In ({0})  ", ids);
            }
            return sbSql.ToString();
        }

        /// <summary>
        /// 用户历史消费及业绩（含下级）
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public decimal GetChildPerformanceDetailSum(string userId, string websiteOwner, int lastYearMonth)
        {
            decimal childTotalCount = 0;
            decimal userTotalCount = 0;
            string sql = "";
            List<string> childList = new List<string>() { };
            bool hasChild = true;
            string tempIds = userId;
            while (hasChild)
            {
                tempIds = "'" + tempIds.Replace(",", "','") + "'";
                sql = string.Format("SELECT DISTINCT [UserId] FROM [ZCJ_TeamPerformanceDetails] where [WebsiteOwner]='{0}' AND [DistributionOwner] In ({1})",
                    websiteOwner, tempIds);
                List<UserInfo> tempList = Query<UserInfo>(sql);
                if (tempList.Count == 0) break;

                List<string> tempIdList = tempList.Select(p => p.UserID).ToList();
                tempIds = ZentCloud.Common.MyStringHelper.ListToStr(tempIdList, "", ",");
                childList.AddRange(tempIdList);
            }
            if (childList.Count > 0)
            {
                string childIds = ZentCloud.Common.MyStringHelper.ListToStr(childList, "", ",");
                sql = string.Format(" SELECT SUM(Performance) FROM [ZCJ_TeamPerformanceDetails] WHERE {0} And YearMonth <{1} ",
                    GetPerformanceDetailsParamString(childIds, "", websiteOwner, 0), lastYearMonth);
                var childTotal = GetSingle(sql);
                if (childTotal != null) childTotalCount = Convert.ToDecimal(childTotal);
            }
            sql = string.Format(" SELECT SUM(Performance) FROM [ZCJ_TeamPerformanceDetails] WHERE {0} ",
                GetPerformanceDetailsParamString(userId, "", websiteOwner, 0));
            var userTotal = GetSingle(sql);
            if (userTotal != null) userTotalCount = Convert.ToDecimal(userTotal);

            return childTotalCount + userTotalCount;
        }

        public List<string> GetPerformanceChildrenIdList(string parentIds, string websiteOwner, int yearMonth)
        {
            List<string> nList = new List<string>();
            string tempIds = parentIds;
            bool hasChild = true;
            StringBuilder sbSql = new StringBuilder();
            while (hasChild)
            {
                tempIds = "'" + tempIds.Replace(",", "','") + "'";

                sbSql = new StringBuilder();
                sbSql.AppendFormat(" With Temp AS( ");
                sbSql.AppendFormat(" SELECT UserID ");
                sbSql.AppendFormat(" From [ZCJ_UserInfo] ");
                sbSql.AppendFormat(" WHERE WebsiteOwner='{1}' And DistributionOwner In ({0}) And  ", tempIds, websiteOwner);
                sbSql.AppendFormat(" Not Exists(SELECT 1 FROM [ZCJ_TeamPerformanceDetails] where WebsiteOwner='{2}' And AddType='{0}' and Performance>0 and [YearMonth]<{1}) ",
                    "变更", yearMonth, websiteOwner);
                sbSql.AppendFormat(" Union  ");
                sbSql.AppendFormat(" SELECT UserID From( ");
                sbSql.AppendFormat(" SELECT row_number() over(partition by [UserId] order by AddTime ASC) Num, ");
                sbSql.AppendFormat("  [UserId] ");
                sbSql.AppendFormat("   FROM [ZCJ_TeamPerformanceDetails] ");
                sbSql.AppendFormat("   WHERE WebsiteOwner='{3}' And AddType='{1}' and  Performance<0 and [YearMonth]>{2} And DistributionOwner In ({0}) ",
                    tempIds, "变更", yearMonth, websiteOwner);
                sbSql.AppendFormat(" ) A WHERE Num=1 ");
                sbSql.AppendFormat(" ) SELECT DISTINCT Convert(int, row_number() over(order by UserID ASC)) AS AutoID,UserID FROM Temp ");
                List<UserInfo> tempList = Query<UserInfo>(sbSql.ToString());
                if (tempList.Count == 0) break;

                List<string> tempIdList = tempList.Select(p => p.UserID).Distinct().ToList();
                tempIds = ZentCloud.Common.MyStringHelper.ListToStr(tempIdList, "", ",");
                nList.AddRange(tempIdList);
            }
            return nList;
        }
        public decimal GetPerformanceDetailSum(string userIds, string parentIds, string websiteOwner, int yearMonth,
            string start = "", string ids = "")
        {
            decimal result = 0;
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" SELECT SUM([Performance]) FROM [ZCJ_TeamPerformanceDetails] WHERE ");
            sbSql.AppendFormat(GetPerformanceDetailsParamString(userIds, parentIds, websiteOwner, yearMonth, start, ids));
            var totalCount = GetSingle(sbSql.ToString());
            if (totalCount != null)
            {
                result = Convert.ToDecimal(totalCount);
            }
            return result;
        }

        public List<TeamPerformanceDetails> GetPerformanceDetailList(int rows, int page, string userIds, string parentIds,
            string websiteOwner, int yearMonth, string start = "", string ids = "")
        {
            return GetLit<TeamPerformanceDetails>(rows, page,
                GetPerformanceDetailsParamString(userIds, parentIds, websiteOwner, yearMonth, start, ids),
                "AddTime desc,AutoId desc");
        }
        public int GetPerformanceDetailCount(string userIds, string parentIds, string websiteOwner, int yearMonth,
            string start = "", string ids = "")
        {
            return GetCount<TeamPerformanceDetails>(
                GetPerformanceDetailsParamString(userIds, parentIds, websiteOwner, yearMonth, start, ids));
        }
        #endregion 团队业绩

        /// <summary>
        /// 变更推荐人
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="id"></param>
        /// <param name="spreadid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateDistributionOwner(string websiteOwner, string id, string spreadid, UserInfo currentUserInfo, out string msg)
        {
            msg = "";
            DateTime curTime = DateTime.Now;

            UserInfo curUser = GetColByKey<UserInfo>("AutoID", id, "AutoID,UserID,TrueName,Phone,MemberApplyStatus,DistributionOwner", websiteOwner: websiteOwner);
            UserInfo oDisUser = GetColByKey<UserInfo>("UserID", curUser.DistributionOwner, "AutoID,UserID,TrueName,Phone,MemberApplyStatus", websiteOwner: websiteOwner);
            UserInfo tDisUser = GetColByKey<UserInfo>("AutoID", spreadid, "AutoID,UserID,TrueName,Phone,MemberApplyStatus", websiteOwner: websiteOwner);

            if (curUser == null)
            {
                msg = "会员为空";
                return false;
            }
            if (oDisUser == null)
            {
                msg = "原推荐人为空";
                return false;
            }
            if (tDisUser == null)
            {
                msg = "新推荐人为空";
                return false;
            }
            List<string> idList = new List<string>();
            GetParentIdList(ref idList, tDisUser.UserID, websiteOwner);
            if (idList.Contains(curUser.UserID))
            {
                msg = "新推荐人不能为会员的下级";
                return false;
            }

            BLLUserScoreDetailsInfo bllScore = new BLLUserScoreDetailsInfo();
            UserScoreDetailsInfo oRegLog = bllScore.GetNewScore(websiteOwner, "TotalAmount", "", curUser.UserID, "AutoID,AddTime", "变更推荐人,他人代替注册,注册会员,撤单");

            if (oRegLog == null)
            {
                msg = "注册记录为空";
                return false;
            }
            string start = oRegLog.AddTime.ToString("yyyy-MM-dd HH:mm");

            List<UserScoreDetailsInfo> reduceScoreList = new List<UserScoreDetailsInfo>();
            List<UserScoreDetailsInfo> addScoreList = new List<UserScoreDetailsInfo>();
            List<TeamPerformanceDetails> addPerformanceList = new List<TeamPerformanceDetails>();

            string addNote = string.Format("{4}[{5}]的推荐人由{0}[{1}]改为{2}[{3}]",
                        oDisUser.TrueName, oDisUser.Phone, tDisUser.TrueName, tDisUser.Phone, curUser.TrueName, curUser.Phone);

            #region 计算变更时分佣积分
            List<UserScoreDetailsInfo> list = bllScore.GetScoreList(int.MaxValue, 1, websiteOwner, "TotalAmount",
                colName: "AutoID,UserID,Score,AddNote,AddTime,ScoreEvent,EventScore,DeductScore",
                scoreEvents: "返利,返购房补助", startTime: start, relationUserID: curUser.UserID);

            if (curUser.MemberApplyStatus == 9 && list.Count <= 0)
            {
                msg = "历史分佣为空";
                return false;
            }
            else if (curUser.MemberApplyStatus == 9 && list.Count > 0)
            {
                List<SumInfo> sumList = list.GroupBy(p => new
                {
                    p.UserID,
                    p.ScoreEvent,
                    p.Ex5
                }).Select(g => new SumInfo
                {
                    UserID = g.Key.UserID,
                    ScoreEvent = g.Key.ScoreEvent,
                    Ex5 = g.Key.Ex5,
                    SumScore = g.Sum(pi => pi.Score),
                    SumEventScore = g.Sum(pi => pi.EventScore),
                    SumDeductScore = g.Sum(pi => pi.DeductScore)
                }).ToList();
                foreach (SumInfo item in sumList)
                {
                    UserScoreDetailsInfo tItem = new UserScoreDetailsInfo();
                    tItem.UserID = item.UserID;
                    tItem.AddNote = addNote;
                    tItem.AddTime = curTime;
                    tItem.ScoreEvent = item.ScoreEvent == "返购房补助" ? "变更扣购房补助" : "变更扣返利";
                    tItem.Score = (0 - item.SumScore);
                    tItem.EventScore = item.SumEventScore;
                    tItem.DeductScore = (0 - item.SumDeductScore);
                    tItem.RelationUserID = curUser.UserID;
                    tItem.ScoreType = "TotalAmount";
                    tItem.Ex5 = item.Ex5;
                    tItem.WebSiteOwner = websiteOwner;
                    reduceScoreList.Add(tItem);

                    if (item.UserID == oDisUser.UserID)
                    {
                        UserScoreDetailsInfo tnItem = new UserScoreDetailsInfo();
                        tnItem.UserID = tDisUser.UserID;
                        tnItem.AddNote = addNote;
                        tnItem.AddTime = curTime;
                        tnItem.ScoreEvent = item.ScoreEvent;
                        tnItem.Score = item.SumScore;
                        tnItem.EventScore = item.SumEventScore;
                        tnItem.DeductScore = item.SumDeductScore;
                        tnItem.RelationUserID = curUser.UserID;
                        tnItem.ScoreType = "TotalAmount";
                        tnItem.Ex5 = item.Ex5;
                        tnItem.WebSiteOwner = websiteOwner;
                        addScoreList.Add(tnItem);
                    }
                }
            }
            #endregion 计算变更时分佣积分

            #region 构造所得业绩

            string yearMonthString = curTime.ToString("yyyyMM");
            int yearMonth = Convert.ToInt32(yearMonthString);
            decimal oSumPerformance = GetChildPerformanceDetailSum(curUser.UserID, websiteOwner, yearMonth);

            if (curUser.MemberApplyStatus == 9 && oSumPerformance <= 0)
            {
                msg = "历史业绩为空";
                return false;
            }
            else if (curUser.MemberApplyStatus == 9 && oSumPerformance > 0)
            {
                TeamPerformanceDetails perDetail = new TeamPerformanceDetails();
                perDetail.AddType = "变更";
                perDetail.AddNote = addNote;
                perDetail.AddTime = curTime;
                perDetail.DistributionOwner = oDisUser.UserID;
                perDetail.UserId = curUser.UserID;
                perDetail.UserName = curUser.TrueName;
                perDetail.UserPhone = curUser.Phone;
                perDetail.Performance = (0 - oSumPerformance);
                perDetail.YearMonth = yearMonth;
                perDetail.WebsiteOwner = websiteOwner;
                addPerformanceList.Add(perDetail);
                TeamPerformanceDetails tPerDetail = new TeamPerformanceDetails();
                tPerDetail.AddType = "变更";
                tPerDetail.AddNote = addNote;
                tPerDetail.AddTime = curTime;
                tPerDetail.DistributionOwner = tDisUser.UserID;
                tPerDetail.UserId = curUser.UserID;
                tPerDetail.UserName = curUser.TrueName;
                tPerDetail.UserPhone = curUser.Phone;
                tPerDetail.Performance = oSumPerformance;
                tPerDetail.YearMonth = yearMonth;
                tPerDetail.WebsiteOwner = websiteOwner;
                addPerformanceList.Add(tPerDetail);
            }
            #endregion

            BLLTransaction tran = new BLLTransaction();
            if (Update(curUser,
                string.Format("DistributionOwner='{0}'", tDisUser.UserID),
                string.Format("AutoID={0}", curUser.AutoID), tran) <= 0)
            {
                tran.Rollback();
                msg = "变更推荐人失败";
                return false;
            }
            int logId = bllUser.AddScoreDetail(curUser.UserID, websiteOwner, (double)0,
                    addNote, "TotalAmount", (double)0, "", "变更推荐人", "",
                    "", (double)0, (double)0, "", tran, addtime: curTime);
            if (logId <= 0)
            {
                tran.Rollback();
                msg = "变更记录失败";
                return false;
            }
            foreach (UserScoreDetailsInfo reduceItem in reduceScoreList)
            {
                reduceItem.RelationID = logId.ToString();
                if (!Add(reduceItem, tran))
                {
                    tran.Rollback();
                    msg = "变更分佣失败";
                    return false;
                }
            }
            foreach (UserScoreDetailsInfo addItem in addScoreList)
            {
                addItem.RelationID = logId.ToString();
                if (!Add(addItem, tran))
                {
                    tran.Rollback();
                    msg = "变更分佣失败";
                    return false;
                }
            }
            foreach (TeamPerformanceDetails addItem in addPerformanceList)
            {
                if (!Add(addItem, tran))
                {
                    tran.Rollback();
                    msg = "变更业绩失败";
                    return false;
                }
            }
            tran.Commit();

            bllLog.Add(EnumLogType.ShMember, EnumLogTypeAction.Update, currentUserInfo.UserID, addNote, targetID: curUser.UserID);
            //计算余额，计算业绩
            Thread th1 = new Thread(delegate()
            {
                CheckTotalAmount(curUser.AutoID, websiteOwner, 7);
                CheckTotalAmount(oDisUser.AutoID, websiteOwner, 7);
                CheckTotalAmount(tDisUser.AutoID, websiteOwner, 7);
                BuildCurMonthPerformanceByUserID(websiteOwner, curUser.UserID);
            });
            th1.Start();


            //更改推荐人成功，记录日志并修改到分销会员表
            new BLLUserDistributionMember().SetUserDistributionOwnerInMember(new List<string>() { currentUserInfo.UserID }, spreadid, websiteOwner);
            ToLog("月供宝会员变更推荐人 修改关系 userid:" + currentUserInfo.UserID + "  DistributionOwner" + spreadid, "D:\\log\\BLLUserDistributionMember.txt");


            return true;
        }
        /// <summary>
        /// 获取规则查询条件
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSetParamString(string websiteOwner, string userId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" WebsiteOwner ='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(userId)) sb.AppendFormat(" And UserId ='{0}' ", userId);
            return sb.ToString();
        }
        /// <summary>
        /// 获取规则数量
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSetCount(string websiteOwner, string userId)
        {
            return GetCount<TeamPerformanceSet>(GetSetParamString(websiteOwner, userId));
        }
        /// <summary>
        /// 获取规则列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TeamPerformanceSet> GetSetList(int rows, int page, string websiteOwner, string userId)
        {
            return GetLit<TeamPerformanceSet>(rows, page, GetSetParamString(websiteOwner, userId));
        }
        /// <summary>
        /// 计算管理奖
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="yearMonth"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="minLevel"></param>
        /// <returns></returns>
        public bool ComputeReward(out string msg, int yearMonth, string websiteOwner, int minLevel)
        {
            msg = "计算完成";

            #region 获取基本规则
            List<TeamPerformanceSet> baseSets = GetListByKey<TeamPerformanceSet>("UserId", websiteOwner, websiteOwner: websiteOwner);
            if (baseSets.Count == 0)
            {
                msg = "未设置计算规则";
                return false;
            }
            baseSets = baseSets.OrderByDescending(p => p.Performance).ToList();
            #endregion

            //查出所有本月业绩
            List<TeamPerformance> rewardList = GetChildPerformanceList(int.MaxValue, 1, "", websiteOwner, yearMonth,
                "", "AutoID,UserId,DistributionOwner,Performance");
            if (rewardList.Count == 0) return true;

            if (rewardList.Exists(p => p.FlowActionId > 0))
            {
                msg = "已有人提交当月管理奖确认审核";
                return false;
            }

            #region 计算每行业绩的奖励
            foreach (var item in rewardList)
            {
                UserInfo itemUser = GetColByKey<UserInfo>("UserID", item.UserId,
                    "AutoID,UserID,MemberLevel,IsDisable,MemberApplyStatus,EmptyBill,IsLock", websiteOwner: websiteOwner);
                if (itemUser == null || itemUser.IsDisable == 1 || itemUser.IsLock == 1 ||
                    itemUser.MemberApplyStatus != 9 || itemUser.MemberLevel < minLevel) continue;

                List<TeamPerformanceSet> userSets = GetListByKey<TeamPerformanceSet>("UserId", item.UserId, websiteOwner: websiteOwner);
                if (userSets.Count == 0)
                {
                    userSets = baseSets;
                }
                else
                {
                    userSets = userSets.OrderByDescending(p => p.Performance).ToList();
                }
                TeamPerformanceSet itemSet = userSets.FirstOrDefault(p => p.Performance <= item.Performance);
                if (itemSet == null) continue;

                item.Rate = itemSet.RewardRate;
                item.TotalReward = item.Rate / 100 * item.Performance;
            }
            #endregion

            #region 汇总每行业绩的下级奖励
            bool buildResult = true;
            foreach (var item in rewardList.Where(p => p.TotalReward != 0 && !string.IsNullOrWhiteSpace(p.DistributionOwner)))
            {
                buildResult = BuildRewardList(out msg, rewardList, item.DistributionOwner, item.TotalReward);
            }
            #endregion
            #region 计算所得奖励
            foreach (var item in rewardList)
            {
                item.Reward = item.TotalReward - item.ChildReward;
                Update(item,
                    string.Format("Rate={0},Reward={1},ChildReward={2},TotalReward={3}", item.Rate, item.Reward, item.ChildReward, item.TotalReward),
                    string.Format("AutoID={0}", item.AutoID));
            }
            #endregion
            return true;
        }
        /// <summary>
        /// 管理奖数据处理
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="rewardList"></param>
        /// <param name="distributionOwner"></param>
        /// <param name="reward"></param>
        /// <returns></returns>
        public bool BuildRewardList(out string msg, List<TeamPerformance> rewardList, string distributionOwner, decimal reward)
        {
            msg = "";
            TeamPerformance pPeward = rewardList.FirstOrDefault(p => p.UserId == distributionOwner);
            if (pPeward == null)
            {
                msg = "业绩计算出错";
                return false;
            }
            if (pPeward.Rate > 0)
            {
                pPeward.ChildReward += reward;
                return true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(pPeward.DistributionOwner))
                {
                    bool buildResult = BuildRewardList(out msg, rewardList, pPeward.DistributionOwner, reward);
                    return buildResult;
                }
                return true;
            }
        }





        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class SumInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string UserID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ScoreEvent { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Ex5 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double SumScore { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double SumEventScore { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double SumDeductScore { get; set; }
        }

        [Serializable]
        public class InitUserDistributionMemberInfo : ZentCloud.ZCBLLEngine.ModelTable
        {
            public string DistributionOwner { get; set; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using System.Data;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 用户积分表
    /// </summary>
    public class BLLUserScore : BLL
    {
        ZentCloud.BLLJIMP.Model.UserInfo userInfo;
        public BLLUserScore(string userId)
            : base(userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("userid can not be empty");  //exception需要重组
            }

            userInfo = this.Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("UserID='{0}'", this.UserID));
            if (userInfo == null)
            {
                throw new Exception("user not exist");  //exception需要重组
            }
        }

        //返回用户积分
        public double GetTotalScore()
        {
            return userInfo.TotalScore;
        }

        //给用户增加积分(负值为减积分)，同时写入积分历史,返回修改的delta分值,
        //并调用微信模板消息通知用户
        public double UpdateUserScoreWithWXTMNotify(UserScore userScore, string accessToken)
        {
            double deltaScore = UpdateUserScore(userScore);
            if ((int)deltaScore != 0 && accessToken != string.Empty)
            {
                switch (userScore.UserScoreType)
                {
                    case UserScoreType.ShareActivityToWXFriendGroup:
                    case UserScoreType.ShareArticleToWXFriendGroup:
                    case UserScoreType.SharePositionToWXFriendGroup:
                    case UserScoreType.ShareTutorToWXFriendGroup:
                    case UserScoreType.TutorShareActivityToWXFriendGroup:
                    case UserScoreType.TutorShareArticleToWXFriendGroup:
                    case UserScoreType.TutorShareTutorToWXFriendGroup:
                    case UserScoreType.TutorSharePositionToWXFriendGroup:
                        System.Threading.Thread.Sleep(30 * 1000);
                        break;
                    default:
                        break;
                }

                BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
                BLLWeixin.TMScoreNotification notificaiton = new BLLWeixin.TMScoreNotification();
                notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/Index.aspx", System.Web.HttpContext.Current.Request.Url.Host);
                notificaiton.TemplateId = "8JLCEV3HmIYV3C3bDiGARxRBJqAZosnQEOI4C0d5He4";
                notificaiton.First = "您有新的积分变化，详情如下";
                notificaiton.Account = userInfo.WXNickname;
                notificaiton.Time = DateTime.Now.ToString();
                notificaiton.Type = userScore.RecordTypeString;
                notificaiton.CreditChange = "变化";
                notificaiton.Number = userScore.Score.ToString();
                notificaiton.Amount = userInfo.TotalScore.ToString();
                notificaiton.Remark = "您可以点击下方菜单进入五步会，赚取更多积分！";
                bllWeixin.SendTemplateMessage(accessToken, userInfo.WXOpenId, notificaiton);

            }
            return deltaScore;
        }

        //给用户增加积分(负值为减积分)，同时写入积分历史,返回修改的delta分值
        public double UpdateUserScore(UserScore userScore)
        {
            try
            {
                ZentCloud.ZCBLLEngine.BLLTransaction bllTransaction = new ZentCloud.ZCBLLEngine.BLLTransaction();

                //记录加减分历史
                if (userScore.Score != 0)
                {
                    if (SaveScoreRecord(userScore, bllTransaction) == 0)
                    {
                        bllTransaction.Rollback();
                        return 0;
                    }
                }

                userInfo.TotalScore += userScore.Score;
                if (userScore.Score > 0) //减分不计入累计积分
                {
                    userInfo.HistoryTotalScore += userScore.Score;
                }
                if (this.Update(userInfo, string.Format(" TotalScore={0},HistoryTotalScore={1}",userInfo.TotalScore,userInfo.HistoryTotalScore), string.Format(" AutoID={0}", userInfo.AutoID), bllTransaction) < 1) //更改用户积分
                {
                    bllTransaction.Rollback();
                    return 0;
                }

                bllTransaction.Commit();
                return userScore.Score;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //保存加分历史记录，返回加分值，0为保存失败。
        private double SaveScoreRecord( UserScore userScore, BLLTransaction transaction)
        {
            string deltaScoreStr = string.Empty;
            if (userScore.Score > 0)
            {
                deltaScoreStr = string.Format("+{0}", userScore.Score);
            }
            else
            {
                deltaScoreStr = string.Format("{0}", userScore.Score);
            }
            BLLJIMP.Model.WBHScoreRecord srInfo = new BLLJIMP.Model.WBHScoreRecord()
            {
                InsertDate = DateTime.Now,

                ScoreNum = deltaScoreStr,
                WebsiteOwner = this.userInfo.WebsiteOwner,
                UserId = this.UserID,
                NameStr = userScore.RecordTypeString,
                Nums = userScore.nums,
                RecordType = userScore.RecordType,
            };

            if( this.Add(srInfo))
            {
                return userScore.Score;
            }
            return 0;
        }

        public static string SendTMUserScoreDailyAccountBillNotify(string receiverUserId, string websiteOwner)
        {
            BLLWeixin bllWeixin = new BLLWeixin("");
            BLLWeixin.TMScoreNotification notificaiton = new BLLWeixin.TMScoreNotification();
            notificaiton.Url = string.Format("http://{0}/WuBuHui/MyCenter/Index.aspx", System.Web.HttpContext.Current.Request.Url.Host);
            notificaiton.TemplateId = "8JLCEV3HmIYV3C3bDiGARxRBJqAZosnQEOI4C0d5He4";
            notificaiton.First = "您今日的积分对账单，详情如下";
            notificaiton.Account = "积分账户";
            notificaiton.Time = DateTime.Now.ToString();
            notificaiton.Type = "每日积分对账单";
            //notificaiton.CreditChange = "每日积分对账单";
            UserInfo userInfo = bllWeixin.Get<UserInfo>(string.Format("UserId='{0}' and WebsiteOwner='{1}'", receiverUserId, websiteOwner));
            if (userInfo == null)
            {
                return string.Format("用户 {0} 不存在", receiverUserId);
            }
            notificaiton.Number = userInfo.TotalScore.ToString();
            notificaiton.Amount = userInfo.TotalScore.ToString();
            notificaiton.Remark = "您可以点击下方菜单进入五步会，赚取更多积分！";
            return bllWeixin.SendTemplateMessage(bllWeixin.GetAccessToken(), userInfo.WXOpenId, notificaiton);
        }


        public static bool SendTMAllUserScoreDailyAccountBillNotify(string websiteOwner)
        {
            BLLWeixin bllWeixin = new BLLWeixin("");
            WeixinFollowers model = Common.JSONHelper.JsonToModel<WeixinFollowers>(bllWeixin.GetFollower(bllWeixin.GetAccessToken(), string.Empty));
            while (model.count > 0)
            {
                Dictionary<string, object> dicOpenId = (Dictionary<string, object>)model.data;
                object[] openidArry = (object[])dicOpenId.First().Value;
                foreach (var openid in openidArry)
                {
                    UserInfo userInfo = bllWeixin.Get<UserInfo>(string.Format("WXOpenId='{0}' and WebsiteOwner='{1}'", openid, websiteOwner));
                    if (userInfo == null)
                    {
                        continue;
                    }
                    SendTMUserScoreDailyAccountBillNotify(userInfo.UserID, websiteOwner);
                }
                model = Common.JSONHelper.JsonToModel<WeixinFollowers>(bllWeixin.GetFollower(bllWeixin.GetAccessToken(), string.Empty));

            }
            return true;
        }

        public enum UserScoreType
        {
            //加分
            SubscriteWXMP = 1,      //关注微信公众号加积分
            RegistWebSite =2,       //注册站点

            TutorAnswerQuestionToHim = 10,      //导师回答提问给他的问题
            TutorAnswerQuestionToOthers = 11,   //导师回答提问给别人的问题
            TutorQuestioinIsAnswered = 12,      //导师提的问题被回复
            UserQuestionIsAnswered = 20,        //用户提的问题被回复

            ShareArticleToWXFriend = 30,          //分享文章到朋友，微信群
            ShareArticleToWXFriendGroup = 31,     //分享文章到朋友圈
            ShareActivityToWXFriend = 40,         //分享活动到朋友，微信群
            ShareActivityToWXFriendGroup = 41,    //分享活动到朋友圈
            InviteFriendRegister    = 50,         //邀请好友注册
            SharePositionToWXFriend = 60,            //分享职位到朋友，微信群
            SharePositionToWXFriendGroup = 61,       //分享职位到朋友圈
            ShareTutorToWXFriend = 70,          //分享导师到朋友，微信群
            ShareTutorToWXFriendGroup = 71,     //分享导师到朋友圈

            TutorShareArticleToWXFriend = 80,          //导师分享文章到朋友，微信群
            TutorShareArticleToWXFriendGroup = 81,     //导师分享文章到朋友圈
            TutorShareActivityToWXFriend = 90,         //导师分享活动到朋友，微信群
            TutorShareActivityToWXFriendGroup = 91,    //导师分享活动到朋友圈
            TutorInviteFriendRegister    = 100,        //导师邀请好友注册
            TutorSharePositionToWXFriend = 110,            //导师分享职位到朋友，微信群
            TutorSharePositionToWXFriendGroup = 120,       //导师分享职位到朋友圈
            TutorShareTutorToWXFriend = 130,          //导师分享导师到朋友，微信群
            TutorShareTutorToWXFriendGroup = 140,     //导师分享导师到朋友圈

            //减分
            ExchangeGoodInScoreMall = 500           //积分商城兑换商品
        }

        public UserScore GetDefinedUserScore (UserScoreType type)
        {
            UserScore userScore = new UserScore();
            userScore.UserScoreType = type;
            switch (type)
            {
                case UserScoreType.SubscriteWXMP:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 50;
                    userScore.RecordTypeString = "关注公众号加分";
                    break;
                case UserScoreType.RegistWebSite:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 25;
                    userScore.RecordTypeString = "注册账号加分";
                    break;

                case UserScoreType.TutorAnswerQuestionToHim:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 20;
                    userScore.RecordTypeString = "导师回复提给他的问题加分";
                    break;
                case UserScoreType.TutorAnswerQuestionToOthers:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 20;
                    userScore.RecordTypeString = "导师回答其他导师的问题加分";
                    break;
                case UserScoreType.TutorQuestioinIsAnswered:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 20;
                    userScore.RecordTypeString = "导师提的问题被回复加分";
                    break;
                case UserScoreType.UserQuestionIsAnswered:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 20;
                    userScore.RecordTypeString = "您的问题被导师回答加分";
                    break;

                    //用户分享行为
                case UserScoreType.InviteFriendRegister:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 50;
                    userScore.RecordTypeString = "邀请好友注册成功";
                    break;
                case UserScoreType.ShareArticleToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享文章给好友";
                    break;
                case UserScoreType.ShareActivityToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享活动给好友";
                    break;
                case UserScoreType.SharePositionToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享职位给好友";
                    break;
                case UserScoreType.ShareTutorToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享导师给好友";
                    break;
                case UserScoreType.ShareArticleToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享文章到朋友圈";
                    break;
                case UserScoreType.ShareActivityToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享活动到朋友圈";
                    break;
                case UserScoreType.SharePositionToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享职位到朋友圈";
                    break;
                case UserScoreType.ShareTutorToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 1;
                    userScore.RecordTypeString = "分享导师到朋友圈";
                    break;
                    //
                    //
                    //
                    //导师分享行为
                case UserScoreType.TutorInviteFriendRegister:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 50;
                    userScore.RecordTypeString = "导师邀请好友注册成功";
                    break;
                case UserScoreType.TutorShareArticleToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享文章给好友";
                    break;
                case UserScoreType.TutorShareActivityToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享活动给好友";
                    break;
                case UserScoreType.TutorSharePositionToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score =10;
                    userScore.RecordTypeString = "导师分享职位给好友";
                    break;
                case UserScoreType.TutorShareTutorToWXFriend:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score =10;
                    userScore.RecordTypeString = "导师分享导师给好友";
                    break;
                case UserScoreType.TutorShareArticleToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享文章到朋友圈";
                    break;
                case UserScoreType.TutorShareActivityToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享活动到朋友圈";
                    break;
                case UserScoreType.TutorSharePositionToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享职位到朋友圈";
                    break;
                case UserScoreType.TutorShareTutorToWXFriendGroup:
                    userScore.nums = "34";
                    userScore.RecordType = "2";
                    userScore.Score = 10;
                    userScore.RecordTypeString = "导师分享导师到朋友圈";
                    break;

                case UserScoreType.ExchangeGoodInScoreMall:
                    userScore.nums = "34";
                    userScore.RecordType = "1";
                    userScore.Score = 0;
                    userScore.RecordTypeString = "积分商城兑换商品";
                    break;
            }
            return userScore;
        }

        public struct UserScore
        {
            public UserScoreType UserScoreType { get; set; }
            public string nums {get; set;}
            public string RecordType {get; set;}
            public double Score {get; set;}
            public string RecordTypeString { get; set; }
        }

        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="totalScore"></param>
        /// <returns></returns>
        public int GetUserLevelByTotalScore(double totalScore) {
            int level =1;
            foreach (var item in GetList<UserLevelConfig>(string.Format("WebSiteOwner='{0}' Order by LevelNumber ASC", WebsiteOwner)))
            {
                if ((totalScore>=item.FromHistoryScore)&&((totalScore<=item.ToHistoryScore)))
                {
                    return item.LevelNumber;
                }
            }
            return level;
        }

    }
 
}

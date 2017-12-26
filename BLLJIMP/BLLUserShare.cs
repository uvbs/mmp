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
    /// 用户分享 BLL
    /// </summary>
    public class BLLUserShare : BLL
    {

        //记录用户分享
        //userinfo：记录当前用户的分享
        //输入 id:分享id，wxsharetype：1朋好圈，0朋友，微信群
        //
        //返回0：记录成功，返回-1:已经分享过不记录，返回-2：操作失败
        //-3： 未定义的分享类型
        public ReturnValue RecordUserShare(UserInfo userInfo, ShareType type, string id, string wxsharetype, string websiteOwner, bool isTMNotify = false)
        {
            ReturnValue returnValue = new ReturnValue();
            BLLUserScore.UserScoreType userScoreType;
            switch (type)
            {
                    //分享文章
                case ShareType.ShareArticleToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.ShareArticleToWXFriend;
                    break;
                case ShareType.ShareArticleToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.ShareArticleToWXFriendGroup;
                    break;
                case ShareType.TutorShareArticleToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareArticleToWXFriend;
                    break;
                case ShareType.TutorShareArticleToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareArticleToWXFriendGroup;
                    break;

                    //分享活动
                case ShareType.ShareActivityToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.ShareActivityToWXFriend;
                    break;
                case ShareType.ShareActivityToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.ShareActivityToWXFriendGroup;
                    break;
                case ShareType.TutorShareActivityToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareActivityToWXFriend;
                    break;
                case ShareType.TutorShareActivityToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareActivityToWXFriendGroup;
                    break;

                //分享职位
                case ShareType.SharePositionToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.SharePositionToWXFriend;
                    break;
                case ShareType.SharePositionToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.SharePositionToWXFriendGroup;
                    break;
                case ShareType.TutorSharePositionToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.TutorSharePositionToWXFriend;
                    break;
                case ShareType.TutorSharePositionToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.TutorSharePositionToWXFriendGroup;
                    break;

                //分享导师
                case ShareType.ShareTutorToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.ShareTutorToWXFriend;
                    break;
                case ShareType.ShareTutorToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.ShareTutorToWXFriendGroup;
                    break;
                case ShareType.TutorShareTutorToWXFriend:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareTutorToWXFriend;
                    break;
                case ShareType.TutorShareTutorToWXFriendGroup:
                    userScoreType = BLLUserScore.UserScoreType.TutorShareTutorToWXFriend;
                    break;
                default:
                    return new ReturnValue() { Code = -2, Msg = "未定义的分享类型！" };
            }

            //检查是已经分享过
            if (this.GetCount<WBHShareRecord>(string.Format("UserId='{0}' And ShareId='{1}' And Type='{2}' And WeixinShareType={3}",
                                userInfo.UserID, id, GetShareTypeString(type), wxsharetype)) > 0)
            {
                returnValue.Code = -1;
                returnValue.Msg = "已经分享过";
                return returnValue;
            }

            //更新分享积分
            BLLUserScore userScore = new BLLUserScore(userInfo.UserID);
            if (isTMNotify)
            {
                userScore.UpdateUserScoreWithWXTMNotify(userScore.GetDefinedUserScore(userScoreType), new ZentCloud.BLLJIMP.BLLWeixin("").GetAccessToken());
            }
            else
            {
                userScore.UpdateUserScore(userScore.GetDefinedUserScore(userScoreType));
            }

            //记录分享记录
            WBHShareRecord record = new WBHShareRecord();
            record.ShareId = int.Parse(id);
            record.Type = GetShareTypeString(type);
            record.UserId = userInfo.UserID;
            record.WeiXinShareType = int.Parse(wxsharetype);
            record.InsertDate = DateTime.Now;
            bool bo = this.Add(record);
            if (bo)
            {
                returnValue.Code = 1;
                returnValue.Msg = "分享成功";
            }
            else
            {
                returnValue.Code = 0;
                returnValue.Msg = "分享失败";
            }
            return returnValue;
        }

        public string GetShareTypeString(ShareType type)
        {
            switch (type)
            {
                case ShareType.ShareArticleToWXFriend:
                case ShareType.ShareArticleToWXFriendGroup:
                case ShareType.TutorShareArticleToWXFriend:
                case ShareType.TutorShareArticleToWXFriendGroup:
                    return "分享文章";

                case ShareType.ShareActivityToWXFriend:
                case ShareType.ShareActivityToWXFriendGroup:
                case ShareType.TutorShareActivityToWXFriend:
                case ShareType.TutorShareActivityToWXFriendGroup:
                    return "分享活动";

                case ShareType.SharePositionToWXFriend:
                case ShareType.SharePositionToWXFriendGroup:
                case ShareType.TutorSharePositionToWXFriend:
                case ShareType.TutorSharePositionToWXFriendGroup:
                    return "分享职位";

                case ShareType.ShareTutorToWXFriend:
                case ShareType.ShareTutorToWXFriendGroup:
                case ShareType.TutorShareTutorToWXFriend:
                case ShareType.TutorShareTutorToWXFriendGroup:
                    return "分享导师";

                default:
                    return "分享未定义";
            }
        }

        public enum ShareType
        {
            //微信分享
            ShareArticleToWXFriend = 1,             //分享文章给朋友，微信群
            ShareArticleToWXFriendGroup = 2,        //分享文章到朋友圈
            TutorShareArticleToWXFriend = 3,             //导师分享文章给朋友，微信群
            TutorShareArticleToWXFriendGroup = 4,        //导师分享文章到朋友圈

            ShareActivityToWXFriend = 11,           //分享活动给朋友，微信群
            ShareActivityToWXFriendGroup = 12,       //分享活动到朋友圈
            TutorShareActivityToWXFriend = 13,           //导师分享活动给朋友，微信群
            TutorShareActivityToWXFriendGroup = 14,       //导师分享活动到朋友圈

            SharePositionToWXFriend = 21,           //分享职位给朋友，微信群
            SharePositionToWXFriendGroup = 22,       //分享职位到朋友圈
            TutorSharePositionToWXFriend =23,           //导师分享职位给朋友，微信群
            TutorSharePositionToWXFriendGroup = 24,       //导师分享职位到朋友圈

            ShareTutorToWXFriend = 31,                //分享导师给朋友，微信群
            ShareTutorToWXFriendGroup = 32,           //分享导师到朋友圈
            TutorShareTutorToWXFriend = 33,           //导师分享导师给朋友，微信群
            TutorShareTutorToWXFriendGroup = 34       //导师分享导师到朋友圈
        }
    }

}
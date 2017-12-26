using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Comments
{
    /// <summary>
    /// List 的摘要说明   评论列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLJIMP.BLLReview bllReview = new BLLJIMP.BLLReview();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            var articleId = context.Request["article_id"];
            var userAutoId = context.Request["user_autoid"];
            var reviewType = context.Request["review_type"];
            var totalCount = 0;
            BLLJIMP.Enums.ReviewTypeKey nType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (string.IsNullOrWhiteSpace(reviewType) && !string.IsNullOrWhiteSpace(articleId))
            {
                int artId = Convert.ToInt32(articleId);
                JuActivityInfo juArtcle = bllJuActivity.GetJuActivity(artId);
                if (juArtcle.ArticleType.ToLower() == "question")
                {
                    nType = BLLJIMP.Enums.ReviewTypeKey.Answer;
                }
                if (juArtcle.ArticleType.ToLower() == "appointment")
                {
                    nType = BLLJIMP.Enums.ReviewTypeKey.AppointmentComment;
                }
            }
            else
            {
                Enum.TryParse(reviewType, out nType);
            }

            string UserId = "";
            if (!string.IsNullOrWhiteSpace(userAutoId))
            {
                UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                if (user != null)
                {
                    UserId = user.UserID;
                }
                else
                {
                    UserId = "-1";
                }
            }

            var sourceData = this.bllReview.GetReviewList(nType, out totalCount, pageIndex, pageSize, articleId, this.bllJuActivity.WebsiteOwner, this.CurrentUserInfo == null ? "" : this.CurrentUserInfo.UserID, "", UserId);

            List<dynamic> returnList = new List<dynamic>();

            resp.isSuccess = true;

            foreach (var item in sourceData)
            {
                int actId = 0;
                int.TryParse(item.Expand1, out actId);
                JuActivityInfo actInfo = bllJuActivity.GetJuActivity(actId);
                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    create_time =bllUser.GetTimeStamp(item.InsertDate),
                    reply_count = item.ReplyCount,//回复数
                    praise_count = item.PraiseCount,//点赞数
                    pv = item.Pv,//浏览数
                    curruser_ispraise = item.CurrUserIsPraise,
                    article_id = actInfo != null ? actInfo.JuActivityID : 0,
                    article_name = actInfo != null ? actInfo.ActivityName : "",
                    pub_user = new
                    {
                        id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                        user_id = item.PubUser == null ? "" : item.PubUser.UserID,
                        user_name = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                        avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                        istutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser)
                    },
                    replay_touser = new
                    {
                        id = item.ReplayToUser == null ? 0 : item.ReplayToUser.AutoID,
                        user_id = item.ReplayToUser == null ? "" : item.ReplayToUser.UserID,
                        user_name = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyName(item.ReplayToUser),
                        avatar = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyAvatar(item.ReplayToUser),
                        istutor = item.ReplayToUser == null ? false : bllUser.IsTutor(item.ReplayToUser)
                    }
                });
            }
            resp.returnObj = new 
            {
                totalcount=totalCount,
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// GetCommentList 的摘要说明
    /// </summary>
    public class GetCommentList : BaseHandlerNoAction
    {
        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;

        BLLJIMP.BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);
            var articleId = context.Request["articleid"];
            var userAutoId = context.Request["user_autoid"];
            var reviewType = context.Request["review_type"];

            currentUserInfo = bllUser.GetCurrentUserInfo();

            var totalCount = 0;

            BLLJIMP.Enums.ReviewTypeKey nType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (string.IsNullOrWhiteSpace(reviewType) && !string.IsNullOrWhiteSpace(articleId))
            {
                int artId = Convert.ToInt32(articleId);
                JuActivityInfo juArtcle = bll.GetJuActivity(artId);
                if (juArtcle.ArticleType.ToLower() == "question")
                {
                    nType = BLLJIMP.Enums.ReviewTypeKey.Answer;
                }
            }
            else
            {
                Enum.TryParse(reviewType, out nType);
            }
            string userId = "";
            if (!string.IsNullOrWhiteSpace(userAutoId))
            {
                UserInfo user = bllUser.GetUserInfoByAutoID(int.Parse(userAutoId));
                if (user != null)
                {
                    userId = user.UserID;
                }
                else
                {
                    userId = "-1";
                }

            }
            //仅显示审核通过的
            var sourceData = this.bllReview.GetReviewList(nType, out totalCount, pageIndex, pageSize, articleId, this.bll.WebsiteOwner, this.currentUserInfo == null ? "" : this.currentUserInfo.UserID, "", userId);

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                int actId = 0;
                int.TryParse(item.Expand1, out actId);
                JuActivityInfo actInfo = bll.GetJuActivity(actId);
                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    createDate = item.InsertDate.ToString(),
                    replyCount = item.ReplyCount,//回复数
                    praiseCount = item.PraiseCount,//点赞数
                    pv = item.Pv,//浏览数
                    currUserIsPraise = item.CurrUserIsPraise,
                    articleId = actInfo != null ? actInfo.JuActivityID : 0,
                    articleName = actInfo != null ? actInfo.ActivityName : "",
                    pubUser = new
                    {
                        id = item.PubUser == null ? 0 : item.PubUser.AutoID,
                        userId = item.PubUser == null ? "" : item.PubUser.UserID,
                        userName = item.PubUser == null ? "" : bllUser.GetUserDispalyName(item.PubUser),
                        avatar = item.PubUser == null ? "" : bllUser.GetUserDispalyAvatar(item.PubUser),
                        isTutor = item.PubUser == null ? false : bllUser.IsTutor(item.PubUser)
                    },
                    replayToUser = new
                    {
                        id = item.ReplayToUser == null ? 0 : item.ReplayToUser.AutoID,
                        userId = item.ReplayToUser == null ? "" : item.ReplayToUser.UserID,
                        userName = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyName(item.ReplayToUser),
                        avatar = item.ReplayToUser == null ? "" : bllUser.GetUserDispalyAvatar(item.ReplayToUser),
                        isTutor = item.ReplayToUser == null ? false : bllUser.IsTutor(item.ReplayToUser)
                    }
                });
            }

            dynamic result = new
            {
                totalcount = totalCount,
                list = returnList
            };
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.result = result;
            bllUser.ContextResponse(context,apiResp);
        }
    }
}
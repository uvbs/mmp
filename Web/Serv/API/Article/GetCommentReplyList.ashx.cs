using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// GetCommentReplyList 的摘要说明
    /// </summary>
    public class GetCommentReplyList : BaseHandlerNoAction
    {

        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// <summary>
        /// 用户信息
        /// </summary>
        UserInfo curUser = new UserInfo();

        BLLJIMP.BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["pageindex"]),
                pageSize = Convert.ToInt32(context.Request["pagesize"]);
            var commentId = context.Request["commentid"];

            var totalCount = 0;

            curUser = bllUser.GetCurrentUserInfo();

            //仅显示审核通过的
            var sourceData = this.bllReview.GetReviewList(BLLJIMP.Enums.ReviewTypeKey.CommentReply, out totalCount, pageIndex, pageSize, commentId, bllReview.WebsiteOwner, this.curUser == null ? "" : curUser.UserID, " AutoId ASC ");

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(new
                {
                    id = item.ReviewMainId,
                    content = item.ReviewContent,
                    createDate = item.InsertDate.ToString(),
                    replyCount = item.ReplyCount,//回复数
                    praiseCount = item.PraiseCount,//点赞数
                    currUserIsPraise = item.CurrUserIsPraise,
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
            bllUser.ContextResponse(context, apiResp);
        }
    }
}
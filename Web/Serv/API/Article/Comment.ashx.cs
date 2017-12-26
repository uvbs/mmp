using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// Comment 的摘要说明
    /// </summary>
    public class Comment : BaseHandlerNoAction
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
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 消息中心模块
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            var articleId = context.Request["articleid"];
            var content = context.Request["content"];
            var replyId = Convert.ToInt32(context.Request["replyid"]);//评论了文章里的哪个评论

            int isHideUserName = Convert.ToInt32(context.Request["ishideusername"]);

            currentUserInfo = bllUser.GetCurrentUserInfo();
            if (this.currentUserInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                apiResp.msg = "请先登录";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (string.IsNullOrWhiteSpace(articleId) || string.IsNullOrWhiteSpace(content))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (bllUser.GetCount<JuActivityInfo>(string.Format(" JuActivityID = {0} ", articleId)) == 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            //敏感词检查
            BLLFilterWord bllFilterWord = new BLLFilterWord();
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bllUser.WebsiteOwner, out errmsg, "0"))
            {
                apiResp.msg = errmsg;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            //添加评论
            int reviewId = 0;
            JuActivityInfo article = bll.GetJuActivity(int.Parse(articleId), true);
            BLLJIMP.Enums.ReviewTypeKey reviewType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (article.ArticleType == "Question") reviewType = BLLJIMP.Enums.ReviewTypeKey.Answer;
            var addResult = bllReview.AddReview(reviewType, articleId, replyId, this.currentUserInfo.UserID, "评论", content, this.bll.WebsiteOwner, out reviewId, isHideUserName);

            if (addResult)
            {
                if (reviewType == BLLJIMP.Enums.ReviewTypeKey.Answer)
                {
                    bllUser.AddUserScoreDetail(this.currentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.AnswerQuestions), this.bll.WebsiteOwner, null, null);
                }

                if (article.ArticleType == "Question")
                {
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.QuestionIsAnswered, this.currentUserInfo, article, article.UserID, content);

                    List<UserInfo> users = bllUser.GetRelationUserList(BLLJIMP.Enums.CommRelationType.JuActivityFollow, articleId);
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FollowQuestionIsAnswered, this.currentUserInfo, article, users, content);
                }
                apiResp.status = true;
                apiResp.msg = "评论完成";
                apiResp.result = reviewId.ToString();
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "评论出错";
            }
            bllReview.ContextResponse(context, apiResp);
        }
    }
}
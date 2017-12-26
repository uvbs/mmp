using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Comments
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();

        BLLFilterWord bllFilterWord = new BLLFilterWord();

        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 消息中心模块
        /// </summary>
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        public void ProcessRequest(HttpContext context)
        {
            string articleId = context.Request["activity_id"];
            string content = context.Request["content"];
            var replyId = Convert.ToInt32(context.Request["reply_id"]);//评论了文章里的哪个评论
            int isHideUserName = Convert.ToInt32(context.Request["is_hide_user_name"]);
            resp.isSuccess = false;
            if (string.IsNullOrWhiteSpace(articleId) || string.IsNullOrWhiteSpace(content))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "activity_id、content 为必填项,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllJuActivity.GetCount<JuActivityInfo>(string.Format(" WebsiteOwner='{0}' AND  JuActivityID = {1} ",bllJuActivity.WebsiteOwner,articleId)) == 0)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "不存在活动";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //敏感词检查
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bllJuActivity.WebsiteOwner, out errmsg, "0"))
            {
                resp.errmsg = errmsg;
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //添加评论
            int reviewId = 0;
            JuActivityInfo article = bllJuActivity.GetJuActivity(int.Parse(articleId), true);
            BLLJIMP.Enums.ReviewTypeKey reviewType = BLLJIMP.Enums.ReviewTypeKey.ArticleComment;
            if (article.ArticleType == "Question") reviewType = BLLJIMP.Enums.ReviewTypeKey.Answer;
            if (article.ArticleType == "Appointment") reviewType = BLLJIMP.Enums.ReviewTypeKey.AppointmentComment;
            var addResult = bllReview.AddReview(reviewType, articleId, replyId, this.CurrentUserInfo.UserID, "评论", content, this.bllJuActivity.WebsiteOwner, out reviewId, isHideUserName);
            if (addResult)
            {
                if (reviewType == BLLJIMP.Enums.ReviewTypeKey.Answer)
                {
                    bllUser.AddUserScoreDetail(this.CurrentUserInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.AnswerQuestions), this.bllJuActivity.WebsiteOwner, null, null);
                }
                resp.isSuccess = true;
                resp.returnValue = reviewId.ToString();

                if (article.ArticleType == "Question")
                {
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.QuestionIsAnswered, this.CurrentUserInfo, article, article.UserID, content);
                    List<UserInfo> users = bllUser.GetRelationUserList(BLLJIMP.Enums.CommRelationType.JuActivityFollow, articleId);
                    bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.FollowQuestionIsAnswered, this.CurrentUserInfo, article, users, content);
                }
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.isSuccess = false;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}
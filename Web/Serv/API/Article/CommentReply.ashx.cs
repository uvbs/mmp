using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// CommentReply 的摘要说明
    /// </summary>
    public class CommentReply : BaseHandlerNoAction
    {

        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 专家业务逻辑
        /// </summary>
        BLLTutor bllTutor = new BLLTutor();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            var commentId = context.Request["commentid"];
            var replyId = Convert.ToInt32(context.Request["replyid"]);//回复了评论里的哪个回复
            var content = context.Request["content"];
            int isHideUserName = Convert.ToInt32(context.Request["isHideUserName"]);

            currentUserInfo = bllUser.GetCurrentUserInfo();
            if (this.currentUserInfo == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                apiResp.msg = "请先登录";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (string.IsNullOrWhiteSpace(commentId) || string.IsNullOrWhiteSpace(content))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                bllReview.ContextResponse(context, apiResp);
                return;
            }

          
            if (bllReview.GetCount<ReviewInfo>(string.Format(" ReviewMainId = {0} ", commentId)) == 0)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                bllReview.ContextResponse(context, apiResp);
                return;
            }

            //敏感词检查
            BLLFilterWord bllFilterWord = new BLLFilterWord();
            string errmsg = "";
            if (!bllFilterWord.CheckFilterWord(content, this.bllReview.WebsiteOwner, out errmsg, "0"))
            {
                apiResp.msg = errmsg;
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bllReview.ContextResponse(context, apiResp);
                return;
            }

            //添加回复

            int reviewId = 0;

            var addResult = bllReview.AddReview(BLLJIMP.Enums.ReviewTypeKey.CommentReply, commentId, replyId, this.currentUserInfo.UserID, "评论", content, this.bllReview.WebsiteOwner, out reviewId, isHideUserName);

            if (addResult)
            {
                apiResp.status = true;
                apiResp.result = reviewId.ToString();

                if (bllUser.IsTutor(this.currentUserInfo.UserID))
                {
                    bllTutor.UpdateAnswers(this.currentUserInfo.UserID);
                }
            }
            else
            {
                apiResp.status = false;
            }
            bllReview.ContextResponse(context, apiResp);
        }

    }
}
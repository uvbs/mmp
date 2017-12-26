using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Question
{
    /// <summary>
    /// QuestionPostHandler 的摘要说明
    /// </summary>
    public class QuestionPostHandler : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        BLLQuestion bllQuestion = new BLLQuestion();
        UserInfo currentUserInfo = new UserInfo();
        string websiteOwner;
        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;


            currentUserInfo = bllQuestion.GetCurrentUserInfo();
            if (currentUserInfo == null)
            {
                resp.Msg = "您还没有登录！";
                result = JSONHelper.ObjectToJson(resp);
                context.Response.Write(result);
                return;
            }
            websiteOwner = bllQuestion.WebsiteOwner;

            result = PostQuestion(context);
            context.Response.Write(result);
        }
        private string PostQuestion(HttpContext context)
        {
            string postData = context.Request["PostData"];
            string msg = "";
            int correctCount = 0;
            string userHostAddress = context.Request.UserHostAddress;
            List<QuestionPostModel> postList = JSONHelper.JsonToModel<List<QuestionPostModel>>(postData);//jSON 反序列化
            bool result = bllQuestion.PostQuestion(postList, 0, currentUserInfo.UserID, websiteOwner
                ,userHostAddress, out correctCount, out msg,0);

            resp.IsSuccess = result;
            resp.Result = correctCount;
            resp.Msg = msg;

            return JSONHelper.ObjectToJson(resp);
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
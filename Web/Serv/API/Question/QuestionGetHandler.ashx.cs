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
    /// QuestionGetHandler 的摘要说明
    /// </summary>
    public class QuestionGetHandler : IHttpHandler, IReadOnlySessionState
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

            result = GetQuestionList(context);
            context.Response.Write(result);
        }
        private string GetQuestionList(HttpContext context)
        {
            List<BLLJIMP.Model.Question> questions = null;
            string msg = "";

            bool result = bllQuestion.GetQuestionList(context.Request["Questionnaires"], context.Request["ReturnAll"]
                , context.Request["IsRanSortQuestion"], context.Request["IsRanSortAnswer"], context.Request["UsedID"]
                ,null, out questions, out msg);
            
            var resultList = from p in questions
                             select new
                        {
                            p.QuestionID,
                            p.QuestionName,
                            p.QuestionType,
                            p.IsRequired,
                            Answers = (from s in p.Answers
                                       select new
                                       {
                                           s.AnswerID,
                                           s.AnswerName
                                       })
                        };

            resp.Result = resultList;
            resp.IsSuccess = result;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Question
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            List<BLLJIMP.Model.Question> questions = null;
            string msg = "";

            bool result = bllQuestion.GetQuestionList(context.Request["question_collection"], context.Request["full_return"]
                , context.Request["question_random"], context.Request["option_random"], context.Request["used_questions"]
                , null, out questions, out msg);

            if (result)
            {
                var resultList = from p in questions
                                 select new
                                 {
                                     question_id = p.QuestionID,
                                     question_name = p.QuestionName,
                                     question_type = p.QuestionType,
                                     question_required = p.IsRequired,
                                     question_options = (from s in p.Answers
                                                         select new
                                                         {
                                                             option_id = s.AnswerID,
                                                             option_name = s.AnswerName
                                                         })
                                 };

                apiResp.result = resultList;
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            apiResp.status = result;
            apiResp.msg = msg;
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}
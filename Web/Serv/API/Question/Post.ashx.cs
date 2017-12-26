using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Question
{
    /// <summary>
    /// Post 的摘要说明
    /// </summary>
    public class Post : BaseHandlerNeedLoginNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = JsonConvert.DeserializeObject<RequestModel>(context.Request["data"]);
            }
            catch (Exception ex)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
                bllQuestion.ContextResponse(context, resp);
                return;
            }
            string msg = "";
            bool result = false;

            QuestionnaireSet nSet = bllQuestion.GetByKey<QuestionnaireSet>("AutoID", requestModel.set_id.ToString());
            result = bllQuestion.CheckQuestionnaireSet(nSet,out msg);
            if (!result)
            {
                apiResp.status = result;
                apiResp.msg = msg;
                bllQuestion.ContextResponse(context, apiResp);
                return;
            }
            int correctCount = 0;
            string userHostAddress = context.Request.UserHostAddress;

            List<QuestionPostModel> postList = (from p in requestModel.answer_list
                                                select new QuestionPostModel
                                                {
                                                    QuestionID = p.question_id,
                                                    Answer = p.option_ids
                                                }).ToList();

            int questionnaireId = nSet.QuestionnaireId.HasValue?nSet.QuestionnaireId.Value:0;
            result = bllQuestion.PostQuestion(postList, requestModel.set_id, CurrentUserInfo.UserID, bllQuestion.WebsiteOwner
                , userHostAddress, out correctCount, out msg, questionnaireId);

            bool have_score = correctCount >= nSet.WinCount;
            if (have_score && nSet.Score > 0 && nSet.ScoreNum > 0)
            {
                string msgscore = "";
                bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, "QuestionnaireSet", bllQuestion.WebsiteOwner, out msgscore, nSet.Score, "答题获得积分", null, false, nSet.AutoID.ToString());
            }
            dynamic resultObj = new
            {
                have_score = have_score,
                correct_count = correctCount
            };

            apiResp.code = (int)(result ? APIErrCode.IsSuccess : APIErrCode.OperateFail);
            apiResp.status = result;
            apiResp.result = resultObj;
            apiResp.msg = msg;
            bllQuestion.ContextResponse(context, apiResp);
        }

        public class RequestModel
        {
            /// <summary>
            /// 答题设置id
            /// </summary>
            public int set_id { get; set; }
            /// <summary>
            /// 回答
            /// </summary>
            public List<AnswerModel> answer_list { get; set; }
        }


        public class AnswerModel
        {
            /// <summary>
            /// 问题ID
            /// </summary>
            public int question_id { get; set; }
            /// <summary>
            /// 选项ID，多选时用|分隔
            /// 填空题内容
            /// </summary>
            public string option_ids { get; set; }
        }
    }
}
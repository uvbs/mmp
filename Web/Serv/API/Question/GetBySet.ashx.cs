using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Question
{
    /// <summary>
    /// GetBySet 的摘要说明
    /// </summary>
    public class GetBySet : BaseHandlerNeedLoginNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            List<BLLJIMP.Model.Question> questions = null;
            string msg = "";
            bool result = bllQuestion.GetQuestionListBySet(Convert.ToInt32(context.Request["id"]), out questions, out msg);

            if (result)
            {
                BLLJIMP.Model.QuestionnaireSet QuestionnaireSetModel = bllQuestion.GetByKey<BLLJIMP.Model.QuestionnaireSet>("AutoID", context.Request["id"]);

                bool isUserAnswer = bllQuestion.ExistsRecordCount(CurrentUserInfo.UserID,QuestionnaireSetModel.AutoID,null);
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

                apiResp.result = new
                {
                    id = QuestionnaireSetModel.AutoID,
                    title = QuestionnaireSetModel.Title,
                    img = QuestionnaireSetModel.Img,
                    describe = QuestionnaireSetModel.Description,
                    bgimg_index = QuestionnaireSetModel.BgImgIndex,
                    bgimg_answer = QuestionnaireSetModel.BgImgAnswer,
                    bgimg_end = QuestionnaireSetModel.BgImgEnd,
                    is_useranswer = isUserAnswer,
                    is_moreanswer = QuestionnaireSetModel.IsMoreAnswer,
                    win_count = QuestionnaireSetModel.WinCount,
                    win_btn_text = QuestionnaireSetModel.WinBtnText,
                    win_btn_url = QuestionnaireSetModel.WinBtnUrl,
                    win_describe = QuestionnaireSetModel.WinDescription,
                    questions = resultList
                };

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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
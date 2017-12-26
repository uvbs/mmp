using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// 获取调查问卷详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            Questionnaire QuestionnaireModel = bllQuestion.GetByKey<Questionnaire>("QuestionnaireID", id);
            if (QuestionnaireModel == null)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "问卷没有找到";
                bllQuestion.ContextResponse(context, resp);
                return;
            }

            ResponseModel responseModel = new ResponseModel();
            responseModel.id = QuestionnaireModel.QuestionnaireID;
            responseModel.questionnaire_name = QuestionnaireModel.QuestionnaireName;
            responseModel.questionnaire_summary = QuestionnaireModel.QuestionnaireSummary;
            responseModel.questionnaire_content = QuestionnaireModel.QuestionnaireContent;
            responseModel.questionnaire_image = QuestionnaireModel.QuestionnaireImage;
            responseModel.questionnaire_visible = QuestionnaireModel.QuestionnaireVisible;
            responseModel.questionnaire_stopdate = DateTimeHelper.DateTimeToStr(QuestionnaireModel.QuestionnaireStopDate);
            responseModel.add_score = QuestionnaireModel.AddScore;

            List<ZentCloud.BLLJIMP.Model.Question> OldQuestionList = bllQuestion.GetList<ZentCloud.BLLJIMP.Model.Question>(int.MaxValue, string.Format("QuestionnaireID={0}", id), "Sort Asc");//旧问题

            responseModel.question_list = (from p in OldQuestionList
                                           select new ResponseQuestionModel
                                           {
                                               id = p.QuestionID,
                                               question_name = p.QuestionName,
                                               question_type = p.QuestionType,
                                               is_required = p.IsRequired
                                           }).ToList();
            for (int i = 0; i < responseModel.question_list.Count; i++)
            {
                List<Answer> OldAnswerList = bllQuestion.GetListByKey<Answer>("QuestionID", responseModel.question_list[i].id.ToString());//选项
                responseModel.question_list[i].answer_list = (from p in OldAnswerList
                                                              select new ResponseAnswerModel
                                                             {
                                                                 id = p.AnswerID,
                                                                 answer_name = p.AnswerName,
                                                                 is_correct = p.IsCorrect
                                                             }).ToList();

            }
            resp.isSuccess = true;
            resp.returnObj = responseModel;
            bllQuestion.ContextResponse(context, resp);
        }

        public class ResponseModel
        {
            /// <summary>
            /// 问卷id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 问卷名称
            /// </summary>
            public string questionnaire_name { get; set; }
            /// <summary>
            /// 问卷介绍及说明
            /// </summary>
            public string questionnaire_content { get; set; }
            /// <summary>
            /// 问卷停止日期
            /// </summary>
            public string questionnaire_stopdate { get; set; }
            /// <summary>
            /// 问卷是否可见 0不可见 1可见
            /// </summary>
            public int questionnaire_visible { get; set; }
            /// <summary>
            /// 问卷图片
            /// </summary>
            public string questionnaire_image { get; set; }
            /// <summary>
            /// 问卷描述用于分享时显示
            /// </summary>
            public string questionnaire_summary { get; set; }
            /// <summary>
            /// 问题列表
            /// </summary>
            public List<ResponseQuestionModel> question_list { get; set; }
            /// <summary>
            /// 赠送积分
            /// </summary>
            public int add_score { get; set; }
        }

        public class ResponseQuestionModel
        {
            /// <summary>
            /// 问题id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 问题名称
            /// </summary>
            public string question_name { get; set; }
            /// <summary>
            /// 问题类型 0单选1多选2填空
            /// </summary>
            public int question_type { get; set; }
            /// <summary>
            /// 是否必填 0否1必填
            /// </summary>
            public int is_required { get; set; }
            /// <summary>
            /// 选项
            /// </summary>
            public List<ResponseAnswerModel> answer_list { get; set; }
        }
        public class ResponseAnswerModel
        {
            /// <summary>
            /// 选项id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 选项名称
            /// </summary>
            public string answer_name { get; set; }
            /// <summary>
            /// 是否正确答案
            /// </summary>
            public int is_correct { get; set; }
        }
    }
}
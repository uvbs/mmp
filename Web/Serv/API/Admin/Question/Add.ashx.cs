using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// 添加调查问卷
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
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
            if (string.IsNullOrWhiteSpace(requestModel.questionnaire_name))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入问卷名称";
                bllQuestion.ContextResponse(context, resp);
                return;
            }
            if (requestModel.question_list == null || requestModel.question_list.Count <= 0)
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请至少添加一个问题";
                bllQuestion.ContextResponse(context, resp);
                return;
            }
            foreach (var item in requestModel.question_list)
            {
                if (item.question_type != 2 && (item.answer_list == null || item.answer_list.Count <= 0))
                {
                    resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "选择题至少需要一个选项";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
            }

            BLLTransaction tran = new BLLTransaction();//事务
            try
            {
                Questionnaire QuestionnaireModel = new Questionnaire();//数据库问卷表模型
                QuestionnaireModel.QuestionnaireID = int.Parse(bllQuestion.GetGUID(TransacType.AddQuestionnaire));
                QuestionnaireModel.QuestionnaireName = requestModel.questionnaire_name;
                QuestionnaireModel.QuestionnaireContent = requestModel.questionnaire_content;
                QuestionnaireModel.QuestionnaireStopDate = requestModel.questionnaire_stopdate;
                QuestionnaireModel.QuestionnaireVisible = requestModel.questionnaire_visible;
                QuestionnaireModel.QuestionnaireImage = requestModel.questionnaire_image;
                QuestionnaireModel.QuestionnaireSummary = requestModel.questionnaire_summary;
                QuestionnaireModel.WebsiteOwner = bllQuestion.WebsiteOwner;
                QuestionnaireModel.InsertDate = DateTime.Now;
                QuestionnaireModel.AddScore = requestModel.add_score;

                if (!bllQuestion.Add(QuestionnaireModel, tran))//添加问卷表
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "添加问卷失败";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
                foreach (var item in requestModel.question_list)//添加问题表
                {
                    ZentCloud.BLLJIMP.Model.Question question = new ZentCloud.BLLJIMP.Model.Question();
                    question.QuestionID = int.Parse(bllQuestion.GetGUID(TransacType.AddQuestion));
                    question.QuestionnaireID = QuestionnaireModel.QuestionnaireID;
                    question.QuestionName = item.question_name;
                    question.QuestionType = item.question_type;
                    question.IsRequired = item.is_required;
                    if (!bllQuestion.Add(question, tran))
                    {
                        tran.Rollback();
                        resp.errcode = (int)APIErrCode.OperateFail;
                        resp.errmsg = "添加问题失败";
                        bllQuestion.ContextResponse(context, resp);
                        return;
                    }
                    foreach (var AnswerItem in item.answer_list)
                    {
                        Answer answer = new Answer();
                        answer.AnswerID = int.Parse(bllQuestion.GetGUID(TransacType.AddAnswer));
                        answer.AnswerName = AnswerItem.answer_name;
                        answer.IsCorrect = AnswerItem.is_correct;
                        answer.QuestionID = question.QuestionID;
                        answer.QuestionnaireID = QuestionnaireModel.QuestionnaireID;
                        if (!bllQuestion.Add(answer, tran))
                        {
                            tran.Rollback();
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = "添加选项失败";
                            bllQuestion.ContextResponse(context, resp);
                            return;
                        }
                    }
                }


                tran.Commit();
                resp.isSuccess = true;
                resp.errcode = (int)APIErrCode.IsSuccess;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            bllQuestion.ContextResponse(context, resp);
        }

        public class RequestModel
        {
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
            public DateTime? questionnaire_stopdate { get; set; }
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
            public List<RequestQuestionModel> question_list { get; set; }
            /// <summary>
            /// 赠送积分
            /// </summary>
            public int add_score { get; set; }
        }

        public class RequestQuestionModel
        {
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
            public List<RequestAnswerModel> answer_list { get; set; }
        }
        public class RequestAnswerModel
        {
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
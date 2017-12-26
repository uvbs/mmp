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
    /// 更新调查问卷
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            //forceDelete等于1时，进行强制删除，会清除原有答题进行删除
            string forceDelete = context.Request["force_delete"];
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
            if (requestModel.id == 0)
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入Id";
                bllQuestion.ContextResponse(context, resp);
                return;
            }
            Questionnaire questionnaireModel = bllQuestion.GetByKey<Questionnaire>("QuestionnaireID", requestModel.id.ToString());
            if (questionnaireModel == null)
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "问卷没有找到";
                bllQuestion.ContextResponse(context, resp);
                return;
            }
            if (forceDelete != "1")
            {
                if (bllQuestion.GetCountByKey<QuestionnaireRecord>("QuestionnaireID", requestModel.id.ToString()) > 0)
                {
                    resp.errcode = (int)APIErrCode.LotteryHaveRecord;
                    resp.errmsg = string.Format("已经有人答题，不能进行修改");
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
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

            List<RequestQuestionModel> addRequestQuestionList = requestModel.question_list.Where(p => p.id == 0).ToList();
            List<RequestQuestionModel> updateRequestQuestionList = requestModel.question_list.Where(p => p.id > 0).ToList();

            List<ZentCloud.BLLJIMP.Model.Question> deleteQuestionList = new List<ZentCloud.BLLJIMP.Model.Question>();
            List<ZentCloud.BLLJIMP.Model.Question> AddQuestionList = new List<ZentCloud.BLLJIMP.Model.Question>();
            List<ZentCloud.BLLJIMP.Model.Question> UpdateQuestionList = new List<ZentCloud.BLLJIMP.Model.Question>();


            #region 参数对应 检查问题

            questionnaireModel.QuestionnaireName = requestModel.questionnaire_name;
            questionnaireModel.QuestionnaireContent = requestModel.questionnaire_content;
            questionnaireModel.QuestionnaireStopDate = requestModel.questionnaire_stopdate;
            questionnaireModel.QuestionnaireVisible = requestModel.questionnaire_visible;
            questionnaireModel.QuestionnaireImage = requestModel.questionnaire_image;
            questionnaireModel.QuestionnaireSummary = requestModel.questionnaire_summary;
            questionnaireModel.AddScore = requestModel.add_score;

            List<ZentCloud.BLLJIMP.Model.Question> OldQuestionList = bllQuestion.GetListByKey<ZentCloud.BLLJIMP.Model.Question>("QuestionnaireID", requestModel.id.ToString());//旧问题
            foreach (var item in OldQuestionList)
            {
                RequestQuestionModel nQuestion = updateRequestQuestionList.FirstOrDefault(p => p.id == item.QuestionID);
                if (nQuestion == null)//该问题被删除了
                {
                    deleteQuestionList.Add(item);
                }
                else
                {
                    item.QuestionName = nQuestion.question_name;
                    item.QuestionType = nQuestion.question_type;
                    item.IsRequired = nQuestion.is_required;

                    List<RequestAnswerModel> AddRequestAnswerList = nQuestion.answer_list.Where(p => p.id == 0).ToList();
                    List<RequestAnswerModel> UpdateRequestAnswerList = nQuestion.answer_list.Where(p => p.id > 0).ToList();
                    List<Answer> OldAnswerList = bllQuestion.GetListByKey<Answer>("QuestionID", item.QuestionID.ToString());//旧选项
                    if (item.QuestionType == 2 && OldAnswerList.Count > 0)
                    {
                        for (int i = 0; i < OldAnswerList.Count; i++)
                        {
                            OldAnswerList[i].PostType = -1;
                        }
                        item.Answers = OldAnswerList;
                    }
                    else
                    {
                        List<Answer> AnswerList = new List<Answer>();
                        foreach (var AnswerItem in OldAnswerList)
                        {
                            RequestAnswerModel nAnswer = UpdateRequestAnswerList.FirstOrDefault(p => p.id == AnswerItem.AnswerID);
                            if (nAnswer == null)//该问题被删除了
                            {
                                AnswerItem.PostType = -1;
                                AnswerList.Add(AnswerItem);
                            }
                            else
                            {
                                AnswerItem.AnswerName = nAnswer.answer_name;
                                AnswerItem.IsCorrect = nAnswer.is_correct;
                                AnswerItem.PostType = 2;
                                AnswerList.Add(AnswerItem);
                            }
                        }
                        foreach (var AnswerItem in AddRequestAnswerList)
                        {
                            Answer answer = new Answer();
                            answer.QuestionID = item.QuestionID;
                            answer.QuestionnaireID = questionnaireModel.QuestionnaireID;
                            answer.AnswerName = AnswerItem.answer_name;
                            answer.IsCorrect = AnswerItem.is_correct;
                            answer.PostType = 1;
                            AnswerList.Add(answer);
                        }
                        item.Answers = AnswerList;
                    }
                    UpdateQuestionList.Add(item);
                }
            }
            foreach (var item in addRequestQuestionList)
            {
                ZentCloud.BLLJIMP.Model.Question question = new ZentCloud.BLLJIMP.Model.Question();
                question.QuestionName = item.question_name;
                question.QuestionType = item.question_type;
                question.IsRequired = item.is_required;
                question.QuestionnaireID = questionnaireModel.QuestionnaireID;
                
                List<Answer> AnswerList = new List<Answer>();
                foreach (var AnswerItem in item.answer_list)
                {
                    Answer answer = new Answer();
                    answer.AnswerName = AnswerItem.answer_name;
                    answer.IsCorrect = AnswerItem.is_correct;
                    answer.QuestionnaireID = questionnaireModel.QuestionnaireID;
                    answer.PostType = 1;
                    AnswerList.Add(answer);
                }
                question.Answers = AnswerList;
                AddQuestionList.Add(question);
            }

            #endregion

            BLLTransaction tran = new BLLTransaction();//事务
            try
            {
                if (!bllQuestion.Update(questionnaireModel, tran))//修改问卷表
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "修改问卷失败";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
                if (bllQuestion.DeleteByKey<QuestionnaireRecord>("QuestionnaireID", questionnaireModel.QuestionnaireID.ToString(), tran) < 0)
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "清除旧答题记录失败";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
                if (bllQuestion.DeleteByKey<QuestionnaireRecordDetail>("QuestionnaireID", questionnaireModel.QuestionnaireID.ToString(), tran) < 0)
                {
                    tran.Rollback();
                    resp.errcode = (int)APIErrCode.OperateFail;
                    resp.errmsg = "清除旧答题记录详情失败";
                    bllQuestion.ContextResponse(context, resp);
                    return;
                }
                if (deleteQuestionList.Count > 0)
                {
                    foreach (var item in deleteQuestionList)
                    {
                        if (bllQuestion.Delete(item, tran) <= 0)
                        {
                            tran.Rollback();
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = "删除旧问题失败";
                            bllQuestion.ContextResponse(context, resp);
                            return;
                        }
                        if (bllQuestion.DeleteByKey<Answer>("QuestionID", item.QuestionID.ToString(), tran) < 0)
                        {
                            tran.Rollback();
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = "清除旧选项失败";
                            bllQuestion.ContextResponse(context, resp);
                            return;
                        }
                    }
                }
                if (UpdateQuestionList.Count > 0)
                {
                    foreach (var item in UpdateQuestionList)
                    {
                        if (!bllQuestion.Update(item, tran))
                        {
                            tran.Rollback();
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = "更新问题失败";
                            bllQuestion.ContextResponse(context, resp);
                            return;
                        }
                        foreach (var AnswerItem in item.Answers)
                        {
                            if (AnswerItem.PostType == -1)
                            {
                                if (bllQuestion.Delete(AnswerItem, tran) <= 0)
                                {
                                    tran.Rollback();
                                    resp.errcode = (int)APIErrCode.OperateFail;
                                    resp.errmsg = "删除选项失败";
                                    bllQuestion.ContextResponse(context, resp);
                                    return;
                                }
                            }
                            else if (AnswerItem.PostType == 1)
                            {
                                AnswerItem.AnswerID = Convert.ToInt32(bllQuestion.GetGUID(TransacType.AddAnswer));
                                AnswerItem.QuestionID = item.QuestionID;
                                if (!bllQuestion.Add(AnswerItem, tran))
                                {
                                    tran.Rollback();
                                    resp.errcode = (int)APIErrCode.OperateFail;
                                    resp.errmsg = "新增选项失败";
                                    bllQuestion.ContextResponse(context, resp);
                                    return;
                                }
                            }
                            else if (AnswerItem.PostType == 2)
                            {
                                if (!bllQuestion.Update(AnswerItem, tran))
                                {
                                    tran.Rollback();
                                    resp.errcode = (int)APIErrCode.OperateFail;
                                    resp.errmsg = "更新选项失败";
                                    bllQuestion.ContextResponse(context, resp);
                                    return;
                                }
                            }
                        }
                    }
                }
                if (AddQuestionList.Count > 0)
                {
                    foreach (var item in AddQuestionList)//添加问题表
                    {
                        item.QuestionID = int.Parse(bllQuestion.GetGUID(TransacType.AddQuestion));
                        if (!bllQuestion.Add(item, tran))
                        {
                            tran.Rollback();
                            resp.errcode = (int)APIErrCode.OperateFail;
                            resp.errmsg = "添加问题失败";
                            bllQuestion.ContextResponse(context, resp);
                            return;
                        }

                        foreach (var AnswerItem in item.Answers)
                        {
                            AnswerItem.AnswerID = Convert.ToInt32(bllQuestion.GetGUID(TransacType.AddAnswer));
                            AnswerItem.QuestionID = item.QuestionID;
                            if (!bllQuestion.Add(AnswerItem, tran))
                            {
                                tran.Rollback();
                                resp.errcode = (int)APIErrCode.OperateFail;
                                resp.errmsg = "新增选项失败";
                                bllQuestion.ContextResponse(context, resp);
                                return;
                            }
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
            public List<RequestAnswerModel> answer_list { get; set; }
        }
        public class RequestAnswerModel
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
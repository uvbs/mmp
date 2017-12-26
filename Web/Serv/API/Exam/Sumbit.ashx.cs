using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Exam
{
    /// <summary>
    /// 提交试卷
    /// </summary>
    public class Sumbit : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {

            BLLQuestion bllQuestion = new BLLQuestion();
            BLLUser bllUser = new BLLUser();
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            int examId = 0;
            try
            {
                string jsonData = context.Request["JsonData"];
                QuestionnaireRecordList list = ZentCloud.Common.JSONHelper.JsonToModel<QuestionnaireRecordList>(jsonData);
                if (list.Data.Count > 0)
                {
                    examId = list.Data[0].QuestionnaireID;
                    if (bllUser.GetCount<QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", CurrentUserInfo.UserID, list.Data[0].QuestionnaireID)) > 0)
                    {

                        apiResp.msg = "您已经提交过了";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;

                    }

                    long recordId = Convert.ToInt64(bllQuestion.GetRecordGUID());
                    foreach (var item in list.Data)
                    {
                        ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail model = new BLLJIMP.Model.QuestionnaireRecordDetail();
                        model.UserID = CurrentUserInfo.UserID;
                        model.QuestionnaireID = item.QuestionnaireID;
                        model.QuestionID = item.QuestionID;
                        model.AnswerID = item.AnswerID;
                        model.AnswerContent = item.AnswerContent;
                        model.RecordID = recordId;

                        if (!bllUser.Add(model))
                        {
                            tran.Rollback();
                            apiResp.msg = "提交失败";
                            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                            return;

                        }
                    }

                    QuestionnaireRecord record = new QuestionnaireRecord();
                    record.UserId = CurrentUserInfo.UserID;
                    record.QuestionnaireID = list.Data[0].QuestionnaireID;
                    record.InsertDate = DateTime.Now;
                    record.IP = context.Request.UserHostAddress;
                    record.RecordID = recordId;
                    if (!bllUser.Add(record))
                    {
                        tran.Rollback();
                        apiResp.msg = "提交失败";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }


                }
                else//自动提交
                {
                    QuestionnaireRecord record = new QuestionnaireRecord();
                    record.UserId = CurrentUserInfo.UserID;
                    record.QuestionnaireID = int.Parse(context.Request["examId"]);
                    record.InsertDate = DateTime.Now;
                    record.IP = context.Request.UserHostAddress;
                    record.RecordID = 0;
                    if (bllQuestion.Add(record))
                    {
                        apiResp.status = true;
                        Questionnaire questionModelA = bllUser.Get<Questionnaire>(string.Format(" QuestionnaireID={0} ", examId));
                        questionModelA.SubmitCount = bllUser.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID={0}", examId));
                        bllUser.Update(questionModelA);

                    }
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                    return;
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.msg = ex.Message;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;

            }
            tran.Commit();
            apiResp.status = true;

            int submitCount = bllUser.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID={0}", examId));
            Questionnaire questionModel = bllUser.Get<Questionnaire>(string.Format(" QuestionnaireID={0} ", examId));
            if (questionModel != null)
            {
                questionModel.SubmitCount = submitCount;
                bllUser.Update(questionModel);
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));



        }

        /// <summary>
        /// 问卷记录集合
        /// </summary>
        [Serializable]
        public class QuestionnaireRecordList
        {
            public List<QuestionnaireRecordDetailModel> Data { get; set; }
        }
        /// <summary>
        /// 提交问卷记录 反序列化模型
        /// </summary>
        [Serializable]
        public class QuestionnaireRecordDetailModel
        {
            /// <summary>
            /// 问卷编号 对应ZCJ_Questionnaire
            /// </summary>
            public int QuestionnaireID { get; set; }
            /// <summary>
            /// 问题编号 对应ZCJ_Question
            /// </summary>
            public int QuestionID { get; set; }
            /// <summary>
            /// 问题选项编号 对应ZCJ_Answer
            /// </summary>
            public int? AnswerID { get; set; }
            /// <summary>
            /// 答案选项 用于填空
            /// </summary>
            public string AnswerContent { get; set; }

            /// <summary>
            /// 推广人id
            /// </summary>
            public string PreUserId { get; set; }

        }

    }
}
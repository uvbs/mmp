using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Forbes;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.customize.forbes.question
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo = new UserInfo();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string result = "false";
            try
            {
                if (bllUser.IsLogin)
                {

                    currentUserInfo = bllUser.GetCurrentUserInfo();
                }
                else
                {
                    resp.errcode = 1;
                    resp.errmsg = "尚未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }
                string Action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(Action))
                {
                    MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action不存在";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
        outoff:
            context.Response.Write(result);
        }

        /// <summary>
        /// 获取我的问题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyQuestion(HttpContext context) {


            int count=int.Parse(context.Request["count"]);
            int unAnswerCount = bll.GetCount<ForbesQuestionPersonal>(string.Format(" Count={0} And Status=0 And UserId='{1}'",count,currentUserInfo.UserID));//检查第一道题是否还有未完成的题目
            if (unAnswerCount> 0)
            {

                ForbesQuestionPersonal model = bll.Get<ForbesQuestionPersonal>(string.Format(" Count={0} And Status=0 And UserId='{1}'  order by AutoID ASC" ,count,currentUserInfo.UserID));
                ForbesQuestion question = bll.Get<ForbesQuestion>(string.Format("AutoID={0}", model.QuestionId));

                 ForbesQuestion prequestion=new ForbesQuestion();
                ForbesQuestionPersonal premodel = bll.Get<ForbesQuestionPersonal>(string.Format(" Count={0} And Status=1 And UserId='{1}' order by AutoID DESC", count,currentUserInfo.UserID));
                if (premodel!=null)
                {
                    prequestion = bll.Get<ForbesQuestion>(string.Format("AutoID={0}", premodel.QuestionId));

                }
                return Common.JSONHelper.ObjectToJson(new
                {
                    errcode=0,
                    is_finish = 0,//是否完成标识
                    myquestion_id=model.AutoID,
                    category_name = question.Category,
                    knowledge_lv1 = question.KnowledgeLv1,
                    knowledge_lv2 = question.KnowledgeLv2,
                    question = question.Question,
                    answer_a = "A."+question.AnswerA,
                    answer_b = "B." + question.AnswerB,
                    answer_c = "C." + question.AnswerC,
                    answer_d = "D." + question.AnswerD,
                    correct_answer = question.CorrectAnswer,
                    correct_answer_code = question.CorrectAnswerCode,
                    pre_question=prequestion.Question,
                    pre_answer_a = "A." + prequestion.AnswerA,
                    pre_answer_b = "B." + prequestion.AnswerB,
                    pre_answer_c = "C." + prequestion.AnswerC,
                    pre_answer_d = "D." + prequestion.AnswerD,
                    pre_correctanswer=prequestion.CorrectAnswer
                });

            }
            else
            {
                return Common.JSONHelper.ObjectToJson(new {
                    errcode = 0,
                    is_finish = 1,
                
                
                });
            }



        
        }


        /// <summary>
        /// 获取我的最后的问题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyLastQuestion(HttpContext context)
        {


            int count = int.Parse(context.Request["count"]);
             ForbesQuestionPersonal lastmodel = bll.Get<ForbesQuestionPersonal>(string.Format(" Count={0} And  UserId ='{1}'order by AutoID DESC", count,currentUserInfo.UserID));

                ForbesQuestion question = bll.Get<ForbesQuestion>(string.Format("AutoID={0}", lastmodel.QuestionId));
                return Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    correctanswer = question.CorrectAnswer
                });

            
           




        }

        /// <summary>
        /// 提交答案
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SumbitAnswer(HttpContext context)
        {

            string myQuestionId=context.Request["myquestionid"];
            string answer=context.Request["answer"];
            if (string.IsNullOrEmpty(myQuestionId)||string.IsNullOrEmpty(answer))
            {
                resp.errcode = 1;
                resp.errmsg = "提交信息不完整";
                return Common.JSONHelper.ObjectToJson(resp);
                
            }
            ForbesQuestionPersonal model = bll.Get<ForbesQuestionPersonal>(string.Format(" AutoId={0}",myQuestionId));
            ForbesQuestion question = bll.Get<ForbesQuestion>(string.Format("AutoID={0}", model.QuestionId));
            model.Status = 1;
            if (answer.ToUpper()==question.CorrectAnswerCode.ToUpper())
            {
                 model.IsCorrect=1;//答对了
            }
            if (bll.Update(model))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "更新我的问题失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

            


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
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
    /// GetRecord 的摘要说明
    /// </summary>
    public class GetRecord : BaseHandlerNeedLoginNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();

        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string setId  = context.Request["set_id"];
            int total = 0;
            try
            {
                List<QuestionnaireRecord> list = bllQuestion.GetRecord(pageSize, pageIndex, setId, CurrentUserInfo.UserID, out total, true);

                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.result = new
                {
                    totalcount = total,
                    list = from p in list
                           select new {
                              id = p.AutoId,
                              correct_count = p.CorrectCount,
                              answer_count = p.AnswerCount,
                              date = p.InsertDate.ToString("yyyy-MM-dd HH:mm")
                           }
                };
            }
            catch (Exception ex)
            {
                apiResp.status = false;
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = ex.Message;
            }
            bllQuestion.ContextResponse(context, apiResp);

        }
    }
}
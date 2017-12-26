using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Question
{
    /// <summary>
    /// 调查问卷下拉
    /// </summary>
    public class SelectList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            List<Questionnaire> data = bllQuestion.GetSelectList(bllQuestion.WebsiteOwner,context.Request["type"]);
            var result = from p in data
                         select new
                         {
                             value = p.QuestionnaireID,
                             name = p.QuestionnaireName,
                             count = bllQuestion.GetQuestionCount(p.QuestionnaireID)
                         };
            apiResp.status = true;
            apiResp.result = result;
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}
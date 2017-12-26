using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.QuestionnaireSet
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        public void ProcessRequest(HttpContext context)
        {
            string AutoID = context.Request["AutoID"];
            BLLJIMP.Model.QuestionnaireSet QuestionnaireSetModel = 
                bllQuestion.GetByKey<BLLJIMP.Model.QuestionnaireSet>("AutoID", AutoID);
            if (QuestionnaireSetModel == null)
            {
                apiResp.code = (int)APIErrCode.ContentNotFound;
                apiResp.msg = "没有找到记录";
                bllQuestion.ContextResponse(context, apiResp);
                return;
            }
            
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = QuestionnaireSetModel;
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}
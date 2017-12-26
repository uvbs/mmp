using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.QuestionnaireSet
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        BLLJIMP.BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string AutoID = context.Request["AutoID"];
            BLLJIMP.Model.QuestionnaireSet QuestionnaireSetModel = bllQuestion.GetByKey<BLLJIMP.Model.QuestionnaireSet>("AutoID", AutoID);
            QuestionnaireSetModel = bllQuestion.ConvertRequestToModel<BLLJIMP.Model.QuestionnaireSet>(QuestionnaireSetModel);
            apiResp.status = bllQuestion.Update(QuestionnaireSetModel);
            if (apiResp.status)
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "更新完成";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "编辑答题[id="+AutoID+"]");
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "更新失败";
            }
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}
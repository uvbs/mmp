using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.QuestionnaireSet
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        BLLJIMP.BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string AutoIDs = context.Request["AutoIDs"];
            int nCount = bllQuestion.UpdateMultByKey<BLLJIMP.Model.QuestionnaireSet>("AutoID", AutoIDs, "IsDelete", "1");
            apiResp.status = nCount > 0;
            if (apiResp.status)
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "成功删除" + nCount + "行记录";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing, BLLJIMP.Enums.EnumLogTypeAction.Delete, bllLog.GetCurrUserID(), "删除答题[id=" + AutoIDs + "]");
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "删除失败";
            }
            bllQuestion.ContextResponse(context, apiResp);
        }
    }
}
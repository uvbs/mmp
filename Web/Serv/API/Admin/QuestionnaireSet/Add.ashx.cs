using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.QuestionnaireSet
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLQuestion bllQuestion = new BLLQuestion();
        BLLJIMP.BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.QuestionnaireSet QuestionnaireSetModel =
                bllQuestion.ConvertRequestToModel<BLLJIMP.Model.QuestionnaireSet>(new BLLJIMP.Model.QuestionnaireSet());
            QuestionnaireSetModel.CreateTime = DateTime.Now;
            QuestionnaireSetModel.CreateUserId = currentUserInfo.UserID;
            QuestionnaireSetModel.WebsiteOwner = bllQuestion.WebsiteOwner;
            apiResp.status = bllQuestion.Add(QuestionnaireSetModel);
            if (apiResp.status)
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "新增完成";
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Marketing,BLLJIMP.Enums.EnumLogTypeAction.Add,bllLog.GetCurrUserID(),"添加答题");
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "新增失败";
            }
            bllQuestion.ContextResponse(context, apiResp);
        }

    }
}
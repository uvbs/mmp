using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// GetSignStatus 的摘要说明
    /// </summary>
    public class GetSignStatus : BaseHandlerNeedLoginNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            int activity_id = Convert.ToInt32(context.Request["activity_id"]);
            apiResp.result = bll.GetSignStatus(activity_id, CurrentUserInfo.UserID, bll.WebsiteOwner);
            apiResp.msg = "查询完成";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}
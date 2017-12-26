using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// SignIn 的摘要说明
    /// </summary>
    public class SignIn : BaseHandlerNeedLoginNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            int activity_id = Convert.ToInt32(context.Request["activity_id"]);
            int uid = Convert.ToInt32(context.Request["uid"]);
            if (bll.SignIn(activity_id, uid, bll.WebsiteOwner, CurrentUserInfo))
            {

                apiResp.msg = "签到完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "签到失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
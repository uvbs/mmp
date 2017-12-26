using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ApplyNoPass 的摘要说明
    /// </summary>
    public class ApplyNoPass : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (bllUser.UpdateMultByKey<UserInfo>("AutoID", ids, "MemberApplyStatus", "2")>0)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "提交完成";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "提交失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }
    }
}
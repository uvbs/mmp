using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// SetAccessLevel 的摘要说明
    /// </summary>
    public class SetAccessLevel : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLComponent bll = new BLLJIMP.BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string access_level = context.Request["access_level"];
            if (bll.UpdateMultByKey<BLLJIMP.Model.Component>("AutoId", ids, "AccessLevel", access_level) > 0)
            {
                apiResp.msg = "设置完成";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "设置失败";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TransfersAudit
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginNoAction
    {

        BLLJIMP.BLLTransfersAudit bll = new BLLJIMP.BLLTransfersAudit();
        public void ProcessRequest(HttpContext context)
        {
            if (!bll.IsTransfersAuditPer(CurrentUserInfo) && (CurrentUserInfo.WebsiteOwner != bll.WebsiteOwner) && (CurrentUserInfo.UserType != 1))
            {
                apiResp.status = false;
                apiResp.msg = "您没有审核权限";
                bll.ContextResponse(context, apiResp);
                return;
            }
            string msg = "";
            apiResp.status = bll.Pass(int.Parse(context.Request["id"]), bll.GetCurrUserID(),out msg);
            apiResp.msg = msg;
            bll.ContextResponse(context, apiResp);


        }



    }
}
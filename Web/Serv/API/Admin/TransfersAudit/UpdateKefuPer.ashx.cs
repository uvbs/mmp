using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TransfersAudit
{
    /// <summary>
    /// UpdateKefuPer 的摘要说明
    /// </summary>
    public class UpdateKefuPer : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLTransfersAudit bll = new BLLJIMP.BLLTransfersAudit();
        public void ProcessRequest(HttpContext context)
        {
            if (!bll.IsTransfersAuditPer(CurrentUserInfo)&&(CurrentUserInfo.WebsiteOwner!=bll.WebsiteOwner)&&(CurrentUserInfo.UserType!=1))
            {
                apiResp.status = false;
                apiResp.msg = "您没有审核权限";
                bll.ContextResponse(context, apiResp);
                return;
            }
            string msg = "";
            apiResp.status = bll.UpdateKefuTranPer(int.Parse(context.Request["id"]), context.Request["value"]);
            apiResp.msg = msg;
            bll.ContextResponse(context, apiResp);


        }


    }
}
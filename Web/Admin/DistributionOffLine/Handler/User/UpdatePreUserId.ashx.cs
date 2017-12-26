using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.User
{

    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class UpdatePreUserId : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            string autoIds = context.Request["autoIds"];
            string preUserId=context.Request["preUserId"];
            apiResp.status = bll.UpdatePreUserId(autoIds, preUserId);
            context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
        }




    }


}
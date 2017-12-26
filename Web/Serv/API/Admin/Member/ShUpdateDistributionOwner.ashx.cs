using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ShUpdateDistributionOwner 的摘要说明
    /// </summary>
    public class ShUpdateDistributionOwner : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string spreadid = context.Request["spreadid"];
            string websiteOwner = bll.WebsiteOwner;
            string msg = "";
            bool result = bll.UpdateDistributionOwner(websiteOwner, id, spreadid, currentUserInfo, out msg);

            if (string.IsNullOrWhiteSpace(msg)) msg = "修改成功";
            apiResp.msg = msg;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = result;
            bll.ContextResponse(context, apiResp);
        }
    }
}
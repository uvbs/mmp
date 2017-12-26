using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage.Version
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string os = context.Request["os"];
            string websiteOwner = context.Request["websiteOwner"];
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = bllApp.WebsiteOwner;

            BLLJIMP.Model.AppManageVersion ver = bllApp.GetVersion(websiteOwner, os, id);
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = ver;
            apiResp.msg = "查询完成";
            bllApp.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
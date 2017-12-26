using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.AppManage
{
    /// <summary>
    /// GetVersion 的摘要说明
    /// </summary>
    public class GetVersion : BaseHandlerNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            string os = context.Request["os"];
            string websiteOwner = context.Request["websiteOwner"];
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = bllApp.WebsiteOwner;

            BLLJIMP.Model.AppManageVersion ver = bllApp.GetVersion(websiteOwner, os, "");
            apiResp.status = (ver != null);
            apiResp.code = apiResp.status ? (int)BLLJIMP.Enums.APIErrCode.IsSuccess : (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            apiResp.result =!apiResp.status ? null:new {
                version = ver.AppVersion,
                info = ver.AppVersionInfo,
                show = ver.AppVersionPublishPath,
                install = ver.AppVersionInstallPath
            };
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
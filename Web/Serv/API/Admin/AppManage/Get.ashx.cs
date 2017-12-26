using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage
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
            string websiteOwner = context.Request["websiteOwner"];
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = bllApp.WebsiteOwner;

            BLLJIMP.Model.AppManage app = new BLLJIMP.Model.AppManage();
            if (id == "0"){
                app = bllApp.GetByKey<BLLJIMP.Model.AppManage>("WebsiteOwner", websiteOwner);
            }
            else{
                app = bllApp.GetByKey<BLLJIMP.Model.AppManage>("AutoID", id, websiteOwner: websiteOwner);
            }
            if (app == null) app = new BLLJIMP.Model.AppManage();

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = app;
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
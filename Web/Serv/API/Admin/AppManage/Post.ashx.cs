using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage
{
    /// <summary>
    /// Post 的摘要说明
    /// </summary>
    public class Post : BaseHandlerNeedLoginAdminNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.AppManage app = bllApp.ConvertRequestToModel<BLLJIMP.Model.AppManage>(new BLLJIMP.Model.AppManage());
            if (string.IsNullOrWhiteSpace(app.WebsiteOwner)) app.WebsiteOwner = bllApp.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(app.AppId))
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "请输入应用Id";
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(app.AppName))
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "请输入应用名称";
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.AppManage oApp = new BLLJIMP.Model.AppManage();
            if (app.AutoID ==0){
                oApp = bllApp.GetApp(app.WebsiteOwner,app.AppId);
            }
            else {
                oApp = bllApp.GetByKey<BLLJIMP.Model.AppManage>("AutoID", app.AutoID.ToString(), websiteOwner: app.WebsiteOwner);
            }
            if (oApp == null) {
                apiResp.status = bllApp.Add(app);
            }
            else
            {
                oApp.AppName = app.AppName;
                oApp.AppInfo = app.AppInfo;
                oApp.AppId = app.AppId;
                oApp.StartAdHref = app.StartAdHref;
                //oApp.IosAppId = app.IosAppId;
                //oApp.IosAppPrivate = app.IosAppPrivate;
                //oApp.IosAppPrivateFile = app.IosAppPrivateFile;
                //oApp.IosAppProfile = app.IosAppProfile;
                //oApp.AndroidAppId = app.AndroidAppId;
                //oApp.AndroidAppPrivate = app.AndroidAppPrivate;
                //oApp.AndroidAppCertificateName = app.AndroidAppCertificateName;
                //oApp.AndroidAppCertificateFile = app.AndroidAppCertificateFile;
                apiResp.status = bllApp.Update(oApp);
            }
            apiResp.code = apiResp.status ? (int)BLLJIMP.Enums.APIErrCode.IsSuccess : (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            apiResp.msg = apiResp.status ? "提交完成" : "提交失败";
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
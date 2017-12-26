using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage.Version
{
    /// <summary>
    /// Post 的摘要说明
    /// </summary>
    public class Post : BaseHandlerNeedLoginAdminNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.AppManageVersion ver = bllApp.ConvertRequestToModel<BLLJIMP.Model.AppManageVersion>(new BLLJIMP.Model.AppManageVersion());
            if (string.IsNullOrWhiteSpace(ver.WebsiteOwner)) ver.WebsiteOwner = bllApp.WebsiteOwner;
            if (ver.AutoID == 0)
            {
                ver.AppOS = ver.AppOS.ToLower();
                if (!ver.AppVersionPublishDate.HasValue) ver.AppVersionPublishDate = DateTime.Now;
                apiResp.status = bllApp.Add(ver);
            }
            else
            {

                BLLJIMP.Model.AppManageVersion oVer = bllApp.GetVersion(ver.WebsiteOwner, ver.AppOS, ver.AutoID.ToString());
                oVer.AppVersion = ver.AppVersion;
                oVer.AppVersionInfo = ver.AppVersionInfo;
                oVer.AppVersionPublish = ver.AppVersionPublish;
                oVer.AppVersionPublishPath = ver.AppVersionPublishPath;
                oVer.AppVersionInstallPath = ver.AppVersionInstallPath;
                if (ver.AppVersionPublishDate.HasValue && ver.AppVersionPublishDate.Value.ToString("yyyy/MM/dd HH:mm:ss") != oVer.AppVersionPublishDate.Value.ToString("yyyy/MM/dd HH:mm:ss")) oVer.AppVersionPublishDate = ver.AppVersionPublishDate;
                apiResp.status = bllApp.Update(oVer);
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
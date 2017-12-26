using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string keyword = context.Request["keyword"];
            string websiteOwner = context.Request["websiteOwner"];

            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = bllApp.WebsiteOwner;
            else if (websiteOwner == "all") websiteOwner = "";

            int total = bllApp.GetAppCount(websiteOwner, keyword);
            List<dynamic> rList = new List<dynamic>();
            if (total > 0) { 
                List<BLLJIMP.Model.AppManage> list = bllApp.GetAppList(rows, page, websiteOwner, keyword);
                foreach (BLLJIMP.Model.AppManage item in list)
                {
                    int androidCount = bllApp.GetVersionCount(websiteOwner, "android", "");
                    int iosCount = bllApp.GetVersionCount(websiteOwner, "ios", "");
                    BLLJIMP.Model.AppManageVersion androidVer = bllApp.GetVersion(websiteOwner, "android", "");
                    BLLJIMP.Model.AppManageVersion iosVer = bllApp.GetVersion(websiteOwner, "ios", "");

                    rList.Add(new
                    {
                        item.AutoID,
                        item.AppId,
                        item.AppName,
                        item.AppInfo,
                        item.WebsiteOwner,
                        item.StartAdHref,
                        AndroidCount = androidCount,
                        AndroidPath =androidVer ==null?"":androidVer.AppVersionPublishPath,
                        IosCount = iosCount,
                        IosPath = iosVer == null ? "" : iosVer.AppVersionPublishPath
                    });
                }
            }
            
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = total,
                list = rList
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
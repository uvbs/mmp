using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage.Version
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
            string websiteOwner = context.Request["websiteOwner"];
            string os = context.Request["os"];
            string keyword = context.Request["keyword"];
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = bllApp.WebsiteOwner;
            int total = bllApp.GetVersionCount(websiteOwner, os, keyword);
            List<dynamic> rList = new List<dynamic>();
            if (total > 0) {
                List<BLLJIMP.Model.AppManageVersion> list = bllApp.GetVersionList(rows, page, websiteOwner, os, keyword);
                foreach (var item in list)
                {
                    rList.Add(new
                    {
                        item.AutoID,
                        item.AppVersion,
                        item.AppVersionPublish,
                        item.AppVersionPublishPath,
                        item.AppVersionInstallPath,
                        AppVersionPublishDate = item.AppVersionPublishDate.Value.ToString("yyyy/MM/dd"),
                        item.AppVersionInfo
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
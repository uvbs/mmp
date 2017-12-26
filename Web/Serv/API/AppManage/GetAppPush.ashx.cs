using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.AppManage
{
    /// <summary>
    /// GetAppPush 的摘要说明
    /// </summary>
    public class GetAppPush : BaseHandlerNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            WebsiteInfo website = bllApp.GetWebsiteInfoModelFromDataBase();
            apiResp.status = bllApp.HaveGetuiAppPush(website);
            apiResp.code = (int)APIErrCode.IsSuccess;
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
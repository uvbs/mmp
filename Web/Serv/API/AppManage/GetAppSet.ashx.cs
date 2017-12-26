using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.AppManage
{
    /// <summary>
    /// GetAppSet 的摘要说明
    /// </summary>
    public class GetAppSet : BaseHandlerNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();

        public void ProcessRequest(HttpContext context)
        {
            string appid = context.Request["appid"];
            string websiteOwner = bllApp.WebsiteOwner;
            BLLJIMP.Model.AppManage app = bllApp.GetApp(websiteOwner, appid);

            apiResp.status = (app != null);
            apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
            apiResp.result = !apiResp.status ? null : new
            {
                has_alipay_app_set = bllApp.IsAppAlipay(app),
                has_weixin_app_set = bllApp.IsAppWeixin(app),
                start_ad_href = app.StartAdHref
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
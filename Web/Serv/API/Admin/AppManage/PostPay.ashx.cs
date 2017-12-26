using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.AppManage
{
    /// <summary>
    /// PostPay 的摘要说明
    /// </summary>
    public class PostPay : BaseHandlerNeedLoginAdminNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.AppManage app = bllApp.ConvertRequestToModel<BLLJIMP.Model.AppManage>(new BLLJIMP.Model.AppManage());
            BLLJIMP.Model.AppManage oApp = bllApp.GetByKey<BLLJIMP.Model.AppManage>("AutoID", app.AutoID.ToString(), websiteOwner: app.WebsiteOwner);

            oApp.AlipayAppId = app.AlipayAppId;
            oApp.AlipayPrivatekey = app.AlipayPrivatekey;
            oApp.AlipayPublickey = app.AlipayPublickey;
            oApp.AlipaySignType = app.AlipaySignType;
            oApp.WxAppId = app.WxAppId;
            oApp.WxAppSecret = app.WxAppSecret;
            apiResp.status = bllApp.Update(oApp);
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
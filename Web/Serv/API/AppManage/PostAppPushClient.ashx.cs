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
    /// PostAppPushClient 的摘要说明
    /// </summary>
    public class PostAppPushClient : BaseHandlerNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            string uuid = context.Request["uuid"];
            string clientid = context.Request["clientid"];
            string appid = context.Request["appid"];
            string websiteOwner = bllApp.WebsiteOwner;
            UserInfo user = bllApp.GetCurrentUserInfo();
            WebsiteInfo website = bllApp.GetWebsiteInfoModelFromDataBase();

            AppPushClient uClient = bllApp.GetAppPushClient(websiteOwner, clientid, appid, uuid, "");
            if(uClient != null){
                if (user != null)
                {
                    uClient.UserID = user.UserID;
                    apiResp.status = bllApp.Update(uClient);
                }
                else
                {
                    apiResp.status = true;
                }
                apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(clientid)) uClient = bllApp.GetAppPushClient(websiteOwner, "","", uuid, "");
            if(uClient != null){
                 if (user != null) uClient.UserID = user.UserID;
                 uClient.PushClientId = clientid;
                 uClient.PushAppId = appid;
                 if (!string.IsNullOrWhiteSpace(uClient.PushAppId) && uClient.PushAppId != website.AppPushAppId)
                 {
                     apiResp.status = false;
                     apiResp.code = (int)APIErrCode.OperateFail;
                     bllApp.ContextResponse(context, apiResp);
                     return;
                 }
                apiResp.status = bllApp.Update(uClient);
                apiResp.code = apiResp.status? (int)APIErrCode.IsSuccess:(int)APIErrCode.OperateFail;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            uClient = new AppPushClient();
            uClient.WebsiteOwner = websiteOwner;
            uClient.UUId = uuid;
            uClient.PushClientId = clientid;
            uClient.PushAppId = appid;
            if (user != null) uClient.UserID = user.UserID;
            uClient.InsertDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(uClient.PushAppId) && uClient.PushAppId != website.AppPushAppId)
            {
                apiResp.status = false;
                apiResp.code = (int)APIErrCode.OperateFail;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = bllApp.Add(uClient);
            apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
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
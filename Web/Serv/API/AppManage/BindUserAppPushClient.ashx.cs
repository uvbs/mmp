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
    /// BindUserAppPushClient 的摘要说明
    /// </summary>
    public class BindUserAppPushClient : BaseHandlerNeedLoginNoAction
    {
        BLLAppManage bllApp = new BLLAppManage();
        public void ProcessRequest(HttpContext context)
        {
            string uuid = context.Request["uuid"];
            string clientid = context.Request["clientid"];
            string appid = context.Request["appid"];
            string websiteOwner = bllApp.WebsiteOwner;
            WebsiteInfo website = bllApp.GetWebsiteInfoModelFromDataBase();

            AppPushClient uClient = bllApp.GetAppPushClient(websiteOwner, clientid, appid, uuid, CurrentUserInfo.UserID);
            if (uClient != null)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            uClient = bllApp.GetAppPushClient(websiteOwner, clientid, appid, uuid, "");
            if (uClient != null)
            {
                uClient.UserID = CurrentUserInfo.UserID;
                apiResp.status = bllApp.Update(uClient);
                apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(clientid)) uClient = bllApp.GetAppPushClient(websiteOwner, "","", uuid, "");
            if (uClient != null)
            {
                uClient.UserID = CurrentUserInfo.UserID;
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
                apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
                bllApp.ContextResponse(context, apiResp);
                return;
            }
            uClient = new AppPushClient();
            uClient.WebsiteOwner = websiteOwner;
            uClient.UUId = uuid;
            uClient.PushClientId = clientid;
            uClient.PushAppId = appid;
            uClient.InsertDate = DateTime.Now;
            uClient.UserID = CurrentUserInfo.UserID;

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
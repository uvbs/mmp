using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.Socket;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// QRCodeLogin 的摘要说明
    /// </summary>
    public class QRCodeLogin : IHttpHandler, IRequiresSessionState
    {
        BLLUser bllUser = new BLLUser("");
        protected BaseResponse apiResp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            string redis_key = context.Request["redis_key"];

            QRCodeLoginRedis qrReids = new QRCodeLoginRedis();
            try
            {
                qrReids = RedisHelper.RedisHelper.StringGet<QRCodeLoginRedis>(redis_key);
            }
            catch (Exception ex)
            {
                apiResp.code = (int) ZentCloud.BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "redis服务错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (qrReids == null)
            {
                apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "redis中未找到登录用户";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo curUser = bllUser.GetUserInfo(qrReids.userID, bllUser.WebsiteOwner);
            if (qrReids == null)
            {
                apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "登录用户未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            bllUser.UpdateLastLoginInfo(curUser.UserID, "", "", "", bllUser.WebsiteOwner);
            context.Session[SessionKey.UserID] = curUser.UserID;
            context.Session[SessionKey.LoginStatu] = 1;

            apiResp.status = true;
            apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "登录成功";
            apiResp.result = new
            {
                id = curUser.AutoID,
                username = curUser.UserID,
                nickname = bllUser.GetUserDispalyName(curUser),
                avatar = bllUser.GetUserDispalyAvatar(curUser),
                im_token = curUser.IMToken
            };
            bllUser.ContextResponse(context, apiResp);
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
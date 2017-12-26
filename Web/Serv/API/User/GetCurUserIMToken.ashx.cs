using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// GetCurUserIMToken 的摘要说明
    /// </summary>
    public class GetCurUserIMToken : BaseHandlerNeedLoginNoAction
    {
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
            var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
            if(!string.IsNullOrWhiteSpace(CurrentUserInfo.IMToken)){
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                apiResp.msg = "获取用户成功";
                apiResp.status = true;
                apiResp.result = new {
                    appkey = websiteInfo.NIMAppKey,
                    accid = CurrentUserInfo.AutoID,
                    token = CurrentUserInfo.IMToken
                };
                new BLLJIMP.BLL().ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.BLLIM bllIM = new BLLJIMP.BLLIM(websiteInfo.NIMAppKey, websiteInfo.NIMAppSecret);
            string token = "";
            if (CurrentUserInfo.UserType != 6)
            {

                token = bllIM.RefreshToken(CurrentUserInfo.AutoID.ToString());
                if (string.IsNullOrWhiteSpace(token)) token = bllIM.CreateUser(CurrentUserInfo.AutoID.ToString(), bllUser.GetUserDispalyName(CurrentUserInfo), "", bllUser.GetUserDispalyAvatar(CurrentUserInfo));
                bllUser.UpdateByKey<BLLJIMP.Model.UserInfo>("AutoID", CurrentUserInfo.AutoID.ToString(), "IMToken", token);
            }
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "获取用户成功";
            apiResp.status = true;
            apiResp.result = new
            {
                appkey = websiteInfo.NIMAppKey,
                accid = CurrentUserInfo.AutoID,
                token = token
            };
            new BLLJIMP.BLL().ContextResponse(context, apiResp);
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
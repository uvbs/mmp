using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// GetUserJson 的摘要说明
    /// </summary>
    public class GetUserJson : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string userId = CurrentUserInfo.UserID;
            string websiteOwner = bllUser.WebsiteOwner;
            //bllUser.GetUserJson(websiteOwner, userId);
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取用户信息";
            apiResp.result = bllUser.GetUserJson(websiteOwner, userId);
            bllUser.ContextResponse(context, apiResp);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// IsPayPassword 的摘要说明
    /// </summary>
    public class IsPayPassword : BaseHandlerNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.UserInfo currUser = bllUser.GetCurrentUserInfo();
            if (!string.IsNullOrEmpty(currUser.PayPassword))
            {
                apiResp.status = true;
                apiResp.result = currUser.PayPassword;
                apiResp.msg = "已设置支付密码";
            }
            bllUser.ContextResponse(context, apiResp);
        }

    }
}
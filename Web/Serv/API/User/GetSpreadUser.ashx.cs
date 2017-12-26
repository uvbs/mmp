using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// GetSpreadUser 的摘要说明
    /// </summary>
    public class GetSpreadUser : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string spreadid = context.Request["spreadid"];
            string websiteOwner = bllUser.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(spreadid))
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "请输入手机/编号";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            UserInfo spreadUser = bllUser.GetSpreadUser(spreadid, websiteOwner, 10);
            if (spreadUser == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "推荐人未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = new
            {
                id = spreadUser.AutoID,
                uid = spreadUser.UserID,
                name = spreadUser.TrueName,
                level = spreadUser.MemberLevel
            };
            bllUser.ContextResponse(context, apiResp);

        }
    }
}
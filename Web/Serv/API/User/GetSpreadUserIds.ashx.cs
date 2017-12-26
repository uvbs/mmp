using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// GetSpreadUserIds 的摘要说明
    /// </summary>
    public class GetSpreadUserIds : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string member = context.Request["member"];
            string websiteOwner = bllUser.WebsiteOwner;
            string userIds = bllUser.GetSpreadUserIds(member, websiteOwner);

            List<string> idList = userIds.Split(',').ToList();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询列表完成";
            apiResp.result = new
            {
                totalcount = idList.Count,
                list = idList
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
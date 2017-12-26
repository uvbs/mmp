using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Followers
{
    /// <summary>
    /// UpdateFansCount 的摘要说明 更新粉丝数量
    /// </summary>
    public class UpdateFansCount : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
            //resp.returnValue = bllWeixin.UpdateAllFollowersInfo(bllWeixin.GetCurrWebSiteUserInfo().UserID).ToString();
            //resp.isSuccess = true;
            //context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}
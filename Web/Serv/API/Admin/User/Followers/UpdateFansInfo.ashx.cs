using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Followers
{
    /// <summary>
    /// UpdateFansInfo 的摘要说明   更新粉丝信息
    /// </summary>
    public class UpdateFansInfo : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {

            //resp.returnValue = bllWeixin.SynchronousAllFollowers(bllWeixin.GetCurrUserID(), bllWeixin.GetCurrentUserInfo().WeixinAppId, bllWeixin.GetCurrentUserInfo().WeixinAppSecret).ToString();
            //resp.isSuccess = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}
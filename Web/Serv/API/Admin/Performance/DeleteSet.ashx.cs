using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// DeleteSet 的摘要说明
    /// </summary>
    public class DeleteSet : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();

        public void ProcessRequest(HttpContext context)
        {
            string user_id = context.Request["user_id"];
            string websiteOwner = bll.WebsiteOwner;
            if(bll.Delete(new TeamPerformanceSet(),
                string.Format("UserId='{0}' And WebsiteOwner='{1}'", user_id, websiteOwner)) > 0)
            {
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
                apiResp.msg = "删除成功";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.status = false;
                apiResp.msg = "删除失败";
            }
            bll.ContextResponse(context, apiResp);
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
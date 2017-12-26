using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// GetLockScoreCount 的摘要说明
    /// </summary>
    public class GetLockScoreCount : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lock_type = "1";
            if (!string.IsNullOrWhiteSpace(context.Request["lock_type"])) lock_type = context.Request["lock_type"];
            string lock_status = context.Request["lock_status"];
            BllScore bllScore = new BllScore();
            apiResp.result = bllScore.GetLockScoreCount(bllScore.GetCurrUserID(), bllScore.WebsiteOwner, lock_type, lock_status);
            apiResp.status = true;
            bllScore.ContextResponse(context, apiResp);
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
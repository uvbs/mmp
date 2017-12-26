using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// Summary description for GetTotalLockScore
    /// </summary>
    public class GetTotalLockScore : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            string lock_type = "1";
            if (!string.IsNullOrWhiteSpace(context.Request["lock_type"])) lock_type = context.Request["lock_type"];
            BllScore bllScore = new BllScore();
            apiResp.result = bllScore.GetTotalLockScore(bllScore.GetCurrUserID(), bllScore.WebsiteOwner, lock_type);
            apiResp.status = true;
            bllScore.ContextResponse(context, apiResp);
        }
        
    }
}
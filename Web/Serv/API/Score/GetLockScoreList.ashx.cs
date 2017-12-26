using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// Summary description for GetLockScoreList
    /// </summary>
    public class GetLockScoreList : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            int rows = 10000;
            int page = 1;
            string lock_type = "1";
            if (!string.IsNullOrWhiteSpace(context.Request["rows"])) rows = Convert.ToInt32(context.Request["rows"]);
            if (!string.IsNullOrWhiteSpace(context.Request["page"])) page = Convert.ToInt32(context.Request["page"]);
            if (!string.IsNullOrWhiteSpace(context.Request["lock_type"])) lock_type = context.Request["lock_type"];
            string lock_status = context.Request["lock_status"];
            BllScore bllScore = new BllScore();
            apiResp.result = bllScore.GetLockScoreList(rows, page, bllScore.GetCurrUserID(), bllScore.WebsiteOwner, lock_type, lock_status);
            apiResp.status = true;
            bllScore.ContextResponse(context, apiResp);
        }
        
    }
}
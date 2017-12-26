using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Level
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLDistribution bll = new BLLDistribution();

        public void ProcessRequest(HttpContext context)
        {
            string level = context.Request["level"];
            string type = context.Request["type"];
            string from = context.Request["from"];
            if(string.IsNullOrWhiteSpace(type)) type = "DistributionOnLine";
            string websiteOwner = bll.WebsiteOwner;
            List<UserLevelConfig> list = bll.QueryUserLevelList(websiteOwner, type, level, from, false, "","LevelNumberSort");

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.result = (from p in list
                             select new { 
                                level = p.LevelNumber,
                                from_score =p.FromHistoryScore,
                                name = p.LevelString
                             });
            bll.ContextResponse(context, apiResp);
        }

    }
}
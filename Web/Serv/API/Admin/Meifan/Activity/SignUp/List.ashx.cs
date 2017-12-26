using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Activity.SignUp
{
    /// <summary>
    /// 报名数据
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            int totalCount;
            string keyWord = context.Request["keyword"];
            string activityId = context.Request["activity_id"];
            string activityType=context.Request["activity_type"];
            string activityName=context.Request["activity_name"];
            string fromDate=context.Request["from_date"];
            string toDate=context.Request["to_date"];
            var data = bll.ActivityDataList(pageIndex, pageSize, out totalCount, activityId, "", activityType, keyWord,activityName,fromDate,toDate);
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = data
            };

            bll.ContextResponse(context, apiResp);

        }

    }
}
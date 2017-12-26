using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Match.SignUp
{


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
                var data = bll.ActivityDataList(pageIndex, pageSize, out totalCount, activityId, "", "", keyWord);
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
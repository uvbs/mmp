using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Activity
{
    /// <summary>
    ///活动列表
    /// </summary>
    public class List : BaseHandlerNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount;
            var data = bll.ActivityList(pageIndex, pageSize, "activity","","1", out totalCount);
            var list = from p in data
                       select new
                       {
                           activity_id = p.JuActivityID,
                           activity_name =bll.GetActivityShortName(p.ActivityName),
                           activity_img = p.ThumbnailsPath,
                           summary = p.Summary,
                           status = bll.GetActivityStatus(p)
                           
                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
            };

            bll.ContextResponse(context, apiResp);

        }


        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Match
{
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10; int totalCount;
            string keyWord = context.Request["keyword"];
            string isPublish = context.Request["is_publish"];
            var data = bll.ActivityList(pageIndex, pageSize, "match", keyWord, isPublish, out totalCount);
            var list = from p in data
                       select new
                       {

                           activity_id = p.JuActivityID,
                           activity_name = p.ActivityName,
                           activity_img = p.ThumbnailsPath,
                           summary = p.Summary,
                           //activity_type = p.ActivityType,
                           is_need_pay = p.IsFee,
                           is_publish = p.IsPublish,
                           address = p.ActivityAddress,
                           //signup_count = bll.GetSignUpCount(p.ActivityId)

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
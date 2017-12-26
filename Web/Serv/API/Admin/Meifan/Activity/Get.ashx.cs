using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Activity
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string activityId = context.Request["activity_id"];
            if (string.IsNullOrEmpty(activityId))
            {
                apiResp.status = false;
                apiResp.msg = "activity_id 参数必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var data = bll.GetActivity(activityId);
            if (data == null)
            {
                apiResp.status = false;
                apiResp.msg = "activity_id 参数错误";
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                activity_id = data.JuActivityID,
                activity_name = data.ActivityName,
                activity_img = data.ThumbnailsPath,
                summary = data.Summary,
                activity_type = data.ArticleType,
                begin_date = data.BeginDate,
                end_date = data.EndDate,
                is_need_pay = data.IsFee,
                address = data.ActivityAddress,
                activity_item = from i in bll.ActivityItemList(data.JuActivityID.ToString())
                                select new
                                {
                                    item_id = i.AutoId,
                                    from_date = i.FromDate,
                                    to_date = i.ToDate,
                                    date_range = string.Format("{0}-{1}", i.FromDate, i.ToDate),
                                    group_type = i.GroupType,
                                    is_member = i.IsMember,
                                    amount = i.Amount
                                },
                description = data.ActivityDescription
            };
            bll.ContextResponse(context, apiResp);

        }


        
    }
}
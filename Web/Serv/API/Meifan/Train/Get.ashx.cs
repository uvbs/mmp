﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Train
{
    /// <summary>
    /// 培训详情
    /// </summary>
    public class Get : BaseHandlerNoAction
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
                insert_date = data.CreateDate.ToString("yyyy-MM-dd"),
                description = data.ActivityDescription,
                is_need_pay=data.IsFee,
                items = from i in bll.ActivityItemList(data.JuActivityID.ToString())
                        select new
                        {
                            item_id = i.AutoId,
                            from_date = i.FromDate,
                            to_date = i.ToDate,
                            date_range = string.Format("{0}-{1}", Convert.ToDateTime(i.FromDate).ToString("yyyy/MM/dd"), Convert.ToDateTime(i.ToDate).ToString("yyyy/MM/dd")),
                            group_type = i.GroupType,
                            is_member = i.IsMember,
                            amount = i.Amount
                        },
                date_list = bll.ActivityItemDateList(data.JuActivityID.ToString()),
                default_date = bll.GetDefaultActivityItemDate(data.JuActivityID.ToString()),
                group_list = bll.ActivityItemGroupList(data.JuActivityID.ToString()),
                member_type_list = bll.ActivityItemMemberTypeList(data.JuActivityID.ToString()),
                is_expire = bll.IsExpire(data)

            };
            bll.ContextResponse(context, apiResp);

        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Task
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLTimingTask bll = new BLLJIMP.BLLTimingTask();
        public void ProcessRequest(HttpContext context)
        {
            string fromDateStr = context.Request["from_date"];
            string toDateStr = context.Request["to_date"];
            string channelUserId = context.Request["channel_user_id"];
            string distributionUserId = context.Request["distribution_user_id"];

            BLLJIMP.Model.TimingTask model = new BLLJIMP.Model.TimingTask();
            model.WebsiteOwner = bll.WebsiteOwner;
            model.InsertDate = DateTime.Now;
            model.FromDate = DateTime.Parse(fromDateStr);
            model.ToDate = DateTime.Parse(toDateStr).AddHours(23).AddMinutes(59).AddSeconds(59);
            model.ChannelUserId = channelUserId;
            model.DistributionUserId = distributionUserId;
            model.TaskType = 12;
            model.ScheduleDate = DateTime.Now;
            model.TaskId=bll.GetGUID(BLLJIMP.TransacType.CommAdd);
            model.Status = 1; 
            if (bll.Add(model))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.status = false;
                apiResp.msg = "添加失败";
            }
            bll.ContextResponse(context,apiResp);


           
        }


    }
}
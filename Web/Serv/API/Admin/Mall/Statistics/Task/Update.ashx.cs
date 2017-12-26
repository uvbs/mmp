using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Task
{
    /// <summary>
    /// Update 修改
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 
        /// </summary>
        BLLJIMP.BLLTimingTask bllTimingTask = new BLLJIMP.BLLTimingTask();
        public void ProcessRequest(HttpContext context)
        {
            string id=context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                apiResp.msg = "id为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllTimingTask.ContextResponse(context, apiResp);
                return;
            }

            string fromDateStr = context.Request["from_date"];
            string toDateStr = context.Request["to_date"];
            string channelUserId = context.Request["channel_user_id"];
            string distributionUserId = context.Request["distribution_user_id"];

            var task = bllTimingTask.GetTimingTask(Convert.ToInt32(id));

            if (task == null)
            {
                apiResp.msg = "修改对象不存在";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllTimingTask.ContextResponse(context, apiResp);
                return;
            }

            //清除旧数据
            bllTimingTask.Delete(new WXMallStatisticsOrder(),string.Format("TaskId='{0}'",task.TaskId));
            bllTimingTask.Delete(new WXMallStatisticsProduct(), string.Format("TaskId='{0}'", task.TaskId));

            task.FromDate =DateTime.Parse(fromDateStr);
            task.ToDate = DateTime.Parse(toDateStr).AddHours(23).AddMinutes(59).AddSeconds(59);
            task.ChannelUserId = channelUserId;
            task.DistributionUserId = distributionUserId;
            task.ScheduleDate = DateTime.Now;
            task.Status = 1;
            if (bllTimingTask.Update(task))
            {
                apiResp.msg = "修改完成";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "修改失败";
                apiResp.status = false;
            }
            bllTimingTask.ContextResponse(context, apiResp);



        }
    }
}
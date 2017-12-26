using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Task
{
    /// <summary>
    /// 商城任务统计列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLTimingTask bll = new BLLJIMP.BLLTimingTask();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = 0;
           var sourceData= bll.GetTimingTaskList(pageSize, pageIndex, "", "12", "", out totalCount, "");
           var data = from p in sourceData
                      select new
                      {
                         id=p.AutoId,
                         task_id=p.TaskId,
                         insert_date=p.InsertDateString,
                         from_date=p.FromDate!=null?((DateTime)p.FromDate).ToString("yyyy-MM-dd HH:mm:ss"):"",
                         to_date = p.ToDate != null ? ((DateTime)p.ToDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                         channel_user_id=p.ChannelUserId,
                         channel_name=!string.IsNullOrEmpty(p.ChannelUserId)?bllUser.GetUserInfo(p.ChannelUserId).ChannelName:"",
                         distribution_user_id=p.DistributionUserId,
                         distribution_name = !string.IsNullOrEmpty(p.DistributionUserId) ? bllUser.GetUserDispalyName(p.DistributionUserId) : "",
                         status=p.Status,
                         status_str=p.StatusString
                      };

           apiResp.status = true;
           apiResp.result = new { 
           totalcount=totalCount,
           list=data
           
           };
           bll.ContextResponse(context, apiResp);

        }


    }
}
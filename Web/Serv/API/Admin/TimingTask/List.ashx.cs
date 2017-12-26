using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TimingTask
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLTimingTask bllTimingTask = new BLLJIMP.BLLTimingTask();
        public void ProcessRequest(HttpContext context)
        {
            int page=int.Parse(context.Request["page"]);
            int rows=int.Parse(context.Request["rows"]);
            string keyWord=context.Request["keyword"];
            string status=context.Request["status"];
            string taskType=context.Request["type"];
            string sort = context.Request["sort"];
            int totalCount=0;
            List<BLLJIMP.Model.TimingTask> timingTaskList = bllTimingTask.GetTimingTaskList(rows, page, keyWord, taskType, status, out totalCount,sort);
            List<dynamic> returnList = new List<dynamic>();
            foreach (BLLJIMP.Model.TimingTask item in timingTaskList)
            {
                returnList.Add(new {
                    id=item.AutoId,
                    task_type = item.TaskTypeString,
                    task_info=item.TaskInfo,
                    create_time=item.InsertDate,
                    schedule_time= item.ScheduleDate,
                    finish_time=item.FinishDate,
                    status=item.StatusString
                });
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                rows = returnList,
                total = totalCount
            }));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TimingTask
{
    /// <summary>
    /// CancelTask 的摘要说明 取消任务
    /// </summary>
    public class CancelTask : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLTimingTask bllTimingTask = new BLLJIMP.BLLTimingTask();
        public void ProcessRequest(HttpContext context)
        {
           string ids=context.Request["ids"];
           string[] autoids = ids.Split(',');
           int count = 0;
           for (int i = 0; i < autoids.Length; i++)
           {
               BLLJIMP.Model.TimingTask model = bllTimingTask.GetTimingTask(int.Parse(autoids[i]));
               if (model == null || model.Status != 1) continue;
               model.Status = -1;
               bllTimingTask.Update(model);
               count++;
           }
           apiResp.msg = "取消了"+count+"条任务";
           apiResp.status = true;
           bllTimingTask.ContextResponse(context, apiResp);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
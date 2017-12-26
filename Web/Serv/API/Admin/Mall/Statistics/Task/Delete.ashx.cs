using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Task
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLTimingTask bll = new BLLJIMP.BLLTimingTask();
        public void ProcessRequest(HttpContext context)
        {
           
            string ids = context.Request["ids"];
            if (bll.Delete(new BLLJIMP.Model.TimingTask(),string.Format(" AutoId in({0})",ids))>0)
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.status = false;
                apiResp.msg = "删除失败";
            }
            bll.ContextResponse(context, apiResp);



        }


    }
}
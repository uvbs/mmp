using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectStatus
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string moduleType=context.Request["ModuleType"];
            string status = context.Request["Status"];
            string sort = context.Request["Sort"];
            string statusAction = context.Request["StatusAction"];
            apiResp.status = bll.AddProjectStatus(status, sort, statusAction, moduleType);
            if (apiResp.status)
            {
                apiResp.msg = "添加成功";
            }
            else
            {
                
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

    }
}
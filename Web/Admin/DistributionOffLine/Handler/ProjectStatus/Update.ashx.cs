using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectStatus
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string moduleType = context.Request["ModuleType"];
            string autoId=context.Request["AutoId"];
            string status = context.Request["Status"];
            string sort = context.Request["Sort"];
            string statusAction = context.Request["StatusAction"];
            apiResp.status = bll.UpdateProjectStatus(int.Parse(autoId), status, sort, statusAction, moduleType);
            if (apiResp.status)
            {
                apiResp.msg = "修改成功";
            }
            else
            {

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectStatus
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["autoId"];
            apiResp.status = bll.DeleteProjectStatus(autoId)>0;
            if (apiResp.status)
            {
                apiResp.msg = "删除成功";
            }
            else
            {

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }
    }
}
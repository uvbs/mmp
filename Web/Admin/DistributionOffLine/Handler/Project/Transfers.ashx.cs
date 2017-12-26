using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Project
{
    /// <summary>
    /// Transfers 的摘要说明
    /// </summary>
    public class Transfers : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string projectId=context.Request["ProjectId"];
            string msg = "";
            if (bll.Transfers(int.Parse(projectId),out msg))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = msg;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));




        }


    }
}
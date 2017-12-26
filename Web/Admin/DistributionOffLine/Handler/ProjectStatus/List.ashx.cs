using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectStatus
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string moduleType=context.Request["module_type"];
            int total = 0;
            var list = bll.QueryProjectStatusList(moduleType);
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(
                new
                {
                    total = total,
                    rows = list

                }
                ));

        }

    }
}
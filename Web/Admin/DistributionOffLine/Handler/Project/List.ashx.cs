using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.Project
{
    /// <summary>
    /// 项目列表
    /// </summary>
    public class List : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string keyWord = context.Request["keyWord"];
            string status = context.Request["status"];
            string userId=context.Request["userId"];
            string moduleType=context.Request["module_type"];
            int total = 0;
            var list = bll.QueryProjectList(pageIndex, pageSize, out total, keyWord, status, userId, "", "", moduleType);
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
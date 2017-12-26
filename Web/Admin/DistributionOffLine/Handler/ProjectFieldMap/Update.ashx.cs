using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectFieldMap
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
            string field = context.Request["Field"];
            string fieldShowName = context.Request["FieldShowName"];
            string autoId=context.Request["AutoId"];
            string isNull = context.Request["IsNull"];
            string sort = context.Request["Sort"];
            string modoleType = context.Request["ModuleType"];
            string isShowInList = context.Request["IsShowInList"];
            apiResp.status = bll.UpdateProjectFieldMap(autoId, field, fieldShowName, isNull, sort, modoleType, isShowInList);
            if (apiResp.status)
            {
                apiResp.msg = "编辑成功";
            }
            else
            {

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

    }
}
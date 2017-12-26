using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics
{
    /// <summary>
    /// Export 的摘要说明 导出
    /// </summary>
    public class Export : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 日志BLL
        /// </summary>
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string sDate=context.Request["start_date"];
            string eDate = context.Request["end_date"];

            DataTable dataTable = bllMall.QueryStatisticsData(sDate, eDate);

            DataLoadTool.ExportDataTable(dataTable, string.Format("{0}_{1}_data.xls", bllLog.GetWebsiteInfoModel().WebsiteName, DateTime.Now.ToString()));

            bllLog.Add(BLLJIMP.Enums.EnumLogType.DistributionOffLine, BLLJIMP.Enums.EnumLogTypeAction.Export, bllLog.GetCurrUserID(), "导出商城统计数据");

        }
    }
}
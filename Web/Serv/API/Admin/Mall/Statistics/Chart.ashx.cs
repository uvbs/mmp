using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics
{
    /// <summary>
    /// 商城统计图表数据
    /// </summary>
    public class Chart : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL商城
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            //string startDate = context.Request["start_date"];
            //string eDate = context.Request["stop_date"];
            //string sort = context.Request["sort"];
            int totalCount = 0;
            WebsiteInfo model = bllMall.GetWebsiteInfoModelFromDataBase();
            string monthDate = DateTime.Now.AddDays(-30).ToString();
            if (model != null && model.MallStatisticsLimitDate > 0)
            {
                monthDate = DateTime.Now.AddDays(-model.MallStatisticsLimitDate).ToString();
            }
            string  currentDate = DateTime.Now.ToString();
            List<WXMallStatistics> statisticsList = bllMall.GetWXMallStatisticsList(int.MaxValue, 1, monthDate, currentDate, " Date ", out totalCount);
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = statisticsList;
            bllMall.ContextResponse(context, apiResp);

            

        }
    }
}
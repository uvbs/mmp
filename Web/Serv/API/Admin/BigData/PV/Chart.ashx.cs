using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.BigData.PV
{
    /// <summary>
    /// Chart 的摘要说明   图表数据
    /// </summary>
    public class Chart : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 监测 BLL
        /// </summary>
        BLLJIMP.BLLMonitor bllMonitor = new BLLJIMP.BLLMonitor();
        public void ProcessRequest(HttpContext context)
        {
            string time=context.Request["time"];
            string moduleType = context.Request["module_type"];
            var list = bllMonitor.GetMonitorStatisticsList(50, time, moduleType);
            string monitorPlanIDs = "";
            if (list.Count>0) monitorPlanIDs = ZentCloud.Common.MyStringHelper.ListToStr( list.Select(p => p.MonitorPlanID).ToList(),"",",");
            var locationList = new List<string>{"北京","天津","上海","重庆","河北","河南","云南","辽宁","黑龙江","湖南","安徽","山东","新疆","江苏","浙江","江西", "湖北","广西","甘肃","山西","内蒙古","陕西",  "吉林", "福建", "贵州","广东", "青海", "西藏", "四川", "宁夏", "海南", "台湾", "香港",  "澳门"};
            List<dynamic> result = new List<dynamic>();
            int max = 0;
            foreach (var item in locationList)
            {
                int ncount = 0;
                if (list.Count > 0) {
                    ncount = bllMonitor.GetMonitorStatisticsLocationCount(time, moduleType, monitorPlanIDs, item);
                    if (ncount > max) max = ncount;
                }
                result.Add(new
                {
                    name = item,
                    value = ncount
                });
            }
            apiResp.result = new {
                max= max,
                data = result
            };
            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            bllMonitor.ContextResponse(context, apiResp);
        }
    }
}
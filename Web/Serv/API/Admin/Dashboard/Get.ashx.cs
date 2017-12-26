using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Dashboard
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLDashboard bllDashboard = new BLLDashboard();
        /// <summary>
        /// 微信
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        public void ProcessRequest(HttpContext context)
        {
           DashboardInfo nDashboardInfo = bllDashboard.GetByKey<DashboardInfo>("WebsiteOwner", bllDashboard.WebsiteOwner);
           
           if (nDashboardInfo == null)
           {
               apiResp.result = new DashboardJson();
               apiResp.msg = "没有该站点数据";
               apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
               bllDashboard.ContextResponse(context, apiResp);
               return;
           }
           JToken dashJson = JToken.Parse(nDashboardInfo.Json);
            //dashJson["uv_total"] = bllDashboard.GetDashboardUVTotal(bllDashboard.WebsiteOwner);
            //dashJson["order_total"] = bllDashboard.GetDashboardOrderTotal(bllDashboard.WebsiteOwner, "0,1,2,3");
            //dashJson["member_total"] = bllDashboard.GetDashboardRegUserTotal(bllDashboard.WebsiteOwner);
            //dashJson["fans_total"] = bllDashboard.GetDashboardSubscribeTotal(bllDashboard.WebsiteOwner);
            //dashJson["visit_total"] = bllDashboard.GetDashboardMonitorEventDetailsTotal(bllDashboard.WebsiteOwner);
            dashJson["fans_total"] = bllWeixin.GetFollowerTotalCount(bllWeixin.WebsiteOwner);
            var uvTotal = Convert.ToInt32( dashJson["uv_total"]);

            if (bllDashboard.WebsiteOwner == "comeoncloud")
            {
                dashJson["uv_total"] = uvTotal + 30000;
            }
            
           apiResp.result = dashJson;
           apiResp.msg = "查询完成";
           apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
           apiResp.status = true;
           bllDashboard.ContextResponse(context, apiResp);
        }
    }
}
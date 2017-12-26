using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine.Handler.ProjectCommission
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
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;
            string keyWord = context.Request["keyWord"];
            string userId = context.Request["userId"];
            string type=context.Request["type"];
            string fromDate=context.Request["fromDate"];
            string toDate=context.Request["toDate"];
            if (!string.IsNullOrEmpty(toDate))
            {
                toDate = (DateTime.Parse(toDate).AddHours(23).AddMinutes(59).AddSeconds(59)).ToString();
            }
            if (!string.IsNullOrEmpty(context.Request["to_date"]))
            {
                toDate=context.Request["to_date"];
            }
            int total = 0;
            var list = bll.QueryProjectCommissionList(pageIndex, pageSize, out total, keyWord,userId,type,fromDate,toDate);
            foreach (var item in list)
            {
                item.CommissionUserInfo = bllUser.GetUserInfo(item.CommissionUserId,item.WebsiteOwner);
                item.UserInfo = bllUser.GetUserInfo(item.UserId, item.WebsiteOwner);
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// ComputeReward 的摘要说明
    /// </summary>
    public class ComputeReward : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string yearmonth = context.Request["yearmonth"];
            int yearMonth = Convert.ToInt32(yearmonth);
            string websiteOwner = bll.WebsiteOwner;
            if (yearMonth == 0)
            {
                apiResp.msg = "请选择月份";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //DateTime computeDate = DateTime.ParseExact(yearmonth + "01", "yyyyMMdd", null);
            //if (computeDate.AddMonths(1) > DateTime.Now)
            //{
            //    apiResp.msg = "当前月还未结束";
            //    apiResp.code = (int)APIErrCode.IsNotFound;
            //    bll.ContextResponse(context, apiResp);
            //    return;
            //}
            string msg ="";
            apiResp.status = bll.ComputeReward(out msg, yearMonth, websiteOwner, 30);
            apiResp.msg = msg;
            apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;

            bll.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// GetWithdrawCashList 的摘要说明
    /// </summary>
    public class GetWithdrawCashList : BaseHandlerNeedLoginNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            int totalCount = 0;
            var sourceList = bll.QueryWithdrawCashList(pageIndex, pageSize, CurrentUserInfo.UserID, out totalCount, "", context.Request["type"]);
            var list = from p in sourceList
                       select new
                       {
                           id = p.AutoID,//申请编号
                           amount = p.Amount,//金额
                           status = ConvertStatus(p.Status),//状态
                           time = bll.GetTimeStamp(p.InsertDate),//时间
                           score = p.Score
                       };
            var data = new
            {
                totalcount = totalCount,
                list = list
            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = data;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="statusInt"></param>
        /// <returns></returns>
        public string ConvertStatus(int statusInt)
        {
            string status = "";
            switch (statusInt)
            {
                case 0:
                    status = "待审核";
                    break;
                case 1:
                    status = "已受理";
                    break;
                case 2:
                    status = "审核通过";
                    break;
                case 3:
                    status = "审核不通过";
                    break;
                default:
                    break;
            }
            return status;
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
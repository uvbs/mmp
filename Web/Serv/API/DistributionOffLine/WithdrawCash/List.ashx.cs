using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.WithdrawCash
{
    /// <summary>
    ///我的提现纪录
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;//页码
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;//页数
            int totalCount = 0;
            var sourceList = bll.QueryWithdrawCashList(pageIndex, pageSize, CurrentUserInfo.UserID, out totalCount, "",context.Request["type"]);
            var list = from p in sourceList
                       select new
                       {
                           id=p.AutoID,//申请编号
                           bank_account=p.BankAccount,//银行账号
                           bank_account_name=p.AccountName,//银行开户名称
                           amount=p.Amount,//金额
                           status = ConvertStatus(p.Status),//状态
                           time=bll.GetTimeStamp(p.InsertDate),//时间
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
        private string ConvertStatus(int statusInt) {
            return bll.ConvertStatus(statusInt);
        }
    }
}
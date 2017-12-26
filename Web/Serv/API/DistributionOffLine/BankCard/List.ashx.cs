using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.BankCard
{
    /// <summary>
    /// 获取银行卡列表
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 分销业务逻辑层
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDistributionOffLine = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 1;
            string keyWord = context.Request["keyword"];
            int totalCount=0;
            List<BindBankCard> bankList = bllDistributionOffLine.GetBindBankCardList(pageSize,pageIndex,keyWord,bllDistributionOffLine.GetCurrUserID(),out totalCount);
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in bankList)
            {
                returnList.Add(new 
                {
                    id=item.AutoID,
                    account_name=item.AccountName,
                    bank_account=item.BankAccount,
                    bank_name=item.BankName
                });
            }
            apiResp.result = new 
            {
                list=returnList,
                totalcount = totalCount
            };
            apiResp.status = true;
            bllDistributionOffLine.ContextResponse(context, apiResp);
        }
    }
}
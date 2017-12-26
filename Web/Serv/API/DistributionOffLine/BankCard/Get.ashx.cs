using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.BankCard
{
    /// <summary>
    /// 获银行卡详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 分销业务逻辑层
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDistributionOffLine = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string autoId= context.Request["id"];
            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.msg = "银行卡id为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            BindBankCard model = bllDistributionOffLine.GetBindBankCard(int.Parse(autoId));
            if (model.UserId != bllDistributionOffLine.GetCurrUserID())
            {
                apiResp.msg = "无权操作";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.InadequatePermissions;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            if (model == null)
            {
                apiResp.msg = "没有找到银行卡信息";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.result = new 
            {
                id = model.AutoID,
                account_name = model.AccountName,
                bank_account = model.BankAccount,
                bank_name = model.BankName,
                account_branch_name = model.AccountBranchName,
                account_branch_province = model.AccountBranchProvince,
                account_branch_city = model.AccountBranchCity,
                bank_code = model.BankCode
            };
            bllDistributionOffLine.ContextResponse(context, apiResp);
        }
    }
}
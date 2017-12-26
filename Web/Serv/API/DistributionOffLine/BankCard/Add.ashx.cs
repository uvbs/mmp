using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.BankCard
{
    /// <summary>
    /// 添加银行卡
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 分销业务逻辑层 
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDistributionOffLine = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {
                apiResp.msg = "json格式错误,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.account_name))
            {
                apiResp.msg = "开户人姓名不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.bank_account))
            {
                apiResp.msg = "银行账号不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.bank_name))
            {
                apiResp.msg = "开户人银行名称不能为空";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            BindBankCard model = new BindBankCard();
            model.AccountName = requestModel.account_name;
            model.BankAccount = requestModel.bank_account;
            model.BankName = requestModel.bank_name;
            model.AccountBranchName = requestModel.account_branch_name;
            model.AccountBranchProvince = requestModel.account_branch_province;
            model.AccountBranchCity = requestModel.account_branch_city;
            model.BankCode = requestModel.bank_code;
            model.InsertDate = DateTime.Now;
            model.UserId = bllDistributionOffLine.GetCurrUserID();
            if (bllDistributionOffLine.Add(model))
            {
                apiResp.msg = "添加银行卡成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "添加银行卡出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllDistributionOffLine.ContextResponse(context, apiResp);
        }

        public class RequestModel 
        {
            /// <summary>
            /// 开户人姓名
            /// </summary>
            public string account_name { get; set; }
            /// <summary>
            /// 银行账号
            /// </summary>
            public string bank_account { get; set; }
            /// <summary>
            /// 开户银行名称
            /// </summary>
            public string bank_name { get; set; }
            /// <summary>
            /// 开户网点
            /// </summary>
            public string account_branch_name { get; set; }
            /// <summary>
            /// 开户行省份
            /// </summary>
            public string account_branch_province { get; set; }
            /// <summary>
            /// 开户行所在市
            /// </summary>
            public string account_branch_city { get; set; }
            /// <summary>
            /// 银行代码 
            /// </summary>
            public string bank_code { get; set; }

        }

        
    }
}
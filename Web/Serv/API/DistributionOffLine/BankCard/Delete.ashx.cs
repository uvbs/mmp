using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.BankCard
{
    /// <summary>
    /// 删除银行卡
    /// </summary>
    public class Delete : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 分销业务逻辑层
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDistributionOffLine = new BLLJIMP.BLLDistributionOffLine();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            if (string.IsNullOrEmpty(id))
            {
                apiResp.msg = "ids不能为空,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            BindBankCard model=bllDistributionOffLine.GetBindBankCard(int.Parse(id));
            if (bllDistributionOffLine.GetCurrUserID() != model.UserId)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.InadequatePermissions;
                apiResp.msg = "无权操作";
                bllDistributionOffLine.ContextResponse(context, apiResp);
                return;
            }
            if (bllDistributionOffLine.Delete(model) > 0)
            {
                apiResp.msg = "删除成功";
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "删除出错";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            bllDistributionOffLine.ContextResponse(context, apiResp);
        }
    }
}
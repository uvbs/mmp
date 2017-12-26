using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TransfersAudit
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLTransfersAudit bll = new BLLJIMP.BLLTransfersAudit();
        public void ProcessRequest(HttpContext context)
        {
            if (!bll.IsTransfersAuditPer(CurrentUserInfo))
            {
                apiResp.status = false;
                apiResp.msg = "您没有审核权限";
                bll.ContextResponse(context, apiResp);
                return;
            }
            var sourceData = bll.GetByTranId(context.Request["id"]);
            apiResp.status = true;
            apiResp.result = new
            {
                id = sourceData.AutoId,
                tran_info = sourceData.TranInfo,
                type = sourceData.Type,
                amount = sourceData.Amount,
                status = sourceData.Status,
                insert_date = sourceData.InsertDate.ToString()


            };
            bll.ContextResponse(context, apiResp);


        }


    }
}
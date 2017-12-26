using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// UpdateStatus 的摘要说明
    /// </summary>
    public class UpdateStatus : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string settlementId = context.Request["settlement_id"];
            string remark = context.Request["remark"];
            string img = context.Request["img"];
            string status = context.Request["status"];

            if (status=="供应商已确认"&&currentUserInfo.UserType!=7)
            {
               apiResp.msg = "只有商户才可以确认";
               bllMall.ContextResponse(context, apiResp);
               return;
            }


            if (bllMall.UpdateSupplierSettlement(settlementId, status, remark, img))
            {
                apiResp.status = true;
                apiResp.msg = "操作成功";
            }
            else
            {
                apiResp.msg = "操作失败";
            }
            bllMall.ContextResponse(context, apiResp);

        }
    }
}
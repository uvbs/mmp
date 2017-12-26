using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Settlement.Supplier
{
    /// <summary>
    /// UpdateIsInvoice 的摘要说明
    /// </summary>
    public class UpdateIsInvoice : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string isInvoice = context.Request["is_invoice"];
            if (bllMall.Update(new SupplierSettlement(), string.Format("IsInvoice={0}",isInvoice),string.Format("AutoId={0}",id)) > 0)
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
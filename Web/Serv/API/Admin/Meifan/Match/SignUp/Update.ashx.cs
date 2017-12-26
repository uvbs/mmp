using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Match.SignUp
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string orderId = context.Request["id"];
            string remark = context.Request["remark"];
            var data = bll.GetActivityDataByOrderId(orderId);
            data.Remarks = remark;
            if (bll.Update(new ActivityDataInfo(), string.Format(" Remarks='{0}'", remark), string.Format(" OrderId='{0}'", orderId)) > 0)
            {

                apiResp.status = true;
                apiResp.msg = "ok";

            }
            else
            {
                apiResp.msg = "操作失败";
            }

            bll.ContextResponse(context, apiResp);


        }

    }
}
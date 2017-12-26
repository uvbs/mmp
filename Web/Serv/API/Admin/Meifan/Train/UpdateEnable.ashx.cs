using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Train
{
    /// <summary>
    /// UpdateEnable 的摘要说明
    /// </summary>
    public class UpdateEnable : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string activityIds = context.Request["activity_ids"];
            string isEnable = context.Request["is_enable"];
            if (bll.Update(new JuActivityInfo(), string.Format(" IsPublish={0}", isEnable), string.Format(" JuActivityId in({0})", activityIds)) > 0)
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
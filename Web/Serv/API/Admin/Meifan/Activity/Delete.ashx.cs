using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Activity
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {


        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string activityIds = context.Request["activity_ids"];
            if (bll.Update(new JuActivityInfo(), string.Format(" IsDelete=1"), string.Format(" ActivityId in({0})", activityIds)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = "删除失败";
            }

            bll.ContextResponse(context, apiResp);


        }


       
    }
}
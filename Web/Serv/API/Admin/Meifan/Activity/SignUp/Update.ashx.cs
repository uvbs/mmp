using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Activity.SignUp
{
    /// <summary>
    /// 更新报名数据
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string activityId = context.Request["activity_id"];
            string remark=context.Request["remark"];
            if (bll.Update(new ActivityDataInfo(), string.Format(" Remarks='{0}'",remark), string.Format(" ActivityId ='{0}'", activityId)) > 0)
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
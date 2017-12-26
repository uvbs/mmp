using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Template
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            if (bll.DeleteByKey<ComponentTemplate>("AutoId", id)>0)
            {
                apiResp.msg = "删除模板成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "删除模板失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Supplier
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        public void ProcessRequest(HttpContext context)
        {

            string ids = context.Request["ids"];

            string msg = "";
            apiResp.status = bllUser.DeleteSupplier(ids);
            if (!apiResp.status)
            {
                msg = "删除失败";
            }
            apiResp.msg = msg;
            bllUser.ContextResponse(context, apiResp);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Forward
{
    /// <summary>
    /// Delete 的摘要说明 删除微问卷
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL 微转发
        /// </summary>
        BLLJIMP.BLLActivityForwardInfo bllForward = new BLLJIMP.BLLActivityForwardInfo();
        public void ProcessRequest(HttpContext context)
        {
            string ids=context.Request["ids"];

            string[] arrayIds = ids.Split(',');

            foreach (var item in arrayIds)
            {
                ActivityForwardInfo model = bllForward.GetActivityForwardInfo(item);

                if (model == null) continue;

                bllForward.Delete(model);
            }

            apiResp.msg = "删除了"+arrayIds.Length+"条数据!";
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }

      
    }
}
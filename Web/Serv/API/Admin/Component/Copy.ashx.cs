using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// Copy 的摘要说明
    /// </summary>
    public class Copy : BaseHandlerNeedLoginAdminNoAction
    {
        BLL bll = new BLL();
        public void ProcessRequest(HttpContext context)
        {
            string componentId = context.Request["component_id"];
            BLLJIMP.Model.Component model = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, componentId));
            model.AutoId = 0;
            model.ComponentName = model.ComponentName + "--复制";
            model.ComponentKey = null;
            if (!string.IsNullOrWhiteSpace(model.ComponentKey)) model.ComponentKey = "";
            if (bll.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "复制出错";
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
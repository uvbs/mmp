using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// Delete 的摘要说明  删除组件
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            string componentId = context.Request["component_id"];
            if (string.IsNullOrEmpty(componentId))
            {
                apiResp.msg = "component_id 为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }

            BLLJIMP.Model.Component model = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, componentId));
            if (model == null)
            {
                apiResp.msg = "没有找到";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(model.ComponentKey))
            {
                apiResp.msg = "禁止删除";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            
            if (bll.Delete(new BLLJIMP.Model.Component(), string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, componentId)) > 0)
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "删除出错";
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
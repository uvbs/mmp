using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// Get 的摘要说明  获取组件配置详情
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
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
                apiResp.msg = "没有找到组件";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.result = new 
            {
                component_id = model.AutoId,
                component_key = model.ComponentKey,
                component_name = model.ComponentName,
                component_model_id = model.ComponentModelId,
                child_component_ids = model.ChildComponentIds,
                component_config = model.ComponentConfig,
                decription = model.Decription,
                is_oauth = model.IsWXSeniorOAuth,
                access_level = model.AccessLevel
            };
            bll.ContextResponse(context, apiResp);
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Component
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
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
            if (!string.IsNullOrWhiteSpace(model.ComponentConfig))
            {
                apiResp.result = JToken.Parse(model.ComponentConfig);
                //apiResp.result = ZentCloud.Common.JSONHelper.ObjectToJson(model.ComponentConfig);
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}
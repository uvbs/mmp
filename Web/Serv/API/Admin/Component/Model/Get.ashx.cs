using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Model
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        List<string> limitControls = BLLComponent.limitControls;
        public void ProcessRequest(HttpContext context)
        {
            string component_model_id = context.Request["component_model_id"];
            string edit_model = context.Request["edit_model"];
            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", component_model_id);
            if (componentModel == null)
            {
                apiResp.msg = "组件库未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            List<ComponentModelField> componentModelFields = bll.GetListByKey<ComponentModelField>("ComponentModelKey", componentModel.ComponentModelKey)
                .OrderBy(p=>p.ComponentFieldSort).ToList();
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                component_model_id = componentModel.AutoId,
                component_model_key = componentModel.ComponentModelKey,
                component_model_name = componentModel.ComponentModelName,
                component_model_type = componentModel.ComponentModelType,
                component_model_link_url = componentModel.ComponentModelLinkUrl,
                component_model_html_url = componentModel.ComponentModelHtmlUrl,
                is_delete = componentModel.IsDelete,
                component_model_fields = (from p in componentModelFields.Where(p =>  edit_model =="1" || ( p.ComponentFieldType >= 4 && p.ComponentFieldType != 8 && limitControls.Contains(p.ComponentField)))
                                          select new
                              {
                                  component_field_id = p.AutoId,
                                  component_field = p.ComponentField,
                                  component_field_name = p.ComponentFieldName,
                                  component_field_type = p.ComponentFieldType,
                                  component_field_data_type = p.ComponentFieldType,
                                  component_field_data_value = p.ComponentFieldDataValue,
                                  disabled = false
                              })
            };
            bll.ContextResponse(context, apiResp);
        }

    }
}
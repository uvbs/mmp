using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLComponent bll = new BLLJIMP.BLLComponent();
        BLLSlide bllSlide = new BLLSlide();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = bll.ConvertRequestToModel<RequestModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = ex.Message;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.component_id <= 0)
            {
                apiResp.msg = "请传入页面id";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.component_name))
            {
                apiResp.msg = "请输入页面名称";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.component_model_id==0)
            {
                apiResp.msg = "请选择模板";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.Component model = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, requestModel.component_id));

            if (!string.IsNullOrWhiteSpace(requestModel.component_key))
            {
                var oComp = bll.GetComponentByKey(requestModel.component_key, bll.WebsiteOwner);
                if (oComp != null && oComp.AutoId != model.AutoId)
                {
                    apiResp.msg = "标识已被使用";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            model.AutoId = requestModel.component_id;
            model.ComponentKey = requestModel.component_key;
            model.ChildComponentIds = requestModel.child_component_ids;
            model.Decription = requestModel.decription;
            int old_model_id = model.ComponentModelId;
            model.ComponentModelId = requestModel.component_model_id;
            model.ComponentTemplateId = requestModel.component_template_id;
            model.ComponentName = requestModel.component_name;
            model.IsWXSeniorOAuth = requestModel.is_oauth;
            model.AccessLevel = requestModel.access_level;
            model.IsInitData = requestModel.is_init_data;

            if (old_model_id != requestModel.component_model_id)
            {
                ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", requestModel.component_model_id.ToString());
                if (componentModel == null)
                {
                    apiResp.msg = "新组件库未找到";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                model.ComponentType = componentModel.ComponentModelType;
                //if (model.ComponentType != "page") model.WebsiteOwner = "Common";
            }
            model.ComponentConfig = requestModel.component_config;
            if (bll.Update(model))
            {
                //更新幻灯片
                if (!string.IsNullOrWhiteSpace(requestModel.slides))
                {
                    Add.UpdateSlide(requestModel.slides);
                }
                //更新导航
                if (!string.IsNullOrWhiteSpace(requestModel.toolbars))
                {
                    Add.UpdateToolbar(requestModel.toolbars);
                }

                apiResp.result = new
                {
                    component_id = model.AutoId
                };
                apiResp.status = true;
                apiResp.msg = "修改完成";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "修改页面出错";
            }
            bll.ContextResponse(context, apiResp);
        }
        public class RequestModel
        {
            /// <summary>
            /// 页面id
            /// </summary>
            public int component_id { get; set; }
            /// <summary>
            /// 页面标识
            /// </summary>
            public string component_key { get; set; }

            /// <summary>
            /// 组件库ID
            /// </summary>
            public int component_model_id { get; set; }
            /// <summary>
            /// 模板ID
            /// </summary>
            public int component_template_id { get; set; }
            /// <summary>
            /// 页面名称
            /// </summary>
            public string component_name { get; set; }


            /// <summary>
            /// 子页面
            /// </summary>
            public string child_component_ids { get; set; }

            /// <summary>
            /// 页面配置
            /// </summary>
            public string component_config { get; set; }

            /// <summary>
            ///描述
            /// </summary>
            public string decription { get; set; }
            /// <summary>
            /// 是否微信高级授权
            /// </summary>
            public int is_oauth { get; set; }
            /// <summary>
            /// 访问等级
            /// </summary>
            public int access_level { get; set; }
            /// <summary>
            ///幻灯片
            /// </summary>
            public string slides { get; set; }
            /// <summary>
            ///导航
            /// </summary>
            public string toolbars { get; set; }
            /// <summary>
            /// 是否初始化数据
            /// </summary>
            public int is_init_data { get; set; }
        }
    }
}
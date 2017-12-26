using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component.Model
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = JSONHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception ex)
            {
                apiResp.msg = "json格式错误,请检查。";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestModel.component_model_name))
            {
                apiResp.msg = "请录入组件库名称";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.component_model_fields.Count == 0)
            {
                apiResp.msg = "请录入组件库变量字段";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            for (int i = 0; i < requestModel.component_model_fields.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(requestModel.component_model_fields[i].component_field))
                {
                    apiResp.msg = "请完善字段Key";
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            ComponentModel componentModel = new ComponentModel();
            componentModel.ComponentModelKey = string.Format("Model{0}{1}", DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now), Rand.Str(5));
            componentModel.ComponentModelName = requestModel.component_model_name;
            componentModel.ComponentModelType = requestModel.component_model_type;
            componentModel.ComponentModelLinkUrl = requestModel.component_model_link_url;
            componentModel.ComponentModelHtmlUrl = requestModel.component_model_html_url;
            componentModel.IsDelete = requestModel.is_delete;

            List<ComponentModelField> componentModelFields = new List<ComponentModelField>();
            foreach (var item in requestModel.component_model_fields)
            {
                ComponentModelField temp = new ComponentModelField();
                temp.ComponentModelKey = componentModel.ComponentModelKey;
                temp.ComponentField = item.component_field;
                temp.ComponentFieldName = item.component_field_name;
                temp.ComponentFieldType = item.component_field_type;
                temp.ComponentFieldDataType = item.component_field_data_type;
                temp.ComponentFieldDataValue = item.component_field_data_value;
                temp.ComponentFieldSort = item.component_field_sort;
                componentModelFields.Add(temp);
            }
            string errmsg = string.Empty;
            if (bll.AddComponentModel(componentModel, componentModelFields, out errmsg))
            {
                apiResp.status = true;
                apiResp.msg = "添加完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = errmsg;
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
        public class RequestModel
        {
            /// <summary>
            /// 组件库名称
            /// </summary>
            public string component_model_name { get; set; }
            /// <summary>
            /// 组件库分类
            /// </summary>
            public string component_model_type { get; set; }
            /// <summary>
            /// 页面访问链接
            /// </summary>
            public string component_model_link_url { get; set; }
            /// <summary>
            /// 页面相对路径
            /// </summary>
            public string component_model_html_url { get; set; }
            /// <summary>
            /// 是否作废
            /// </summary>
            public int is_delete { get; set; }
            /// <summary>
            /// 字段列表
            /// </summary>
            public List<RequestFieldModel> component_model_fields { get; set; }

        }
        
        public class RequestFieldModel
        {
            /// <summary>
            /// 字段Key
            /// </summary>
            public string component_field { get; set; }

            /// <summary>
            /// 字段名称
            /// </summary>
            public string component_field_name { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public int component_field_type { get; set; }

            /// <summary>
            /// 数据来源
            /// </summary>
            public int component_field_data_type { get; set; }

            /// <summary>
            /// 数据 
            /// </summary>
            public string component_field_data_value { get; set; }
            /// <summary>
            /// 排序 
            /// </summary>
            public int component_field_sort { get; set; }
        }
    }
}
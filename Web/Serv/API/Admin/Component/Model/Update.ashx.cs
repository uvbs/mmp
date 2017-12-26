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
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
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
                apiResp.code = (int)APIErrCode.IsNotFound;
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

            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", requestModel.component_model_id.ToString());
            if (componentModel == null)
            {
                apiResp.msg = "原组件库未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            componentModel.ComponentModelName = requestModel.component_model_name;
            componentModel.ComponentModelType = requestModel.component_model_type;
            componentModel.ComponentModelLinkUrl = requestModel.component_model_link_url;
            componentModel.ComponentModelHtmlUrl = requestModel.component_model_html_url;
            componentModel.IsDelete = requestModel.is_delete;
            
            List<ComponentModelField> OldList = bll.GetListByKey<ComponentModelField>("ComponentModelKey", componentModel.ComponentModelKey);
            List<ComponentModelField> AddComponentModelFields = new List<ComponentModelField>();
            List<ComponentModelField> UpdateComponentModelFields = new List<ComponentModelField>();
            List<ComponentModelField> DeleteComponentModelFields = new List<ComponentModelField>();

            foreach (var item in requestModel.component_model_fields.Where(p=>p.component_field_id==0))
            {
                ComponentModelField temp = new ComponentModelField();
                temp.ComponentModelKey = componentModel.ComponentModelKey;
                temp.ComponentField = item.component_field;
                temp.ComponentFieldName = item.component_field_name;
                temp.ComponentFieldType = item.component_field_type;
                temp.ComponentFieldDataType = item.component_field_data_type;
                temp.ComponentFieldDataValue = item.component_field_data_value;
                temp.ComponentFieldSort = item.component_field_sort;
                AddComponentModelFields.Add(temp);
            }
            foreach (var item in OldList)
            {
                RequestFieldModel nField = requestModel.component_model_fields.FirstOrDefault(p => p.component_field_id == item.AutoId);
                if (nField == null)
                {
                    DeleteComponentModelFields.Add(item);
                }
                else
                {
                    item.ComponentField = nField.component_field;
                    item.ComponentFieldName = nField.component_field_name;
                    item.ComponentFieldType = nField.component_field_type;
                    item.ComponentFieldDataType = nField.component_field_data_type;
                    item.ComponentFieldDataValue = nField.component_field_data_value;
                    item.ComponentFieldSort = nField.component_field_sort;
                    UpdateComponentModelFields.Add(item);
                }
            }

            string errmsg = string.Empty;
            if (bll.UpdateComponentModel(componentModel, AddComponentModelFields, UpdateComponentModelFields
                , DeleteComponentModelFields, out errmsg))
            {
                apiResp.status = true;
                apiResp.msg = "修改完成";
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
            /// 组件库id
            /// </summary>
            public int component_model_id { get; set; }
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
            /// 字段id
            /// </summary>
            public int component_field_id { get; set; }
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
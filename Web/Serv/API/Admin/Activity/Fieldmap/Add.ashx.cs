using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Fieldmap
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {

            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_id))
            {
                resp.errmsg = "activity_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            if (string.IsNullOrEmpty(requestModel.field_name))
            {
                resp.errmsg = "field_name 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(requestModel.activity_id), false);
            if (juActivity == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该活动";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            int fieldIndex = 1001;
            ActivityFieldMappingInfo fieMapModel = bll.Get<ActivityFieldMappingInfo>(string.Format(string.Format(" ActivityID={0} Order by ExFieldIndex Desc", juActivity.SignUpActivityID)));
            if (fieMapModel != null)
            {
                fieldIndex = fieMapModel.ExFieldIndex + 1;
            }
            ActivityFieldMappingInfo fieldMap = new ActivityFieldMappingInfo();
            fieldMap.ActivityID = juActivity.SignUpActivityID;
            fieldMap.ExFieldIndex = fieldIndex;
            fieldMap.MappingName = requestModel.field_name;
            fieldMap.FieldIsNull = requestModel.field_null;
            fieldMap.FormatValiFunc = requestModel.field_format_vali_func;
            fieldMap.FormatValiExpression = requestModel.field_format_vali_expression;
            fieldMap.FieldType = requestModel.field_type;
            fieldMap.IsMultiline = requestModel.field_multiline;
            if (bll.Add(fieldMap))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "添加出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }

        public class RequestModel
        {
            /// <summary>
            /// 活动id
            /// </summary>
            public string activity_id { get; set; }


            /// <summary>
            /// 显示名称
            /// </summary>
            public string field_name { get; set; }

            /// <summary>
            ///是否为空
            /// </summary>
            public int field_null { get; set; }


            /// <summary>
            /// 格式验证表达式
            /// </summary>
            public string field_format_vali_expression { get; set; }

            /// <summary>
            ///数据格式验证方法：邮箱、手机、表达式
            /// </summary>
            public string field_format_vali_func { get; set; }


            /// <summary>
            /// 字段类型  1.微信推广字段 0或null为普通字段
            /// </summary>
            public int field_type { get; set; }

            /// <summary>
            /// 是否多行
            /// </summary>
            public int field_multiline { get; set; }

        }
    }
}
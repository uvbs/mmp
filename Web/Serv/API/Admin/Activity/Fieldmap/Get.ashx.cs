using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Fieldmap
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string activityId=context.Request["activity_id"];
            string fieldIndex=context.Request["field_index"];
            if (string.IsNullOrEmpty(activityId))
            {
                resp.errmsg = "activity_id 为必填,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(fieldIndex))
            {
                resp.errmsg = "field_index 为必填,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId), false);
            if (juActivity == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该活动";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }


            ActivityFieldMappingInfo fieldMap = bll.Get<ActivityFieldMappingInfo>(string.Format(" ActivityID={0} AND ExFieldIndex={1}", juActivity.SignUpActivityID, int.Parse(fieldIndex)));
            if (fieldMap == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该条报名字段";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            resp.isSuccess = true;
            resp.returnObj=new 
            {
                activity_id = fieldMap.ActivityID,
                field_name = fieldMap.FieldName,
                field_index = fieldMap.ExFieldIndex,
                field_null = fieldMap.FieldIsNull,
                field_default = fieldMap.FieldIsDefauld,
                field_type = fieldMap.FieldType,
                field_format_vali_func = fieldMap.FormatValiFunc
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        public class RequestModel
        {
            /// <summary>
            /// 活动id
            /// </summary>
            public string activity_id { get; set; }

            /// <summary>
            /// 字段名称
            /// </summary>
            public int field_index { get; set; }

            /// <summary>
            /// 显示名称
            /// </summary>
            public string field_name { get; set; }

            /// <summary>
            /// 是否为默认  
            /// </summary>
            public int field_default { get; set; }

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
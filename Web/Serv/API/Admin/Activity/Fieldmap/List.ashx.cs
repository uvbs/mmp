using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Fieldmap
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string activityId = context.Request["activity_id"];
                if (string.IsNullOrEmpty(activityId))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "activity_id 为必填项,请检查";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId),false);
                if (juActivity == null)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "没有找到该活动";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" ActivityID in ({0}) ", juActivity.SignUpActivityID));

                int totalCount = bllJuActivity.GetCount<ActivityFieldMappingInfo>(sbWhere.ToString());

                var list = bllActivity.GetActivityFieldMappingList(juActivity.SignUpActivityID);

                resp.isSuccess = true;

                List<dynamic> returnList = new List<dynamic>();

                foreach (var item in list)
                {
                    returnList.Add(new
                        {
                            activity_id=item.ActivityID,
                            field_index="K"+item.ExFieldIndex,
                            field_name = item.MappingName,
                            field_null=item.FieldIsNull,
                            field_type=item.FieldType,
                            field_default=item.FieldIsDefauld,
                            field_format_valifunc=item.FormatValiFunc
                        });
                }

                resp.returnObj = new
                {
                    totalcount = totalCount,
                    list = returnList
                };
            }
            catch (Exception ex)
            {

                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
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
            /// 字段名称
            /// </summary>
            public int field_index { get; set; }

            /// <summary>
            /// 显示名称
            /// </summary>
            public string field_name { get; set; }

            /// <summary>
            ///是否为空
            /// </summary>
            public int field_null { get; set; }

            /// <summary>
            /// 字段类型  1.微信推广字段 0或null为普通字段
            /// </summary>
            public int field_type { get; set; }

            /// <summary>
            /// 是否为默认  
            /// </summary>
            public int field_default { get; set; }

            /// <summary>
            ///数据格式验证方法：邮箱、手机、表达式
            /// </summary>
            public string field_format_valifunc { get; set; }

        }



    }
}
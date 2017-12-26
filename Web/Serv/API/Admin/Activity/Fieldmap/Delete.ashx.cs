using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.Fieldmap
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string activityId = context.Request["activity_id"];
            string fieldIndex = context.Request["field_index"];
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
            JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId),false);
            if (juActivity == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该活动";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ActivityFieldMappingInfo fieldMap = bll.Get<ActivityFieldMappingInfo>(string.Format(" ActivityID={0} AND ExFieldIndex={1}", juActivity.SignUpActivityID, fieldIndex));
            if (fieldMap == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                resp.errmsg = "没有找到该条记录";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bll.Delete(new ActivityFieldMappingInfo(), string.Format(" ActivityID={0} AND ExFieldIndex={1}", juActivity.SignUpActivityID, fieldIndex)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "删除出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}
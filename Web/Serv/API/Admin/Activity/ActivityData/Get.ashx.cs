using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.ActivityData
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        //BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        //BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            //string activityId = context.Request["activity_id"];
            //string activityUid = context.Request["activity_uid"];
            //if(string.IsNullOrEmpty(activityId)||string.IsNullOrEmpty(activityUid))
            //{
            //    resp.errmsg = "activity_id和activity_uid 为必填项,请检查";
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId),false);

            //ActivityDataInfo activityData = bllActivity.GetActivityDataInfo(juActivity.SignUpActivityID, int.Parse(activityUid));



        }
    }
}
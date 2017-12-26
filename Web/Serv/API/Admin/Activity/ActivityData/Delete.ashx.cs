using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity.ActivityData
{
    /// <summary>
    /// Delete 的摘要说明
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            string activityId = context.Request["activity_id"];
            string activityUids = context.Request["activity_uids"];
            if (string.IsNullOrEmpty(activityId))
            {
                resp.errmsg = "activity_id 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(activityUids))
            {
                resp.errmsg = "activityUid 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            JuActivityInfo juActivity = bllJuActivity.GetJuActivity(int.Parse(activityId),false);
            if (juActivity == null)
            {
                resp.errmsg = "没有找到该条活动";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllActivity.Delete(new ActivityDataInfo(), string.Format(" WebsiteOwner='{0}' AND isDelete=0 AND ActivityID='{1}' AND UID in ({2})", bllJuActivity.WebsiteOwner, juActivity.SignUpActivityID, activityUids)) == activityUids.Split(',').Length)
            {
                bllJuActivity.UpSignUpTotalCount(juActivity.JuActivityID);//更新 报名人数
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "删除报名数据出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


    }
}
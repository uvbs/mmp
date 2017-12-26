using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// UpdateLastLocation 的摘要说明   
    /// </summary>
    public class UpdateLastLocation :BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string lastLocation = context.Request["city"];
            if (string.IsNullOrEmpty(longitude))
            {
                resp.errmsg = "longitude 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(latitude))
            {
                resp.errmsg = "latitude 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(lastLocation))
            {
                resp.errmsg = "lastLocation 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllUser.Update(CurrentUserInfo, string.Format(" LastLoginLatitude='{0}',LastLoginLongitude='{1}',LastLoginCity='{2}' ", latitude,longitude,lastLocation), string.Format(" WebsiteOwner='{0}' AND AutoID={1}",bllUser.WebsiteOwner,CurrentUserInfo.AutoID)) > 0)
            {
                resp.isSuccess = true;
                resp.errmsg = "ok";
            }
            else
            {
                resp.errmsg = "出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}
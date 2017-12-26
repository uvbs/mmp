using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SignIn
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class AddAuto : BaseHandlerNeedLoginNoAction
    {
        BLLSignIn bllSignIn = new BLLSignIn();
        public void ProcessRequest(HttpContext context)
        {
            double longitude = Convert.ToDouble(context.Request["longitude"]);
            double latitude = Convert.ToDouble(context.Request["latitude"]);
            int addressId =Convert.ToInt32(context.Request["address_id"]);
            string errmsg = string.Empty;
            if (bllSignIn.AddSignInLog(addressId, CurrentUserInfo.UserID, longitude, latitude, bllSignIn.WebsiteOwner, out errmsg))
            {
                apiResp.status = true;
                apiResp.msg = "签到成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = errmsg;
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllSignIn.ContextResponse(context, apiResp);
        }
    }
}
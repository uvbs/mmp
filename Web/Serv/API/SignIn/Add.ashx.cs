using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.SignIn
{
    /// <summary>
    /// LBS签到
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLSignIn bllSignIn = new BLLSignIn();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int address_id = Convert.ToInt32(context.Request["address_id"]);
            double longitude = Convert.ToDouble(context.Request["longitude"]);
            double latitude = Convert.ToDouble(context.Request["latitude"]);
            string errmsg = string.Empty;
            if (bllSignIn.AddSignInLog(address_id, CurrentUserInfo.UserID, longitude, latitude, bllSignIn.WebsiteOwner, out errmsg))
            {
                bllUser.AddUserScoreDetail(CurrentUserInfo.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.LBSSignIn), CurrentUserInfo.WebsiteOwner, null, null);
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
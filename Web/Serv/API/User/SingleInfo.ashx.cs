using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 简单信息
    /// </summary>
    public class SingleInfo : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            apiResp.msg ="查询完成";
            apiResp.status=true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = new
            {
                id = CurrentUserInfo.AutoID,
                username = CurrentUserInfo.UserID,
                nick_name = bllUser.GetUserDispalyName(CurrentUserInfo),//CurrentUserInfo.WXNickname,
                avatar = bllUser.GetUserDispalyAvatar(CurrentUserInfo),
                birthday = CurrentUserInfo.Birthday,
                gender = CurrentUserInfo.Gender,
                credit_acount = CurrentUserInfo.CreditAcount,
                totalscore = CurrentUserInfo.TotalScore,
                totalamount = CurrentUserInfo.TotalAmount,
                level = CurrentUserInfo.MemberLevel
            };
            bllUser.ContextResponse(context, apiResp);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// bindWXOpenId 的摘要说明
    /// </summary>
    public class BindWXOpenId : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrWhiteSpace(CurrentUserInfo.WXOpenId))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该会员已绑定微信";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (context.Session["currWXOpenId"] == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "微信OpendId未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            string openId = context.Session["currWXOpenId"].ToString();
            string websiteOwner = bllUser.WebsiteOwner;

            UserInfo oUser = bllUser.GetUserInfoByOpenId(openId, websiteOwner);
            if (oUser != null && oUser.AutoID != CurrentUserInfo.AutoID)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该微信已绑定其他会员";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if(bllUser.Update(CurrentUserInfo,string.Format("WXOpenId='{0}'",openId),
                string.Format("WebsiteOwner='{0}' And AutoID={1}", websiteOwner, CurrentUserInfo.AutoID)) > 0)
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "微信绑定成功";
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "微信绑定失败";
            }
            bllUser.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
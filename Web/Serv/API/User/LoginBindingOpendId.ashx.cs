using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// LoginBindOpendId 的摘要说明
    /// </summary>
    public class LoginBindingOpendId : BaseHandlerRequiresSessionNoAction
    {
        
        BLLUser bll = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Expires = 0;
            string openId = "";
            if (context.Session["currWXOpenId"] != null){
                openId = context.Session["currWXOpenId"].ToString();
            }
            string phone = context.Request["phone"];
            string password = context.Request["password"];
            string limitmember = context.Request["limitmember"];
            string websiteOwner = bll.WebsiteOwner;
            UserInfo loginUser = new UserInfo();
            string msg = "";
            if (!bll.Login(phone, password, out loginUser, out msg, websiteOwner))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = msg;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (limitmember == "1" && loginUser.MemberLevel < 10)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "仅支持会员登录";
                bll.ContextResponse(context, apiResp);
                return;
            }
            //登录不绑定OpenId

            //if (!string.IsNullOrWhiteSpace(openId))
            //{
            //    if (!string.IsNullOrWhiteSpace(loginUser.WXOpenId) &&
            //        loginUser.WXOpenId != openId)
            //    {
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "该会员已绑定其他微信";
            //        bll.ContextResponse(context, apiResp);
            //        return;
            //    }
            //    UserInfo opUser = bll.GetUserInfoByOpenId(openId,websiteOwner);
            //    if (opUser != null && opUser.AutoID != loginUser.AutoID)
            //    {
            //        context.Session[SessionKey.UserID] = loginUser.UserID;
            //        context.Session[SessionKey.LoginStatu] = 1;
            //        bll.AddLoginLogs(loginUser.UserID);
            //        bll.UpdateLastLoginInfo(loginUser.UserID, websiteOwner: websiteOwner);
            //        apiResp.code = (int)APIErrCode.IsSuccess;

            //        apiResp.status = true;
            //        apiResp.msg = "登录成功";
            //        return;
            //    }


            //    if (bll.Update(loginUser, string.Format("WXOpenId='{0}'", openId), string.Format(" AutoID = {0} And WebsiteOwner='{1}'", loginUser.AutoID, websiteOwner)) > 0)
            //    {
            //        context.Session[SessionKey.UserID] = loginUser.UserID;
            //        context.Session[SessionKey.LoginStatu] = 1;
            //        context.Response.Cookies.Add(bll.CreateLoginCookie(loginUser.UserID));

            //        bll.AddLoginLogs(loginUser.UserID);
            //        bll.UpdateLastLoginInfo(loginUser.UserID, websiteOwner:websiteOwner);

            //        apiResp.code = (int)APIErrCode.IsSuccess;
            //        apiResp.status = true;
            //        apiResp.msg = "登录成功";
            //    }
            //    else
            //    {
            //        apiResp.code = (int)APIErrCode.OperateFail;
            //        apiResp.msg = "绑定微信号出错";
            //    }
            //}
            //else
            //{
                context.Session[SessionKey.UserID] = loginUser.UserID;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Response.Cookies.Add(bll.CreateLoginCookie(loginUser.UserID, loginUser.WXOpenId, loginUser.WXNickname));

                bll.AddLoginLogs(loginUser.UserID);
                bll.UpdateLastLoginInfo(loginUser.UserID, websiteOwner: websiteOwner);

                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
                apiResp.msg = "登录成功";
            //}
            bll.ContextResponse(context, apiResp);
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
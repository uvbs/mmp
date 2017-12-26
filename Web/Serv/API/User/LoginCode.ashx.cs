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
    /// 登录
    /// </summary>
    public class LoginCode : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// Api响应模型
        /// </summary>
        BaseResponse apiResp = new BaseResponse();
        /// <summary>
        /// 用户处理逻辑BLL
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            string userId = context.Request["username"];
            string passWord = context.Request["password"];

            if (string.IsNullOrEmpty(userId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "手机或账号不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(passWord))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "密码不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string checkCode = context.Request["checkcode"];
            if (string.IsNullOrEmpty(checkCode))
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码不能为空";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            object serverCheckCode = context.Session["CheckCode"];
            if (serverCheckCode == null)
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码已过期";
                bllUser.ContextResponse(context, apiResp);
                return;
            }

            if (string.IsNullOrWhiteSpace(checkCode) || 
                !checkCode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                apiResp.code = (int)APIErrCode.CheckCodeErr;
                apiResp.msg = "验证码错误";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo userInfo;
            string msg = "";
            if (bllUser.Login(userId, passWord, out userInfo, out msg, bllUser.WebsiteOwner))
            {
                if (userInfo.WebsiteOwner != bllUser.WebsiteOwner)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "登录失败";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }

                if (context.Session["currWXOpenId"]!=null)
                {
                    bllUser.UpdateUserWxOpenId(userInfo.UserID, context.Session["currWXOpenId"].ToString(), bllUser.WebsiteOwner);//更新用户openid
                }
                bllUser.UpdateLastLoginInfo(userInfo.UserID, context.Request["city"]
                    , context.Request["longitude"], context.Request["latitude"]);

                context.Session[SessionKey.UserID] = userInfo.UserID;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Response.Cookies.Add(bllUser.CreateLoginCookie(userInfo.UserID, userInfo.WXOpenId, userInfo.WXNickname));

                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "登录成功";
                apiResp.status = true;
                apiResp.result = new{
                    session_id = context.Session.SessionID
                };
            }
            else
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = msg;
            }
            bllUser.ContextResponse(context, apiResp);
        }

        /// <summary>
        /// 默认响应模型
        /// </summary>
        public class DefaultResponse
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }

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
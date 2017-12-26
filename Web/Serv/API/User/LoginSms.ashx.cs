using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 手机验证码
    /// </summary>
    public class LoginSms : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 用户处理逻辑BLL
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            string phone = context.Request["phone"];//手机号 
            string smsVerCode = context.Request["sms_vercode"];//短信验证码
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "手机号码不能为空";
                goto outoff;

            }
            if (string.IsNullOrEmpty(smsVerCode))
            {
                resp.errcode = 1;
                resp.errmsg = "验证码不能为空";
                goto outoff;
            }
            var userInfo = bllUser.GetUserInfoByPhone(phone);
            if (userInfo == null)
            {
                resp.errcode = 1;
                resp.errmsg = "手机号尚未注册,请先注册";
                goto outoff;
            }

            string msg = "";
            if (bllUser.SmsLogin(phone, smsVerCode, out msg))
            {
                bllUser.UpdateLastLoginInfo(userInfo.UserID, context.Request["city"]
                    , context.Request["longitude"], context.Request["latitude"]);
                context.Session[SessionKey.UserID] = userInfo.UserID;
                var data = new
                {
                    errcode = 0,
                    errmsg = "ok",
                    session_id = context.Session.SessionID

                };
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
                return;

            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
        outoff:
            result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }
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
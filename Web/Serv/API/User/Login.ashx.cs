using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 登录
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
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
            string userId = context.Request["username"];
            string passWord = context.Request["password"];
            string applyStatus = context.Request["applystatus"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.errcode = 1;
                resp.errmsg = "用户名不能为空";
                goto outoff;
            }
            if (string.IsNullOrEmpty(passWord))
            {
                resp.errcode = 1;
                resp.errmsg = "密码不能为空";
                goto outoff;
            }

            UserInfo userInfo;
            string msg = "";
            if (bllUser.Login(userId, passWord, out userInfo, out msg, bllUser.WebsiteOwner, applyStatus))
            {
                if (userInfo.WebsiteOwner != bllUser.WebsiteOwner)
                {
                    resp.errcode = 1;
                    resp.errmsg = "登录失败";
                    goto outoff;
                }

                if (context.Session["currWXOpenId"]!=null) {
                    bllUser.UpdateUserWxOpenId(userInfo.UserID, context.Session["currWXOpenId"].ToString(), bllUser.WebsiteOwner);//更新用户openid
                }

                bllUser.UpdateLastLoginInfo(userInfo.UserID, context.Request["city"]
                    , context.Request["longitude"], context.Request["latitude"], bllUser.WebsiteOwner);

                context.Session[SessionKey.UserID] = userInfo.UserID;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Response.Cookies.Add(bllUser.CreateLoginCookie(userInfo.UserID, userInfo.WXOpenId, userInfo.WXNickname));

                var data = new
                {
                    errcode = 0,
                    errmsg = "ok",
                    session_id= context.Session.SessionID
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
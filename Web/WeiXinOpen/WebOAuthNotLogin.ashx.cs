using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.WeiXinOpen
{
    /// <summary>
    /// WebOAuthNotLogin 的摘要说明
    /// </summary>
    public class WebOAuthNotLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string redirectUrl = HttpUtility.UrlDecode(context.Request["redirecturl"]);//要跳转的地址
            string openId = context.Request["openid"];//微信OpenId
            if (string.IsNullOrEmpty(redirectUrl))
            {
                context.Response.Write("redirecturl 必传");
                return;
            }
            if (string.IsNullOrEmpty(openId))
            {
                context.Response.Write("openid 必传");
                return;
            }
            context.Session["currWXOpenId"] = openId;
            context.Response.Redirect(redirectUrl);


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
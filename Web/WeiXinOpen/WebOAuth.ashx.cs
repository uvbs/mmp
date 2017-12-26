using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WeiXinOpen
{
    /// <summary>
    /// 微信开放平台网页授权统一处理
    /// 所有通过开放平台网页授权登录的都在这里中转再设置登录
    /// </summary>
    public class WebOAuth : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string redirectUrl = HttpUtility.UrlDecode(context.Request["redirecturl"]);//登录后要跳转的地址
            string openId = context.Request["openid"];//微信OpenId
            string sign = context.Request["sign"];//签名
            string websiteOwner=context.Request["websiteowner"];
            string autoLoginKey=context.Request["autologinkey"];
            //string token = context.Request["token"];
            //if (string.IsNullOrEmpty(token))
            //{
            //    context.Response.Write("token 必传");
            //    return;
            //}
            //WeixinOpenOAuthTemp tokenRecord = bllUser.Get<WeixinOpenOAuthTemp>(string.Format(" Token='{0}'", token));
            //if (tokenRecord == null)
            //{
            //    context.Response.Write("token 错误");
            //    return;
            //}
            //bllUser.Delete(new WeixinOpenOAuthTemp(), string.Format("Token='{0}'", tokenRecord.Token));//删除临时数据
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
            //if (string.IsNullOrEmpty(websiteOwner))
            //{
            //    context.Response.Write("websiteowner 必传");
            //    return;
            //}
            if (string.IsNullOrEmpty(sign))
            {
                context.Response.Write("sign 必传");
                return;
            }
            if (!VerSign(openId, sign))
            {
                context.Response.Write("验签失败");
                return;
            }
            var currentUserInfo = bllUser.GetUserInfoByOpenId(openId,websiteOwner);
            if (currentUserInfo == null)
            {
                context.Response.Write("用户不存在");
                return;
            }
            if (!string.IsNullOrEmpty(autoLoginKey))
            {
                autoLoginKey = HttpUtility.UrlDecode(autoLoginKey);
                HttpCookie cookie = new HttpCookie("comeoncloudAutoLoginToken");
                cookie.Value = autoLoginKey;
                cookie.Expires = DateTime.Now.AddDays(30);
                context.Response.Cookies.Add(cookie);

            }

            context.Session[ZentCloud.Common.SessionKey.UserID] = currentUserInfo.UserID;
            context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1;
            context.Session["currWXOpenId"] = currentUserInfo.WXOpenId;
            context.Response.Redirect(redirectUrl);
            //context.Response.Redirect(tokenRecord.Url);


        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="openId">微信OpenId</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public bool VerSign(string openId, string sign)
        {
            string signKey = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenWebOAuthKey");//Md5 key
            string signVer = ZentCloud.Common.DEncrypt.GetMD5(openId + signKey);//验证签名
            if (sign == signVer)
            {
                return true;
            }
            return false;

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
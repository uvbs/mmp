using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// RegisterBinding 的摘要说明
    /// </summary>
    public class RegisterBinding : IHttpHandler, IRequiresSessionState
    {
        DefaultResponse resp = new DefaultResponse();
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;

            string email = context.Request["email"];
            string pwd = context.Request["pwd"];

            if (string.IsNullOrEmpty(email))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入邮箱";

                bllUser.ContextResponse(context, resp);
                return;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "请输入密码";

                bllUser.ContextResponse(context, resp);
                return;
            }

            if (context.Session["currWXOpenId"] == null)
            {
                resp.errcode = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "微信OpenId为空";

                bllUser.ContextResponse(context, resp);
                return;
            }
            string openId = context.Session["currWXOpenId"].ToString();
            bool IsSyncPass = true;
            string basePath = string.Format("http://{0}", context.Request.Url.Authority);

            #region 同步账号注册检查
            if (bllUser.WebsiteOwner == "guoye" || bllUser.WebsiteOwner == "guoyetest")
            {
                IsSyncPass = false;
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("email", email);
                param.Add("pwd", pwd);
                param.Add("openid", openId);

                using (HttpWebResponse wr = ZentCloud.Common.HttpInterFace.CreatePostHttpResponse(
                    "http://www.chinayie.com/api/binding", param, null, null, Encoding.UTF8, null))
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        JToken jto = JToken.Parse(sr.ReadToEnd());
                        if (jto["result"] != null && jto["result"].ToString().ToLower() == "true")
                        {
                            IsSyncPass = true;
                        }
                        else if (jto["result"] != null && jto["msg"] != null && jto["result"].ToString().ToLower() == "false")
                        {
                            resp.errcode = (int)APIErrCode.RegisterFailure;
                            resp.errmsg = jto["msg"].ToString();
                            bllUser.ContextResponse(context, resp);
                            return;
                        }
                    }
                }
            }
            #endregion

            if (IsSyncPass)
            {
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = (int)APIErrCode.OperateFail;
                resp.errmsg = "绑定失败";
            }
            bllUser.ContextResponse(context, resp);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;
using System.Reflection;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 父处理文件 必须登录 Session只读
    /// </summary>
    public class BaseHandlerNeedLogin : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        /// <summary>
        /// 当前用户信息
        /// </summary>
        protected UserInfo currentUserInfo;
        /// <summary>
        /// 默认响应模型
        /// </summary>
        protected DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        /// <summary>
        /// 基地址
        /// </summary>
        protected string baseUrl = "";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                baseUrl = string.Format("http://{0}", context.Request.Url.Authority);
                if (!bllUser.IsLogin)
                {
                    resp.errcode = (int)APIErrCode.UserIsNotLogin;
                    resp.errmsg = "请先登录";
                    result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    goto outoff;
                }

                else
                {
                    currentUserInfo = bllUser.GetCurrentUserInfo();
                }
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法 

                    if (method == null)
                    {
                        resp.errcode = -3;
                        resp.errmsg = "action not exist";
                        result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        context.Response.Write(result);
                        return;

                    }

                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errcode = -3;
                    resp.errmsg = "action not exist";
                    result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.Message;
                result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result = string.Format("{0}({1})", context.Request["callback"], result);

            }
            else
            {
                //返回json数据
            }
        outoff:

            context.Response.Write(result);

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
            public string msg { get; set; } 
            /// <summary>
            /// 业务层结果
            /// </summary>
            public dynamic result { get; set; }

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
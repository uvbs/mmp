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
    /// 处理文件父 不必登录 Session只读
    /// </summary>
    public class BaseHandler : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        protected DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 基地址
        /// </summary>
        //protected string baseUrl = "";
        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                //baseUrl = string.Format("http://{0}",context.Request.Url.Host);
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法 

                    if (method==null)
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
                if (ex.InnerException != null)
                {
                    resp.errmsg = ex.InnerException.Message;
                }
                else
                {
                    resp.errmsg = ex.Message;
                }
                result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            try
            {
                if (!string.IsNullOrEmpty(context.Request["callback"]))
                {
                    //返回 jsonp数据
                    result = string.Format("{0}({1})", context.Request["callback"], result);
                }
            }
            catch (Exception)
            {
            }
            context.Response.ClearContent();
            context.Response.Write(result);

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
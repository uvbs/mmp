using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ZentCloud.JubitIMP.Web.Handler;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 微信API
    /// </summary>
    public class WXAPI : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 微信Bll
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 响应模型
        /// </summary>
        protected AshxResponse resp = new AshxResponse(); // 统一回复响应数据
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["action"];

                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "action is null";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result=string.Format("{0}({1})", context.Request["callback"], result);

            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 获取 微信JSAPI 配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetJsApiConfig(HttpContext context) {

            try
            {
                var cardId = context.Request["cardId"];

                return bllWeixin.GetJSAPIConfig(context.Request["url"], cardId);

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// Summary description for BaseHandlerRequiresSessionNoAction
    /// </summary>
    public class BaseHandlerRequiresSessionNoAction : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Api响应模型
        /// </summary>
        protected BaseResponse apiResp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            this.GetType().GetMethod("ProcessRequest").Invoke(this, new[] { context });

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
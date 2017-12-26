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
    ///接口基处理文件不需要登录 V2版 
    /// </summary>
    public class BaseHandlerNoAction : IHttpHandler,IRequiresSessionState 
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        protected DefaultResponse resp = new DefaultResponse();
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
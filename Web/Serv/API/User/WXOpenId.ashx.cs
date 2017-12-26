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
    /// WXOpenId 的摘要说明
    /// </summary>
    public class WXOpenId : IHttpHandler, IReadOnlySessionState
    {

        AshxResponse resp = new AshxResponse();
        BLL bll = new BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            
            if (context.Session["currWXOpenId"] != null)
            {
                resp.IsSuccess = true;
                resp.Result = context.Session["currWXOpenId"].ToString();
            }
            else
            {
                resp.Msg = "OpenId未找到！";
            }
            bll.ContextResponse(context, resp);
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
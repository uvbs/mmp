using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen
{
    /// <summary>
    /// BLogin 的摘要说明  
    /// </summary>
    public class BLogin :BaseHanderOpen
    {

        public void ProcessRequest(HttpContext context)
        {
           string appId=context.Request["appid"];
           context.Session[SessionKey.LoginStatu] = 1;
           context.Session[SessionKey.UserID] = appId;
           context.Response.Redirect("/Index");
        }
        
    }
}
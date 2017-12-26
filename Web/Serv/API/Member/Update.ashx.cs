using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginNoAction
    {
        public void ProcessRequest(HttpContext context)
        {
            Admin.Member.Update aup = new Admin.Member.Update();
            aup.ProcessRequest(context);
        }
    }
}
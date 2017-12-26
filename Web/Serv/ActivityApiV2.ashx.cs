using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 活动报名接口
    /// </summary>
    public class ActivityApiV2 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {


                BLLActivity bll = new BLLActivity("");
                bll.APIActionResult(context);

            }
            catch (Exception ex)
            {
                context.Response.Write("Exception");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// GetUploadImgPathList 的摘要说明
    /// </summary>
    public class GetUploadImgPathList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                List<string> urlList = new List<string>();

                string url = context.Request["url"];

                if (!string.IsNullOrWhiteSpace(url))
                {
                    urlList = url.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    context.Session["SendBatchDataImgList"] = urlList;
                }

                context.Response.Write(urlList.Count.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write("-1");
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
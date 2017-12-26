using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// ExportFromRedis 的摘要说明
    /// </summary>
    public class ExportFromRedis : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string cache = context.Request["cache"];
            if (RedisHelper.RedisHelper.KeyExists(cache))
            {
                ExportCache exCache = RedisHelper.RedisHelper.StringGet<ExportCache>(cache);
                context.Response.ContentType = "application/vnd.ms-excel";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Charset = "";
                context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + exCache.FileName);
                context.Response.BinaryWrite(exCache.Stream.GetBuffer());
                context.Response.End();
            }
            else
            {
                context.Response.Write("文件已过期");
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
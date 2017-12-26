﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// Create 的摘要说明
    /// </summary>
    public class Create : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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
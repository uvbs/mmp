﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.LiveChat.RoomUser
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
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
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.distribution
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string indexStr = File.ReadAllText(this.Server.MapPath("app.html"));
            //this.Response.Redirect("index.html");
            this.Response.Write(indexStr);
        }
    }
}
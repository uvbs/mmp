using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.guoye
{
    public partial class app : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string indexStr = File.ReadAllText(this.Server.MapPath("index.html"));
            //this.Response.Redirect("index.html");
            this.Response.Write(indexStr);
        }
    }
}
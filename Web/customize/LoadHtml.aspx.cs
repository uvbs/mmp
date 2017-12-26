using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize
{
    public partial class LoadHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var path = Request["path"];
            if (!string.IsNullOrWhiteSpace(path))
            {
                string indexStr = File.ReadAllText(this.Server.MapPath(path));
                this.Response.Write(indexStr);
            }            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Monitor
{
    public partial class UVEventDetails : System.Web.UI.Page
    {
        protected string date = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
              date=Request["date"];  
        }
    }
}
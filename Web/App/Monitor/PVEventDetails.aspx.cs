using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Monitor
{
    public partial class PVEventDetails : System.Web.UI.Page
    {
        protected string date = string.Empty;
        protected string userId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            date = Request["date"];
            if (!string.IsNullOrEmpty(Request["userId"]))
            {
                userId = Request["userId"];
            }
        }
    }
}
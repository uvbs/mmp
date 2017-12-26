using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Error
{
    public partial class CommonMsg : System.Web.UI.Page
    {
        public string icon = string.Empty;
        public string msg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            icon = Request["icon"];

            if (string.IsNullOrWhiteSpace(icon))
            {
                icon = "icon-kulian";
            }

            msg = Request["msg"];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Monitors
{
    public partial class MonitorPlanManage : System.Web.UI.Page
    {
        public string planId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            planId = Request["id"];
        }
    }
}
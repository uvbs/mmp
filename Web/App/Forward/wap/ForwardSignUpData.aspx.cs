using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class ForwardSignUpData : System.Web.UI.Page
    {
        public string MonitorPlanID;
        public string ActivityId;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                MonitorPlanID = Request["Mid"];
                ActivityId = Request["Aid"];
            }

        }
    }
}
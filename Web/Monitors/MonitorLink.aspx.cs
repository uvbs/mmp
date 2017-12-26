using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Monitors
{
    public partial class MonitorLink : System.Web.UI.Page
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string PlanId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            PlanId = Request["id"];
           
        }
    }
}
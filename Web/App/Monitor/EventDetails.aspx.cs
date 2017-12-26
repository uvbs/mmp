using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Text;


namespace ZentCloud.JubitIMP.Web.App.Monitor
{
    public partial class EventDetails : System.Web.UI.Page
    {
        public JuActivityInfo activity;
        int aid = 0;
        BLLMonitor bll;
        protected string uv = string.Empty;
        protected string spreadUserID = string.Empty;
        protected string share = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            uv = Request["uv"];
            aid = Convert.ToInt32(Request["aid"]);
            share=Request["share"];
            spreadUserID = Request["spreaduserid"];
            bll = new BLLMonitor();
            activity = bll.Get<JuActivityInfo>(" JuActivityID =  " + aid.ToString());
            if (activity == null)
            {
                this.ViewState["mplanId"] = Request["aid"];
                return;
            }
            this.ViewState["mplanId"] = activity.MonitorPlanID;




        }
    }
}
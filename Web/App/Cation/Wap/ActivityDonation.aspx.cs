using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class ActivityDonation : System.Web.UI.Page
    {
        public BLLJIMP.Model.JuActivityInfo JuactivityInfo;
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            JuactivityInfo = bllJuactivity.GetJuActivityByActivityID(Request["activityid"]);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class RemindPay : System.Web.UI.Page
    {
        /// <summary>
        /// 活动配置信息
        /// </summary>
        public BLLJIMP.Model.ActivityConfig ActivityConfig;
        /// <summary>
        /// 活动信息
        /// </summary>
        public BLLJIMP.Model.JuActivityInfo JuactivityInfo;
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            JuactivityInfo = bllJuactivity.GetJuActivityByActivityID(Request["activityid"]);
            ActivityConfig = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllJuactivity.WebsiteOwner));
            if (ActivityConfig == null)
            {
                ActivityConfig = new BLLJIMP.Model.ActivityConfig() { ShowName = "活动" };
            }

            
            
            

        }
    }
}
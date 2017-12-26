using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Activity.SignUp
{
    public partial class List : System.Web.UI.Page
    {
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 
        /// </summary>
        public JuActivityInfo activityInfo = new JuActivityInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            activityInfo = bll.GetActivity(Request["activity_id"]);



        }
    }
}
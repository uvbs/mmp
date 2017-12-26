using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward
{
    public partial class WxFansUserInfo : System.Web.UI.Page
    {
        public string ActivitId = string.Empty;
        public string userId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ActivitId = Request["ActivityId"];
            userId=Request["LinkName"];
        }
    }
}
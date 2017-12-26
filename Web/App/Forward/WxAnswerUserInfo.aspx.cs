using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward
{
    public partial class WxAnswerUserInfo : System.Web.UI.Page
    {
        protected string spreadUserId = string.Empty;
        protected string activityId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            spreadUserId=Request["spreadUserId"];
            activityId = Request["aid"];
        }
    }
}
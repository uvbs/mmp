using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.User
{
    public partial class SendWxMsgList : System.Web.UI.Page
    {
        public string planId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            planId = Request["planId"];
        }
    }
}
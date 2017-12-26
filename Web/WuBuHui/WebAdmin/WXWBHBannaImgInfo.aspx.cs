using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin
{
    public partial class WXWBHBannaImgInfo : System.Web.UI.Page
    {
        public string AutoId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["AutoId"];
            }
        }
    }
}
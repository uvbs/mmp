using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward
{
    public partial class WXForwarUserInfo : System.Web.UI.Page
    {
        public string ActivitId;
        public string Mid;
        public string uid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActivitId = Request["id"];
                Mid = Request["Mid"];
                uid = Request["LName"];
            }
        }
    }
}
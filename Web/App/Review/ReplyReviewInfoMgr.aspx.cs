using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Review
{
    public partial class ReplyReviewInfoMgr : System.Web.UI.Page
    {
        public string ReviewId;
        protected void Page_Load(object sender, EventArgs e)
        {
            ReviewId = Request["Id"];
        }
    }
}
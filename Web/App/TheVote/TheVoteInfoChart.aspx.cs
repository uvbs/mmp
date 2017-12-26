using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.TheVote
{
    public partial class TheVoteInfoChart : System.Web.UI.Page
    {
        public string autoId = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                autoId = Request["id"];
            }
        }
    }
}
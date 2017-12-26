using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class QuestionDialog : System.Web.UI.Page
    {
        public string FeedBackID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            FeedBackID = Request["feedbackid"];
        }
    }
}
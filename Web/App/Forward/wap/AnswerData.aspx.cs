using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class AnswerData : System.Web.UI.Page
    {
        public string articleId = string.Empty;
        public string spreadUserId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            articleId=Request["aid"];
            spreadUserId=Request["sid"];
        }
    }
}
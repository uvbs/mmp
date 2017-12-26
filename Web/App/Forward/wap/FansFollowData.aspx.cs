using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class FansFollowData : System.Web.UI.Page
    {
        public int articleId=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            articleId = Convert.ToInt32(Request["Aid"]);
        }
    }
}
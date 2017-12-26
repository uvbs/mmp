using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.ScoreDefine
{
    public partial class ScoreWithdrawCashList : System.Web.UI.Page
    {
        protected string moduleName = "积分";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request["moduleName"])) moduleName = Request["moduleName"];
        }
    }
}
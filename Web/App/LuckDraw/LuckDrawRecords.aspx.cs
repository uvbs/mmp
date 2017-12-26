using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw
{
    public partial class LuckDrawRecords : System.Web.UI.Page
    {
        protected string lotteryId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["lotteryId"]))
            {
                lotteryId = Request["lotteryId"];
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw
{
    public partial class LuckDrawUserInfo : System.Web.UI.Page
    {

        public int lotteryId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["lotteryId"]))
            {
                lotteryId = Convert.ToInt32(Request["lotteryId"]);
            }
        }
    }
}
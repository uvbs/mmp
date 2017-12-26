using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw
{
    public partial class LuckDrawCompile : System.Web.UI.Page
    {
        public string backUrl = "LuckDrawCompile.aspx";
        public string lotteryType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            string reqBackUrl = Request["BackUrl"];
            if (!string.IsNullOrWhiteSpace(reqBackUrl))
            {
                backUrl = HttpUtility.UrlDecode(reqBackUrl);
            }
            lotteryType=Request["lotteryType"];
        }
    }
}
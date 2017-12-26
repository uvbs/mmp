using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw
{
    public partial class LuckDrawEdit : System.Web.UI.Page
    {
        protected WXLotteryV1 lottery = new WXLotteryV1();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["lotteryId"]))
            {
                int lotteryId = Convert.ToInt32(Request["lotteryId"]);
                lottery = bllUser.Get<WXLotteryV1>(string.Format(" LotteryID={0} ", lotteryId));
            }
        }
    }
}
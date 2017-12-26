using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw.wap
{
    public partial class Detail : System.Web.UI.Page
    {
        protected WXLotteryV1 lottery = new WXLotteryV1();

        BLLJIMP.BllLottery bllLottery = new BLLJIMP.BllLottery();

        protected WebsiteInfo webSite = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["lotteryId"]))
            {
                lottery = bllLottery.Get<WXLotteryV1>(string.Format(" WebsiteOwner='{0}' AND LotteryID={1}",bllLottery.WebsiteOwner,int.Parse(Request["lotteryId"])));
            }

            webSite = bllLottery.GetWebsiteInfoModel();
        }
    }
}
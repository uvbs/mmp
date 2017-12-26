using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class WXLotteryRecordsV1 : System.Web.UI.Page
    {
        BLLJIMP.BllLottery bllLottery = new BLLJIMP.BllLottery();
        public  List<WXAwardsV1> AwardList;
        protected void Page_Load(object sender, EventArgs e)
        {
            AwardList = bllLottery.GetList<WXAwardsV1>(string.Format("LotteryId={0}",Request["id"]));
        }
    }
}
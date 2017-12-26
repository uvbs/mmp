using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.LuckDraw
{
    public partial class LuckDrawPage : System.Web.UI.Page
    {
        protected int lotteryId;
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        protected WXLotteryV1 model = new WXLotteryV1();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["lotteryId"]))
            {
                Response.Write("抽奖活动编号不能为空");
                Response.End();
            }
            model = bll.Get<WXLotteryV1>(string.Format("LotteryID={0}", int.Parse(Request["lotteryId"])));
            if (model == null)
            {
                Response.Write("抽奖活动不存在");
                Response.End();
            }
            lotteryId = model.LotteryID;
        }
    }
}
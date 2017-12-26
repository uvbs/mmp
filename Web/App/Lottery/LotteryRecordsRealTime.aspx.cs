using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class LotteryRecordsRealTime : System.Web.UI.Page
    {
        public int id;
        BLLJIMP.BllLottery bll = new BLLJIMP.BllLottery();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.Write("活动编号不能为空");
                Response.End();

            }
            var data = bll.Get<WXLotteryV1>(string.Format("LotteryID={0}",int.Parse(Request["id"])));
            if (data==null)
            {
                Response.Write("活动不存在");
                Response.End();
            }
            id = data.LotteryID;


        }
    }
}
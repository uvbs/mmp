using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class WXLotteryMgrV1 : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        public string lotteryType = "scratch";
        public string moduleName = "刮奖";
        public int isHideImg = 0;
        public int isHidePV = 0;
        public int isHideUV = 0;
        public int isHideDefaultLottery = 0;
        public int isHidePersionCount = 0;
        public int isHideWinning = 0;
        public int isHideRecordsRealTime = 0;
        public int isHideResetLottery = 0;
        public int isHideUrl = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
            if (!string.IsNullOrEmpty(Request["moduleName"]))
            {
                moduleName = Request["moduleName"];
            }
            if (!string.IsNullOrEmpty(Request["lotteryType"]))
            {
                lotteryType = Request["lotteryType"];
            }
            isHideImg = Convert.ToInt32(Request["isHideImg"]);
            isHidePV = Convert.ToInt32(Request["isHidePV"]);
            isHideUV = Convert.ToInt32(Request["isHideUV"]);
            isHideDefaultLottery = Convert.ToInt32(Request["isHideDefaultLottery"]);
            isHidePersionCount = Convert.ToInt32(Request["isHidePersionCount"]);
            isHideWinning = Convert.ToInt32(Request["isHideWinning"]);
            isHideRecordsRealTime = Convert.ToInt32(Request["isHideRecordsRealTime"]);
            isHideResetLottery = Convert.ToInt32(Request["isHideResetLottery"]);
            isHideUrl = Convert.ToInt32(Request["isHideUrl"]);
        }
    }
}
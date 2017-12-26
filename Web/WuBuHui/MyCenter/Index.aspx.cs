using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.MyCenter
{
    public partial class Index : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string BannerStr = "";
        public string MarketNewsIds = "237";
        public string MarketInterPretedids = "240";
        public bool isWeixinFollower = true;
        public string IsHaveUnReadMessage = "false";
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();
        ZentCloud.BLLJIMP.BLLWeixin bllweixin = new ZentCloud.BLLJIMP.BLLWeixin("");
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                GetBanner();
            }

            isWeixinFollower = IsWeixinFollower();
            if (isWeixinFollower)
            {
                
                ZentCloud.BLLJIMP.Model.UserInfo userInfo = bllweixin.GetCurrentUserInfo();
                if (userInfo.ISWXmpScoreAdded != 1)
                {
                    ZentCloud.BLLJIMP.BLLUserScore bllUserScore = new BLLJIMP.BLLUserScore(userInfo.UserID);
                    userInfo.TotalScore += bllUserScore.UpdateUserScoreWithWXTMNotify(bllUserScore.GetDefinedUserScore(ZentCloud.BLLJIMP.BLLUserScore.UserScoreType.SubscriteWXMP), 
                                    bllweixin.GetAccessToken());
                    userInfo.ISWXmpScoreAdded = 1;
                    bll.Update(userInfo);
                }
            }
            IsHaveUnReadMessage = bllNotice.IsHaveUnReadMessage(bll.GetCurrentUserInfo().UserID).ToString();
        }

        private void GetBanner()
        {
            List<BLLJIMP.Model.WBHBannaImg> wbiInfos = bll.GetList<BLLJIMP.Model.WBHBannaImg>(string.Format(" WebsiteOwner='{0}' Order By Sort ASC",bll.WebsiteOwner));
            if (wbiInfos != null)
            {
                foreach (BLLJIMP.Model.WBHBannaImg item in wbiInfos)
                {
                    BannerStr += "<a href=\"" + item.BannaUrl + "\" class=\"sliderlist\">";
                    BannerStr += "<img src=\"" + item.BannaImg + "\" alt=\"\"></a>";
                }
            }
        }
        protected bool IsWeixinFollower()
        {
            ZentCloud.BLLJIMP.BLLWeixin bllweixin = new ZentCloud.BLLJIMP.BLLWeixin("");
            ZentCloud.BLLJIMP.Model.WebsiteInfo currWebsiteInfoModel = (ZentCloud.BLLJIMP.Model.WebsiteInfo)HttpContext.Current.Session["WebsiteInfoModel"];
            ZentCloud.BLLJIMP.Model.UserInfo currWebsiteOwner = new ZentCloud.BLLJIMP.BLLUser("").GetUserInfo(currWebsiteInfoModel.WebsiteOwner);
            //string accesstoken1 = bllweixin.GetAccessToken("wubuhui");
            string accesstoken = bllweixin.GetAccessToken();
            var currentUserInfo=bllweixin.GetCurrentUserInfo();
            return bllweixin.IsWeixinFollower(accesstoken, currentUserInfo.WXOpenId);
            //return bllweixin.IsWeixinFollower(accesstoken1, "oTtgeuMbREgwlrjEoBLPMKjzr2tg");
        }

    }
}

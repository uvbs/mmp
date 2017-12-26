using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.StockPlayer
{
    public partial class StockPlayerSite : System.Web.UI.MasterPage
    {
        protected string iconcss = "";
        protected UserInfo curUser = null;
        protected string logo = "/customize/StockPlayer/Img/logo.png";
        protected string ico = "/customize/StockPlayer/Img/logo.png";
        protected string ol_a="4";
        protected string ol_b="4";
        protected string ol_s = "A*(L*L+B*L)";
        protected string ol_icos = "";
        protected int port = 80;
        protected string wxOpenId = "";
        protected CompanyWebsite_Config webSite = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            BLLUser bllUser = new BLLUser();
            curUser = bllUser.GetCurrentUserInfo();

            if (curUser == null && this.Session["currWXOpenId"] != null)
            {
                wxOpenId = this.Session["currWXOpenId"].ToString();
                curUser = bllUser.GetUserInfoByOpenId(wxOpenId, bllUser.WebsiteOwner);
                if (curUser!=null)
                {
                    Session[SessionKey.LoginStatu] = 1;
                    Session[SessionKey.UserID] = curUser.UserID;
                }

            }

            webSite = bllUser.GetColByKey<CompanyWebsite_Config>("WebsiteOwner", "stockplayer", "AutoID,WebsiteImage,DistributionQRCodeIcon,WebsiteTitle,WebsiteDescription");
            if (webSite != null){
                if (!string.IsNullOrWhiteSpace(webSite.WebsiteImage)) logo = webSite.WebsiteImage;
                if (!string.IsNullOrWhiteSpace(webSite.DistributionQRCodeIcon)) ico = webSite.DistributionQRCodeIcon;
            }
            List<UserLevelConfig> dataList = bllUser.GetColList<UserLevelConfig>(int.MaxValue, 1, 
                string.Format("WebsiteOwner='{0}' And LevelType='{1}'",bllUser.WebsiteOwner,"OnlineTimes"),
                "AutoId,LevelIcon,FromHistoryScore,ToHistoryScore,LevelString");
            if (dataList.Count > 0)
            {
                ol_a = dataList[0].FromHistoryScore.ToString();
                ol_b = dataList[0].ToHistoryScore.ToString();
                ol_s = dataList[0].LevelString;
                ol_icos = ZentCloud.Common.MyStringHelper.ListToStr(dataList.Select(p => p.LevelIcon).ToList(), "", ",");
            }
            int tport = ZentCloud.Common.ConfigHelper.GetConfigInt("WebSocketPort");
            if (tport != 0) port = tport;
        }
    }
}
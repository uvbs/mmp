using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Distribution
{
    public partial class DistributionTree : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        public string RootNodes = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUserInfo = bllUser.GetCurrentUserInfo();
            if (!bllUser.IsDistributionChannel(currentUserInfo))
            {
                var website = bllUser.GetWebsiteInfoModelFromDataBase();
                int count = bllUser.GetCount<UserInfo>(string.Format("DistributionOwner='{0}'", bllUser.WebsiteOwner));
                RootNodes = "{ \"name\": \"" + website.WebsiteName + "\", \"id\": \"" + bllUser.WebsiteOwner + "\", \"count\": " + count + ", \"times\": " + count + ", \"isParent\": true,\"icon\":\"/Plugins/zTree/css/zTreeStyle/img/diy/user.png\", \"tip\":\"\"}";

            }
            else
            {
                //当前用户是渠道
                int count = bllUser.GetCount<UserInfo>(string.Format("DistributionOwner='{0}'",currentUserInfo.UserID));
                RootNodes = "{ \"name\": \"" + bllUser.GetUserDispalyName(currentUserInfo) + "\", \"id\": \"" + currentUserInfo.UserID + "\", \"count\": " + count + ", \"times\": " + count + ", \"isParent\": true,\"icon\":\"/Plugins/zTree/css/zTreeStyle/img/diy/user.png\", \"tip\":\"\"}";

            }


        }
    }
}
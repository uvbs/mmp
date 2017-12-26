using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine
{
    public partial class DistributionTree : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public string RootNodes = "";//根节点
        protected void Page_Load(object sender, EventArgs e)
        {

            var website = bllUser.GetWebsiteInfoModelFromDataBase();

            int count = bllUser.GetCount<UserInfo>(string.Format("DistributionOffLinePreUserId='{0}'", bllUser.WebsiteOwner));

            RootNodes = "{ name: \"" + website.WebsiteName + "\", id: \"" + bllUser.WebsiteOwner + "\", count: " + count + ", times: " + count + ", isParent: true,icon:\"/Plugins/zTree/css/zTreeStyle/img/diy/user.png\", tip:\"\"}";



        }
    }
}
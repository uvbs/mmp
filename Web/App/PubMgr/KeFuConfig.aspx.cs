using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.PubMgr
{
    public partial class KeFuConfig : System.Web.UI.Page
    {
        public ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        BLLUser userBll=new BLLUser("");
        protected void Page_Load(object sender, EventArgs e)
        {
            currWebSiteUserInfo = this.userBll.GetUserInfo(DataLoadTool.GetWebsiteInfoModel().WebsiteOwner);
        }
    }
}
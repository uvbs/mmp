using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class AllForwardListWap : System.Web.UI.Page
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        protected WebsiteInfo website=new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!bllUser.IsLogin)
            {
                Response.Redirect(Request.Url.ToString(),true);
                return;
            }
            var currUser = bllUser.GetCurrentUserInfo();
            website = bllWebsite.GetWebsiteInfo(bllWebsite.WebsiteOwner);//当前站点
            if (website.IsNeedDistributionRecommendCode == 0)
            {
                if (string.IsNullOrWhiteSpace(currUser.DistributionOwner)&&currUser.UserID!=bllUser.WebsiteOwner)
                {
                    bllUser.Update(new BLLJIMP.Model.UserInfo(), string.Format(" DistributionOwner = '{0}' ", bllUser.WebsiteOwner), string.Format(" AutoID = {0} ", currUser.AutoID));
                }
            }
        }
    }
}
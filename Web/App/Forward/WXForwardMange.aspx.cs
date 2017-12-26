using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.App.Forward
{
    public partial class WXForwardMange : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bllWebSite = new BLLJIMP.BLLWebSite();
        protected WebsiteInfo model = new WebsiteInfo();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllWebSite.GetWebsiteInfo(bllWebSite.WebsiteOwner);

            
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }

        }
    }
}
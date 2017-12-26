using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    /// <summary>
    /// 微信开放平台授权页
    /// </summary>
    public partial class OAuthOpen : System.Web.UI.Page
    {
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo currentWebsiteInfo = new WebsiteInfo();
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            currentWebsiteInfo = bll.GetWebsiteInfoModelFromDataBase();
        }
    }
}
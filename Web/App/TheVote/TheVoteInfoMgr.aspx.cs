using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.TheVote
{
    public partial class TheVoteInfoMgr : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        //public string DoMain = "";
        //BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected void Page_Load(object sender, EventArgs e)
        {
            //DoMain = Request.Url.Host;
            //if (!bllUser.GetUserInfo(bllUser.WebsiteOwner).WeixinIsAdvancedAuthenticate.Equals(1))
            //{
            //    DoMain = "xixinxian.comeoncloud.net";
            //}
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}
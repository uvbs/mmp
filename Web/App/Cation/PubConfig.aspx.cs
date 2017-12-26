using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class PubConfig : System.Web.UI.Page
    {
        /// <summary>
        /// 用户Bll
        /// </summary>
        BLLJIMP.BLLUser userBll=new BLLJIMP.BLLUser("");
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin weixinBll=new BLLJIMP.BLLWeixin("");
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        public UserInfo user;
        /// <summary>
        /// 回复菜单
        /// </summary>
        public WeixinReplyRuleInfo menuModel;
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo currentWebsiteInfo = new WebsiteInfo();
        /// <summary>
        /// 授权域名
        /// </summary>
        public string OauthDomain;
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            this.user = userBll.GetCurrWebSiteUserInfo();
            menuModel = weixinBll.Get<ZentCloud.BLLJIMP.Model.WeixinReplyRuleInfo>(string.Format(" UserID = '{0}' and RuleType = 4 ", user.UserID));
            currentWebsiteInfo = userBll.GetWebsiteInfoModelFromDataBase();
            OauthDomain = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenOAuthDoMain");
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }

        }
    }
}
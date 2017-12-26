using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class Withdraw : DistributionBase
    {
        /// <summary>
        /// 是否显示微信到账
        /// </summary>
        public bool isShowWeixin;
        /// <summary>
        /// 支付配置
        /// </summary>
        BLLJIMP.BllPay bllPay = new BLLJIMP.BllPay();
        /// <summary>
        /// 站点BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 
        /// </summary>
        public CompanyWebsite_Config config;
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        protected void Page_Load(object sender, EventArgs e)
        {
            var payConfig = bllPay.GetPayConfig();
            if (payConfig != null)
            {
                if ((!string.IsNullOrEmpty(payConfig.WXAppId)) && (!string.IsNullOrEmpty(payConfig.WXMCH_ID)) && (!string.IsNullOrEmpty(payConfig.WXPartnerKey)))
                {
                    isShowWeixin = true;
                }
            }

            if (CurrentUserInfo.WebsiteOwner == "songhe")
            {
                isShowWeixin = false;
            }

            config = bllWebsite.GetCompanyWebsiteConfig();
            if (config == null)
            {
                config = new CompanyWebsite_Config();
            }
            if (Request["ischannel"] != null && Request["ischannel"].ToString() == "1")
            {
                UserInfo channelUserInfo = bllUser.Get<UserInfo>(string.Format("MgrUserId='{0}'", CurrentUserInfo.UserID));
                if (channelUserInfo == null && (!bllDis.IsChannel(CurrentUserInfo)))
                {
                    Response.Redirect("/error/commonmsg.aspx?msg=您还不是渠道身份，无法访问，请联系商家升级为渠道。");
                    return;
                }
                CurrentUserInfo = channelUserInfo;
            }




        }
    }
}
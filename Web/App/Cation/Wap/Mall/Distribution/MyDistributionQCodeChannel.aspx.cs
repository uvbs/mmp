using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class MyDistributionQCodeChannel : DistributionBase
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 分销BLL
        /// </summary>
        BLLJIMP.BLLDistribution bllDis = new BLLJIMP.BLLDistribution();
        /// <summary>
        /// 全局设置BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 用户BLL
        /// </summary>
        public BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 二维码链接
        /// </summary>
        public string qrcondeUrl = string.Empty;
        /// <summary>
        /// 分享用户
        /// </summary>
        public UserInfo channelUser = null;
        /// <summary>
        /// 当前站点
        /// </summary>
        public WebsiteInfo website = null;
        /// <summary>
        /// 分享标题
        /// </summary>
        public string shareTitle = string.Empty;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string pageTitle = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            var reqUserAutoId = Request["sid"];//分享用户的AutoId
            website = bllUser.GetWebsiteInfoModelFromDataBase();
            CompanyWebsite_Config config = bllWebsite.GetCompanyWebsiteConfig();
            if (string.IsNullOrWhiteSpace(website.DistributionShareQrcodeBgImg))
            {
                website.DistributionShareQrcodeBgImg = "http://files.comeoncloud.net/img/gxfc.png";
            }
            channelUser = bllUser.GetUserInfoByAutoID(Convert.ToInt32(reqUserAutoId));
            if (!(bllDis.IsChannel(channelUser)))
            {
                Response.End();
            }
            qrcondeUrl = bllDis.GetDistributionWxQrcodeLimitUrl(channelUser.UserID, "channel");
            if (!string.IsNullOrEmpty(config.DistributionQRCodeIcon) && !string.IsNullOrEmpty(qrcondeUrl))
            {
                qrcondeUrl = bllWeixin.GetQRCodeImg(qrcondeUrl, config.DistributionQRCodeIcon);
            }
            pageTitle = bllUser.GetUserDispalyName(channelUser) + "邀请您关注 " + website.WXMallName;


        }
    }
}
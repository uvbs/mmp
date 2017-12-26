using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class QCodePage : System.Web.UI.Page
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
        /// 当前用户
        /// </summary>
        public UserInfo currUser = null;
        /// <summary>
        /// 分享用户
        /// </summary>
        public UserInfo shareUser = null;
        /// <summary>
        /// 当前站点
        /// </summary>
        public WebsiteInfo website = null;
        /// <summary>
        /// 是否分享
        /// </summary>
        public bool isShare = false;
        /// <summary>
        /// 分享标题
        /// </summary>
        public string shareTitle = string.Empty;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string pageTitle = string.Empty;

        public CompanyWebsite_Config config;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                var reqSid = Request["sid"];//分享用户的AutoId
                if (!string.IsNullOrEmpty(reqSid))
                {
                    shareUser = bllUser.GetUserInfoByAutoID(int.Parse(reqSid));
                }

                var reqUserAutoId = Request["autoid"];

                currUser = bllUser.GetUserInfoByAutoID(int.Parse(reqUserAutoId));

                website = bllUser.GetWebsiteInfoModelFromDataBase(currUser.WebsiteOwner);

                config = bllWebsite.GetCompanyWebsiteConfig(currUser.WebsiteOwner);

                if (string.IsNullOrWhiteSpace(website.DistributionShareQrcodeBgImg))
                {
                    website.DistributionShareQrcodeBgImg = "http://files.comeoncloud.net/img/gxfc.png";
                }

                string currUserShowName = bllUser.GetUserDispalyName(currUser);

                shareTitle = currUserShowName + "邀请您加入 " + website.WXMallName;

                if (!string.IsNullOrWhiteSpace(reqUserAutoId))
                {
                    bllWeixin.GetDistributionWxQrcodeLimit(out qrcondeUrl, currUser);
                    if (!string.IsNullOrEmpty(config.DistributionQRCodeIcon) && !string.IsNullOrEmpty(qrcondeUrl))
                    {
                        try
                        {
                            qrcondeUrl = bllWeixin.GetQRCodeImg(qrcondeUrl, config.DistributionQRCodeIcon);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    pageTitle = currUserShowName + "的专属二维码";
                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
                Response.End();

            }
        }
    }
}
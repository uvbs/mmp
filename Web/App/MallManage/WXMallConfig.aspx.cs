using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallConfig : System.Web.UI.Page
    {
        ///// <summary>
        ///// 当前站点所有都信息
        ///// </summary>
        //public ZentCloud.BLLJIMP.Model.UserInfo currWebSiteUserInfo;
        /// <summary>
        /// 当前站点信息
        /// </summary>
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;
        /// <summary>
        /// 积分设置
        /// </summary>
        public ZentCloud.BLLJIMP.Model.ScoreConfig scoreConfig ;
        BLLCompanyWebSite bllCompanyWebSite = new BLLCompanyWebSite();
        BLLWebSite bllWebsite = new BLLWebSite();
        public List<string> toolBars = new List<string>();
        public List<string> slides = new List<string>();
        BLLSlide bllSlide = new BLLSlide();
        BLLUser userBll = new BLLUser("");
        BLLMall bllMall = new BLLMall();
        BllScore bllScore = new BllScore();
        public CompanyWebsite_Config CompanyWebsiteConfig = new CompanyWebsite_Config();
        public string WXMallIndexUrl = "";//商城主页
        /// <summary>
        /// 门店自提时间段json
        /// </summary>
        public string StoreSinceTimeJson = "";
        /// <summary>
        /// 送货上门时间段json
        /// </summary>
        public string HomeDeliveryTimeJson = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //currWebSiteUserInfo = this.userBll.GetUserInfo(userBll.WebsiteOwner);
            currWebSiteInfo = bllMall.GetWebsiteInfoModelFromDataBase();
            if (string.IsNullOrEmpty(currWebSiteInfo.ProductImgRatio1))
            {
                currWebSiteInfo.ProductImgRatio1 = (600).ToString();
            }
            if (string.IsNullOrEmpty(currWebSiteInfo.ProductImgRatio2))
            {
                currWebSiteInfo.ProductImgRatio2 = (600).ToString();
            }
            WXMallIndexUrl = string.Format("http://{0}/customize/comeoncloud/Index.aspx?key=MallHome", Request.Url.Host);
            if (currWebSiteInfo != null)
            {
                if (currWebSiteInfo.MallTemplateId.Equals(1))//外卖
                {
                    WXMallIndexUrl = string.Format("http://{0}/App/Cation/wap/mall/IndexV2.aspx", Request.Url.Host);
                }
            }

            toolBars = bllCompanyWebSite.GetToolBarList(int.MaxValue, 1, bllMall.WebsiteOwner, null, null, false)
                .OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();

            slides = bllSlide.GetCurrWebsiteAllTypeList();
            scoreConfig = bllScore.GetScoreConfig();
            if (scoreConfig==null)
            {
                scoreConfig = new BLLJIMP.Model.ScoreConfig();
            }
            CompanyWebsiteConfig = bllWebsite.GetCompanyWebsiteConfig();

            StoreSinceTimeJson = CompanyWebsiteConfig.StoreSinceTimeJson;
            if (string.IsNullOrEmpty(StoreSinceTimeJson))
            {
                StoreSinceTimeJson = "[]";
            }

            HomeDeliveryTimeJson = CompanyWebsiteConfig.HomeDeliveryTimeJson;
            if (string.IsNullOrEmpty(HomeDeliveryTimeJson))
            {
                HomeDeliveryTimeJson = "[]";
            }

        }
    }
}
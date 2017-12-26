using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine
{
    public partial class DistributionConfig : System.Web.UI.Page
    {
        /// <summary>
        /// 分销活动ID
        /// </summary>
        public string ActivityID;
        /// <summary>
        /// 分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bllDis = new BLLJIMP.BLLDistributionOffLine();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLSlide bllSlide = new BLLJIMP.BLLSlide();
        /// <summary>
        ///系统推荐码
        /// </summary>
        public int SysRecommendCode;
        /// <summary>
        /// 
        /// </summary>
        public List<string> slides = new List<string>();
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ActivityID = bllDis.GetDistributionOffLineApplyActivityID();
            SysRecommendCode = bllUser.GetCurrWebSiteUserInfo().AutoID;

            slides = bllSlide.GetCurrWebsiteAllTypeList();
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
        }
    }
}
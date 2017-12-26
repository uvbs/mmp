using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution
{
    public partial class MyDistributionQCode : DistributionBase
    {
        /// <summary>
        /// 二维码
        /// </summary>
       // public System.Text.StringBuilder QCode = new System.Text.StringBuilder();
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

        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();

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
        //public string qrCode

        public int sid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {


            var reqUserAutoId = Request["sid"];//分享用户的AutoId

            website = bllUser.GetWebsiteInfoModelFromDataBase();
            config = bllWebsite.GetCompanyWebsiteConfig();

            if (string.IsNullOrWhiteSpace(reqUserAutoId))
            {
                if (bllUser.WebsiteOwner == "youxiu")
                {
                    var myOrderCount = bllDis.GetMyOrderCount();

                    if (myOrderCount == 0)
                    {
                        //跳转到提示购买页
                        Response.Redirect("/Error/MallDistbIsNotMemberError.aspx");
                        Response.End();
                        return;
                    }
                }

                if (!bllUser.IsDistributionMember(CurrentUserInfo))
                {
                    Response.Redirect("/Error/CommonMsg.aspx?msg=您还不是代言人，代言人才能获取我的二维码");
                    Response.End();
                    return;
                }

                currUser = bllDis.GetCurrentUserInfo();

                sid = currUser.AutoID;

                string currUserShowName = bllUser.GetUserDispalyName(currUser);

                shareTitle = currUserShowName + "邀请您加入 " + website.WXMallName;


                qrcondeUrl = bllDis.CreateUserDistributionImage(currUser.WXOpenId, currUser.WebsiteOwner);

                pageTitle = currUserShowName + "的专属二维码";
            }
            else
            {
                isShare = true;
                sid = Convert.ToInt32(reqUserAutoId);
                shareUser = bllUser.GetUserInfoByAutoID(sid);

                qrcondeUrl = bllDis.CreateUserDistributionImage(shareUser.WXOpenId, shareUser.WebsiteOwner);

                pageTitle = bllUser.GetUserDispalyName(shareUser) + "邀请您关注 " + website.WXMallName;
            }

            if (!string.IsNullOrWhiteSpace(qrcondeUrl))
            {
                qrcondeUrl = bllJuActivity.DownLoadImageToOss(qrcondeUrl, bllJuActivity.WebsiteOwner, true);//qrcondeUrl.Replace(Common.ConfigHelper.GetConfigString("WebSitePath"),"");
            }

        }
    }
}
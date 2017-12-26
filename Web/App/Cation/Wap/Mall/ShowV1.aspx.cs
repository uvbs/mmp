using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class ShowV1 : System.Web.UI.Page
    {
        public BLLJIMP.Model.WebsiteInfo websiteInfo;
        public WXMallProductInfo model;
        BLLJIMP.BLLMall bll = new BLLJIMP.BLLMall();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public string shareLink = "";
        /// <summary>
        /// 分享用户信息
        /// </summary>
        public UserInfo ShareUserInfo;
        protected void Page_Load(object sender, EventArgs e)
        {

            websiteInfo = DataLoadTool.GetWebsiteInfoModel();
            if (string.IsNullOrEmpty(websiteInfo.MallType))
            {
                websiteInfo.MallType = "0";
            }
            model = bll.GetProduct(Request["pid"]);
           shareLink = Request.Url.AbsoluteUri;
           if (bll.GetWebsiteInfoModel().IsDistributionMall.Equals(1))
           {
               if (bll.IsLogin)
               {
                   UserInfo CurrentUserInfo = bll.GetCurrentUserInfo();
                   if (bll.IsWeiXinBrowser&&(string.IsNullOrEmpty(CurrentUserInfo.WXNickname)||string.IsNullOrEmpty(CurrentUserInfo.WXHeadimgurl)))
                   {
                       Session.Clear();
                       Response.Redirect(Request.Url.AbsoluteUri);
                   }
                   if (string.IsNullOrEmpty(Request["sid"]))
                   {
                       shareLink = string.Format("http://{0}{1}?action=show&pid={2}&sid={3}", Request.Url.Host, Request.FilePath, Request["pid"], CurrentUserInfo.AutoID);

                   }
                   else
                   {
                       ShareUserInfo = bllUser.GetUserInfoByAutoID(int.Parse(Request["sid"]));

                   }
                   
               }
               else
               {
                   Response.Write("请用微信打开");
                   Response.End();
               }
               
           }




        }
    }
}
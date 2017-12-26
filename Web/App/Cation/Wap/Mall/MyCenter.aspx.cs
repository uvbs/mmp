using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class MyCenter : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo userInfo;
        public string WXMallIndexUrl = "Index.aspx";//商城主页
        public BLLJIMP.Model.WebsiteInfo websiteInfo;
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllMall.IsLogin)
            {
                userInfo = DataLoadTool.GetCurrUserModel();
                
            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }
            currWebSiteInfo=bllMall.GetWebsiteInfoModel();
            if (currWebSiteInfo.MallTemplateId.Equals(1))//外卖
            {
                WXMallIndexUrl = "Indexv2.aspx";
            }
            websiteInfo = DataLoadTool.GetWebsiteInfoModel();
            if (string.IsNullOrEmpty(websiteInfo.MallType))
            {
                websiteInfo.MallType = "0";
            }


        }
    }
}
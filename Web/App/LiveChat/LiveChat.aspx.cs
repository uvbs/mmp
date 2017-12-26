using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.LiveChat
{
    public partial class LiveChat : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfo currentUserInfo = new UserInfo();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLLiveChat bll = new BLLJIMP.BLLLiveChat();
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// WebSocket主机
        /// </summary>
        public string WebSocketHost = "";
        /// <summary>
        /// 配置
        /// </summary>
        public CompanyWebsite_Config config;

        /// <summary>
        /// 当前站点信息
        /// </summary>
        public WebsiteInfo WebsiteInfo;
        /// <summary>
        /// 
        /// </summary>
        public List<LiveChatDetail> RecordList = new List<LiveChatDetail>();
        /// <summary>
        /// 商品信息
        /// </summary>
        public WXMallProductInfo productInfo = new WXMallProductInfo();
        /// <summary>
        /// 是否所有客服都己下线
        /// </summary>
        //public string IsAllKefuOffLine = "false";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllUser.IsLogin)
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();
                //if (currentUserInfo.UserID=="jubit")
                //{
                //    currentUserInfo.AutoID = 7984;
                //}
            }
            else
            {
                Response.Redirect("/error/commonmsg.aspx?msg=请用微信打开");
            }
            WebSocketHost = ZentCloud.Common.ConfigHelper.GetConfigString("WebSocketHost");
            config = bllWebsite.GetCompanyWebsiteConfig();
            if (string.IsNullOrEmpty(config.DistributionQRCodeIcon))
            {
                config.DistributionQRCodeIcon = "/img/icons/kefu.png";
            }
            RecordList = bll.GetLiveChatDetailList(currentUserInfo.AutoID.ToString());
            WebsiteInfo = bll.GetWebsiteInfoModelFromDataBase(bll.WebsiteOwner);
            //IsAllKefuOffLine = bll.IsAllKefuOffLine(bll.WebsiteOwner).ToString();
            if (!string.IsNullOrEmpty(Request["product_id"]))
            {
                productInfo = bllMall.GetProduct(int.Parse(Request["product_id"]));

            }


        }
    }
}
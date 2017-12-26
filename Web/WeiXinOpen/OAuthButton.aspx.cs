using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WeiXinOpen
{
    public partial class OAuthButton : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 站点信息
        /// </summary>
        public WebsiteInfo tagetWebsiteInfo;
        /// <summary>
        /// 授权域名
        /// </summary>
        public string OauthDomain;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["WebsiteOwner"]))
            {
                Response.Write("websiteowner 参数必传");
                Response.End();
            }
            tagetWebsiteInfo = bll.Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", Request["WebsiteOwner"]));
            //
           //把当前要授权的站点所有者存下来
            //SystemSet systemSet = bll.GetSysSet();
            //systemSet.TempOauthWebsiteOwner = Request["WebsiteOwner"];
            Session["TempOauthWebsiteOwner"] = Request["WebsiteOwner"];
            //if (!bll.Update(systemSet))
            //{
            //    Response.Write("websiteowner 保存失败");
            //    Response.End();
            //}
            OauthDomain=ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenOAuthDoMain");


           
        }
    }
}
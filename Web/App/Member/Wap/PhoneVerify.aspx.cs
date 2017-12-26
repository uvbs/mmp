using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Member.Wap
{
    public partial class PhoneVerify : System.Web.UI.Page
    {
        protected UserInfo curUser;
        protected string memberStandardDescription;
        protected string ico_css_file;
        protected string referrer;
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebSite = new BLLWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            referrer = this.Request["referrer"];
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            memberStandardDescription = nWebsiteConfig.MemberStandardDescription;
            if (nWebsiteConfig.MemberStandard > 1)
            {
                this.Response.Redirect("CompleteUserInfo.aspx?referrer=" + HttpUtility.UrlEncode(referrer));
                return;
            }
            curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null) curUser = new UserInfo();
            //if (curUser.IsPhoneVerify == 1)
            //{
            //    this.Response.Redirect("/Error/IsPhoneVerify.htm");
            //    return;
            //}

            //头部图标引用
            ico_css_file = bllWebSite.GetIcoFilePath();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Member.Wap
{
    public partial class LoginSms : System.Web.UI.Page
    {
        BLLWebSite bllWebSite = new BLLWebSite();
        protected string ico_css_file;
        protected string pageName = "登录";
        protected CompanyWebsite_Config nWebsiteConfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();

            //头部图标引用
            ico_css_file = bllWebSite.GetIcoFilePath();
        }
    }
}
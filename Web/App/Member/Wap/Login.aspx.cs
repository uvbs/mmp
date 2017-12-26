﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZCJson.Linq;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Member.Wap
{
    public partial class Login : System.Web.UI.Page
    {
        BLLWebSite bllWebSite = new BLLWebSite();
        protected string ico_css_file;
        protected string pageName="登录";
        protected CompanyWebsite_Config nWebsiteConfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();

            //头部图标引用
            ico_css_file = bllWebSite.GetIcoFilePath();
        }
    }
}
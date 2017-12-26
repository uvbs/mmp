using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web
{
    public partial class adminlogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var bll = new BLLJIMP.BLL();
            BLLJIMP.BLLWebSite bllCompanyConfig=new BLLJIMP.BLLWebSite();
            string curPath = this.Request.FilePath;
            if (curPath == "/"||curPath.Contains("/login"))
            {
                if ((bll.WebsiteOwner == "hailan"||bll.WebsiteOwner=="hailandev")&&bll.IsWeiXinBrowser)
                {
                    this.Response.Redirect("/customize/comeoncloud/Index.aspx?key=MallHome", true);
                }
                var companyConfig = bllCompanyConfig.GetCompanyWebsiteConfig();
                if (companyConfig.IsEnableCustomizeLoginPage==1)
                {
                    Response.Redirect("/AdminLogin.html");
                }

                if (bll.WebsiteOwner.ToLower() == "stockplayer")
                {
                    this.Response.Redirect("/customize/StockPlayer/Src/Index/Index.aspx", true);
                }

                if (bll.WebsiteOwner.ToLower() == "songhe")
                {
                    this.Response.Redirect("/customize/comeoncloud/Index.aspx?key=MallHome", true);
                }

            }

        }
    }
}